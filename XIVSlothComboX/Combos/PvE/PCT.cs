using System;
using FFXIVClientStructs.FFXIV.Client.Game.Gauge;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Combos.PvE;

internal class PCT
{
    public const byte JobID = 42;

    public const uint
        短1 = 34650,
        短2 = 34651,
        短3 = 34652,
        长1 = 34653,
        长2 = 34654,
        长3 = 34655,
        AOE短1 = 34656,
        AOE短2 = 34657,
        AOE短3 = 34658,
        AOE长1 = 34659,
        AOE长2 = 34660,
        AOE长3 = 34661,
        白豆 = 34662,
        黑豆 = 34663,
        反转 = 34683,
        生物画1 = 34664,
        生物画2 = 34665,
        生物画3 = 34666,
        生物画4 = 34667,
        生物1具现 = 34670,
        生物2具现 = 34671,
        生物3具现 = 34672,
        生物4具现 = 34673,
        莫古 = 34676,
        蔬菜 = 34677,
        武器画 = 34668,
        武器具现 = 34674,
        锤1 = 34678,
        锤2 = 34679,
        锤3 = 34680,
        大招 = 34681,
        彩虹 = 34688,
        风景画 = 34669,
        黑魔纹 = 34675;

    public static class Buffs
    {
        public const ushort
            长Buff = 3674,
            连击2buff = 3675,
            连击3buff = 3676,
            黑豆buff = 3691,
            锤子预备 = 3680,
            彩虹预备 = 3679,
            免费反转 = 3690,
            大招预备 = 3681,
            加速 = 3688,
            团辅 = 3685;
        
    }

    public static class Debuffs
    {
    }

    public static class Config
    {
        
    }

    internal class PCT_ST_EasyMode : CustomCombo
    {
        protected internal override CustomComboPreset Preset => CustomComboPreset.PCT_ST_EasyMode;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            var gauge = new TmpPCTGauge();
            
