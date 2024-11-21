using System.Runtime.CompilerServices;

namespace XIVSlothComboX.Window.help;

public static class ImGuiIDGenerator
{
    public static int Id;

    public static void Reset()
    {
        Id = 1000000;
    }

    public static string GetID()
    {
        return $"XIVSlothComboX_Number_{Id++}";
    }

    public static string GetID(object obj,[CallerMemberName]string callerMemberName="", [CallerLineNumber]int lineNum = 0)
    {
        return $"XIVSlothComboX_{obj.GetHashCode()}_{callerMemberName}_{lineNum}";
    }
}