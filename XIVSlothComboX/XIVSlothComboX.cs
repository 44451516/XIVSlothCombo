using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dalamud;
using Dalamud.Game;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.Command;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Interface.Windowing;
using Dalamud.Memory;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using ECommons;
using FFXIVClientStructs.FFXIV.Client.Game;
using XIVSlothComboX.Attributes;
using XIVSlothComboX.Combos;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;
using XIVSlothComboX.Window;
using XIVSlothComboX.Window.Tabs;
using Status = Dalamud.Game.ClientState.Statuses.Status;

namespace XIVSlothComboX
{
    /// <summary> Main plugin implementation. </summary>
    public sealed partial class XIVSlothComboX : IDalamudPlugin
    {
        private const string Command = "/scombo";

        
        private readonly ConfigWindow _ConfigWindow;
        internal static XIVSlothComboX P = null!;
        internal WindowSystem _WindowSystem;
        private static uint? jobID;
        

        public static readonly List<uint> DisabledJobsPVE = new List<uint>()
        {
            // ADV.JobID,
            //AST.JobID,
            BLM.JobID,
            // BLU.JobID,
            // BRD.JobID,
            // DNC.JobID,
            // DOL.JobID,
            // DRG.JobID,
            // DRK.JobID,
            // GNB.JobID,
            // MCH.JobID,
            // MNK.JobID,
            NIN.JobID,
            // PLD.JobID,
            // RDM.JobID,
            // RPR.JobID,
            // SAM.JobID,
            // SCH.JobID,
            // SGE.JobID,
            // SMN.JobID,
            // WAR.JobID,
            // WHM.JobID
        };

        public static readonly List<uint> DisabledJobsPVP = new List<uint>()
        {
            // ADV.JobID,
            // AST.JobID,
            // BLM.JobID,
            // BLU.JobID,
            //BRD.JobID,
            // DNC.JobID,
            // DOL.JobID,
            // DRG.JobID,
            // DRK.JobID,
            // GNB.JobID,
            // MCH.JobID,
            // MNK.JobID,
            // NIN.JobID,
            // PLD.JobID,
            // RDM.JobID,
            // RPR.JobID,
            // SAM.JobID,
            // SCH.JobID,
            // SGE.JobID,
            // SMN.JobID,
            // WAR.JobID,
            // WHM.JobID
        };

        public static uint? JobID
        {
            get => jobID;
            set
            {
                if (jobID != value && value != null)
                {
                    Service.PluginLog.Debug($"Switched to job {value}");
                    PvEFeatures.HasToOpenJob = true;
                }
                jobID = value;
            }
        }

        

        private readonly TextPayload starterMotd = new("[Sloth Message of the Day] ");


        private uint autoActionId = 0;
        private int CustomTimelineIndex = 0;

        
        
        
        
        // private bool isAuto = false;

        private CancellationTokenSource autoTokenSource = new(); // 令牌对象
        private CancellationToken autoToken;


        /// <summary> Initializes a new instance of the <see cref="XIVSlothComboX"/> class. </summary>
        /// <param name="pluginInterface"> Dalamud plugin interface. </param>
        public XIVSlothComboX(DalamudPluginInterface pluginInterface)
        {
            P = this;
            pluginInterface.Create<Service>();
            ECommonsMain.Init(pluginInterface, this);
          
            Service.Configuration = pluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
            Service.Address = new PluginAddressResolver();
            Service.Address.Setup(Service.SigScanner);
            PresetStorage.Init();
   

            Service.ComboCache = new CustomComboCache();
            Service.IconReplacer = new IconReplacer();
            ActionWatching.Enable();
            Combos.JobHelpers.AST.Init();

            _ConfigWindow = new();
            _WindowSystem = new();
            _WindowSystem.AddWindow(_ConfigWindow);
            
            Service.Interface.UiBuilder.Draw += _WindowSystem.Draw;
            Service.Interface.UiBuilder.OpenConfigUi += OnOpenConfigUi;

            Service.CommandManager.AddHandler(Command,
                new CommandInfo(OnCommand)
                {
                    HelpMessage = "Open a window to edit custom combo settings.",
                    ShowInHelp = true,
                });

            Service.ClientState.Login += PrintLoginMessage;

            if (Service.ClientState.IsLoggedIn)
            {
                ResetFeatures();
            }

            Service.Framework.Update += OnFrameworkUpdate;
            

            autoToken = autoTokenSource.Token; // 开关绑

            // TestFeatures.function();
            KillRedundantIDs();
            HandleConflictedCombos();
            Service.IconManager = new IconManager();
        }

        
        private static void HandleConflictedCombos()
        {
            var enabledCopy = Service.Configuration.EnabledActions.ToHashSet(); //Prevents issues later removing during enumeration
            foreach (var preset in enabledCopy)
            {
                if (!PresetStorage.IsEnabled(preset)) continue;

                var conflictingCombos = preset.GetAttribute<ConflictingCombosAttribute>();
                if (conflictingCombos != null)
                {
                    foreach (var conflict in conflictingCombos.ConflictingPresets)
                    {
                        if (PresetStorage.IsEnabled(conflict))
                        {
                            Service.Configuration.EnabledActions.Remove(conflict);
                            Service.Configuration.Save();
                        }
                    }
                }
            }
        }

        
        private static void OnFrameworkUpdate(IFramework framework)
        {
            if (Service.ClientState.LocalPlayer is not null)
                JobID = Service.ClientState.LocalPlayer?.ClassJob?.Id;

            BlueMageService.PopulateBLUSpells();
        }
        
