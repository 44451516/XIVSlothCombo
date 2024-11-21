using System;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using XIVSlothComboX.Window.help;

namespace XIVSlothComboX.Window;

public class ImGUIHelper
{
    public static void DrawSimpleList<T>(string title, IList<T> list, Func<T> addCallback, Func<T, T> drawCallback)
        {
            using (new GroupWrapper())
            {
                ImGui.Text(title);
                using (new IndentGroupWrapper())
                {
                    ImGui.Text($"Count: {list.Count}");
                    ImGui.SameLine();
                    if (ImGui.Button("Add", new Vector2(60, 20)))
                    {
                        list.Add(addCallback.Invoke());
                        return;
                    }

                    var removeIndex = -1;
                    var swapIndexA = -1;
                    var swapIndexB = -1;
                    for (int i = 0; i < list.Count; i++)
                    {
                        using (new GroupWrapper())
                        {
                            ImGui.Text($"Index: {i}");
                            ImGui.SameLine();
                            var value = list[i];
                            list[i] = drawCallback.Invoke(value);

                            if (i > 0)
                            {
                                ImGui.SameLine();
                                if (ImGui.Button("上移"))
                                {
                                    swapIndexA = i;
                                    swapIndexB = i - 1;
                                }
                            }

                            if (i < list.Count - 1)
                            {
                                ImGui.SameLine();
                                if (ImGui.Button("下移"))
                                {
                                    swapIndexA = i;
                                    swapIndexB = i + 1;
                                }
                            }

                            ImGui.SameLine();
                            if (ImGui.Button("X"))
                            {
                                removeIndex = i;
                            }
                        }
                    }

                    if (removeIndex >= 0)
                        list.RemoveAt(removeIndex);

                    if (swapIndexA != -1)
                    {
                        (list[swapIndexA], list[swapIndexB]) = (list[swapIndexB], list[swapIndexA]);
                    }
                }
            }
        }
}