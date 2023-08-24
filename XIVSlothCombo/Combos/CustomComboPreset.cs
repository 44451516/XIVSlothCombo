using Dalamud.Utility;
using XIVSlothCombo.Attributes;
using XIVSlothCombo.Combos.PvE;
using XIVSlothCombo.Combos.PvP;
using XIVSlothCombo.CustomComboNS.Functions;

namespace XIVSlothCombo.Combos
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
        [CustomComboInfo("无人岛冲刺", "将冲刺替换为无人岛冲刺.\\n仅在无人岛使用.\\n图标不会更改.\\n不要和SimpleTweaks的[海岛冲刺替换]一起使用.", ADV.JobID)]
        ALL_IslandSanctuary_Sprint = 100093,

        #region Global Tank Features
        [CustomComboInfo("全局防护职业功能", "防护职业的通用功能和选项\\n取消勾选这个选项不会禁用里面的功能。", ADV.JobID)]
        ALL_Tank_Menu = 100099,

        [ReplaceSkill(All.LowBlow, PLD.ShieldBash)]
        [ParentCombo(ALL_Tank_Menu)]
        [CustomComboInfo("防护：打断功能", "当目标可被打断时，使用插言替换掉下踢。\\n骑士可以额外配置盾牌猛击的使用。", ADV.JobID)]
        ALL_Tank_Interrupt = 100000,

        [ReplaceSkill(All.Reprisal)]
        [ParentCombo(ALL_Tank_Menu)]
        [CustomComboInfo("防护：血仇防顶", "当目标已被赋予雪仇效果时，将雪仇替换为飞石", ADV.JobID)]
        ALL_Tank_Reprisal = 100001,
        #endregion

        #region Global Healer Features
        [CustomComboInfo("全局治疗职业功能", "治疗职业的通用功能和选项\\n取消勾选这个选项不会禁用里面的功能。", ADV.JobID)]
        ALL_Healer_Menu = 100098,

        [ReplaceSkill(AST.Ascend, WHM.Raise, SCH.Resurrection, SGE.Egeiro)]
        [ConflictingCombos(AST_Raise_Alternative, SCH_Raise, SGE_Raise, WHM_Raise)]
        [ParentCombo(ALL_Healer_Menu)]
        [CustomComboInfo("治疗：复活功能", "将复活替换为即刻咏唱", ADV.JobID)]
        ALL_Healer_Raise = 100010,
        #endregion

        #region Global Magical Ranged Features
        [CustomComboInfo("全局远程魔法进攻职业功能", "远程魔法进攻职业的通用功能和选项\\n取消勾选这个选项不会禁用里面的功能。", ADV.JobID)]
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
        [CustomComboInfo("全局近战进攻职业功能", "近战进攻职业功能的通用功能和选项\\n取消勾选这个选项不会禁用里面的功能。", ADV.JobID)]
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
        [CustomComboInfo("全局物理远程职业功能", "物理远程职业功能的通用功能和选项\\n取消勾选这个选项不会禁用里面的功能。", ADV.JobID)]
        ALL_Ranged_Menu = 100095,

        [ReplaceSkill(MCH.Tactician, BRD.Troubadour, DNC.ShieldSamba)]
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

        #region DPS
        [ReplaceSkill(AST.Malefic, AST.Malefic2, AST.Malefic3, AST.Malefic4, AST.FallMalefic, AST.Combust, AST.Combust2, AST.Combust3, AST.Gravity, AST.Gravity2)]
        [CustomComboInfo("续dot", "用以下选项替换煞星或烧灼", AST.JobID, 0, "", "")]
        AST_ST_DPS = 1004,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("烧灼上线选项", "Adds Combust to the DPS feature if it's not present on current target, or is about to expire.", AST.JobID, 0, "", "")]
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
        [CustomComboInfo("Card Play Weave Option", "Weaves your card (best used with Quick Target Cards)", AST.JobID, 6)]
        AST_DPS_AutoPlay = 1037,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("Redraw Option", "Weaves Redraw if you pull 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 card with 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 seal you already have and you can use Redraw.", AST.JobID, 7)]
        AST_DPS_AutoPlay_Redraw = 1038,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("星力选项", "当集齐三个星标时将星力加入循环", AST.JobID, 8, "", "")]
        AST_DPS_Astrodyne = 1009,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("小奥秘卡 插入选项", "加入小奥秘卡（小阿卡纳）", AST.JobID, 9, "", "")]
        AST_DPS_AutoCrownDraw = 1012,

        [ParentCombo(AST_ST_DPS)]
        [CustomComboInfo("王冠之领主选项", "循环加入王冠之领主", AST.JobID, 10, "", "")]
        AST_DPS_LazyLord = 1014,
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
        [ReplaceSkill(AST.Play)]
        [CustomComboInfo("用出卡替换抽卡", "Play turns into Draw when no card is drawn, as well as the usual Play behavior.", AST.JobID, 6, "", "")]
        AST_Cards_DrawOnPlay = 1000,

        [ParentCombo(AST_Cards_DrawOnPlay)]
        [CustomComboInfo("重抽功能", "当你抽到星标重复的卡且重抽技能可用时，将抽卡替换为重抽", AST.JobID)]
        AST_Cards_Redraw = 1032,


        [ReplaceSkill(AST.Play)]
        //Works With AST_Cards_DrawOnPlay as 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 feature, or by itself if AST_Cards_DrawOnPlay is disabled.
        //Do not do ConflictingCombos with AST_Cards_DrawOnPlay
        [CustomComboInfo("用星力替换出卡", "当你拥有三颗星星时自动替换出卡为星力", AST.JobID, 18, "", "")]
        AST_Cards_AstrodyneOnPlay = 1015,

        [CustomComboInfo("快速目标发牌", "抽卡时自动选中一个合适的队友。", AST.JobID)]
        AST_Cards_QuickTargetCards = 1029,

        [ParentCombo(AST_Cards_QuickTargetCards)]
        [CustomComboInfo("将坦克和奶妈加入自动发牌目标选择", "如果没有 DPS 可选时选择坦克和奶妈", AST.JobID)]
        AST_Cards_QuickTargetCards_TargetExtra = 1031,
        #endregion

        // Last value = 1039

        #endregion

        #region BLACK MAGE
        [ReplaceSkill(BLM.Transpose)]
        [CustomComboInfo("灵极魂/星灵移位功能", "当灵极魂可用时将星灵移位替换为灵极魂。", BLM.JobID, 0, "", "")]
        BLM_Mana = 2001,

        [ReplaceSkill(BLM.LeyLines)]
        [CustomComboInfo("魔纹步功能", "使用黑魔纹后将其替换为魔纹步。", BLM.JobID, 0, "", "")]
        BLM_LeyLines = 2002,

        [ReplaceSkill(BLM.Blizzard, BLM.Freeze)]
        [CustomComboInfo("冰1/2/3", "当自身没有灵极冰状态时替换冰结为冰封，根据自身等级自动替换玄冰为冰冻。", BLM.JobID, 0, "", "")]
        BLM_Blizzard = 2003,

        [ReplaceSkill(BLM.Scathe)]
        [ConflictingCombos(BLM_SimpleMode, BLM_Simple_Transpose, BLM_Paradox)]
        [CustomComboInfo("异言功能", "当异言可用时替换崩溃为异言。", BLM.JobID, 0, "", "")]
        BLM_ScatheXeno = 2004,

        [ReplaceSkill(BLM.Fire)]
        [CustomComboInfo("火炎/爆炎功能", "当自身没有星极火状态或使用火起手时，用爆炎替换火炎。", BLM.JobID, 0, "", "")]
        BLM_Fire_1to3 = 2005,

        [ParentCombo(BLM_SimpleMode)]
        [CustomComboInfo("雷云功能", "当自身存在雷云状态且目标身上没有dot存在或dot即将结束时，自动插入闪雷/暴雷.", BLM.JobID, 0, "", "")]
        BLM_Thunder = 2006,

        [ReplaceSkill(BLM.Flare)]
        [CustomComboInfo("简易 AoE 功能", "将核爆整合为一键AoE循环。", BLM.JobID, -1, "", "")]
        BLM_AoE_SimpleMode = 2008,

        [ParentCombo(BLM_Thunder)]
        [CustomComboInfo("闪雷/暴雷功能", "当目标身上没有dot存在或dot即将结束时，自动插入闪雷/暴雷.", BLM.JobID, 0, "", "")]
        BLM_ThunderUptime = 2011,

        [ReplaceSkill(BLM.Scathe)]
        [ConflictingCombos(BLM_ScatheXeno, BLM_Simple_Transpose, BLM_Paradox)]
        [CustomComboInfo("标准循环", "将崩溃整合为一键单体标准循环。", BLM.JobID, -3, "", "")]
        BLM_SimpleMode = 2012,

        [ParentCombo(BLM_SimpleMode)]
        [CustomComboInfo("冷却选项", "Adds Manafont, Sharpcast, Amplifier onto the Simple BLM feature.", BLM.JobID, 0, "", "")]
        BLM_Simple_Buffs = 2013,

        [ParentCombo(BLM_SimpleMode)]
        [CustomComboInfo("黑魔纹选项", "加入黑魔纹至一键循环.", BLM.JobID, 0, "", "")]
        BLM_Simple_Buffs_LeyLines = 2014,

        [ParentCombo(BLM_SimpleMode)]
        [CustomComboInfo("瞬发选项", "加入三连咏唱至一键循环.", BLM.JobID, 0, "", "")]
        BLM_Simple_Casts = 2015,

        [ParentCombo(BLM_Simple_Casts)]
        [CustomComboInfo("保留瞬发选项", "保留即刻咏唱和一层三连咏唱，用于在移动时瞬发技能。", BLM.JobID, 0, "", "")]
        BLM_Simple_Casts_Pooling = 2016,

        [ParentCombo(BLM_SimpleMode)]
        [CustomComboInfo("保留异言选项", "一键循环保留一个异言便于走位", BLM.JobID, 0, "", "")]
        BLM_Simple_XenoPooling = 2017,

        [ParentCombo(BLM_SimpleMode)]
        [CustomComboInfo("火起手", "Adds the 火起手 to Simple BLM.", BLM.JobID, 0, "", "")]
        BLM_Simple_Opener = 2018,

        [ParentCombo(BLM_Simple_Opener)]
        [CustomComboInfo("火起手+单三连咏唱", "Modifies the Simple 火起手 to only use 1 Triplecast.", BLM.JobID, 0, "", "")]
        BLM_Simple_OpenerAlternate = 2019,

        [ParentCombo(BLM_AoE_SimpleMode)]
        [CustomComboInfo("秽浊/魔泉 核爆 功能", "当自身处于星极火状态且秽浊可用时插入秽浊，在秽浊后插入魔泉以便再使用一次核爆。", BLM.JobID, 0, "", "")]
        BLM_AoE_Simple_Foul = 2020,

        [ReplaceSkill(BLM.Scathe)]
        [ConflictingCombos(BLM_ScatheXeno, BLM_SimpleMode, BLM_Paradox)]
        [CustomComboInfo("星灵循环", "将崩溃整合为一键星灵循环（需要90级）。", BLM.JobID, -2, "", "")]
        BLM_Simple_Transpose = 2021,

        [ParentCombo(BLM_Simple_Transpose)]
        [CustomComboInfo("保留三连咏唱选项", "保留一层三连咏唱用于移动时瞬发技能.", BLM.JobID, 0, "", "")]
        BLM_Simple_Transpose_Pooling = 2022,

        [ReplaceSkill(BLM.Scathe)]
        [ConflictingCombos(BLM_ScatheXeno, BLM_SimpleMode, BLM_Simple_Transpose)]
        [CustomComboInfo("悖论循环", "将崩溃整合为悖论循环（需要90级）。\\n它的读条数很少，但比标准循环的伤害低 9-13%%。", BLM.JobID, -2, "", "")]
        BLM_Paradox = 2023,

        [ParentCombo(BLM_Simple_Transpose)]
        [CustomComboInfo("黑魔纹选项", "在星灵循环中插入黑魔纹。", BLM.JobID, 0, "", "")]
        BLM_Simple_Transpose_LeyLines = 2024,

        [ParentCombo(BLM_Paradox)]
        [CustomComboInfo("黑魔纹选项", "在悖论循环中插入黑魔纹。", BLM.JobID, 0, "", "")]
        BLM_Paradox_LeyLines = 2025,

        [ParentCombo(BLM_SimpleMode)]
        [CustomComboInfo("瞬发走位选项", "移动时自动插入即刻咏唱/三连咏唱瞬发技能。", BLM.JobID, 0, "", "")]
        BLM_Simple_CastMovement = 2026,

        [ParentCombo(BLM_Simple_CastMovement)]
        [CustomComboInfo("异言走位选项", "移动时自动插入异言。", BLM.JobID, 0, "", "")]
        BLM_Simple_CastMovement_Xeno = 2027,

        [ParentCombo(BLM_Simple_CastMovement)]
        [CustomComboInfo("崩溃走位选项", "移动时自动插入崩溃。", BLM.JobID, 0, "", "")]
        BLM_Simple_CastMovement_Scathe = 2028,

        [ParentCombo(BLM_Simple_Transpose)]
        [CustomComboInfo("雷云功能", "当自身存在雷云状态且目标身上没有dot存在或dot即将结束时，自动插入闪雷/暴雷.", BLM.JobID, 0, "", "")]
        BLM_TransposeThunder = 2029,

        [ParentCombo(BLM_TransposeThunder)]
        [CustomComboInfo("闪雷/暴雷功能", "当目标身上没有dot存在或dot即将结束时，自动插入闪雷/暴雷.", BLM.JobID, 0, "", "")]
        BLM_TransposeThunderUptime = 2030,

        [ReplaceSkill(BLM.AetherialManipulation)]
        [CustomComboInfo("以太步特性", "当自身不在魔纹步范围内且站立不动时，把以太步替换为魔纹步", BLM.JobID, 0, "", "")]
        BLM_AetherialManipulation = 2031,

        [Variant]
        [VariantParent(BLM_SimpleMode, BLM_AoE_SimpleMode)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", BLM.JobID)]
        BLM_Variant_Rampart = 2032,

        [Variant]
        [CustomComboInfo("复活 选项", "当你有即刻BUFF时，替换即刻为成多变复活", BLM.JobID)]
        BLM_Variant_Raise = 2033,

        [Variant]
        [VariantParent(BLM_SimpleMode, BLM_AoE_SimpleMode)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", BLM.JobID)]
        BLM_Variant_Cure = 2034,

        [ReplaceSkill(BLM.Scathe)]
        [ConflictingCombos(BLM_SimpleMode, BLM_Simple_Transpose, BLM_Paradox)]
        [CustomComboInfo("自定义循环", "将崩溃整合为单体自定义循环。", BLM.JobID, -3, "", "")]
        BLM_Zimo = 2500,

        // Last value = 2034

        #endregion

        #region BLUE MAGE

        [BlueInactive(BLU.SongOfTorment, BLU.Bristle)]
        [ReplaceSkill(BLU.SongOfTorment)]
        [CustomComboInfo("苦闷之歌buff强化", "将苦闷之歌用怒发冲冠代替，使苦闷之歌处于怒发冲冠的buff下", BLU.JobID)]
        BLU_BuffedSoT = 70000,

        [BlueInactive(BLU.Whistle, BLU.Tingle, BLU.MoonFlute, BLU.JKick, BLU.TripleTrident, BLU.Nightbloom, BLU.RoseOfDestruction, BLU.FeatherRain, BLU.Bristle, BLU.GlassDance, BLU.Surpanakha, BLU.MatraMagic, BLU.ShockStrike, BLU.PhantomFlurry)]
        [ReplaceSkill(BLU.MoonFlute)]
        [CustomComboInfo("月之笛起手连击", "Puts the Full 月之笛起手连击 on Moon Flute or Whistle.", BLU.JobID)]
        BLU_Opener = 70001,

        [BlueInactive(BLU.MoonFlute, BLU.Tingle, BLU.ShockStrike, BLU.Whistle, BLU.FinalSting)]
        [ReplaceSkill(BLU.FinalSting)]
        [CustomComboInfo("终极针连击", "Turns Final Sting into the buff combo of: Moon Flute, Tingle, Whistle, Final Sting. Will use any primals off cooldown before casting Final Sting.", BLU.JobID)]
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
        [CustomComboInfo("坦克Debuff特性", "Puts Devour, Off-Guard, Lucid Dreaming, and Bad Breath into one button when under Tank Mimicry.", BLU.JobID)]
        BLU_DebuffCombo = 70006,

        [BlueInactive(BLU.MagicHammer)]
        [ReplaceSkill(BLU.MagicHammer)]
        [CustomComboInfo("昏乱/魔法锤Debuff特性", "当冷却完毕时，用魔法锤代替昏乱", BLU.JobID)]
        BLU_Addle = 70007,

        [BlueInactive(BLU.FeatherRain, BLU.ShockStrike, BLU.RoseOfDestruction, BLU.GlassDance)]
        [ReplaceSkill(BLU.FeatherRain)]
        [CustomComboInfo("蛮神技特性", "在冷却结束后，将所有蛮神技能整合至飞翎羽。\\n如果在冷却未结束的时候使用，将导致月之笛爆发不完整。", BLU.JobID)]
        BLU_PrimalCombo = 70008,

        [BlueInactive(BLU.BlackKnightsTour, BLU.WhiteKnightsTour)]
        [ReplaceSkill(BLU.BlackKnightsTour, BLU.WhiteKnightsTour)]
        [CustomComboInfo("骑士之旅特性", "在敌人处于“止步”或“减速”状态下时，自动将在黑/白骑士之旅间切换。", BLU.JobID)]
        BLU_KnightCombo = 70009,

        [BlueInactive(BLU.PeripheralSynthesis, BLU.MustardBomb)]
        [ReplaceSkill(BLU.PeripheralSynthesis)]
        [CustomComboInfo("生成外设-芥末爆弹", "Turns 生成外设-芥末爆弹 when target is under the effect of Lightheaded.", BLU.JobID)]
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
        [CustomComboInfo("伶牙俐齿续dot模式A", "当目标身上没有毒/风dot时，替换伶牙俐齿为毒/风箭。\\n当还未习得伶牙俐齿时会在毒/风箭之间自动切换。", BRD.JobID, 0, "", "")]
        BRD_IronJaws = 3003,

        [ReplaceSkill(BRD.IronJaws)]
        [ConflictingCombos(BRD_IronJaws)]
        [CustomComboInfo("伶牙俐齿续dot模式B", "当目标身上没有毒/风dot时，替换伶牙俐齿为毒/风箭。 \\n伶牙俐齿仅会在风/毒dot即将结束时复现。", BRD.JobID, 0, "", "")]
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
        [CustomComboInfo("简易诗人", "Adds every single target ability to one button,\\nIf there are DoTs on target, Simple Bard will try to maintain their uptime.", BRD.JobID, 0, "", "")]
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
        [CustomComboInfo("防止资源浪费相关选项", "Adds enemy health checking on mobs for buffs, DoTs and Songs.\\nThey will not be reapplied if less than specified.", BRD.JobID, 0, "", "")]
        BRD_Simple_NoWaste = 3019,

        [ParentCombo(BRD_ST_SimpleMode)]
        [CustomComboInfo("单体连击打断技能设置", "如果可用，在循环中加入打断技能", BRD.JobID, 0, "", "")]
        BRD_Simple_Interrupt = 3020,

        [CustomComboInfo("禁用绝峰箭", "不在一键连击中自动替换插入绝峰箭。", BRD.JobID, 0, "", "")]
        BRD_RemoveApexArrow = 3021,

        //[ConflictingCombos(BardoGCDSingleTargetFeature)]
        //[ParentCombo(SimpleBardFeature)]
        //[CustomComboInfo("单体简易起手", "在单体一键连击中加入最佳起手技能。\\n此选项与其它绝大部分类似选项均有冲突。", BRD.JobID, 0, "", "")]
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
        [CustomComboInfo("只有起手", "你可以自动对新目标释放Dots直到第一次自动刷新(伶牙俐齿)\\n即启用本选项后，在第一次自动刷新(伶牙俐齿)后不会对新目标上Dot(包括同一目标上天断Dot后)", BRD.JobID, 0, "", "")]
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

        #region Single Target Multibutton
        [ReplaceSkill(DNC.Cascade)]
        [ConflictingCombos(DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("单目标多按钮", "使用扇舞与伶俐值的单目标连击", DNC.JobID, 0, "", "")]
        DNC_ST_MultiButton = 4000,

        [ParentCombo(DNC_ST_MultiButton)]
        [CustomComboInfo("单体目标伶俐防溢出设置", "伶俐超过下面设置的数值后，将剑舞加入循环", DNC.JobID, 0, "", "")]
        DNC_ST_EspritOvercap = 4001,

        [ParentCombo(DNC_ST_MultiButton)]
        [CustomComboInfo("幻扇溢出保护", "当量谱上幻扇满了后，将扇舞序加入循环", DNC.JobID, 0, "", "")]
        DNC_ST_FanDanceOvercap = 4003,

        [ParentCombo(DNC_ST_MultiButton)]
        [CustomComboInfo("扇舞选项", "当扇舞急·扇舞终可用时，加入循环。", DNC.JobID, 0, "", "")]
        DNC_ST_FanDance34 = 4004,
        #endregion

        #region AoE Multibutton
        [ReplaceSkill(DNC.Windmill)]
        [ConflictingCombos(DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("AOE多按钮", "使用扇舞与伶俐值的AOE连击", DNC.JobID, 0, "", "")]
        DNC_AoE_MultiButton = 4010,

        [ParentCombo(DNC_AoE_MultiButton)]
        [CustomComboInfo("AoE伶俐防溢出设置", "伶俐超过下面设置的数值后，将剑舞加入循环", DNC.JobID, 0, "", "")]
        DNC_AoE_EspritOvercap = 4011,

        [ParentCombo(DNC_AoE_MultiButton)]
        [CustomComboInfo("AoE幻扇溢出保护", "当量谱上幻扇满了后，将扇舞破加入循环", DNC.JobID, 0, "", "")]
        DNC_AoE_FanDanceOvercap = 4013,

        [ParentCombo(DNC_AoE_MultiButton)]
        [CustomComboInfo("AOE幻扇选项", "当扇舞急·扇舞终可用时，加入循环。", DNC.JobID, 0, "", "")]
        DNC_AoE_FanDance34 = 4014,
        #endregion

        #region Dance Features
        [ConflictingCombos(DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("跳舞功能", "标准舞步和技巧舞步相关功能与设置\\n折叠此选项不会关闭其中的功能。", DNC.JobID, 0, "", "")]
        DNC_Dance_Menu = 4020,

        #region Combined Dance Feature
        [ReplaceSkill(DNC.StandardStep)]
        [ParentCombo(DNC_Dance_Menu)]
        [ConflictingCombos(DNC_DanceStepCombo, DNC_DanceComboReplacer, DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("舞步整合", "整合标准与技巧舞步到一键 (SS)" +
        "\\nStandard > Technical." +
        "\\nThis combos out into Tillana and Starfall Dance.", DNC.JobID, 0, "", "")]
        DNC_CombinedDances = 4022,

        [ParentCombo(DNC_CombinedDances)]
        [CustomComboInfo("进攻之探戈附加设置", "在技巧舞步结束后接续进攻之探戈。", DNC.JobID, 0, "", "")]
        DNC_CombinedDances_Devilment = 4023,

        [ParentCombo(DNC_CombinedDances)]
        [CustomComboInfo("百花争艳附加设置", "将百花争艳加入到舞步整合。", DNC.JobID, 0, "", "")]
        DNC_CombinedDances_Flourish = 4024,
        #endregion

        [ParentCombo(DNC_Dance_Menu)]
        [ConflictingCombos(DNC_DanceStepCombo, DNC_CombinedDances, DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("自定义舞步功能",
        "Change custom actions into dance steps while dancing." +
        "\\nThis helps ensure you can still dance with combos on, without using auto dance." +
        "\\nYou can change the respective actions by inputting action IDs below for each dance step." +
        "\\nThe defaults are Cascade, Flourish, Fan Dance and Fan Dance II. If set to 0, they will reset to these actions." +
        "\\nYou can get Action IDs with Garland Tools by searching for the action and clicking the cog.", DNC.JobID, 0, "", "")]
        DNC_DanceComboReplacer = 4025,
        #endregion

        #region Flourishing Features
        [ConflictingCombos(DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("百花争艳期间功能", "百花争艳与幻扇相关设置.\\n折叠此选项不会禁用里面的功能." +
        "\\nCollapsing this category does NOT disable the features inside.", DNC.JobID, 0, "", "")]
        DNC_FlourishingFeatures_Menu = 4030,

        [ReplaceSkill(DNC.Flourish)]
        [ParentCombo(DNC_FlourishingFeatures_Menu)]
        [ConflictingCombos(DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("百花齐放中扇舞相关设置", "Replace Flourish with Fan Dance 3 & 4 during weave-windows, when Flourish is on cooldown.", DNC.JobID, 0, "", "")]
        DNC_FlourishingFanDances = 4032,
        #endregion

        #region Fan Dance Combo Features
        [ParentCombo(DNC_FlourishingFeatures_Menu)]
        [ConflictingCombos(DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("扇舞连击", "扇舞连击相关选项" +
        "\\nFan Dance 3 takes priority over Fan Dance 4.", DNC.JobID, 0, "", "")]
        DNC_FanDanceCombos = 4033,

        [ReplaceSkill(DNC.FanDance1)]
        [ParentCombo(DNC_FanDanceCombos)]
        [CustomComboInfo("扇舞·序 -> 扇舞·急", "当可用时，将扇舞·序替换为扇舞·急。", DNC.JobID, 0, "", "")]
        DNC_FanDance_1to3_Combo = 4034,

        [ReplaceSkill(DNC.FanDance1)]
        [ParentCombo(DNC_FanDanceCombos)]
        [CustomComboInfo("扇舞·序 -> 扇舞·终", "当可用时，将扇舞·序替换为扇舞·终。", DNC.JobID, 0, "", "")]
        DNC_FanDance_1to4_Combo = 4035,

        [ReplaceSkill(DNC.FanDance2)]
        [ParentCombo(DNC_FanDanceCombos)]
        [CustomComboInfo("扇舞·破 -> 扇舞·急", "当可用时，将扇舞·破替换为扇舞·急。", DNC.JobID, 0, "", "")]
        DNC_FanDance_2to3_Combo = 4036,

        [ReplaceSkill(DNC.FanDance2)]
        [ParentCombo(DNC_FanDanceCombos)]
        [CustomComboInfo("扇舞·破 -> 扇舞·终", "当可用时，将扇舞·破替换为扇舞·终。", DNC.JobID, 0, "", "")]
        DNC_FanDance_2to4_Combo = 4037,
        #endregion

        // Devilment --> Starfall
        [ReplaceSkill(DNC.Devilment)]
        [ConflictingCombos(DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("进攻之探戈 -> 流星舞", "使用完进攻之探戈后将其替换为流星舞。", DNC.JobID, 0, "", "")]
        DNC_Starfall_Devilment = 4038,

        [ReplaceSkill(DNC.StandardStep, DNC.TechnicalStep)]
        [ConflictingCombos(DNC_CombinedDances, DNC_DanceComboReplacer)]
        [CustomComboInfo("舞步连击相关", "Change Standard Step and Technical Step into each dance step, while dancing." +
        "\\nWorks with Simple Dancer and Simple Dancer AoE.", DNC.JobID, 0, "", "")]
        DNC_DanceStepCombo = 4039,

        #region Simple Dancer (Single Target)
        [ReplaceSkill(DNC.Cascade)]
        [ConflictingCombos(DNC_ST_MultiButton, DNC_AoE_MultiButton, DNC_CombinedDances, DNC_DanceComboReplacer, DNC_FlourishingFeatures_Menu, DNC_Starfall_Devilment)]
        [CustomComboInfo("简易舞者（单目标）", "Single button, single target. Includes songs, flourishes and overprotections." +
        "\\nConflicts with all other non-simple toggles, except 'Dance Step Combo'.", DNC.JobID, 0, "", "")]
        DNC_ST_SimpleMode = 4050,

        [ParentCombo(DNC_ST_SimpleMode)]
        [CustomComboInfo("单体连击打断技能设置", "循环加入打断技能", DNC.JobID, 5, "", "")]
        DNC_ST_Simple_Interrupt = 4051,

        [ParentCombo(DNC_ST_SimpleMode)]
        [ConflictingCombos(DNC_ST_Simple_StandardFill)]
        [CustomComboInfo("简易标准舞步选项", "循环加入标准舞步", DNC.JobID, 1, "", "")]
        DNC_ST_Simple_SS = 4052,

        [ParentCombo(DNC_ST_SimpleMode)]
        [ConflictingCombos(DNC_ST_Simple_SS)]
        [CustomComboInfo("简易标准舞步选项", "只加入标准舞步和其结束进循环." +
        "\\nStandard Step itself must be initiated manually when using this option.", DNC.JobID, 1, "", "")]
        DNC_ST_Simple_StandardFill = 4061,

        [ParentCombo(DNC_ST_SimpleMode)]
        [ConflictingCombos(DNC_ST_Simple_TechFill)]
        [CustomComboInfo("简易技巧舞步选项", "Includes Technical Step, all dance steps and Technical Finish in the rotation.", DNC.JobID, 2, "", "")]
        DNC_ST_Simple_TS = 4053,

        [ParentCombo(DNC_ST_SimpleMode)]
        [ConflictingCombos(DNC_ST_Simple_TS)]
        [CustomComboInfo("简易技巧舞步选项", "只加入技巧舞步和其结束进循环." +
        "\\nTechnical Step itself must be initiated manually when using this option.", DNC.JobID, 2, "", "")]
        DNC_ST_Simple_TechFill = 4054,

        [ParentCombo(DNC_ST_SimpleMode)]
        [CustomComboInfo("简易技巧进攻之探戈", "将进攻之探戈加入循环." +
        "\\nWill activate only during Technical Finish if you're Lv70 or above." +
        "\\nWill be used on cooldown below Lv70.", DNC.JobID, 2, "", "")]
        DNC_ST_Simple_Devilment = 4055,

        [ParentCombo(DNC_ST_SimpleMode)]
        [CustomComboInfo("简易扇舞选项", "将剑舞加入循环当伶俐超过临界点", DNC.JobID, 3, "", "")]
        DNC_ST_Simple_SaberDance = 4063,

        [ParentCombo(DNC_ST_SimpleMode)]
        [CustomComboInfo("简易百花争艳", "将百花争艳加入循环。", DNC.JobID, 3, "", "")]
        DNC_ST_Simple_Flourish = 4056,

        [ParentCombo(DNC_ST_SimpleMode)]
        [CustomComboInfo("简易幻扇", "幻扇将要溢出时使用一次" +
        "\\nWeaves feathers where possible during Technical Finish." +
        "\\nWeaves feathers outside of burst when target is below set HP percentage (Set to 0 to disable)." +
        "\\nWeaves feathers whenever available when under Lv.70.", DNC.JobID, 4, "", "")]
        DNC_ST_Simple_Feathers = 4057,

        /*
        [ParentCombo(DNC_ST_Simple_Feathers)]
        [CustomComboInfo("简易幻扇囤积", "幻扇将要溢出时使用一次" +
        "\\nWeaves feathers where possible during Technical Finish." +
        "\\nWeaves feathers outside of burst when target is below set HP percentage.", DNC.JobID, 4, "", "")]
        DNC_ST_Simple_FeatherPooling = 4058,
        */

        [ParentCombo(DNC_ST_SimpleMode)]
        [CustomComboInfo("简易紧急恢复", "自身血量低于设定值时使用治疗华尔兹、内丹", DNC.JobID, 5, "", "")]
        DNC_ST_Simple_PanicHeals = 4059,

        [ParentCombo(DNC_ST_SimpleMode)]
        [CustomComboInfo("简易即兴表演", "当即兴表演可用时将其加入循环。", DNC.JobID, 5, "", "")]
        DNC_ST_Simple_Improvisation = 4060,

        [ParentCombo(DNC_ST_SimpleMode)]
        [CustomComboInfo("简易速行起手", "Uses Peloton when you are out of combat, do not already have the Peloton buff and are performing Standard Step with greater than 5s remaining of your dance." +
        "\\nWill not override Dance Step Combo Feature.", DNC.JobID, 5, "", "")]
        DNC_ST_Simple_Peloton = 4062,
        #endregion

        #region Simple Dancer (AoE)
        [ReplaceSkill(DNC.Windmill)]
        [ConflictingCombos(DNC_ST_MultiButton, DNC_AoE_MultiButton, DNC_CombinedDances, DNC_DanceComboReplacer, DNC_FlourishingFeatures_Menu, DNC_Starfall_Devilment)]
        [CustomComboInfo("简易舞者（AOE）", "Single button, AoE. Includes songs, flourishes and overprotections." +
        "\\nConflicts with all other non-simple toggles, except 'Dance Step Combo'.", DNC.JobID, 0, "", "")]
        DNC_AoE_SimpleMode = 4070,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo("简易AoE中断", "在AoE循环中加入中断(伤头)(如果当前目标可被打断)。", DNC.JobID, 0, "", "")]
        DNC_AoE_Simple_Interrupt = 4071,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [ConflictingCombos(DNC_AoE_Simple_StandardFill)]
        [CustomComboInfo("简易标准舞步选项（AOE）", "循环加入标准舞步", DNC.JobID, 1, "", "")]
        DNC_AoE_Simple_SS = 4072,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [ConflictingCombos(DNC_AoE_Simple_SS)]
        [CustomComboInfo("简易标准舞步选项（AOE）", "将标准舞步加入AOE循环" +
        "\\nStandard Step itself must be initiated manually when using this option.", DNC.JobID, 2, "", "")]
        DNC_AoE_Simple_StandardFill = 4081,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [ConflictingCombos(DNC_AoE_Simple_TechFill)]
        [CustomComboInfo("简易技巧舞步选项（AOE）", "Includes Technical Step, all dance steps and Technical Finish in the AoE rotation.", DNC.JobID, 3, "", "")]
        DNC_AoE_Simple_TS = 4073,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [ConflictingCombos(DNC_AoE_Simple_TS)]
        [CustomComboInfo("简易技巧舞步选项（AOE）", "将技巧舞步加入AOE循环" +
        "\\nTechnical Step itself must be initiated manually when using this option.", DNC.JobID, 4, "", "")]
        DNC_AoE_Simple_TechFill = 4074,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo("简易AoE技巧进攻之探戈", "将进攻之探戈加入AOE循环" +
        "\\nWill activate only during Technical Finish if you're Lv70 or above." +
        "\\nWill be used on cooldown below Lv70.", DNC.JobID, 5, "", "")]
        DNC_AoE_Simple_Devilment = 4075,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo("简易剑舞选项", "伶俐值超过阈值时将剑舞加入AOE循环", DNC.JobID, 6, "", "")]
        DNC_AoE_Simple_SaberDance = 4082,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo("简易AoE百花争艳", "将百花争艳加入AoE循环。", DNC.JobID, 6, "", "")]
        DNC_AoE_Simple_Flourish = 4076,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo("简易AoE幻扇", "幻扇将要溢出时使用一次" +
        "\\nWeaves feathers where possible during Technical Finish." +
        "\\nWeaves feathers whenever available when under Lv.70.", DNC.JobID, 7, "", "")]
        DNC_AoE_Simple_Feathers = 4077,

        /*
        [ParentCombo(DNC_AoE_Simple_Feathers)]
        [CustomComboInfo("简易AoE幻扇囤积", "幻扇将要溢出时使用一次", DNC.JobID, 8, "", "")]
        DNC_AoE_Simple_FeatherPooling = 4078,
        */

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo("简易AoE紧急恢复", "自身血量低于设定值时使用治疗华尔兹、内丹", DNC.JobID, 9, "", "")]
        DNC_AoE_Simple_PanicHeals = 4079,

        [ParentCombo(DNC_AoE_SimpleMode)]
        [CustomComboInfo("简易AoE即兴表演", "当即兴表演可用时将其加入AoE循环。", DNC.JobID, 10, "", "")]
        DNC_AoE_Simple_Improvisation = 4080,
        #endregion

        #region Variant
        [Variant]
        [VariantParent(DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", DNC.JobID)]
        DNC_Variant_Rampart = 4083,

        [Variant]
        [VariantParent(DNC_ST_SimpleMode, DNC_AoE_SimpleMode)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", DNC.JobID)]
        DNC_Variant_Cure = 4084,


        #endregion

        // Last value = 4084

        #endregion

        #region DARK KNIGHT

        [ParentCombo(DRK_SouleaterCombo)]
        [CustomComboInfo("主连击Buff整合", "将所有增益技能整合至噬魂斩连击中.", DRK.JobID, 0, "", "")]
        DRK_MainComboBuffs_Group = 5098,

        [ConflictingCombos(DRK_oGCD)]
        [ParentCombo(DRK_SouleaterCombo)]
        [CustomComboInfo("暗黑骑士能力技整合 设置", "将所有能力技整合至噬魂斩连击中.", DRK.JobID, 0, "", "")]
        DRK_MainComboCDs_Group = 5099,

        [ReplaceSkill(DRK.Souleater)]
        [CustomComboInfo("噬魂斩连击", "用基础循环替换掉噬魂斩。 \\n如果所有的次级选项都被开启，那么就可以进行一键循环(简单黑骑)", DRK.JobID, 0, "", "")]
        DRK_SouleaterCombo = 5000,

        [ReplaceSkill(DRK.StalwartSoul)]
        [CustomComboInfo("刚魂连击", "用基础循环替换掉刚魂。", DRK.JobID, 0, "", "")]
        DRK_StalwartSoulCombo = 5001,

        [ReplaceSkill(DRK.Souleater)]
        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("血溅 设置", "当血乱激活时，使用血溅和寂灭替换掉噬魂战和刚魂。", DRK.JobID, 0, "", "")]
        DRK_Bloodspiller = 5002,

        [ReplaceSkill(DRK.StalwartSoul)]
        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("暗血量表溢出特性 SoulCombo", "当你的暗血即将溢出时，使用寂灭替换掉刚魂。", DRK.JobID, 0, "", "")]
        DRK_Overcap = 5003,

        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("掠影示现特性", "当掠影示现未在冷却中且暗血值足够时将其插入主连击中.", DRK.JobID, 0, "", "")]
        DRK_LivingShadow = 5004,

        [ParentCombo(DRK_SouleaterCombo)]
        [CustomComboInfo("暗影锋防覆盖 设置", "Uses Edge of Shadow if you are above 8,500 mana or Darkside is about to expire (10sec or less)", DRK.JobID, 0, "", "")]
        DRK_ManaOvercap = 5005,

        [ReplaceSkill(DRK.CarveAndSpit, DRK.AbyssalDrain)]
        [ConflictingCombos(DRK_MainComboCDs_Group)]
        [CustomComboInfo("能力技特性", "按照掠影示现 > 腐秽大地 > 精雕怒斩 > 腐秽黑暗的顺序将能力技整合到精雕怒斩与吸血深渊。", DRK.JobID, 0, "", "")]
        DRK_oGCD = 5006,

        [ParentCombo(DRK_oGCD)]
        [CustomComboInfo("暗影使者 设置", "Adds Shadowbringer to 能力技特性 ", DRK.JobID, 0, "", "")]
        DRK_Shadowbringer_oGCD = 5007,

        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("跳斩特性", "当暗黑激活时将跳斩添加到主连击。", DRK.JobID, 0, "", "")]
        DRK_Plunge = 5008,

        [ParentCombo(DRK_Bloodspiller)]
        [CustomComboInfo("延后使用血溅 设置", "Delays Bloodspiller by 2 GCDs when Delirium is used during even windows, uses it regularly during odd windows. Useful for feeding into raid buffs at level 90.", DRK.JobID, 0, "", "")]
        DRK_DelayedBloodspiller = 5010,

        [ParentCombo(DRK_SouleaterCombo)]
        [CustomComboInfo("伤残特性", "Replace 噬魂斩连击 Feature with Unmend when you are out of range.", DRK.JobID, 0, "", "")]
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
        [CustomComboInfo("暗影锋爆发 设置", "在爆发期直到MP到达设定值之前都会一直插入暗影锋.", DRK.JobID, 0, "", "")]
        DRK_EoSPooling = 5020,

        [ParentCombo(DRK_Shadowbringer)]
        [CustomComboInfo("暗影使者爆发设置", "将暗影使者打入偶数分钟的爆发窗口中。", DRK.JobID, 0, "", "")]
        DRK_ShadowbringerBurst = 5021,

        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("精雕怒斩特性", "当自身存在暗影状态时插入精雕怒斩至主连击中.", DRK.JobID, 0, "", "")]
        DRK_CarveAndSpit = 5022,

        [ParentCombo(DRK_Plunge)]
        [CustomComboInfo("近战时使用跳斩 设置", "当你在目标圈内且自身存在暗影状态时在主连击中自动插入跳斩.\\n会使用上方选择的所有跳斩层数.", DRK.JobID, 0, "", "")]
        DRK_MeleePlunge = 5023,

        [ParentCombo(DRK_MainComboCDs_Group)]
        [CustomComboInfo("腐秽大地特性", "Adds Salted Earth on main combo while Darkside is up, will use Salt and Darkness if unlocked.", DRK.JobID, 0, "", "")]
        DRK_SaltedEarth = 5024,

        [ParentCombo(DRK_MainComboBuffs_Group)]
        [CustomComboInfo("血乱整合 设置", "当自身存在暗影状态且血乱可用时插入血乱.同样当血乱即将结束冷却时会根据自身暗血值先插入血溅/寂灭以防止暗血值溢出.", DRK.JobID, 0, "", "")]
        DRK_Delirium = 5025,

        [ParentCombo(DRK_MainComboBuffs_Group)]
        [CustomComboInfo("嗜血整合 设置", "若自身存在暗影状态且嗜血未在冷却中", DRK.JobID, 0, "", "")]
        DRK_BloodWeapon = 5026,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("嗜血 设置", "若自身存在暗影状态且嗜血未在冷却中", DRK.JobID, 0, "", "")]
        DRK_AoE_BloodWeapon = 5027,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("血乱 设置", "若自身存在暗影状态且嗜血未在冷却中", DRK.JobID, 0, "", "")]
        DRK_AoE_Delirium = 5028,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("腐秽大地 设置", "若自身存在暗影状态且腐秽大地未在冷却中", DRK.JobID, 0, "", "")]
        DRK_AoE_SaltedEarth = 5029,

        [ParentCombo(DRK_StalwartSoulCombo)]
        [CustomComboInfo("掠影示现 设置", "若自身存在暗影状态且掠影示现未在冷却中", DRK.JobID, 0, "", "")]
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

        [ReplaceSkill(DRG.Jump, DRG.HighJump)]
        [CustomComboInfo("跳跃 替换 幻象冲", "幻象冲准备就绪时，用幻象冲 替换 （高）跳跃。", DRG.JobID, 0)]
        DRG_Jump = 6000,

        #region Advanced Dragoon
        [ReplaceSkill(DRG.TrueThrust)]
        [CustomComboInfo("高级龙骑", "将 精准刺 设为单体连招的起始。", DRG.JobID, 1, "", "")]
        DRG_STCombo = 6100,

        [ParentCombo(DRG_STCombo)]
        [CustomComboInfo("88级起手", "将开场技能添加到战斗循环中。当战斗连祷和猛枪技能冷却后，并且在战斗外使用真北或在战斗开始时使用回避跳跃技能时激活。可选项：使用操作或鼠标以获得最佳目标选定", DRG.JobID, 0, "", "")]
        DRG_ST_Opener = 6101,

        [ParentCombo(DRG_STCombo)]
        [CustomComboInfo("主连击CD整合", "cd技能循环整合", DRG.JobID, 0, "", "")]
        DRG_ST_CDs = 6199,

        [ParentCombo(DRG_STCombo)]
        [CustomComboInfo("主连击Buff整合", "buff技能循环整合", DRG.JobID, 0, "", "")]
        DRG_ST_Buffs = 6198,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("天龙点睛选项", "将天龙点睛加入循环", DRG.JobID, 13, "", "")]
        DRG_ST_Wyrmwind = 6102,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("武神枪和死者之岸选项", "将武神枪和死者之岸加入循环", DRG.JobID, 18, "", "")]
        DRG_ST_GeirskogulNastrond = 6103,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("跳跃选项", "Adds Spineshatter Dive, Dragonfire Dive, and Stardiver to the rotation.\\n Select options below for when to use dives.", DRG.JobID, 14, "", "")]
        DRG_ST_Dives = 6104,


        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("高跳选项", "将高跳/跳跃加入循环", DRG.JobID, 19, "", "")]
        DRG_ST_HighJump = 6105,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("幻象冲选项", "将幻象冲加入循环", DRG.JobID, 20, "", "")]
        DRG_ST_Mirage = 6106,

        [ParentCombo(DRG_ST_Buffs)]
        [CustomComboInfo("猛枪选项", "猛枪加入循环", DRG.JobID, 21, "", "")]
        DRG_ST_Lance = 6107,

        [ParentCombo(DRG_ST_Buffs)]
        [CustomComboInfo("巨龙视线选项", "巨龙视线设置。注意：需要自行选择最优目标", DRG.JobID, 22, "", "")]
        DRG_ST_DragonSight = 6108,

        [ParentCombo(DRG_ST_Buffs)]
        [CustomComboInfo("战斗连祷选项", "将战斗连祷加入循环", DRG.JobID, 23, "", "")]
        DRG_ST_Litany = 6109,

        [ParentCombo(DRG_ST_CDs)]
        [CustomComboInfo("龙剑选项", "Includes Life Surge, while under proper buffs, onto proper GCDs, to the rotation.", DRG.JobID, 24, "", "")]
        DRG_ST_LifeSurge = 6110,

        [ParentCombo(DRG_STCombo)]
        [CustomComboInfo("超出近战范围选项", "超出近战范围时，将主要AOE连击替换为贯穿尖", DRG.JobID, 25, "", "")]
        DRG_ST_RangedUptime = 6111,

        [ParentCombo(DRG_ST_Dives)]
        [CustomComboInfo("冲冲冲设置", "Uses Spineshatter Dive, Dragonfire Dive, and Stardiver when in the target's target ring (1 yalm) and closer.", DRG.JobID, 14, "", "")]
        DRG_ST_Dives_Melee = 6112,

        [ParentCombo(DRG_STCombo)]
        [CustomComboInfo("回复设置", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", DRG.JobID, 0, "", "")]
        DRG_ST_ComboHeals = 6113,

        #endregion

        #region 高级龙骑 AoE
        [ReplaceSkill(DRG.DoomSpike)]
        [CustomComboInfo("高级龙骑AOE", "死天枪连击", DRG.JobID, 26, "", "")]
        DRG_AoECombo = 6200,

        [ParentCombo(DRG_AoECombo)]
        [CustomComboInfo("天龙点睛设置", "将天龙点睛插入AOE循环", DRG.JobID, 27, "", "")]
        DRG_AoE_WyrmwindFeature = 6201,

        [ParentCombo(DRG_AoECombo)]
        [CustomComboInfo("武神枪与死者之岸设置", "将武神枪与死者之岸插入AOE循环", DRG.JobID, 28, "", "")]
        DRG_AoE_GeirskogulNastrond = 6202,

        [ParentCombo(DRG_AoECombo)]
        [CustomComboInfo("跳跃类能力技设置", "Includes Spineshatter Dive, Dragonfire Dive and Stardiver to the AoE rotation.", DRG.JobID, 29, "", "")]
        DRG_AoE_Dives = 6203,

        [ParentCombo(DRG_AoECombo)]
        [CustomComboInfo("高跳设置", "在连击中插入高跳。", DRG.JobID, 33, "", "")]
        DRG_AoE_HighJump = 6204,

        [ParentCombo(DRG_AoECombo)]
        [CustomComboInfo("幻象冲设置", "在AOE连击中插入幻象冲", DRG.JobID, 34, "", "")]
        DRG_AoE_Mirage = 6205,

        #region Buffs AoE Feature
        [ParentCombo(DRG_AoECombo)]
        [CustomComboInfo("buff设置", "在连击中插入猛枪和战斗连祷。", DRG.JobID, 35, "", "")]
        DRG_AoE_Buffs = 6206,


        [ParentCombo(DRG_AoE_Buffs)]
        [CustomComboInfo("巨龙视线设置Aoe", "在连击中插入巨龙视线。需要自行选择最优目标。", DRG.JobID, 36, "", "")]
        DRG_AoE_DragonSight = 6207,
        #endregion

        [ParentCombo(DRG_AoECombo)]
        [CustomComboInfo("龙剑设置 AoE Feature", "Includes Life Surge, while under proper buffs, onto proper GCDs, to the AoE rotation.", DRG.JobID, 37, "", "")]
        DRG_AoE_LifeSurge = 6208,

        [ParentCombo(DRG_AoECombo)]
        [CustomComboInfo("超出近战范围选项", "超出近战范围时，将主要AOE连击替换为贯穿尖", DRG.JobID, 40, "", "")]
        DRG_AoE_RangedUptime = 6209,

        [ParentCombo(DRG_AoE_Dives)]
        [CustomComboInfo("冲冲冲设置", "Uses Spineshatter Dive, Dragonfire Dive, and Stardiver when in the target's target ring (1 yalm) and closer.", DRG.JobID, 29, "", "")]
        DRG_AoE_Dives_Melee = 6210,

        [ParentCombo(DRG_AoECombo)]
        [CustomComboInfo("回复设置", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", DRG.JobID, 0, "", "")]
        DRG_AoE_ComboHeals = 6211,

        #endregion

        [ReplaceSkill(DRG.Stardiver)]
        [CustomComboInfo("坠星冲设置", "Turns Stardiver into Nastrond during Life of the Dragon, and Geirskogul outside of Life of the Dragon.", DRG.JobID, 26, "", "")]
        DRG_StardiverFeature = 6300,

        [ReplaceSkill(DRG.LanceCharge)]
        [CustomComboInfo("猛枪整合到战斗连祷", "猛枪冷却完毕后整合至战斗连祷", DRG.JobID, 26, "", "")]
        DRG_BurstCDFeature = 6400,

        [ParentCombo(DRG_BurstCDFeature)]
        [CustomComboInfo("巨龙视线选项", "Adds Dragon Sight to Lance Charge, will take precedence over Battle Litany.", DRG.JobID, 26, "", "")]
        DRG_BurstCDFeature_DragonSight = 6401,

        [Variant]
        [VariantParent(DRG_STCombo, DRG_AoECombo)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", DRG.JobID)]
        DRG_Variant_Cure = 6500,

        [Variant]
        [VariantParent(DRG_STCombo, DRG_AoECombo)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", DRG.JobID)]
        DRG_Variant_Rampart = 6600,

        // Last value = 6600

        #endregion

        #region GUNBREAKER

        [CustomComboInfo("技能速度", "支持不同的技能速度", GNB.JobID, 0)]
        GNB_ST_SkSSupport = 7000,

        #region ST
        [ReplaceSkill(GNB.KeenEdge)]
        [CustomComboInfo("绝枪战士一键连击", "替换利刃斩和爆发击防止子弹溢出", GNB.JobID, 0, "", "")]
        GNB_ST_MainCombo = 7001,

        #region Gnashing Fang
        [ParentCombo(GNB_ST_MainCombo)]
        [CustomComboInfo("烈牙与续剑整合到主连击", "将烈牙与续剑整合到主连击。烈牙必须被手动激活，之后会被替换到主连击。", GNB.JobID, 0, "", "")]
        GNB_ST_Gnashing = 7002,

        [ParentCombo(GNB_ST_Gnashing)]
        [CustomComboInfo("烈牙启动", "先从烈牙开始倾泻晶壤.", GNB.JobID, 0, "", "")]
        GNB_ST_GnashingFang_Starter = 7003,
        #endregion

        #region Cooldowns
        [ParentCombo(GNB_ST_MainCombo)]
        [CustomComboInfo("暗黑骑士能力技整合 设置", "选择是否在无情状态下将各种能力技整合至主连击中", GNB.JobID, 0, "", "")]
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

        [ConflictingCombos(GNB_NoMercy_Cooldowns)]
        [ParentCombo(GNB_ST_MainCombo_CooldownsGroup)]
        [CustomComboInfo("无情整合到主连击", "当晶壤积累满时在主连击中插入无情.", GNB.JobID, 0, "", "")]
        GNB_ST_NoMercy = 7008,

        [ParentCombo(GNB_ST_MainCombo_CooldownsGroup)]
        [CustomComboInfo("弓形冲波整合到主连击", "弓形冲波整合到主连击", GNB.JobID, 0, "", "")]
        GNB_ST_BowShock = 7009,

        [ParentCombo(GNB_ST_MainCombo_CooldownsGroup)]
        [CustomComboInfo("音速破整合到主连击", "在主连击中插入音速破.", GNB.JobID, 0, "", "")]
        GNB_ST_SonicBreak = 7010,

        [ParentCombo(GNB_ST_MainCombo_CooldownsGroup)]
        [CustomComboInfo("爆发击整合到主连击", "当在无情状态下且已使用过烈牙后，在主连击中插入爆发击和超高速(若可用).", GNB.JobID, 0, "", "")]
        GNB_ST_BurstStrike = 7011,
        #endregion

        #region Rough Divide
        [ParentCombo(GNB_ST_MainCombo)]
        [CustomComboInfo("粗分斩设置", "当可用时在主连击中插入粗分斩.", GNB.JobID, 0, "", "")]
        GNB_ST_RoughDivide = 7012,

        [ParentCombo(GNB_ST_RoughDivide)]
        [CustomComboInfo("近战时插入粗分斩 设置", "Uses Rough Divide when under No Mercy, burst cooldowns when available, not moving, and in the target ring (1 yalm).\\nWill use as many stacks as selected in the above slider.", GNB.JobID, 0, "", "")]
        GNB_ST_MeleeRoughDivide = 7013,
        #endregion

        [ParentCombo(GNB_ST_MainCombo)]
        [CustomComboInfo("闪雷弹激活", "当自身与所选目标在近战距离外时，插入闪雷弹.", GNB.JobID, 0, "", "")]
        GNB_ST_RangedUptime = 7014,
        #endregion

        #region Gnashing Fang
        [ReplaceSkill(GNB.GnashingFang)]
        [CustomComboInfo("烈牙连击", "将续剑添加到烈牙.", GNB.JobID, 0, "", "")]
        GNB_GF_Continuation = 7200,

        [ParentCombo(GNB_GF_Continuation)]
        [CustomComboInfo("无情整合到烈牙", "当无情冷却结束时将其整合到烈牙.", GNB.JobID, 0, "", "")]
        GNB_GF_NoMercy = 7201,

        [ParentCombo(GNB_GF_Continuation)]
        [CustomComboInfo("倍攻整合到烈牙", "当拥有无情BUFF时将倍攻整合到烈牙", GNB.JobID, 0, "", "")]
        GNB_GF_DoubleDown = 7202,

        [ParentCombo(GNB_GF_Continuation)]
        [CustomComboInfo("烈牙CD选项", "Adds Bloodfest/Sonic Break/Bow Shock/Blasting Zone on Gnashing Fang, order dependent on No Mercy buff.\\nBurst Strike and Hypervelocity added if there's charges while No Mercy buff is up.", GNB.JobID, 0, "", "")]
        GNB_GF_Cooldowns = 7203,
        #endregion

        #region AoE
        [ReplaceSkill(GNB.DemonSlice)]
        [CustomComboInfo("绝枪战士AOE功能", "将恶魔杀替换为恶魔杀连击。", GNB.JobID, 0, "", "")]
        GNB_AoE_MainCombo = 7300,

        [ConflictingCombos(GNB_NoMercy_Cooldowns)]
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
        #endregion

        #region Burst Strike
        [ReplaceSkill(GNB.BurstStrike)]
        [CustomComboInfo("爆发击 Feature", "爆发击 相关功能", GNB.JobID, 0, "", "")]
        GNB_BS = 7400,


        [ParentCombo(GNB_BS)]
        [CustomComboInfo("爆发击续剑", "超高速可以使用的时候，超高速替换爆发击", GNB.JobID, 0, "", "")]
        GNB_BS_Continuation = 7401,

        [ParentCombo(GNB_BS)]
        [CustomComboInfo("血壤替换爆发击特性", "当你没有晶壤可用时使用血壤替换爆发击。", GNB.JobID, 0, "", "")]
        GNB_BS_Bloodfest = 7402,

        [ParentCombo(GNB_BS)]
        [CustomComboInfo("爆发击整合 设置", "无情状态下且有三颗晶壤时，用倍攻代替爆发击", GNB.JobID, 0, "", "")]
        GNB_BS_DoubleDown = 7403,
        #endregion

        #region No Mercy
        [ConflictingCombos(GNB_ST_NoMercy, GNB_AoE_NoMercy)]
        [ReplaceSkill(GNB.NoMercy)]
        [CustomComboInfo("无情", "无情冷却替换无情", GNB.JobID, 0, "", "")]
        GNB_NoMercy_Cooldowns = 7500,

        [ParentCombo(GNB_NoMercy_Cooldowns)]
        [CustomComboInfo("倍攻", "无情状态下，用倍攻代替无情", GNB.JobID, 0, "", "")]
        GNB_NoMercy_Cooldowns_DD = 7501,

        [ParentCombo(GNB_NoMercy_Cooldowns)]
        [CustomComboInfo("音速破/弓形冲波", "无情状态下，用音速破/弓形冲波代替无情", GNB.JobID, 0, "", "")]
        GNB_NoMercy_Cooldowns_SonicBreakBowShock = 7502,
        #endregion

        [CustomComboInfo("极光保护机制", "当选择目标身上已有极光状态时，将极光变为原初的勇猛.\\n 译者注:有bug，是自身身上有极光，将极光变为原初的勇猛", GNB.JobID, 0, "", "")]
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

        [ReplaceSkill(MCH.CleanShot, MCH.HeatedCleanShot, MCH.SplitShot, MCH.HeatedSplitShot)]
        [ConflictingCombos(MCH_ST_SimpleMode)]
        [CustomComboInfo("(热)狙击弹连击一键整合", "替换热弹为连击循环中的任一技能", MCH.JobID, 0, "", "")]
        MCH_ST_MainCombo = 8000,

        [ReplaceSkill(MCH.RookAutoturret, MCH.AutomatonQueen)]
        [CustomComboInfo("超档炮塔/人偶替换设置", "在技能可用时，将车式浮空炮塔和后式自走人偶转换为超档车式炮塔和超档自走人偶", MCH.JobID, 0, "", "")]
        MCH_Overdrive = 8002,

        [ReplaceSkill(MCH.GaussRound, MCH.Ricochet)]
        [CustomComboInfo("虹吸弹 / 弹射设置", "将虹吸弹和弹射替换为一个或其他需要更多充电电能的技能.", MCH.JobID, 0, "", "")]
        MCH_GaussRoundRicochet = 8003,

        [ReplaceSkill(MCH.Drill, MCH.AirAnchor, MCH.HotShot)]
        [CustomComboInfo("钻头/空气锚（热弹）选项", "钻头、空气锚 (热弹)、回转飞锯根据cd时间互相替换.", MCH.JobID, 0, "", "")]
        MCH_HotShotDrillChainSaw = 8004,

        [ParentCombo(MCH_ST_MainCombo)]
        [ConflictingCombos(MCH_ST_MainComboAlternate)]
        [CustomComboInfo("钻头/空气锚/回转飞锯整合 设置", "Air Anchor followed by Drill is added onto main combo if you use Reassemble.\\nIf Air Anchor is on cooldown and you use Reassemble, Chain Saw will be added to main combo instead.", MCH.JobID, 0, "", "")]
        MCH_ST_MainCombo_Cooldowns = 8005,

        [ReplaceSkill(MCH.HeatBlast)]
        [ConflictingCombos(MCH_ST_SimpleMode)]
        [CustomComboInfo("热冲击一键整合", "整合超荷至热冲击.", MCH.JobID, 0, "", "")]
        MCH_HeatblastGaussRicochet = 8006,

        [ReplaceSkill(MCH.AutoCrossbow)]
        [CustomComboInfo("自动弩一键整合", "将自动弩切换为超荷，并插入虹吸弹/弹射。", MCH.JobID, 0, "", "")]
        MCH_AutoCrossbowGaussRicochet = 8018,

        [ParentCombo(MCH_ST_MainCombo)]
        [ConflictingCombos(MCH_ST_MainCombo_Cooldowns)]
        [CustomComboInfo("交替插入钻头/空气锚/热弹被添加到连击循环中", "将钻头/空气锚/热弹插入主连击循环中(注意:只有当你在整备Buff中\\n或整备正在冷却中该选项才会生效。(若整备可用但却没有使用时则不会插入这些技能)", MCH.JobID, 0, "", "")]
        MCH_ST_MainComboAlternate = 8007,

        [ParentCombo(MCH_ST_MainCombo)]
        [CustomComboInfo("一键热冲击 添加到主循环选项", "当选项被启用时，在主循环中添加一键热冲击。", MCH.JobID, 0, "", "")]
        MCH_ST_MainCombo_HeatBlast = 8008,

        [ParentCombo(MCH_ST_MainCombo)]
        [CustomComboInfo("电量溢出后自动替换炮台/人偶", "Overcharge protection for your Battery, If you are at 100 battery charge Rook Autoturret/Automaton Queen will be added to your (Heated) Shot Combo.", MCH.JobID, 0, "", "")]
        MCH_ST_MainCombo_OverCharge = 8009,

        [ParentCombo(MCH_AoE_SimpleMode)]
        [CustomComboInfo("电量溢出后自动替换炮台/人偶", "为散射/霰弹枪增加了过电量保护。.", MCH.JobID, 0, "", "")]
        MCH_AoE_OverCharge = 8010,

        [ParentCombo(MCH_AoE_SimpleMode)]
        [CustomComboInfo("虹吸弹/弹射整合 设置", "在超荷过程中为AOE连击循环增加虹吸弹/弹射", MCH.JobID, 0, "", "")]
        MCH_AoE_GaussRicochet = 8011,

        [ParentCombo(MCH_AoE_GaussRicochet)]
        [CustomComboInfo("不限制虹吸弹/弹射的插入时机", "在超荷窗口之外的AOE连击循环中也会插入可用的虹吸弹/弹射.", MCH.JobID, 0, "", "")]
        MCH_AoE_Gauss = 8012,

        [ConflictingCombos(MCH_ST_MainCombo_RicochetGauss)]
        [ParentCombo(MCH_ST_MainCombo)]
        [CustomComboInfo("弹射 & 虹吸弹 设置", "将弹射和虹吸弹添加到主连击循环中.这将使用所有电能.", MCH.JobID, 0, "", "")]
        MCH_ST_MainCombo_RicochetGaussCharges = 8017,

        [ConflictingCombos(MCH_ST_MainCombo_RicochetGaussCharges)]
        [ParentCombo(MCH_ST_MainCombo)]
        [CustomComboInfo("Ricochet & Gauss Round Overcap Protection Option", "将弹射和虹吸弹添加到主连击循环中.将各留下一个电能.", MCH.JobID, 0, "", "")]
        MCH_ST_MainCombo_RicochetGauss = 8013,

        [ParentCombo(MCH_ST_MainCombo)]
        [CustomComboInfo("枪管加热避免延后浪费 设置", "如果热量在5-20之间，则将枪管加热添加到主连击循环中。", MCH.JobID, 0, "", "")]
        MCH_ST_BarrelStabilizer_DriftProtection = 8014,

        [ParentCombo(MCH_HeatblastGaussRicochet)]
        [CustomComboInfo("野火 设置", "Adds Wildfire to the 热冲击一键整合 if Wildfire is off cooldown and you have enough Heat Gauge for Hypercharge then Hypercharge will be replaced with Wildfire.\\nAlso weaves Ricochet/Gauss Round on Heat Blast when necessary.", MCH.JobID, 0, "", "")]
        MCH_ST_Wildfire = 8015,

        [ParentCombo(MCH_AoE_SimpleMode)]
        [CustomComboInfo("毒菌冲击整合 设置", "在散射的功能中添加毒菌冲击", MCH.JobID, 0, "", "")]
        MCH_AoE_Simple_Bioblaster = 8016,

        [CustomComboInfo("枪管加热 设置", "当低于50热度且冷却完毕时，在一键热冲击循环和一键自动弩循环中插入枪管加热", MCH.JobID, 0, "", "")]
        MCH_ST_AutoBarrel = 8019,

        [ReplaceSkill(MCH.SplitShot, MCH.HeatedSplitShot)]
        [ConflictingCombos(MCH_ST_MainCombo, MCH_HeatblastGaussRicochet)]
        [CustomComboInfo("一键简单机工循环", "Single button, single target machinist, including buffs and overcap protections.\\nConflicts with other single target toggles!\\nMade to work optimally with 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 2.5 GCD.\\nThe use of latency mitigation tools is recommended due to XIV's network handling.", MCH.JobID, 0, "", "")]
        MCH_ST_SimpleMode = 8020,

        [ParentCombo(MCH_ST_SimpleMode)]
        [CustomComboInfo("单体连击打断技能设置", "Uses interrupt during the rotation, if applicable.", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Interrupt = 8021,

        [ParentCombo(MCH_ST_SimpleMode)]
        [CustomComboInfo("简单的机工召唤", "Adds Automaton Queen or Rook Autoturret uses to the feature, based on your current level.\\nAttempts to use Automaton Queen at optimal intervals between :55 to :05 windows.", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Gadget = 8022,

        [ParentCombo(MCH_ST_SimpleMode)]
        [CustomComboInfo("简单的组装", "整备将与下面的技能一起使用.\\n在获得钻头之前，它将使用与狙击弹共用.", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Assembling = 8023,

        [ParentCombo(MCH_ST_SimpleMode)]
        [CustomComboInfo("简单的虹吸弹和弹射", "将虹吸弹和弹射加入到循环中.", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_GaussRicochet = 8024,

        [ParentCombo(MCH_ST_SimpleMode)]
        [CustomComboInfo("简单的野火和超荷", "Adds Hypercharge and Wildfire uses to the feature.\\nIt respects the 8 second rule of Drill, Air Anchor and Chain Saw.", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_WildCharge = 8025,

        [ParentCombo(MCH_ST_SimpleMode)]
        [CustomComboInfo("枪管加热", "添加 枪管加热 到循环。\\n当热量表<50，且野火已过冷却时间或即将过冷却时间。", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Stabilizer = 8026,

        [ParentCombo(MCH_AoE_SimpleMode)]
        [CustomComboInfo("超荷", "Adds Hypercharge to the AoE.", MCH.JobID, 0, "", "")]
        MCH_AoE_Simple_Hypercharge = 8027,

        [ReplaceSkill(MCH.SpreadShot)]
        [CustomComboInfo("机工简易AoE一键连击", "当机工士等级达到82级以上时，散射技能会升级为霰弹枪。当超荷时，散射和霰弹枪技能会转化为自动弩技能。毒菌冲击技能在CD结束后优先使用", MCH.JobID, 0, "", "")]
        MCH_AoE_SimpleMode = 8028,

        [ParentCombo(MCH_ST_Simple_Assembling)]
        [CustomComboInfo("钻头", "当可以的时候整备与钻头一起使用.", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Assembling_Drill = 8029,

        [ParentCombo(MCH_ST_Simple_Assembling)]
        [CustomComboInfo("空气锚", "当可以的时候整备与空气锚一起使用.", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Assembling_AirAnchor = 8030,

        [ParentCombo(MCH_ST_Simple_Assembling)]
        [CustomComboInfo("回转飞锯", "当可以的时候整备与回转飞锯一起使用.", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Assembling_ChainSaw = 8031,

        [ParentCombo(MCH_ST_Simple_Assembling_Drill)]
        [CustomComboInfo("只使用钻头", "仅在 整备 达到最大积蓄次数时才使用 钻头", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Assembling_Drill_MaxCharges = 8032,

        [ParentCombo(MCH_ST_Simple_Assembling_AirAnchor)]
        [CustomComboInfo("只使用空气锚", "仅在 整备 达到最大积蓄次数时才使用 空气锚", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Assembling_AirAnchor_MaxCharges = 8033,

        [ParentCombo(MCH_ST_Simple_Assembling_ChainSaw)]
        [CustomComboInfo("只使用回转飞锯", "仅在 整备 达到最大积蓄次数时才使用 回转飞锯", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Assembling_ChainSaw_MaxCharges = 8034,

        [ParentCombo(MCH_ST_Simple_Stabilizer)]
        [CustomComboInfo("在特定时机插入", "仅为野火使用枪管加热.", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_Stabilizer_Wildfire_Only = 8035,

        [ParentCombo(MCH_ST_SimpleMode)]
        [CustomComboInfo("高延迟模式", "高ping友好模式 \\n它限制了在 超荷 持续时间内使用 虹吸弹/弹射 以防止漂移。\\n使用此功能时预计会有少量 DPS 损失。", MCH.JobID, 0, "", "")]
        MCH_ST_Simple_High_Latency_Mode = 8036,

        [ParentCombo(MCH_ST_SimpleMode)]
        [CustomComboInfo("内丹选项", "当低于此生命值百分比时，使用 内丹", MCH.JobID, 0, "", "")]
        MCH_ST_SecondWind = 8037,

        [ParentCombo(MCH_AoE_SimpleMode)]
        [CustomComboInfo("内丹选项", "当低于此生命值百分比时，使用 内丹", MCH.JobID, 0, "", "")]
        MCH_AoE_SecondWind = 8038,

        [Variant]
        [VariantParent(MCH_ST_SimpleMode, MCH_AoE_SimpleMode)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", MCH.JobID)]
        MCH_Variant_Rampart = 8039,

        [Variant]
        [VariantParent(MCH_ST_SimpleMode, MCH_AoE_SimpleMode)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", MCH.JobID)]
        MCH_Variant_Cure = 8040,

        // Last value = 8040

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
        [CustomComboInfo("连击连击", "将连击替换为其 Combo 链。\\n如果启用所有子选项，将变成武僧的一键循环。滑块的值可以用来控制功力 buff 和破碎拳 dot 的正常运行时间。", MNK.JobID, -2, "", "")]
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
        [CustomComboInfo("阴阳开场", "Start with the 阴阳开场 on the main combo. Requires level 68 for Riddle of Fire.\\nA 1.93/1.94 GCD is highly recommended.", MNK.JobID, 0, "", "")]
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
        [CustomComboInfo("回复设置", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", MNK.JobID, 0, "", "")]
        MNK_ST_ComboHeals = 9026,

        [ParentCombo(MNK_AoE_SimpleMode)]
        [CustomComboInfo("回复设置", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", MNK.JobID, 0, "", "")]
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
        [CustomComboInfo("下忍模式 - 单目标", "将双刃旋替换为一键单目标连击。\\n一键输出，下忍的理想之选。", NIN.JobID)]
        NIN_ST_SimpleMode = 10000,

        [ParentCombo(NIN_ST_SimpleMode)]
        [CustomComboInfo("平衡起手", "Starts with the Balance opener.\\nDoes pre-pull first, if you enter combat before hiding the opener will fail.\\nLikewise, moving during TCJ will cause the opener to fail too.\\nRequires you to be out of combat with majority of your cooldowns available for it to work.", NIN.JobID)]
        NIN_ST_SimpleMode_BalanceOpener = 10001,

        [ReplaceSkill(NIN.DeathBlossom)]
        [ConflictingCombos(NIN_AoE_AdvancedMode)]
        [CustomComboInfo("下忍模式 - AoE", "将血雨飞花替换为一键AOE连击", NIN.JobID)]
        NIN_AoE_SimpleMode = 10002,

        [ReplaceSkill(NIN.SpinningEdge)]
        [ConflictingCombos(NIN_ST_SimpleMode)]
        [CustomComboInfo("上忍模式 - 单目标", "将双刃旋替换为一键单目标连击。\\n自定义循环，上忍的理想之选。", NIN.JobID)]
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
        [CustomComboInfo("平衡起手", "Starts with the Balance opener.\\nDoes pre-pull first, if you enter combat before hiding the opener will fail.\\nLikewise, moving during TCJ will cause the opener to fail too.\\nRequires you to be out of combat with majority of your cooldowns available for it to work.", NIN.JobID)]
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
        [CustomComboInfo("上忍模式 - AoE", "将血雨飞花替换为一键群体连击。\\n自定义循环，上忍的理想之选。", NIN.JobID)]
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

        [ConflictingCombos(NIN_ST_AdvancedMode_BalanceOpener, NIN_ST_AdvancedMode_BalanceOpener, NIN_ST_AdvancedMode_Kassatsu, NIN_AoE_AdvancedMode_Kassatsu, NIN_KassatsuChiJin)]
        [ReplaceSkill(NIN.Kassatsu)]
        [CustomComboInfo("攻其不备 替换 生杀予夺", "隐遁状态下或发动水遁之术后，使用 攻其不备 替换 生杀予夺.\\n推荐同时使用冷却CD监视插件.", NIN.JobID)]
        NIN_KassatsuTrick = 10054,

        [ReplaceSkill(NIN.TenChiJin)]
        [CustomComboInfo("命水 替换 天地人", "发动水遁之术 后，使用 命水 替换 天地人.\\n推荐同时使用冷却CD监视插件.", NIN.JobID)]
        NIN_TCJMeisui = 10055,

        [ConflictingCombos(NIN_ST_AdvancedMode_BalanceOpener, NIN_ST_AdvancedMode_BalanceOpener, NIN_KassatsuTrick, NIN_ST_AdvancedMode_Kassatsu, NIN_AoE_AdvancedMode_Kassatsu)]
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

        //// Last value = 11032

        [ConflictingCombos(PLD_ST_AdvancedMode)]
        [ReplaceSkill(PLD.FastBlade)]
        [CustomComboInfo("骑士简单模式 - 单目标", $"一键输出(先锋剑)", PLD.JobID)]
        PLD_ST_SimpleMode = 11000,

        [ConflictingCombos(PLD_AoE_AdvancedMode)]
        [ReplaceSkill(PLD.TotalEclipse)]
        [CustomComboInfo("骑士简单模式 - AoE", $"一键输出(全蚀斩)", PLD.JobID)]
        PLD_AoE_SimpleMode = 11001,

        [ConflictingCombos(PLD_ST_SimpleMode)]
        [ReplaceSkill(PLD.FastBlade)]
        [CustomComboInfo("骑士高级模式 - 单目标", $"自定义循环链(先锋剑)", PLD.JobID)]
        PLD_ST_AdvancedMode = 11002,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("起手设置", "设置多少GCD起手", PLD.JobID)]
        PLD_ST_AdvancedMode_Open = 110034,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("战逃反应 Option", "Adds 战逃反应 to Advanced Mode.", PLD.JobID)]
        PLD_ST_AdvancedMode_FoF = 11003,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("投盾 Option", "Adds 投盾 to Advanced Mode if out of range.", PLD.JobID)]
        PLD_ST_AdvancedMode_ShieldLob = 11004,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("厄运流转 Option", "Adds 厄运流转 to Advanced Mode.", PLD.JobID)]
        PLD_ST_AdvancedMode_CircleOfScorn = 11005,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("深奥之灵 / 偿赎剑 Option", "Adds 深奥之灵 / 偿赎剑 to Advanced Mode", PLD.JobID)]
        PLD_ST_AdvancedMode_SpiritsWithin = 11006,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("盾阵 / 圣盾阵 Option", "Adds 盾阵 / 圣盾阵 Sheltron to Advanced Mode", PLD.JobID)]
        PLD_ST_AdvancedMode_Sheltron = 11007,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("沥血剑 Option", "Adds 沥血剑 to Advanced Mode", PLD.JobID)]
        PLD_ST_AdvancedMode_GoringBlade = 11008,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("圣灵 Option", "Adds 圣灵 to Advanced Mode", PLD.JobID)]
        PLD_ST_AdvancedMode_HolySpirit = 11009,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("安魂祈祷 Option", "Adds 安魂祈祷 to Advanced Mode", PLD.JobID)]
        PLD_ST_AdvancedMode_Requiescat = 11010,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("调停 Option", "Adds 调停 to Advanced Mode", PLD.JobID)]
        PLD_ST_AdvancedMode_Intervene = 11011,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("恕罪剑 Option", "Adds 恕罪剑 to Advanced Mode", PLD.JobID)]
        PLD_ST_AdvancedMode_Atonement = 11012,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("悔罪 Option", "Adds 悔罪 to Advanced Mode", PLD.JobID)]
        PLD_ST_AdvancedMode_Confiteor = 11013,

        [ParentCombo(PLD_ST_AdvancedMode)]
        [CustomComboInfo("Blades of 信仰/真理/英勇 Option", "Adds Blades of 信仰/真理/英勇 to Advanced Mode", PLD.JobID)]
        PLD_ST_AdvancedMode_Blades = 11014,

        [ConflictingCombos(PLD_AoE_SimpleMode)]
        [ReplaceSkill(PLD.TotalEclipse)]
        [CustomComboInfo("骑士高级模式 - AoE", $"自定义循环链(全蚀斩)", PLD.JobID)]
        PLD_AoE_AdvancedMode = 11015,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("战逃反应 AOE Option", "Adds 战逃反应 to Advanced Mode.", PLD.JobID)]
        PLD_AoE_AdvancedMode_FoF = 11016,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("Spirits 深奥之灵 AOE Option", "Adds 深奥之灵 to Advanced Mode", PLD.JobID)]
        PLD_AoE_AdvancedMode_SpiritsWithin = 11017,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("厄运流转 AOE Option", "Adds 厄运流转 to Advanced Mode.", PLD.JobID)]
        PLD_AoE_AdvancedMode_CircleOfScorn = 11018,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("安魂祈祷 AOE Option", "Adds 安魂祈祷 to Advanced Mode.", PLD.JobID)]
        PLD_AoE_AdvancedMode_Requiescat = 11019,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("圣环 AOE Option", "Adds 圣环 to Advanced Mode.", PLD.JobID)]
        PLD_AoE_AdvancedMode_HolyCircle = 11020,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("悔罪 AOE Option", "Adds 悔罪 to Advanced Mode", PLD.JobID)]
        PLD_AoE_AdvancedMode_Confiteor = 11021,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("Blades of 信仰/真理/英勇 AOE Option", "Adds Blades of 信仰/真理/英勇 to Advanced Mode", PLD.JobID)]
        PLD_AoE_AdvancedMode_Blades = 11022,

        [ParentCombo(PLD_AoE_AdvancedMode)]
        [CustomComboInfo("盾阵 / 圣盾阵 AOE Option", "Adds 盾阵 / 圣盾阵 to Advanced Mode", PLD.JobID)]
        PLD_AoE_AdvancedMode_Sheltron = 11023,


        [ReplaceSkill(PLD.Requiescat)]
        [CustomComboInfo("安魂祈祷 Spender Option", "当安魂祈祷状态下用下面的技能替换", PLD.JobID)]
        PLD_Requiescat_Options = 11024,

        [ReplaceSkill(PLD.SpiritsWithin, PLD.Expiacion)]
        [CustomComboInfo("深奥之灵/偿赎剑  Feature", "当厄运流转冷却用(深奥之灵/偿赎剑)代替(好像有bug)", PLD.JobID)]
        PLD_SpiritsWithin = 11025,
        
        
       

        [Variant]
        [VariantParent(PLD_ST_SimpleMode, PLD_ST_AdvancedMode, PLD_AoE_SimpleMode, PLD_AoE_AdvancedMode)]
        [CustomComboInfo("精神镖选项", "当没有dot或dot剩余时间少于3s时，使用多变精神镖", PLD.JobID)]
        PLD_Variant_SpiritDart = 11030,

        [Variant]
        [VariantParent(PLD_ST_SimpleMode, PLD_ST_AdvancedMode, PLD_AoE_SimpleMode, PLD_AoE_AdvancedMode)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", PLD.JobID)]
        PLD_Variant_Cure = 11031,

        [Variant]
        [VariantParent(PLD_ST_SimpleMode, PLD_ST_AdvancedMode, PLD_AoE_SimpleMode, PLD_AoE_AdvancedMode)]
        [CustomComboInfo("最后通牒 选项", "冷却结束时使用多变最后通牒", PLD.JobID)]
        PLD_Variant_Ultimatum = 11032,

        //// Last value = 11032

        #endregion

        #region REAPER

        [CustomComboInfo("身位喜好", "为身位相关的功能选择身位喜好", RPR.JobID, 0, "", "")]
        ReaperPositionalConfig = 12000,

        #region Single Target (Slice) Combo Section
        [ReplaceSkill(RPR.Slice)]
        [CustomComboInfo("切割连击", "将切割替换为切割连击。如果启用了所有子选项将变为一键循环(Advanced 镰刀)", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo = 12001,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("灵魂切割设置", "当魂衣值低于50点，并且目标拥有死亡烙印Debuff时，将灵魂切割加入连击。", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_SoulSlice = 12002,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("死亡之影设置", "Adds Shadow of Death to Slice Combo if Death's Design is not present on current target, or is about to expire.", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_SoD = 12003,

        [ParentCombo(RPR_ST_SliceCombo_SoD)]
        [CustomComboInfo("夜游魂衣双死亡之影设置", "前两次的夜游魂衣的2分钟爆发期时使用两次死亡之影(双夜游魂衣爆发)。", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_SoD_Double = 12004,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("下踢设置", "当目标的施法可以被打断时，将下踢加入连击。", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_Stun = 12005,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("回复设置", "Adds Bloodbath and Second Wind to the combo at 65%% and 40%% HP, respectively.", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_ComboHeals = 12006,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("距离设置", "Replaces the combo chain with Harpe (or Harvest Moon, if available) when outside of melee range. Will not override Communio.", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_RangedFiller = 12007,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("夜游魂衣设置", "当目标有死亡烙印Debuff并且魂衣大于等于50时将夜游魂衣加入连击。", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_Enshroud = 12008,

        [ParentCombo(RPR_ST_SliceCombo_Enshroud)]
        [CustomComboInfo("夜游魂衣爆发(双夜游魂衣)设置", "Uses Enshroud at 50 Shroud during Arcane Circle (mimics the 2-minute Double Enshroud window) and will use Enshroud for odd minute bursts.\\nBelow level 88, will use Enshroud at 50 gauge.", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_EnshroudPooling = 12009,

        [ParentCombo(RPR_ST_SliceCombo_GibbetGallows)]
        [CustomComboInfo("夜游魂切割设置", "当拥有2层虚无魂时将夜游魂切割加入连击。", RPR.JobID, 1, "", "")]
        RPR_ST_SliceCombo_GibbetGallows_Lemure = 12010,

        [ParentCombo(RPR_ST_SliceCombo_GibbetGallows)]
        [CustomComboInfo("团契设置", "当剩余1层夜游魂时将团契加入连击。", RPR.JobID, 1, "", "")]
        RPR_ST_SliceCombo_GibbetGallows_Communio = 12011,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("神秘环设置", "当神秘环可用并且目标有死亡烙印Debuff时将其加入连击。", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_ArcaneCircle = 12012,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("大丰收设置", "当大丰收可用时将其加入连击。", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_PlentifulHarvest = 12013,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("绞决/缢杀设置", "当目标附加 死亡烙印 状态时，将 绞决 和 缢杀 加入到主循环中。", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_GibbetGallows = 12014,

        [ParentCombo(RPR_ST_SliceCombo_GibbetGallows)]
        [CustomComboInfo("虚无收割/交错收割 选项", "当自身处在 夜游魂 状态期间，将 虚无收割 和 交错收割 加入到主循环中。", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_GibbetGallows_VoidCross = 12065,

        [ReplaceSkill(RPR.ShadowOfDeath)]
        [ParentCombo(RPR_ST_SliceCombo_GibbetGallows)]
        [CustomComboInfo("绞决/缢杀替换死亡之影设置", "如果在身位喜好里设置了，将死亡之影替换为绞决/缢杀", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_GibbetGallows_OnSoD = 12015,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("暴食与隐匿挥割设置", "Adds Gluttony and Blood Stalk to the combo when target is afflicted with Death's Design, and the skills are off cooldown and < 50 soul.", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_GluttonyBloodStalk = 12016,

        [ParentCombo(RPR_ST_SliceCombo_GibbetGallows_Communio)]
        [CustomComboInfo("移动时团契", "当移动时使用死亡之影替换团契", RPR.JobID, 0, "", "")]
        RPR_ST_SliceCombo_GibbetGallows_Communio_Movement = 12017,

        [ParentCombo(RPR_ST_SliceCombo)]
        [CustomComboInfo("90级起手选项", "将90级起手加入主连击，在下面选用哪个起手.", RPR.JobID, -1, "", "")]
        RPR_ST_SliceCombo_Opener = 12018,
        #endregion

        #region AoE (Scythe) Combo Section
        [ReplaceSkill(RPR.SpinningScythe)]
        [CustomComboInfo("旋转钐割连击", "旋转钐割加入循环。\\n如果启用所有子选项则变为一键循环（简单AOE）", RPR.JobID, 0, "", "")]
        RPR_AoE_ScytheCombo = 12020,

        [ParentCombo(RPR_AoE_ScytheCombo)]
        [CustomComboInfo("灵魂钐割设置", "当目标有死亡烙印Debuff并且灵魂小于50 将灵魂钐割加入AOE连击", RPR.JobID, 0, "", "")]
        RPR_AoE_ScytheCombo_SoulScythe = 12021,

        [ParentCombo(RPR_AoE_ScytheCombo)]
        [CustomComboInfo("死亡之涡设置", "Adds Whorl of Death to AoE combo if Death's Design is not present on current target, or is about to expire.", RPR.JobID, 0, "", "")]
        RPR_AoE_ScytheCombo_WoD = 12022,

        [ParentCombo(RPR_AoE_ScytheCombo)]
        [CustomComboInfo("断首设置", "当目标有死亡烙印Debuff且处于妖异之镰状态下时将断首加入AOE连击。\\n在夜游魂衣时会重复使用阴冷收割。", RPR.JobID, 0, "", "")]
        RPR_AoE_ScytheCombo_Guillotine = 12023,

        [ParentCombo(RPR_AoE_ScytheCombo_Guillotine)]
        [CustomComboInfo("阴冷收割 选项", "当自身处在 夜游魂 状态期间，将 阴冷收割 加入到主循环中。", RPR.JobID, 0, "", "")]
        RPR_AoE_ScytheCombo_Guillotine_GrimReaping = 12066,

        [ParentCombo(RPR_AoE_ScytheCombo)]
        [CustomComboInfo("神秘环设置", "当神秘环不在冷却时将其加入AOE连击。", RPR.JobID, 0, "", "")]
        RPR_AoE_ScytheCombo_ArcaneCircle = 12024,

        [ParentCombo(RPR_AoE_ScytheCombo_ArcaneCircle)]
        [CustomComboInfo("大丰收设置", "当大丰收不在冷却且准备好时将其加入AOE连击。", RPR.JobID, 0, "", "")]
        RPR_AoE_ScytheCombo_PlentifulHarvest = 12025,

        [ParentCombo(RPR_AoE_ScytheCombo)]
        [CustomComboInfo("夜游魂衣设置", "当目标有死亡烙印Debuff且魂衣大于等于50时将夜游魂衣加入AOE连击。", RPR.JobID, 0, "", "")]
        RPR_AoE_ScytheCombo_Enshroud = 12026,

        [ParentCombo(RPR_AoE_ScytheCombo_Guillotine)]
        [CustomComboInfo("夜游魂钐割 选项", "当有2个虚无魂时，将 夜游魂钐割 加入到AOE连击循环。", RPR.JobID, 1, "", "")]
        RPR_AoE_ScytheCombo_Lemure = 12027,

        [ParentCombo(RPR_AoE_ScytheCombo_Guillotine)]
        [CustomComboInfo("团契设置", "当剩余1层夜游魂时将团契加入AOE连击。", RPR.JobID, 2, "", "")]
        RPR_AoE_ScytheCombo_Communio = 12028,

        [ParentCombo(RPR_AoE_ScytheCombo)]
        [CustomComboInfo("暴食与束缚挥割设置", "当目标有死亡烙印Debuff并且灵魂小于50时将暴食与束缚挥割加入AOE连击。", RPR.JobID, 0, "", "")]
        RPR_AoE_ScytheCombo_GluttonyGrimSwathe = 12029,
        #endregion

        #region Blood Stalk/Grim Swathe Combo Section
        [ReplaceSkill(RPR.BloodStalk, RPR.GrimSwathe)]
        [CustomComboInfo("暴食替换隐匿挥割/束缚挥割功能", "当暴食可用时将其替换到隐匿挥割/束缚挥割上。", RPR.JobID, 0, "", "")]
        RPR_GluttonyBloodSwathe = 12041,

        [ParentCombo(RPR_GluttonyBloodSwathe)]
        [CustomComboInfo("绞决/缢杀/断首替换隐匿挥割/束缚挥割功能", "将隐匿挥割替换为绞决/缢杀。\\n将束缚挥割替换为断首。", RPR.JobID, 0, "", "")]
        RPR_GluttonyBloodSwathe_BloodSwatheCombo = 12040,

        [ParentCombo(RPR_GluttonyBloodSwathe)]
        [CustomComboInfo("夜游魂衣连击设置", "Adds Enshroud combo (Void/Cross Reaping, Communio, and Lemure's Slice) on Blood Stalk and Grim Swathe.", RPR.JobID, 0, "", "")]
        RPR_GluttonyBloodSwathe_Enshroud = 12042,
        #endregion

        #region Miscellaneous
        [ReplaceSkill(RPR.ArcaneCircle)]
        [CustomComboInfo("神秘环大丰收特性。", "当你拥有死亡祭品层数时，使用大丰收替换神秘环。", RPR.JobID, 0, "", "")]
        RPR_ArcaneCirclePlentifulHarvest = 12051,

        [ReplaceSkill(RPR.HellsEgress, RPR.HellsIngress)]
        [CustomComboInfo("回退特性", "当你拥有回退预备状态时，将地狱入境与地狱出境替换为回退。", RPR.JobID, 0, "", "")]
        RPR_Regress = 12052,

        [ReplaceSkill(RPR.Slice, RPR.SpinningScythe, RPR.ShadowOfDeath, RPR.Harpe, RPR.BloodStalk)]
        [CustomComboInfo("播魂种提醒功能", "脱战时将魂播种替换至下列选择的技能上\\n在战斗中如果没有选择目标也会将播魂种替换至勾刃上", RPR.JobID, 0, "", "")]
        RPR_Soulsow = 12053,

        [ReplaceSkill(RPR.Harpe)]
        [ParentCombo(RPR_Soulsow)]
        [CustomComboInfo("勾刃收获月特性", "当你拥有播魂种BUFF时，使用收获月替换勾刃。", RPR.JobID, 0, "", "")]
        RPR_Soulsow_HarpeHarvestMoon = 12054,

        [ReplaceSkill(RPR.Harpe, RPR.Slice)]
        [ParentCombo(RPR_Soulsow)]
        [CustomComboInfo("勾刃效果提高设置", "防止收获月在拥有勾刃效果提高时替换勾刃。", RPR.JobID, 0, "", "")]
        RPR_Soulsow_HarpeHarvestMoon_EnhancedHarpe = 12055,

        [ReplaceSkill(RPR.Harpe, RPR.Slice)]
        [ParentCombo(RPR_Soulsow)]
        [CustomComboInfo("战斗外勾刃设置", "防止收获月在非战斗状态下替换勾刃。", RPR.JobID, 0, "", "")]
        RPR_Soulsow_HarpeHarvestMoon_CombatHarpe = 12056,

        [ReplaceSkill(RPR.Enshroud)]
        [CustomComboInfo("夜游魂衣保护功能", "将夜游魂衣替换为绞决/缢杀来防止妖异之镰浪费。", RPR.JobID, 0, "", "")]
        RPR_EnshroudProtection = 12057,

        [ReplaceSkill(RPR.Gibbet, RPR.Gallows, RPR.Guillotine)]
        [CustomComboInfo("团契替换绞决/缢杀/断首功能", "将团契加入到绞决/缢杀/断首", RPR.JobID, 0, "", "")]
        RPR_CommunioOnGGG = 12058,

        [ParentCombo(RPR_CommunioOnGGG)]
        [CustomComboInfo("夜游魂切割/钐割设置", "将夜游魂切割加入到绞决/缢杀", RPR.JobID, 0, "", "")]
        RPR_LemureOnGGG = 12060,

        [ReplaceSkill(RPR.Enshroud)]
        [CustomComboInfo("夜游魂衣替换团契功能", "当夜游魂衣可用时替换团契。", RPR.JobID, 0, "", "")]
        RPR_EnshroudCommunio = 12059,

        [ReplaceSkill(RPR.Slice, RPR.ShadowOfDeath, RPR.Enshroud)]
        [CustomComboInfo("真北 开关", "Adds True North to Slice, Shadow of Death, Enshroud, and Blood Stalk when under Gluttony and if Gibbet/Gallows options are selected to replace those skills.", RPR.JobID, 0)]
        RPR_TrueNorth = 12061,

        [ReplaceSkill(RPR.Harpe)]
        [ParentCombo(RPR_Soulsow)]
        [CustomComboInfo("战斗中播魂种提醒", "没有目标选择时 将播魂种替换至勾刃上", RPR.JobID, 0, "", "")]
        RPR_Soulsow_Combat = 12062,

        [ReplaceSkill(RPR.Gibbet, RPR.Gallows)]
        [CustomComboInfo("动态真北", "当你不在触发增益状态的正确身位时，将 真北 加入到 绞决/缢杀 释放前的 切割 之前。", RPR.JobID, 0, "", "")]
        RPR_TrueNorthDynamic = 12063,

        [ParentCombo(RPR_TrueNorthDynamic)]
        [CustomComboInfo("为 暴食 保持真北 选项", "Will hold the last charge of True North for use with Gluttony, even when out of position for Gibbet/Gallows potency bonuses.", RPR.JobID, 0, "", "")]
        RPR_TrueNorthDynamic_HoldCharge = 12064,

        [Variant]
        [VariantParent(RPR_ST_SliceCombo, RPR_AoE_ScytheCombo)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", RPR.JobID)]
        RPR_Variant_Cure = 12067,

        [Variant]
        [VariantParent(RPR_ST_SliceCombo, RPR_AoE_ScytheCombo)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", RPR.JobID)]
        RPR_Variant_Rampart = 12068,
        #endregion

        // Last value = 12068

        #endregion

        #region RED MAGE

        /* RDM Feature Numbering
        Numbering Scheme: 13[Section][Feature Number][Sub-Feature]
        Example: 13110 (Section 1: Openers, Feature Number 1, Sub-feature 0)
        New features should be added to the appropriate sections.
        If more than 10 sub features, use the next feature number if available
        The three digets after RDM.JobID can be used to reorder items in the list
        */
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
        [CustomComboInfo("划圆斩 设置", "当你的两种魔元都在60以上时，使用划圆斩", RDM.JobID, 420)]
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
        #endregion

        #endregion

        #region SAGE

        /* SGE Feature Numbering
        Numbering Scheme: 14[Feature][Option][Sub-Option]
        Example: 14110 (Feature Number 1, Option 1, no suboption)
        New features should be added to the appropriate sections.
        */

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
        #endregion

        #region Diagnosis Simple Single Target Heal
        [ReplaceSkill(SGE.Diagnosis)]
        [CustomComboInfo("单目标治疗功能", "支持软目标。\\n以下选项按优先顺序排列。", SGE.JobID, 300, "", "")]
        SGE_ST_Heal = 14300,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("康复 选项", "当你的目标有一个可清除的debuff时则使用 康复。", SGE.JobID, 301, "", "")]
        SGE_ST_Heal_Esuna = 14301,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("心关", "如果心关从未使用过，则对选中目标使用心关。", SGE.JobID, 305, "", "")]
        SGE_ST_Heal_Kardia = 14310,

        [ParentCombo(SGE_ST_Heal)]
        [CustomComboInfo("均衡诊断选项", "当所选目标没有盾值时，替换诊断为均衡诊断。", SGE.JobID, 313, "", "")]
        SGE_ST_Heal_Diagnosis = 14320,


        [ParentCombo(SGE_ST_Heal_Diagnosis)]
        [CustomComboInfo("忽略护盾检查，强制整合", "Warning, will force the use of Eukrasia Diagnosis, and normal Diagnosis will be unavailable.", SGE.JobID)]
        SGE_ST_Heal_Diagnosis_IgnoreShield = 14321,

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

        [ParentCombo(SGE_AoE_Heal_EPrognosis)]
        [CustomComboInfo("忽略护盾检查，强制整合", "Warning, will force the use of Eukrasia Prognosis, and normal Prognosis will be unavailable.", SGE.JobID, 520, "", "")]
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
        [CustomComboInfo("月光连", "替换 月光 技能为它的连击链。\\n如果下面的全部子选项都有相应选择会形成一个一键连击循环 （高级武士）", SAM.JobID, 0, "", "")]
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
        [CustomComboInfo("90级武士起手爆发", "Adds the Level 90 Opener to the main combo.\\nOpener triggered by using Meikyo Shisui before combat. If you have any Sen, Hagakure will be used to clear them.\\nWill work at any levels of Kenki, requires 2 charges of Meikyo Shisui and all CDs ready. If conditions aren't met it will skip into the regular rotation. \\nIf the Opener is interrupted, it will exit the opener via 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 Goken and 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 Kaeshi: Goken at the end or via the last Yukikaze. If the latter, CDs will be used on cooldown regardless of burst options.", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_Opener = 15007,

        [ConflictingCombos(SAM_GyotenYaten)]
        [ParentCombo(SAM_ST_GekkoCombo)]
        [CustomComboInfo("填充技能选项", "在适当的时候将选定的填充技能组合添加到输出循环中。选择技能速度与下面的Fuka buff。\\n在你死亡或不使用开场爆发的情况下失效。", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_FillerCombos = 15008,

        #region CDs on Main Combo
        [ParentCombo(SAM_ST_GekkoCombo)]
        [CustomComboInfo("主连击CD整合", "将CD技能加入循环", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs = 15099,

        [ParentCombo(SAM_ST_GekkoCombo_CDs)]
        [CustomComboInfo("将意气冲天加入循环", "当剑气达到或低于50时，可将意气冲天加入到月光和满月连击中。\\n将在义气冲天还剩10秒时消耗剑气，防止剑气溢出。", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs_Ikishoten = 15009,

        [ParentCombo(SAM_ST_GekkoCombo_CDs)]
        [CustomComboInfo("将居合术加入循环", "Adds Midare: Setsugekka, Higanbana, and Kaeshi: Setsugekka when ready and when you're not moving to main combo.", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs_Iaijutsu = 15010,

        #region Ogi Namikiri on Main Combo
        [ParentCombo(SAM_ST_GekkoCombo_CDs)]
        [CustomComboInfo("将奥义斩浪加入循环", "在未移动和技能冷却完毕的情况下，将奥义斩浪和回返斩浪加入循环。", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs_OgiNamikiri = 15011,

        [ParentCombo(SAM_ST_GekkoCombo_CDs_OgiNamikiri)]
        [CustomComboInfo("奥义斩浪爆发选项", "Saves Ogi Namikiri for even minute burst windows.\\nIf you don't activate the opener or die, Ogi Namikiri will instead be used on CD.", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs_OgiNamikiri_Burst = 15012,
        #endregion

        [ParentCombo(SAM_ST_GekkoCombo_CDs)]
        [CustomComboInfo("将明镜止水加入循环", "当冷却完毕时将明镜止水加入循环", SAM.JobID, 0, "", "")]
        SAM_ST_GekkoCombo_CDs_MeikyoShisui = 15013,

        #region Meikyo Shisui on Main Combo
        [ParentCombo(SAM_ST_GekkoCombo_CDs_MeikyoShisui)]
        [CustomComboInfo("明镜止水爆发选项", "Saves Meikyo Shisui for burst windows.\\nIf you don't activate the opener or die, Meikyo Shisui will instead be used on CD.", SAM.JobID, 0, "", "")]
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
        [CustomComboInfo("必杀剑·闪影爆发选项", "Saves Senei for even minute burst windows.\\nIf you don't activate the opener or die, Senei will instead be used on CD.", SAM.JobID, 0, "", "")]
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
        [CustomComboInfo("满月连", "用满月连击的组合代替满月。\\n如果所有子选项都被选中，将变成一个完整的一键AOE循环（高级武士）。", SAM.JobID, 0, "", "")]
        SAM_AoE_MangetsuCombo = 15020,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [ConflictingCombos(SAM_AoE_OkaCombo_TwoTarget)]
        [CustomComboInfo("樱花 添加到 满月 连击", "将樱花连击加如到AOE连击中。\\n樱花连击中将试情况使用明镜止水", SAM.JobID, 0, "", "")]
        SAM_AoE_MangetsuCombo_Oka = 15021,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("将居合术加入AOE连击", "Adds Tenka Goken, Midare: Setsugekka, and Kaeshi: Goken when ready and when you're not moving to Mangetsu combo.", SAM.JobID, 0, "", "")]
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
        [CustomComboInfo("意气冲天 添加到 满月 连击", "当剑气达到或低于50时，可将意气冲天加入到月光和满月连击中。\\n将在义气冲天还剩10秒时消耗剑气，防止剑气溢出。", SAM.JobID, 0, "", "")]
        SAM_AOE_GekkoCombo_CDs_Ikishoten = 15040,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("叶隐 添加到 满月 连击", "当有三层剑压时，将 叶隐 添加到 满月 连击。", SAM.JobID, 0, "", "")]
        SAM_AoE_MangetsuCombo_Hagakure = 15041,

        [ReplaceSkill(SAM.Oka)]
        [CustomComboInfo("樱花连", "替换樱花为相应连击。", SAM.JobID, 0, "", "")]
        SAM_AoE_OkaCombo = 15026,

        [ParentCombo(SAM_AoE_OkaCombo)]
        [ConflictingCombos(SAM_AoE_MangetsuCombo_Oka)]
        [CustomComboInfo("樱花双目标功能", "Adds the Yukikaze combo, Mangetsu combo, Senei, Shinten, and Shoha to Oka combo.\\nUsed for two targets only and when Lv86 and above.", SAM.JobID, 0, "", "")]
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
        [CustomComboInfo("意气冲天 奥义斩浪 连击特性", "Replace Ikishoten with Ogi Namikiri and then Kaeshi Namikiri when available.\\nIf you have full Meditation stacks, Ikishoten becomes Shoha while you have Ogi Namikiri ready.", SAM.JobID, 0, "", "")]
        SAM_Ikishoten_OgiNamikiri = 15037,

        [ReplaceSkill(SAM.Gekko, SAM.Yukikaze, SAM.Kasha)]
        [CustomComboInfo("真北 开关", "处于明镜止水状态时，将真北插入所有的单体连击", SAM.JobID, 0, "", "")]
        SAM_TrueNorth = 15038,

        [ParentCombo(SAM_ST_GekkoCombo)]
        [CustomComboInfo("回复设置", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", SAM.JobID, 0, "", "")]
        SAM_ST_ComboHeals = 15043,

        [ParentCombo(SAM_AoE_MangetsuCombo)]
        [CustomComboInfo("回复设置", "Adds Bloodbath and Second Wind to the combo, using them when below the HP Percentage threshold.", SAM.JobID, 0, "", "")]
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
        [CustomComboInfo("小仙女", "Change all fairy actions into Fairy Summons if you do not have 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 fairy summoned.", SCH.JobID, 11)]
        SCH_FairyReminder = 16500,

        [ReplaceSkill(SCH.DeploymentTactics)]
        [CustomComboInfo("展开战术", "Changes Deployment Tactics to Adloquium until 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 party member has the Galvanize buff.", SCH.JobID, 12)]
        SCH_DeploymentTactics = 16600,

        [ParentCombo(SCH_DeploymentTactics)]
        [CustomComboInfo("秘策", "Adds Recitation when off cooldown to force 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 critical Galvanize buff on 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 party member.", SCH.JobID)]
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

        [ReplaceSkill(SMN.Ruin, SMN.Ruin2, SMN.Outburst, SMN.Tridisaster)]
        [ConflictingCombos(SMN_Simple_Combo)]
        [CustomComboInfo("高级自定义循环开关", "Advanced combo features for 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 greater degree of customisation.\\nAccommodates SpS builds.\\nRuin III is left unchanged for mobility purposes.", SMN.JobID, 0, "", "")]
        SMN_Advanced_Combo = 17000,


        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("亚灵神替换", "Adds Deathflare, Ahk Morn and Revelation to the single target and AoE combos.", SMN.JobID, 11, "", "")]
        SMN_Advanced_Combo_DemiSummons_Attacks = 17002,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("宝石耀&宝石辉替换", "Adds Gemshine and Precious Brilliance to the single target and AoE combos, respectively.", SMN.JobID, 4, "", "")]
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

        [ReplaceSkill(SMN.Fester)]
        [CustomComboInfo("能量吸收替换", "当自身没有以太超流时 用能量吸收替换溃烂爆发。", SMN.JobID, 6, "", "")]
        SMN_EDFester = 17008,

        [ReplaceSkill(SMN.Painflare)]
        [CustomComboInfo("能量抽取替换", "当自身没有以太超流时 用能量抽取替换痛苦核爆", SMN.JobID, 7, "", "")]
        SMN_ESPainflare = 17009,

        // BONUS TWEAKS
        [CustomComboInfo("宝石兽自动召唤", "当没有宝石兽跟随时，大部分技能变为召唤宝石兽", SMN.JobID, 8, "", "")]
        SMN_CarbuncleReminder = 17010,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入毁绝", "Adds Ruin IV to the single target and AoE combos.\\nUses when moving during Garuda Phase and you have no attunement, when moving during Ifrit phase, or when you have no active Egi or Demi summon.", SMN.JobID, 0, "", "")]
        SMN_Advanced_Combo_Ruin4 = 17011,

        [ParentCombo(SMN_EDFester)]
        [CustomComboInfo("毁绝替换", "Changes Fester to Ruin IV when out of Aetherflow stacks, Energy Drain is on cooldown, and Ruin IV is available.", SMN.JobID, 0, "", "")]
        SMN_EDFester_Ruin4 = 17013,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入能量吸收和能量抽取", "将能量系技能在冷却完毕后加入到循环和AOE循环中", SMN.JobID, 1, "", "")]
        SMN_Advanced_Combo_EDFester = 17014,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入三神召唤", "Adds Egi summons to the single target and AoE combos.\\nWill prioritise the Egi selected below.\\nIf no option is selected, the feature will default to summoning Titan first.", SMN.JobID, 3, "", "")]
        SMN_DemiEgiMenu_EgiOrder = 17016,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入灼热之光", "在循环和AOE循环中加入灼热之光\\n冷却完毕立刻使用", SMN.JobID, 9, "", "")]
        SMN_SearingLight = 17018,

        [ParentCombo(SMN_SearingLight)]
        [CustomComboInfo("灼热之光爆发设置", "只在亚神召唤中使用灼热之光\\n进阶选项在 高级能力技选项下\\n不适用咏速套装", SMN.JobID, 0, "")]
        SMN_SearingLight_Burst = 170181, // Genesis, why must you be like this -K

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环中加入亚神召唤", "循环和AOE循环中加入 龙神召唤 不死鸟召唤", SMN.JobID, 10, "", "")]
        SMN_Advanced_Combo_DemiSummons = 17020,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("即刻咏唱三神技能选项", "使用三神技能时自动使用即刻", SMN.JobID, 8, "", "")]
        SMN_DemiEgiMenu_SwiftcastEgi = 17023,

        [CustomComboInfo("亚神技能替换亚神召唤", "Adds Enkindle Bahamut, Enkindle Phoenix and Astral Flow to their relevant summons.", SMN.JobID, 11, "", "")]
        SMN_DemiAbilities = 17024,

        [ParentCombo(SMN_Advanced_Combo_EDFester)]
        [CustomComboInfo("高级能力技选项", "只在选定条件下以及灼热之光buff中使用 能力技", SMN.JobID, 1, "", "")]
        SMN_DemiEgiMenu_oGCDPooling = 17025,

        [ConflictingCombos(ALL_Caster_Raise)]
        [CustomComboInfo("替代性的复活功能", "当即刻冷却完毕 替换 复生", SMN.JobID, 8, "", "")]
        SMN_Raise = 17027,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入苏生之炎", "循环 AOE循环中 加入苏生之炎", SMN.JobID, 13, "", "")]
        SMN_Advanced_Combo_DemiSummons_Rekindle = 17028,

        [ReplaceSkill(SMN.Ruin4)]
        [CustomComboInfo("毁荡走位设置", "当你没有毁绝预备buff时，用毁荡替换毁绝。", SMN.JobID, 9, "", "")]
        SMN_RuinMobility = 17030,

        [ParentCombo(SMN_Advanced_Combo)]
        [CustomComboInfo("循环加入醒梦", "当MP低于设定值时，将醒梦加入到循环中", SMN.JobID, 2, "", "")]
        SMN_Lucid = 17031,

        [CustomComboInfo("三神星极超流技替换", "Adds Egi Abilities (Astral Flow) to Egi summons when ready.\\nEgi abilities will appear on their respective Egi summon ability, as well as Titan.", SMN.JobID, 12, "", "")]
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
        [CustomComboInfo("毁绝替换痛苦核爆", "Changes Painflare to Ruin IV when out of Aetherflow stacks, Energy Siphon is on cooldown, and Ruin IV is up.", SMN.JobID, 0, "", "")]
        SMN_ESPainflare_Ruin4 = 17039,


        [ParentCombo(SMN_Ifrit_Cyclone)]
        [CustomComboInfo("深红旋风进阶", "Only uses Crimson Cyclone if you are not moving, or have no remaining Ifrit Attunement charges.", SMN.JobID, 0, "", "")]
        SMN_Ifrit_Cyclone_Option = 17040,

        [ConflictingCombos(SMN_Advanced_Combo)]
        [ReplaceSkill(SMN.Ruin, SMN.Ruin2, SMN.Outburst, SMN.Tridisaster)]
        [CustomComboInfo("简易召唤", "General purpose one-button combo.\\nBursts on Bahamut phase.\\nSummons Titan, Garuda, then Ifrit.\\nSwiftcasts on Slipstream unless drifted.", SMN.JobID, -1, "", "")]
        SMN_Simple_Combo = 17041,

        [ParentCombo(SMN_DemiEgiMenu_oGCDPooling)]
        [CustomComboInfo("爆发延迟开关", "自定义爆发延迟设置\\n适用于咏速套装", SMN.JobID, 2, "", "")]
        SMN_Advanced_Burst_Delay_Option = 17043,


        [ParentCombo(SMN_DemiEgiMenu_oGCDPooling)]
        [CustomComboInfo("灼热之光防顶", "检查任何 灼热之光 状态，而不仅仅是你自己的。\\n如果有多个召唤师并担心你的灼热之光被覆盖，请使用此选项。", SMN.JobID, 1, "", "")]
        SMN_Advanced_Burst_Any_Option = 17044,

        [Variant]
        [VariantParent(SMN_Simple_Combo, SMN_Advanced_Combo)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", SMN.JobID)]
        SMN_Variant_Rampart = 17045,

        [Variant]
        [VariantParent(SMN_Raise)]
        [CustomComboInfo("复活 选项", "当你有即刻BUFF时，替换即刻为成多变复活", SMN.JobID)]
        SMN_Variant_Raise = 17046,

        [Variant]
        [VariantParent(SMN_Simple_Combo, SMN_Advanced_Combo)]
        [CustomComboInfo("治疗 选项", "在下水道使用治疗当HP低于某个值", SMN.JobID)]
        SMN_Variant_Cure = 17047,

        // Last value = 17047 (170181)

        #endregion

        #region WARRIOR

        [ReplaceSkill(WAR.StormsPath)]
        [CustomComboInfo("暴风斩连击", "All in one main combo feature, adds Storm's Eye/Path.\\nIf all sub-options and Fell Cleave/Decimate Options are toggled, will turn into 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 full one button rotation (Advanced Warrior).", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath = 18000,

        [ReplaceSkill(WAR.StormsEye)]
        [CustomComboInfo("暴风碎连击", "将暴风碎替换为暴风碎连击", WAR.JobID, 0, "", "")]
        War_ST_StormsEye = 18001,

        [ReplaceSkill(WAR.Overpower)]
        [CustomComboInfo("超压斧连击", "将超压斧替换为AOE连击", WAR.JobID, 0, "", "")]
        WAR_AoE_Overpower = 18002,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("兽魂监控", "Replace Single target or AoE combo with gauge spender if you are about to overcap and are before 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 step of 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 combo that would generate Beast gauge.", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_OvercapProtection = 18003,

        [ReplaceSkill(WAR.NascentFlash)]
        [CustomComboInfo("原初的勇猛", "在同步到76级以下时，将原初的勇猛替换为原初的直觉.", WAR.JobID, 0, "", "")]
        WAR_NascentFlash = 18005,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("动乱", "在有暴风碎BUFF时，将动乱加入到暴风斩连击", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_Upheaval = 18007,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("蛮荒崩裂", "在蛮荒崩裂预备状态下，将裂石飞环/地毁人亡替换为蛮荒崩裂(也加入AOE连击)", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_PrimalRend = 18008,

        [ParentCombo(WAR_AoE_Overpower)]
        [CustomComboInfo("群山隆起", "在有暴风碎BUFF时，将群山隆起加入到AOE连击", WAR.JobID, 0, "", "")]
        WAR_AoE_Overpower_Orogeny = 18009,

        [ParentCombo(WAR_ST_StormsPath)]
        [CustomComboInfo("裂石飞环/地毁人亡", "当兽魂高于50及以上时，将 裂石飞环 加入主连击，地毁人亡 加入AoE连击。\\n战嚎时将使用 狂魂/混沌旋风，原初的解放时将使用 裂石飞环/钢铁旋风。\\n当 原初的解放 小于30秒时开始留资源。", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_Spender = 18011,

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
        [CustomComboInfo("飞斧", "Replace 暴风斩连击 with Tomahawk when you are out of range.", WAR.JobID, 0, "", "")]
        WAR_ST_StormsPath_RangedUptime = 18016,

        [ReplaceSkill(WAR.FellCleave, WAR.Decimate)]
        [CustomComboInfo("战壕与裂石飞环/地毁人亡整合", "小于等于设置距离时，将裂石飞环/地毁人亡整合到战壕", WAR.JobID, 0, "", "")]
        WAR_InfuriateFellCleave = 18018,

        [ReplaceSkill(WAR.InnerRelease)]
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
        [CustomComboInfo("猛攻", "在 战场风暴 状态下以及在目标环（1m）内并且不移动时使用 猛攻。 \\n将使用上面滑块中选择的层数。", WAR.JobID, 0, "", "")]
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

        // Last value = 18027

        #endregion

        #region WHITE MAGE

        #region Single Target DPS Feature

        [ReplaceSkill(WHM.Stone1, WHM.Stone2, WHM.Stone3, WHM.Stone4, WHM.Glare1, WHM.Glare3)]
        [CustomComboInfo("整合单体输出技能", "咏唱闪耀/飞石后插入能力技.", WHM.JobID, 10, "", "")]
        WHM_ST_MainCombo = 19099,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("开场技能整合至闪灼 设置", "将所有的能力技延迟到第三发闪灼释放之后，仅当闪灼可用时生效" +
        "\\nOnly works with Glare III.", WHM.JobID, 11, "", "")]
        WHM_ST_MainCombo_NoSwiftOpener = 19023,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("保持dot不断 设置", "Adds Aero/Dia to the single target combo if the debuff is not present on current target, or is about to expire.", WHM.JobID, 12, "", "")]
        WHM_ST_MainCombo_DoT = 19013,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("法令 设置", "将法令插入单目标输出循环中.", WHM.JobID, 13, "", "")]
        WHM_ST_MainCombo_Assize = 19009,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("苦难之心 设置", "当苦难之心可用时将其插入单目标输出循环中.", WHM.JobID, 14, "", "")]
        WHM_ST_MainCombo_Misery_oGCD = 19017,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("百合保护", "当有三档治疗百合时在单目标输出循环中插入苦难之心.", WHM.JobID, 15, "", "")]
        WHM_ST_MainCombo_LilyOvercap = 19016,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("神速咏唱 设置", "将神速咏唱插入单目标输出循环中.", WHM.JobID, 16, "", "")]
        WHM_ST_MainCombo_PresenceOfMind = 19008,

        [ParentCombo(WHM_ST_MainCombo)]
        [CustomComboInfo("循环加入醒梦", "当MP低于设定值时将醒梦插入单目标输出循环中.", WHM.JobID, 17, "", "")]
        WHM_ST_MainCombo_Lucid = 19006,

        #endregion

        #region AoE DPS Feature

        [ReplaceSkill(WHM.Holy, WHM.Holy3)]
        [CustomComboInfo("AoE DPS连击", "整合AoE技能到神圣/豪圣.", WHM.JobID, 20, "", "")]
        WHM_AoE_DPS = 19190,

        [ParentCombo(WHM_AoE_DPS)]
        [CustomComboInfo("法令 设置", "将法令整合AoE循环中.", WHM.JobID, 21, "", "")]
        WHM_AoE_DPS_Assize = 19192,

        [ParentCombo(WHM_AoE_DPS)]
        [CustomComboInfo("苦难之心 设置", "当苦难之心可用时将其插入AoE循环中.", WHM.JobID, 22, "", "")]
        WHM_AoE_DPS_Misery = 19194,

        [ParentCombo(WHM_AoE_DPS)]
        [CustomComboInfo("百合保护", "当有三档治疗百合时在AoE循环中插入苦难之心.", WHM.JobID, 23, "", "")]
        WHM_AoE_DPS_LilyOvercap = 19193,

        [ParentCombo(WHM_AoE_DPS)]
        [CustomComboInfo("神速咏唱 设置", "在移动时或不损失gcd的情况下将 神速咏唱 插入AoE输出循环中。", WHM.JobID, 24, "", "")]
        WHM_AoE_DPS_PresenceOfMind = 19195,

        [ParentCombo(WHM_AoE_DPS)]
        [CustomComboInfo("循环加入醒梦", "当MP低于设定值，在移动中或不损失gcd的情况下将 醒梦 插入AoE输出循环中。", WHM.JobID, 25, "", "")]
        WHM_AoE_DPS_Lucid = 19191,

        #endregion

        [ReplaceSkill(WHM.AfflatusSolace)]
        [CustomComboInfo("安慰之心与苦难之心整合 设置", "当苦难之心可用时，替换安慰之心为苦难之心.", WHM.JobID, 30, "", "")]
        WHM_SolaceMisery = 19000,

        [ReplaceSkill(WHM.AfflatusRapture)]
        [CustomComboInfo("狂喜之心与苦难之心整合 设置", "当苦难之心可用时，替换狂喜之心为苦难之心.", WHM.JobID, 40, "", "")]
        WHM_RaptureMisery = 19001,

        #region Afflatus Feature

        [ReplaceSkill(WHM.Cure2)]
        [CustomComboInfo("治疗百合状态", "当有治疗百合时将 救疗 替换为 安慰之心。" +
        "\\nChanges Medica into Afflatus Rapture when Lilies are up.", WHM.JobID, 50, "", "")]
        WHM_Afflatus = 19003,

        [ParentCombo(WHM_Afflatus)]
        [ReplaceSkill(WHM.Cure2)]
        [CustomComboInfo("治疗时使用苦难之心 设置", "当苦难之心可用时，替换救疗为苦难之心.", WHM.JobID, 51, "", "")]
        WHM_Cure2_Misery = 19012,

        [ParentCombo(WHM_Afflatus)]
        [CustomComboInfo("康复 选项", "当你的目标有一个可清除的debuff时则使用 康复。", WHM.JobID)]
        WHM_Cure2_Esuna = 19027,

        #region oGCD Heals/Shields Option

        [ParentCombo(WHM_Afflatus)]
        [CustomComboInfo("瞬发治疗/护盾", "整合治疗和减伤能力技至救疗", WHM.JobID, 52, "", "")]
        WHM_Afflatus_oGCDHeals = 19018,

        [ParentCombo(WHM_Afflatus_oGCDHeals)]
        [CustomComboInfo("插入神名 设置.", "等待校对 满足 HP 条件时在能力技窗口重点显示神名", WHM.JobID, 53, "", "")]
        WHM_Afflatus_oGCDHeals_TetraWeave = 19019,

        [ParentCombo(WHM_Afflatus_oGCDHeals)]
        [CustomComboInfo("优先使用神名 设置", "等待校对 满足 HP 条件时优先显示神名", WHM.JobID, 54, "", "")]
        WHM_Afflatus_oGCDHeals_Tetra = 19020,

        [ParentCombo(WHM_Afflatus_oGCDHeals)]
        [CustomComboInfo("插入神祝祷 设置", "仅在目标身上没有防护罩效果时在治疗魔法GCD窗口中插入神祝祷.", WHM.JobID, 55, "", "")]
        WHM_Afflatus_oGCDHeals_BenisonWeave = 19021,

        [ParentCombo(WHM_Afflatus_oGCDHeals)]
        [CustomComboInfo("优先使用神祝祷 设置", "当目标身上没有防护罩效果时替换救疗为神祝祷.", WHM.JobID, 56, "", "")]
        WHM_Afflatus_oGCDHeals_Benison = 19022,

        [ParentCombo(WHM_Afflatus_oGCDHeals)]
        [CustomComboInfo("神名与神祝祷排序使用 设置", "Displays oGCD Heals/Shields over Afflatus" +
        "\\n(only applies to GCD options for Tetragrammaton and Divine Benison).", WHM.JobID, 57, "", "")]
        WHM_Afflatus_oGCDHeals_Prio = 19024,

        #endregion

        #endregion

        #region Medica Feature

        [ReplaceSkill(WHM.Medica2)]
        [CustomComboInfo("Medica II Feature", "Replaces Medica II with Medica whenever you are under Medica II's regen effect or below Lv.50.", WHM.JobID, 60, "", "")]
        WHM_Medica = 19007,

        [ParentCombo(WHM_Medica)]
        [CustomComboInfo("Afflatus Rapture Option", "Adds Afflatus Rapture when available.", WHM.JobID, 61, "", "")]
        WHM_Medica_Rapture = 19011,

        [ParentCombo(WHM_Medica)]
        [CustomComboInfo("苦难之心 设置", "Adds Afflatus Misery when available.", WHM.JobID, 62, "", "")]
        WHM_Medica_Misery = 19010,

        [ParentCombo(WHM_Medica)]
        [CustomComboInfo("Thin Air Option", "Adds Thin Air when available.", WHM.JobID, 63, "", "")]
        WHM_Medica_ThinAir = 19200,

        #endregion

        [ReplaceSkill(WHM.Cure2)]
        [CustomComboInfo("救疗同步 设置", "当等级同步至30级以下时替换救疗为治疗.", WHM.JobID, 70, "", "")]
        WHM_CureSync = 19002,

        [ReplaceSkill(All.Swiftcast)]
        [ConflictingCombos(ALL_Healer_Raise)]
        [CustomComboInfo("替代性的复活功能", "整合即刻咏唱至复活.", WHM.JobID, 80, "", "")]
        WHM_Raise = 19004,

        [ReplaceSkill(WHM.Raise)]
        [CustomComboInfo("无中生有-复活特性", "在即刻复活前插入无中生有.", WHM.JobID, 90, "", "")]
        WHM_ThinAirRaise = 19014,

        [Variant]
        [VariantParent(WHM_ST_MainCombo_DoT, WHM_AoE_DPS)]
        [CustomComboInfo("精神镖选项", "当没有dot或dot剩余时间少于3s时，使用多变精神镖", WHM.JobID)]
        WHM_DPS_Variant_SpiritDart = 19025,

        [Variant]
        [VariantParent(WHM_ST_MainCombo, WHM_AoE_DPS)]
        [CustomComboInfo("铁壁 选项", "冷却结束时使用多变铁壁", WHM.JobID)]
        WHM_DPS_Variant_Rampart = 19026,

        // Last value = 19026

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
        [SecretCustomCombo]
        [CustomComboInfo("Emergency Heals Feature", "Uses Recuperate when your HP is under the set threshold and you have sufficient MP.", ADV.JobID, 1)]
        PvP_EmergencyHeals = 1100000,

        [SecretCustomCombo]
        [CustomComboInfo("Emergency Guard Feature", "Uses Guard when your HP is under the set threshold.", ADV.JobID, 2)]
        PvP_EmergencyGuard = 1100010,

        [SecretCustomCombo]
        [CustomComboInfo("Quick Purify Feature", "受到选定的debuff时使用净化", ADV.JobID, 4)]
        PvP_QuickPurify = 1100020,

        [SecretCustomCombo]
        [CustomComboInfo("Prevent Mash Cancelling Feature", "Stops you cancelling your guard if you're pressing buttons quickly.", ADV.JobID, 3)]
        PvP_MashCancel = 1100030,

        // Last value = 1100030
        // Extra 0 on the end keeps things working the way they should be. Nothing to see here.

        #endregion

        #region ASTROLOGIAN
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "Turns Fall Malefic into an all-in-one damage button.", AST.JobID)]
        ASTPvP_Burst = 111000,

        [ParentCombo(ASTPvP_Burst)]
        [CustomComboInfo("Double Cast Option", "Adds Double Cast to Burst Mode.", AST.JobID)]
        ASTPvP_DoubleCast = 111001,

        [ParentCombo(ASTPvP_Burst)]
        [CustomComboInfo("Card Option", "Adds Drawing and Playing Cards to Burst Mode.", AST.JobID)]
        ASTPvP_Card = 111002,

        [SecretCustomCombo]
        [CustomComboInfo("Double Cast Heal Feature", "Adds Double Cast to Aspected Benefic.", AST.JobID)]
        ASTPvP_Heal = 111003,

        // Last value = 111003
        #endregion

        #region BLACK MAGE
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "把火炎和冰结改为伤害combo按键", BLM.JobID)]
        BLMPvP_BurstMode = 112000,

        [ParentCombo(BLMPvP_BurstMode)]
        [SecretCustomCombo]
        [CustomComboInfo("Night Wing Option", "Adds Night Wing to Burst Mode.", BLM.JobID)]
        BLMPvP_BurstMode_NightWing = 112001,

        [ParentCombo(BLMPvP_BurstMode)]
        [SecretCustomCombo]
        [CustomComboInfo("Aetherial Manipulation Option", "爆发CD没好的时候，使用以太步拉近距离", BLM.JobID)]
        BLMPvP_BurstMode_AetherialManip = 112002,

        // Last value = 112002

        #endregion

        #region BARD
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "把强劲射击改为伤害combo按键", BRDPvP.JobID)]
        BRDPvP_BurstMode = 113000,

        // Last value = 113000

        #endregion

        #region DANCER
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "把喷泉连改成伤害combo按键", DNC.JobID)]
        DNCPvP_BurstMode = 114000,

        [SecretCustomCombo]
        [ParentCombo(DNCPvP_BurstMode)]
        [CustomComboInfo("刃舞选项", "Adds Honing Dance to the main combo when in melee range (respects global offset).\\nThis option prevents early use of Honing Ovation!\\nKeep Honing Dance bound to another key if you want to end early.", DNC.JobID)]
        DNCPvP_BurstMode_HoningDance = 114001,

        [SecretCustomCombo]
        [ParentCombo(DNCPvP_BurstMode)]
        [CustomComboInfo("治疗之华尔兹选项", "Adds Curing Waltz to the combo when available, and your HP is at or below the set percentage.", DNC.JobID)]
        DNCPvP_BurstMode_CuringWaltz = 114002,

        // Last value = 114002

        #endregion

        #region DARK KNIGHT
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "Turns 噬魂斩连击 into an all-in-one damage button.", DRK.JobID)]
        DRKPvP_Burst = 115000,

        [SecretCustomCombo]
        [ParentCombo(DRKPvP_Burst)]
        [CustomComboInfo("Plunge Option", "Adds Plunge to Burst Mode.", DRK.JobID)]
        DRKPvP_Plunge = 115001,

        [SecretCustomCombo]
        [ParentCombo(DRKPvP_Plunge)]
        [CustomComboInfo("近战时使用跳斩 设置", "Uses Plunge whilst in melee range, and not just as 当 深谋远虑之策 冷却完毕时，将 生命活性法 替换为 深谋远虑之策。 gap-closer.", DRK.JobID)]
        DRKPvP_PlungeMelee = 115002,

        // Last value = 115002

        #endregion

        #region DRAGOON
        [SecretCustomCombo]
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

        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "Turns Solid Barrel Combo into an all-in-one damage button.", GNB.JobID)]
        GNBPvP_Burst = 117000,

        [ParentCombo(GNBPvP_Burst)]
        [CustomComboInfo("倍攻", "Adds Double Down to 爆发模式 while under the No Mercy buff.", GNB.JobID)]
        GNBPvP_DoubleDown = 117001,

        [SecretCustomCombo]
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
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "把蓄力冲击改为伤害combo按键", MCHPvP.JobID)]
        MCHPvP_BurstMode = 118000,

        [SecretCustomCombo]
        [ParentCombo(MCHPvP_BurstMode)]
        [CustomComboInfo("Alternate Drill Option", "Saves Drill for use after Wildfire.", MCHPvP.JobID)]
        MCHPvP_BurstMode_AltDrill = 118001,

        [SecretCustomCombo]
        [ParentCombo(MCHPvP_BurstMode)]
        [CustomComboInfo("Alternate Analysis Option", "Uses Analysis with Air Anchor instead of Chain Saw.", MCHPvP.JobID)]
        MCHPvP_BurstMode_AltAnalysis = 118002,

        // Last value = 118002

        #endregion

        #region MONK
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "Turns Phantom Rush Combo into an all-in-one damage button.", MNK.JobID)]
        MNKPvP_Burst = 119000,

        [ParentCombo(MNKPvP_Burst)]
        [SecretCustomCombo]
        [CustomComboInfo("Thunderclap Option", "Adds Thunderclap to 爆发模式 when not buffed with Wind Resonance.", MNK.JobID)]
        MNKPvP_Burst_Thunderclap = 119001,

        [ParentCombo(MNKPvP_Burst)]
        [SecretCustomCombo]
        [CustomComboInfo("Riddle of Earth Option", "Adds Riddle of Earth and Earth's Reply to 爆发模式 when in combat.", MNK.JobID)]
        MNKPvP_Burst_RiddleOfEarth = 119002,

        // Last value = 119002

        #endregion

        #region NINJA
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "把旋风刃连击改为伤害combo按键", NINPvP.JobID)]
        NINPvP_ST_BurstMode = 120000,

        [SecretCustomCombo]
        [CustomComboInfo("AOE爆发模式", "把风魔手里剑改为AOE伤害combo按键", NINPvP.JobID)]
        NINPvP_AoE_BurstMode = 120001,

        [ParentCombo(NINPvP_ST_BurstMode)]
        [SecretCustomCombo]
        [CustomComboInfo("命水 选项", "Uses Three Mudra on Meisui when HP is under the set threshold.", NINPvP.JobID)]
        NINPvP_ST_Meisui = 120002,

        [ParentCombo(NINPvP_AoE_BurstMode)]
        [SecretCustomCombo]
        [CustomComboInfo("命水 选项", "Uses Three Mudra on Meisui when HP is under the set threshold.", NINPvP.JobID)]
        NINPvP_AoE_Meisui = 120003,

        // Last value = 120003

        #endregion

        #region PALADIN
        [SecretCustomCombo]
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
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "把切割改为伤害combo按键 \\n添加灵魂切割到主连击", RPR.JobID)]
        RPRPvP_Burst = 122000,

        [SecretCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo("斩首令选项", "Adds Death Warrant onto the main combo when Plentiful Harvest is ready to use, or when Plentiful Harvest's cooldown is longer than Death Warrant's.\\nRespects Immortal Sacrifice Pooling Option.", RPR.JobID)]
        RPRPvP_Burst_DeathWarrant = 122001,

        [SecretCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo("大丰收开启选项", "大丰收起手以便立即获取lb槽", RPR.JobID)]
        RPRPvP_Burst_PlentifulOpener = 122002,

        [SecretCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo("Plentiful Harvest + Immortal Sacrifice Pooling Option", "Pools stacks of Immortal Sacrifice before using Plentiful Harvest.\\nAlso holds Plentiful Harvest if Death Warrant is on cooldown.\\nSet the value to 3 or below to use Plentiful Harvest as soon as it's available.", RPR.JobID)]
        RPRPvP_Burst_ImmortalPooling = 122003,

        [SecretCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo("夜游魂衣爆发选项", "Adds Lemure's Slice to the main combo during the Enshroud burst phase.\\nContains burst options.", RPR.JobID)]
        RPRPvP_Burst_Enshrouded = 122004,

        #region RPR Enshrouded Option
        [SecretCustomCombo]
        [ParentCombo(RPRPvP_Burst_Enshrouded)]
        [CustomComboInfo("夜游魂斩首令选项", "在夜游魂衣爆发期间，如果有机会，将死亡授权书添加到主要组合中。", RPR.JobID)]
        RPRPvP_Burst_Enshrouded_DeathWarrant = 122005,

        [SecretCustomCombo]
        [ParentCombo(RPRPvP_Burst_Enshrouded)]
        [CustomComboInfo("团契设置", "当你有一个夜游魂衣剩余时，将团契加入主组合。", RPR.JobID)]
        RPRPvP_Burst_Enshrouded_Communio = 122006,
        #endregion

        [SecretCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo("远程收获月选项", "Adds Harvest Moon onto the main combo when you're out of melee range, the GCD is not rolling and it's available for use.", RPR.JobID)]
        RPRPvP_Burst_RangedHarvest = 122007,

        [SecretCustomCombo]
        [ParentCombo(RPRPvP_Burst)]
        [CustomComboInfo("神秘环设置", "Adds Arcane Circle to the main combo when under the set HP perecentage.", RPR.JobID)]
        RPRPvP_Burst_ArcaneCircle = 122008,

        // Last value = 122008

        #endregion

        #region RED MAGE
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "赤飞石/赤火炎改为伤害combo按键", RDMPvP.JobID)]
        RDMPvP_BurstMode = 123000,


        [SecretCustomCombo]
        [ParentCombo(RDMPvP_BurstMode)]
        [CustomComboInfo("不使用 疲惫 选项", "Prevents Frazzle from being used in Burst Mode.", RDMPvP.JobID)]
        RDMPvP_FrazzleOption = 123001,

        // Last value = 123001

        #endregion

        #region SAGE
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "把注药 III 改成伤害combo按键", SGE.JobID)]
        SGEPvP_BurstMode = 124000,

        [ParentCombo(SGEPvP_BurstMode)]
        [CustomComboInfo("Pneuma Option", "Adds Pneuma to Burst Mode.", SGE.JobID)]
        SGEPvP_BurstMode_Pneuma = 124001,

        // Last value = 124001

        #endregion

        #region SAMURAI

        #region Burst Mode
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "Adds Meikyo Shisui, Midare: Setsugekka, Ogi Namikiri, Kaeshi: Namikiri and Soten to Meikyo Shisui.\\nWill only cast Midare: Setsugekka and Ogi Namikiri when you're not moving.\\nWill not use if target is guarding.", SAM.JobID)]
        SAMPvP_BurstMode = 125000,

        [SecretCustomCombo]
        [ParentCombo(SAMPvP_BurstMode)]
        [CustomComboInfo("Chiten Option", "Adds Chiten to 爆发模式 when in combat and HP is below 95%.", SAM.JobID)]
        SAMPvP_BurstMode_Chiten = 125001,

        [SecretCustomCombo]
        [ParentCombo(SAMPvP_BurstMode)]
        [CustomComboInfo("Mineuchi Option", "Adds Mineuchi to Burst Mode.", SAM.JobID)]
        SAMPvP_BurstMode_Stun = 125002,

        [SecretCustomCombo]
        [ParentCombo(SAMPvP_BurstMode)]
        [CustomComboInfo("Burst Mode on 花车连 Option", "Adds 爆发模式 to 花车连 instead.", SAM.JobID, 1)]
        SAMPvP_BurstMode_MainCombo = 125003,
        #endregion

        #region Kasha Features
        [SecretCustomCombo]
        [CustomComboInfo("Kasha Combo Features", "花车连的功能集合", SAM.JobID)]
        SAMPvP_KashaFeatures = 125004,

        [SecretCustomCombo]
        [ParentCombo(SAMPvP_KashaFeatures)]
        [CustomComboInfo("使用必杀剑·早天拉近距离", "Adds Soten to the 花车连 when out of melee range.", SAM.JobID)]
        SAMPvP_KashaFeatures_GapCloser = 125005,

        [SecretCustomCombo]
        [ParentCombo(SAMPvP_KashaFeatures)]
        [CustomComboInfo("AoE Melee Protection Option", "在超出近战范围时，将AOE连击变成不可用状态", SAM.JobID)]
        SAMPvP_KashaFeatures_AoEMeleeProtection = 125006,
        #endregion

        // Last value = 125006

        #endregion

        #region SCHOLAR
        [SecretCustomCombo]
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
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "将毁灭三号变成一个多合一的伤害按钮。只有在近战范围内才会使用深红旋风。", SMNPvP.JobID)]
        SMNPvP_BurstMode = 127000,

        [SecretCustomCombo]
        [ParentCombo(SMNPvP_BurstMode)]
        [CustomComboInfo("守护之光选项", "Adds Radiant Aegis to 爆发模式 when available, and your HP is at or below the set percentage.", SMNPvP.JobID)]
        SMNPvP_BurstMode_RadiantAegis = 127001,

        // Last value = 127001

        #endregion

        #region WARRIOR
        [SecretCustomCombo]
        [CustomComboInfo("爆发模式", "把重劈改为伤害combo按键", WARPvP.JobID)]
        WARPvP_BurstMode = 128000,

        [SecretCustomCombo]
        [ParentCombo(WARPvP_BurstMode)]
        [CustomComboInfo("血气选项", "Allows use of Bloodwhetting any time, not just between GCDs.", WARPvP.JobID)]
        WARPvP_BurstMode_Bloodwhetting = 128001,

        [SecretCustomCombo]
        [ParentCombo(WARPvP_BurstMode)]
        [CustomComboInfo("献身选项", "Removes Blota from 爆发模式 if Primal Rend has 5 seconds or less on its cooldown.", WARPvP.JobID)]
        WARPvP_BurstMode_Blota = 128002,

        // Last value = 128002

        #endregion

        #region WHITE MAGE
        [SecretCustomCombo]
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

        [SecretCustomCombo]
        [CustomComboInfo("Aquaveil Feature", "Adds Aquaveil to Cure II when available.", WHM.JobID)]
        WHMPvP_Aquaveil = 129004,

        [SecretCustomCombo]
        [CustomComboInfo("Cure III Feature", "Adds Cure III to Cure II when available.", WHM.JobID)]
        WHMPvP_Cure3 = 129005,

        // Last value = 129005

        #endregion

        #endregion
    }
}