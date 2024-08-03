﻿using Dalamud.Utility;
using XIVSlothComboX.Attributes;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Combos.PvP;
using XIVSlothComboX.CustomComboNS.Functions;

namespace XIVSlothComboX.Combos
{
    /// <summary> Combo presets. </summary>
    public enum CustomComboPreset
    {
        #region PvE Combos

        #region Misc

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", ADV.JobID)]
        AdvAny = 0,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", AST.JobID)]
        AstAny = AdvAny + AST.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", BLM.JobID)]
        BlmAny = AdvAny + BLM.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", BRD.JobID)]
        BrdAny = AdvAny + BRD.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", DNC.JobID)]
        DncAny = AdvAny + DNC.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", DOH.JobID)]
        DohAny = AdvAny + DOH.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", DOL.JobID)]
        DolAny = AdvAny + DOL.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", DRG.JobID)]
        DrgAny = AdvAny + DRG.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", DRK.JobID)]
        DrkAny = AdvAny + DRK.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", GNB.JobID)]
        GnbAny = AdvAny + GNB.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", MCH.JobID)]
        MchAny = AdvAny + MCH.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", MNK.JobID)]
        MnkAny = AdvAny + MNK.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", NIN.JobID)]
        NinAny = AdvAny + NIN.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", PLD.JobID)]
        PldAny = AdvAny + PLD.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", RDM.JobID)]
        RdmAny = AdvAny + RDM.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", RPR.JobID)]
        RprAny = AdvAny + RPR.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", SAM.JobID)]
        SamAny = AdvAny + SAM.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", SCH.JobID)]
        SchAny = AdvAny + SCH.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", SGE.JobID)]
        SgeAny = AdvAny + SGE.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", SMN.JobID)]
        SmnAny = AdvAny + SMN.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", WAR.JobID)]
        WarAny = AdvAny + WAR.JobID,

        [CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled.", WHM.JobID)]
        WhmAny = AdvAny + WHM.JobID,

        [CustomComboInfo("Disabled", "This should not be used.", ADV.JobID)]
        Disabled = 99999,

        #endregion

        #region GLOBAL FEATURES

        [ReplaceSkill(All.Sprint)]
        [CustomComboInfo("无人岛冲刺", "将冲刺替换为无人岛冲刺.\n仅在无人岛使用.\n图标不会更改.\n不要和SimpleTweaks的[海岛冲刺替换]一起使用.", ADV.JobID)]
        ALL_IslandSanctuary_Sprint = 100093,

        #region Global Tank Features

        [CustomComboInfo("全局防护职业功能", "防护职业的通用功能和选项\n取消勾选这个选项不会禁用里面的功能。", ADV.JobID)]
        ALL_Tank_Menu = 100099,

        [ReplaceSkill(All.LowBlow, PLD.ShieldBash)]
        [ParentCombo(ALL_Tank_Menu)]
        [CustomComboInfo("防护：打断功能", "当目标可被打断时，使用插言替换掉下踢。\n骑士可以额外配置盾牌猛击的使用。", ADV.JobID)]
        ALL_Tank_Interrupt = 100000,

        [ReplaceSkill(All.Reprisal)]
        [ParentCombo(ALL_Tank_Menu)]
        [CustomComboInfo("防护：血仇防顶", "当目标已被赋予雪仇效果时，将雪仇替换为飞石", ADV.JobID)]
        ALL_Tank_Reprisal = 100001,

        #endregion

        #region Global Healer Features

        [CustomComboInfo("全局治疗职业功能", "治疗职业的通用功能和选项\n取消勾选这个选项不会禁用里面的功能。", ADV.JobID)]
        ALL_Healer_Menu = 100098,

        [ReplaceSkill(AST.Ascend, WHM.Raise, SCH.Resurrection, SGE.Egeiro)]
        [ConflictingCombos(AST_Raise_Alternative, SCH_Raise, SGE_Raise, WHM_Raise)]
        [ParentCombo(ALL_Healer_Menu)]
        [CustomComboInfo("治疗：复活功能", "将复活替换为即刻咏唱", ADV.JobID)]
        ALL_Healer_Raise = 100010,

        #endregion

        #region Global Magical Ranged Features

        [CustomComboInfo("全局远程魔法进攻职业功能", "远程魔法进攻职业的通用功能和选项\n取消勾选这个选项不会禁用里面的功能。", ADV.JobID)]
        ALL_Caster_Menu = 100097,

        [ReplaceSkill(All.Addle)]
        [ParentCombo(ALL_Caster_Menu)]
        [CustomComboInfo("远程魔法进攻: 昏乱防顶", "当目标已被赋予昏乱效果时，将昏乱替换为裂石飞环", ADV.JobID)]
        ALL_Caster_Addle = 100020,

        [ReplaceSkill(RDM.Verraise, SMN.Resurrection, BLU.AngelWhisper)]
        [ConflictingCombos(SMN_Raise, RDM_Raise)]
        [ParentCombo(ALL_Caster_Menu)]
        [CustomComboInfo("远程魔法进攻：复活功能", "将复活替换为即刻咏唱（赤复活替换为连续咏唱）", ADV.JobID)]
        ALL_Caster_Raise = 100021,

        #endregion

        #region Global Melee Features

        [CustomComboInfo("全局近战进攻职业功能", "近战进攻职业功能的通用功能和选项\n取消勾选这个选项不会禁用里面的功能。", ADV.JobID)]
        ALL_Melee_Menu = 100096,

        [ReplaceSkill(All.Feint)]
        [ParentCombo(ALL_Melee_Menu)]
        [CustomComboInfo("近战进攻：牵制防顶", "当目标已被赋予牵制效果时，将牵制替换为火炎", ADV.JobID)]
        ALL_Melee_Feint = 100030,

        [ReplaceSkill(All.TrueNorth)]
        [ParentCombo(ALL_Melee_Menu)]
        [CustomComboInfo("近战：真北保护", "当自身拥有真北时，将真北替换为火炎", ADV.JobID)]
        ALL_Melee_TrueNorth = 100031,

        #endregion

        #region Global Ranged Physical Features

        [CustomComboInfo("全局物理远程职业功能", "物理远程职业功能的通用功能和选项\n取消勾选这个选项不会禁用里面的功能。", ADV.JobID)]
        ALL_Ranged_Menu = 100095,

        [ReplaceSkill(MCH.Tactician, BRD.Troubadour, DNC.防守之桑巴ShieldSamba)]
        [ParentCombo(ALL_Ranged_Menu)]
        [CustomComboInfo("物理远程进攻：减伤防顶", "当目标已被赋予行吟/策动/防守之桑巴其中之一效果时，将自己的对应技能变为坠星冲。", ADV.JobID)]
        ALL_Ranged_Mitigation = 100040,

        [ReplaceSkill(All.FootGraze)]
        [ParentCombo(ALL_Ranged_Menu)]
        [CustomComboInfo("物理远程进攻：打断选项", "当目标可被打断时，使用伤头代替伤足", ADV.JobID)]
        ALL_Ranged_Interrupt = 100041,

        #endregion

        //Non-gameplay Features
        //[CustomComboInfo("输出战斗日志", "将你使用的技能输出到聊天框", ADV.JobID)]
        //AllOutputCombatLog = 100094,

        // Last value = 100094

        #endregion

        // Jobs

        #region ASTROLOGIAN

        [ReplaceSkill(All.Repose)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            AST.JobID, -10, "", ""
        )]
        AST_Advanced_CustomMode = 10180,

        #region DPS

        [ReplaceSkill
        (
            AST.Malefic, AST.Malefic2, AST.Malefic3, AST.Malefic4, AST.FallMalefic, AST.Combust, AST.Combust2, AST.Combust3, AST.Gravity,
            AST.Gravity2
        )]
        [CustomComboInfo("续dot", "用以下选项替换煞星或烧灼", AST.JobID, 0, "", "")]
        AST_ST_DPS = 1004,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo
        (
            "烧灼上线选项", "Adds Combust to the DPS feature if it's not present on current target, or is about to expire.", AST.JobID, 0, "",
            ""
        )]
        AST_ST_DPS_CombustUptime = 1018,