        private static void KillRedundantIDs()
        {
            List<int> redundantIDs = Service.Configuration.EnabledActions.Where(x => int.TryParse(x.ToString(), out _)).OrderBy(x => x).Cast<int>()
                .ToList();
            foreach (int id in redundantIDs)
            {
                Service.Configuration.EnabledActions.RemoveWhere(x => (int)x == id);
            }

            Service.Configuration.Save();
        }

        private void ResetFeatures()
        {
            // Enumerable.Range is a start and count, not a start and end.
            // Enumerable.Range(Start, Count)
            // Service.Configuration.ResetFeatures("v3.0.17.0_NINRework", Enumerable.Range(10000, 100).ToArray());
            // Service.Configuration.ResetFeatures("v3.0.17.0_DRGCleanup", Enumerable.Range(6100, 400).ToArray());
            // Service.Configuration.ResetFeatures("v3.0.18.0_GNBCleanup", Enumerable.Range(7000, 700).ToArray());
        }

        private void DrawUI()
        {
            _ConfigWindow.Draw();
        }

     

        private void OnOpenConfigUi()
        {
            _ConfigWindow.IsOpen = !_ConfigWindow.IsOpen;
        }

        private void PrintLoginMessage()
        {
            // Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(task => ResetFeatures());
            if (!Service.Configuration.HideMessageOfTheDay)
                Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(task => PrintMotD());
        }

        private void PrintMotD()
        {
            Service.ChatGui.PrintError(AboutUs.Ban);
            // Service.ChatGui.PrintError($"[XIVSlothComboX] 和 [XIVSlothCombo]冲突，请关闭或者删掉一个");
        }

        /// <inheritdoc/>
        public string Name => "XIVSlothComboX";

        /// <inheritdoc/>
        public void Dispose()
        {
            autoActionId = 0;
            // isAuto = false;
            
            autoTokenSource.Cancel();
            _ConfigWindow?.Dispose();

            Service.CommandManager.RemoveHandler(Command);
            Service.Framework.Update -= OnFrameworkUpdate;
            Service.Interface.UiBuilder.OpenConfigUi -= OnOpenConfigUi;
            Service.Interface.UiBuilder.Draw -= DrawUI;

            Service.IconReplacer?.Dispose();
            Service.ComboCache?.Dispose();
            Service.IconManager?.Dispose();
            ActionWatching.Dispose();


            Combos.JobHelpers.AST.Dispose();


            Service.ClientState.Login -= PrintLoginMessage;
            ECommonsMain.Dispose();
            
            P = null;

        }


