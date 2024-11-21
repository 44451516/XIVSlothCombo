using System;
using ImGuiNET;

namespace XIVSlothComboX.Window.help
{
    public struct GroupWrapper : IDisposable
    {
        public GroupWrapper()
        {
            ImGui.PushID(ImGuiIDGenerator.GetID());
            ImGui.BeginGroup();
        }

        public void Dispose()
        {
            ImGui.EndGroup();
            ImGui.PopID();
        }
    }

    public struct IndentGroupWrapper : IDisposable
    {
        public IndentGroupWrapper()
        {
            ImGui.PushID(ImGuiIDGenerator.GetID());
            ImGui.BeginGroup();
            ImGui.Indent();
        }

        public void Dispose()
        {
            ImGui.Unindent();
            ImGui.EndGroup();
            ImGui.PopID();
        }
    }
}