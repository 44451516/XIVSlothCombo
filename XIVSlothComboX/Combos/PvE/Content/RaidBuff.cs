using XIVSlothComboX.CustomComboNS.Functions;

namespace XIVSlothComboX.Combos.PvE;

public class RaidBuff
{
    internal const ushort

        // Heals
        // Heals
        强化药 = 49,
        灼热之光 = 2703,
        连环计 = 1221,
        星空 = 3685,
        占卜 = 1878,
        义结金兰 = 1185,
        战斗连祷 = 786,

        //忍者夺取
        攻其不备 = 3254,
        罐毒之术 = 3849,
        受伤加重 = 638,
        技巧舞步结束TechnicalFinish = 1822,
        战斗之声 = 141,
        留空 = 0;


    public static bool 爆发期()
    {
        if (CustomComboFunctions.HasEffect(强化药) && CustomComboFunctions.GetBuffRemainingTime(强化药) < 25f)
        {
            return true;
        }

        if (CustomComboFunctions.HasEffectAny(灼热之光))
        {
            return true;
        }

        if (CustomComboFunctions.HasEffectAny(技巧舞步结束TechnicalFinish))
        {
            return true;
        }

        if (CustomComboFunctions.HasEffectAny(星空))
            return true;

        if (CustomComboFunctions.HasEffectAny(占卜))
            return true;

        if (CustomComboFunctions.HasEffectAny(义结金兰))
            return true;

        if (CustomComboFunctions.HasEffectAny(战斗连祷))
            return true;
        
        if (CustomComboFunctions.HasEffectAny(战斗之声))
            return true;


        if (CustomComboFunctions.FindTargetEffectAny(攻其不备) is not null)
        {
            return true;
        }

        if (CustomComboFunctions.FindTargetEffectAny(连环计) is not null)
        {
            return true;
        }

        if (CustomComboFunctions.FindTargetEffectAny(罐毒之术) is not null)
        {
            return true;
        }

        return false;
    }
}