        private void OnCommand(string command, string arguments)
        {
            string[]? argumentsParts = arguments.Split();
            var setOutChat = Service.Configuration.SetOutChat;
            IPlayerCharacter? localPlayer = Service.ClientState.LocalPlayer;

            // Service.ChatGui.Print(argumentsParts[0].ToLower());

            switch (argumentsParts[0].ToLower())
            {
                case "unsetall": // unset all features
                {
                    foreach (CustomComboPreset preset in Enum.GetValues<CustomComboPreset>())
                    {
                        Service.Configuration.EnabledActions.Remove(preset);
                    }

                    Service.ChatGui.Print("All UNSET");
                    Service.Configuration.Save();
                    break;
                }

                case "set_custom_int_value": // unset all features
                {
                    string? targetPreset = argumentsParts[1].ToString();
                    int.TryParse(argumentsParts[2].ToString(), out int oder);
                    PluginConfiguration.SetCustomIntValue(targetPreset, oder);
                    Service.Configuration.Save();
                    break;
                }

                case "set_smn": // unset all features
                {
                    int.TryParse(argumentsParts[1].ToString(), out int oder);
                    PluginConfiguration.SetCustomIntValue(SMN.Config.召唤顺序, oder);
                    Service.Configuration.Save();
                    break;
                }


                case "set": // set a feature
                {
                    // if (!Service.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat])
                    {
                        string? targetPreset = argumentsParts[1].ToLowerInvariant();
                        foreach (CustomComboPreset preset in Enum.GetValues<CustomComboPreset>())
                        {
                            if (preset.ToString().ToLowerInvariant() != targetPreset)
                                continue;

                            Service.Configuration.EnabledActions.Add(preset);
                            if (setOutChat)
                            {
                                Service.ChatGui.Print($"{preset} SET");
                            }
                        }

                        Service.Configuration.Save();
                    }

                    // else
                    // {
                    //     Service.ChatGui.PrintError("Features cannot be set in combat.");
                    // }

                    break;
                }

                case "toggle": // toggle a feature
                {
                    // if (!Service.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat])
                    {
                        string? targetPreset = argumentsParts[1].ToLowerInvariant();
                        foreach (CustomComboPreset preset in Enum.GetValues<CustomComboPreset>())
                        {
                            if (preset.ToString().ToLowerInvariant() != targetPreset)
                                continue;

                            if (Service.Configuration.EnabledActions.Contains(preset))
                            {
                                Service.Configuration.EnabledActions.Remove(preset);

                                if (setOutChat)
                                {
                                    Service.ChatGui.Print($"{preset} UNSET");
                                }
                            }

                            else
                            {
                                Service.Configuration.EnabledActions.Add(preset);
                                if (setOutChat)
                                {
                                    Service.ChatGui.Print($"{preset} SET");
                                }
                            }
                        }

                        Service.Configuration.Save();
                    }

                    // else
                    // {
                    //     Service.ChatGui.PrintError("Features cannot be toggled in combat.");
                    // }

                    break;
                }

                case "unset": // unset a feature
                {
                    // if (!Service.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat])
                    {
                        string? targetPreset = argumentsParts[1].ToLowerInvariant();
                        foreach (CustomComboPreset preset in Enum.GetValues<CustomComboPreset>())
                        {
                            if (preset.ToString().ToLowerInvariant() != targetPreset)
                                continue;

                            Service.Configuration.EnabledActions.Remove(preset);
                            if (setOutChat)
                            {
                                Service.ChatGui.Print($"{preset} UNSET");
                            }
                        }

                        Service.Configuration.Save();
                    }

                    // else
                    // {
                    //     Service.ChatGui.PrintError("Features cannot be unset in combat.");
                    // }

                    break;
                }

                case "list": // list features
                {
                    string? filter = argumentsParts.Length > 1 ? argumentsParts[1].ToLowerInvariant() : "all";

                    if (filter == "set") // list set features
                    {
                        foreach (bool preset in Enum.GetValues<CustomComboPreset>()
                                     .Select(preset => PresetStorage.IsEnabled(preset)))
                        {
                            if (setOutChat)
                            {
                                Service.ChatGui.Print(preset.ToString());
                            }
                        }
                    }

                    else if (filter == "unset") // list unset features
                    {
                        foreach (bool preset in Enum.GetValues<CustomComboPreset>().Select(preset => !PresetStorage.IsEnabled(preset)))
                        {
                            if (setOutChat)
                            {
                                Service.ChatGui.Print(preset.ToString());
                            }
                        }
                    }

                    else if (filter == "all") // list all features
                    {
                        foreach (CustomComboPreset preset in Enum.GetValues<CustomComboPreset>())
                        {
                            if (setOutChat)
                            {
                                Service.ChatGui.Print(preset.ToString());
                            }
                        }
                    }

                    else
                    {
                        Service.ChatGui.PrintError("Available list filters: set, unset, all");
                    }

                    break;
                }

                case "enabled": // list all currently enabled features
                {
                    foreach (CustomComboPreset preset in Service.Configuration.EnabledActions.OrderBy(x => x))
                    {
                        if (int.TryParse(preset.ToString(), out int pres)) continue;
                        if (setOutChat)
                        {
                            Service.ChatGui.Print($"{(int)preset} - {preset}");
                        }
                    }

                    break;
                }

                case "debug": // debug logging
                {
                    try
                    {
                        string? specificJob = argumentsParts.Length == 2 ? argumentsParts[1].ToLower() : "";

                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                        using StreamWriter file = new($"{desktopPath}/SlothDebug.txt", append: false); // Output path

                        file.WriteLine("START DEBUG LOG");
                        file.WriteLine("");
                        file.WriteLine($"Plugin Version: {GetType().Assembly.GetName().Version}"); // Plugin version
                        file.WriteLine("");
                        file.WriteLine($"Installation Repo: {RepoCheckFunctions.FetchCurrentRepo()?.InstalledFromUrl}"); // Installation Repo
                        file.WriteLine("");
                        file.WriteLine($"Current Job: "
                                       + // Current Job
                                       $"{localPlayer.ClassJob.GameData.Name} / "
                                       + // - Client Name
                                       $"{localPlayer.ClassJob.GameData.NameEnglish} / "
                                       + // - EN Name
                                       $"{localPlayer.ClassJob.GameData.Abbreviation}"); // - Abbreviation
                        file.WriteLine($"Current Job Index: {localPlayer.ClassJob.GameData.JobIndex}"); // Job Index
                        file.WriteLine("");
                        file.WriteLine($"Current Zone: {Service.ClientState.TerritoryType}"); // Current zone location
                        file.WriteLine($"Current Party Size: {Service.PartyList.Length}"); // Current party size
                        file.WriteLine("");
                        file.WriteLine($"START ENABLED FEATURES");

                        int i = 0;
                        if (string.IsNullOrEmpty(specificJob))
                        {
                            foreach (CustomComboPreset preset in Service.Configuration.EnabledActions.OrderBy(x => x))
                            {
                                if (int.TryParse(preset.ToString(), out _))
                                {
                                    i++;
                                    continue;
                                }

                                file.WriteLine($"{(int)preset} - {preset}");
                            }
                        }

                        else
                        {
                            foreach (CustomComboPreset preset in Service.Configuration.EnabledActions.OrderBy(x => x))
                            {
                                if (int.TryParse(preset.ToString(), out _))
                                {
                                    i++;
                                    continue;
                                }

                                if (preset.ToString()[..3].ToLower() == specificJob
                                    || // Job identifier
                                    preset.ToString()[..3].ToLower() == "all"
                                    || // Adds in Globals
                                    preset.ToString()[..3].ToLower() == "pvp") // Adds in PvP Globals
                                    file.WriteLine($"{(int)preset} - {preset}");
                            }
                        }

                        file.WriteLine($"END ENABLED FEATURES");
                        file.WriteLine("");
                        file.WriteLine($"Redundant IDs found: {i}");

                        if (i > 0)
                        {
                            file.WriteLine($"START REDUNDANT IDs");
                            foreach (CustomComboPreset preset in Service.Configuration.EnabledActions.Where(x => int.TryParse(x.ToString(), out _))
                                         .OrderBy(x => x))
                            {
                                file.WriteLine($"{(int)preset}");
                            }

                            file.WriteLine($"END REDUNDANT IDs");
                            file.WriteLine("");
                        }

                        file.WriteLine($"Status Effect Count: {localPlayer.StatusList.Count(x => x != null)}");

                        if (localPlayer.StatusList.Length > 0)
                        {
                            file.WriteLine($"START STATUS EFFECTS");
                            foreach (Status? status in localPlayer.StatusList)
                            {
                                file.WriteLine($"ID: {status.StatusId}, COUNT: {status.StackCount}, SOURCE: {status.SourceId}");
                            }

                            file.WriteLine($"END STATUS EFFECTS");
                        }

                        file.WriteLine("END DEBUG LOG");
                        Service.ChatGui.Print("Please check your desktop for SlothDebug.txt and upload this file where requested.");

                        break;
                    }

                    catch (Exception ex)
                    {
                        // Dalamud.Logging.PluginLog.Error(ex, "Debug Log");
                        Service.ChatGui.Print("Unable to write Debug log.");
                        break;
                    }
                }

                case "action":
                {
                    autoActionId = 0;
                    try
                    {
                        autoActionId = uint.Parse(argumentsParts[1]);
                    }
                    catch (Exception exception)
                    {
                        autoActionId = 0;
                        // Dalamud.Logging.PluginLog.Error(exception, "Debug Log");
                    }


                    break;
                }

                case "auto1":
                {
                    autoActionId = 0;
                    autoTokenSource.Cancel();

                    if (localPlayer == null)
                        return;

                    switch (localPlayer.ClassJob.Id)
                    {
                        case GNB.JobID:
                        {
                            autoActionId = GNB.利刃斩KeenEdge;
                            break;
                        }

                        case DRK.JobID:
                        {
                            autoActionId = DRK.Souleater;
                            break;
                        }

                        case PLD.JobID:
                        {
                            autoActionId = PLD.先锋剑FastBlade;
                            break;
                        }

                        case WAR.JobID:
                        {
                            autoActionId = WAR.StormsPath;
                            break;
                        }

                        case DNC.JobID:
                        {
                            autoActionId = DNC.瀑泻Cascade;
                            break;
                        }

                        case MCH.JobID:
                        {
                            autoActionId = MCH.分裂弹SplitShot;
                            break;
                        }
                    }

                    if (autoActionId != 0)
                    {
                        autoTokenSource = new();
                        autoToken = autoTokenSource.Token;
                    }

                    Task.Run(async delegate
                        {
                            while (!autoToken.IsCancellationRequested)
                            {
                                Random random = new Random();
                                var interval = random.Next(32, 64);

                                await Service.Framework.RunOnFrameworkThread(() =>
                                {
                                    if (!Service.ClientState.IsLoggedIn)
                                    {
                                        autoActionId = 0;
                                        autoTokenSource.Cancel();
                                    }

                                    unsafe
                                    {
                                        var targetObjectId = localPlayer.TargetObjectId;
                                        if (autoActionId != 0)
                                        {
                                            if (targetObjectId != 0)
                                            {
                                                IGameObject? targetObject = Service.ClientState.LocalPlayer.TargetObject;

                                                if (targetObject != null && targetObject is IBattleChara battleChara)
                                                {
                                                    if (battleChara.ObjectKind == ObjectKind.BattleNpc)
                                                    {
                                                        {
                                                            ActionManager.Instance()->UseAction(ActionType.Action, autoActionId, targetObjectId);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                });
                                await Task.Delay(TimeSpan.FromMilliseconds(interval));
                            }
                        },
                        autoToken);
                    break;
                }

                case "auto0":
                {
                    // isAuto = false;
                    autoActionId = 0;
                    autoTokenSource.Cancel();

                    break;
                }

                case "fakename":
                {
                    if (argumentsParts.Length > 1)
                    {
                        if (localPlayer != null)
                        {
                            unsafe
                            {
                                FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject* Struct =
                                    (FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)localPlayer.Address;
                                SafeMemory.WriteBytes((IntPtr)Struct->Name.GetPinnableReference(), SeStringUtils.NameText(argumentsParts[1]));
                            }
                        }
                    }

                    break;
                }

                case "useitem":
                {
                    // Dalamud.Logging.PluginLog.Error("1");
                    CustomComboFunctions.Useitem(4551);

                    break;
                }

                case "use_custom_timeline":
                {
                    CustomTimelineIndex = 0;
                    try
                    {
                        CustomTimelineIndex = int.Parse(argumentsParts[1]);
                        if (CustomTimelineIndex > 0)
                        {
                            CustomComboFunctions.LoadCustomTime(CustomTimelineIndex);
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    break;
                }
                
                case "stop_custom_timeline":
                {
                    CustomComboFunctions.ResetCustomTime();

                    break;
                }


                default:
                {
                    _ConfigWindow.IsOpen = !_ConfigWindow.IsOpen;
                    PvEFeatures.HasToOpenJob = true;
                    if (argumentsParts[0].Length > 0)
                    {
                        var jobname = ConfigWindow.groupedPresets.Where(x => x.Value.Any(y => y.Info.JobShorthand.Equals(argumentsParts[0].ToLower(), StringComparison.CurrentCultureIgnoreCase))).FirstOrDefault().Key;
                        var header = $"{jobname} - {argumentsParts[0].ToUpper()}";
                        Service.PluginLog.Debug($"{jobname}");
                        PvEFeatures.HeaderToOpen = header;
                    }
                    break;
                }
            }

            Service.Configuration.Save();
        }
        
    }
}