        [ReplaceSkill(AST.Gravity, AST.Gravity2)]
        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("AoE DPS连击", "下面的每个选项（醒梦/自动抽卡/星力……）也将添加到 重力aoe循环", AST.JobID, 1, "", "")]
        AST_AoE_DPS = 1013,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("光速选项", "循环加入光速", AST.JobID, 2, "", "")]
        AST_DPS_LightSpeed = 1020,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("醒梦选项", "当 MP 低于滑动条的值时在循环中加入醒梦", AST.JobID, 3, "", "")]
        AST_DPS_Lucid = 1008,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("占卜选项", "循环加入占卜", AST.JobID, 4, "", "")]
        AST_DPS_Divination = 1016,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("抽卡选项", "抽卡", AST.JobID, 5, "", "")]
        AST_DPS_AutoDraw = 1011,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("自动发卡", "Weaves your card (best used with Quick Target Cards)", AST.JobID, 6)]
        AST_DPS_AutoPlay = 1037,


        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("星力选项", "当集齐三个星标时将星力加入循环", AST.JobID, 8, "", "")]
        AST_DPS_Astrodyne = 1009,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("小奥秘卡 插入选项", "加入小奥秘卡（小阿卡纳）", AST.JobID, 9, "", "")]
        AST_DPS_AutoCrownDraw = 1012,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("王冠之领主选项[失效]", "循环加入王冠之领主", AST.JobID, 10, "", "")]
        AST_DPS_LazyLord = 1014,


        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("Oracle Option", "Adds Oracle after Divination", AST.JobID)]
        AST_DPS_Oracle = 1015,

        #endregion

        #region Healing

        [ReplaceSkill(AST.Benefic2)]
        [CustomComboInfo("简易治疗（单目标）", "", AST.JobID, 2)]
        AST_ST_SimpleHeals = 1023,

        [ParentCombo(AST_ST_SimpleHeals)]
        [CustomComboInfo("Essential Dignity Option", "当目标体力低于你设定的值时，自动添加先天禀赋进入单体治疗连击", AST.JobID)]
        AST_ST_SimpleHeals_EssentialDignity = 1024,

        [ParentCombo(AST_ST_SimpleHeals)]
        [CustomComboInfo("Celestial Intersection Option", "添加天星交错进入单体治疗连击", AST.JobID)]
        AST_ST_SimpleHeals_CelestialIntersection = 1025,

        [ParentCombo(AST_ST_SimpleHeals)]
        [CustomComboInfo("Aspected Benefic Option", "在目标身上没有吉星相位或快要结束时，自动更换福星为吉星相位", AST.JobID)]
        AST_ST_SimpleHeals_AspectedBenefic = 1027,

        [ParentCombo(AST_ST_SimpleHeals)]
        [CustomComboInfo("康复 选项", "当你的目标有一个可清除的debuff时则使用 康复。", AST.JobID)]
        AST_ST_SimpleHeals_Esuna = 1039,

        [ParentCombo(AST_ST_SimpleHeals)]
        [CustomComboInfo("擢升设置", "添加擢升进入治疗连击", AST.JobID)]
        AST_ST_SimpleHeals_Exaltation = 1028,

        [ReplaceSkill(AST.AspectedHelios)]
        [CustomComboInfo("阳星相位", "当你处于阳星相位hot时，用阳星来替换阳星相位", AST.JobID, 3, "", "")]
        AST_AoE_SimpleHeals_AspectedHelios = 1010,

        [ParentCombo(AST_AoE_SimpleHeals_AspectedHelios)]
        [CustomComboInfo("Celestial Opposition Option", "添加天星冲日进入AOE治疗连击", AST.JobID)]
        AST_AoE_SimpleHeals_CelestialOpposition = 1021,

        [ParentCombo(AST_AoE_SimpleHeals_AspectedHelios)]
        [CustomComboInfo("Lazy Lady Option", "Adds Lady of Crowns, if the card is drawn", AST.JobID)]
        AST_AoE_SimpleHeals_LazyLady = 1022,

        [ParentCombo(AST_AoE_SimpleHeals_AspectedHelios)]
        [CustomComboInfo("Horoscope Option", "添加天宫图进入治疗连击", AST.JobID)]
        AST_AoE_SimpleHeals_Horoscope = 1026,

        [ReplaceSkill(AST.Benefic2)]
        [CustomComboInfo("福星降级", "如果福星还没学会或用不了，将福星替换为吉星", AST.JobID, 4, "", "")]
        AST_Benefic = 1002,

        [ParentCombo(AST_ST_SimpleHeals)]
        [CustomComboInfo("The Spire Option", "Adds The Spire when the card has been drawn", AST.JobID)]
        AST_ST_SimpleHeals_Spire = 1030,

        [ParentCombo(AST_ST_SimpleHeals)]
        [CustomComboInfo("The Ewer Option", "Adds The Ewer when the card has been drawn", AST.JobID)]
        AST_ST_SimpleHeals_Ewer = 1032,

        #endregion

        #region Utility

        [ReplaceSkill(All.Swiftcast)]
        [ConflictingCombos(ALL_Healer_Raise)]
        [CustomComboInfo("替代性的复活功能", "自动把即刻咏唱改为生辰", AST.JobID, 5, "", "")]
        AST_Raise_Alternative = 1003,

        [Variant]
        [VariantParent(AST_ST_DPS_CombustUptime, AST_AoE_DPS)]
        [CustomComboInfo("精神镖选项", "当没有dot或dot剩余时间少于3s时，使用多变精神镖", AST.JobID)]
        AST_Variant_SpiritDart = 1035,

        [Variant]
        [VariantParent(AST_ST_DPS)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", AST.JobID)]
        AST_Variant_Rampart = 1036,

        #endregion

        #region Cards

        [CustomComboInfo("快速目标发牌", "抽卡时自动选中一个合适的队友。", AST.JobID)]
        AST_Cards_QuickTargetCards = 1029,

        [ParentCombo(AST_Cards_QuickTargetCards)]
        [CustomComboInfo("将坦克和奶妈加入自动发牌目标选择", "如果没有 DPS 可选时选择坦克和奶妈", AST.JobID)]
        AST_Cards_QuickTargetCards_TargetExtra = 1031,

        #endregion

        // Last value = 1039

        #endregion

        #region BLACK MAGE

        [ReplaceSkill(All.Sleep)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            BLM.JobID, -10, "", ""
        )]
        BLM_Advanced_CustomMode = 20120,


        [ReplaceSkill(BLM.Scathe)]
        [ConflictingCombos(BLM_Scathe_Xeno, BLM_ST_AdvancedMode, BLM_ST_AdvancedMode)]
        [CustomComboInfo("标准循环", "将崩溃整合为一键单体标准循环。", BLM.JobID, -3, "", "")]
        BLM_ST_SimpleMode = 2012,

        #region Advanced ST

        [ReplaceSkill(BLM.Fire)]
        [ConflictingCombos(BLM_Scathe_Xeno, BLM_ST_SimpleMode)]
        [CustomComboInfo
        (
            "Advanced Mode - Single Target",
            "Replaces Fire with a full one-button single target rotation.\nThese features are ideal if you want to customize the rotation.",
            BLM.JobID, -9, "", ""
        )]
        BLM_ST_AdvancedMode = 2021,

        [ParentCombo(BLM_ST_AdvancedMode)]
        [CustomComboInfo("Thunder I/III Option", "Adds Thunder I/Thunder III when the debuff isn't present or is expiring.", BLM.JobID)]
        BLM_ST_Adv_Thunder = 2029,

        [ParentCombo(BLM_ST_Adv_Thunder)]
        [CustomComboInfo
        (
            "Thundercloud Spender Option", "Spends Thundercloud as soon as possible rather than waiting until Thunder is expiring.",
            BLM.JobID
        )]
        BLM_ST_Adv_Thunder_ThunderCloud = 2030,

        [ParentCombo(BLM_ST_AdvancedMode)]
        [CustomComboInfo("Umbral Soul Option", "Uses Transpose/Umbral Soul when no target is selected.", BLM.JobID, 10, "", "")]
        BLM_Adv_UmbralSoul = 2035,

        [ParentCombo(BLM_ST_AdvancedMode)]
        [CustomComboInfo("Movement Options", "Choose options to be used during movement.", BLM.JobID)]
        BLM_Adv_Movement = 2036,

        [ParentCombo(BLM_ST_AdvancedMode)]
        [CustomComboInfo("Triplecast/Swiftcast Option", "Adds Triplecast/Swiftcast to the rotation.", BLM.JobID, -8, "", "")]
        BLM_Adv_Casts = 2039,

        [ParentCombo(BLM_Adv_Casts)]
        [CustomComboInfo("Pool Triplecast Option", "Keep one Triplecast charge for movement.", BLM.JobID)]
        BLM_Adv_Triplecast_Pooling = 2040,

        [ParentCombo(BLM_ST_AdvancedMode)]
        [CustomComboInfo("Cooldown Options", "Select which cooldowns to add to the rotation.", BLM.JobID, -8, "", "")]
        BLM_Adv_Cooldowns = 2042,

        [ParentCombo(BLM_ST_AdvancedMode)]
        [CustomComboInfo
        (
            "Opener Option", "Adds the Lv.90 opener." + "\nWill default to the Standard opener when nothing is selected.", BLM.JobID,
            -10, "", ""
        )]
        BLM_Adv_Opener = 2043,

        [ParentCombo(BLM_ST_AdvancedMode)]
        [CustomComboInfo
        (
            "Rotation Option", "Choose which rotation to use." + "\nWill default to the Standard rotation when nothing is selected.",
            BLM.JobID, -9, "", ""
        )]
        BLM_Adv_Rotation = 2045,

        #endregion


        [ParentCombo(BLM_ST_SimpleMode)]
        [CustomComboInfo("雷云功能", "当自身存在雷云状态且目标身上没有dot存在或dot即将结束时，自动插入闪雷/暴雷.", BLM.JobID, 0, "", "")]
        BLM_Thunder = 2006,

        [ReplaceSkill(BLM.Flare)]
        [CustomComboInfo("简易 AoE 功能", "将核爆整合为一键AoE循环。", BLM.JobID, -1, "", "")]
        BLM_AoE_SimpleMode = 2008,

        #region Advanced AoE

        [ReplaceSkill(BLM.Blizzard2, BLM.HighBlizzard2)]
        [ConflictingCombos(BLM_AoE_SimpleMode)]
        [CustomComboInfo
        (
            "Advanced Mode - AoE",
            "Replaces Blizzard II with a full one-button AoE rotation.\nThese features are ideal if you want to customize the rotation.", BLM.JobID,
            -8, "", ""
        )]
        BLM_AoE_AdvancedMode = 2054,

        [ParentCombo(BLM_AoE_AdvancedMode)]
        [CustomComboInfo("Thunder Uptime Option", "Adds Thunder II/Thunder IV during Umbral Ice.", BLM.JobID, 1, "", "")]
        BLM_AoE_Adv_ThunderUptime = 2055,

        [ParentCombo(BLM_AoE_Adv_ThunderUptime)]
        [CustomComboInfo("Uptime in Astral Fire", "Maintains uptime during Astral Fire.", BLM.JobID, 1, "", "")]
        BLM_AoE_Adv_ThunderUptime_AstralFire = 2056,

        [ParentCombo(BLM_AoE_AdvancedMode)]
        [CustomComboInfo("Foul Option", "Adds Foul when available during Astral Fire.", BLM.JobID, 2, "", "")]
        BLM_AoE_Adv_Foul = 2044,

        [ParentCombo(BLM_AoE_AdvancedMode)]
        [CustomComboInfo("Umbral Soul Option", "Use Transpose/Umbral Soul when no target is selected.", BLM.JobID, 99, "", "")]
        BLM_AoE_Adv_UmbralSoul = 2049,

        [ParentCombo(BLM_AoE_AdvancedMode)]
        [CustomComboInfo("Cooldown Options", "Select which cooldowns to add to the rotation.", BLM.JobID, 1, "", "")]
        BLM_AoE_Adv_Cooldowns = 2052,

        [ParentCombo(BLM_AoE_SimpleMode)]
        [CustomComboInfo("秽浊/魔泉 核爆 功能", "当自身处于星极火状态且秽浊可用时插入秽浊，在秽浊后插入魔泉以便再使用一次核爆。", BLM.JobID, 0, "", "")]
        BLM_AoE_Simple_Foul = 2020,

        #endregion


        #region Variant

        [Variant]
        [VariantParent(BLM_ST_SimpleMode, BLM_AoE_SimpleMode)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", BLM.JobID)]
        BLM_Variant_Rampart = 2032,

        [Variant]
        [CustomComboInfo("复活 选项", "当你有即刻BUFF时，替换即刻为成多变复活", BLM.JobID)]
        BLM_Variant_Raise = 2033,

        [Variant]
        [VariantParent(BLM_ST_SimpleMode, BLM_AoE_SimpleMode)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", BLM.JobID)]
        BLM_Variant_Cure = 2034,

        #endregion

        #region Miscellaneous

        [ReplaceSkill(BLM.Transpose)]
        [CustomComboInfo("灵极魂/星灵移位功能", "当灵极魂可用时将星灵移位替换为灵极魂。", BLM.JobID, 0, "", "")]
        BLM_UmbralSoul = 2001,

        [ReplaceSkill(BLM.LeyLines)]
        [CustomComboInfo("魔纹步功能", "使用黑魔纹后将其替换为魔纹步。", BLM.JobID, 0, "", "")]
        BLM_Between_The_LeyLines = 2002,

        [ReplaceSkill(BLM.Blizzard, BLM.Freeze)]
        [CustomComboInfo("冰1/2/3", "当自身没有灵极冰状态时替换冰结为冰封，根据自身等级自动替换玄冰为冰冻。", BLM.JobID, 0, "", "")]
        BLM_Blizzard_1to3 = 2003,


        [ReplaceSkill(BLM.Scathe)]
        [ConflictingCombos(BLM_ST_SimpleMode, BLM_ST_AdvancedMode)]
        [CustomComboInfo("异言功能", "当异言可用时替换崩溃为异言。", BLM.JobID, 0, "", "")]
        BLM_Scathe_Xeno = 2004,

        [ReplaceSkill(BLM.Fire)]
        [CustomComboInfo("火炎/爆炎功能", "当自身没有星极火状态或使用火起手时，用爆炎替换火炎。", BLM.JobID, 0, "", "")]
        BLM_Fire_1to3 = 2005,


        [ReplaceSkill(BLM.AetherialManipulation)]
        [CustomComboInfo
        (
            "Aetherial Manipulation Feature",
            "Replaces Aetherial Manipulation with Between the Lines when you are out of active Ley Lines and standing still.", BLM.JobID
        )]
        BLM_Aetherial_Manipulation = 2046,

        #endregion

        // Last value = 2034

        #endregion

        #region BLUE MAGE

        [BlueInactive(BLU.SongOfTorment, BLU.Bristle)]
        [ReplaceSkill(BLU.SongOfTorment)]
        [CustomComboInfo("苦闷之歌buff强化", "将苦闷之歌用怒发冲冠代替，使苦闷之歌处于怒发冲冠的buff下", BLU.JobID)]
        BLU_BuffedSoT = 70000,

        [BlueInactive
        (
            BLU.Whistle, BLU.Tingle, BLU.MoonFlute, BLU.JKick, BLU.TripleTrident, BLU.Nightbloom, BLU.RoseOfDestruction, BLU.FeatherRain,
            BLU.Bristle, BLU.GlassDance, BLU.Surpanakha, BLU.MatraMagic, BLU.ShockStrike, BLU.PhantomFlurry
        )]
        [ReplaceSkill(BLU.MoonFlute)]
        [ConflictingCombos(BLU_NewMoonFluteOpener)]
        [CustomComboInfo("月笛开启器", "将满月笛开启器放在月笛或口哨上。", BLU.JobID)]
        BLU_Opener = 70001,

        [BlueInactive(BLU.MoonFlute, BLU.Tingle, BLU.ShockStrike, BLU.Whistle, BLU.FinalSting)]
        [ReplaceSkill(BLU.FinalSting)]
        [CustomComboInfo("终极针连击", "将终极之刺变成月笛叮当口哨终极之刺的组合效果： 月笛、叮当、口哨、最终刺痛。在施放最终刺痛前，会使用任何已冷却的原初效果。", BLU.JobID)]
        BLU_FinalSting = 70002,

        [BlueInactive(BLU.RoseOfDestruction, BLU.FeatherRain, BLU.GlassDance, BLU.JKick)]
        [ParentCombo(BLU_FinalSting)]
        [CustomComboInfo("蛮神技能冷却选项", "将任何已经冷却完毕的蛮神技能加入终极针连击中", BLU.JobID)]
        BLU_Primals = 70003,

        [BlueInactive(BLU.RamsVoice, BLU.Ultravibration)]
        [ReplaceSkill(BLU.Ultravibration)]
        [CustomComboInfo("寒冰咆哮-超振动", "如果目标未被冰冻，则将超震动更改为寒冰咆哮。如果目标已被冰冻，则会即刻咏唱超震动。", BLU.JobID)]
        BLU_Ultravibrate = 70005,

        [BlueInactive(BLU.Offguard, BLU.BadBreath, BLU.Devour)]
        [ReplaceSkill(BLU.Devour, BLU.Offguard, BLU.BadBreath)]
        [CustomComboInfo("坦克Debuff特性", "在坦克模仿状态下，将吞噬、脱离警戒、清醒梦境和口臭放入一个按钮。", BLU.JobID)]
        BLU_DebuffCombo = 70006,

        [BlueInactive(BLU.MagicHammer)]
        [ReplaceSkill(BLU.MagicHammer)]
        [CustomComboInfo("昏乱/魔法锤Debuff特性", "当冷却完毕时，用魔法锤代替昏乱", BLU.JobID)]
        BLU_Addle = 70007,

        [BlueInactive(BLU.FeatherRain, BLU.ShockStrike, BLU.RoseOfDestruction, BLU.GlassDance)]
        [ReplaceSkill(BLU.FeatherRain)]
        [CustomComboInfo("蛮神技特性", "在冷却结束后，将所有蛮神技能整合至飞翎羽。\n如果在冷却未结束的时候使用，将导致月之笛爆发不完整。", BLU.JobID)]
        BLU_PrimalCombo = 70008,

        [BlueInactive(BLU.BlackKnightsTour, BLU.WhiteKnightsTour)]
        [ReplaceSkill(BLU.BlackKnightsTour, BLU.WhiteKnightsTour)]
        [CustomComboInfo("骑士之旅特性", "在敌人处于“止步”或“减速”状态下时，自动将在黑/白骑士之旅间切换。", BLU.JobID)]
        BLU_KnightCombo = 70009,

        [BlueInactive(BLU.PeripheralSynthesis, BLU.MustardBomb)]
        [ReplaceSkill(BLU.PeripheralSynthesis)]
        [CustomComboInfo("生成外设-芥末爆弹", "当目标受到 '光头强'的影响时，生成外设-芥末爆弹。", BLU.JobID)]
        BLU_LightHeadedCombo = 70010,

        [BlueInactive(BLU.BasicInstinct)]
        [ParentCombo(BLU_FinalSting)]
        [CustomComboInfo("单挑模式", "如果你在单挑副本，使用斗争本能。", BLU.JobID)]
        BLU_SoloMode = 70011,

        [BlueInactive(BLU.HydroPull)]
        [ParentCombo(BLU_Ultravibrate)]
        [CustomComboInfo("水力吸引设置", "在使用寒冰咆哮之前先用水力吸引。", BLU.JobID)]
        BLU_HydroPull = 70012,

        [BlueInactive(BLU.JKick)]
        [ParentCombo(BLU_PrimalCombo)]
        [CustomComboInfo("正义飞踢选项", "将正义飞踢加入蛮神技能选项", BLU.JobID)]
        BLU_PrimalCombo_JKick = 70013,


        [BlueInactive(BLU.SeaShanty)]
        [ParentCombo(BLU_PrimalCombo)]
        [CustomComboInfo("海洋棚屋选项", "为原始特质添加海洋棚屋", BLU.JobID)]
        BLU_PrimalCombo_SeaShanty = 70024,



        [BlueInactive(BLU.PerpetualRay, BLU.SharpenedKnife)]
        [CustomComboInfo("永恒射线-锋利菜刀", "当目标是眩晕且在近战范围内时，将永恒射线转为锋利菜刀。", BLU.JobID)]
        BLU_PerpetualRayStunCombo = 70014,

        [BlueInactive(BLU.FeatherRain, BLU.ShockStrike, BLU.RoseOfDestruction, BLU.GlassDance)]
        [ParentCombo(BLU_PrimalCombo)]
        [CustomComboInfo("月之笛爆发选项", "如果技能已经冷却完毕而月之笛即将冷却结束，则保留冷却完毕的技能至月之笛爆发期使用。", BLU.JobID)]
        BLU_PrimalCombo_Pool = 70015,

        [BlueInactive(BLU.SonicBoom, BLU.SharpenedKnife)]
        [CustomComboInfo("音爆选项", "在近战范围内，使用锋利菜刀替代音爆。", BLU.JobID)]
        BLU_MeleeCombo = 70016,

        [BlueInactive(BLU.MatraMagic)]
        [ParentCombo(BLU_PrimalCombo)]
        [CustomComboInfo("马特拉魔法选项", "将马特拉魔法添加到蛮神特性中", BLU.JobID)]
        BLU_PrimalCombo_Matra = 70017,

        [BlueInactive(BLU.Surpanakha)]
        [ParentCombo(BLU_PrimalCombo)]
        [CustomComboInfo("穿甲散弹选项", "将穿甲散弹添加到蛮神特性中", BLU.JobID)]
        BLU_PrimalCombo_Suparnakha = 70018,

        [BlueInactive(BLU.PhantomFlurry)]
        [ParentCombo(BLU_PrimalCombo)]
        [CustomComboInfo("鬼宿脚选项", "将鬼宿脚添加到蛮神特性中", BLU.JobID)]
        BLU_PrimalCombo_PhantomFlurry = 70019,

        [BlueInactive(BLU.Nightbloom, BLU.Bristle)]
        [ParentCombo(BLU_PrimalCombo)]
        [CustomComboInfo("月下彼岸花选项", "将月下彼岸花添加到蛮神特性中", BLU.JobID)]
        BLU_PrimalCombo_Nightbloom = 70020,

        [ReplaceSkill(BLU.MoonFlute)]
        [BlueInactive(BLU.Whistle, BLU.Tingle, BLU.RoseOfDestruction, BLU.MoonFlute, BLU.JKick, BLU.TripleTrident, BLU.Nightbloom, BLU.SeaShanty, BLU.BeingMortal, BLU.ShockStrike, BLU.Surpanakha, BLU.MatraMagic, BLU.PhantomFlurry, BLU.Bristle)]
        [ConflictingCombos(BLU_Opener)]
        [CustomComboInfo("全新 BLU 月笛开启器（80 级版）", "将月笛变为完整开启器（80 级更新）", BLU.JobID)]
        BLU_NewMoonFluteOpener = 70021,

        [BlueInactive(BLU.BreathOfMagic, BLU.MortalFlame)]
        [ParentCombo(BLU_NewMoonFluteOpener)]
        [CustomComboInfo("DoT开场者", "将开场者改为使用必杀火焰或魔法之息，而不是使用翼之斥力", BLU.JobID)]
        BLU_NewMoonFluteOpener_DoTOpener = 70022,

        [ReplaceSkill(BLU.DeepClean)]
        [BlueInactive(BLU.PeatPelt, BLU.DeepClean)]
        [CustomComboInfo("泥炭清洁", "如果当前目标没有受到乞丐袭扰，则将'深度清洁'改为'泥炭佩尔特'", BLU.JobID)]
        BLU_PeatClean = 70023,

        [BlueInactive(BLU.WingedReprobation)]
        [ParentCombo(BLU_PrimalCombo)]
        [CustomComboInfo("有翼斥力选项", "Adds 翼斥力 to the combo.", BLU.JobID)]
        BLU_PrimalCombo_WingedReprobation = 70025,

        // Last value = 70020

        #endregion

        #region BARD

        [ReplaceSkill(BRD.HeavyShot, BRD.BurstShot)]
        [ConflictingCombos(BRD_ST_SimpleMode)]
        [CustomComboInfo("直线射击 替换 强力射击 选项", "触发直线射击预备状态时，替换强力射击/爆发射击为直线射击/辉煌箭。", BRD.JobID, 0, "", "")]
        BRD_StraightShotUpgrade = 3001,

        [ConflictingCombos(BRD_ST_SimpleMode)]
        [ParentCombo(BRD_StraightShotUpgrade)]
        [CustomComboInfo("Dot选项", "开启此选项可适时插入毒/风箭。", BRD.JobID, 0, "", "")]
        BRD_DoTMaintainance = 3002,

        [ReplaceSkill(BRD.IronJaws)]
        [ConflictingCombos(BRD_IronJaws_Alternate)]
        [CustomComboInfo("伶牙俐齿续dot模式A", "当目标身上没有毒/风dot时，替换伶牙俐齿为毒/风箭。\n当还未习得伶牙俐齿时会在毒/风箭之间自动切换。", BRD.JobID, 0, "", "")]
        BRD_IronJaws = 3003,

        [ReplaceSkill(BRD.IronJaws)]
        [ConflictingCombos(BRD_IronJaws)]
        [CustomComboInfo("伶牙俐齿续dot模式B", "当目标身上没有毒/风dot时，替换伶牙俐齿为毒/风箭。 \n伶牙俐齿仅会在风/毒dot即将结束时复现。", BRD.JobID, 0, "", "")]
        BRD_IronJaws_Alternate = 3004,

        [ReplaceSkill(BRD.BurstShot, BRD.QuickNock)]
        [ConflictingCombos(BRD_ST_SimpleMode)]
        [CustomComboInfo("爆发射击/连珠箭 替换 绝峰箭 选项", "当灵魂之声蓄满时，替换爆发射击/连珠箭为绝峰箭，触发爆破箭预备状态时替换为爆破箭。", BRD.JobID, 0, "", "")]
        BRD_Apex = 3005,

        [ReplaceSkill(BRD.Bloodletter)]
        [ConflictingCombos(BRD_ST_SimpleMode)]
        [CustomComboInfo("单目标能力技插入选项", "在三歌循环中根据cd时间替换失血箭为其他能力技。", BRD.JobID, 0, "", "")]
        BRD_ST_oGCD = 3006,

        [ReplaceSkill(BRD.RainOfDeath)]
        [ConflictingCombos(BRD_AoE_Combo)]
        [CustomComboInfo("AOE能力技插入选项", "在三歌循环中根据cd时间替换死亡箭雨为其他能力技。", BRD.JobID, 0, "", "")]
        BRD_AoE_oGCD = 3007,

        [ReplaceSkill(BRD.QuickNock, BRD.Ladonsbite)]
        [ConflictingCombos(BRD_AoE_SimpleMode)]
        [CustomComboInfo("AOE连击", "影噬箭 可用时把连珠箭/百首龙牙箭替换为 影噬箭。", BRD.JobID, 0, "", "")]
        BRD_AoE_Combo = 3008,

        [ReplaceSkill(BRD.HeavyShot, BRD.BurstShot)]
        [ConflictingCombos(BRD_StraightShotUpgrade, BRD_DoTMaintainance, BRD_Apex, BRD_ST_oGCD, BRD_IronJawsApex)]
        [CustomComboInfo
        (
            "简易诗人",
            "Adds every single target ability to one button,\nIf there are DoTs on target, Simple Bard will try to maintain their uptime.", BRD.JobID,
            0, "", ""
        )]
        BRD_ST_SimpleMode = 3009,

        [ParentCombo(BRD_ST_SimpleMode)]
        [CustomComboInfo("简易诗人Dots", "如果目标身上不存在风/毒dot，开启此选项会在连击中加入风/毒箭。", BRD.JobID, 0, "", "")]
        BRD_Simple_DoT = 3010,

        [ParentCombo(BRD_ST_SimpleMode)]
        [CustomComboInfo("简易诗人唱歌", "这个选项将诗人的三首歌加入简易诗人功能里", BRD.JobID, 0, "", "")]
        BRD_Simple_Song = 3011,

        [ParentCombo(BRD_AoE_oGCD)]
        [CustomComboInfo("唱歌相关设置", "在AOE连击中加入三首歌循环。", BRD.JobID, 0, "", "")]
        BRD_oGCDSongs = 3012,

        [CustomComboInfo("Buff技能设置", "将猛者强击/战斗之声整合至纷乱箭。", BRD.JobID, 0, "", "")]
        BRD_Buffs = 3013,

        [CustomComboInfo("一键唱歌", "根据CD，将另外两首歌加入放浪神的小步舞曲.", BRD.JobID, 0, "", "")]
        BRD_OneButtonSongs = 3014,

        [ReplaceSkill(BRD.QuickNock, BRD.Ladonsbite)]
        [CustomComboInfo("简易诗人(AOE)", "连珠箭/百首龙牙箭 插入能力技。", BRD.JobID, 0, "", "")]
        BRD_AoE_SimpleMode = 3015,

        [ParentCombo(BRD_AoE_SimpleMode)]
        [CustomComboInfo("简易诗人唱歌(AOE)", "在简单的AOE中插入歌曲。", BRD.JobID, 0, "", "")]
        BRD_AoE_Simple_Songs = 3016,

        [ParentCombo(BRD_ST_SimpleMode)]
        [CustomComboInfo("简易诗人Buffs", "自动插入Buff技能。", BRD.JobID, 0, "", "")]
        BRD_Simple_Buffs = 3017,

        [ParentCombo(BRD_Simple_Buffs)]
        [CustomComboInfo("光明神的最终乐章替换设置", "当可用时自动插入光明神的最终乐章。", BRD.JobID, 0, "", "")]
        BRD_Simple_BuffsRadiant = 3018,

        [ParentCombo(BRD_ST_SimpleMode)]
        [CustomComboInfo
        (
            "防止资源浪费相关选项",
            "Adds enemy health checking on mobs for buffs, DoTs and Songs.\nThey will not be reapplied if less than specified.", BRD.JobID, 0, "",
            ""
        )]
        BRD_Simple_NoWaste = 3019,

        [ParentCombo(BRD_ST_SimpleMode)]
        [CustomComboInfo("单体连击打断技能设置", "如果可用，在循环中加入打断技能", BRD.JobID, 0, "", "")]
        BRD_Simple_Interrupt = 3020,

        [CustomComboInfo("禁用绝峰箭", "不在一键连击中自动替换插入绝峰箭。", BRD.JobID, 0, "", "")]
        BRD_RemoveApexArrow = 3021,

        //[ConflictingCombos(BardoGCDSingleTargetFeature)]
        //[ParentCombo(SimpleBardFeature)]
        //[CustomComboInfo("单体简易起手", "在单体一键连击中加入最佳起手技能。\n此选项与其它绝大部分类似选项均有冲突。", BRD.JobID, 0, "", "")]
        //BardSimpleOpener = 3022,

        [ParentCombo(BRD_ST_SimpleMode)]
        [CustomComboInfo("一键循环异言设置", "将失血箭集中于最佳爆发期", BRD.JobID, 0, "", "")]
        BRD_Simple_Pooling = 3023,

        [ConflictingCombos(BRD_ST_SimpleMode)]
        [ParentCombo(BRD_IronJaws)]
        [CustomComboInfo("伶牙俐齿替换绝峰箭", "在有条件的情况下，将 绝峰箭 和 爆破箭 添加到 伶牙俐齿 上。", BRD.JobID, 0, "", "")]
        BRD_IronJawsApex = 3024,

        [ParentCombo(BRD_ST_SimpleMode)]
        [CustomComboInfo("简易猛者中续伶牙俐齿", "Enable the snapshotting of DoTs, within the remaining time of Raging Strikes below:", BRD.JobID, 0, "", "")]
        BRD_Simple_RagingJaws = 3025,

        [ParentCombo(BRD_Simple_DoT)]
        [CustomComboInfo("只有起手", "你可以自动对新目标释放Dots直到第一次自动刷新(伶牙俐齿)\n即启用本选项后，在第一次自动刷新(伶牙俐齿)后不会对新目标上Dot(包括同一目标上天断Dot后)", BRD.JobID, 0, "", "")]
        BRD_Simple_DoTOpener = 3026,

        [ParentCombo(BRD_AoE_Simple_Songs)]
        [CustomComboInfo("除外放浪神的小步舞曲", "不使用放浪神的小步舞曲", BRD.JobID, 0, "", "")]
        BRD_AoE_Simple_SongsExcludeWM = 3027,

        [ParentCombo(BRD_ST_SimpleMode)]
        [CustomComboInfo("内丹选项", "当低于此生命值百分比时，使用内丹", BRD.JobID, 0, "", "")]
        BRD_ST_SecondWind = 3028,

        [ParentCombo(BRD_AoE_SimpleMode)]
        [CustomComboInfo("内丹选项", "当低于此生命值百分比时，使用内丹", BRD.JobID, 0, "", "")]
        BRD_AoE_SecondWind = 3029,

        [Variant]
        [VariantParent(BRD_ST_SimpleMode, BRD_AoE_SimpleMode)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", BRD.JobID)]
        BRD_Variant_Rampart = 3030,

        [Variant]
        [VariantParent(BRD_ST_SimpleMode, BRD_AoE_SimpleMode)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", BRD.JobID)]
        BRD_Variant_Cure = 3031,

        // Last value = 3031

        #endregion

        #region DANCER

        #region Simple Dancer (Double Targets)

        [ReplaceSkill(DNC.瀑泻Cascade)]
        [CustomComboInfo
        (
            "单体模式",
            "Single button, double targets. Includes songs, flourishes and overprotections.\nConflicts with all other non-simple toggles, except 'Dance Step Combo'.",
            DNC.JobID, 0, "", ""
        )]
        DNC_DT_SimpleMode = 4065,

        
        
        

        [ParentCombo(DNC_DT_SimpleMode)]
        [CustomComboInfo("倒计时自动跳小舞", "起手自动跳舞", DNC.JobID, 0, "", "")]
        DNC_DT_Simple_AUTO_SS = 4091,

        [ParentCombo(DNC_DT_SimpleMode)]
        [CustomComboInfo("卡GCD-大舞小舞", "不知道什么原因小舞大舞卡GCD才能让循环工整、如果你知道告诉我那是最好的了", DNC.JobID, 1, "", "")]
        DNC_DT_Simple_GCD = 4095,
        
        
        [ParentCombo(DNC_DT_SimpleMode)]
        [CustomComboInfo("标准舞步", "Includes Standard Step (and all steps) in the rotation.", DNC.JobID, 2, "", "")]
        DNC_DT_Simple_SS = 4066,

        [ParentCombo(DNC_DT_SimpleMode)]
        [CustomComboInfo("技巧舞步", "Includes Technical Step, all dance steps and Technical Finish in the rotation.", DNC.JobID, 2, "", "")]
        DNC_DT_Simple_TS = 4067,

        [ParentCombo(DNC_DT_SimpleMode)]
        [CustomComboInfo("简易百花争艳", "Includes Flourish in the rotation.", DNC.JobID, 2, "", "")]
        DNC_DT_Simple_Flourish = 4068,


        [ParentCombo(DNC_DT_SimpleMode)]
        [CustomComboInfo("剑舞/晓之舞", "剑舞/晓之舞加入循环", DNC.JobID, 2, "", "")]
        DNC_DT_Simple_SaberDance = 4069,


        [ParentCombo(DNC_DT_SimpleMode)]
        [CustomComboInfo("进攻之探戈", "Includes Flourish in the rotation.", DNC.JobID, 2, "", "")]
        DNC_DT_Simple_Devilment = 4090,



        [ParentCombo(DNC_DT_SimpleMode)]
        [CustomComboInfo("最后一舞", "最后一舞加入循环", DNC.JobID, 2, "", "")]
        DNC_DT_Simple_最后一舞 = 4092,

        [ParentCombo(DNC_DT_SimpleMode)]
        [CustomComboInfo("舞步终结", "舞步终结加入循环", DNC.JobID, 2, "", "")]
        DNC_DT_Simple_舞步终结 = 4093,

        #endregion


        [ReplaceSkill(DNC.标准舞步StandardStep, DNC.技巧舞步TechnicalStep)]
        [CustomComboInfo("舞步连击相关", "在跳舞时将标准舞步和技巧舞步更改为每个舞步.", DNC.JobID, 1, "", "")]
        DNC_DanceStepCombo = 4039,


        #region Simple Dancer (AoE)

        [ReplaceSkill(DNC.风车Windmill)]
        // [ConflictingCombos( )]
        [CustomComboInfo
        (
            "Simple Dancer (AoE) Feature",
            "Single button, AoE. Includes songs, flourishes and overprotections." + "\nConflicts with all other non-simple toggles, except 'Dance Step Combo'.", DNC.JobID, 4, "", ""
        )]
        DNC_AoE_SimpleMode = 4070,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo
        (
            "Simple AoE Interrupt Option", "Includes an interrupt in the AoE rotation (if your current target can be interrupted).",
            DNC.JobID, 0, "", ""
        )]
        DNC_AoE_Simple_Interrupt = 4071,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [ConflictingCombos(DNC_AoE_Simple_StandardFill)]
        [CustomComboInfo("Simple AoE Standard Dance Option", "Includes Standard Step (and all steps) in the AoE rotation.", DNC.JobID, 1, "", "")]
        DNC_AoE_Simple_SS = 4072,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [ConflictingCombos(DNC_AoE_Simple_SS)]
        [CustomComboInfo
        (
            "Simple AoE Standard Fill Option",
            "Adds ONLY Standard dance steps and Standard Finish to the AoE rotation." + "\nStandard Step itself must be initiated manually when using this option.", DNC.JobID, 2, "", ""
        )]
        DNC_AoE_Simple_StandardFill = 4081,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [ConflictingCombos(DNC_AoE_Simple_TechFill)]
        [CustomComboInfo
        (
            "Simple AoE Technical Dance Option", "Includes Technical Step, all dance steps and Technical Finish in the AoE rotation.",
            DNC.JobID, 3, "", ""
        )]
        DNC_AoE_Simple_TS = 4073,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [ConflictingCombos(DNC_AoE_Simple_TS)]
        [CustomComboInfo
        (
            "Simple AoE Tech Fill Option",
            "Adds ONLY Technical dance steps and Technical Finish to the AoE rotation." + "\nTechnical Step itself must be initiated manually when using this option.", DNC.JobID, 4, "", ""
        )]
        DNC_AoE_Simple_TechFill = 4074,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo
        (
            "Simple AoE Tech Devilment Option",
            "Includes Devilment in the AoE rotation." + "\nWill activate only during Technical Finish if you Lv70 or above.", DNC.JobID, 5, "", ""
        )]
        DNC_AoE_Simple_Devilment = 4075,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo("Simple AoE Flourish Option", "Includes Flourish in the AoE rotation.", DNC.JobID, 6, "", "")]
        DNC_AoE_Simple_Flourish = 4076,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo("Simple AoE Feathers Option", "Includes feather usage in the AoE rotation.", DNC.JobID, 7, "", "")]
        DNC_AoE_Simple_Feathers = 4077,

        [ParentCombo(DNC_AoE_Simple_Feathers)]
        [CustomComboInfo
        (
            "Simple AoE Feather Pooling Option", "Expends a feather in the next available weave window when capped.", DNC.JobID, 8, "",
            ""
        )]
        DNC_AoE_Simple_FeatherPooling = 4078,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo
        (
            "Simple AoE Panic Heals Option",
            "Includes Curing Waltz and Second Wind in the AoE rotation when available and your HP is below the set percentages.", DNC.JobID, 9, "",
            ""
        )]
        DNC_AoE_Simple_PanicHeals = 4079,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo("Simple AoE Improvisation Option", "Includes Improvisation in the AoE rotation when available.", DNC.JobID, 10, "", "")]
        DNC_AoE_Simple_Improvisation = 4080,


        [Variant]
        [VariantParent(DNC_AoE_SimpleMode)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", DNC.JobID)]
        DNC_Variant_Rampart = 4085,

        [Variant]
        [VariantParent(DNC_DT_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", DNC.JobID)]
        DNC_Variant_Cure = 4086,

        #endregion

        #endregion

        #region DARK KNIGHT

        [ReplaceSkill(DRK.SyphonStrike)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            DRK.JobID, -10, "", ""
        )]
        DRK_Advanced_CustomMode = 50981,


        [ParentCombo(DRK_SouleaterCombo)]
        [CustomComboInfo("主连击Buff整合", "将所有增益技能整合至噬魂斩连击中.", DRK.JobID, 0, "", "")]
        DRK_MainComboBuffs_Group = 5098,

        [ConflictingCombos(DRK_oGCD)]
        [ParentCombo(DRK_SouleaterCombo)]
        [CustomComboInfo("暗黑骑士能力技整合 设置", "将所有能力技整合至噬魂斩连击中.", DRK.JobID, 0, "", "")]
        DRK_MainComboCDs_Group = 5099,

        [ReplaceSkill(DRK.Souleater)]
        [CustomComboInfo("噬魂斩连击", "用基础循环替换掉噬魂斩。 \n如果所有的次级选项都被开启，那么就可以进行一键循环(简单黑骑)", DRK.JobID, 0, "", "")]
        DRK_SouleaterCombo = 5000,

        [ReplaceSkill(DRK.刚魂StalwartSoul)]
        [CustomComboInfo("刚魂连击", "用基础循环替换掉刚魂。", DRK.JobID, 0, "", "")]
        DRK_StalwartSoulCombo = 5001,

        // [ReplaceSkill(DRK.Souleater)]
        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("血溅 设置", "当血乱激活时，使用血溅和寂灭替换掉噬魂斩和刚魂。", DRK.JobID, 0, "", "")]
        DRK_Bloodspiller = 5002,

        // [ReplaceSkill(DRK.StalwartSoul)]
        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("暗血量表溢出特性", "当你的暗血即将溢出时，使用寂灭替换掉刚魂。", DRK.JobID, 0, "", "")]
        DRK_Overcap = 5003,

        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("掠影示现特性", "当掠影示现未在冷却中将其插入主连击中.", DRK.JobID, 0, "", "")]
        DRK_LivingShadow = 5004,

        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("蔑视厌恶特性", "当蔑视厌恶在冷却中将其插入主连击中.", DRK.JobID, 0, "", "")]
        DRK_蔑视厌恶 = 50041,

        [ParentCombo(DRK_SouleaterCombo)]
        [CustomComboInfo
        (
            "暗影锋防覆盖 设置", "Uses Edge of Shadow if you are above 8,500 mana or Darkside is about to expire (10sec or less)", DRK.JobID, 0,
            "", ""
        )]
        DRK_ManaOvercap = 5005,

        [ReplaceSkill(DRK.精雕怒斩CarveAndSpit, DRK.AbyssalDrain)]
        [ConflictingCombos(DRK_MainComboCDs_Group)]
        [CustomComboInfo("能力技特性", "按照掠影示现 > 腐秽大地 > 精雕怒斩 > 腐秽黑暗的顺序将能力技整合到精雕怒斩与吸血深渊。", DRK.JobID, 0, "", "")]
        DRK_oGCD = 5006,

        [ParentCombo(DRK_oGCD)]
        [CustomComboInfo("暗影使者设置", "将暗影使者整合到能力技特性", DRK.JobID, 0, "", "")]
        DRK_Shadowbringer_oGCD = 5007,


        [ParentCombo(DRK_Bloodspiller)]
        [CustomComboInfo("延后使用血溅设置", "当血乱在偶数窗口使用时，将血溅延迟2个GCD，在奇数窗口直接使用。更容易对团辅。", DRK.JobID, 0, "", "")]
        DRK_DelayedBloodspiller = 5010,

        [ParentCombo(DRK_SouleaterCombo)]
        [CustomComboInfo("伤残特性", "在射程外替换为伤残", DRK.JobID, 0, "", "")]
        DRK_RangedUptime = 5011,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("吸血深渊特性", "当你的血量低于60%时，在刚魂连击中插入吸血深渊.", DRK.JobID, 0, "", "")]
        DRK_AoE_AbyssalDrain = 5013,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("AOE暗影使者特性", "在刚魂连击中插入暗影使者.", DRK.JobID, 0, "", "")]
        DRK_AoE_Shadowbringer = 5014,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("暗影波动防溢出 设置", "当你的mp值高于8500或暗影状态即将结束时", DRK.JobID, 0, "", "")]
        DRK_AoE_ManaOvercap = 5015,

        [ParentCombo(DRK_SouleaterCombo)]
        [CustomComboInfo("暗血量表溢出特性", "当你的暗血不低于80时，将血溅整合到主连击。", DRK.JobID, 0, "", "")]
        DRK_BloodGaugeOvercap = 5016,

        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("暗影使者能力技特性", "当自身存在暗影状态时插入暗影使者至主连击中.会使用所有可用层数.", DRK.JobID, 0, "", "")]
        DRK_Shadowbringer = 5019,

        [ParentCombo(DRK_ManaOvercap)]
        [CustomComboInfo("暗影锋爆发设置", "在爆发期直到MP到达设定值之前都会一直插入暗影锋.", DRK.JobID, 0, "", "")]
        DRK_EoSPooling = 5020,

        [ParentCombo(DRK_Shadowbringer)]
        [CustomComboInfo("暗影使者爆发设置", "将暗影使者打入偶数分钟的爆发窗口中。", DRK.JobID, 0, "", "")]
        DRK_ShadowbringerBurst = 5021,

        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("精雕怒斩特性", "当自身存在暗影状态时插入精雕怒斩至主连击中.", DRK.JobID, 0, "", "")]
        DRK_CarveAndSpit = 5022,


        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("腐秽大地特性", "若自身存在暗影状态且腐秽大地未在冷却中.", DRK.JobID, 0, "", "")]
        DRK_SaltedEarth = 5024,

        [ParentCombo(DRK_MainComboBuffs_Group)]
        [CustomComboInfo("血乱整合设置", "当自身存在暗影状态且血乱可用时插入血乱.同样当血乱即将结束冷却时会根据自身暗血值先插入血溅/寂灭以防止暗血值溢出.", DRK.JobID, 0, "", "")]
        DRK_Delirium = 5025,

        [ParentCombo(DRK_MainComboBuffs_Group)]
        [CustomComboInfo("嗜血设置", "若自身存在暗影状态且嗜血未在冷却中", DRK.JobID, 0, "", "")]
        DRK_BloodWeapon = 5026,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("嗜血设置", "若自身存在暗影状态且嗜血未在冷却中", DRK.JobID, 0, "", "")]
        DRK_AoE_BloodWeapon = 5027,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("血乱设置", "若自身存在暗影状态且嗜血未在冷却中", DRK.JobID, 0, "", "")]
        DRK_AoE_Delirium = 5028,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("腐秽大地设置", "若自身存在暗影状态且腐秽大地未在冷却中", DRK.JobID, 0, "", "")]
        DRK_AoE_SaltedEarth = 5029,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("掠影示现设置", "若自身存在暗影状态且掠影示现未在冷却中", DRK.JobID, 0, "", "")]
        DRK_AoE_LivingShadow = 5030,

        [Variant]
        [VariantParent(DRK_SouleaterCombo, DRK_StalwartSoulCombo)]
        [CustomComboInfo("精神镖选项", "当没有dot或dot剩余时间少于3s时，使用多变精神镖", DRK.JobID)]
        DRK_Variant_SpiritDart = 5031,

        [Variant]
        [VariantParent(DRK_SouleaterCombo, DRK_StalwartSoulCombo)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", DRK.JobID)]
        DRK_Variant_Cure = 5032,

        [Variant]
        [VariantParent(DRK_SouleaterCombo, DRK_StalwartSoulCombo)]
        [CustomComboInfo("最后通牒 选项", "冷却结束时使用多变最后通牒", DRK.JobID)]
        DRK_Variant_Ultimatum = 5033,

        // Last value = 5033

        #endregion

        #region DRAGOON

        [ReplaceSkill(DRG.VorpalThrust)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            DRG.JobID, -10, "", ""
        )]
        DRG_Advanced_CustomMode = 60001,

        [ReplaceSkill(DRG.TrueThrust)]
        [ConflictingCombos(DRG_ST_AdvancedMode)]
        [CustomComboInfo("Simple Mode - Single Target", "Replaces True Thrust with a full one-button single target rotation.\nThis is the ideal option for newcomers to the job.", DRG.JobID)]
        DRG_ST_SimpleMode = 6001,

        #region Advanced ST Dragoon

        [ReplaceSkill(DRG.TrueThrust)]
        [ConflictingCombos(DRG_ST_SimpleMode)]
        [CustomComboInfo("Advanced Mode - Single Target", "Replaces True Thrust with a full one-button single target rotation.\nThese features are ideal if you want to customize the rotation.", DRG.JobID)]
        DRG_ST_AdvancedMode = 6100,

        [ParentCombo(DRG_ST_AdvancedMode)]
        [CustomComboInfo("Level 100 Opener", "Adds the Balance opener to the rotation.", DRG.JobID)]
        DRG_ST_Opener = 6101,

        #region Buffs ST

        [ParentCombo(DRG_ST_AdvancedMode)]
        [CustomComboInfo("Buffs Option", "Adds various buffs to the rotation.", DRG.JobID, 2, "", "")]
        DRG_ST_Buffs = 6102,

        [ParentCombo(DRG_ST_Buffs)]
        [CustomComboInfo("Battle Litany Option", "Adds Battle Litany to the rotation.", DRG.JobID)]
        DRG_ST_Litany = 6103,

        [ParentCombo(DRG_ST_Buffs)]
        [CustomComboInfo("Lance Charge Option", "Adds Lance Charge to the rotation.", DRG.JobID)]
        DRG_ST_Lance = 6104,

        #endregion

        #region Cooldowns ST

        [ParentCombo(DRG_ST_AdvancedMode)]
        [CustomComboInfo("Cooldowns Option", "Adds various cooldowns to the rotation.", DRG.JobID)]
        DRG_ST_CDs = 6106,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("Life Surge Option", "Adds Life Surge, on the proper GCD, to the rotation.", DRG.JobID)]
        DRG_ST_LifeSurge = 6107,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("Dragonfire Dive Option", "Adds Dragonfire Dive to the rotation.", DRG.JobID)]
        DRG_ST_Dives_Dragonfire = 6108,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("Rise of the Dragon Option", "Adds Rise of the Dragon to the rotation.", DRG.JobID)]
        DRG_ST_Dives_RiseOfTheDragon = 6109,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("Stardiver Option", "Adds Stardiver to the rotation.", DRG.JobID)]
        DRG_ST_Stardiver = 6110,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("Starcross Option", "Adds Starcross to the rotation.", DRG.JobID)]
        DRG_ST_Starcross = 6111,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("High Jump Option", "Adds (High) Jump to the rotation.", DRG.JobID)]
        DRG_ST_HighJump = 6112,

        [ParentCombo(DRG_ST_HighJump)]
        [CustomComboInfo("Mirage Dive Option", "Adds Mirage Dive to the rotation.", DRG.JobID)]
        DRG_ST_Mirage = 6113,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("Geirskogul Option", "Adds Geirskogul to the rotation.", DRG.JobID)]
        DRG_ST_Geirskogul = 6114,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("Nastrond Option", "Adds Nastrond to the rotation.", DRG.JobID)]
        DRG_ST_Nastrond = 6115,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("Wyrmwind Thrust Option", "Adds Wyrmwind Thrust to the rotation.", DRG.JobID)]
        DRG_ST_Wyrmwind = 6116,

        #endregion

        [ParentCombo(DRG_ST_AdvancedMode)]
        [CustomComboInfo("Ranged Uptime Option", "Adds Piercing Talon to the rotation when you are out of melee range.", DRG.JobID)]
        DRG_ST_RangedUptime = 6117,

        [ParentCombo(DRG_ST_AdvancedMode)]
        [CustomComboInfo("Combo Heals Option", "Adds Bloodbath and Second Wind to the rotation.", DRG.JobID)]
        DRG_ST_ComboHeals = 6118,

        [ParentCombo(DRG_ST_AdvancedMode)]
        [CustomComboInfo("Dynamic True North Option", "Adds True North before Chaos Thrust/Chaotic Spring, Fang And Claw and Wheeling Thrust when you are not in the correct position for the enhanced potency bonus.", DRG.JobID)]
        DRG_TrueNorthDynamic = 6119,

        #endregion

        [ReplaceSkill(DRG.DoomSpike)]
        [ConflictingCombos(DRG_AOE_AdvancedMode)]
        [CustomComboInfo("Simple Mode - AoE", "Replaces Doom Spike with a full one-button AoE rotation.\nThis is the ideal option for newcomers to the job.", DRG.JobID)]
        DRG_AOE_SimpleMode = 6200,

        #region Advanced AoE Dragoon

        [ReplaceSkill(DRG.DoomSpike)]
        [ConflictingCombos(DRG_AOE_SimpleMode)]
        [CustomComboInfo("Advanced Mode - AoE", "Replaces Doom Spike with a full one-button AoE rotation.\nThese features are ideal if you want to customize the rotation.", DRG.JobID)]
        DRG_AOE_AdvancedMode = 6201,

        #region Buffs AoE

        [ParentCombo(DRG_AOE_AdvancedMode)]
        [CustomComboInfo("Buffs AoE Option", "Adds Lance Charge and Battle Litany to the rotation.", DRG.JobID)]
        DRG_AoE_Buffs = 6202,

        [ParentCombo(DRG_AoE_Buffs)]
        [CustomComboInfo("Battle Litany AoE Option", "Adds Battle Litany to the rotation.", DRG.JobID)]
        DRG_AoE_Litany = 6203,

        [ParentCombo(DRG_AoE_Buffs)]
        [CustomComboInfo("Lance Charge AoE Option", "Adds Lance Charge to the rotation.", DRG.JobID)]
        DRG_AoE_Lance = 6204,

        #endregion

        #region CDs AoE

        [ParentCombo(DRG_AOE_AdvancedMode)]
        [CustomComboInfo("Cooldowns Option", "Adds various cooldowns to the rotation.", DRG.JobID)]
        DRG_AoE_CDs = 6205,

        [ParentCombo(DRG_AoE_CDs)]
        [CustomComboInfo("Life Surge Option", "Adds Life Surge, onto proper GCDs, to the rotation.", DRG.JobID)]
        DRG_AoE_LifeSurge = 6206,

        [ParentCombo(DRG_AoE_CDs)]
        [CustomComboInfo("Dragonfire Dive Option", "Adds Dragonfire Dive to the rotation.", DRG.JobID)]
        DRG_AoE_Dragonfire_Dive = 6207,

        [ParentCombo(DRG_AoE_CDs)]
        [CustomComboInfo("Rise of the Dragon Option", "Adds Rise of the Dragonj to the rotation.", DRG.JobID)]
        DRG_AoE_RiseOfTheDragon = 6208,

        [ParentCombo(DRG_AoE_CDs)]
        [CustomComboInfo("Stardiver Option", "Adds Stardiver to the rotation.", DRG.JobID)]
        DRG_AoE_Stardiver = 6209,

        [ParentCombo(DRG_AoE_CDs)]
        [CustomComboInfo("Starcross Option", "Adds Starcross to the rotation.", DRG.JobID)]
        DRG_AoE_Starcross = 6210,

        [ParentCombo(DRG_AoE_CDs)]
        [CustomComboInfo("High Jump Option", "Adds (High) Jump to the rotation.", DRG.JobID)]
        DRG_AoE_HighJump = 6211,

        [ParentCombo(DRG_AoE_HighJump)]
        [CustomComboInfo("Mirage Dive Option", "Adds Mirage Dive to the rotation.", DRG.JobID)]
        DRG_AoE_Mirage = 6212,

        [ParentCombo(DRG_AoE_CDs)]
        [CustomComboInfo("Geirskogul Option", "Adds Geirskogul to the rotation.", DRG.JobID)]
        DRG_AoE_Geirskogul = 6213,

        [ParentCombo(DRG_AoE_CDs)]
        [CustomComboInfo("Nastrond Option", "Adds Nastrond to the rotation.", DRG.JobID)]
        DRG_AoE_Nastrond = 6214,

        [ParentCombo(DRG_AoE_CDs)]
        [CustomComboInfo("Wyrmwind Option", "Adds Wyrmwind Thrust to the rotation.", DRG.JobID)]
        DRG_AoE_Wyrmwind = 6215,

        #endregion

        [ParentCombo(DRG_AOE_AdvancedMode)]
        [CustomComboInfo("Ranged Uptime Option", "Adds Piercing Talon to the rotation when you are out of melee range.", DRG.JobID)]
        DRG_AoE_RangedUptime = 6216,

        [ParentCombo(DRG_AOE_AdvancedMode)]
        [CustomComboInfo("Combo Heals Option", "Adds Bloodbath and Second Wind to the rotation.", DRG.JobID)]
        DRG_AoE_ComboHeals = 6217,

        #endregion

        [ReplaceSkill(DRG.LanceCharge)]
        [CustomComboInfo("Lance Charge to Battle Litany Feature", "Turns Lance Charge into Battle Litany when the former is on cooldown.", DRG.JobID)]
        DRG_BurstCDFeature = 6301,

        [Variant]
        [VariantParent(DRG_ST_AdvancedMode, DRG_AOE_AdvancedMode)]
        [CustomComboInfo("Cure Option", "Use Variant Cure when HP is below set threshold.", DRG.JobID)]
        DRG_Variant_Cure = 6302,

        [Variant]
        [VariantParent(DRG_ST_AdvancedMode, DRG_AOE_AdvancedMode)]
        [CustomComboInfo("Rampart Option", "Use Variant Rampart on cooldown.", DRG.JobID)]
        DRG_Variant_Rampart = 6303,

        #endregion

        #region GUNBREAKER

        [ReplaceSkill(GNB.残暴弹BrutalShell)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            BLM.JobID, -10, "", ""
        )]
        GNB_Advanced_CustomMode = 69999,


        #region ST

        [ReplaceSkill(GNB.利刃斩KeenEdge)]
        [CustomComboInfo("绝枪战士一键连击", "替换利刃斩和爆发击防止子弹溢出", GNB.JobID, 0, "", "")]
        GNB_ST_MainCombo = 7001,

        [ParentCombo(GNB_ST_MainCombo)]
        [CustomComboInfo("起手设置", "设置多少秒起手\n建议自定义循环起手", GNB.JobID, 0)]
        GNB_START_GCD = 6999,

        [ParentCombo(GNB_ST_MainCombo)]
        [CustomComboInfo("先打什么", "尽可能按照下面的循环打\n 默认是倍攻-子弹连-师心连", GNB.JobID, 0)]
        GNB_ST_SkSSupport = 7000,

        #region Gnashing Fang

        [ParentCombo(GNB_ST_MainCombo)]
        [CustomComboInfo("烈牙与续剑整合到主连击", "将烈牙与续剑整合到主连击。关闭之后后续连击仍然会被替换到主连击。", GNB.JobID, 0, "", "")]
        GNB_ST_Gnashing = 7002,

        #endregion

        #region Cooldowns

        [ParentCombo(GNB_ST_MainCombo)]
        [CustomComboInfo("绝枪战士能力技整合 设置", "选择是否在无情状态下将各种能力技整合至主连击中", GNB.JobID, 0, "", "")]
        GNB_ST_MainCombo_CooldownsGroup = 7004,

        [ParentCombo(GNB_ST_MainCombo_CooldownsGroup)]
        [CustomComboInfo("倍攻整合到主连击", "在无情状态下才在主连击中插入倍攻", GNB.JobID, 0, "", "")]
        GNB_ST_DoubleDown = 7005,

        [ParentCombo(GNB_ST_MainCombo_CooldownsGroup)]
        [CustomComboInfo("危险领域/爆破领域整合到主连击", "添加危险领域/爆破领域整合到主连击", GNB.JobID, 0, "", "")]
        GNB_ST_BlastingZone = 7006,

        [ParentCombo(GNB_ST_MainCombo_CooldownsGroup)]
        [CustomComboInfo("血壤整合到主连击", "当晶壤为0时在主连击中插入血壤.", GNB.JobID, 0, "", "")]
        GNB_ST_Bloodfest = 7007,

        [ParentCombo(GNB_ST_MainCombo_CooldownsGroup)]
        [CustomComboInfo("无情整合到主连击", "当晶壤积累满时在主连击中插入无情.", GNB.JobID, 0, "", "")]
        GNB_ST_NoMercy = 7008,

        [ParentCombo(GNB_ST_MainCombo_CooldownsGroup)]
        [CustomComboInfo("弓形冲波整合到主连击", "弓形冲波整合到主连击", GNB.JobID, 0, "", "")]
        GNB_ST_BowShock = 7009,


        [ParentCombo(GNB_ST_MainCombo_CooldownsGroup)]
        [CustomComboInfo("爆发击整合到主连击", "在主连击中插入爆发击和超高速(若可用).", GNB.JobID, 0, "", "")]
        GNB_ST_BurstStrike = 7011,

        #endregion


        [ParentCombo(GNB_ST_MainCombo)]
        [CustomComboInfo("闪雷弹激活", "当自身与所选目标在近战距离外时，插入闪雷弹.", GNB.JobID, 0, "", "")]
        GNB_ST_RangedUptime = 7014,

        #endregion

        #region Gnashing Fang

        [ReplaceSkill(GNB.烈牙GnashingFang)]
        [CustomComboInfo("烈牙连击", "将续剑添加到烈牙.", GNB.JobID, 0, "", "")]
        GNB_GF_Continuation = 7200,

        [ParentCombo(GNB_GF_Continuation)]
        [CustomComboInfo("无情整合到烈牙", "当无情冷却结束时将其整合到烈牙.", GNB.JobID, 0, "", "")]
        GNB_GF_NoMercy = 7201,

        [ParentCombo(GNB_GF_Continuation)]
        [CustomComboInfo("倍攻整合到烈牙", "当拥有无情BUFF时将倍攻整合到烈牙", GNB.JobID, 0, "", "")]
        GNB_GF_DoubleDown = 7202,

        [ParentCombo(GNB_GF_Continuation)]
        [CustomComboInfo("烈牙CD选项", "在咬裂牙技能上添加血壤、音速破、弓形冲波和爆破领域，顺序取决于无情增益效果。\n如果在无情增益效果持续时有充能，则添加爆发击和超高速。", GNB.JobID, 0, "", "")]
        GNB_GF_Cooldowns = 7203,

        #endregion

        #region AoE

        [ReplaceSkill(GNB.恶魔切DemonSlice)]
        [CustomComboInfo("绝枪战士AOE功能", "将恶魔杀替换为恶魔杀连击。", GNB.JobID, 0, "", "")]
        GNB_AoE_MainCombo = 7300,

        [ParentCombo(GNB_AoE_MainCombo)]
        [CustomComboInfo("无情整合至AoE连击", "当无情可用时在AoE连击中插入无情.", GNB.JobID, 0, "", "")]
        GNB_AoE_NoMercy = 7301,

        [ParentCombo(GNB_AoE_MainCombo)]
        [CustomComboInfo("弓形冲波整合到AOE连击", "当可用时在AoE连击中插入弓形冲波.", GNB.JobID, 0, "", "")]
        GNB_AoE_BowShock = 7302,

        [ParentCombo(GNB_AoE_MainCombo)]
        [CustomComboInfo("血壤整合至AoE连击", "当可用时在AoE连击中插入血壤.会先使用命运之环以确保使用血壤不会使晶壤溢出.", GNB.JobID, 0, "", "")]
        GNB_AoE_Bloodfest = 7303,

        [ParentCombo(GNB_AoE_MainCombo)]
        [CustomComboInfo("倍攻整合至AoE连击", "当倍攻可用且晶壤有2个以上时在AoE连击中插入倍攻.", GNB.JobID, 0, "", "")]
        GNB_AoE_DoubleDown = 7304,

        [ParentCombo(GNB_AoE_MainCombo)]
        [CustomComboInfo("危险领域AOE选项", "当可用时在AoE连击中插入危险领域.", GNB.JobID, 0, "", "")]
        GNB_AOE_DangerZone = 7305,

        [ParentCombo(GNB_AoE_MainCombo)]
        [CustomComboInfo("音速破AOE选项", "当可用时在AoE连击中插入音速破.", GNB.JobID, 0, "", "")]
        GNB_AOE_SonicBreak = 7306,

        [ParentCombo(GNB_AoE_MainCombo)]
        [CustomComboInfo("晶壤溢出特性", "当晶壤将要溢出时使用命运之环替换掉AOE连击。", GNB.JobID, 0, "", "")]
        GNB_AOE_Overcap = 7307,

        [ParentCombo(GNB_AoE_MainCombo)]
        [CustomComboInfo("师心连", "当可师心连可以使用的时候用师心连代替AOE连击", GNB.JobID, 0, "", "")]
        GNB_AOE_ReignOfBeasts = 7308,

        #endregion

        #region Burst Strike

        [ReplaceSkill(GNB.爆发击BurstStrike)]
        [CustomComboInfo("爆发击 Feature", "爆发击 相关功能", GNB.JobID, 0, "", "")]
        GNB_BS = 7400,


        [ParentCombo(GNB_BS)]
        [CustomComboInfo("爆发击续剑", "超高速可以使用的时候，超高速替换爆发击", GNB.JobID, 0, "", "")]
        GNB_BS_Continuation = 7401,

        [ParentCombo(GNB_BS)]
        [CustomComboInfo("血壤替换爆发击特性", "当你没有晶壤可用时使用血壤替换爆发击。", GNB.JobID, 0, "", "")]
        GNB_BS_Bloodfest = 7402,

        [ParentCombo(GNB_BS)]
        [CustomComboInfo("爆发击整合 设置", "无情状态下且晶壤大于2时，用倍攻代替爆发击", GNB.JobID, 0, "", "")]
        GNB_BS_DoubleDown = 7403,

        [ParentCombo(GNB_BS)]
        [CustomComboInfo("狮心连 设置", "存在师心连预备的时候用师心连代替爆发击", GNB.JobID, 0, "", "")]
        GNB_BS_ReignOfBeasts = 7406,

        #endregion


        [CustomComboInfo("极光保护机制", "自身身上有极光，将极光变为原初的勇猛", GNB.JobID, 0, "", "")]
        GNB_AuroraProtection = 7600,

        [Variant]
        [VariantParent(GNB_ST_MainCombo, GNB_AoE_MainCombo)]
        [CustomComboInfo("精神镖选项", "当没有dot或dot剩余时间少于3s时，使用多变精神镖", GNB.JobID)]
        GNB_Variant_SpiritDart = 7033,

        [Variant]
        [VariantParent(GNB_ST_MainCombo, GNB_AoE_MainCombo)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", GNB.JobID)]
        GNB_Variant_Cure = 7034,

        [Variant]
        [VariantParent(GNB_ST_MainCombo, GNB_AoE_MainCombo)]
        [CustomComboInfo("最后通牒 选项", "冷却结束时使用多变最后通牒", GNB.JobID)]
        GNB_Variant_Ultimatum = 7035,

        // Last value = 7600

        #endregion

        #region MACHINIST

        #region 自定义循环

        [ReplaceSkill(MCH.SlugShot)]
        // [ConflictingCombos(MCH_ST_AdvancedMode)]
        [CustomComboInfo("自定义循环", "自定义循环", MCH.JobID)]
        MCH_ST_CustomMode = 81001,

        #endregion

        
        #region Advanced ST

        [ReplaceSkill(MCH.SplitShot, MCH.HeatedSplitShot)]
        [CustomComboInfo
        ("Advanced Mode - Single Target", "Replaces Split Shot with a one-button full single target rotation.\nThese features are ideal if you want to customize the rotation.", MCH.JobID)]
        MCH_ST_AdvancedMode = 8100,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [ConflictingCombos(MCH_GaussRoundRicochet, MCH_Heatblast_GaussRound)]
        [CustomComboInfo("Level 100 Opener Option", "Uses the Balance opener.", MCH.JobID)]
        MCH_ST_Adv_Opener = 8101,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Hot Shot / Air Anchor Option", "Adds Hot Shot/Air Anchor to the rotation.", MCH.JobID)]
        MCH_ST_Adv_AirAnchor = 8102,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Reassemble Option", "Adds Reassemble to the rotation.", MCH.JobID)]
        MCH_ST_Adv_Reassemble = 8103,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [ConflictingCombos(MCH_GaussRoundRicochet, MCH_Heatblast_GaussRound)]
        [CustomComboInfo
        (
            "Gauss Round / Ricochet \nDouble Check / Checkmate option",
            "Adds Gauss Round and Ricochet or Double Check and Checkmate to the rotation. Will prevent overcapping.", MCH.JobID
        )]
        MCH_ST_Adv_GaussRicochet = 8104,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Hypercharge Option", "Adds Hypercharge to the rotation.", MCH.JobID)]
        MCH_ST_Adv_Hypercharge = 8105,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Heat Blast / Blazing Shot Option", "Adds Heat Blast or Blazing Shot to the rotation", MCH.JobID)]
        MCH_ST_Adv_Heatblast = 8106,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Rook Autoturret/Automaton Queen Option", "Adds Rook Autoturret or Automaton Queen to the rotation.", MCH.JobID)]
        MCH_Adv_TurretQueen = 8107,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Wildfire Option", "Adds Wildfire to the rotation.", MCH.JobID)]
        MCH_ST_Adv_WildFire = 8108,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Drill Option", "Adds Drill to the rotation.", MCH.JobID)]
        MCH_ST_Adv_Drill = 8109,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Barrel Stabilizer Option", "Adds Barrel Stabilizer to the rotation.", MCH.JobID)]
        MCH_ST_Adv_Stabilizer = 8110,

        [ParentCombo(MCH_ST_Adv_Stabilizer)]
        [CustomComboInfo("Full Metal Field Option", "Adds Full Metal Field to the rotation.", MCH.JobID)]
        MCH_ST_Adv_Stabilizer_FullMetalField = 8111,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Chain Saw Option", "Adds Chain Saw to the rotation.", MCH.JobID)]
        MCH_ST_Adv_Chainsaw = 8112,

        [ParentCombo(MCH_ST_Adv_Chainsaw)]
        [CustomComboInfo("Excavator Option", "Adds Excavator to the rotation.", MCH.JobID)]
        MCH_ST_Adv_Excavator = 8116,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Rook / Queen Overdrive Option", "Adds Rook or Queen Overdrive to the rotation.", MCH.JobID)]
        MCH_ST_Adv_QueenOverdrive = 8115,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Head Graze Option", "Uses Head Graze to interrupt during the rotation, where applicable.", MCH.JobID)]
        MCH_ST_Adv_Interrupt = 8113,

        [ParentCombo(MCH_ST_AdvancedMode)]
        [CustomComboInfo("Second Wind Option", "Use Second Wind when below the set HP percentage.", MCH.JobID)]
        MCH_ST_Adv_SecondWind = 8114,

        #endregion
        

        #region Advanced AoE

        [ReplaceSkill(MCH.SpreadShot, MCH.Scattergun)]
        [CustomComboInfo
        ("Advanced Mode - AoE", "Replaces Spread Shot with a one-button full single target rotation.\nThese features are ideal if you want to customize the rotation.", MCH.JobID)]
        MCH_AoE_AdvancedMode = 8300,

        [ParentCombo(MCH_AoE_AdvancedMode)]
        [CustomComboInfo("Reassemble Option", "Adds Reassemble to the rotation.", MCH.JobID)]
        MCH_AoE_Adv_Reassemble = 8301,

        [ParentCombo(MCH_AoE_AdvancedMode)]
        [ConflictingCombos(MCH_GaussRoundRicochet, MCH_Heatblast_GaussRound)]
        [CustomComboInfo("Gauss Round / Ricochet \nDouble Check / Checkmate option", "Adds Gauss Round and Ricochet or Double Check and Checkmate to the rotation.", MCH.JobID)]
        MCH_AoE_Adv_GaussRicochet = 8302,

        [ParentCombo(MCH_AoE_AdvancedMode)]
        [CustomComboInfo("Hypercharge Option", "Adds Hypercharge to the rotation.", MCH.JobID)]
        MCH_AoE_Adv_Hypercharge = 8303,

        [ParentCombo(MCH_AoE_AdvancedMode)]
        [CustomComboInfo("Rook Autoturret/Automaton Queen Option", "Adds Rook Autoturret or Automaton Queen to the rotation.", MCH.JobID)]
        MCH_AoE_Adv_Queen = 8304,

        [ParentCombo(MCH_AoE_AdvancedMode)]
        [CustomComboInfo("Flamethrower Option", "Adds Flamethrower to the rotation.", MCH.JobID)]
        MCH_AoE_Adv_FlameThrower = 8305,

        [ParentCombo(MCH_AoE_AdvancedMode)]
        [CustomComboInfo("Bioblaster Option", "Adds Bioblaster to the rotation.", MCH.JobID)]
        MCH_AoE_Adv_Bioblaster = 8306,

        [ParentCombo(MCH_AoE_AdvancedMode)]
        [CustomComboInfo("Barrel Stabilizer Option", "Adds Barrel Stabilizer to the rotation.", MCH.JobID)]
        MCH_AoE_Adv_Stabilizer = 8307,

        [ParentCombo(MCH_AoE_Adv_Stabilizer)]
        [CustomComboInfo("Full Metal Field Option", "Adds Full Metal Field to the rotation.", MCH.JobID)]
        MCH_AoE_Adv_Stabilizer_FullMetalField = 8308,

        [ParentCombo(MCH_AoE_AdvancedMode)]
        [CustomComboInfo("Chain Saw Option", "Adds Chain Saw to the the rotation.", MCH.JobID)]
        MCH_AoE_Adv_Chainsaw = 8309,

        [ParentCombo(MCH_AoE_Adv_Chainsaw)]
        [CustomComboInfo("Excavator Option", "Adds Excavator to the rotation.", MCH.JobID)]
        MCH_AoE_Adv_Excavator = 8310,

        [ParentCombo(MCH_AoE_AdvancedMode)]
        [CustomComboInfo("Second Wind Option", "Use Second Wind when below the set HP percentage.", MCH.JobID)]
        MCH_AoE_Adv_SecondWind = 8399,

        #endregion

        #region Variant

        [Variant]
        [VariantParent(MCH_ST_AdvancedMode, MCH_AoE_AdvancedMode)]
        [CustomComboInfo("Rampart Option", "Use Variant Rampart on cooldown.", MCH.JobID)]
        MCH_Variant_Rampart = 8039,

        [Variant]
        [VariantParent(MCH_ST_AdvancedMode, MCH_AoE_AdvancedMode)]
        [CustomComboInfo("Cure Option", "Use Variant Cure when HP is below set threshold.", MCH.JobID)]
        MCH_Variant_Cure = 8040,

        #endregion

        [ReplaceSkill(MCH.RookAutoturret, MCH.AutomatonQueen)]
        [CustomComboInfo("Overdrive Feature", "Replace Rook Autoturret and Automaton Queen with Overdrive while active.", MCH.JobID)]
        MCH_Overdrive = 8002,

        [ReplaceSkill(MCH.GaussRound, MCH.Ricochet)]
        [ConflictingCombos(MCH_ST_Adv_Opener, MCH_ST_Adv_GaussRicochet, MCH_AoE_Adv_GaussRicochet, MCH_Heatblast_GaussRound)]
        [CustomComboInfo
        (
            "Gauss Round / Ricochet \nDouble Check / Checkmate Feature",
            "Replace Gauss Round and Ricochet or Double Check and Checkmate with one or the other depending on which has more charges.", MCH.JobID
        )]
        MCH_GaussRoundRicochet = 8003,

        // [ReplaceSkill(MCH.Drill, MCH.AirAnchor, MCH.HotShot)]
        // [CustomComboInfo("Drill/Air Anchor (Hot Shot) Feature",
        //     "Replace Drill and Air Anchor (Hot Shot) with one or the other (or Chain Saw) depending on which is on cooldown.", MCH.JobID)]
        // MCH_HotShotDrillChainsaw = 8004,

        [ReplaceSkill(MCH.钻头Drill, MCH.AirAnchor, MCH.HotShot, MCH.ChainSaw)]
        [CustomComboInfo("Big Hitter Feature", "Replace Hot Shot, Drill, Air Anchor, Chainsaw and Excavator depending on which is on cooldown.", MCH.JobID)]
        MCH_HotShotDrillChainsawExcavator = 8004,

        [ReplaceSkill(MCH.热冲击HeatBlast)]
        [CustomComboInfo("Single Button Heat Blast Feature", "Turns Heat Blast into Hypercharge when at or above 50 heat.", MCH.JobID)]
        MCH_Heatblast = 8006,

        [ParentCombo(MCH_Heatblast)]
        [CustomComboInfo("Barrel Option", "Adds Barrel Stabilizer to the feature when below 50 Heat Gauge.", MCH.JobID)]
        MCH_Heatblast_AutoBarrel = 8052,

        [ParentCombo(MCH_Heatblast)]
        [CustomComboInfo("Wildfire Option", "Adds Wildfire to the feature when at or above 50 heat.", MCH.JobID)]
        MCH_Heatblast_Wildfire = 8015,

        [ParentCombo(MCH_Heatblast)]
        [ConflictingCombos(MCH_ST_Adv_Opener, MCH_ST_Adv_GaussRicochet, MCH_AoE_Adv_GaussRicochet, MCH_GaussRoundRicochet)]
        [CustomComboInfo
        (
            "Gauss Round / Ricochet \nDouble Check / Checkmate Option",
            "Switches between Heat Blast and either Gauss Round and Ricochet or Double Check and Checkmate, depending on cooldown timers.",
            MCH.JobID
        )]
        MCH_Heatblast_GaussRound = 8016,

        [ReplaceSkill(MCH.AutoCrossbow)]
        [CustomComboInfo("Single Button Auto Crossbow Feature", "Turns Auto Crossbow into Hypercharge when at or above 50 heat.", MCH.JobID)]
        MCH_AutoCrossbow = 8018,

        [ParentCombo(MCH_AutoCrossbow)]
        [CustomComboInfo("Barrel Option", "Adds Barrel Stabilizer to the feature when below 50 Heat Gauge.", MCH.JobID)]
        MCH_AutoCrossbow_AutoBarrel = 8019,

        [ParentCombo(MCH_AutoCrossbow)]
        [CustomComboInfo
        (
            "Gauss Round / Ricochet\n Double Check / Checkmate Option",
            "Switches between Auto Crossbow and either Gauss Round and Ricochet or Double Check and Checkmate, depending on cooldown timers.",
            MCH.JobID
        )]
        MCH_AutoCrossbow_GaussRound = 8020,

        [ReplaceSkill(MCH.Dismantle)]
        [CustomComboInfo
        (
            "Physical Ranged DPS: Double Dismantle Protection", "Prevents the use of Dismantle when target already has the effect.",
            MCH.JobID
        )]
        All_PRanged_Dismantle = 8042,

        [ReplaceSkill(MCH.Dismantle)]
        [CustomComboInfo("Dismantle - Tactician", "Swap dismantle with tactician when dismantle is on cooldown.", MCH.JobID)]
        MCH_DismantleTactician = 8058,

        // Last value = 8058

        #endregion


        #region MONK

        [ReplaceSkill(MNK.ArmOfTheDestroyer)]
        [CustomComboInfo("破坏神冲 Combo", "将破坏神冲替换为其 Combo 链", MNK.JobID, 0, "", "")]
        MNK_AoE_SimpleMode = 9000,

        [ReplaceSkill(MNK.DragonKick)]
        [CustomComboInfo("双龙脚 --> 连击 功能", "在拥有连击效果提高时使用连击替换双龙脚。", MNK.JobID, 0, "", "")]
        MNK_DragonKick_Bootshine = 9001,

        [ReplaceSkill(MNK.TrueStrike)]
        [CustomComboInfo("双掌打特性", "如果你功力buff不足六秒，使用双掌打替换正拳。", MNK.JobID, 0, "", "")]
        MNK_TwinSnakes = 9011,

        [ReplaceSkill(MNK.Bootshine)]
        [ConflictingCombos(MNK_ST_SimpleMode)]
        [CustomComboInfo("基础循环", "整合一键基础循环", MNK.JobID, 0, "", "")]
        MNK_BasicCombo = 9002,

        [ReplaceSkill(MNK.PerfectBalance)]
        [CustomComboInfo("震脚特性", "如果你拥有三档脉轮，那么使用必杀技替换震脚。", MNK.JobID, 0, "", "")]
        MNK_PerfectBalance = 9003,

        [ReplaceSkill(MNK.DragonKick)]
        [CustomComboInfo("连击平衡特性", "如果你拥有三档脉轮，那么使用必杀技替换双龙脚。", MNK.JobID, 0, "", "")]
        MNK_BootshineBalance = 9004,

        [ReplaceSkill(MNK.HowlingFist, MNK.Enlightenment)]
        [CustomComboInfo("空鸣拳/万象斗气圈特性", "当你的斗气满时使用空鸣拳/万象斗气圈替换掉破坏神冲", MNK.JobID, 0, "", "")]
        MNK_HowlingFistMeditation = 9005,

        [ReplaceSkill(MNK.Bootshine)]
        [ConflictingCombos(MNK_BasicCombo)]
        [CustomComboInfo("连击连击", "将连击替换为其 Combo 链。\n如果启用所有子选项，将变成武僧的一键循环。滑块的值可以用来控制功力 buff 和破碎拳 dot 的正常运行时间。", MNK.JobID, -2, "", "")]
        MNK_ST_SimpleMode = 9006,

        [ReplaceSkill(MNK.MasterfulBlitz)]
        [CustomComboInfo("震脚特性+", "当震脚激活时，使用可用的技能替换必杀技。", MNK.JobID, 0, "", "")]
        MNK_PerfectBalance_Plus = 9007,

        [ParentCombo(MNK_ST_SimpleMode)]
        [CustomComboInfo("必杀技加入循环", "将必杀技加入循环", MNK.JobID, 0, "", "")]
        MNK_ST_Simple_MasterfulBlitz = 9008,

        [ParentCombo(MNK_AoE_SimpleMode)]
        [CustomComboInfo("必杀技加入AOE循环", "将必杀技添加进AoE循环。", MNK.JobID, 0, "", "")]
        MNK_AoE_Simple_MasterfulBlitz = 9009,

        [ReplaceSkill(MNK.RiddleOfFire)]
        [CustomComboInfo("红莲极意/义结金兰特性", "如果红莲极意进入冷却，那么使用义结金兰替换红莲极意。", MNK.JobID, 0, "", "")]
        MNK_Riddle_Brotherhood = 9012,

        [ParentCombo(MNK_ST_SimpleMode)]
        [CustomComboInfo("主连击CD整合", "当红莲极意进入冷却时，将各种能力技整合进连击。", MNK.JobID, 0, "", "")]
        MNK_ST_Simple_CDs = 9013,

        [ParentCombo(MNK_ST_Simple_CDs)]
        [CustomComboInfo("疾风极意整合", "将疾风极意整合进连击。", MNK.JobID, 0, "", "")]
        MNK_ST_Simple_CDs_RiddleOfWind = 9014,

        [ParentCombo(MNK_ST_Simple_CDs)]
        [CustomComboInfo("震脚整合", "将震脚整合进连击。", MNK.JobID, 0, "", "")]
        MNK_ST_Simple_CDs_PerfectBalance = 9015,

        [ParentCombo(MNK_ST_Simple_CDs)]
        [CustomComboInfo("义结金兰整合", "将义结金兰整合进连击。", MNK.JobID, 0, "", "")]
        MNK_ST_Simple_CDs_Brotherhood = 9016,

        [ParentCombo(MNK_ST_SimpleMode)]
        [CustomComboInfo("斗气整合", "将 斗气 消耗添加进主循环中", MNK.JobID, 0, "", "")]
        MNK_ST_Simple_Meditation = 9017,

        [ParentCombo(MNK_ST_SimpleMode)]
        [CustomComboInfo
        (
            "阴阳开场",
            "Start with the Lunar Solar Opener on the main combo. Requires level 68 for Riddle of Fire.\nA 1.93/1.94 GCD is highly recommended.",
            MNK.JobID, 0, "", ""
        )]
        MNK_ST_Simple_LunarSolarOpener = 9018,

        [ParentCombo(MNK_AoE_SimpleMode)]
        [CustomComboInfo("AoE 循环中的能力技使用", "AoE循环下，在红莲极意中插入各种能力技。", MNK.JobID, 0, "", "")]
        MNK_AoE_Simple_CDs = 9019,

        [ParentCombo(MNK_AoE_Simple_CDs)]
        [CustomComboInfo("疾风极意应用于 AoE 循环", "将疾风极意添加进一键 AoE 循环。", MNK.JobID, 0, "", "")]
        MNK_AoE_Simple_CDs_RiddleOfWind = 9020,

        [ParentCombo(MNK_AoE_Simple_CDs)]
        [CustomComboInfo("震脚应用于 AoE 循环", "将震脚添加进一键 AoE 循环", MNK.JobID, 0, "", "")]
        MNK_AoE_Simple_CDs_PerfectBalance = 9021,

        [ParentCombo(MNK_AoE_Simple_CDs)]
        [CustomComboInfo("义结金兰应用于 AoE 循环", "将义结金兰添加进一键 AoE 循环。", MNK.JobID, 0, "", "")]
        MNK_AoE_Simple_CDs_Brotherhood = 9022,

        [ParentCombo(MNK_AoE_SimpleMode)]
        [CustomComboInfo("斗气应用于 AoE 循环", "将斗气添加进一键 AoE 循环。", MNK.JobID, 0, "", "")]
        MNK_AoE_Simple_Meditation = 9023,

        [ParentCombo(MNK_AoE_SimpleMode)]
        [CustomComboInfo("轻身步法应用于 AoE 循环", "将轻身步法添加进一键 AoE 循环", MNK.JobID, 0, "", "")]
        MNK_AoE_Simple_Thunderclap = 9024,

        [ParentCombo(MNK_ST_SimpleMode)]
        [CustomComboInfo("轻身步法应用于单体循环", "将轻身步法添加进一键单体循环。", MNK.JobID, 0, "", "")]
        MNK_ST_Simple_Thunderclap = 9025,

        [ParentCombo(MNK_ST_SimpleMode)]
        [CustomComboInfo
        (
            "回复设置", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", MNK.JobID, 0, "",
            ""
        )]
        MNK_ST_ComboHeals = 9026,

        [ParentCombo(MNK_AoE_SimpleMode)]
        [CustomComboInfo
        (
            "回复设置", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", MNK.JobID, 0, "",
            ""
        )]
        MNK_AoE_ComboHeals = 9027,

        [ParentCombo(MNK_ST_Simple_Meditation)]
        [CustomComboInfo("斗气 选项", "当你超出攻击范围且不在起手/爆发期时，将循环替换为 斗气", MNK.JobID, 0, "", "")]
        MNK_ST_Meditation_Uptime = 9028,

        [ParentCombo(MNK_ST_SimpleMode)]
        [CustomComboInfo("动态真北选项", "当你不在触发增益状态的正确身位时，在触发增益状态前将 真北 加入到主循环中", MNK.JobID, 0, "", "")]
        MNK_TrueNorthDynamic = 9029,

        [Variant]
        [VariantParent(MNK_ST_SimpleMode, MNK_AoE_SimpleMode)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", MNK.JobID)]
        MNK_Variant_Cure = 9030,

        [Variant]
        [VariantParent(MNK_ST_SimpleMode, MNK_AoE_SimpleMode)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", MNK.JobID)]
        MNK_Variant_Rampart = 9031,

        // Last value = 9031

        #endregion

        #region NINJA

        [ReplaceSkill(NIN.SpinningEdge)]
        [ConflictingCombos(NIN_ArmorCrushCombo, NIN_ST_AdvancedMode, NIN_KassatsuChiJin, NIN_KassatsuTrick)]
        [CustomComboInfo("下忍模式 - 单目标", "将双刃旋替换为一键单目标连击。\n一键输出，下忍的理想之选。", NIN.JobID)]
        NIN_ST_SimpleMode = 10000,

        [ParentCombo(NIN_ST_SimpleMode)]
        [CustomComboInfo
        (
            "平衡起手",
            "Starts with the Balance opener.\nDoes pre-pull first, if you enter combat before hiding the opener will fail.\nLikewise, moving during TCJ will cause the opener to fail too.\nRequires you to be out of combat with majority of your cooldowns available for it to work.",
            NIN.JobID
        )]
        NIN_ST_SimpleMode_BalanceOpener = 10001,

        [ReplaceSkill(NIN.DeathBlossom)]
        [ConflictingCombos(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("下忍模式 - AoE", "将血雨飞花替换为一键AOE连击", NIN.JobID)]
        NIN_AoE_SimpleMode = 10002,

        [ReplaceSkill(NIN.SpinningEdge)]
        [ConflictingCombos(NIN_ST_SimpleMode)]
        [CustomComboInfo("上忍模式 - 单目标", "将双刃旋替换为一键单目标连击。\n自定义循环，上忍的理想之选。", NIN.JobID)]
        NIN_ST_AdvancedMode = 10003,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("飞刀 选项", "如果在近战攻击距离以外，将飞刀加入循环。", NIN.JobID)]
        NIN_ST_AdvancedMode_RangedUptime = 10004,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("夺取", "将 夺取 加入上忍模式循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Mug = 10005,

        [ConflictingCombos(NIN_ST_AdvancedMode_Mug_AlignBefore)]
        [ParentCombo(NIN_ST_AdvancedMode_Mug)]
        [CustomComboInfo("对齐 夺取 和 攻其不备", "Only uses Mug whilst the target has Trick Attack, otherwise will use on cooldown.", NIN.JobID)]
        NIN_ST_AdvancedMode_Mug_AlignAfter = 10006,

        [ConflictingCombos(NIN_ST_AdvancedMode_Mug_AlignAfter)]
        [ParentCombo(NIN_ST_AdvancedMode_Mug)]
        [CustomComboInfo("在 攻其不备 前使用 夺取", "对齐 夺取 与 攻其不备，但 夺取 至少早 攻其不备 1个GCD", NIN.JobID)]
        NIN_ST_AdvancedMode_Mug_AlignBefore = 10007,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("攻其不备 选项", "将 攻其不备 加入一键循环", NIN.JobID)] //Has Config
        NIN_ST_AdvancedMode_TrickAttack = 10008,

        [ParentCombo(NIN_ST_AdvancedMode_TrickAttack)]
        [CustomComboInfo("在 攻其不备 前保留CD", "在 攻其不备 冷却结束前，停止使用CD超过15s的能力技", NIN.JobID)] //HasConfig
        NIN_ST_AdvancedMode_TrickAttack_Cooldowns = 10009,

        [ParentCombo(NIN_ST_AdvancedMode_TrickAttack)]
        [CustomComboInfo("延后 攻其不备", "进入战斗至少8s后再使用 攻其不备", NIN.JobID)]
        NIN_ST_AdvancedMode_TrickAttack_Delayed = 10010,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("忍术 选项", "将 忍术 加入上忍循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Ninjitsus = 10011,

        [ParentCombo(NIN_ST_AdvancedMode_Ninjitsus)]
        [CustomComboInfo("保留1层充能", "防止一次性使用两个忍术", NIN.JobID)]
        NIN_ST_AdvancedMode_Ninjitsus_ChargeHold = 10012,

        [ParentCombo(NIN_ST_AdvancedMode_Ninjitsus)]
        [CustomComboInfo("使用 风魔手里剑", "使用忍术释放 风魔手里剑（仅在雷遁获取前生效）", NIN.JobID)]
        NIN_ST_AdvancedMode_Ninjitsus_FumaShuriken = 10013,

        [ParentCombo(NIN_ST_AdvancedMode_Ninjitsus)]
        [CustomComboInfo("使用 雷遁", "使用忍术释放 雷遁", NIN.JobID)]
        NIN_ST_AdvancedMode_Ninjitsus_Raiton = 10014,

        [ParentCombo(NIN_ST_AdvancedMode_Ninjitsus)]
        [CustomComboInfo("使用 水遁", "使用忍术释放 水遁", NIN.JobID)]
        NIN_ST_AdvancedMode_Ninjitsus_Suiton = 10015,

        [ParentCombo(NIN_ST_AdvancedMode_Ninjitsus)]
        [CustomComboInfo("使用 风遁", "使用忍术释放 风遁", NIN.JobID)]
        NIN_ST_AdvancedMode_Ninjitsus_Huton = 10016,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("断绝/梦幻三段", "将 断绝/梦幻三段 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_AssassinateDWAD = 10017,

        [ConflictingCombos(NIN_KassatsuTrick, NIN_KassatsuChiJin)]
        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("生杀予夺 选项", "将 生杀予夺 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Kassatsu = 10018,

        [ParentCombo(NIN_ST_AdvancedMode_Kassatsu)]
        [CustomComboInfo($"使用 冰晶乱流之术", "使用 生杀予夺 释放 冰晶乱流之术", NIN.JobID)]
        NIN_ST_AdvancedMode_Kassatsu_HyoshoRaynryu = 10019,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("强甲破点突 选项", "将 强甲破点突 加入一键循环", NIN.JobID)] //Has Config
        NIN_ST_AdvancedMode_ArmorCrush = 10020,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("风来刃 选项", "将 风来刃 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Huraijin = 10021,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("六道轮回 选项", "将 六道轮回 加入一键循环", NIN.JobID)] //Has Config
        NIN_ST_AdvancedMode_Bhavacakra = 10022,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("天地人 选项", "将 天地人 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_TCJ = 10023,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("命水 选项", "将 命水 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Meisui = 10024,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("分身之术 选项", "将 分身之术 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Bunshin = 10025,

        [ParentCombo(NIN_ST_AdvancedMode_Bunshin)]
        [CustomComboInfo("残影镰鼬 选项", "将 残影镰鼬 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Bunshin_Phantom = 10026,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("月影雷兽爪/月影雷兽牙 选项", "将 月影雷兽爪/月影雷兽牙 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Raiju = 10027,

        [ParentCombo(NIN_ST_AdvancedMode_Raiju)]
        [CustomComboInfo("雷兽爪突进选项", "当处于攻击范围外时，使用 月影雷兽爪", NIN.JobID)]
        NIN_ST_AdvancedMode_Raiju_Forked = 10028,

        [ConflictingCombos(NIN_KassatsuChiJin, NIN_KassatsuTrick)]
        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo
        (
            "平衡起手",
            "Starts with the Balance opener.\nDoes pre-pull first, if you enter combat before hiding the opener will fail.\nLikewise, moving during TCJ will cause the opener to fail too.\nRequires you to be out of combat with majority of your cooldowns available for it to work.",
            NIN.JobID
        )]
        NIN_ST_AdvancedMode_BalanceOpener = 10029,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("真北 选项", "将 真北 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_TrueNorth = 10030,

        [ParentCombo(NIN_ST_AdvancedMode_TrueNorth)]
        [CustomComboInfo("仅在 强甲破点突 前使用", "仅在 强甲破点突 前使用 真北", NIN.JobID)]
        NIN_ST_AdvancedMode_TrueNorth_ArmorCrush = 10031,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("内丹选项", "将 内丹 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_SecondWind = 10032,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("残影 选项", "将 残影 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_ShadeShift = 10033,

        [ParentCombo(NIN_ST_AdvancedMode)]
        [CustomComboInfo("浴血 选项", "将 浴血 加入一键循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Bloodbath = 10034,

        [ReplaceSkill(NIN.DeathBlossom)]
        [ConflictingCombos(NIN_AoE_SimpleMode)]
        [CustomComboInfo("上忍模式 - AoE", "将血雨飞花替换为一键群体连击。\n自定义循环，上忍的理想之选。", NIN.JobID)]
        NIN_AoE_AdvancedMode = 10035,

        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("断绝/梦幻三段", "将 断绝/梦幻三段 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_AssassinateDWAD = 10036,

        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("忍术 选项", "将 忍术 加入上忍循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_Ninjitsus = 10037,

        [ParentCombo(NIN_AoE_AdvancedMode_Ninjitsus)]
        [CustomComboInfo("保留1层充能", "防止一次性使用两个忍术", NIN.JobID)]
        NIN_AoE_AdvancedMode_Ninjitsus_ChargeHold = 10038,

        [ParentCombo(NIN_AoE_AdvancedMode_Ninjitsus)]
        [CustomComboInfo("使用 火遁", "使用忍术释放 火遁", NIN.JobID)]
        NIN_AoE_AdvancedMode_Ninjitsus_Katon = 10039,

        [ParentCombo(NIN_AoE_AdvancedMode_Ninjitsus)]
        [CustomComboInfo("使用 土遁", "使用忍术释放 土遁", NIN.JobID)]
        NIN_AoE_AdvancedMode_Ninjitsus_Doton = 10040,

        [ParentCombo(NIN_AoE_AdvancedMode_Ninjitsus)]
        [CustomComboInfo("使用 风遁", "使用忍术释放 风遁", NIN.JobID)]
        NIN_AoE_AdvancedMode_Ninjitsus_Huton = 10041,

        [ConflictingCombos(NIN_KassatsuTrick, NIN_KassatsuChiJin)]
        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("生杀予夺 选项", "将 生杀予夺 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_Kassatsu = 10042,

        [ParentCombo(NIN_AoE_AdvancedMode_Kassatsu)]
        [CustomComboInfo("劫火灭却 选项", "将 劫火灭却之术 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_GokaMekkyaku = 10043,

        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("风来刃 选项", "将 风来刃 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_Huraijin = 10044,

        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("通灵之术·大虾蟆 选项", "将 通灵之术·大虾蟆 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_HellfrogMedium = 10045,

        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("天地人 选项", "将 天地人 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_TCJ = 10046,

        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("命水 选项", "将 命水 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_Meisui = 10047,

        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("分身之术 选项", "将 分身之术 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_Bunshin = 10048,

        [ParentCombo(NIN_AoE_AdvancedMode_Bunshin)]
        [CustomComboInfo("残影镰鼬 选项", "将 残影镰鼬 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_Bunshin_Phantom = 10049,

        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("内丹选项", "将 内丹 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_SecondWind = 10050,

        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("残影 选项", "将 残影 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_ShadeShift = 10051,

        [ParentCombo(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("浴血 选项", "将 浴血 加入一键循环", NIN.JobID)]
        NIN_AoE_AdvancedMode_Bloodbath = 10052,

        [ReplaceSkill(NIN.ArmorCrush)]
        [ConflictingCombos(NIN_ST_SimpleMode)]
        [CustomComboInfo("强甲破点突连击 选项", "使用 强甲破点突 做为连击的起始技.", NIN.JobID)]
        NIN_ArmorCrushCombo = 10053,

        [ConflictingCombos
        (
            NIN_ST_AdvancedMode_BalanceOpener, NIN_ST_AdvancedMode_BalanceOpener, NIN_ST_AdvancedMode_Kassatsu,
            NIN_AoE_AdvancedMode_Kassatsu, NIN_KassatsuChiJin
        )]
        [ReplaceSkill(NIN.Kassatsu)]
        [CustomComboInfo("攻其不备 替换 生杀予夺", "隐遁状态下或发动水遁之术后，使用 攻其不备 替换 生杀予夺.\n推荐同时使用冷却CD监视插件.", NIN.JobID)]
        NIN_KassatsuTrick = 10054,

        [ReplaceSkill(NIN.TenChiJin)]
        [CustomComboInfo("命水 替换 天地人", "发动水遁之术 后，使用 命水 替换 天地人.\n推荐同时使用冷却CD监视插件.", NIN.JobID)]
        NIN_TCJMeisui = 10055,

        [ConflictingCombos
        (
            NIN_ST_AdvancedMode_BalanceOpener, NIN_ST_AdvancedMode_BalanceOpener, NIN_KassatsuTrick, NIN_ST_AdvancedMode_Kassatsu,
            NIN_AoE_AdvancedMode_Kassatsu
        )]
        [ReplaceSkill(NIN.Chi)]
        [CustomComboInfo("生杀予夺 地之印/人之印 开关", "发动 生杀予夺 后，使用 人之印 替换 地之印.", NIN.JobID)]
        NIN_KassatsuChiJin = 10056,

        [ReplaceSkill(NIN.Hide)]
        [CustomComboInfo("隐遁 替换 夺取/攻其不备", "战斗状态下用 夺取 替换 隐遁，隐遁状态下用 攻其不备 替换 隐遁", NIN.JobID)]
        NIN_HideMug = 10057,

        [ReplaceSkill(NIN.AeolianEdge)]
        [CustomComboInfo("旋风刃 替换为 忍术", "此功能效果无效：当使用任意结印时，将 旋风刃 Combo 替换为 忍术。", NIN.JobID)]
        NIN_AeolianNinjutsu = 10058,

        [ReplaceSkill(NIN.Huraijin)]
        [CustomComboInfo("风来刃 替换为 雷兽", "当 月影雷兽爪 和 月影雷兽牙 可以使用时，替换 风来刃.", NIN.JobID)]
        NIN_HuraijinRaiju = 10059,

        [ParentCombo(NIN_HuraijinRaiju)]
        [CustomComboInfo("风来刃 替换为 月影雷兽牙", "当 月影雷兽牙 可以使用时，替换 风来刃.", NIN.JobID)]
        NIN_HuraijinRaiju_Fleeting = 10060,

        [ParentCombo(NIN_HuraijinRaiju)]
        [CustomComboInfo("风来刃 替换为 月影雷兽爪", "当 月影雷兽爪 可以使用时，替换 风来刃.", NIN.JobID)]
        NIN_HuraijinRaiju_Forked = 10061,

        [ReplaceSkill(NIN.Ten, NIN.Chi, NIN.Jin)]
        [CustomComboInfo("简化忍术", "简化忍术结印的操作.", NIN.JobID)]
        NIN_Simple_Mudras = 10062,

        [ReplaceSkill(NIN.TenChiJin)]
        [ParentCombo(NIN_TCJMeisui)]
        [CustomComboInfo("天地人 开关", "Turns Ten Chi Jin (the move) into Ten, Chi, and Jin.", NIN.JobID)]
        NIN_TCJ = 10063,

        [ReplaceSkill(NIN.Huraijin)]
        [CustomComboInfo("风来刃/强点破甲突", "使用 绝风 后，用 强甲破点突 替换 风来刃", NIN.JobID)]
        NIN_HuraijinArmorCrush = 10064,

        [ParentCombo(NIN_ST_AdvancedMode_Ninjitsus_Raiton)]
        [CustomComboInfo("雷遁 选项", "将 雷遁 加入到循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Raiton_Uptime = 10065,

        [ParentCombo(NIN_ST_AdvancedMode_Bunshin_Phantom)]
        [CustomComboInfo("残影镰鼬 选项", "将 残影镰鼬 加入到循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Phantom_Uptime = 10066,

        [ParentCombo(NIN_ST_AdvancedMode_Ninjitsus_Suiton)]
        [CustomComboInfo("水遁之术 选项", "将 水遁之术 加入到循环", NIN.JobID)]
        NIN_ST_AdvancedMode_Suiton_Uptime = 10067,

        [ParentCombo(NIN_ST_AdvancedMode_TrueNorth_ArmorCrush)]
        [CustomComboInfo("动态真北选项", "在强点破甲突前面插入一个真北，如果你不在正确的身位上.", NIN.JobID)]
        NIN_ST_AdvancedMode_TrueNorth_ArmorCrush_Dynamic = 10068,

        [Variant]
        [VariantParent(NIN_ST_SimpleMode, NIN_ST_AdvancedMode, NIN_AoE_SimpleMode, NIN_AoE_AdvancedMode)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", NIN.JobID)]
        NIN_Variant_Cure = 10069,

        [Variant]
        [VariantParent(NIN_ST_SimpleMode, NIN_ST_AdvancedMode, NIN_AoE_SimpleMode, NIN_AoE_AdvancedMode)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", NIN.JobID)]
        NIN_Variant_Rampart = 10070,

        // Last value = 10070

        #endregion

        #region PALADIN

        [ReplaceSkill(PLD.暴乱剑RiotBlade)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            PLD.JobID, -10, "", ""
        )]
        PLD_Advanced_CustomMode = 110001,


        //// Last value = 11032

        [ReplaceSkill(PLD.先锋剑FastBlade)]
        [CustomComboInfo("骑士高级模式 - 单目标[推荐]", $"自定义循环链(先锋剑)", PLD.JobID)]
        PLD_ST_AdvancedMode = 11002,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("起手设置", "设置多少GCD起手", PLD.JobID)]
        PLD_ST_AdvancedMode_Open = 110034,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("战逃反应 Option", "Adds 战逃反应 to Advanced Mode.", PLD.JobID)]
        PLD_ST_AdvancedMode_FoF = 11003,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("投盾 Option", "Adds 调停 to Advanced Mode if out of range.", PLD.JobID)]
        PLD_ST_AdvancedMode_ShieldLob = 11004,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("厄运流转选项", "添加 厄运流转 到自定义循环.", PLD.JobID)]
        PLD_ST_AdvancedMode_CircleOfScorn = 11005,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("深奥之灵 / 偿赎剑选项", "添加 深奥之灵 / 偿赎剑 到自定义循环", PLD.JobID)]
        PLD_ST_AdvancedMode_SpiritsWithin = 11006,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("盾阵 / 圣盾阵选项", "添加 盾阵 / 圣盾阵到自定义循环", PLD.JobID)]
        PLD_ST_AdvancedMode_Sheltron = 11007,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("沥血剑选项", "添加 沥血剑 到自定义循环", PLD.JobID)]
        PLD_ST_AdvancedMode_GoringBlade = 11008,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("圣灵选项", "添加 圣灵到自定义循环", PLD.JobID)]
        PLD_ST_AdvancedMode_HolySpirit = 11009,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("安魂祈祷", "添加 安魂祈祷到自定义循环", PLD.JobID)]
        PLD_ST_AdvancedMode_Requiescat = 11010,


        // [ParentCombo(PLD_ST_AdvancedMode)] [CustomComboInfo("荣誉之剑", "添加 荣誉之剑到自定义循环", PLD.JobID)]
        // PLD_ST_AdvancedMode_荣誉之剑 = 110101,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("调停选项", "添加 调停 到自定义循环", PLD.JobID)]
        PLD_ST_AdvancedMode_Intervene = 11011,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("赎罪剑 Option", "Adds 赎罪剑 to Advanced Mode", PLD.JobID)]
        PLD_ST_AdvancedMode_Atonement = 11012,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("悔罪选项", "添加 悔罪 到自定义循环", PLD.JobID)]
        PLD_ST_AdvancedMode_Confiteor = 11013,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("信仰/真理/英勇之剑选项", "添加 信仰/真理/英勇之剑 到自定义循环", PLD.JobID)]
        PLD_ST_AdvancedMode_Blades = 11014,

        [ReplaceSkill(PLD.全蚀斩TotalEclipse)]
        [CustomComboInfo("骑士高级模式 - AoE", $"自定义循环链(全蚀斩)", PLD.JobID)]
        PLD_AoE_AdvancedMode = 11015,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("战逃反应选项", "添加 战逃反应 到自定义循环.", PLD.JobID)]
        PLD_AoE_AdvancedMode_FoF = 11016,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("深奥之灵 / 偿赎剑选项", "添加 深奥之灵 / 偿赎剑到自定义循环", PLD.JobID)]
        PLD_AoE_AdvancedMode_SpiritsWithin = 11017,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("厄运流转选项", "添加 厄运流转 到自定义循环.", PLD.JobID)]
        PLD_AoE_AdvancedMode_CircleOfScorn = 11018,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("安魂祈祷选项", "添加 安魂祈祷 到自定义循环.", PLD.JobID)]
        PLD_AoE_AdvancedMode_Requiescat = 11019,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("圣环选项", "添加 圣环到自定义循环.", PLD.JobID)]
        PLD_AoE_AdvancedMode_HolyCircle = 11020,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("悔罪Option", "添加 悔罪到自定义循环", PLD.JobID)]
        PLD_AoE_AdvancedMode_Confiteor = 11021,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("信仰/真理/英勇之剑选项", "添加 信仰/真理/英勇之剑 到自定义循环", PLD.JobID)]
        PLD_AoE_AdvancedMode_Blades = 11022,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("盾阵 / 圣盾阵选项", "添加 盾阵 / 圣盾阵 到自定义循环", PLD.JobID)]
        PLD_AoE_AdvancedMode_Sheltron = 11023,


        [ReplaceSkill(PLD.安魂祈祷Requiescat)]
        [CustomComboInfo("安魂祈祷替换选项", "当安魂祈祷状态下用下面的技能替换安魂祈祷", PLD.JobID)]
        PLD_Requiescat_Options = 11024,

        [ReplaceSkill(PLD.深奥之灵SpiritsWithin, PLD.偿赎剑Expiacion)]
        [CustomComboInfo("深奥之灵/偿赎剑/厄运流转替换选项", "当厄运流转冷却用(深奥之灵/偿赎剑)代替(好像有bug).", PLD.JobID)]
        PLD_SpiritsWithin = 11025,


        [Variant]
        [VariantParent(PLD_ST_AdvancedMode, PLD_AoE_AdvancedMode)]
        [CustomComboInfo("精神镖选项", "当没有dot或dot剩余时间少于3s时，使用多变精神镖", PLD.JobID)]
        PLD_Variant_SpiritDart = 11030,

        [Variant]
        [VariantParent(PLD_ST_AdvancedMode, PLD_AoE_AdvancedMode)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", PLD.JobID)]
        PLD_Variant_Cure = 11031,

        [Variant]
        [VariantParent(PLD_ST_AdvancedMode, PLD_AoE_AdvancedMode)]
        [CustomComboInfo("最后通牒 选项", "冷却结束时使用多变最后通牒", PLD.JobID)]
        PLD_Variant_Ultimatum = 11032,

        //// Last value = 11032

        #endregion

        #region REAPER

        #region Simple ST

        [ReplaceSkill(RPR.Slice)]
        [ConflictingCombos(RPR_ST_AdvancedMode)]
        [CustomComboInfo("Simple Mode - Single Target", "Replaces Slice with a one-button full single target rotation.\nThis is ideal for newcomers to the job.", RPR.JobID)]
        RPR_ST_SimpleMode = 12000,

        #endregion

        #region Advanced ST

        [ReplaceSkill(RPR.Slice)]
        [CustomComboInfo("Advanced Mode - Single Target", "Replaces Slice with a full one-button single target rotation.\nThese features are ideal if you want to customize the rotation.", RPR.JobID)]
        RPR_ST_AdvancedMode = 12001,

        [ParentCombo(RPR_ST_AdvancedMode)]
        [CustomComboInfo("Level 100 Opener Option", "Adds the Level 100 Opener to the rotation.", RPR.JobID)]
        RPR_ST_Opener = 12002,

        [ParentCombo(RPR_ST_AdvancedMode)]
        [CustomComboInfo("Shadow Of Death Option", "Adds Shadow of Death to the rotation.", RPR.JobID)]
        RPR_ST_SoD = 12003,

        [ParentCombo(RPR_ST_AdvancedMode)]
        [CustomComboInfo("Soul Slice Option", "Adds Soul Slice to the rotation.", RPR.JobID)]
        RPR_ST_SoulSlice = 12004,

        #region Cooldowns ST

        [ParentCombo(RPR_ST_AdvancedMode)]
        [CustomComboInfo("Cooldowns Option", "Adds various cooldowns to the rotation.", RPR.JobID)]
        RPR_ST_CDs = 12005,

        [ParentCombo(RPR_ST_CDs)]
        [CustomComboInfo("Arcane Circle Option", "Adds Arcane Circle to the rotation.", RPR.JobID)]
        RPR_ST_ArcaneCircle = 12006,

        [ParentCombo(RPR_ST_ArcaneCircle)]
        [CustomComboInfo("Plentiful Harvest Option", "Adds Plentiful Harvest to the rotation.", RPR.JobID)]
        RPR_ST_PlentifulHarvest = 12007,

        [ParentCombo(RPR_ST_CDs)]
        [CustomComboInfo("Bloodstalk Option", "Adds Bloodstalk to the rotation.", RPR.JobID)]
        RPR_ST_Bloodstalk = 12008,

        [ParentCombo(RPR_ST_CDs)]
        [CustomComboInfo("Gluttony Option", "Adds Gluttony to the rotation.", RPR.JobID)]
        RPR_ST_Gluttony = 12009,

        [ParentCombo(RPR_ST_CDs)]
        [CustomComboInfo("Enshroud Option", "Adds Enshroud to the rotation.", RPR.JobID)]
        RPR_ST_Enshroud = 12010,

        [ParentCombo(RPR_ST_Enshroud)]
        [CustomComboInfo("Double Enshroud Harvest moon", "Uses Harvest Moon in 2 minute burst.", RPR.JobID)]
        RPR_ST_EnshroudHarvestMoon = 12011,

        [ParentCombo(RPR_ST_Enshroud)]
        [CustomComboInfo("Void/Cross Reaping Option", "Adds Void Reaping and Cross Reaping to the rotation.\n(Disabling this may stop the one-button combo working during enshroud)", RPR.JobID)]
        RPR_ST_Reaping = 12012,

        [ParentCombo(RPR_ST_Enshroud)]
        [CustomComboInfo("Lemure's Slice Option", "Adds Lemure's Slice to the rotation.", RPR.JobID)]
        RPR_ST_Lemure = 12013,

        [ParentCombo(RPR_ST_Enshroud)]
        [CustomComboInfo("Sacrificium Option", "Adds Sacrificium to the rotation.", RPR.JobID)]
        RPR_ST_Sacrificium = 12014,

        [ParentCombo(RPR_ST_Enshroud)]
        [CustomComboInfo("Communio Finisher Option", "Adds Communio to the rotation.", RPR.JobID)]
        RPR_ST_Communio = 12015,

        [ParentCombo(RPR_ST_CDs)]
        [CustomComboInfo("Perfectio Option", "Adds Perfectio to the rotation.", RPR.JobID)]
        RPR_ST_Perfectio = 12016,

        #endregion

        [ParentCombo(RPR_ST_AdvancedMode)]
        [CustomComboInfo("Gibbet and Gallows Option", "Adds Gibbet and Gallows to the rotation.", RPR.JobID)]
        RPR_ST_GibbetGallows = 12017,

        [ParentCombo(RPR_ST_AdvancedMode)]
        [CustomComboInfo("Ranged Filler Option", "Replaces the combo chain with Harpe when outside of melee range. Will not override Communio.", RPR.JobID)]
        RPR_ST_RangedFiller = 12018,

        [ParentCombo(RPR_ST_RangedFiller)]
        [CustomComboInfo("Add Harvest Moon", "Adds Harvest Moon if available, when outside of melee range. Will not override Communio.", RPR.JobID)]
        RPR_ST_RangedFillerHarvestMoon = 12019,

        [ParentCombo(RPR_ST_AdvancedMode)]
        [CustomComboInfo("Combo Heals Option", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", RPR.JobID)]
        RPR_ST_ComboHeals = 12097,

        [ParentCombo(RPR_ST_AdvancedMode)]
        [CustomComboInfo("Dynamic True North Feature", "Adds True North before Gibbet/Gallows when you are not in the correct position.", RPR.JobID)]
        RPR_ST_TrueNorthDynamic = 12098,

        [ParentCombo(RPR_ST_TrueNorthDynamic)]
        [CustomComboInfo("Hold True North for Gluttony Option", "Will hold the last charge of True North for use with Gluttony, even when out of position for Gibbet/Gallows.", RPR.JobID)]
        RPR_ST_TrueNorthDynamic_HoldCharge = 12099,

        //last value = 12019

        #endregion

        #region Simple AoE

        [ReplaceSkill(RPR.SpinningScythe)]
        [ConflictingCombos(RPR_AoE_AdvancedMode)]
        [CustomComboInfo("Simple Mode - AoE", "Replaces Spinning Scythe with a one-button full single target rotation.\nThis is ideal for newcomers to the job.", RPR.JobID)]
        RPR_AoE_SimpleMode = 12100,

        #endregion

        #region Advanced AoE

        [ReplaceSkill(RPR.SpinningScythe)]
        [CustomComboInfo("Advanced Mode - AoE", "Replaces Spinning Scythe with a full one-button AoE rotation.\nThese features are ideal if you want to customize the rotation.", RPR.JobID)]
        RPR_AoE_AdvancedMode = 12101,

        [ParentCombo(RPR_AoE_AdvancedMode)]
        [CustomComboInfo("Whorl Of Death Option", "Adds Whorl of Death to the rotation.", RPR.JobID)]
        RPR_AoE_WoD = 12102,

        [ParentCombo(RPR_AoE_AdvancedMode)]
        [CustomComboInfo("Soul Scythe Option", "Adds Soul Scythe to the rotation.", RPR.JobID)]
        RPR_AoE_SoulScythe = 12103,

        #region Cooldowns

        [ParentCombo(RPR_AoE_AdvancedMode)]
        [CustomComboInfo("Cooldowns Option", "Adds various cooldowns to the rotation.", RPR.JobID)]
        RPR_AoE_CDs = 12104,

        [ParentCombo(RPR_AoE_CDs)]
        [CustomComboInfo("Arcane Circle Option", "Adds Arcane Circle to the rotation.", RPR.JobID)]
        RPR_AoE_ArcaneCircle = 12105,

        [ParentCombo(RPR_AoE_ArcaneCircle)]
        [CustomComboInfo("Plentiful Harvest Option", "Adds Plentiful Harvest to the rotation.", RPR.JobID)]
        RPR_AoE_PlentifulHarvest = 12106,

        [ParentCombo(RPR_AoE_CDs)]
        [CustomComboInfo("Grim Swathe Option", "Adds Grim Swathe to the rotation.", RPR.JobID)]
        RPR_AoE_GrimSwathe = 12107,

        [ParentCombo(RPR_AoE_CDs)]
        [CustomComboInfo("Gluttony Option", "Adds Gluttony to the rotation.", RPR.JobID)]
        RPR_AoE_Gluttony = 12108,

        [ParentCombo(RPR_AoE_CDs)]
        [CustomComboInfo("Enshroud Option", "Adds Enshroud to the rotation.", RPR.JobID)]
        RPR_AoE_Enshroud = 12109,

        [ParentCombo(RPR_AoE_Enshroud)]
        [CustomComboInfo("Grim Reaping Option", "Adds Grim Reaping to the rotation.\n(Disabling this may stop the one-button combo working during enshroud)", RPR.JobID)]
        RPR_AoE_Reaping = 12110,

        [ParentCombo(RPR_AoE_Enshroud)]
        [CustomComboInfo("Lemure's Scythe Option", "Adds Lemure's Scythe to the rotation.", RPR.JobID)]
        RPR_AoE_Lemure = 12111,

        [ParentCombo(RPR_AoE_Enshroud)]
        [CustomComboInfo("Sacrificium Option", "Adds Sacrificium to the rotation.", RPR.JobID)]
        RPR_AoE_Sacrificium = 12112,

        [ParentCombo(RPR_AoE_Enshroud)]
        [CustomComboInfo("Communio Finisher Option", "Adds Communio to the rotation.", RPR.JobID)]
        RPR_AoE_Communio = 12113,

        [ParentCombo(RPR_AoE_CDs)]
        [CustomComboInfo("Perfectio Option", "Adds Perfectio to the rotation.", RPR.JobID)]
        RPR_AoE_Perfectio = 12114,

        #endregion

        [ParentCombo(RPR_AoE_AdvancedMode)]
        [CustomComboInfo("Guillotine Option", "Adds Guillotine to the rotation.", RPR.JobID)]
        RPR_AoE_Guillotine = 12115,

        [ParentCombo(RPR_AoE_AdvancedMode)]
        [CustomComboInfo("Combo Heals Option", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", RPR.JobID)]
        RPR_AoE_ComboHeals = 12116,

        // Last value = 12116

        #endregion

        #region Blood Stalk/Grim Swathe Combo Section

        [ReplaceSkill(RPR.BloodStalk, RPR.GrimSwathe)]
        [CustomComboInfo("Gluttony on Blood Stalk/Grim Swathe Feature", "Blood Stalk and Grim Swathe will turn into Gluttony when it is available.", RPR.JobID)]
        RPR_GluttonyBloodSwathe = 12200,

        [ParentCombo(RPR_GluttonyBloodSwathe)]
        [CustomComboInfo("Gibbet and Gallows/Guillotine on Blood Stalk/Grim Swathe Feature", "Adds Gibbet and Gallows on Blood Stalk.\nAdds Guillotine on Grim Swathe.", RPR.JobID)]
        RPR_GluttonyBloodSwathe_BloodSwatheCombo = 12201,

        [ParentCombo(RPR_GluttonyBloodSwathe)]
        [CustomComboInfo("Enshroud Combo Option", "Adds Enshroud combo (Void/Cross Reaping, Communio, and Lemure's Slice) on Blood Stalk and Grim Swathe.", RPR.JobID)]
        RPR_GluttonyBloodSwathe_Enshroud = 12202,

        // Last value = 12202

        #endregion

        #region Miscellaneous

        [ReplaceSkill(RPR.ArcaneCircle)]
        [CustomComboInfo("Arcane Circle Harvest Feature", "Replaces Arcane Circle with Plentiful Harvest when you have stacks of Immortal Sacrifice.", RPR.JobID)]
        RPR_ArcaneCirclePlentifulHarvest = 12300,

        [ReplaceSkill(RPR.HellsEgress, RPR.HellsIngress)]
        [CustomComboInfo("Regress Feature", "Changes both Hell's Ingress and Hell's Egress turn into Regress when Threshold is active.", RPR.JobID)]
        RPR_Regress = 12301,

        [ReplaceSkill(RPR.Slice, RPR.SpinningScythe, RPR.ShadowOfDeath, RPR.Harpe, RPR.BloodStalk)]
        [CustomComboInfo("Soulsow Reminder Feature", "Adds Soulsow to the skills selected below when out of combat. \nWill also add Soulsow to Harpe when in combat and no target is selected.", RPR.JobID)]
        RPR_Soulsow = 12302,

        [ReplaceSkill(RPR.Harpe)]
        [ParentCombo(RPR_Soulsow)]
        [CustomComboInfo("Harpe Harvest Moon Feature", "Replaces Harpe with Harvest Moon when you are in combat with Soulsow active.", RPR.JobID)]
        RPR_Soulsow_HarpeHarvestMoon = 12303,

        [ReplaceSkill(RPR.Enshroud)]
        [CustomComboInfo("Enshroud Protection Feature", "Turns Enshroud into Gibbet/Gallows to protect Soul Reaver waste.", RPR.JobID)]
        RPR_EnshroudProtection = 12306,

        [ReplaceSkill(RPR.Gibbet, RPR.Gallows, RPR.Guillotine)]
        [CustomComboInfo("Communio on Gibbet/Gallows and Guillotine Feature", "Adds Communio to Gibbet/Gallows and Guillotine.", RPR.JobID)]
        RPR_CommunioOnGGG = 12307,

        [ParentCombo(RPR_CommunioOnGGG)]
        [CustomComboInfo("Lemure's Slice/Scythe Option", "Adds Lemure's Slice to Gibbet/Gallows and Lemure's Scythe to Guillotine.", RPR.JobID)]
        RPR_LemureOnGGG = 12308,

        [ReplaceSkill(RPR.Enshroud)]
        [CustomComboInfo("Enshroud to Communio Feature", "Turns Enshroud to Communio when available to use.", RPR.JobID)]
        RPR_EnshroudCommunio = 12309,

        [ParentCombo(RPR_EnshroudProtection)]
        [CustomComboInfo("True North Feature", "Adds True North when under Gluttony and if Gibbet/Gallows options are selected to replace those skills.", RPR.JobID, 0)]
        RPR_TrueNorthEnshroud = 12310,

        [ReplaceSkill(RPR.Harpe)]
        [ParentCombo(RPR_Soulsow)]
        [CustomComboInfo("Soulsow Reminder during Combat", "Adds Soulsow to Harpe during combat when no target is selected.", RPR.JobID)]
        RPR_Soulsow_Combat = 12311,

        [ParentCombo(RPR_GluttonyBloodSwathe)]
        [CustomComboInfo("True North Feature", "Adds True North when under Gluttony and if Gibbet/Gallows options are selected to replace those skills.", RPR.JobID, 0)]
        RPR_TrueNorthGluttony = 12312,

        [Variant]
        [VariantParent(RPR_ST_AdvancedMode, RPR_AoE_AdvancedMode)]
        [CustomComboInfo("Cure Option", "Use Variant Cure when HP is below set threshold.", RPR.JobID)]
        RPR_Variant_Cure = 12313,

        [Variant]
        [VariantParent(RPR_ST_AdvancedMode, RPR_AoE_AdvancedMode)]
        [CustomComboInfo("Rampart Option", "Use Variant Rampart on cooldown.", RPR.JobID)]
        RPR_Variant_Rampart = 12314,

        // Last value = 12314

        #endregion

        #endregion

        #region RED MAGE

        /* RDM Feature Numbering
        Numbering Scheme: 13[Section][Feature Number][Sub-Feature]
        Example: 13110 (Section 1: Openers, Feature Number 1, Sub-feature 0)
        New features should be added to the appropriate sections.
        If more than 10 sub features, use the next feature number if available
        The three digets after RDM.JobID can be used to reorder items in the list
        */

        [ReplaceSkill(All.Sleep)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            RDM.JobID, -10, "", ""
        )]
        RDM_Advanced_CustomMode = 130000,

        #region Single Target DPS

        [ReplaceSkill(RDM.Jolt, RDM.Jolt2)]
        [CustomComboInfo("赤魔简单模式 - 单目标", "启用以下所有选项.", RDM.JobID, 1)]
        RDM_ST_DPS = 13000,

        [ParentCombo(RDM_ST_DPS)]
        [CustomComboInfo("魔三连起手", "以【魔三连】作为起手代替【摇荡】（必须在近战距离内）", RDM.JobID, 110)]
        RDM_Balance_Opener = 13110,

        [ParentCombo(RDM_Balance_Opener)]
        [CustomComboInfo("无视魔元", "无视魔元是否平衡，但所有技能都必须在可用状态", RDM.JobID, 111)]
        RDM_Balance_Opener_AnyMana = 13111,

        [ParentCombo(RDM_ST_DPS)]
        [CustomComboInfo("赤疾风 / 赤闪雷 替代", "以【赤疾风/赤闪雷（或其升级）】代替【摇荡】", RDM.JobID, 210)]
        RDM_ST_ThunderAero = 13210,

        [ParentCombo(RDM_ST_ThunderAero)]
        [CustomComboInfo("促进 替代", "当没有【赤飞石/赤火炎预备】时，添加【促进】", RDM.JobID, 211)]
        RDM_ST_ThunderAero_Accel = 13211,

        [ParentCombo(RDM_ST_ThunderAero_Accel)]
        [CustomComboInfo("在循环中使用 即刻咏唱", "当所有短CD技都用完后，添加【即刻咏唱】", RDM.JobID, 212)]
        RDM_ST_ThunderAero_Accel_Swiftcast = 13212,

        [ParentCombo(RDM_ST_DPS)]
        [CustomComboInfo("赤飞石 / 赤火炎 替代", "以【赤飞石/赤火炎】代替【摇荡】", RDM.JobID, 220)]
        RDM_ST_FireStone = 13220,

        [ParentCombo(RDM_ST_DPS)]
        [CustomComboInfo("能力技 设置", "在循环中加入能力技", RDM.JobID, 240)]
        RDM_ST_oGCD = 13240,

        [ParentCombo(RDM_ST_DPS)]
        [CustomComboInfo("魔三连 设置", "在循环中加入魔三连（必须在近战范围内或启用了【使用短兵相接接近目标】）", RDM.JobID, 410)]
        RDM_ST_MeleeCombo = 13410,

        [ParentCombo(RDM_ST_MeleeCombo)]
        [CustomComboInfo("倍增与鼓励使用 设置", "加入倍增与鼓励（必须在近战范围内或启用了【使用短兵相接接近目标】）", RDM.JobID, 411)]
        RDM_ST_MeleeCombo_ManaEmbolden = 13411,

        [ParentCombo(RDM_ST_MeleeCombo_ManaEmbolden)]
        [CustomComboInfo("倍增与鼓励延后使用 设置", "延后使用这两个技能直到你能打出两套魔三连", RDM.JobID, 412)]
        RDM_ST_MeleeCombo_ManaEmbolden_DoubleCombo = 13412,

        [ParentCombo(RDM_ST_MeleeCombo)]
        [CustomComboInfo("使用短兵相接接近目标 设置", "当你不在近战范围内并魔元足够你开始魔三连的时候，使用短兵相接接近目标", RDM.JobID, 430)]
        RDM_ST_MeleeCombo_CorpsGapCloser = 13430,

        [ParentCombo(RDM_ST_MeleeCombo)]
        [CustomComboInfo("不平衡魔元处理 设置", "通过使用【促进】来平衡魔元以开始魔三连", RDM.JobID, 410)]
        RDM_ST_MeleeCombo_UnbalanceMana = 13440,

        [ParentCombo(RDM_ST_DPS)]
        [CustomComboInfo("近战技能终结 设置", "在魔三连后加入【赤神圣/赤核爆】等终结技能", RDM.JobID, 510)]
        RDM_ST_MeleeFinisher = 13510,

        [ParentCombo(RDM_ST_DPS)]
        [CustomComboInfo("在循环使用 醒梦", "当MP小于指定值时，使用【醒梦】", RDM.JobID, 610)]
        RDM_ST_Lucid = 13610,

        #endregion

        #region AoE DPS

        [ReplaceSkill(RDM.Scatter, RDM.Impact)]
        [CustomComboInfo("赤魔简单模式 - AOE", "启用以下所有选项", RDM.JobID, 310)]
        RDM_AoE_DPS = 13310,

        [ParentCombo(RDM_AoE_DPS)]
        [ReplaceSkill(RDM.Scatter, RDM.Impact)]
        [CustomComboInfo("AOE的 促进 选项", "使用【促进】来增加伤害", RDM.JobID, 320)]
        RDM_AoE_Accel = 13320,

        [ParentCombo(RDM_AoE_Accel)]
        [CustomComboInfo("对单目标时使用即刻咏唱 设置", "当所有的短CD技能用完了或你低于50级时，在循环中加入【即刻咏唱】", RDM.JobID, 321)]
        RDM_AoE_Accel_Swiftcast = 13321,

        [ParentCombo(RDM_AoE_Accel)]
        [CustomComboInfo("使用 促进 设置", "只在能力技时间窗内使用【促进】", RDM.JobID, 322)]
        RDM_AoE_Accel_Weave = 13322,

        [ParentCombo(RDM_AoE_DPS)]
        [CustomComboInfo("能力技 设置", "在循环中加入能力技", RDM.JobID, 240)]
        RDM_AoE_oGCD = 13241,

        [ParentCombo(RDM_AoE_DPS)]
        [CustomComboInfo("划圆斩 设置", "当你的两种魔元都在50以上时，使用划圆斩", RDM.JobID, 420)]
        RDM_AoE_MeleeCombo = 13420,

        [ParentCombo(RDM_AoE_MeleeCombo)]
        [CustomComboInfo("倍增与鼓励使用 设置", "加入倍增与鼓励（必须在近战范围内）", RDM.JobID, 411)]
        RDM_AoE_MeleeCombo_ManaEmbolden = 13421,

        [ParentCombo(RDM_AoE_MeleeCombo)]
        [CustomComboInfo("使用短兵相接接近目标 设置", "当你不在近战范围内并魔元足够你开始魔三连的时候，使用短兵相接接近目标", RDM.JobID, 430)]
        RDM_AoE_MeleeCombo_CorpsGapCloser = 13422,

        [ParentCombo(RDM_AoE_MeleeCombo)]
        [CustomComboInfo("不平衡魔元处理 设置", "使用促进触发黑白魔元中较低的一方以便使用魔三连.", RDM.JobID, 410)]
        RDM_AoE_MeleeCombo_UnbalanceMana = 13423,

        [ParentCombo(RDM_AoE_DPS)]
        [CustomComboInfo("近战技能终结 设置", "在魔三连后加入【赤神圣/赤核爆】等终结技能", RDM.JobID, 510)]
        RDM_AoE_MeleeFinisher = 13424,

        [ParentCombo(RDM_AoE_DPS)]
        [CustomComboInfo("在循环使用 醒梦", "当MP小于指定值时，使用【醒梦】", RDM.JobID, 610)]
        RDM_AoE_Lucid = 13425,

        #endregion

        #region QoL

        [ReplaceSkill(All.Swiftcast)]
        [ConflictingCombos(ALL_Caster_Raise)]
        [CustomComboInfo("赤复活 设置", "当存在连续咏唱状态或使用即刻咏唱后，将即刻咏唱替换为赤复活.", RDM.JobID, 620)]
        RDM_Raise = 13620,

        #endregion

        #region Sections 8 to 9 - Miscellaneous

        [ReplaceSkill(RDM.Displacement)]
        [CustomComboInfo("移转与短兵相接替换 设置", "当距离过远时替换移转为短兵相接.", RDM.JobID, 810)]
        RDM_CorpsDisplacement = 13810,

        [ReplaceSkill(RDM.Embolden)]
        [CustomComboInfo("鼓励与倍增替换 设置", "当鼓励在冷却时替换其为倍增.", RDM.JobID, 820)]
        RDM_EmboldenManafication = 13820,

        [ReplaceSkill(RDM.MagickBarrier)]
        [CustomComboInfo("抗死与昏乱替换 设置", "当抗死在冷却时替换其为昏乱.", RDM.JobID, 820)]
        RDM_MagickBarrierAddle = 13821,

        [Variant]
        [VariantParent(RDM_ST_DPS, RDM_AoE_DPS)]
        [CustomComboInfo("铁壁 选项", "Use Variant Rampart on cooldown. Replaces Jolts.", RDM.JobID)]
        RDM_Variant_Rampart = 13830,

        [Variant]
        [VariantParent(RDM_Raise)]
        [CustomComboInfo("复活 选项", "Turn Swiftcast into Variant Raise whenever you have the Swiftcast or Dualcast buffs.", RDM.JobID)]
        RDM_Variant_Raise = 13831,

        [Variant]
        [VariantParent(RDM_ST_DPS, RDM_AoE_DPS)]
        [CustomComboInfo("治疗 选项", "Use Variant Cure when HP is below set threshold. Replaces Jolts.", RDM.JobID)]
        RDM_Variant_Cure = 13832,

        [Variant]
        [CustomComboInfo("Cure on Vercure Option", "Replaces Vercure with Variant Cure.", RDM.JobID)]
        RDM_Variant_Cure2 = 13833,


        [ReplaceSkill(RDM.Embolden)]
        [CustomComboInfo("Embolden Overlap Protection", "Disables Embolden when buffed by another Red Mage's Embolden.", RDM.JobID, 820)]
        RDM_EmboldenProtection = 13835,

        [ReplaceSkill(RDM.MagickBarrier)]
        [CustomComboInfo("Magick Barrier Overlap Protection", "Disables Magick Barrier when buffed by another Red Mage's Magick Barrier.", RDM.JobID, 820)]
        RDM_MagickProtection = 13836,

        #endregion

        #endregion

        #region SAGE

        /* SGE Feature Numbering
        Numbering Scheme: 14[Feature][Option][Sub-Option]
        Example: 14110 (Feature Number 1, Option 1, no suboption)
        New features should be added to the appropriate sections.
        */

        [ReplaceSkill(All.Repose)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            SGE.JobID, -10, "", ""
        )]
        SGE_Advanced_CustomMode = 141001,


        #region Single Target DPS Feature

        [ReplaceSkill(SGE.Dosis, SGE.Dosis2, SGE.Dosis3)]
        [CustomComboInfo("整合单体输出技能", "注药I/II/III 各种选项", SGE.JobID, 100, "", "")]
        SGE_ST_DPS = 14100,


        [ParentCombo(SGE_ST_DPS)]
        [CustomComboInfo("循环加入醒梦", "当MP小于滑块值时，将醒梦整合至此循环", SGE.JobID, 120, "", "")]
        SGE_ST_DPS_Lucid = 14110,

        [ParentCombo(SGE_ST_DPS)]
        [CustomComboInfo("均衡注药 设置", "自动插入均衡注药续Dot.", SGE.JobID, 110, "", "")]
        SGE_ST_DPS_EDosis = 14120,

        [ParentCombo(SGE_ST_DPS)]
        [CustomComboInfo("移动选项", "移动时使用选择的顺发技能", SGE.JobID, 112, "", "")]
        SGE_ST_DPS_Movement = 14130,

        [ParentCombo(SGE_ST_DPS)]
        [CustomComboInfo("发炎 选项", "使用发炎如果它可用并且在距离内", SGE.JobID, 111, "", "")]
        SGE_ST_DPS_Phlegma = 14140,

        [ParentCombo(SGE_ST_DPS)]
        [CustomComboInfo("心关 提醒选项", "当没有 心关 状态时插入 心关。", SGE.JobID, 122, "", "")]
        SGE_ST_DPS_Kardia = 14150,

        [ParentCombo(SGE_ST_DPS)]
        [CustomComboInfo("根素选项", "当蛇胆少于指定值时插入 根素。", SGE.JobID, 121, "", "")]
        SGE_ST_DPS_Rhizo = 14160,

        [ParentCombo(SGE_ST_DPS)]
        [CustomComboInfo("精神干预 Option", "Weaves(?) Psych when available.", SGE.JobID, 112, "", "")]
        SGE_ST_DPS_Psyche = 14008,

        #endregion

        #region AoE DPS Feature

        [ReplaceSkill(SGE.Phlegma, SGE.Phlegma2, SGE.Phlegma3)]
        [CustomComboInfo("AoE DPS连击", "", SGE.JobID, 200, "", "")]
        SGE_AoE_DPS = 14200,

        [ParentCombo(SGE_AoE_DPS)]
        [CustomComboInfo("箭毒 无发炎层数时选项", "当没有发炎层数时使用箭毒，优先于失衡", SGE.JobID, 210, "", "")]
        SGE_AoE_DPS_NoPhlegmaToxikon = 14210,

        [ParentCombo(SGE_AoE_DPS)]
        [CustomComboInfo("箭毒 超出发炎范围时选项", "当超出发炎范围时使用箭毒，优先于失衡", SGE.JobID, 220, "", "")]
        SGE_AoE_DPS_OutOfRangeToxikon = 14220,

        [ParentCombo(SGE_AoE_DPS)]
        [CustomComboInfo("失衡 无发炎层数时选项", "当超出发炎范围时使用失衡", SGE.JobID, 230, "", "")]
        SGE_AoE_DPS_NoPhlegmaDyskrasia = 14230,

        [ParentCombo(SGE_AoE_DPS)]
        [CustomComboInfo("失衡 无目标锁定选项", "当无目标锁定时使用失衡", SGE.JobID, 240, "", "")]
        SGE_AoE_DPS_NoTargetDyskrasia = 14240,

        [ParentCombo(SGE_AoE_DPS)]
        [CustomComboInfo("循环加入醒梦", "当MP小于滑块值时，将醒梦整合至此循环", SGE.JobID, 250, "", "")]
        SGE_AoE_DPS_Lucid = 14250,

        [ParentCombo(SGE_AoE_DPS)]
        [CustomComboInfo("根素选项", "当蛇胆少于指定值时插入 根素。", SGE.JobID, 121, "", "")]
        SGE_AoE_DPS_Rhizo = 14260,


        [ParentCombo(SGE_AoE_DPS)]
        [CustomComboInfo("Psyche Option", "Weaves Psyche if available.", SGE.JobID, 2, "", "")]
        SGE_AoE_DPS_Psyche = 14051,

        [ParentCombo(SGE_AoE_DPS)]
        [CustomComboInfo("均衡 Option", "Uses 均衡 for Eukrasia Dyskrasia.", SGE.JobID, 1, "", "")]
        SGE_AoE_DPS_EDyskrasia = 14052,


        [ParentCombo(SGE_AoE_DPS)]
        [CustomComboInfo("发炎 Option", "Uses 发炎 if available.", SGE.JobID, 3, "", "")]
        SGE_AoE_DPS_Phlegma = 14010,

        [ParentCombo(SGE_AoE_DPS)]
        [CustomComboInfo("箭毒 Option", "Use 箭毒 if available.", SGE.JobID, 4, "", "")]
        SGE_AoE_DPS_Toxikon = 14011,

        #endregion

        #region Diagnosis Simple Single Target Heal

        [ReplaceSkill(SGE.Diagnosis)]
        [CustomComboInfo("单目标治疗功能", "支持软目标。\n以下选项按优先顺序排列。", SGE.JobID, 300, "", "")]
        SGE_ST_Heal = 14300,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("康复 选项", "当你的目标有一个可清除的debuff时则使用 康复。", SGE.JobID, 301, "", "")]
        SGE_ST_Heal_Esuna = 14301,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("心关", "如果心关从未使用过，则对选中目标使用心关。", SGE.JobID, 305, "", "")]
        SGE_ST_Heal_Kardia = 14310,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo
        (
            "Eukrasian Diagnosis Option", "Diagnosis becomes Eukrasian Diagnosis if the shield is not applied to the target.", SGE.JobID,
            313, "", ""
        )]
        SGE_ST_Heal_EDiagnosis = 14320,


        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("拯救选项", "插入拯救.", SGE.JobID, 306, "", "")]
        SGE_ST_Heal_Soteria = 14330,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("活化选项", "插入活化.", SGE.JobID, 307, "", "")]
        SGE_ST_Heal_Zoe = 14340,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("消化选项", "当盾值存在时激活消化。", SGE.JobID, 309, "", "")]
        SGE_ST_Heal_Pepsis = 14350,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("白牛清汁选项", "插入白牛清汁.", SGE.JobID, 303, "", "")]
        SGE_ST_Heal_Taurochole = 14360,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("输血选项", "插入输血.", SGE.JobID, 310, "", "")]
        SGE_ST_Heal_Haima = 14370,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("根素选项", "当没有蛇胆时插入根素.", SGE.JobID, 304, "", "")]
        SGE_ST_Heal_Rhizomata = 14380,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("混合选项", "插入混合.", SGE.JobID, 308, "", "")]
        SGE_ST_Heal_Krasis = 14390,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("灵橡清汁选项", "插入灵橡清汁.", SGE.JobID, 302, "", "")]
        SGE_ST_Heal_Druochole = 14400,

        #endregion

        #region Sage Simple AoE Heal

        [ReplaceSkill(SGE.Prognosis)]
        [CustomComboInfo("群体治疗技能一键整合至预后", "按照个人喜好制定群体治疗技能一键整合方式.", SGE.JobID, 500, "", "")]
        SGE_AoE_Heal = 14500,


        [ParentCombo(SGE_AoE_Heal)]
        [CustomComboInfo("自生", "自动插入自生。", SGE.JobID, 504, "", "")]
        SGE_AoE_Heal_Physis = 14510,

        [ParentCombo(SGE_AoE_Heal)]
        [CustomComboInfo("均衡预后", "当没有盾值时，替换预后为均衡预后。", SGE.JobID, 520, "", "")]
        SGE_AoE_Heal_EPrognosis = 14520,

        [ParentCombo(SGE_AoE_Heal)]
        [CustomComboInfo("Philosophia Option", "Adds Philosophia.", SGE.JobID, 505, "", "")]
        SGE_AoE_Heal_Philosophia = 14050,


        [ParentCombo(SGE_AoE_Heal_EPrognosis)]
        [CustomComboInfo
        (
            "忽略护盾检查，强制整合", "Warning, will force the use of Eukrasia Prognosis, and normal Prognosis will be unavailable.", SGE.JobID,
            520, "", ""
        )]
        SGE_AoE_Heal_EPrognosis_IgnoreShield = 14521,

        [ParentCombo(SGE_AoE_Heal)]
        [CustomComboInfo("整体论", "自动插入整体论。", SGE.JobID, 505, "", "")]
        SGE_AoE_Heal_Holos = 14530,

        [ParentCombo(SGE_AoE_Heal)]
        [CustomComboInfo("泛输血", "自动插入泛输血。", SGE.JobID, 506, "", "")]
        SGE_AoE_Heal_Panhaima = 14540,

        [ParentCombo(SGE_AoE_Heal)]
        [CustomComboInfo("消化选项", "当盾值存在时激活消化。", SGE.JobID, 507, "", "")]
        SGE_AoE_Heal_Pepsis = 14550,

        [ParentCombo(SGE_AoE_Heal)]
        [CustomComboInfo("寄生清汁", "整合寄生清汁.", SGE.JobID, 503, "", "")]
        SGE_AoE_Heal_Ixochole = 14560,

        [ParentCombo(SGE_AoE_Heal)]
        [CustomComboInfo("坚角清汁", "整合坚角清汁.", SGE.JobID, 502, "", "")]
        SGE_AoE_Heal_Kerachole = 14570,

        [ParentCombo(SGE_AoE_Heal)]
        [CustomComboInfo("根素选项", "当没有蛇胆时插入根素.", SGE.JobID, 501, "", "")]
        SGE_AoE_Heal_Rhizomata = 14580,

        #endregion

        #region Misc Healing

        [ReplaceSkill(SGE.Taurochole, SGE.Druochole, SGE.Ixochole, SGE.Kerachole)]
        [CustomComboInfo("根素 设置", "蛇胆不满时插入根素", SGE.JobID, 600, "", "")]
        SGE_Rhizo = 14600,

        [ReplaceSkill(SGE.Druochole)]
        [CustomComboInfo("替换灵橡清汁为白牛清汁 设置", "当白牛青汁可用时，替换灵橡清汁为白牛清汁.", SGE.JobID, 700, "", "")]
        SGE_DruoTauro = 14700,

        [ReplaceSkill(SGE.Pneuma)]
        [CustomComboInfo("将活化整合至魂灵风息 设置", "魂灵风息变为活化，使用后变回魂灵风息.", SGE.JobID, 701, "", "")] //Temporary to keep the order
        SGE_ZoePneuma = 141000,

        #endregion

        #region Utility

        [ReplaceSkill(All.Swiftcast)]
        [ConflictingCombos(ALL_Healer_Raise)]
        [CustomComboInfo("即刻复活 设置", "整合即可咏唱至复苏", SGE.JobID, 800, "", "")]
        SGE_Raise = 14800,

        [ReplaceSkill(SGE.Soteria)]
        [CustomComboInfo("替换拯救为心关 设置", "当未使用心关或拯救处于冷却状态时，替换拯救为心关。", SGE.JobID, 900, "", "")]
        SGE_Kardia = 14900,

        [ReplaceSkill(SGE.Eukrasia)]
        [CustomComboInfo("均衡技能整合 设置", "使用均衡后将其替换为下列选择的技能之一.", SGE.JobID, 1000, "", "")]
        SGE_Eukrasia = 14910,

        [ReplaceSkill(SGE.Kerachole)]
        [CustomComboInfo("Spell Overlap Protection", "Prevents you from wasting actions if under the effect of someone else's actions", SGE.JobID, 1000, "", "")]
        SGE_OverProtect = 14043,

        [ParentCombo(SGE_OverProtect)]
        [CustomComboInfo("Under Kerachole", "Don't use Kerachole when under the effect of someone's Kerachole", SGE.JobID, 1000, "", "")]
        SGE_OverProtect_Kerachole = 14044,

        [ParentCombo(SGE_OverProtect_Kerachole)]
        [CustomComboInfo("Under Sacred Soil", "Don't use Kerachole when under the effect of someone's Sacred Soil", SGE.JobID, 1000, "", "")]
        SGE_OverProtect_SacredSoil = 14045,

        [ParentCombo(SGE_OverProtect)]
        [CustomComboInfo("Under Panhaima", "Don't use Panhaima when under the effect of someone's Panhaima", SGE.JobID, 1000, "", "")]
        SGE_OverProtect_Panhaima = 14046,

        [ParentCombo(SGE_OverProtect)]
        [CustomComboInfo("Under Philosophia", "Don't use Philosophia when under the effect of someone's Philosophia", SGE.JobID, 1000, "", "")]
        SGE_OverProtect_Philosophia = 14047,

        [Variant]
        [VariantParent(SGE_ST_DPS_EDosis, SGE_AoE_DPS)]
        [CustomComboInfo("精神镖选项", "当没有dot或dot剩余时间少于3s时，使用多变精神镖", SGE.JobID)]
        SGE_DPS_Variant_SpiritDart = 14920,

        [Variant]
        [VariantParent(SGE_ST_DPS, SGE_AoE_DPS)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", SGE.JobID)]
        SGE_DPS_Variant_Rampart = 14930,

        #endregion

        // Last value = 14930

        #endregion

        #region SAMURAI

        #region Overcap Features

        [ReplaceSkill(SAM.Kasha, SAM.Gekko, SAM.Yukikaze)]
        [CustomComboInfo("武士单体技能替代选项", "当剑气达到或超过所选数量时，将必杀剑·震天添加到单体连击中。", SAM.JobID, 0, "", "")]
        SAM_ST_Overcap = 15001,

        [ReplaceSkill(SAM.Mangetsu, SAM.Oka)]
        [CustomComboInfo("武士AOE技能替代选项", "当剑气达到或超过所选数量时，将必杀剑·九天添加到主要AOE连击中。", SAM.JobID, 0, "", "")]
        SAM_AoE_Overcap = 15002,

        #endregion

        #region Main Combo (Gekko) Features

        [ReplaceSkill(SAM.Gekko)]
        [CustomComboInfo("月光连", "替换 月光 技能为它的连击链。\n如果下面的全部子选项都有相应选择会形成一个一键连击循环 （高级武士）", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo = 15003,

        [ParentCombo(SAM_ST_GekkoCombo)]
        [CustomComboInfo("燕飞特性", "当你离开射程时，使用燕飞。", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_RangedUptime = 15004,

        [ParentCombo(SAM_ST_GekkoCombo)]
        [CustomComboInfo("雪风连击加入循环", "Adds Yukikaze combo to main combo. Will add Yukikaze during Meikyo Shisui as well", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_Yukikaze = 15005,

        [ParentCombo(SAM_ST_GekkoCombo)]
        [CustomComboInfo("花车连击加入循环", "将花车连击加入主循环。在明镜止水期间也会加入花车连击", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_Kasha = 15006,

        [ConflictingCombos(SAM_GyotenYaten)]
        [ParentCombo(SAM_ST_GekkoCombo)]
        [CustomComboInfo
        (
            "Level 90 Samurai Opener",
            "Adds the Level 90 Opener to the main combo.\nOpener triggered by using Meikyo Shisui before combat. If you have any Sen, Hagakure will be used to clear them.\nWill work at any levels of Kenki, requires 2 charges of Meikyo Shisui and all CDs ready. If conditions aren't met it will skip into the regular rotation. \nIf the Opener is interrupted, it will exit the opener via a Goken and a Kaeshi: Goken at the end or via the last Yukikaze. If the latter, CDs will be used on cooldown regardless of burst options.",
            SAM.JobID
        )]
        SAM_ST_GekkoCombo_Opener = 15007,

        [ConflictingCombos(SAM_GyotenYaten)]
        [ParentCombo(SAM_ST_GekkoCombo)]
        [CustomComboInfo("填充技能选项", "在适当的时候将选定的填充技能组合添加到输出循环中。选择技能速度与下面的Fuka buff。\n在你死亡或不使用开场爆发的情况下失效。", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_FillerCombos = 15008,

        #region CDs on Main Combo

        [ParentCombo(SAM_ST_GekkoCombo)]
        [CustomComboInfo("主连击CD整合", "将CD技能加入循环", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs = 15099,

        [ParentCombo(SAM_ST_GekkoCombo_CDs)]
        [CustomComboInfo("将意气冲天加入循环", "当剑气达到或低于50时，可将意气冲天加入到月光和满月连击中。\n将在义气冲天还剩10秒时消耗剑气，防止剑气溢出。", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs_Ikishoten = 15009,

        [ParentCombo(SAM_ST_GekkoCombo_CDs)]
        [CustomComboInfo
        (
            "将居合术加入循环",
            "Adds Midare: Setsugekka, Higanbana, and Kaeshi: Setsugekka when ready and when you're not moving to main combo.", SAM.JobID, 0, "", ""
        )]
        SAM_ST_GekkoCombo_CDs_Iaijutsu = 15010,

        #region Ogi Namikiri on Main Combo

        [ParentCombo(SAM_ST_GekkoCombo_CDs)]
        [CustomComboInfo("将奥义斩浪加入循环", "在未移动和技能冷却完毕的情况下，将奥义斩浪和回返斩浪加入循环。", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs_OgiNamikiri = 15011,

        [ParentCombo(SAM_ST_GekkoCombo_CDs_OgiNamikiri)]
        [CustomComboInfo
        (
            "奥义斩浪爆发选项",
            "Saves Ogi Namikiri for even minute burst windows.\nIf you don't activate the opener or die, Ogi Namikiri will instead be used on CD.",
            SAM.JobID, 0, "", ""
        )]
        SAM_ST_GekkoCombo_CDs_OgiNamikiri_Burst = 15012,

        #endregion

        [ParentCombo(SAM_ST_GekkoCombo_CDs)]
        [CustomComboInfo("将明镜止水加入循环", "当冷却完毕时将明镜止水加入循环", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs_MeikyoShisui = 15013,

        #region Meikyo Shisui on Main Combo

        [ParentCombo(SAM_ST_GekkoCombo_CDs_MeikyoShisui)]
        [CustomComboInfo
        (
            "明镜止水爆发选项",
            "Saves Meikyo Shisui for burst windows.\nIf you don't activate the opener or die, Meikyo Shisui will instead be used on CD.", SAM.JobID,
            0, "", ""
        )]
        SAM_ST_GekkoCombo_CDs_MeikyoShisui_Burst = 15014,

        #endregion

        [ParentCombo(SAM_ST_GekkoCombo_CDs)]
        [CustomComboInfo("将照破加入循环", "当拥有三档剑压时，将照破加入循环", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs_Shoha = 15015,

        [ConflictingCombos(SAM_Shinten_Shoha_Senei)]
        [ParentCombo(SAM_ST_GekkoCombo_CDs)]
        [CustomComboInfo("将必杀剑·闪影加入循环", "当必杀剑·闪影冷却完毕且剑气超过25时，将必杀剑·闪影加入循环", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs_Senei = 15016,

        [ParentCombo(SAM_ST_GekkoCombo_CDs_Senei)]
        [CustomComboInfo
        (
            "必杀剑·闪影爆发选项",
            "Saves Senei for even minute burst windows.\nIf you don't activate the opener or die, Senei will instead be used on CD.", SAM.JobID, 0,
            "", ""
        )]
        SAM_ST_GekkoCombo_CDs_Senei_Burst = 15017,

        [ParentCombo(SAM_ST_Overcap)]
        [CustomComboInfo("震天 选项", "当目标血量小于此百分比时，只要有25点剑气就将震天加入循环.", SAM.JobID, 0, "", "")]
        SAM_ST_Execute = 15046,

        #endregion

        #endregion

        #region Yukikaze/Kasha Combos

        [ReplaceSkill(SAM.Yukikaze)]
        [CustomComboInfo("雪风连", "替换雪风为相应连击。", SAM.JobID, 0, "", "")]
        SAM_ST_YukikazeCombo = 15018,

        [ReplaceSkill(SAM.Kasha)]
        [CustomComboInfo("花车连", "替换花车为相应连击。", SAM.JobID, 0, "", "")]
        SAM_ST_KashaCombo = 15019,

        #endregion

        #region AoE Combos

        [ReplaceSkill(SAM.Mangetsu)]
        [CustomComboInfo("满月连", "用满月连击的组合代替满月。\n如果所有子选项都被选中，将变成一个完整的一键AOE循环（高级武士）。", SAM.JobID, 0, "", "")]
        SAM_AoE_MangetsuCombo = 15020,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [ConflictingCombos(SAM_AoE_OkaCombo_TwoTarget)]
        [CustomComboInfo("樱花 添加到 满月 连击", "将樱花连击加如到AOE连击中。\n樱花连击中将试情况使用明镜止水", SAM.JobID, 0, "", "")]
        SAM_AoE_MangetsuCombo_Oka = 15021,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo
        (
            "将居合术加入AOE连击",
            "Adds Tenka Goken, Midare: Setsugekka, and Kaeshi: Goken when ready and when you're not moving to Mangetsu combo.", SAM.JobID, 0, "", ""
        )]
        SAM_AoE_MangetsuCombo_TenkaGoken = 15022,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("将奥义斩浪加入AOE连击", "在未移动和技能冷却完毕的情况下，将奥义斩浪加入循环。", SAM.JobID, 0, "", "")]
        SAM_AoE_MangetsuCombo_OgiNamikiri = 15023,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("将无明照破加入AOE连击", "当拥有三档剑压时，将无明照破加入AOE循环", SAM.JobID, 0, "", "")]
        SAM_AoE_MangetsuCombo_Shoha2 = 15024,

        [ConflictingCombos(SAM_Kyuten_Shoha2_Guren)]
        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("将必杀剑·红莲加入AOE连击", "当必杀剑·红莲冷却完毕且剑气超过25时，将必必杀剑·红莲加入循环", SAM.JobID, 0, "", "")]
        SAM_AoE_MangetsuCombo_Guren = 15025,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("明镜止水 添加到 满月 连击", "明镜止水 添加到 满月 连击。", SAM.JobID, 0, "", "")]
        SAM_AoE_MangetsuCombo_MeikyoShisui = 15039,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("意气冲天 添加到 满月 连击", "当剑气达到或低于50时，可将意气冲天加入到月光和满月连击中。\n将在义气冲天还剩10秒时消耗剑气，防止剑气溢出。", SAM.JobID, 0, "", "")]
        SAM_AOE_GekkoCombo_CDs_Ikishoten = 15040,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("叶隐 添加到 满月 连击", "当有三层剑压时，将 叶隐 添加到 满月 连击。", SAM.JobID, 0, "", "")]
        SAM_AoE_MangetsuCombo_Hagakure = 15041,

        [ReplaceSkill(SAM.Oka)]
        [CustomComboInfo("樱花连", "替换樱花为相应连击。", SAM.JobID, 0, "", "")]
        SAM_AoE_OkaCombo = 15026,

        [ParentCombo(SAM_AoE_OkaCombo)]
        [ConflictingCombos(SAM_AoE_MangetsuCombo_Oka)]
        [CustomComboInfo
        (
            "樱花双目标功能",
            "Adds the Yukikaze combo, Mangetsu combo, Senei, Shinten, and Shoha to Oka combo.\nUsed for two targets only and when Lv86 and above.",
            SAM.JobID, 0, "", ""
        )]
        SAM_AoE_OkaCombo_TwoTarget = 150261,

        #endregion

        #region Cooldown Features

        [ReplaceSkill(SAM.MeikyoShisui)]
        [CustomComboInfo("明镜止水", "Replace Meikyo Shisui with Jinpu, Shifu, and Yukikaze depending on what is needed.", SAM.JobID, 0, "", "")]
        SAM_JinpuShifu = 15027,

        #endregion

        #region Iaijutsu Features

        [ReplaceSkill(SAM.Iaijutsu)]
        [CustomComboInfo("居合术功能", "居合术整合", SAM.JobID, 0, "", "")]
        SAM_Iaijutsu = 15028,

        [ParentCombo(SAM_Iaijutsu)]
        [CustomComboInfo("使用燕回返代替居合术", "当闪为空时，用燕回返代替居合术。", SAM.JobID, 0, "", "")]
        SAM_Iaijutsu_TsubameGaeshi = 15029,

        [ParentCombo(SAM_Iaijutsu)]
        [CustomComboInfo("照破 替换 居合术", "当剑压积累到3档时，替换居合术为照破。", SAM.JobID, 0, "", "")]
        SAM_Iaijutsu_Shoha = 15030,

        [ParentCombo(SAM_Iaijutsu)]
        [CustomComboInfo("使用奥义斩浪代替居合术", "处于奥义斩浪预备的时候，使用回奥义斩浪和返斩浪代替居合术。", SAM.JobID, 0, "", "")]
        SAM_Iaijutsu_OgiNamikiri = 15031,

        #endregion

        #region Shinten Features

        [ReplaceSkill(SAM.Shinten)]
        [CustomComboInfo("照破 替换 必杀剑·震天", "当剑压积累到3档时，替换必杀剑·震天为照破。", SAM.JobID, 0, "", "")]
        SAM_Shinten_Shoha = 15032,

        [ConflictingCombos(SAM_ST_GekkoCombo_CDs_Senei)]
        [ParentCombo(SAM_Shinten_Shoha)]
        [CustomComboInfo("必杀剑·闪影 替换 必杀剑·震天", "当必杀剑·闪影冷却结束后，替换必杀剑·震天为必杀剑·闪影。", SAM.JobID, 0, "", "")]
        SAM_Shinten_Shoha_Senei = 15033,

        #endregion

        #region Kyuten Features

        [ReplaceSkill(SAM.Kyuten)]
        [CustomComboInfo("无名照破 替换 必杀剑·九天", "当剑压积累到3档时，替换必杀剑·九天为无名照破。", SAM.JobID, 0, "", "")]
        SAM_Kyuten_Shoha2 = 15034,

        [ConflictingCombos(SAM_AoE_MangetsuCombo_Guren)]
        [ParentCombo(SAM_Kyuten_Shoha2)]
        [CustomComboInfo("必杀剑·红莲 替换 必杀剑·九天", "当必杀剑·红莲冷却结束后，替换必杀剑·九天为必杀剑·红莲。", SAM.JobID, 0, "", "")]
        SAM_Kyuten_Shoha2_Guren = 15035,

        #endregion

        #region Other

        [ConflictingCombos(SAM_ST_GekkoCombo_Opener, SAM_ST_GekkoCombo_FillerCombos)]
        [ReplaceSkill(SAM.Gyoten)]
        [CustomComboInfo("必杀剑·晓天 特性", "根据与目标的距离自动将必杀剑·晓天变为必杀剑·夜天/晓天。", SAM.JobID, 0, "", "")]
        SAM_GyotenYaten = 15036,

        [ReplaceSkill(SAM.Ikishoten)]
        [CustomComboInfo
        (
            "意气冲天 奥义斩浪 连击特性",
            "Replace Ikishoten with Ogi Namikiri and then Kaeshi Namikiri when available.\nIf you have full Meditation stacks, Ikishoten becomes Shoha while you have Ogi Namikiri ready.",
            SAM.JobID, 0, "", ""
        )]
        SAM_Ikishoten_OgiNamikiri = 15037,

        [ReplaceSkill(SAM.Gekko, SAM.Yukikaze, SAM.Kasha)]
        [CustomComboInfo("真北 开关", "处于明镜止水状态时，将真北插入所有的单体连击", SAM.JobID, 0, "", "")]
        SAM_TrueNorth = 15038,

        [ParentCombo(SAM_ST_GekkoCombo)]
        [CustomComboInfo
        (
            "回复设置", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", SAM.JobID, 0, "",
            ""
        )]
        SAM_ST_ComboHeals = 15043,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo
        (
            "回复设置", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", SAM.JobID, 0, "",
            ""
        )]
        SAM_AoE_ComboHeals = 15045,

        [Variant]
        [VariantParent(SAM_ST_GekkoCombo, SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", SAM.JobID)]
        SAM_Variant_Cure = 15047,

        [Variant]
        [VariantParent(SAM_ST_GekkoCombo, SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", SAM.JobID)]
        SAM_Variant_Rampart = 15048,

        #endregion

        // Last value = 15048

        #endregion

        #region SCHOLAR

        /* SCH Feature Numbering
        Numbering Scheme: 16[Feature][Option][Sub-Option]
        Example: 16110 (Feature Number 1, Option 1, no suboption)
        New features should be added to the appropriate sections.
        */


        [ReplaceSkill(All.Repose)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            SCH.JobID, -10, "", ""
        )]
        SCH_Advanced_CustomMode = 161001,


        #region DPS

        [ReplaceSkill(SCH.Ruin, SCH.Broil, SCH.Broil2, SCH.Broil3, SCH.Broil4, SCH.Bio, SCH.Bio2, SCH.Biolysis)]
        [CustomComboInfo("整合单体输出技能", "Replaces Ruin I / Broils with options below", SCH.JobID, 1)]
        SCH_DPS = 16100,

        [ParentCombo(SCH_DPS)]
        [CustomComboInfo("醒梦选项", "当 MP 低于滑动条的值时在循环中加入醒梦", SCH.JobID, 110)]
        SCH_DPS_Lucid = 16110,

        [ParentCombo(SCH_DPS)]
        [CustomComboInfo("连环计", "将 连环计 加入到循环，并且防顶。", SCH.JobID, 120)]
        SCH_DPS_ChainStrat = 16120,

        [ParentCombo(SCH_DPS)]
        [CustomComboInfo("以太超流", "当自身没有以太超流层数时使用以太超流技能补充", SCH.JobID, 130)]
        SCH_DPS_Aetherflow = 16130,

        [ParentCombo(SCH_DPS)]
        [CustomComboInfo("能量吸收 插入选项", "在 以太超流 即将冷却完毕时，插入 能量吸收 消耗剩余豆子。", SCH.JobID, 131)]
        SCH_DPS_EnergyDrain = 16160,

        [ParentCombo(SCH_DPS_EnergyDrain)]
        [CustomComboInfo("能量吸收 爆发选项", "当 连环计 冷却完毕或少于10秒时，保留 能量吸收。", SCH.JobID, 133)]
        SCH_DPS_EnergyDrain_BurstSaver = 16161,

        [ParentCombo(SCH_DPS)]
        [CustomComboInfo("移动毁荡", "在移动时使用 毁坏。", SCH.JobID, 150)]
        SCH_DPS_Ruin2Movement = 16140,

        [ParentCombo(SCH_DPS)]
        [CustomComboInfo("毒菌/蛊毒法", "Automatic DoT uptime.", SCH.JobID, 140)]
        SCH_DPS_Bio = 16150,

        [ParentCombo(SCH_DPS)]
        [CustomComboInfo("转化 起手选项", "在战斗开始时使用 转化。", SCH.JobID, 170)]
        SCH_DPS_Dissipation_Opener = 16170,


        [ReplaceSkill(SCH.ArtOfWar, SCH.ArtOfWarII)]
        [CustomComboInfo("AoE DPS连击", "将 破阵法 替换为一键连击。", SCH.JobID, 3)]
        SCH_AoE = 16101,

        [ParentCombo(SCH_AoE)]
        [CustomComboInfo("醒梦选项", "当 MP 低于滑动条的值时在循环中加入醒梦", SCH.JobID)]
        SCH_AoE_Lucid = 16111,

        [ParentCombo(SCH_AoE)]
        [CustomComboInfo("以太超流", "当自身没有以太超流层数时使用以太超流技能补充", SCH.JobID)]
        SCH_AoE_Aetherflow = 16121,

        #endregion

        #region Healing

        [ReplaceSkill(SCH.FeyBlessing)]
        [CustomComboInfo("异想的祥光 替换 慰藉", "炽天使同行状态下，将 异想的祥光 变为 慰藉.", SCH.JobID, 9)]
        SCH_Consolation = 16210,

        [ReplaceSkill(SCH.Lustrate)]
        [CustomComboInfo("生命活性法 替换 深谋远虑之策", "Change Lustrate into Excogitation when Excogitation is ready.", SCH.JobID, 6)]
        SCH_Lustrate = 16220,

        [ReplaceSkill(SCH.Recitation)]
        [CustomComboInfo("秘策 选项", "Change Recitation into either Adloquium, Succor, Indomitability, or Excogitation when used.", SCH.JobID, 7)]
        SCH_Recitation = 16230,


        [ReplaceSkill(SCH.WhisperingDawn)]
        [CustomComboInfo("小仙女治疗选项", "Change Whispering Dawn into Fey Illumination, Fey Blessing, then Whispering Dawn when used.", SCH.JobID, 8)]
        SCH_Fairy_Combo = 16240,

        [ParentCombo(SCH_Fairy_Combo)]
        [CustomComboInfo("异想的祥光 替换 慰藉", "炽天使同行状态下，将 异想的祥光 变为 慰藉。", SCH.JobID)]
        SCH_Fairy_Combo_Consolation = 16241,

        [ReplaceSkill(SCH.Succor)]
        [CustomComboInfo("群体治疗技能一键整合至预后", "用以下选项替换 士气高扬之策：", SCH.JobID, 5)]
        SCH_AoE_Heal = 16250,

        [ParentCombo(SCH_AoE_Heal)]
        [CustomComboInfo("循环加入醒梦", "当MP不够使用士气高扬之策时，将醒梦加入到循环中。", SCH.JobID)]
        SCH_AoE_Heal_Lucid = 16251,

        [ParentCombo(SCH_AoE_Heal)]
        [CustomComboInfo("以太超流 选项", "当自身没有以太超流层数时使用以太超流技能补充", SCH.JobID)]
        SCH_AoE_Heal_Aetherflow = 16252,

        [ParentCombo(SCH_AoE_Heal_Aetherflow)]
        [CustomComboInfo("不屈不挠之策 独立选项", "只有在 不屈不挠之策 准备好的情况下才会使用 以太超流。", SCH.JobID)]
        SCH_AoE_Heal_Aetherflow_Indomitability = 16253,

        [ParentCombo(SCH_AoE_Heal)]
        [CustomComboInfo("不屈不挠之策 选项", "在 士气高扬之策 前优先使用 不屈不挠之策。", SCH.JobID)]
        SCH_AoE_Heal_Indomitability = 16254,

        [ReplaceSkill(SCH.Physick)]
        [CustomComboInfo("单目标治疗功能", "Change Physick into Adloquium, Lustrate, then Physick with below options:", SCH.JobID, 4)]
        SCH_ST_Heal = 16260,

        [ParentCombo(SCH_ST_Heal)]
        [CustomComboInfo("醒梦选项", "当 MP 低于滑动条的值时在循环中加入醒梦", SCH.JobID, 1)]
        SCH_ST_Heal_Lucid = 16261,

        [ParentCombo(SCH_ST_Heal)]
        [CustomComboInfo("以太超流", "当自身没有以太超流层数时使用以太超流技能补充", SCH.JobID, 2)]
        SCH_ST_Heal_Aetherflow = 16262,

        [ParentCombo(SCH_ST_Heal)]
        [CustomComboInfo("康复 选项", "当你的目标有一个可清除的debuff时则使用 康复。", SGE.JobID, 3)]
        SCH_ST_Heal_Esuna = 16265,

        [ParentCombo(SCH_ST_Heal)]
        [CustomComboInfo("鼓舞激励之策 选项", "当没有 鼓舞 状态或目标生命值低于指定值时插入 鼓舞激励之策：", SCH.JobID, 4)]
        SCH_ST_Heal_Adloquium = 16263,


        [ParentCombo(SCH_ST_Heal)]
        [CustomComboInfo("生命活性法 选项", "当目标生命值低于指定值时插入 生命活性法：", SCH.JobID, 5)]
        SCH_ST_Heal_Lustrate = 16264,

        #endregion

        #region Utilities

        [ReplaceSkill(SCH.EnergyDrain, SCH.Lustrate, SCH.SacredSoil, SCH.Indomitability, SCH.Excogitation)]
        [CustomComboInfo("以太超流", "Change Aetherflow-using skills to Aetherflow, Recitation, or Dissipation as selected.", SCH.JobID, 9)]
        SCH_Aetherflow = 16300,

        [ParentCombo(SCH_Aetherflow)]
        [CustomComboInfo("秘策", "在 深谋远虑之策 或 不屈不挠之策 前优先使用 秘策。", SCH.JobID)]
        SCH_Aetherflow_Recite = 16310,

        [ParentCombo(SCH_Aetherflow)]
        [CustomComboInfo("转化", "If Aetherflow is on cooldown, show Dissipation instead.", SCH.JobID)]
        SCH_Aetherflow_Dissipation = 16320,

        [ReplaceSkill(All.Swiftcast)]
        [ConflictingCombos(ALL_Healer_Raise)]
        [CustomComboInfo("即刻复活设置", "当 即刻咏唱 冷却完毕时替换 复生。", SCH.JobID, 10)]
        SCH_Raise = 16400,

        [ReplaceSkill(SCH.WhisperingDawn, SCH.FeyBlessing, SCH.FeyBlessing, SCH.Aetherpact, SCH.Dissipation)]
        [CustomComboInfo("小仙女", "Change all fairy actions into Summon Eos when the Fairy is not summoned.", SCH.JobID, 11)]
        SCH_FairyReminder = 16500,

        [ReplaceSkill(SCH.DeploymentTactics)]
        [CustomComboInfo("展开战术", "队友没有 鼓舞 状态时，将 展开战术 替换为 鼓舞激励之策。", SCH.JobID, 12)]
        SCH_DeploymentTactics = 16600,

        [ParentCombo(SCH_DeploymentTactics)]
        [CustomComboInfo("秘策", "当 秘策 冷却完毕且队友没有鼓舞状态时将 鼓舞激励之策 替换为 秘策。", SCH.JobID)]
        SCH_DeploymentTactics_Recitation = 16610,

        [Variant]
        [VariantParent(SCH_DPS_Bio, SCH_AoE)]
        [CustomComboInfo("精神镖选项", "当没有dot或dot剩余时间少于3s时，使用多变精神镖", SCH.JobID)]
        SCH_DPS_Variant_SpiritDart = 16700,

        [Variant]
        [VariantParent(SCH_DPS, SCH_AoE)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", SCH.JobID)]
        SCH_DPS_Variant_Rampart = 16800,

        #endregion

        // Last value = 16800

        #endregion

        #region SUMMONER

        [ReplaceSkill(All.Sleep)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            SMN.JobID, -10, "", ""
        )]
        SMN_Advanced_CustomMode = 170000,

        [ReplaceSkill(SMN.Ruin, SMN.Ruin2, SMN.Outburst, SMN.Tridisaster)]
        [CustomComboInfo
        (
            "高级自定义循环开关",
            "Advanced combo features for a greater degree of customisation.\nAccommodates SpS builds.\nRuin III is left unchanged for mobility purposes.",
            SMN.JobID, 1, "", ""
        )]
        SMN_Advanced_Combo = 17000,


        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("亚灵神替换", "Adds Deathflare, Ahk Morn and Revelation to the single target and AoE combos.", SMN.JobID, 11, "", "")]
        SMN_Advanced_Combo_DemiSummons_Attacks = 17002,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo
        (
            "宝石耀&宝石辉替换", "Adds Gemshine and Precious Brilliance to the single target and AoE combos, respectively.", SMN.JobID, 4, "",
            ""
        )]
        SMN_Advanced_Combo_EgiSummons_Attacks = 17004,


        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("螺旋气流-迦楼罗替换", "螺旋气流 可用时替换 毁荡和三重灾祸", SMN.JobID, 6, "", "")]
        SMN_Garuda_Slipstream = 17005,


        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("深红旋风-伊芙利特替换", "深红旋风 深红强袭 可用时替换 毁荡和三重灾祸", SMN.JobID, 7, "", "")]
        SMN_Ifrit_Cyclone = 17006,


        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("山崩-泰坦替换", "山崩 可用时替换 毁荡和三重灾祸", SMN.JobID, 5, "", "")]
        SMN_Titan_MountainBuster = 17007,

        [ReplaceSkill(SMN.溃烂爆发Fester)]
        [CustomComboInfo("能量吸收替换", "当自身没有以太超流时 用能量吸收替换溃烂爆发。", SMN.JobID, 6, "", "")]
        SMN_EDFester = 17008,

        [ReplaceSkill(SMN.痛苦核爆Painflare)]
        [CustomComboInfo("能量抽取替换", "当自身没有以太超流时 用能量抽取替换痛苦核爆", SMN.JobID, 7, "", "")]
        SMN_ESPainflare = 17009,

        // BONUS TWEAKS
        [CustomComboInfo("宝石兽自动召唤", "当没有宝石兽自动召唤宝石兽", SMN.JobID, 8, "", "")]
        SMN_CarbuncleReminder = 17010,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo
        (
            "循环加入毁绝",
            "Adds Ruin IV to the single target and AoE combos.\nUses when moving during Garuda Phase and you have no attunement, when moving during Ifrit phase, or when you have no active Egi or Demi summon.",
            SMN.JobID, 0, "", ""
        )]
        SMN_Advanced_Combo_Ruin4 = 17011,

        [ParentCombo(SMN_EDFester)]
        [CustomComboInfo("毁绝替换", "当没有以太超流层数时 自身处于毁绝准备中 并且能量吸收进入冷却，溃烂爆发技能变为毁绝，", SMN.JobID, 0, "", "")]
        SMN_EDFester_Ruin4 = 17013,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入能量吸收和能量抽取", "将能量系技能在冷却完毕后加入到循环和AOE循环中", SMN.JobID, 1, "", "")]
        SMN_Advanced_Combo_EDFester = 17014,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入三神召唤", "将三神召唤加入到循环和AOE循环中\n可选择召唤顺序.\n默认采用泰坦优先.", SMN.JobID, 3, "", "")]
        SMN_DemiEgiMenu_EgiOrder = 17016,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入灼热之光", "在循环和AOE循环中加入灼热之光\n冷却完毕立刻使用", SMN.JobID, 9, "", "")]
        SMN_SearingLight = 17018,

        [ParentCombo(SMN_SearingLight)]
        [CustomComboInfo("灼热之光爆发设置", "只在亚神召唤中使用灼热之光\n进阶选项在 高级能力技选项下\n不适用咏速套装", SMN.JobID, 0, "")]
        SMN_SearingLight_Burst = 170181, // Genesis, why must you be like this -K

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环中加入亚神召唤", "循环和AOE循环中加入 龙神召唤 不死鸟召唤", SMN.JobID, 10, "", "")]
        SMN_Advanced_Combo_DemiSummons = 17020,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("即刻咏唱三神技能选项", "使用三神技能时自动使用即刻", SMN.JobID, 8, "", "")]
        SMN_DemiEgiMenu_SwiftcastEgi = 17023,

        [CustomComboInfo("亚神技能替换亚神召唤", "龙神迸发 不死鸟迸发 死星核爆 可用时替换掉 对应的亚神召唤", SMN.JobID, 11, "", "")]
        SMN_DemiAbilities = 17024,

        [ParentCombo(SMN_Advanced_Combo_EDFester)]
        [CustomComboInfo("爆发选项", "只在选定条件下中使用溃烂爆发/痛苦核爆", SMN.JobID, 1, "", "")]
        SMN_DemiEgiMenu_oGCDPooling = 17025,

        [ConflictingCombos(ALL_Caster_Raise)]
        [CustomComboInfo("替代性的复活功能", "当即刻冷却完毕 替换 复生", SMN.JobID, 8, "", "")]
        SMN_Raise = 17027,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入苏生之炎", "循环 AOE循环中 加入苏生之炎", SMN.JobID, 13, "", "")]
        SMN_Advanced_Combo_DemiSummons_Rekindle = 17028,

        [ReplaceSkill(SMN.毁绝Ruin4)]
        [CustomComboInfo("毁荡走位设置", "当你没有毁绝预备buff时，用毁荡替换毁绝。", SMN.JobID, 9, "", "")]
        SMN_RuinMobility = 17030,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入醒梦", "当MP低于设定值时，将醒梦加入到循环中", SMN.JobID, 2, "", "")]
        SMN_Lucid = 17031,

        [CustomComboInfo("三神星极超流技替换", "三神星极超流技可用时 替换 三神召唤技能", SMN.JobID, 12, "", "")]
        SMN_Egi_AstralFlow = 17034,

        [ParentCombo(SMN_SearingLight)]
        [CustomComboInfo("只在单循环中使用", "禁止此功能在AOE循环中使用", SMN.JobID, 2, "", "")]
        SMN_SearingLight_STOnly = 17036,


        [ParentCombo(SMN_DemiEgiMenu_oGCDPooling)]
        [CustomComboInfo("只在单循环中使用", "禁止此功能在AOE循环中使用", SMN.JobID, 3, "", "")]
        SMN_DemiEgiMenu_oGCDPooling_Only = 17037,


        [ParentCombo(SMN_DemiEgiMenu_SwiftcastEgi)]
        [CustomComboInfo("只在单循环中使用", "禁止此功能在AOE循环中使用", SMN.JobID, 2, "", "")]
        SMN_DemiEgiMenu_SwiftcastEgi_Only = 17038,


        [ParentCombo(SMN_ESPainflare)]
        [CustomComboInfo
        (
            "毁绝替换痛苦核爆", "Changes Painflare to Ruin IV when out of Aetherflow stacks, Energy Siphon is on cooldown, and Ruin IV is up.",
            SMN.JobID, 0, "", ""
        )]
        SMN_ESPainflare_Ruin4 = 17039,


        [ParentCombo(SMN_Ifrit_Cyclone)]
        [CustomComboInfo
        (
            "深红旋风进阶", "Only uses Crimson Cyclone if you are not moving, or have no remaining Ifrit Attunement charges.", SMN.JobID, 0,
            "", ""
        )]
        SMN_Ifrit_Cyclone_Option = 17040,


        // [ParentCombo(SMN_DemiEgiMenu_oGCDPooling)] [CustomComboInfo("爆发延迟开关", "自定义爆发延迟设置\n适用于咏速套装", SMN.JobID, 2, "", "")]
        // SMN_Advanced_Burst_Delay_Option = 17043,


        [ParentCombo(SMN_DemiEgiMenu_oGCDPooling)]
        [CustomComboInfo("灼热之光防顶", "检查任何 灼热之光 状态，而不仅仅是你自己的。\n如果有多个召唤师并担心你的灼热之光被覆盖，请使用此选项。", SMN.JobID, 1, "", "")]
        SMN_Advanced_Burst_Any_Option = 17044,

        [Variant]
        [VariantParent(SMN_Advanced_Combo)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", SMN.JobID)]
        SMN_Variant_Rampart = 17045,

        [Variant]
        [VariantParent(SMN_Raise)]
        [CustomComboInfo("复活 选项", "当你有即刻BUFF时，替换即刻为成多变复活", SMN.JobID)]
        SMN_Variant_Raise = 17046,

        [Variant]
        [VariantParent(SMN_Advanced_Combo)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", SMN.JobID)]
        SMN_Variant_Cure = 17047,

        // Last value = 17047 (170181)

        #endregion

        #region VIPER

        [ReplaceSkill(VPR.SteelFangs)]
        [ConflictingCombos(VPR_ST_AdvancedMode)]
        [CustomComboInfo("Simple Mode - Single Target", "Replaces Steel Fangs with a full one-button single target rotation.\nThis is the ideal option for newcomers to the job.", VPR.JobID)]
        VPR_ST_SimpleMode = 30000,

        #region Advanced ST Viper

        [ReplaceSkill(VPR.SteelFangs)]
        [ConflictingCombos(VPR_ST_SimpleMode)]
        [CustomComboInfo("Advanced Mode - Single Target", "Replaces Steel Fangs with a full one-button single target rotation.\nThese features are ideal if you want to customize the rotation.", VPR.JobID)]
        VPR_ST_AdvancedMode = 30001,

        [ParentCombo(VPR_ST_AdvancedMode)]
        [ConflictingCombos(VPR_ReawakenLegacy)]
        [CustomComboInfo("Level 100 Opener", "Adds the Balance opener to the rotation.\n Does not check positional choice.\n Always does Hunter's Coil first ( FLANK )", VPR.JobID)]
        VPR_ST_Opener = 30002,

        #region Cooldowns ST

        [ParentCombo(VPR_ST_AdvancedMode)]
        [CustomComboInfo("Cooldowns Option", "Adds various cooldowns to the rotation.", VPR.JobID)]
        VPR_ST_CDs = 30004,

        [ParentCombo(VPR_ST_CDs)]
        [CustomComboInfo("Serpents Ire", "Adds Serpents Ire to the rotation.", VPR.JobID)]
        VPR_ST_SerpentsIre = 30005,

        [ParentCombo(VPR_ST_CDs)]
        [CustomComboInfo("Vicewinder", "Adds Vicewinder to the rotation.", VPR.JobID)]
        VPR_ST_Vicewinder = 30006,

        [ParentCombo(VPR_ST_CDs)]
        [CustomComboInfo("Vicewinder Combo", "Adds Swiftskin's Coil and Hunter's Coil to the rotation.", VPR.JobID)]
        VPR_ST_VicewinderCombo = 30007,
        
        [ParentCombo(VPR_ST_VicewinderCombo)]
        [CustomComboInfo("Dynamic First Coil", "Uses Hunter's Coil first if you are on the flank, and Swiftskin's Coil first if you are on the rear.\nDefaults to your selection if you are elsewhere.", VPR.JobID)]
        VPR_ST_DynamicFirstCoil = 30013,
        #endregion

        [ParentCombo(VPR_ST_AdvancedMode)]
        [CustomComboInfo("Serpents Tail", "Adds Serpents Tail to the rotation.", VPR.JobID)]
        VPR_ST_SerpentsTail = 30008,

        [ParentCombo(VPR_ST_AdvancedMode)]
        [CustomComboInfo("Uncoiled Fury", "Adds Uncoiled Fury to the rotation.", VPR.JobID)]
        VPR_ST_UncoiledFury = 30009,

        [ParentCombo(VPR_ST_AdvancedMode)]
        [CustomComboInfo("Uncoiled Fury Combo", "Adds Uncoiled Twinfang and Uncoiled Twinblood to the rotation.", VPR.JobID)]
        VPR_ST_UncoiledFuryCombo = 30010,

        [ParentCombo(VPR_ST_AdvancedMode)]
        [ConflictingCombos(VPR_ReawakenLegacy)]
        [CustomComboInfo("Reawaken", "Adds Reawaken to the rotation.", VPR.JobID)]
        VPR_ST_Reawaken = 30011,

        [ParentCombo(VPR_ST_AdvancedMode)]
        [ConflictingCombos(VPR_ReawakenLegacy)]
        [CustomComboInfo("Reawaken Combo", "Adds Generation and Legacy to the rotation.", VPR.JobID)]
        VPR_ST_ReawakenCombo = 30012,

        [ParentCombo(VPR_ST_AdvancedMode)]
        [CustomComboInfo("Ranged Uptime Option", "Adds Writhing Snap to the rotation when you are out of melee range.", VPR.JobID)]
        VPR_ST_RangedUptime = 30095,

        [ParentCombo(VPR_ST_RangedUptime)]
        [CustomComboInfo("Add Uncoiled Fury", "Adds Uncoiled Fury to the rotation when you are out of melee range and have Rattling Coil charges.", VPR.JobID)]
        VPR_ST_RangedUptimeUncoiledFury = 30096,

        [ParentCombo(VPR_ST_AdvancedMode)]
        [CustomComboInfo("Combo Heals Option", "Adds Bloodbath and Second Wind to the rotation.", VPR.JobID)]
        VPR_ST_ComboHeals = 30097,

        [ParentCombo(VPR_ST_AdvancedMode)]
        [CustomComboInfo("Dynamic True North Option", "Adds True North when you are not in the correct position for the enhanced potency bonus..\n Only for basic combo. Does NOT care about Vicewinder positionals.", VPR.JobID)]
        VPR_TrueNorthDynamic = 30098,

        [ParentCombo(VPR_TrueNorthDynamic)]
        [CustomComboInfo("Hold True North for Vicewinder", "Will hold the last charge of True North for use with Vicewinder, even when out of position for other Positionals.", VPR.JobID)]
        VPR_TrueNorthDynamic_HoldCharge = 30099,

        #endregion

        [ReplaceSkill(VPR.SteelMaw)]
        [ConflictingCombos(VPR_AoE_AdvancedMode)]
        [CustomComboInfo("Simple Mode - AoE", "Replaces Steel Maw with a full one-button AoE rotation.\nThis is the ideal option for newcomers to the job.", VPR.JobID)]
        VPR_AoE_SimpleMode = 30100,

        #region Advanced AoE Viper

        [ReplaceSkill(VPR.SteelMaw)]
        [ConflictingCombos(VPR_AoE_SimpleMode)]
        [CustomComboInfo("Advanced Mode - AoE", "Replaces Steel Maw with a full one-button AoE rotation.\nThese features are ideal if you want to customize the rotation.", VPR.JobID)]
        VPR_AoE_AdvancedMode = 30101,

        #region Cooldowns AoE

        [ParentCombo(VPR_AoE_AdvancedMode)]
        [CustomComboInfo("Cooldowns Option", "Adds various cooldowns to the rotation.", VPR.JobID)]
        VPR_AoE_CDs = 30103,

        [ParentCombo(VPR_AoE_CDs)]
        [CustomComboInfo("Serpents Ire", "Adds Serpents Ire to the rotation.", VPR.JobID)]
        VPR_AoE_SerpentsIre = 30104,

        [ParentCombo(VPR_AoE_CDs)]
        [CustomComboInfo("Vicepit", "Adds Vicepit to the rotation.", VPR.JobID)]
        VPR_AoE_Vicepit = 30105,

        [ParentCombo(VPR_AoE_CDs)]
        [CustomComboInfo("Vicepit Combo", "Adds Swiftskin's Den and Hunter's Den to the rotation.", VPR.JobID)]
        VPR_AoE_VicepitCombo = 30106,

        #endregion

        [ParentCombo(VPR_AoE_AdvancedMode)]
        [CustomComboInfo("Serpents Tail", "Adds Serpents Tail to the rotation.", VPR.JobID)]
        VPR_AoE_SerpentsTail = 30107,

        [ParentCombo(VPR_AoE_AdvancedMode)]
        [CustomComboInfo("Uncoiled Fury", "Adds Uncoiled Fury to the rotation.", VPR.JobID)]
        VPR_AoE_UncoiledFury = 30108,

        [ParentCombo(VPR_AoE_AdvancedMode)]
        [CustomComboInfo("Uncoiled Fury Combo", "Adds Uncoiled Twinfang and Uncoiled Twinblood to the rotation.", VPR.JobID)]
        VPR_AoE_UncoiledFuryCombo = 30109,

        [ParentCombo(VPR_AoE_AdvancedMode)]
        [ConflictingCombos(VPR_ReawakenLegacy)]
        [CustomComboInfo("Reawaken", "Adds Reawaken to the rotation.", VPR.JobID)]
        VPR_AoE_Reawaken = 30110,

        [ParentCombo(VPR_AoE_AdvancedMode)]
        [ConflictingCombos(VPR_ReawakenLegacy)]
        [CustomComboInfo("Reawaken Combo", "Adds Generation and Legacy to the rotation.", VPR.JobID)]
        VPR_AoE_ReawakenCombo = 30112,

        [ParentCombo(VPR_AoE_AdvancedMode)]
        [CustomComboInfo("Combo Heals Option", "Adds Bloodbath and Second Wind to the rotation.", VPR.JobID)]
        VPR_AoE_ComboHeals = 30199,

        #endregion

        [ReplaceSkill(VPR.Vicewinder)]
        [CustomComboInfo("Vicewinder - Coils", "Replaces Vicewinder with Hunter's/Swiftskin's Coils.", VPR.JobID)]
        VPR_VicewinderCoils = 30200,
        
        [ParentCombo(VPR_VicewinderCoils)]
        [CustomComboInfo("Dynamic First Coil", "Uses Hunter's Coil first if you are on the flank, and Swiftskin's Coil first if you are on the rear.\nDefaults to your selection if you are elsewhere.", VPR.JobID)]
        VPR_DynamicFirstCoil = 30209,
        
        [ParentCombo(VPR_VicewinderCoils)]
        [CustomComboInfo("Dynamic True North Option", "Adds True North when you are not in the correct position for the enhanced potency bonus.", VPR.JobID)]
        VPR_VicewinderCoilsTN = 30208,

        [ReplaceSkill(VPR.Vicepit)]
        [CustomComboInfo("Vicepit - Dens", "Replaces Vicepit with Hunter's/Swiftskin's Dens.", VPR.JobID)]
        VPR_VicepitDens = 30201,

        [ReplaceSkill(VPR.UncoiledFury)]
        [CustomComboInfo("Uncoiled - Twins", "Replaces Uncoiled Fury with Uncoiled Twinfang and Uncoiled Twinblood.", VPR.JobID)]
        VPR_UncoiledTwins = 30202,

        [ReplaceSkill(VPR.Reawaken, VPR.SteelFangs)]
        [ConflictingCombos(VPR_ST_Reawaken, VPR_ST_ReawakenCombo, VPR_AoE_Reawaken, VPR_AoE_ReawakenCombo, VPR_ST_Opener)]
        [CustomComboInfo("Reawaken - Generation", "Replaces Option with the Generations.", VPR.JobID)]
        VPR_ReawakenLegacy = 30203,

        [ParentCombo(VPR_ReawakenLegacy)]
        [CustomComboInfo("Reawaken - Legacy", "Replaces Option with the Legacys.", VPR.JobID)]
        VPR_ReawakenLegacyWeaves = 30204,

        [ReplaceSkill(VPR.SerpentsTail)]
        [CustomComboInfo("Combined Combo Ability Feature", "Combines Serpent's Tail, Twinfang, and Twinblood to one button.", VPR.JobID)]
        VPR_TwinTails = 30205,

        [ParentCombo(VPR_VicewinderCoils)]
        [CustomComboInfo("Include Twin Combo Actions", "Adds Twinfang and Twinblood to the button.", VPR.JobID)]
        VPR_VicewinderCoils_oGCDs = 30206,

        [ParentCombo(VPR_VicepitDens)]
        [CustomComboInfo("Include Twin Combo Actions", "Adds Twinfang and Twinblood to the button.", VPR.JobID)]
        VPR_VicepitDens_oGCDs = 30207,
        
        #endregion
        #region WARRIOR

        [ReplaceSkill(WAR.Maim)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            WAR.JobID, -10, "", ""
        )]
        WAR_Advanced_CustomMode = 180001,

        [ReplaceSkill(WAR.StormsPath)]
        [CustomComboInfo("暴风斩连击", "战士主连击整合，加入暴风斩/碎.\n如果下面的所有子选项和裂石飞环/地毁人亡都被打开并加以设定，会形成一个完整的一键连击（高级战士，有一定智力的战士0.0）.", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath = 18000,

        [ReplaceSkill(WAR.StormsEye)]
        [CustomComboInfo("暴风碎连击", "将暴风碎替换为暴风碎连击", WAR.JobID, 0, "", "")]
        War_ST_StormsEye = 18001,

        [ReplaceSkill(WAR.Overpower)]
        [CustomComboInfo("超压斧连击", "将超压斧替换为AOE连击", WAR.JobID, 0, "", "")]
        WAR_AoE_Overpower = 18002,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("兽魂监控", "如果兽魂快溢出了，将消耗兽魂的技能加入到暴风斩连击和AOE连击", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_OvercapProtection = 18003,

        [ReplaceSkill(WAR.NascentFlash)]
        [CustomComboInfo("原初的勇猛", "在同步到76级以下时，将原初的勇猛替换为原初的直觉.", WAR.JobID, 0, "", "")]
        WAR_NascentFlash = 18005,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("动乱", "在有暴风碎BUFF时，将动乱加入到暴风斩连击", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_Upheaval = 18007,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("原初激震", "在有原初激震BUFF时，将原初激震加入到暴风斩连击", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_原初激震 = 180071,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("蛮荒崩裂", "在蛮荒崩裂预备状态下，将裂石飞环/地毁人亡替换为蛮荒崩裂(也加入AOE连击)", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_PrimalRend = 18008,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("破坏斧", "在破坏斧状态下，将破坏斧入到暴风斩连击", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_破坏斧 = 180081,

        [ParentCombo(WAR_AoE_Overpower)]
        [CustomComboInfo("群山隆起", "在有暴风碎BUFF时，将群山隆起加入到AOE连击", WAR.JobID, 0, "", "")]
        WAR_AoE_Overpower_Orogeny = 18009,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo
        (
            "裂石飞环/地毁人亡", "当兽魂高于50及以上时，将 裂石飞环 加入主连击，地毁人亡 加入AoE连击。\n战嚎时将使用 狂魂/混沌旋风，原初的解放时将使用 裂石飞环/钢铁旋风。\n当 超过设置的仪表盘会释放资源。", WAR.JobID, 0,
            "", ""
        )]
        WAR_ST_StormsPath_FellCleave = 18011,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("猛攻", "在有暴风碎BUFF时，将猛攻加入到暴风斩连击", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_Onslaught = 18012,

        [ParentCombo(WAR_AoE_Overpower)]
        [CustomComboInfo("战嚎 AoE", "在兽魂不低于50的时候将战壕加入AOE连击.", WAR.JobID, 0, "", "")]
        WAR_AoE_Overpower_Infuriate = 18013,

        [ParentCombo(WAR_AoE_Overpower)]
        [CustomComboInfo("原初的解放 AoE", "将原初的解放加入到暴风斩连击", WAR.JobID, 0, "", "")]
        WAR_AoE_Overpower_InnerRelease = 18014,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("飞斧", "在如果离boss太远，将飞斧加入到暴风斩连击.", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_RangedUptime = 18016,


        [ReplaceSkill(WAR.裂石飞环FellCleave, WAR.Decimate)]
        [CustomComboInfo("战壕与裂石飞环/地毁人亡整合", "小于等于设置距离时，将裂石飞环/地毁人亡整合到战壕", WAR.JobID, 0, "", "")]
        WAR_InfuriateFellCleave = 18018,

        [ReplaceSkill(WAR.原初的解放InnerRelease)]
        [CustomComboInfo("蛮荒崩裂Main combo", "在蛮荒崩裂预备状态下，将原初的解放替换为蛮荒崩裂.", WAR.JobID, 0, "", "")]
        WAR_PrimalRend_InnerRelease = 18019,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("原初的解放加入暴风斩循环", "将原初的解放加入到暴风斩连击", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_InnerRelease = 18020,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("战壕加入暴风斩循环", "在兽魂不低于50的时候将战壕加入暴风斩连击.", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_Infuriate = 18021,

        [ParentCombo(WAR_InfuriateFellCleave)]
        [CustomComboInfo("原初的解放 优先选项", "当 原初的解放 可用时，不使用 战嚎。", WAR.JobID, 0, "", "")]
        WAR_InfuriateFellCleave_IRFirst = 18022,

        [ParentCombo(WAR_ST_StormsPath_PrimalRend)]
        [CustomComboInfo("蛮荒崩裂 近战选项", "在目标圈内（1米）时并且不移动时使用 蛮荒崩裂，否则在 蛮荒崩裂 剩余时间小于10秒时使用。", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_PrimalRend_CloseRange = 18023,

        [ParentCombo(WAR_ST_StormsPath_Onslaught)]
        [CustomComboInfo("猛攻", "在 战场风暴 状态下以及在目标环（1m）内并且不移动时使用 猛攻。 \n将使用上面滑块中选择的层数。", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_Onslaught_MeleeSpender = 18024,

        [Variant]
        [VariantParent(WAR_ST_StormsPath, WAR_AoE_Overpower)]
        [CustomComboInfo("精神镖选项", "当没有dot或dot剩余时间少于3s时，使用多变精神镖", WAR.JobID)]
        WAR_Variant_SpiritDart = 18025,

        [Variant]
        [VariantParent(WAR_ST_StormsPath, WAR_AoE_Overpower)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", WAR.JobID)]
        WAR_Variant_Cure = 18026,

        [Variant]
        [VariantParent(WAR_ST_StormsPath, WAR_AoE_Overpower)]
        [CustomComboInfo("最后通牒 选项", "冷却结束时使用多变最后通牒", WAR.JobID)]
        WAR_Variant_Ultimatum = 18027,

        [ParentCombo(WAR_AoE_Overpower)]
        [CustomComboInfo("Steel Cyclone / Decimate Option", "Adds Steel Cyclone / Decimate to Advanced Mode.", WAR.JobID)]
        WAR_AoE_Overpower_Decimate = 18028,

        // Last value = 18027

        #endregion

        #region WHITE MAGE

        [ReplaceSkill(All.Repose)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            WHM.JobID, -10, "", ""
        )]
        WHM_Advanced_CustomMode = 190999,


        #region Single Target DPS Feature

        [ReplaceSkill(WHM.Stone1, WHM.Stone2, WHM.Stone3, WHM.Stone4, WHM.Glare1, WHM.Glare3)]
        [CustomComboInfo("Single Target DPS Feature", "Collection of cooldowns and spell features on Glare/Stone.", WHM.JobID, 1, "", "")]
        WHM_ST_MainCombo = 19099,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("Opener Option", "Use the 神速咏唱 opener", WHM.JobID, 11, "", "")]
        WHM_ST_MainCombo_Opener = 19023,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("Aero/Dia Uptime Option", "Adds Aero/Dia to the single target combo if the debuff is not present on current target, or is about to expire.", WHM.JobID, 12, "", "")]
        WHM_ST_MainCombo_DoT = 19013,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo
        (
            "闪炽-快速咏唱续剑",
            "闪炽加入循环 移动或者剩余时间不够的时候", WHM.JobID, 12, "", ""
        )]
        WHM_ST_MainCombo_Glare4 = 19018,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("Assize Option", "Adds Assize to the single target combo.", WHM.JobID, 13, "", "")]
        WHM_ST_MainCombo_Assize = 19009,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo
        (
            "Afflatus Misery Option", "Adds Afflatus Misery to the single target combo when it is ready to be used.", WHM.JobID, 14, "",
            ""
        )]
        WHM_ST_MainCombo_Misery_oGCD = 19017,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo
        (
            "Lily Overcap Protection Option", "Adds Afflatus Rapture to the single target combo when at three Lilies.", WHM.JobID, 15,
            "", ""
        )]
        WHM_ST_MainCombo_LilyOvercap = 19016,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("Presence of Mind Option", "Adds Presence of Mind to the single target combo.", WHM.JobID, 16, "", "")]
        WHM_ST_MainCombo_PresenceOfMind = 19008,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("Lucid Dreaming Option", "Adds Lucid Dreaming to the single target combo when below set MP value.", WHM.JobID, 17, "", "")]
        WHM_ST_MainCombo_Lucid = 19006,

        #endregion

        #region AoE DPS Feature

        [ReplaceSkill(WHM.Holy, WHM.Holy3)]
        [CustomComboInfo("AoE DPS Feature", "Collection of cooldowns and spell features on Holy/Holy III.", WHM.JobID, 2, "", "")]
        WHM_AoE_DPS = 19190,

        [ParentCombo(WHM_AoE_DPS)]
        [CustomComboInfo("Assize Option", "Adds Assize to the AoE combo.", WHM.JobID, 21, "", "")]
        WHM_AoE_DPS_Assize = 19192,

        [ParentCombo(WHM_AoE_DPS)]
        [CustomComboInfo("Afflatus Misery Option", "Adds Afflatus Misery to the AoE combo when it is ready to be used.", WHM.JobID, 22, "", "")]
        WHM_AoE_DPS_Misery = 19194,

        [ParentCombo(WHM_AoE_DPS)]
        [CustomComboInfo("Lily Overcap Protection Option", "Adds Afflatus Rapture to the AoE combo when at three Lilies.", WHM.JobID, 23, "", "")]
        WHM_AoE_DPS_LilyOvercap = 19193,

        [ParentCombo(WHM_AoE_DPS)]
        [CustomComboInfo
        (
            "Presence of Mind Option", "Adds Presence of Mind to the AoE combo if you are moving or it can be weaved without GCD delay.",
            WHM.JobID, 24, "", ""
        )]
        WHM_AoE_DPS_PresenceOfMind = 19195,

        [ParentCombo(WHM_AoE_DPS)]
        [CustomComboInfo
        (
            "Lucid Dreaming Option",
            "Adds Lucid Dreaming to the AoE combo when below the set MP value if you are moving or it can be weaved without GCD delay.", WHM.JobID,
            25, "", ""
        )]
        WHM_AoE_DPS_Lucid = 19191,

        #endregion

        [ReplaceSkill(WHM.AfflatusSolace)]
        [CustomComboInfo
        (
            "Solace into Misery Feature", "Replaces Afflatus Solace with Afflatus Misery when it is ready to be used.", WHM.JobID, 30,
            "", ""
        )]
        WHM_SolaceMisery = 19000,

        [ReplaceSkill(WHM.AfflatusRapture)]
        [CustomComboInfo
        (
            "Rapture into Misery Feature", "Replaces Afflatus Rapture with Afflatus Misery when it is ready to be used.", WHM.JobID, 40,
            "", ""
        )]
        WHM_RaptureMisery = 19001,

        #region AoE Heals Feature

        [ReplaceSkill(WHM.Medica)]
        [CustomComboInfo("Simple Heals (AoE)", "Replaces Medica with a one button AoE healing setup.", WHM.JobID, 4, "", "")]
        WHM_AoEHeals = 19007,

        [ParentCombo(WHM_AoEHeals)]
        [CustomComboInfo("Afflatus Rapture Option", "Uses Afflatus Rapture when available.", WHM.JobID, 61, "", "")]
        WHM_AoEHeals_Rapture = 19011,

        [ParentCombo(WHM_AoEHeals)]
        [CustomComboInfo("Afflatus Misery Option", "Uses Afflatus Misery when available.", WHM.JobID, 62, "", "")]
        WHM_AoEHeals_Misery = 19010,

        [ParentCombo(WHM_AoEHeals)]
        [CustomComboInfo("Thin Air Option", "Uses Thin Air when available.", WHM.JobID, 63, "", "")]
        WHM_AoeHeals_ThinAir = 19200,

        [ParentCombo(WHM_AoEHeals)]
        [CustomComboInfo("Cure III Option", "Replaces Medica with Cure III when available.", WHM.JobID)]
        WHM_AoEHeals_Cure3 = 19201,

        [ParentCombo(WHM_AoEHeals)]
        [CustomComboInfo("Assize Option", "Uses Assize when available.", WHM.JobID)]
        WHM_AoEHeals_Assize = 19202,

        [ParentCombo(WHM_AoEHeals)]
        [CustomComboInfo("Plenary Indulgence Option", "Uses Plenary Indulgence when available.", WHM.JobID)]
        WHM_AoEHeals_Plenary = 19203,

        [ParentCombo(WHM_AoEHeals)]
        [CustomComboInfo("Lucid Dreaming Option", "Uses Lucid Dreaming when available.", WHM.JobID)]
        WHM_AoEHeals_Lucid = 19204,

        #endregion

        #region Single Target Heals

        [ReplaceSkill(WHM.Cure)]
        [CustomComboInfo("Simple Heals (Single Target)", "Replaces Cure with a one button single target healing setup.", WHM.JobID, 3)]
        WHM_STHeals = 19300,

        [ParentCombo(WHM_STHeals)]
        [CustomComboInfo("Regen Option", "Applies Regen to the target if missing.", WHM.JobID)]
        WHM_STHeals_Regen = 19301,

        [ParentCombo(WHM_STHeals)]
        [CustomComboInfo("Benediction Option", "Uses Benediction when target is below HP threshold.", WHM.JobID)]
        WHM_STHeals_Benediction = 19302,

        [ParentCombo(WHM_STHeals)]
        [CustomComboInfo("Afflatus Solace Option", "Uses Afflatus Solace when available.", WHM.JobID)]
        WHM_STHeals_Solace = 19303,

        [ParentCombo(WHM_STHeals)]
        [CustomComboInfo("Thin Air Option", "Uses Thin Air when available.", WHM.JobID)]
        WHM_STHeals_ThinAir = 19304,

        [ParentCombo(WHM_STHeals)]
        [CustomComboInfo("Tetragrammaton Option", "Uses Tetragrammaton when available.", WHM.JobID)]
        WHM_STHeals_Tetragrammaton = 19305,

        [ParentCombo(WHM_STHeals)]
        [CustomComboInfo("Divine Benison Option", "Uses Divine Benison when available.", WHM.JobID)]
        WHM_STHeals_Benison = 19306,

        [ParentCombo(WHM_STHeals)]
        [CustomComboInfo("Aqualveil Option", "Uses Aquaveil when available.", WHM.JobID)]
        WHM_STHeals_Aquaveil = 19307,

        [ParentCombo(WHM_STHeals)]
        [CustomComboInfo("Lucid Dreaming Option", "Uses Lucid Dreaming when available.", WHM.JobID)]
        WHM_STHeals_Lucid = 19308,

        [ParentCombo(WHM_STHeals)]
        [CustomComboInfo("Esuna Option", "Applies Esuna to your target if there is a cleansable debuff.", WHM.JobID)]
        WHM_STHeals_Esuna = 19309,

        #endregion

        [ReplaceSkill(WHM.Cure2)]
        [CustomComboInfo("Cure II Sync Feature", "Changes Cure II to Cure when synced below Lv.30.", WHM.JobID, 70, "", "")]
        WHM_CureSync = 19002,

        [ReplaceSkill(All.Swiftcast)]
        [ConflictingCombos(ALL_Healer_Raise)]
        [CustomComboInfo("Alternative Raise Feature", "Changes Swiftcast to Raise.", WHM.JobID, 80, "", "")]
        WHM_Raise = 19004,

        [ReplaceSkill(WHM.Raise)]
        [ParentCombo(WHM_Raise)]
        [CustomComboInfo("Thin Air Raise Feature", "Adds Thin Air to the Global Raise Feature/Alternative Raise Feature.", WHM.JobID, 90, "", "")]
        WHM_ThinAirRaise = 19014,

        [Variant]
        [VariantParent(WHM_ST_MainCombo_DoT, WHM_AoE_DPS)]
        [CustomComboInfo("Spirit Dart Option", "Use Variant Spirit Dart whenever the debuff is not present or less than 3s.", WHM.JobID)]
        WHM_DPS_Variant_SpiritDart = 19025,

        [Variant]
        [VariantParent(WHM_ST_MainCombo, WHM_AoE_DPS)]
        [CustomComboInfo("Rampart Option", "Use Variant Rampart on cooldown.", WHM.JobID)]
        WHM_DPS_Variant_Rampart = 19026,

        // Last value = 19027

        #endregion

        #region PICTOMANCER

        [ReplaceSkill(All.Repose)]
        [CustomComboInfo
        (
            "自定义循环",
            "自定义循环",
            PCT.JobID, -10, "", ""
        )]
        PCT_Advanced_CustomMode = 210000,

        [ReplaceSkill(PCT.短1)]
        [CustomComboInfo("一键模式 - 单目标", "将1替换为一键单目标连击。", PCT.JobID)]
        PCT_ST_EasyMode = 21010,


        [ReplaceSkill(PCT.AOE短1)]
        [CustomComboInfo("一键模式 - 多目标", "将aoe1替换为一键多目标连击。", PCT.JobID)]
        PCT_AOE_EasyMode = 21011,


        [ReplaceSkill(PCT.FireInRed)]
        [ConflictingCombos(CombinedAetherhues)]
        [CustomComboInfo("Simple Mode - Single Target", $"Replaces Fire in Red with a one-button full single target rotation.\nThis is ideal for newcomers to the job.", PCT.JobID)]
        PCT_ST_SimpleMode = 20000,

        [ReplaceSkill(PCT.FireIIinRed)]
        [ConflictingCombos(CombinedAetherhues)]
        [CustomComboInfo("Simple Mode - AoE", $"Replaces Fire II in Red with a one-button full single target rotation.\nThis is ideal for newcomers to the job.", PCT.JobID)]
        PCT_AoE_SimpleMode = 20001,

        [ReplaceSkill(PCT.FireInRed, PCT.FireIIinRed)]
        [ConflictingCombos(PCT_ST_SimpleMode, PCT_AoE_SimpleMode)]
        [CustomComboInfo("Combined Aetherhues Feature", "Combines aetherhue actions onto one button for their respective target types.", PCT.JobID)]
        CombinedAetherhues = 20002,

        [ReplaceSkill(PCT.CreatureMotif, PCT.WeaponMotif, PCT.LandscapeMotif)]
        [CustomComboInfo("One Button Motifs", "Combine Motifs and Muses into one button.", PCT.JobID)]
        CombinedMotifs = 20003,

        [ReplaceSkill(PCT.HolyInWhite)]
        [CustomComboInfo("One Button Paint", "Combines paint consuming actions into one button.", PCT.JobID)]
        CombinedPaint = 20004,

        #endregion

        // Non-combat

        #region DOH

        // [CustomComboInfo("Placeholder", "Placeholder.", DOH.JobID)]
        // DohPlaceholder = 50001,

        #endregion

        #region DOL

        [ReplaceSkill(DOL.AgelessWords, DOL.SolidReason)]
        [CustomComboInfo("理智同兴", "当理智同兴可用时，将农夫之智或石工之理替换为理智同兴", DOL.JobID)]
        DOL_Eureka = 51001,

        [ReplaceSkill(DOL.ArborCall, DOL.ArborCall2, DOL.LayOfTheLand, DOL.LayOfTheLand2)]
        [CustomComboInfo("[采矿工/园艺工] 定位和真相 选项", "如果没启用，则用 矿脉勘探/三角测量 替换 大地勘探/树木之声，山丘之相/丛林之相替换 大地勘探II/树木之声II。", DOL.JobID)]
        DOL_NodeSearchingBuffs = 51012,

        [ReplaceSkill(DOL.Cast)]
        [CustomComboInfo("抛竿", "正在钓鱼时，将抛竿替换为提钩", DOL.JobID)]
        FSH_CastHook = 51002,

        [CustomComboInfo("[捕鱼人] 水下 选项", "在水下时用水下能力取代捕鱼能力", DOL.JobID)]
        FSH_Swim = 51008,

        [ReplaceSkill(DOL.Cast)]
        [ParentCombo(FSH_Swim)]
        [CustomComboInfo("[捕鱼人] 抛竿 替换 刺鱼", "在水下时将 抛竿 替换为 刺鱼", DOL.JobID)]
        FSH_CastGig = 51003,

        [ReplaceSkill(DOL.SurfaceSlap)]
        [ParentCombo(FSH_Swim)]
        [CustomComboInfo("拍击水面 替换 熟能生巧", "在水下时将 拍击水面 替换为 熟能生巧", DOL.JobID)]
        FSH_SurfaceTrade = 51004,

        [ReplaceSkill(DOL.PrizeCatch)]
        [ParentCombo(FSH_Swim)]
        [CustomComboInfo("大鱼猎手 替换 嘉惠", "在水下时将 大鱼猎手 替换为 嘉惠", DOL.JobID)]
        FSH_PrizeBounty = 51005,

        [ReplaceSkill(DOL.Snagging)]
        [ParentCombo(FSH_Swim)]
        [CustomComboInfo("钓组 替换 打捞", "在水下时将 钓组 替换为 打捞", DOL.JobID)]
        FSH_SnaggingSalvage = 51006,

        [ReplaceSkill(DOL.CastLight)]
        [ParentCombo(FSH_Swim)]
        [CustomComboInfo("垂钓之光 替换 电水流", "在水下时将 垂钓之光 替换为 电水流", DOL.JobID)]
        FSH_CastLight_ElectricCurrent = 51007,

        [ReplaceSkill(DOL.Mooch, DOL.MoochII)]
        [ParentCombo(FSH_Swim)]
        [CustomComboInfo("以小钓大 替换 鲨鱼之眼", "在水下时将 以小钓大 替换为 鲨鱼之眼", DOL.JobID)]
        FSH_Mooch_SharkEye = 51009,

        [ReplaceSkill(DOL.FishEyes)]
        [ParentCombo(FSH_Swim)]
        [CustomComboInfo("鱼眼 替换 聚精鱼眼", "在水下时将 鱼眼 替换为 聚精鱼眼", DOL.JobID)]
        FSH_FishEyes_VitalSight = 51010,

        [ReplaceSkill(DOL.Chum)]
        [ParentCombo(FSH_Swim)]
        [CustomComboInfo("撒饵 替换 闭气", "在水下时将 撒饵 替换为 闭气", DOL.JobID)]
        FSH_Chum_BaitedBreath = 51011,

        // Last value = 51011

        #endregion

        #endregion

        #region PvP Combos

        #region PvP GLOBAL FEATURES

        [PvPCustomCombo]
        [CustomComboInfo
        (
            "Emergency Heals Feature", "Uses Recuperate when your HP is under the set threshold and you have sufficient MP.", ADV.JobID,
            1
        )]
        PvP_EmergencyHeals = 1100000,

        [PvPCustomCombo]
        [CustomComboInfo("Emergency Guard Feature", "Uses Guard when your HP is under the set threshold.", ADV.JobID, 2)]
        PvP_EmergencyGuard = 1100010,

        [PvPCustomCombo]
        [CustomComboInfo("Quick Purify Feature", "受到选定的debuff时使用净化", ADV.JobID, 4)]
        PvP_QuickPurify = 1100020,

        [PvPCustomCombo]
        [CustomComboInfo("Prevent Mash Cancelling Feature", "Stops you cancelling your guard if you're pressing buttons quickly.", ADV.JobID, 3)]
        PvP_MashCancel = 1100030,

        // Last value = 1100030
        // Extra 0 on the end keeps things working the way they should be. Nothing to see here.

        #endregion

        #region ASTROLOGIAN

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "Turns Fall Malefic into an all-in-one damage button.", AST.JobID)]
        ASTPvP_Burst = 111000,

        [ParentCombo(ASTPvP_Burst)]
        [CustomComboInfo("Double Cast Option", "Adds Double Cast to Burst Mode.", AST.JobID)]
        ASTPvP_DoubleCast = 111001,

        [ParentCombo(ASTPvP_Burst)]
        [CustomComboInfo("Card Option", "Adds Drawing and Playing Cards to Burst Mode.", AST.JobID)]
        ASTPvP_Card = 111002,

        [PvPCustomCombo]
        [CustomComboInfo("Double Cast Heal Feature", "Adds Double Cast to Aspected Benefic.", AST.JobID)]
        ASTPvP_Heal = 111003,

        // Last value = 111003

        #endregion

        #region BLACK MAGE

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "把火炎和冰结改为伤害combo按键", BLM.JobID)]
        BLMPvP_BurstMode = 112000,

        [ParentCombo(BLMPvP_BurstMode)]
        [PvPCustomCombo]
        [CustomComboInfo("Night Wing Option", "Adds Night Wing to Burst Mode.", BLM.JobID)]
        BLMPvP_BurstMode_NightWing = 112001,

        [ParentCombo(BLMPvP_BurstMode)]
        [PvPCustomCombo]
        [CustomComboInfo("Aetherial Manipulation Option", "爆发CD没好的时候，使用以太步拉近距离", BLM.JobID)]
        BLMPvP_BurstMode_AetherialManip = 112002,

        // Last value = 112002

        #endregion

        #region BARD

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "把强劲射击改为伤害combo按键", BRDPvP.JobID)]
        BRDPvP_BurstMode = 113000,

        // Last value = 113000

        #endregion

        #region DANCER

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "把喷泉连改成伤害combo按键", DNC.JobID)]
        DNCPvP_BurstMode = 114000,

        [PvPCustomCombo]
        [ParentCombo(DNCPvP_BurstMode)]
        [CustomComboInfo
        (
            "刃舞选项",
            "Adds Honing Dance to the main combo when in melee range (respects global offset).\nThis option prevents early use of Honing Ovation!\nKeep Honing Dance bound to another key if you want to end early.",
            DNC.JobID
        )]
        DNCPvP_BurstMode_HoningDance = 114001,

        [PvPCustomCombo]
        [ParentCombo(DNCPvP_BurstMode)]
        [CustomComboInfo("治疗之华尔兹选项", "Adds Curing Waltz to the combo when available, and your HP is at or below the set percentage.", DNC.JobID)]
        DNCPvP_BurstMode_CuringWaltz = 114002,

        // Last value = 114002

        #endregion

        #region DARK KNIGHT

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "Turns 噬魂斩连击 into an all-in-one damage button.", DRK.JobID)]
        DRKPvP_Burst = 115000,

        [PvPCustomCombo]
        [ParentCombo(DRKPvP_Burst)]
        [CustomComboInfo("Plunge Option", "Adds Plunge to Burst Mode.", DRK.JobID)]
        DRKPvP_Plunge = 115001,

        [PvPCustomCombo]
        [ParentCombo(DRKPvP_Plunge)]
        [CustomComboInfo("近战时使用跳斩 设置", "Uses Plunge whilst in melee range, and not just as a gap-closer.", DRK.JobID)]
        DRKPvP_PlungeMelee = 115002,

        // Last value = 115002

        #endregion

        #region DRAGOON

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "Using Elusive Jump turns Wheeling Thrust Combo into all-in-one burst damage button.", DRG.JobID)]
        DRGPvP_Burst = 116000,

        [ParentCombo(DRGPvP_Burst)]
        [CustomComboInfo("Geirskogul Option", "Adds Geirskogul to Burst Mode.", DRG.JobID)]
        DRGPvP_Geirskogul = 116001,

        [ParentCombo(DRGPvP_Geirskogul)]
        [CustomComboInfo("Nastrond Option", "Adds Nastrond to Burst Mode.", DRG.JobID)]
        DRGPvP_Nastrond = 116002,

        [ParentCombo(DRGPvP_Burst)]
        [CustomComboInfo("Horrid Roar Option", "Adds Horrid Roar to Burst Mode.", DRG.JobID)]
        DRGPvP_HorridRoar = 116003,

        [ParentCombo(DRGPvP_Burst)]
        [CustomComboInfo("Sustain Chaos Spring Option", "Adds Chaos Spring to 爆发模式 when below the set HP percentage.", DRG.JobID)]
        DRGPvP_ChaoticSpringSustain = 116004,

        [ParentCombo(DRGPvP_Burst)]
        [CustomComboInfo("天龙点睛选项", "Adds Wyrmwind Thrust to Burst Mode.", DRG.JobID)]
        DRGPvP_WyrmwindThrust = 116006,

        [ParentCombo(DRGPvP_Burst)]
        [CustomComboInfo("High Jump Weave Option", "Adds High Jump to Burst Mode.", DRG.JobID)]
        DRGPvP_HighJump = 116007,

        [ParentCombo(DRGPvP_Burst)]
        [CustomComboInfo("Elusive Jump Burst Protection Option", "Disables Elusive Jump if Burst is not ready.", DRG.JobID)]
        DRGPvP_BurstProtection = 116008,

        // Last value = 116008

        #endregion

        #region GUNBREAKER

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "Turns Solid Barrel Combo into an all-in-one damage button.", GNB.JobID)]
        GNBPvP_Burst = 117000,

        [ParentCombo(GNBPvP_Burst)]
        [CustomComboInfo("倍攻", "Adds Double Down to 爆发模式 while under the No Mercy buff.", GNB.JobID)]
        GNBPvP_DoubleDown = 117001,

        [PvPCustomCombo]
        [CustomComboInfo("Gnashing Fang Continuation Feature", "Adds Continuation onto Gnashing Fang.", GNB.JobID)]
        GNBPvP_GnashingFang = 117002,

        [ParentCombo(GNBPvP_Burst)]
        [CustomComboInfo("Draw And Junction Option", "Adds Draw And Junction to Burst Mode.", GNB.JobID)]
        GNBPvP_DrawAndJunction = 117003,

        [ParentCombo(GNBPvP_Burst)]
        [CustomComboInfo("Gnashing Fang Option", "Adds Gnashing Fang to 爆发模式 while under the No Mercy buff.", GNB.JobID)]
        GNBPvP_ST_GnashingFang = 117004,

        [ParentCombo(GNBPvP_Burst)]
        [CustomComboInfo("Continuation Option", "Adds Continuation to Burst Mode.", GNB.JobID)]
        GNBPvP_ST_Continuation = 117005,

        [ParentCombo(GNBPvP_Burst)]
        [CustomComboInfo("粗分斩设置", "Weaves Rough Divide when No Mercy Buff is about to expire.", GNB.JobID)]
        GNBPvP_RoughDivide = 117006,

        [ParentCombo(GNBPvP_Burst)]
        [CustomComboInfo("Junction Cast DPS Option", "Adds Junction Cast (DPS) to Burst Mode.", GNB.JobID)]
        GNBPvP_JunctionDPS = 117007,

        [ParentCombo(GNBPvP_Burst)]
        [CustomComboInfo("Junction Cast Healer Option", "Adds Junction Cast (Healer) to Burst Mode.", GNB.JobID)]
        GNBPvP_JunctionHealer = 117008,

        [ParentCombo(GNBPvP_Burst)]
        [CustomComboInfo("Junction Cast Tank Option", "Adds Junction Cast (Tank) to Burst Mode.", GNB.JobID)]
        GNBPvP_JunctionTank = 117009,

        // Last value = 117009

        #endregion

        #region MACHINIST

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "把蓄力冲击改为伤害combo按键", MCHPvP.JobID)]
        MCHPvP_BurstMode = 118000,

        [PvPCustomCombo]
        [ParentCombo(MCHPvP_BurstMode)]
        [CustomComboInfo("Alternate Drill Option", "Saves Drill for use after Wildfire.", MCHPvP.JobID)]
        MCHPvP_BurstMode_AltDrill = 118001,

        [PvPCustomCombo]
        [ParentCombo(MCHPvP_BurstMode)]
        [CustomComboInfo("Alternate Analysis Option", "Uses Analysis with Air Anchor instead of Chain Saw.", MCHPvP.JobID)]
        MCHPvP_BurstMode_AltAnalysis = 118002,

        // Last value = 118002

        #endregion

        #region MONK

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "Turns Phantom Rush Combo into an all-in-one damage button.", MNK.JobID)]
        MNKPvP_Burst = 119000,

        [ParentCombo(MNKPvP_Burst)]
        [PvPCustomCombo]
        [CustomComboInfo("Thunderclap Option", "Adds Thunderclap to 爆发模式 when not buffed with Wind Resonance.", MNK.JobID)]
        MNKPvP_Burst_Thunderclap = 119001,

        [ParentCombo(MNKPvP_Burst)]
        [PvPCustomCombo]
        [CustomComboInfo("Riddle of Earth Option", "Adds Riddle of Earth and Earth's Reply to 爆发模式 when in combat.", MNK.JobID)]
        MNKPvP_Burst_RiddleOfEarth = 119002,

        // Last value = 119002

        #endregion

        #region NINJA

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "把旋风刃连击改为伤害combo按键", NINPvP.JobID)]
        NINPvP_ST_BurstMode = 120000,

        [PvPCustomCombo]
        [CustomComboInfo("AOE爆发模式", "把风魔手里剑改为AOE伤害combo按键", NINPvP.JobID)]
        NINPvP_AoE_BurstMode = 120001,

        [ParentCombo(NINPvP_ST_BurstMode)]
        [PvPCustomCombo]
        [CustomComboInfo("命水 选项", "Uses Three Mudra on Meisui when HP is under the set threshold.", NINPvP.JobID)]
        NINPvP_ST_Meisui = 120002,

        [ParentCombo(NINPvP_AoE_BurstMode)]
        [PvPCustomCombo]
        [CustomComboInfo("命水 选项", "Uses Three Mudra on Meisui when HP is under the set threshold.", NINPvP.JobID)]
        NINPvP_AoE_Meisui = 120003,

        // Last value = 120003

        #endregion

        #region PALADIN

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "Turns 王权剑 Combo into an all-in-one damage button.", PLD.JobID)]
        PLDPvP_Burst = 121000,

        [ParentCombo(PLDPvP_Burst)]
        [CustomComboInfo("Shield Bash Option", "Adds Shield Bash to Burst Mode.", PLD.JobID)]
        PLDPvP_ShieldBash = 121001,

        [ParentCombo(PLDPvP_Burst)]
        [CustomComboInfo("Confiteor Option", "Adds Confiteor to Burst Mode.", PLD.JobID)]
        PLDPvP_Confiteor = 121002,

        // Last value = 121002

        #endregion

        #region REAPER

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "把切割改为伤害combo按键 \n添加灵魂切割到主连击", RPR.JobID)]
        RPRPvP_Burst = 122000,

        [PvPCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo
        (
            "斩首令选项",
            "Adds Death Warrant onto the main combo when Plentiful Harvest is ready to use, or when Plentiful Harvest's cooldown is longer than Death Warrant's.\nRespects Immortal Sacrifice Pooling Option.",
            RPR.JobID
        )]
        RPRPvP_Burst_DeathWarrant = 122001,

        [PvPCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo("大丰收开启选项", "大丰收起手以便立即获取lb槽", RPR.JobID)]
        RPRPvP_Burst_PlentifulOpener = 122002,

        [PvPCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo
        (
            "Plentiful Harvest + Immortal Sacrifice Pooling Option",
            "Pools stacks of Immortal Sacrifice before using Plentiful Harvest.\nAlso holds Plentiful Harvest if Death Warrant is on cooldown.\nSet the value to 3 or below to use Plentiful Harvest as soon as it's available.",
            RPR.JobID
        )]
        RPRPvP_Burst_ImmortalPooling = 122003,

        [PvPCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo("夜游魂衣爆发选项", "Adds Lemure's Slice to the main combo during the Enshroud burst phase.\nContains burst options.", RPR.JobID)]
        RPRPvP_Burst_Enshrouded = 122004,

        #region RPR Enshrouded Option

        [PvPCustomCombo]
        [ParentCombo(RPRPvP_Burst_Enshrouded)]
        [CustomComboInfo("夜游魂斩首令选项", "在夜游魂衣爆发期间，如果有机会，将死亡授权书添加到主要组合中。", RPR.JobID)]
        RPRPvP_Burst_Enshrouded_DeathWarrant = 122005,

        [PvPCustomCombo]
        [ParentCombo(RPRPvP_Burst_Enshrouded)]
        [CustomComboInfo("团契设置", "当你有一个夜游魂衣剩余时，将团契加入主组合。", RPR.JobID)]
        RPRPvP_Burst_Enshrouded_Communio = 122006,

        #endregion

        [PvPCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo
        (
            "远程收获月选项",
            "Adds Harvest Moon onto the main combo when you're out of melee range, the GCD is not rolling and it's available for use.", RPR.JobID
        )]
        RPRPvP_Burst_RangedHarvest = 122007,

        [PvPCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo("神秘环设置", "Adds Arcane Circle to the main combo when under the set HP perecentage.", RPR.JobID)]
        RPRPvP_Burst_ArcaneCircle = 122008,

        // Last value = 122008

        #endregion

        #region RED MAGE

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "赤飞石/赤火炎改为伤害combo按键", RDMPvP.JobID)]
        RDMPvP_BurstMode = 123000,


        [PvPCustomCombo]
        [ParentCombo(RDMPvP_BurstMode)]
        [CustomComboInfo("不使用 疲惫 选项", "Prevents Frazzle from being used in Burst Mode.", RDMPvP.JobID)]
        RDMPvP_FrazzleOption = 123001,

        // Last value = 123001

        #endregion

        #region SAGE

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "把注药 III 改成伤害combo按键", SGE.JobID)]
        SGEPvP_BurstMode = 124000,

        [ParentCombo(SGEPvP_BurstMode)]
        [CustomComboInfo("Pneuma Option", "Adds Pneuma to Burst Mode.", SGE.JobID)]
        SGEPvP_BurstMode_Pneuma = 124001,

        // Last value = 124001

        #endregion

        #region SAMURAI

        #region Burst Mode

        [PvPCustomCombo]
        [CustomComboInfo
        (
            "爆发模式",
            "Adds Meikyo Shisui, Midare: Setsugekka, Ogi Namikiri, Kaeshi: Namikiri and Soten to Meikyo Shisui.\nWill only cast Midare: Setsugekka and Ogi Namikiri when you're not moving.\nWill not use if target is guarding.",
            SAM.JobID
        )]
        SAMPvP_BurstMode = 125000,

        [PvPCustomCombo]
        [ParentCombo(SAMPvP_BurstMode)]
        [CustomComboInfo("Chiten Option", "Adds Chiten to 爆发模式 when in combat and HP is below 95%.", SAM.JobID)]
        SAMPvP_BurstMode_Chiten = 125001,

        [PvPCustomCombo]
        [ParentCombo(SAMPvP_BurstMode)]
        [CustomComboInfo("Mineuchi Option", "Adds Mineuchi to Burst Mode.", SAM.JobID)]
        SAMPvP_BurstMode_Stun = 125002,

        [PvPCustomCombo]
        [ParentCombo(SAMPvP_BurstMode)]
        [CustomComboInfo("Burst Mode on 花车连 Option", "Adds 爆发模式 to 花车连 instead.", SAM.JobID, 1)]
        SAMPvP_BurstMode_MainCombo = 125003,

        #endregion

        #region Kasha Features

        [PvPCustomCombo]
        [CustomComboInfo("Kasha Combo Features", "花车连的功能集合", SAM.JobID)]
        SAMPvP_KashaFeatures = 125004,

        [PvPCustomCombo]
        [ParentCombo(SAMPvP_KashaFeatures)]
        [CustomComboInfo("使用必杀剑·早天拉近距离", "Adds Soten to the 花车连 when out of melee range.", SAM.JobID)]
        SAMPvP_KashaFeatures_GapCloser = 125005,

        [PvPCustomCombo]
        [ParentCombo(SAMPvP_KashaFeatures)]
        [CustomComboInfo("AoE Melee Protection Option", "在超出近战范围时，将AOE连击变成不可用状态", SAM.JobID)]
        SAMPvP_KashaFeatures_AoEMeleeProtection = 125006,

        #endregion

        // Last value = 125006

        #endregion

        #region SCHOLAR

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "Turns Broil IV into all-in-one damage button.", SCH.JobID)]
        SCHPvP_Burst = 126000,

        [ParentCombo(SCHPvP_Burst)]
        [CustomComboInfo("Expedient Option", "Adds Expedient to 爆发模式 to empower Biolysis.", SCH.JobID)]
        SCHPvP_Expedient = 126001,

        [ParentCombo(SCHPvP_Burst)]
        [CustomComboInfo("Biolysis Option", "Adds Biolysis use on cooldown to Burst Mode.", SCH.JobID)]
        SCHPvP_Biolysis = 126002,

        [ParentCombo(SCHPvP_Burst)]
        [CustomComboInfo("Deployment Tactics Option", "Adds Deployment Tactics to 爆发模式 when available.", SCH.JobID)]
        SCHPvP_DeploymentTactics = 126003,

        // Last value = 126003

        #endregion

        #region SUMMONER

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "将毁灭三号变成一个多合一的伤害按钮。只有在近战范围内才会使用深红旋风。", SMNPvP.JobID)]
        SMNPvP_BurstMode = 127000,

        [PvPCustomCombo]
        [ParentCombo(SMNPvP_BurstMode)]
        [CustomComboInfo("守护之光选项", "Adds Radiant Aegis to 爆发模式 when available, and your HP is at or below the set percentage.", SMNPvP.JobID)]
        SMNPvP_BurstMode_RadiantAegis = 127001,

        // Last value = 127001

        #endregion

        #region WARRIOR

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "把重劈改为伤害combo按键", WARPvP.JobID)]
        WARPvP_BurstMode = 128000,

        [PvPCustomCombo]
        [ParentCombo(WARPvP_BurstMode)]
        [CustomComboInfo("血气选项", "Allows use of Bloodwhetting any time, not just between GCDs.", WARPvP.JobID)]
        WARPvP_BurstMode_Bloodwhetting = 128001,

        [PvPCustomCombo]
        [ParentCombo(WARPvP_BurstMode)]
        [CustomComboInfo("献身选项", "Removes Blota from 爆发模式 if Primal Rend has 5 seconds or less on its cooldown.", WARPvP.JobID)]
        WARPvP_BurstMode_Blota = 128002,


        [PvPCustomCombo]
        [ParentCombo(WARPvP_BurstMode)]
        [CustomComboInfo("Primal Rend Option", "Adds Primal Rend to Burst Mode.", WARPvP.JobID)]
        WARPvP_BurstMode_PrimalRend = 128004,


        // Last value = 128002

        #endregion

        #region WHITE MAGE

        [PvPCustomCombo]
        [CustomComboInfo("爆发模式", "Turns Glare into an all-in-one damage button.", WHM.JobID)]
        WHMPvP_Burst = 129000,

        [ParentCombo(WHMPvP_Burst)]
        [CustomComboInfo("Misery Option", "Adds Afflatus Misery to Burst Mode.", WHM.JobID)]
        WHMPvP_Afflatus_Misery = 129001,

        [ParentCombo(WHMPvP_Burst)]
        [CustomComboInfo("Miracle of Nature Option", "Adds Miracle of Nature to Burst Mode.", WHM.JobID)]
        WHMPvP_Mirace_of_Nature = 129002,

        [ParentCombo(WHMPvP_Burst)]
        [CustomComboInfo("Seraph Strike Option", "Adds Seraph Strike to Burst Mode.", WHM.JobID)]
        WHMPvP_Seraph_Strike = 129003,

        [PvPCustomCombo]
        [CustomComboInfo("Aquaveil Feature", "Adds Aquaveil to Cure II when available.", WHM.JobID)]
        WHMPvP_Aquaveil = 129004,

        [PvPCustomCombo]
        [CustomComboInfo("Cure III Feature", "Adds Cure III to Cure II when available.", WHM.JobID)]
        WHMPvP_Cure3 = 129005,

        // Last value = 129005

        #endregion

        #endregion
    }
}