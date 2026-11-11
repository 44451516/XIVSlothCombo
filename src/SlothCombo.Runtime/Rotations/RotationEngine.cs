using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using SlothCombo.Abstractions;
using SlothCombo.Runtime.Hooking;
using SlothCombo.Runtime.Services;

namespace SlothCombo.Runtime.Rotations;

public static class RotationEngine
{
    private const int IntervalMs = 50;
    private static readonly JsonSerializerOptions PlanJsonOptions = new() { MaxDepth = RotationValidator.MaxJsonDepth };
    private static readonly object Gate = new();
    private static readonly object ObservationGate = new();
    private static readonly ConcurrentQueue<Observation> Observations = new();
    private static IFramework? _framework;
    private static bool _initialized;
    private static bool _running;
    private static bool _emergency;
    private static int _isSubmitting;
    private static int _submittingThread;
    private static int _observationOverflow;
    private static int _manualGeneration;
    private static int _observedManualGeneration;
    private static int _observationCount;
    private static string? _requestedStop;
    private static uint _localEntityId;
    private static uint _jobId;
    private static uint _territory;
    private static uint _lastAction;
    private static uint _lastActionEntityId;
    private static uint _lastActionJobId;
    private static uint _lastActionTerritory;
    private static uint _lastActionGlobalSequence;
    private static bool _hasObservedPlayer;
    private static long _observationEpoch;
    private static long _lastTick;
    private static long _yieldUntil;
    private static long _countdownEndedAt;
    private static bool _hadCountdown;
    private static bool _hadCombat;
    private static string _reason = "not-initialized";
    private static string _folder = "";
    private static string _hotkeyText = "Ctrl+Alt+F12";
    private static Hotkey _hotkey = new(0x7B, true, true, false);
    private static bool _hotkeyHeld;
    private static bool _attemptedThisTick;
    private static string? _fingerprint;
    private static JsonObject? _document;
    private static JsonObject? _phase;
    private static RotationSnapshot? _snapshot;
    private static Dictionary<string, object> _parameters = new(StringComparer.Ordinal);
    private static Dictionary<string, object> _variables = new(StringComparer.Ordinal);
    private static readonly HashSet<string> CompletedSequences = new(StringComparer.Ordinal);
    private static readonly HashSet<(uint Global, ushort Source, uint Action)> Received = [];
    private static readonly Queue<(uint Global, ushort Source, uint Action)> ReceivedOrder = new();
    private static Pending? _pending;
    private static ActiveSequence? _sequence;
    private static readonly Stack<ActiveSequence> SuspendedSequences = new();
    private static readonly HashSet<string> UnavailableFields = new(StringComparer.Ordinal);
    private static float? _countdown;

    public static bool IsSubmitting => Volatile.Read(ref _isSubmitting) != 0 &&
        Volatile.Read(ref _submittingThread) == Environment.CurrentManagedThreadId;