            if (actionID is 短1)
            {
                if (gauge.风景画)
                {
                    if (CanUse(黑魔纹))
                    {
                        return 黑魔纹;
                    }
                }
                if (!gauge.风景画)
                {
                    if (!HasEffect(Buffs.团辅))
                    {
                        return 风景画;
                    }
                    
                }
                if (HasEffect(Buffs.大招预备))
                {
                    return 大招;
                }

                if (HasEffect(Buffs.彩虹预备))
                {
                    return 彩虹;
                }

                if (HasEffect(Buffs.加速))
                {
                    if ((gauge.能量>=50||HasEffect(Buffs.免费反转))&&LevelChecked(反转)&&!HasEffect(Buffs.长Buff))
                    {
                        return 反转;
                    }
                    if (HasEffect(Buffs.长Buff))
                    {
                        if (HasEffect(Buffs.连击3buff))
                        {
                            return 长3;
                        }
                        if (HasEffect(Buffs.连击2buff))
                        {
                            return 长2;
                        }
                        return 长1;
                    }

                    return 短1;
                }
                if (gauge.蔬菜准备&& CanUse(蔬菜))
                {
                    return 蔬菜;
                }
                if (gauge.莫古准备&& CanUse(莫古))
                {
                    return 莫古;
                }

                if (HasEffect(Buffs.锤子预备))
                {
                    return 锤1.OriginalHook();
                }

                if (gauge.武器画)
                {
                    if (CanUse(武器具现))
                    {
                        return 武器具现;
                    }
                    
                }

                if (!gauge.武器画)
                {
                    if (CanUse(武器画))
                    {
                        return 武器画;
                    }
                }
                if (gauge.生物画)
                {
                    if (CanUse(生物4具现))
                    {
                        return 生物4具现;
                    }
                    if (CanUse(生物3具现))
                    {
                        return 生物3具现;
                    }
                    if (CanUse(生物2具现))
                    {
                        return 生物2具现;
                    }
                    if (CanUse(生物1具现))
                    {
                        return 生物1具现;
                    }
                }
                if (!gauge.生物画)
                {
                    if (CanUse(生物画4))
                    {
                        return 生物画4;
                    }
                    if (CanUse(生物画3))
                    {
                        return 生物画3;
                    }
                    if (CanUse(生物画2))
                    {
                        return 生物画2;
                    }
                    if (CanUse(生物画1))
                    {
                        return 生物画1;
                    }
                }

                
                if (HasEffect(Buffs.黑豆buff))
                {
                    return 黑豆;
                }
                if ((gauge.能量>=50||HasEffect(Buffs.免费反转))&&LevelChecked(反转)&&!HasEffect(Buffs.长Buff))
                {
                    return 反转;
                }

                if (HasEffect(Buffs.长Buff))
                {
                    if (HasEffect(Buffs.连击3buff))
                    {
                        return 长3;
                    }
                    if (HasEffect(Buffs.连击2buff))
                    {
                        return 长2;
                    }
                    return 长1;
                }
            }
            return actionID;
        }
    }
    
    internal class PCT_AOE_EasyMode : CustomCombo
    {
        protected internal override CustomComboPreset Preset => CustomComboPreset.PCT_AOE_EasyMode;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            var gauge = new TmpPCTGauge();
            
            if (actionID is AOE短1)
            {
                if (gauge.风景画)
                {
                    if (CanUse(黑魔纹))
                    {
                        return 黑魔纹;
                    }
                }
                if (!gauge.风景画)
                {
                    if (!HasEffect(Buffs.加速))
                    {
                        return 风景画;
                    }
                    
                }
                if (HasEffect(Buffs.大招预备))
                {
                    return 大招;
                }

                if (HasEffect(Buffs.彩虹预备))
                {
                    return 彩虹;
                }
                if (HasEffect(Buffs.加速))
                {
                    if ((gauge.能量>=50||HasEffect(Buffs.免费反转))&&LevelChecked(反转)&&!HasEffect(Buffs.长Buff))
                    {
                        return 反转;
                    }
                    if (HasEffect(Buffs.长Buff))
                    {
                        if (HasEffect(Buffs.连击3buff))
                        {
                            return AOE长3;
                        }
                        if (HasEffect(Buffs.连击2buff))
                        {
                            return AOE长2;
                        }
                        return AOE长1;
                    }

                    return AOE短1;
                }
                if (gauge.蔬菜准备&& CanUse(蔬菜))
                {
                    return 蔬菜;
                }
                if (gauge.莫古准备&& CanUse(莫古))
                {
                    return 莫古;
                }

                if (HasEffect(Buffs.锤子预备))
                {
                    return 锤1.OriginalHook();
                }

                if (gauge.武器画)
                {
                    if (CanUse(武器具现))
                    {
                        return 武器具现;
                    }
                    
                }

                if (!gauge.武器画)
                {
                    if (CanUse(武器画))
                    {
                        return 武器画;
                    }
                }
                if (gauge.生物画)
                {
                    if (CanUse(生物4具现))
                    {
                        return 生物4具现;
                    }
                    if (CanUse(生物3具现))
                    {
                        return 生物3具现;
                    }
                    if (CanUse(生物2具现))
                    {
                        return 生物2具现;
                    }
                    if (CanUse(生物1具现))
                    {
                        return 生物1具现;
                    }
                }
                if (!gauge.生物画)
                {
                    if (CanUse(生物画4))
                    {
                        return 生物画4;
                    }
                    if (CanUse(生物画3))
                    {
                        return 生物画3;
                    }
                    if (CanUse(生物画2))
                    {
                        return 生物画2;
                    }
                    if (CanUse(生物画1))
                    {
                        return 生物画1;
                    }
                }

                
                if (HasEffect(Buffs.黑豆buff))
                {
                    return 黑豆;
                }
                if ((gauge.能量>=50||HasEffect(Buffs.免费反转))&&LevelChecked(反转)&&!HasEffect(Buffs.长Buff))
                {
                    return 反转;
                }

                if (HasEffect(Buffs.长Buff))
                {
                    if (HasEffect(Buffs.连击3buff))
                    {
                        return AOE长3;
                    }
                    if (HasEffect(Buffs.连击2buff))
                    {
                        return AOE长2;
                    }
                    return AOE长1;
                }
            }
            return actionID;
        }
    }
}