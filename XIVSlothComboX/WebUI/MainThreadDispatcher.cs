using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dalamud.Plugin.Services;

namespace XIVSlothComboX.WebUI;

/// <summary>
/// Marshals work from the web server threads onto the Dalamud main thread, because
/// reading or writing the plugin configuration and the game data is only safe there.
/// </summary>
internal sealed class MainThreadDispatcher
{
    private readonly ConcurrentQueue<PendingWork> _queue = new();
    private readonly IFramework _framework;
    private bool _subscribed;

    /// <summary> Creates a dispatcher bound to the given framework. </summary>
    public MainThreadDispatcher(IFramework framework) => _framework = framework;

    private sealed class PendingWork
    {
        public Func<object?> Work { get; init; } = null!;
        public TaskCompletionSource<object?> Completion { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    /// <summary> Subscribes to the framework update loop. </summary>
    public void Start()
    {
        if (_subscribed) return;
        _framework.Update += OnUpdate;
        _subscribed = true;
    }

    /// <summary> Unsubscribes from the framework update loop. </summary>
    public void Stop()
    {
        if (!_subscribed) return;
        _framework.Update -= OnUpdate;
        _subscribed = false;

        // Fail everything still waiting so no HTTP request hangs forever.
        while (_queue.TryDequeue(out var pending))
        {
            pending.Completion.TrySetException(new InvalidOperationException("The plugin is shutting down."));
        }
    }

    /// <summary> Queues work and waits for its result. </summary>
    public async Task<T> RunAsync<T>(Func<T> work)
    {
        var pending = new PendingWork { Work = () => work() };
        _queue.Enqueue(pending);
        var result = await pending.Completion.Task.ConfigureAwait(false);
        return (T)result!;
    }

    /// <summary> Queues work and waits for it to finish. </summary>
    public async Task RunAsync(Action work)
    {
        var pending = new PendingWork { Work = () => { work(); return null; } };
        _queue.Enqueue(pending);
        await pending.Completion.Task.ConfigureAwait(false);
    }

    private void OnUpdate(IFramework framework)
    {
        while (_queue.TryDequeue(out var pending))
        {
            try
            {
                pending.Completion.TrySetResult(pending.Work());
            }
            catch (Exception ex)
            {
                pending.Completion.TrySetException(ex);
            }
        }
    }
}