    public static void Initialize(string folder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);
        lock (Gate)
        {
            if (_initialized)
                return;
            _folder = Path.GetFullPath(folder);
            _framework = Service.Framework ?? throw new InvalidOperationException("Framework is not initialized.");
            _hotkey = new Hotkey(0x7B, true, true, false);
            _hotkeyText = "Ctrl+Alt+F12";
            var settingsPath = Path.Combine(_folder, "rotation-engine.json");
            if (File.Exists(settingsPath))
            {
                try
                {
                    using var reader = new StreamReader(settingsPath, Encoding.UTF8, true);
                    var settings = JsonNode.Parse(reader.ReadToEnd())?.AsObject();
                    ConfigureHotkey(settings?["emergencyHotkey"]?.GetValue<string>() ?? _hotkeyText, false);
                }
                catch (Exception ex)
                {
                    DetourGuard.Report("Rotation engine settings", ex);
                }
            }
            ResetObservedPlayer();
            ResetProgress();
            _emergency = false;
            _hotkeyHeld = false;
            _initialized = true;
            _reason = "stopped";
            _framework.Update += Update;
        }
    }

    public static void Dispose()
    {
        lock (Gate)
        {
            _running = false;
            if (_framework is not null)
                _framework.Update -= Update;
            _framework = null;
            _initialized = false;
            _reason = "disposed";
            Interlocked.Exchange(ref _requestedStop, null);
            _emergency = false;
            _folder = "";
            _jobId = 0;
            ResetObservedPlayer();
            ResetProgress();
            lock (ObservationGate)
            {
                _manualGeneration = 0;
                _observedManualGeneration = 0;
                _observationCount = 0;
                _observationOverflow = 0;
            }
            _territory = 0;
        }
    }

    public static bool OwnsJob(uint job) => RotationStore.IsV8(job);

    public static object GetState()
    {
        lock (Gate)
        {
            ApplyRequestedStop();
            return State();
        }
    }

    public static object Control(JsonObject command)
    {
        lock (Gate)
        {
            try
            {
                ApplyRequestedStop();
                if (!_initialized)
                    return Reply(false, "not-initialized");
                var operation = RotationJson.Text(command, "command", "stop");
                if (operation is "stop" or "pause" or "emergencyStop" or "emergency")
                    StopCore(operation);
                if (command["emergencyHotkey"] is JsonNode hotkey)
                    ConfigureHotkey(hotkey.GetValue<string>(), true);
                switch (operation)
                {
                    case "start":
                    case "resume":
                        if (_emergency)
                            return Reply(false, "emergency-latched:reset-required");
                        if (!RotationNativeAdapter.Available || !RotationNativeAdapter.AgentsAvailable)
                            return Reply(false, "native-engine-capabilities-unavailable");
                        var startJob = ValidateStartJob(command);
                        ResetProgress();
                        _jobId = startJob;
                        _running = true;
                        _reason = "starting";
                        break;
                    case "reset":
                    case "resetEmergency":
                        StopCore("reset");
                        _emergency = false;
                        break;
                    case "emergency":
                    case "emergencyStop":
                        _emergency = true;
                        break;
                    case "pause":
                    case "stop":
                    case "configure":
                        break;
                    default:
                        return Reply(false, $"unsupported-command:{operation}");
                }
                return Reply(true, _reason);
            }
            catch (Exception ex)
            {
                StopCore($"invalid-control:{ex.Message}");
                return Reply(false, _reason);
            }
        }
    }

    internal static uint ValidateStartJob(JsonObject command)
    {
        if (!command.ContainsKey("expectedJobId"))
            throw new ArgumentException("启动必须提供 expectedJobId（界面所选职业 ID），请刷新或更新界面。");
        var expectedJob = RotationValidator.PositiveUInt(command["expectedJobId"], "$.expectedJobId");
        if (!RotationValidator.SupportedJobIds.Contains(expectedJob))
            throw new ArgumentException("expectedJobId 不在首版支持的 22 个战斗职业中。");
        var actualJob = Service.ObjectTable?.LocalPlayer?.ClassJob.RowId ?? 0;
        if (actualJob == 0)
            throw new ArgumentException("无法读取 Runtime 当前玩家职业，不能启动轮转。");
        if (actualJob != expectedJob)
            throw new RotationConflictException($"界面所选职业 {expectedJob} 与游戏当前职业 {actualJob} 不一致，未启动轮转。");
        return actualJob;
    }

    public static void Stop(string reason)
    {
        // Store changes may call this while holding their own lock. Never wait for a tick that is reading the store.
        Interlocked.Exchange(ref _requestedStop, reason);
        Volatile.Write(ref _running, false);
        if (!Monitor.TryEnter(Gate))
            return;
        try
        {
            ApplyRequestedStop();
        }
        finally
        {
            Monitor.Exit(Gate);
        }
    }

    private static void ApplyRequestedStop()
    {
        var reason = Interlocked.Exchange(ref _requestedStop, null);
        if (reason is not null)
            StopCore(reason);
    }

    // Native hooks only enqueue immutable observations; state advances on Framework.Update.
    public static void NotifyActionSent(uint actionId, ushort sequence)
    {
        var epoch = Volatile.Read(ref _observationEpoch);
        if (!Volatile.Read(ref _initialized))
            return;
        var automatic = IsSubmitting;
        Enqueue(new Observation(true, automatic, 0, actionId, sequence, 0, Environment.TickCount64, epoch));
    }

    public static void ObserveAction(uint casterEntityId, uint actionId, ushort sourceSequence, uint globalSequence)
    {
        var epoch = Volatile.Read(ref _observationEpoch);
        if (!Volatile.Read(ref _initialized) || actionId is 7 or 8 ||
            casterEntityId != Volatile.Read(ref _localEntityId))
            return;
        Enqueue(new Observation(false, false, casterEntityId, actionId, sourceSequence, globalSequence, Environment.TickCount64, epoch));
    }

    private static void Enqueue(Observation observation)
    {
        lock (ObservationGate)
        {
            if (!Volatile.Read(ref _initialized) || observation.Epoch != _observationEpoch)
                return;
            if (observation.Sent && !observation.Automatic)
                Interlocked.Increment(ref _manualGeneration);
            if (Interlocked.Increment(ref _observationCount) > 512)
            {
                Interlocked.Decrement(ref _observationCount);
                Volatile.Write(ref _observationOverflow, 1);
                return;
            }
            Observations.Enqueue(observation);
        }
    }

    private static void Update(IFramework framework)
    {
        lock (Gate)
        {
            if (!_initialized)
                return;
            try
            {
                ApplyRequestedStop();
                PollEmergency();
                var now = Environment.TickCount64;
                if (now - _lastTick < IntervalMs)
                    return;
                _lastTick = now;
                if (!_running)
                {
                    RefreshIdleObservation();
                    DrainIdle();
                    return;
                }
                Tick(now);
            }
            catch (RotationUnavailableException ex)
            {
                StopCore($"unavailable:{ex.Message}");
                UnavailableFields.Add(ex.Message);
                DetourGuard.Report("Rotation engine update", ex);
            }
            catch (Exception ex)
            {
                StopCore($"unavailable:{ex.Message}");
                DetourGuard.Report("Rotation engine update", ex);
            }
        }
    }

    private static void Tick(long now)
    {
        _attemptedThisTick = false;
        if (Volatile.Read(ref _observationOverflow) != 0)
        {
            StopCore("action-observation-buffer-overflow");
            return;
        }
        if (!RotationNativeAdapter.Available)
        {
            StopCore("native-action-api-unavailable");
            return;
        }
        var player = Service.ObjectTable.LocalPlayer;
        var unsafeReason = RotationNativeAdapter.UnsafeReason(player);
        if (unsafeReason is not null)
        {
            RefreshObservedPlayer(player);
            StopCore(unsafeReason);
            return;
        }
        var job = player!.ClassJob.RowId;
        if (job != _jobId || (_document is not null &&
            (player.ObjectId != _localEntityId || Service.ClientState.TerritoryType != _territory)))
        {
            RefreshObservedPlayer(player);
            StopCore("player-job-or-territory-changed");
            return;
        }
        _jobId = job;
        RefreshObservedPlayer(player);
        UnavailableFields.Clear();
        if (!OwnsJob(job))
        {
            StopCore("active-v8-rotation-unavailable");
            return;
        }
        var active = RotationStore.GetActive(job);
        if (active is null)
        {
            StopCore("active-rotation-unavailable");
            return;
        }
        var fingerprint = active.ToJsonString(PlanJsonOptions);
        if (_document is not null && fingerprint != _fingerprint)
        {
            StopCore("rotation-or-parameters-changed");
            return;
        }
        _countdown = RotationNativeAdapter.Countdown();
        var combat = Service.Condition[ConditionFlag.InCombat];
        if (_hadCombat && !combat)
        {
            ResetEncounter("combat-ended:waiting-for-next-combat");
            return;
        }
        _hadCombat |= combat;
        if (_countdown is not null)
        {
            _hadCountdown = true;
            _countdownEndedAt = _countdown <= 0 ? (_countdownEndedAt == 0 ? now : _countdownEndedAt) : 0;
        }
        if (!combat && _hadCountdown && _countdown is null)
        {
            ResetEncounter("countdown-cancelled-or-finished:waiting-for-next-pull");
            return;
        }
        if (!combat && _countdownEndedAt != 0 && now - _countdownEndedAt > 1500)
        {
            ResetEncounter("countdown-finished:waiting-for-next-pull");
            return;
        }
        _snapshot = new RotationSnapshot(player, _lastAction, _countdown);
        ConsumeObservations(now);
        if (!_running)
            return;
        if (_document is null)
        {
            _document = (JsonObject)active.DeepClone();
            if (_document["jobId"]?.GetValue<uint>() != job)
                throw new RotationUnavailableException("rotation-job-mismatch");
            _fingerprint = fingerprint;
            _territory = Service.ClientState.TerritoryType;
            RotationValidator.Validate(_document);
            (_parameters, _variables) = RotationEvaluator.Initialize(_document);
            EnterPhase(RotationJson.Text(_document, "entryPhaseId"));
        }
        var manualGeneration = Volatile.Read(ref _manualGeneration);
        if (manualGeneration != _observedManualGeneration)
        {
            _observedManualGeneration = manualGeneration;
            _yieldUntil = now + 2000;
            _pending = null;
            if (_sequence is not null)
                CompletedSequences.Add(_sequence.Key);
            foreach (var suspended in SuspendedSequences)
                CompletedSequences.Add(suspended.Key);
            SuspendedSequences.Clear();
            _sequence = null;
        }
        if (now < _yieldUntil)
        {
            _reason = "manual-yield";
            return;
        }
        if (_pending is { } pending)
        {
            if (now > pending.Deadline)
                StopCore("action-acknowledgement-timeout");
            else
                _reason = "waiting-for-action-effect";
            return;
        }
        if (RotationNativeAdapter.Busy(player))
        {
            _reason = "native-cast-lock-or-manual-queue";
            return;
        }
        if (!combat && _countdown is not > 0)
        {
            _reason = "waiting-for-combat-or-countdown";
            return;
        }
        var evaluator = Evaluator();
        foreach (var transition in RotationJson.Items(_phase, "transitions"))
        {
            if (!CheckCondition(evaluator, transition!["condition"]))
                continue;
            EnterPhase(RotationJson.Text(transition, "toPhaseId"));
            break;
        }
        if (_sequence is not null)
        {
            if (TryRules(player, now, _sequence.RuleIndex))
                return;
            RunSequence(player, now);
            return;
        }
        _reason = combat ? "no-matching-rule" : "waiting-for-combat-or-countdown";
        TryRules(player, now, RotationJson.Items(_phase, "rules").Count);
    }

    private static bool TryRules(IGameObject player, long now, int end)
    {
        var rules = RotationJson.Items(_phase, "rules");
        for (var index = 0; index < end && _running && !_attemptedThisTick; index++)
        {
            var rule = RotationJson.Object(rules[index]);
            if (!RotationJson.Flag(rule, "enabled") || !CheckCondition(Evaluator(), rule["condition"]))
                continue;
            if (rule["action"] is JsonObject action)
            {
                if (TrySubmit(action, player, now))
                    return true;
            }
            else if (rule["sequence"] is JsonObject sequence)
            {
                var key = $"{RotationJson.Text(_phase, "id")}:{RotationJson.Text(rule, "id")}:{RotationJson.Text(sequence, "id")}";
                if (CompletedSequences.Contains(key))
                    continue;
                if (_sequence is not null)
                    SuspendedSequences.Push(_sequence);
                _sequence = new ActiveSequence(sequence, key, index, 0, now);
                RunSequence(player, now);
                return true;
            }
        }
        return _attemptedThisTick;
    }

    private static RotationEvaluator Evaluator()
        => new(_parameters, _variables, path => _snapshot?.Read(path)
            ?? throw new RotationUnavailableException("snapshot-unavailable"));

    private static bool CheckCondition(RotationEvaluator evaluator, JsonNode? condition)
        // Unknown must stop the tick, not permit a lower-priority rule or transition.
        => evaluator.Condition(condition);

    private static void EnterPhase(string id)
    {
        _phase = RotationJson.Items(_document, "phases").OfType<JsonObject>()
            .Single(phase => RotationJson.Text(phase, "id") == id);
        _sequence = null;
        SuspendedSequences.Clear();
        CompletedSequences.Clear();
        Evaluator().Apply(_phase["onEnter"]);
    }

    private static void RunSequence(IGameObject player, long now)
    {
        var sequence = _sequence!;
        var steps = RotationJson.Items(sequence.Definition, "steps");
        var timeout = sequence.Definition["timeoutMs"]?.GetValue<int>() ?? 10000;
        if (now - sequence.StepStarted >= timeout)
        {
            if (RotationJson.Text(sequence.Definition, "timeoutPolicy", "abort") == "abort")
            {
                CompletedSequences.Add(sequence.Key);
                ResumeSuspended(now);
                _reason = "sequence-aborted-on-timeout";
                return;
            }
            AdvanceSequence(now);
            _reason = "sequence-step-skipped-on-timeout";
            return;
        }
        var step = steps[sequence.Step]!;
        if (!CheckCondition(Evaluator(), step["condition"]))
        {
            _reason = "waiting-for-sequence-condition";
            return;
        }
        TrySubmit(RotationJson.Object(step["action"]), player, now, true);
    }

    private static bool TrySubmit(JsonObject action, IGameObject player, long now, bool sequenceStep = false)
    {
        if (!Volatile.Read(ref _running) || _emergency || _attemptedThisTick ||
            Volatile.Read(ref _requestedStop) is not null)
            return false;
        if (!Service.Condition[ConditionFlag.InCombat] &&
            (!RotationJson.Flag(action, "allowPrepull") || _countdown is not > 0))
        {
            _reason = "prepull-not-authorized-or-countdown-inactive";
            return false;
        }
        if (!RotationNativeAdapter.TryPrepare(action, player, out var submission, out var reason))
        {
            _reason = reason;
            return false;
        }
        var manualGeneration = Volatile.Read(ref _manualGeneration);
        if (manualGeneration != _observedManualGeneration || _pending is not null ||
            !Volatile.Read(ref _running) || Volatile.Read(ref _requestedStop) is not null)
            return false;
        var timeout = Math.Max(2500, RotationNativeAdapter.CastMilliseconds(submission!.AdjustedId) + 2500);
        _attemptedThisTick = true;
        _pending = new Pending(submission, action, now, now + timeout,
            sequenceStep ? _sequence?.Key : null, sequenceStep ? _sequence?.Step : null);
        Volatile.Write(ref _submittingThread, Environment.CurrentManagedThreadId);
        Volatile.Write(ref _isSubmitting, 1);
        bool accepted;
        try
        {
            accepted = RotationNativeAdapter.Submit(submission);
        }
        finally
        {
            Volatile.Write(ref _isSubmitting, 0);
            Volatile.Write(ref _submittingThread, 0);
        }
        if (!accepted)
        {
            // An unexpected send despite a rejected return is ambiguous; never retry it.
            if (Observations.Any(observation => observation.Sent && observation.Automatic))
                StopCore("ambiguous-native-action-result");
            else
            {
                _pending = null;
                _reason = "native-submission-rejected";
            }
            return false;
        }
        _reason = "submitted-awaiting-confirmation";
        return true;
    }

    private static void ConsumeObservations(long now)
    {
        while (Observations.TryDequeue(out var observation))
        {
            Interlocked.Decrement(ref _observationCount);
            if (observation.Epoch != _observationEpoch)
                continue;
            if (observation.Sent)
            {
                if (observation.Automatic && _pending is { } sent &&
                    observation.At >= sent.Started &&
                    (observation.Action == sent.Submission.RequestedId || observation.Action == sent.Submission.AdjustedId))
                {
                    if (observation.Source == 0)
                    {
                        StopCore("zero-client-action-sequence");
                        return;
                    }
                    if (sent.Sequence is not null)
                    {
                        StopCore("multiple-sends-for-one-submission");
                        return;
                    }
                    sent.Sequence = observation.Source;
                    sent.WireAction = observation.Action;
                }
                continue;
            }
            if (!RecordObservedEffect(observation))
                continue;
            if (_pending is not { } pending || pending.Sequence is not ushort expected ||
                observation.Source == 0 || observation.Global == 0 ||
                expected != observation.Source || pending.WireAction != observation.Action ||
                observation.At < pending.Started)
                continue;
            _pending = null;
            // A manual send invalidates the pending result even if its effect arrived in the same frame.
            if (Volatile.Read(ref _manualGeneration) != _observedManualGeneration)
                continue;
            Evaluator().Apply(pending.Action["onSuccess"]);
            if (_sequence is not null && pending.SequenceKey == _sequence.Key && pending.SequenceStep == _sequence.Step)
                AdvanceSequence(now);
            else if (_sequence is not null)
                _sequence = _sequence with { StepStarted = now };
        }
    }

    private static bool RecordObservedEffect(Observation observation)
    {
        if (observation.Epoch != _observationEpoch || observation.Sent ||
            observation.Caster == 0 || observation.Caster != _localEntityId ||
            observation.Caster != _lastActionEntityId || observation.Global == 0 || observation.Action is 0 or 7 or 8)
            return false;
        var receipt = (observation.Global, observation.Source, observation.Action);
        if (!Received.Add(receipt))
            return false;
        ReceivedOrder.Enqueue(receipt);
        if (ReceivedOrder.Count > 128)
            Received.Remove(ReceivedOrder.Dequeue());
        // Global sequences wrap; late or duplicate effects must not replace a newer local observation.
        if (_lastActionGlobalSequence == 0 || unchecked((int)(observation.Global - _lastActionGlobalSequence)) > 0)
        {
            _lastActionGlobalSequence = observation.Global;
            _lastAction = observation.Action;
            _snapshot?.ObserveLastAction(observation.Action);
        }
        return true;
    }

    private static void AdvanceSequence(long now)
    {
        var sequence = _sequence!;
        var next = sequence.Step + 1;
        if (next < RotationJson.Items(sequence.Definition, "steps").Count)
            _sequence = sequence with { Step = next, StepStarted = now };
        else if (RotationJson.Flag(sequence.Definition, "loop"))
            _sequence = sequence with { Step = 0, StepStarted = now };
        else
        {
            CompletedSequences.Add(sequence.Key);
            ResumeSuspended(now);
        }
    }

    private static void ResumeSuspended(long now)
        => _sequence = SuspendedSequences.TryPop(out var suspended) ? suspended with { StepStarted = now } : null;

    private static void DrainIdle()
    {
        lock (ObservationGate)
        {
            while (Observations.TryDequeue(out var observation))
            {
                Interlocked.Decrement(ref _observationCount);
                RecordObservedEffect(observation);
            }
            _observedManualGeneration = Volatile.Read(ref _manualGeneration);
            Volatile.Write(ref _observationOverflow, 0);
        }
    }

    private static void RefreshIdleObservation()
    {
        if (!RotationNativeAdapter.Available)
            return;
        var player = Service.ObjectTable.LocalPlayer;
        RefreshObservedPlayer(player);
        _jobId = player?.ClassJob.RowId ?? 0;
    }

    private static void RefreshObservedPlayer(IGameObject? player)
    {
        var entity = player?.ObjectId ?? 0;
        var job = player?.ClassJob.RowId ?? 0;
        var territory = entity == 0 ? 0 : Service.ClientState.TerritoryType;
        if (entity == _lastActionEntityId && job == _lastActionJobId && territory == _lastActionTerritory)
            return;
        lock (ObservationGate)
        {
            Interlocked.Increment(ref _observationEpoch);
            while (Observations.TryDequeue(out _))
                Interlocked.Decrement(ref _observationCount);
            // The legacy observer has no owner identity. Seed only once, never carry it across a later player/job change.
            _lastAction = entity != 0 && !_hasObservedPlayer ? SlothCombo.Data.ActionWatching.LastAction : 0;
            _hasObservedPlayer |= entity != 0;
            _lastActionEntityId = entity;
            _lastActionJobId = job;
            _lastActionTerritory = territory;
            _lastActionGlobalSequence = 0;
            Received.Clear();
            ReceivedOrder.Clear();
            Volatile.Write(ref _localEntityId, entity);
        }
    }

    private static void ResetObservedPlayer()
    {
        lock (ObservationGate)
        {
            Interlocked.Increment(ref _observationEpoch);
            Volatile.Write(ref _localEntityId, 0);
            _lastActionEntityId = 0;
            _lastActionJobId = 0;
            _lastActionTerritory = 0;
            _lastActionGlobalSequence = 0;
            _lastAction = 0;
            _hasObservedPlayer = false;
        }
    }

    private static void StopCore(string reason)
    {
        _running = false;
        _reason = reason;
        ResetProgress();
    }

    private static void ResetEncounter(string reason)
    {
        ResetProgress();
        _running = true;
        _reason = reason;
    }

    private static void ResetProgress()
    {
        _running = false;
        _pending = null;
        _sequence = null;
        SuspendedSequences.Clear();
        _document = null;
        _phase = null;
        _snapshot = null;
        _fingerprint = null;
        UnavailableFields.Clear();
        _parameters.Clear();
        _variables.Clear();
        CompletedSequences.Clear();
        Received.Clear();
        ReceivedOrder.Clear();
        _hadCombat = false;
        _hadCountdown = false;
        _countdown = null;
        _countdownEndedAt = 0;
        _lastTick = 0;
        _yieldUntil = 0;
        _attemptedThisTick = false;
        Volatile.Write(ref _isSubmitting, 0);
        Volatile.Write(ref _submittingThread, 0);
        DrainIdle();
    }

    private static JsonObject Reply(bool ok, string reason)
    {
        var result = State();
        result["ok"] = ok;
        result["reason"] = reason;
        return result;
    }

    private static JsonObject State()
    {
        var variables = new JsonObject();
        foreach (var pair in _variables)
            variables[pair.Key] = RotationJson.Node(pair.Value);
        return new JsonObject
        {
            ["available"] = _initialized && RotationNativeAdapter.Available && RotationNativeAdapter.AgentsAvailable,
            ["initialized"] = _initialized,
            ["running"] = _running,
            ["reason"] = _reason,
            ["jobId"] = _jobId,
            ["planId"] = _document?["id"]?.DeepClone(),
            ["phaseId"] = _phase?["id"]?.DeepClone(),
            ["pendingActionId"] = _pending?.Submission.AdjustedId ?? 0,
            ["pendingSequence"] = _pending?.Sequence,
            ["pendingDeadlineMs"] = _pending is null ? null : Math.Max(0, _pending.Deadline - Environment.TickCount64),
            ["sequenceId"] = _sequence?.Definition["id"]?.DeepClone(),
            ["sequenceStep"] = _sequence?.Step,
            ["lastAction"] = _lastAction,
            ["lastActionSource"] = _lastActionGlobalSequence != 0 ? "self-action-effect"
                : _lastAction != 0 ? "legacy-initial-baseline" : "none",
            ["lastActionGlobalSequence"] = _lastActionGlobalSequence,
            ["countdownRemaining"] = _countdown,
            ["emergencyHotkey"] = _hotkeyText,
            ["emergencyStopped"] = _emergency,
            ["tickIntervalMs"] = IntervalMs,
            ["variables"] = variables,
            ["unavailableFields"] = new JsonArray(UnavailableFields.Select(message => (JsonNode?)JsonValue.Create(message)).ToArray()),
            ["manualYieldMs"] = 2000,
            ["sequenceTimeoutScope"] = "current-step",
            ["pendingTimeoutPolicy"] = "stop-without-retry",
            ["unknownConditionPolicy"] = "stop-engine-without-lower-priority-fallback",
            ["prepullPolicy"] = "every-action-requires-allowPrepull-and-active-countdown",
            ["stopPolicy"] = "prevent-further-submissions-without-cancelling-existing-native-casts",
            ["capabilities"] = new JsonObject
            {
                ["nativeActions"] = RotationNativeAdapter.Available,
                ["movement"] = RotationNativeAdapter.AgentsAvailable,
                ["countdown"] = RotationNativeAdapter.AgentsAvailable,
                ["ground"] = RotationNativeAdapter.GroundAvailable,
                ["groundPlacementPolicy"] = "self-or-current-enemy-anchor-with-current-enemy-required",
                ["partySlots"] = RotationNativeAdapter.PartySlotsAvailable,
                ["partySlotOrder"] = "native-visible-party-pronouns-1-through-8",
                ["gaugeValues"] = "current-job-scalar-properties-in-native-units",
                ["cooldownValues"] = "seconds-remaining-in-native-recast",
                ["enemyCount"] = new JsonObject
                {
                    ["radius"] = 5,
                    ["unit"] = "yalms",
                    ["center"] = "current-enemy-target",
                    ["metric"] = "3d-center-to-center",
                    ["scope"] = "loaded-living-targetable-hostiles",
                },
            },
            ["unsupportedCapabilities"] = new JsonArray("gauge.array-values", "arbitrary-ground-coordinates",
                "friendly-ground-anchors", "unclassified-self-actions-without-current-enemy",
                "automatic-native-queue-or-cast-cancellation"),
        };
    }

    private static void ConfigureHotkey(string text, bool persist)
    {
        var parts = text.Split('+', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var ctrl = false;
        var alt = false;
        var shift = false;
        int key = 0;
        foreach (var part in parts)
        {
            if (part.Equals("Ctrl", StringComparison.OrdinalIgnoreCase) || part.Equals("Control", StringComparison.OrdinalIgnoreCase))
                ctrl = true;
            else if (part.Equals("Alt", StringComparison.OrdinalIgnoreCase))
                alt = true;
            else if (part.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                shift = true;
            else if (key == 0 && part.StartsWith("F", StringComparison.OrdinalIgnoreCase) &&
                int.TryParse(part.AsSpan(1), out var function) && function is >= 1 and <= 24)
                key = 0x70 + function - 1;
            else if (key == 0 && part.Length == 1 && char.IsAsciiLetterOrDigit(part[0]))
                key = char.ToUpperInvariant(part[0]);
            else
                throw new RotationUnavailableException("invalid-emergency-hotkey");
        }
        if (key == 0 || (!ctrl && !alt && !shift))
            throw new RotationUnavailableException("emergency-hotkey-needs-modifier-and-key");
        var keyText = key is >= 0x70 and <= 0x87 ? $"F{key - 0x70 + 1}" : ((char)key).ToString();
        var normalized = string.Join("+", new[] { ctrl ? "Ctrl" : null, alt ? "Alt" : null, shift ? "Shift" : null, keyText }
            .Where(value => value is not null));
        if (persist)
        {
            Directory.CreateDirectory(_folder);
            var path = Path.Combine(_folder, "rotation-engine.json");
            var temporary = path + ".tmp";
            using (var writer = new StreamWriter(temporary, false, new UTF8Encoding(false)))
                writer.Write(new JsonObject { ["emergencyHotkey"] = normalized }.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
            File.Move(temporary, path, true);
        }
        _hotkey = new Hotkey(key, ctrl, alt, shift);
        _hotkeyText = normalized;
    }

    private static void PollEmergency()
    {
        var pressed = Down(_hotkey.Key) && (!_hotkey.Ctrl || Down(0x11)) &&
            (!_hotkey.Alt || Down(0x12)) && (!_hotkey.Shift || Down(0x10));
        if (pressed && !_hotkeyHeld)
        {
            _emergency = true;
            StopCore("emergency-hotkey");
        }
        _hotkeyHeld = pressed;
    }

    private static bool Down(int key) => (GetAsyncKeyState(key) & 0x8000) != 0;

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int virtualKey);

    private sealed record Hotkey(int Key, bool Ctrl, bool Alt, bool Shift);
    private sealed record Observation(bool Sent, bool Automatic, uint Caster, uint Action, ushort Source, uint Global,
        long At, long Epoch);
    private sealed record ActiveSequence(JsonObject Definition, string Key, int RuleIndex, int Step, long StepStarted);
    private sealed class Pending(RotationSubmission submission, JsonObject action, long started, long deadline,
        string? sequenceKey, int? sequenceStep)
    {
        public RotationSubmission Submission { get; } = submission;
        public JsonObject Action { get; } = action;
        public long Started { get; } = started;
        public long Deadline { get; } = deadline;
        public string? SequenceKey { get; } = sequenceKey;
        public int? SequenceStep { get; } = sequenceStep;
        public ushort? Sequence { get; set; }
        public uint WireAction { get; set; }
    }
}
