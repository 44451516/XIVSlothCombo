using System;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dalamud;
using Dalamud.Logging;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using XIVSlothComboX.Attributes;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Window.Tabs
{
    internal class TestFeatures : ConfigWindow
    {
        internal static new void Draw()
        {
            // ImGui.BeginChild("main", new Vector2(0, 0), true);
            ImGui.Text("此选项卡的功能在测试中...");


            #region 假名

            bool isFakeName = Service.Configuration.isFakeName;

            if (ImGui.Checkbox("是否使用假名", ref isFakeName))
            {
                Service.Configuration.isFakeName = isFakeName;
                Service.Configuration.Save();
                function();
            }
            ImGui.PushItemWidth(100);
            string FakeName = Service.Configuration.FakeName;
            bool inputChangedeth = false;
            inputChangedeth |= ImGui.InputText("假名", ref FakeName, 20);

            if (inputChangedeth)
            {
                Service.Configuration.FakeName = FakeName;
                Service.Configuration.Save();
                function();
            }

            #endregion

            ImGui.EndChild();
        }

        private static CancellationTokenSource? tokenSource = new();
        private static CancellationToken cancellationToken;

        public static void function()
        {
            if (Service.ClientState.LocalPlayer == null)
            {
                tokenSource?.Cancel();
                return;
            }

            bool isFakeName = Service.Configuration.isFakeName;
            
            // PluginLog.Error("我执行了2"+isFakeName);
            
            if (isFakeName == false)
            {
                tokenSource?.Cancel();
                return;
            }

            string fakeName = Service.Configuration.FakeName.Trim();
            if (fakeName.IsNullOrEmpty())
            {
                tokenSource?.Cancel();
                return;
            }
            
            {
                tokenSource = new CancellationTokenSource();
                cancellationToken = tokenSource.Token;

                //???
                SafeMemory.WriteBytes(Service.SigScanner.Module.BaseAddress + 0x2180739,
                    SeStringUtils.NameText(Service.Configuration.FakeName.Trim()));
                SafeMemory.WriteBytes(Service.SigScanner.Module.BaseAddress + 0x21601B4,
                    SeStringUtils.NameText(Service.Configuration.FakeName.Trim()));
                SafeMemory.WriteBytes(Service.SigScanner.Module.BaseAddress + 0x2163914,
                    SeStringUtils.NameText(Service.Configuration.FakeName.Trim()));
            }



            // PluginLog.Error("我执行了1");
            {
                Task.Run(async delegate
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            // PluginLog.Error("我执行了1");
                            if (Service.ClientState.IsLoggedIn)
                            {
                                if (Service.ClientState.LocalPlayer != null)
                                {
                                    unsafe
                                    {
                                        // PluginLog.Error("我执行了");
                                        FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject* Struct =
                                            (FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)Service.ClientState.LocalPlayer.Address;

                                        
                                        SafeMemory.WriteBytes((IntPtr)Struct->Name.GetPinnableReference(), SeStringUtils.NameText(fakeName));

                                    }

                                }

                            }
                            await Task.Delay(TimeSpan.FromMilliseconds(5000));
                        }


                    },
                    cancellationToken);

            }


        }

        internal static void Dispose()
        {
            tokenSource.Cancel();
        }
    }
}
