using XIVSlothComboPlugin.Attributes;
using XIVSlothComboPlugin.Combos;

namespace XIVSlothComboPlugin
{
    /// <summary>
    /// Combo presets.
    /// </summary>
    public enum CustomComboPreset
    {
        // ====================================================================================
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
        // ====================================================================================
        #region ADV
        #endregion
        /*
        #region GLOBAL FEATURES
        [CustomComboInfo("Global Interrupt Feature", "Replaces Stun (LowBlow) with interrupt (Interject) when the target can be interrupted.", All.JobID)]
        InterruptFeature = 90000,

        [ConflictingCombos(SchRaiseFeature, WHMRaiseFeature, AstrologianAscendFeature, SageEgeiroFeature)]
        [CustomComboInfo("Global 复活 Feature", "Replaces Swiftcast with 复活/Resurrection/Verraise/Ascend/Egeiro when appropriate.", All.JobID)]
        DoMSwiftcastFeature = 90001,

        #endregion
        */
        // ====================================================================================
        #region ASTROLOGIAN

        [CustomComboInfo("用出卡替换抽卡", "当没有抽取到奥秘卡时，自动将出卡替换为抽卡。", AST.JobID, 0, "强欲之壶", "抽取几张奥秘卡或者什么东西。我不知道，反正是你选择成为占星术士的。")]
        AstrologianCardsOnDrawFeaturelikewhat = 1000,

        [CustomComboInfo("用出王冠卡替换小奥秘卡", "当没有抽取到小奥秘卡时，自动将出王冠卡替换为小奥秘卡。", AST.JobID, 0, "赐予王权", "这是为嫡子和皇女准备的，让我们一起高呼“万岁！”")]
        AstrologianCrownPlayFeature = 1001,

        [CustomComboInfo("根据等级自动将复兴替换为吉星", "当等级同步至26级以下时，自动将福星替换为吉星。", AST.JobID, 0, "新生的奇异博士", "在你游历新生宇宙的时候，将“大”奇异博士变成“小”奇异博士。")]
        AstrologianBeneficFeature = 1002,

        [ConflictingCombos(AstrologianAlternateAscendFeature)]
        [CustomComboInfo("占星的即刻-生辰", "自动把即刻咏唱改为生辰", AST.JobID, 0, "亚拉戈复活机3000", "替代你干活，而且更为迅速，不用谢我，你这个小懒蛋~")]
        AstrologianAscendFeature = 1003,

        [ConflictingCombos(AstrologianAscendFeature)]
        [CustomComboInfo("占星的生辰-即刻", "当即刻咏唱可用时，将生辰改为即刻咏唱", AST.JobID, 0, "亚拉戈复活机3000", "相同的核心，不同的外表！")]
        AstrologianAlternateAscendFeature = 1019,

        [ConflictingCombos(AstrologianAlternateDpsFeature, CustomValuesTest)]
        [CustomComboInfo("单体DPS连击-集中于凶星", "在烧灼buff不存在或即将消失时，自动替换凶星为烧灼", AST.JobID, 0, "红占星? 看起来也不难嘛", "优化你的输出手法，logs金粉不再是梦...")]
        AstrologianDpsFeature = 1004,

        [ParentCombo(AstrologianDpsFeature)]
        [CustomComboInfo("醒梦", "当MP值低于设定值时，在DPS连击中自动增添醒梦。", AST.JobID, 0, "醒梦", "我听说你曾经可以控制你的梦境...然后我就被闹钟吵醒了")]
        AstrologianLucidFeature = 1008,

        [CustomComboInfo("星力", "当星力可用时，在DPS连击中自动添加星力", AST.JobID, 0, "天体之力", "有流星雨？别闹了，贤者的浮游炮它不帅吗？")]
        AstrologianAstrodyneFeature = 1009,

        [CustomComboInfo("阳星相位", "当你处于阳星相位hot时，用阳星来替换阳星相位", AST.JobID, 0, "阳星相位", "阳星？阳性？养星？")]
        AstrologianHeliosFeature = 1010,

        [CustomComboInfo("自动抽奥秘卡", "将自动抽奥秘卡添加进入DPS连击中", AST.JobID, 0, "濑户海马", "你刚刚触发了我的陷阱卡！")]
        AstrologianAutoDrawFeature = 1011,

        [CustomComboInfo("自动抽小奥秘卡", "将自动抽小奥秘卡添加进入DPS连击中 ", AST.JobID, 0, "濑户海马2, 闪电风暴", "这是个陷阱！")]
        AstrologianAutoCrownDrawFeature = 1012,

        [CustomComboInfo("AoE DPS连击", "将自动抽卡和星力添加进入重力AOE连击中", AST.JobID, 0, "强力之卡", "天呐！满屏的AOE!")]
        AstrologianDpsAoEFeature = 1013,

        [CustomComboInfo("王冠之领主", "自动将王冠之领主添加进入DPS/AOE连击中", AST.JobID, 0, "无脑的王冠之领主", "你看起来像是来自于怪物史莱克 - E -")]
        AstrologianLazyLordFeature = 1014,

        [CustomComboInfo("用星力替换出卡", "当你拥有三颗星星时自动替换出卡为星力", AST.JobID, 0, "用星力替换出卡", "你被强化了！快送！")]
        AstrologianAstrodyneOnPlayFeature = 1015,

        [ConflictingCombos(AstrologianDpsFeature, CustomValuesTest)]
        [CustomComboInfo("单体DPS连击-集中于烧灼", "在烧灼buff不存在或即将消失时，自动替换凶星为烧灼", AST.JobID, 0, "增强", "淦！我们现在真的成你的打工仔了")]
        AstrologianAlternateDpsFeature = 1016,

        [ConflictingCombos(AstrologianDpsFeature, AstrologianAlternateDpsFeature, DisableCombustOnDpsFeature)]
        [CustomComboInfo("单体DPS的自定义值测试", "和单体DPS连击-集中于凶星相同，但是允许你自动与目标的最大HP值，当前HP百分比，当前HP值。仅供测试！", AST.JobID, 0, "红占星？但你的志向更为远大", "和另一个一样，只是多了数值设定")]
        CustomValuesTest = 1017,

        [ParentCombo(AstrologianDpsFeature)]
        [ConflictingCombos(AstrologianAlternateDpsFeature)]
        [CustomComboInfo("将DOT移除DPS连击", "将DOT移除DPS连击，其他的DPS不受影响 ", AST.JobID, 0, "减少输出", "好家伙！那个人开始奶人了！")]
        DisableCombustOnDpsFeature = 1018,

        [CustomComboInfo("光速", "将光速添加入DPS连击", AST.JobID, 0, "动力小子", "冲冲冲！")]
        AstrologianLightSpeedFeature = 1020,

        [ParentCombo(AstrologianHeliosFeature)]
        [CustomComboInfo("天行冲日", "添加天行冲日进入AOE治疗连击", AST.JobID, 0)]
        AstrologianCelestialOppositionFeature = 1021,

        [ParentCombo(AstrologianHeliosFeature)]
        [CustomComboInfo("王冠之贵妇", "当抽取到王冠之贵妇时，自动加入治疗连击", AST.JobID, 0)]
        AstrologianLazyLadyFeature = 1022,

        [CustomComboInfo("简单治疗", "单体治疗连击", AST.JobID, 0)]
        AstrologianSimpleSingleTargetHeal = 1023,

        [ParentCombo(AstrologianSimpleSingleTargetHeal)]
        [CustomComboInfo("先天禀赋", "当目标体力低于你设定的值时，自动添加先天禀赋进入单体治疗连击", AST.JobID, 0)]
        AstroEssentialDignity = 1024,

        [ParentCombo(AstrologianSimpleSingleTargetHeal)]
        [CustomComboInfo("天星交错", "添加天行交错进入单体治疗连击", AST.JobID, 0)]
        CelestialIntersectionFeature = 1025,

        [ParentCombo(AstrologianHeliosFeature)]
        [CustomComboInfo("天宫图", "添加天宫图进入治疗连击", AST.JobID, 0)]
        AstrologianHoroscopeFeature = 1026,
        
        [ParentCombo(AstrologianSimpleSingleTargetHeal)]
        [CustomComboInfo("吉星相位", "在目标身上没有吉星相位或快要结束时，自动更换福星为吉星相位", AST.JobID, 0)]
        AspectedBeneficFeature = 1027,

        [ParentCombo(AstrologianSimpleSingleTargetHeal)]
        [CustomComboInfo("Exaltation Feature", "Adds Exaltation.", AST.JobID, 0)]
        ExaltationFeature = 1028,


        #endregion
        // ====================================================================================
        #region BLACK MAGE

        [ConflictingCombos(BlackSimpleFeature)]
        [CustomComboInfo("天语转换", "基于零极冰或星极火时，自动将崩溃更改为火4或冰4。\n崩溃一键循环。\n如果雷dot被启用，同样会在DOT不存在或即将消失时，自动转换为雷3。\n这需要其他功能勾选启用！！！", BLM.JobID, 0, "嗨魔", "一键！")]
        BlackEnochianFeature = 2000,

        [CustomComboInfo("灵极魂/星灵移位切换", "当灵极魂可用时，自动将星灵移位替换为灵极魂。", BLM.JobID, 0, "这啥啊？", "大约就是干好黑魔自己")]
        BlackManaFeature = 2001,

        [CustomComboInfo("魔纹步", "在黑魔纹激活时，自动将黑魔纹替换为魔纹步。", BLM.JobID, 0, "黑魔坟", "黑魔呢？黑魔纹里躺着呢！")]
        BlackLeyLinesFeature = 2002,

        [ParentCombo(BlackEnochianFeature)]
        [CustomComboInfo("冰1/2/3", "灵极冰状态消失时，自动替换冰1为冰3。 在等级同步时，自动替换冰2为冰1", BLM.JobID, 0, "冷酷boy", "冷静下来，乖宝，现在该睡觉了")]
        BlackBlizzardFeature = 2003,

        [ConflictingCombos(BlackEnochianFeature, BlackSimpleFeature)]
        [CustomComboInfo("崩溃/异言", "当异言可用时自动替换崩溃为异言", BLM.JobID, 0, "Glossy paint", "So shiny, so glossy...")]
        BlackScatheFeature = 2004,

        [ParentCombo(BlackEnochianFeature)]
        [CustomComboInfo("火1/3", "当星极火状态消失或火苗状态消失时，自动替换火1为火3。", BLM.JobID, 0, "炽热boy", "这里要烧起来啦！")]
        BlackFire13Feature = 2005,

        [ParentCombo(BlackEnochianFeature)]
        [CustomComboInfo("雷DOT", "在天语转换中启用自动替换火4/冰4为雷1/3。\n 出现了以下两种情况时，才会激活\n- 你身上的雷云BUFF马上要超时，或\n- 你目标身上的雷属性DEFUFF马上要消失\n假设它不会中断BUFF计时器。\n天语转换必须处于激活状态", BLM.JobID, 0, "满电状态", "雷雨招来！")]
        BlackThunderFeature = 2006,

        [ParentCombo(BlackEnochianFeature)]
        [CustomComboInfo("绝望", "当你MP低于2400时，自动替换火4为绝望。\n天语转换必须被激活。", BLM.JobID, 0, "我蓝呢！？", "一切艺术都是派大星！！！")]
        BlackDespairFeature = 2007,

        [CustomComboInfo("AOE连击", "将AOE连击置于核爆键上（仅供测试！）", BLM.JobID, 0, "惠惠模式", "黑より黑く 闇より暗き漆黑に\n我が深紅の混淆を望みたもう\n覺醒のとき來たれり\n無謬の境界に落ちし理\n無行の歪みとなりて\n現出せよ！\nExplosion！")]
        BlackAoEComboFeature = 2008,

        [ParentCombo(BlackEnochianFeature)]
        [CustomComboInfo("冰结 Paradox Feature", "Adds Paradox onto ice phase combo", BLM.JobID, 0, "The sentence below is false.", "The sentence above is true.")]
        BlackBlizzardParadoxFeature = 2009,

        [ParentCombo(BlackEnochianFeature)]
        [CustomComboInfo("Aspect Swap Feature", "Changes Scathe to Blizzard 3 when at 0 MP in Astral 火炎 or to 火炎 3 when at 10000 MP in Umbral Ice with 3 Umbral Hearts.", BLM.JobID, 0, "", "")]
        BlackAspectSwapFeature = 2010,

        [ParentCombo(BlackThunderFeature)]
        [CustomComboInfo("闪雷 Uptime Feature", "Changes Scathe to 闪雷 1/3 when not detected on target.", BLM.JobID, 0, "Bzzt", "Shocking!")]
        BlackThunderUptimeFeature = 2011,

        [ConflictingCombos(BlackEnochianFeature, BlackScatheFeature)]
        [CustomComboInfo("Simple BLM", "Adds the whole rotation to one button.", BLM.JobID, 0, "", "")]
        BlackSimpleFeature = 2012,

        [ParentCombo(BlackSimpleFeature)]
        [CustomComboInfo("Simple Buffs Feature", "Adds 魔泉, 激情咏唱, 详述 onto the Simple BLM feature.", BLM.JobID, 0, "", "!")]
        BlackSimpleBuffsFeature = 2013,

        [ParentCombo(BlackSimpleFeature)]
        [CustomComboInfo("Simple Buffs - 黑魔纹", "Adds 黑魔纹 onto the Simple BLM feature.", BLM.JobID, 0, "", "!")]
        BlackSimpleBuffsLeylinesFeature = 2014,

        [ParentCombo(BlackSimpleFeature)]
        [CustomComboInfo("Simple 抛竿s Feature", "Adds 三连咏唱/Swiftcast onto the Simple BLM feature.", BLM.JobID, 0, "", "!")]
        BlackSimpleCastsFeature = 2015,

        [ParentCombo(BlackSimpleFeature)]
        [CustomComboInfo("Simple 抛竿s - Pooling", "Keep one triplecast usage for movement in the Simple BLM feature.", BLM.JobID, 0, "", "!")]
        BlackSimpleCastPoolingFeature = 2016,

        [ParentCombo(BlackSimpleFeature)]
        [CustomComboInfo("Simple Pooling", "Keep one xenoglossy usage for movement in the Simple BLM feature.", BLM.JobID, 0, "", "!")]
        BlackSimplePoolingFeature = 2017,

        [ParentCombo(BlackSimpleFeature)]
        [CustomComboInfo("Simple 火炎3 Opener", "Adds the 火炎 III Opener to Simple BLM.", BLM.JobID, 0, "", "")]
        BlackSimpleOpenerFeature = 2018,

        [ParentCombo(BlackSimpleFeature)]
        [CustomComboInfo("Simple 火炎3 Opener - 1 Triplecast", "Modifies the Simple 火炎3 Opener to only use 1 Triplecast.", BLM.JobID, 0, "", "")]
        BlackSimpleAltOpenerFeature = 2019,

        [ParentCombo(BlackAoEComboFeature)]
        [CustomComboInfo("Foul / 魔泉 to Flare Option", "Adds foul when available during Astral 火炎. Weaves manafont after foul for additional Flare", BLM.JobID, 0, "", "")]
        BlackAoEFoulOption = 2020,

        #endregion
        // ====================================================================================
        #region BLUE MAGE

        [CustomComboInfo("Buffed 苦闷之歌", "Turns 苦闷之歌 into Bristle so SoT is buffed. \nSpells Required: 苦闷之歌.", BLU.JobID)]
        BluBuffedSoT = 70000,

        [CustomComboInfo("月之笛 Opener", "Puts the Full 月之笛 Opener on 月之笛 or Whistle. \nSpells Required: Whistle, 哔哩哔哩, 月之笛, 正义飞踢, 渔叉三段, 月下彼岸花, Rose of Destruction, 飞翎雨, 怒发冲冠, 冰雪乱舞, 穿甲散弹, 马特拉魔术, 轰雷, 鬼宿脚.", BLU.JobID)]
        BluOpener = 70001,

        [CustomComboInfo("终极针 Combo", "Turns 终极针 into the buff combo of: Moon Flute, 哔哩哔哩, 口笛, 终极针. Will use any primals off CD before casting 终极针. \nSpells Required: Moon Flute, 哔哩哔哩, 口笛, 终极针", BLU.JobID)]
        BluFinalSting = 70002,

        [ParentCombo(BluFinalSting)]
        [CustomComboInfo("Off CD Primal Additions", "Adds any Primals that are off CD to the 终极针 Combo. \nPrimals Used: Feather Rain, 轰雷, 冰雪乱舞, 正义飞踢, Rose of Destruction. ", BLU.JobID)]
        BluPrimals = 70003,

        [CustomComboInfo("Swiftcasted 天使低语", "Turns 天使低语 to Swiftcast when Swiftcast and 天使低语 are available. \nSpells required: 天使低语 ", BLU.JobID)]
        BluRez = 70004,

        [CustomComboInfo("Ram's Voice into 超振动", "Turns Ultravibration into Ram's Voice if Deep 玄冰 isn't on the target. Will swiftcast Ultravibration if available. \nSpells Required: Ram's Voice, 超振动. ", BLU.JobID)]
        BluUltravibrate = 70005,

        [CustomComboInfo("Tank Debuff Feature", "Puts 捕食, Off-Guard, Lucid Dreaming, and 臭气 into one button when under Tank Mimicry. \nSpells Required: Devour, Off-Guard, 臭气.", BLU.JobID)]
        BluDebuffCombo = 70006,

        [CustomComboInfo("Addle/魔法锤 Debuff Feature", "Turns 魔法锤 into Addle when off CD. \nSpells Required: 魔法锤.", BLU.JobID)]
        BluAddleFeature = 70007,

        [CustomComboInfo("Primal Feature", "Turns 飞翎雨 into any Primals that are off CD. \nSpells Required: 飞翎雨, 轰雷, 斗灵弹, 冰雪乱舞, J Kick. \nWill cause primals to desync from 月之笛 burst phases if used on CD.", BLU.JobID)]
        BluPrimalFeature = 70008,

        [CustomComboInfo("Knight's Tour Feature", "Turns Black Knight's Tour or 白骑士之旅 into its counterpart when the enemy is under the effect of the spell's debuff. \nSpells Required: 白骑士之旅, 黑骑士之旅", BLU.JobID)]
        BluKnightFeature = 70009,

        [CustomComboInfo("Peripheral Synthesis into 芥末爆弹", "Turns Peripheral Synthesis into 芥末爆弹 when target is under the effect of Lightheaded. \nSpells Required: Peripheral Synthesis, 芥末爆弹.", BLU.JobID)]
        BluLightheadedCombo = 70010,


        #endregion
        // ====================================================================================
       #region BARD

        [CustomComboInfo("放浪神的小步舞曲设置", "吟唱放浪神期间，将放浪神的小步舞曲替换为完美音调。", BRD.JobID, 0, "放浪？", "听起来比往脑子里灌水还离谱！")]
        BardWanderersPitchPerfectFeature = 3000,

        [ConflictingCombos(SimpleBardFeature)]
        [CustomComboInfo("强力射击设置", "触发直线射击预备状态时，替换强力射击/爆发射击为直线射击/辉煌箭。", BRD.JobID, 0, "……强力射击会触发直线射击……", "还搁这研究诗人呐？机工它不香吗？用枪突突突不爽吗？")]
        BardStraightShotUpgradeFeature = 3001,

        [ConflictingCombos(SimpleBardFeature)]
        [ParentCombo(BardStraightShotUpgradeFeature)]
        [CustomComboInfo("Dot选项", "开启此选项可适时插入毒/风箭。", BRD.JobID, 0, "怎样保持毒一直不断？", "我只要一直打毒/风箭不就行了？（机智")]
        BardDoTMaintain = 3002,

        [ConflictingCombos(BardIronJawsAlternateFeature)]
        [CustomComboInfo("伶牙俐齿续dot模式A", "当目标身上没有毒/风dot时，替换伶牙俐齿为毒/风箭。\n当还未习得伶牙俐齿时会在毒/风箭之间自动切换。", BRD.JobID, 0, "突然觉得伶牙俐齿其实挺符合诗人设定的。", "怎么说？一直在那叭叭叭唱个不停。")]
        BardIronJawsFeature = 3003,

        [ConflictingCombos(BardIronJawsFeature)]
        [CustomComboInfo("伶牙俐齿续dot模式B", "当目标身上没有毒/风dot时，替换伶牙俐齿为毒/风箭。 \n伶牙俐齿仅会在风/毒dot即将结束时复现。", BRD.JobID, 0, "哎，我进来了我又出去了我又进来了，打我啊，笨蛋！")]
        BardIronJawsAlternateFeature = 3004,

        [ConflictingCombos(SimpleBardFeature)]
        [CustomComboInfo("绝峰箭设置", "当灵魂之声蓄满时，替换爆发射击/连珠箭为绝峰箭，触发爆破箭预备状态时替换为爆破箭。", BRD.JobID, 0, "Robin Hood Feature", "Steal from Lolorito and give to Garlemald, I guess?\nGood on ya.")]
        BardApexFeature = 3005,

        [ConflictingCombos(SimpleBardFeature, BardSimpleOpener)]
        [CustomComboInfo("单目标能力技插入选项", "在三歌循环中根据cd时间替换失血箭为其他能力技。", BRD.JobID, 0, "怎么哪儿哪儿都要插能力技？", "你以为你拿个竖琴就只能射一只箭？")]
        BardoGCDSingleTargetFeature = 3006,

        [ConflictingCombos(BardAoEComboFeature)]
        [CustomComboInfo("AOE能力技插入选项", "在三歌循环中根据cd时间替换死亡箭雨为其他能力技。", BRD.JobID, 0, "", "天上下刀子啦！快跑！")]
        BardoGCDAoEFeature = 3007,

        [ConflictingCombos(BardSimpleAoEFeature)]
        [CustomComboInfo("AOE连击设置", "在影噬箭预备状态下替换连珠箭/百首龙牙箭为影噬箭。", BRD.JobID, 0, "", "C-C-C-Combo!")]
        BardAoEComboFeature = 3008,

        [ConflictingCombos(BardStraightShotUpgradeFeature, BardDoTMaintain, BardApexFeature, BardoGCDSingleTargetFeature)]
        [CustomComboInfo("单体简易一键连击", "一键整合所有单体攻击技能。\n如果目标身上有dots，会同时尝试保持dots不断。", BRD.JobID, 0, "这诗人这么难，我脑子不够用了", "那就别带脑子了，开挂!")]
        SimpleBardFeature = 3009,

        [ParentCombo(SimpleBardFeature)]
        [CustomComboInfo("单体连击Dot设置", "如果目标身上不存在风/毒dot，开启此选项会在连击中加入风/毒箭。", BRD.JobID, 0, "", "Dot是啥？能吃吗？")]
        SimpleDoTOption = 3010,

        [ParentCombo(SimpleBardFeature)]
        [CustomComboInfo("单体连击唱歌设置", "在连击中自动加入三首歌循环。", BRD.JobID, 0, "Sing-song", "Look, a raid contribution feature!\nShame nobody will thank you for it")]
        SimpleSongOption = 3011,

        [ParentCombo(BardoGCDAoEFeature)]
        [CustomComboInfo("AOE连击唱歌设置", "在AOE连击中加入三首歌循环。", BRD.JobID, 0, "", "Get your MIDI files ready, it's song time.")]
        BardSongsFeature = 3012,

        [CustomComboInfo("Buff技能设置", "将猛者强击/战斗之声整合至纷乱箭。", BRD.JobID, 0, "", "你被强化了，快送！")]
        BardBuffsFeature = 3013,

        [CustomComboInfo("一键唱歌", "将贤者的叙事谣/军神的赞美歌整合至放浪神的小步舞曲，并根据cd时间自动切换。", BRD.JobID, 0, "EDM songs", "They all sound the same, anyway.")]
        BardOneButtonSongs = 3014,

        [CustomComboInfo("AOE简易一键连击", "在连珠箭/百首龙牙箭连打时插入能力技。", BRD.JobID, 0, "", "Group attacks to make you feel like you're not the worst Ranged DPS in the room")]
        BardSimpleAoEFeature = 3015,

        [ParentCombo(BardSimpleAoEFeature)]
        [CustomComboInfo("AOE连击一键唱歌", "自动插入三首歌循环。", BRD.JobID, 0, "", "Wow. You're performing to a crowd now, huh")]
        SimpleAoESongOption = 3016,

        [ParentCombo(SimpleBardFeature)]
        [CustomComboInfo("单体连击Buff设置", "自动插入Buff技能。", BRD.JobID, 0, "", "Buff for me, buff for you.")]
        BardSimpleBuffsFeature = 3017,

        [ParentCombo(SimpleBardFeature)]
        [CustomComboInfo("光明神的最终乐章替换设置", "当可用时自动插入光明神的最终乐章。", BRD.JobID, 0, "", "Nothing radiant about it, if you ask me.")]
        BardSimpleBuffsRadiantFeature = 3018,

        [ParentCombo(SimpleBardFeature)]
        [CustomComboInfo("零式（高难）模式", "去除目标HP检测，在战斗即将结束时不自动替换dot/buff/歌技能。", BRD.JobID, 0, "But Muh Parse", "Just slings all the shit, all the time!")]
        BardSimpleRaidMode = 3019,

        [ParentCombo(SimpleBardFeature)]
        [CustomComboInfo("单体连击打断技能设置", "在合适时机插入打断技能。", BRD.JobID, 0, "Simple Interr-", "Excuse me, I wa-")]
        BardSimpleInterrupt = 3020,

        [CustomComboInfo("禁止绝峰箭自动替换", "不在一键连击中自动替换插入绝峰箭。", BRD.JobID, 0, "Disable Apex Legends", "Removing features? You? Surely not")]
        BardRemoveApexArrowFeature = 3021,

        [ConflictingCombos(BardoGCDSingleTargetFeature)]
        [ParentCombo(SimpleBardFeature)]
        [CustomComboInfo("单体简易起手", "在单体一键连击中加入最佳起手技能。\n此选项与其它绝大部分类似选项均有冲突。", BRD.JobID, 0, "Totally hands-off feature", "It's like watching a YouTube video!")]
        BardSimpleOpener = 3022,

        [ParentCombo(SimpleBardFeature)]
        [CustomComboInfo("单体连击失血箭设置", "一键连击中将失血箭集中于最佳爆发期自动替换插入。", BRD.JobID, 0, "Dancer pooling feature", "NOW you're Dancing.")]
        BardSimplePooling = 3023,

        [ParentCombo(BardIronJawsFeature)]
        [CustomComboInfo("伶牙俐齿替换绝峰箭", "当绝峰箭/爆破箭可用时，替换伶牙俐齿为绝峰箭/爆破箭。", BRD.JobID, 0, "", "Arrows this, jaws that.\nTruly the apex of BRD technology")]
        BardIronJawsApexFeature = 3024,

        [ParentCombo(SimpleBardFeature)]
        [CustomComboInfo("单体连击dot快照设置", "在猛者强击持续时间低于设定值时，自动替换插入dot技能。", BRD.JobID, 0, "诗人啥时候能跟召唤一样不用上毒了就好了。", "你在想屁吃!")]
        BardSimpleRagingJaws = 3025,

        #endregion
        // ====================================================================================
        #region DANCER

        // Single Target Multibutton Section
        [ConflictingCombos(DancerSimpleFeature, DancerSimpleAoEFeature)]
        [CustomComboInfo("单体目标整合", "替换瀑泻为对应触发连击。", DNC.JobID, 0, "", "")]
        DancerSingleTargetMultibutton = 4000,

        [ParentCombo(DancerSingleTargetMultibutton)]
        [CustomComboInfo("单体目标伶俐防溢出设置", "当拥有85点或以上伶俐时，将剑舞加入到瀑泻连击中。", DNC.JobID, 0, "", "")]
        DancerEspritOvercapSTFeature = 4001,

        [ParentCombo(DancerEspritOvercapSTFeature)]
        [CustomComboInfo("单体目标剑舞即用设置", "当拥有50点伶俐时，将剑舞加入到瀑泻连击中。\n覆写'单体目标伶俐防溢出设置'。", DNC.JobID, 0, "", "")]
        DancerEspritOvercapSTInstantOption = 4002,

        [ParentCombo(DancerSingleTargetMultibutton)]
        [CustomComboInfo("幻扇溢出保护", "当幻扇层数满时，将扇舞·序加入到瀑泻连击中。", DNC.JobID, 0, "", "")]
        DancerFanDanceMainComboOvercapFeature = 4003,

        [ParentCombo(DancerSingleTargetMultibutton)]
        [CustomComboInfo("扇舞替换瀑泻", "当扇舞·急/扇舞·终可用时自动替换瀑泻。", DNC.JobID, 0, "", "")]
        DancerFanDance34OnMainComboFeature = 4004,


        // AoE Multibutton Section
        [ConflictingCombos(DancerSimpleFeature, DancerSimpleAoEFeature)]
        [CustomComboInfo("AoE整合", "替换风车为对应触发连击。", DNC.JobID, 0, "", "")]
        DancerAoEMultibutton = 4010,

        [ParentCombo(DancerAoEMultibutton)]
        [CustomComboInfo("AoE伶俐防溢出设置", "当拥有85点或以上伶俐时，将剑舞加入到风车连击中。", DNC.JobID, 0, "", "")]
        DancerEspritOvercapAoEFeature = 4011,

        [ParentCombo(DancerEspritOvercapAoEFeature)]
        [CustomComboInfo("AoE剑舞即用设置", "当拥有50点伶俐时，将剑舞加入到风车连击中。\n覆写'AoE伶俐防溢出设置'。.", DNC.JobID, 0, "", "")]
        DancerEspritOvercapAoEInstantOption = 4012,

        [ParentCombo(DancerAoEMultibutton)]
        [CustomComboInfo("AoE幻扇溢出保护", "当幻扇层数满时，将扇舞·破加入到风车连击中。", DNC.JobID, 0, "", "")]
        DancerFanDanceAoEComboOvercapFeature = 4013,

        [ParentCombo(DancerAoEMultibutton)]
        [CustomComboInfo("AoE扇舞替换风车", "当扇舞·急/扇舞·终可用时自动替换风车。", DNC.JobID, 0, "", "")]
        DancerFanDanceOnAoEComboFeature = 4014,


        // Dance Features Section (SS/TS)
        [CustomComboInfo("跳舞功能", "标准舞步和技巧舞步相关功能与设置\n折叠此选项不会关闭其中的功能。", DNC.JobID, 0, "", "")]
        DancerMenuDanceFeatures = 4020,

        [ParentCombo(DancerMenuDanceFeatures)]
        [ConflictingCombos(DancerCombinedDanceFeature, DancerDanceComboCompatibility)]
        [CustomComboInfo("跳舞连击", "跳舞时将标准舞步和技巧舞步替换为对应的舞步技能。\n与'单体一键连击'和'AoE一键连击'兼容", DNC.JobID, 0, "", "")]
        DancerDanceStepCombo = 4021,

        [ParentCombo(DancerMenuDanceFeatures)]
        [ConflictingCombos(DancerDanceStepCombo, DancerDanceComboCompatibility, DancerSimpleFeature, DancerSimpleAoEFeature)]
        [CustomComboInfo("舞步整合", "将标准舞步与机巧舞步整合为一个技能。标准舞步>技巧舞步。 此连击后接续提拉纳和流星舞.", DNC.JobID, 0, "", "")]
        DancerCombinedDanceFeature = 4022,

        [ParentCombo(DancerCombinedDanceFeature)]
        [CustomComboInfo("进攻之探戈附加设置", "在技巧舞步结束后接续进攻之探戈。", DNC.JobID, 0, "", "")]
        DancerDevilmentOnCombinedDanceFeature = 4023,

        [ParentCombo(DancerCombinedDanceFeature)]
        [CustomComboInfo("百花争艳附加设置", "将百花争艳加入到舞步整合。", DNC.JobID, 0, "", "")]
        DancerFlourishOnCombinedDanceFeature = 4024,

        [ParentCombo(DancerMenuDanceFeatures)]
        [ConflictingCombos(DancerDanceStepCombo, DancerCombinedDanceFeature, DancerSimpleFeature, DancerSimpleAoEFeature)]
        [CustomComboInfo("自定义舞步功能",
        "自定义在跳舞时变为舞步技能的技能。" +
        "\n此功能可以让你开启了其他连击选项时仍然可以自己跳舞。" +
        "\n你可以通过输入技能ID来改变对应的舞步技能。" +
        "\n默认替换为瀑泻,百花争艳,扇舞·序,扇舞·破. 设置为0时会重置为这些技能。" +
        "\n你可以在Garland Tools找到技能的ID。", DNC.JobID, 0, "", "")]
        DancerDanceComboCompatibility = 4025,


        // Flourishing Features Section
        [ConflictingCombos(DancerSimpleFeature, DancerSimpleAoEFeature)]
        [CustomComboInfo("百花争艳期间功能", "幻扇和百花争艳相关功能与设置\n折叠此选项不会关闭其中的功能。", DNC.JobID, 0, "", "")]
        DancerMenuFlourishingFeatures = 4030,

        [ParentCombo(DancerMenuFlourishingFeatures)]
        [ConflictingCombos(DancerSimpleFeature, DancerSimpleAoEFeature)]
        [CustomComboInfo("百花争艳触发保护", "任意已触发技能可用时将百花争艳替换为对应已触发技能。(对称投掷、非对称投掷、扇舞·急预备、扇舞·终预备)", DNC.JobID, 0, "", "")]
        DancerFlourishProcFeature = 4031,

        [ParentCombo(DancerFlourishProcFeature)]
        [CustomComboInfo("触发保护扇舞设置", "将扇舞·急与扇舞·终加入到触发保护。?", DNC.JobID, 0, "", "")]
        DancerFlourishProcFanDanceWeaveOption = 4032,


        // Fan Dance Combo Features Section
        [ParentCombo(DancerMenuFlourishingFeatures)]
        [ConflictingCombos(DancerSimpleFeature, DancerSimpleAoEFeature)]
        [CustomComboInfo("扇舞连击功能", "扇舞·序连击设置。扇舞·急优先级高于扇舞·终。\n折叠此选项不会关闭其中的功能。", DNC.JobID, 0, "", "")]
        DancerFanDanceComboFeatures = 4033,

        [ParentCombo(DancerFanDanceComboFeatures)]
        [CustomComboInfo("扇舞·序 -> 扇舞·急", "当可用时，将扇舞·序替换为扇舞·急。", DNC.JobID, 0, "", "")]
        DancerFanDance1_3Combo = 4034,

        [ParentCombo(DancerFanDanceComboFeatures)]
        [CustomComboInfo("扇舞·序 -> 扇舞·终", "当可用时，将扇舞·序替换为扇舞·终。", DNC.JobID, 0, "", "")]
        DancerFanDance1_4Combo = 4035,

        [ParentCombo(DancerFanDanceComboFeatures)]
        [CustomComboInfo("扇舞·破 -> 扇舞·急", "当可用时，将扇舞·破替换为扇舞·急。", DNC.JobID, 0, "", "")]
        DancerFanDance2_3Combo = 4036,

        [ParentCombo(DancerFanDanceComboFeatures)]
        [CustomComboInfo("扇舞·破 -> 扇舞·终", "当可用时，将扇舞·破替换为扇舞·终。", DNC.JobID, 0, "", "")]
        DancerFanDance2_4Combo = 4037,

        [ConflictingCombos(DancerSimpleFeature, DancerSimpleAoEFeature)]
        [CustomComboInfo("进攻之探戈 -> 流星舞", "使用完进攻之探戈后将其替换为流星舞。", DNC.JobID, 0, "", "")]
        DancerDevilmentFeature = 4038,


        // Simple Dancer Section
        [ConflictingCombos(DancerSingleTargetMultibutton, DancerAoEMultibutton, DancerCombinedDanceFeature, DancerDanceComboCompatibility, DancerMenuFlourishingFeatures, DancerDevilmentFeature)]
        [CustomComboInfo("单体一键连击", "测试版 - 单体目标，一键连击。 包含跳舞，进攻之探戈，触发/溢出保护。\n与除了'跳舞连击'外的所有单体连击冲突\n后果自负!", DNC.JobID, 0, "", "")]
        DancerSimpleFeature = 4050,

        [ParentCombo(DancerSimpleFeature)]
        [CustomComboInfo("简易中断", "在循环中加入中断(伤头)(如果当前目标可被打断)。", DNC.JobID, 0, "", "")]
        DancerSimpleInterruptFeature = 4051,

        [ParentCombo(DancerSimpleFeature)]
        [CustomComboInfo("简易标准舞步", "将标准舞步加入循环。", DNC.JobID, 0, "", "")]
        DancerSimpleStandardFeature = 4052,

        [ParentCombo(DancerSimpleFeature)]
        [CustomComboInfo("简易技巧舞步", "将技巧舞步加入循环。", DNC.JobID, 0, "", "")]
        DancerSimpleTechnicalFeature = 4053,

        [ParentCombo(DancerSimpleFeature)]
        [CustomComboInfo("简易技巧进攻之探戈", "将进攻之探戈加入循环(技巧舞步结束状态下)。", DNC.JobID, 0, "", "")]
        DancerSimpleDevilmentFeature = 4054,

        [ParentCombo(DancerSimpleFeature)]
        [CustomComboInfo("简易百花争艳", "将百花争艳加入循环。", DNC.JobID, 0, "", "")]
        DancerSimpleFlourishFeature = 4055,

        [ParentCombo(DancerSimpleFeature)]
        [CustomComboInfo("简易幻扇", "将幻扇的使用加入循环。", DNC.JobID, 0, "", "")]
        DancerSimpleFeatherFeature = 4056,

        [ParentCombo(DancerSimpleFeature)]
        [CustomComboInfo("简易幻扇囤积", "只在幻扇层数大于3或技巧舞步状态下使用消耗幻扇层数的技能。", DNC.JobID, 0, "")]
        DancerSimpleFeatherPoolingFeature = 4057,

        [ParentCombo(DancerSimpleFeature)]
        [CustomComboInfo("简易紧急恢复", "当血量低于30%或50%时对应的使用治疗之华尔兹或内丹。", DNC.JobID, 0, "", "")]
        DancerSimplePanicHealsFeature = 4058,

        [ParentCombo(DancerSimpleFeature)]
        [CustomComboInfo("简易即兴表演", "当即兴表演可用时将其加入循环。", DNC.JobID, 0, "", "")]
        DancerSimpleImprovFeature = 4059,


        // Simple Dancer AoE Section
        [ConflictingCombos(DancerSingleTargetMultibutton, DancerAoEMultibutton, DancerCombinedDanceFeature, DancerDanceComboCompatibility, DancerMenuFlourishingFeatures, DancerDevilmentFeature)]
        [CustomComboInfo("AoE一键连击", "测试版 - AoE，一键连击。 包含跳舞，进攻之探戈，触发/溢出保护。\n与除了'跳舞连击'外的所有AoE连击冲突。\n后果自负!", DNC.JobID, 0, "", "")]
        DancerSimpleAoEFeature = 4070,

        [ParentCombo(DancerSimpleAoEFeature)]
        [CustomComboInfo("简易AoE中断", "在AoE循环中加入中断(伤头)(如果当前目标可被打断)。", DNC.JobID, 0, "", "")]
        DancerSimpleAoEInterruptFeature = 4071,

        [ParentCombo(DancerSimpleAoEFeature)]
        [CustomComboInfo("简易AoE标准舞步", "将标准舞步加入AoE循环。", DNC.JobID, 0, "")]
        DancerSimpleAoEStandardFeature = 4072,

        [ParentCombo(DancerSimpleAoEFeature)]
        [CustomComboInfo("简易AoE技巧舞步", "将技巧舞步加入AoE循环。", DNC.JobID, 0, "")]
        DancerSimpleAoETechnicalFeature = 4073,

        [ParentCombo(DancerSimpleAoEFeature)]
        [CustomComboInfo("简易AoE技巧进攻之探戈", "将进攻之探戈加入AoE循环(技巧舞步结束状态下)。", DNC.JobID, 0, "", "")]
        DancerSimpleAoEDevilmentFeature = 4074,

        [ParentCombo(DancerSimpleAoEFeature)]
        [CustomComboInfo("简易AoE百花争艳", "将百花争艳加入AoE循环。", DNC.JobID, 0, "", "")]
        DancerSimpleAoEFlourishFeature = 4075,

        [ParentCombo(DancerSimpleAoEFeature)]
        [CustomComboInfo("简易AoE幻扇", "将幻扇的使用加入AoE循环。", DNC.JobID, 0, "", "")]
        DancerSimpleAoEFeatherFeature = 4076,

        [ParentCombo(DancerSimpleAoEFeature)]
        [CustomComboInfo("简易AoE幻扇囤积", "只在幻扇层数大于3或技巧舞步状态下使用消耗幻扇层数的技能。", DNC.JobID, 0, "", "")]
        DancerSimpleAoEFeatherPoolingFeature = 4077,

        [ParentCombo(DancerSimpleAoEFeature)]
        [CustomComboInfo("简易AoE紧急恢复", "当血量低于30%或50%时对应的使用治疗之华尔兹或内丹。", DNC.JobID, 0, "", "")]
        DancerSimpleAoEPanicHealsFeature = 4078,

        [ParentCombo(DancerSimpleAoEFeature)]
        [CustomComboInfo("简易AoE即兴表演", "当即兴表演可用时将其加入AoE循环。", DNC.JobID, 0, "", "")]
        DancerSimpleAoEImprovFeature = 4079,

        #endregion
        // ====================================================================================
         #region DARK KNIGHT

        [ParentCombo(DarkSouleaterCombo)]
        [CustomComboInfo("主连击Buff整合", "将Buff类技能整合到主连击。", DRK.JobID)]
        DarkMainComboBuffsGroup = 5098,

        [ParentCombo(DarkSouleaterCombo)]
        [CustomComboInfo("主连击CD整合", "将CD类技能整合到主连击。", DRK.JobID)]
        DarkMainComboCDsGroup = 5099,

        [CustomComboInfo("噬魂斩连击", "用基础循环替换掉噬魂斩。 \n如果所有的次级选项都被开启，那么就可以进行一键循环(简单黑骑)", DRK.JobID, 0, "Fetch me their souls!", "Heheheheheh")]
        DarkSouleaterCombo = 5000,

        [CustomComboInfo("刚魂连击", "用基础循环替换掉刚魂。", DRK.JobID, 0, "", "Ugly name for an ugly job")]
        DarkStalwartSoulCombo = 5001,

        [ParentCombo(DarkMainComboBuffsGroup)]
        [CustomComboInfo("血乱特性", "当血乱激活时，使用血溅和寂灭替换掉噬魂战和刚魂。", DRK.JobID, 0, "", "Delirium is what you have if you choose to play DRK.\nDoc's words, not mine")]
        DeliriumFeature = 5002,

        [ParentCombo(DarkStalwartSoulCombo)]
        [CustomComboInfo("暗血量表溢出特性 SoulCombo", "当你的暗血即将溢出时，使用寂灭替换掉刚魂。", DRK.JobID, 0, "", "Hey big spenderrrrr")]
        DRKOvercapFeature = 5003,

        [ParentCombo(DarkMainComboCDsGroup)]
        [CustomComboInfo("掠影示现特性", "如果掠影示现不在冷却，并且你拥有足够的暗血时，将掠影示现整合到连击中。", DRK.JobID, 0, "", "Trick everyone into thinking a party member is standing where they shouldn't be!")]
        DRKLivingShadowFeature = 5004,

        [ParentCombo(DarkSouleaterCombo)]
        [CustomComboInfo("暗影锋溢出特性", "当你的MP超过8500或暗黑即将结束（少于十秒）时，将暗影锋整合到主连击。", DRK.JobID, 0, "Something about mana", "You're basically a black mage! Well done!")]
        DarkManaOvercapFeature = 5005,

        [CustomComboInfo("能力技特性", "按照掠影示现 > 腐秽大地 > 精雕怒斩 > 腐秽黑暗的顺序将能力技整合到精雕怒斩与吸血深渊。", DRK.JobID, 0, "", "Just does your whole job for you, really")]
        DarkoGCDFeature = 5006,

        [ParentCombo(DarkoGCDFeature)]
        [CustomComboInfo("暗影使者能力技特性", "将暗影使者整合到能力技特性。", DRK.JobID, 0, "", "Wasn't this last expansion?")]
        DarkShadowbringeroGCDFeature = 5007,

        [ParentCombo(DarkMainComboCDsGroup)]
        [CustomComboInfo("跳斩特性", "当暗黑激活时将跳斩添加到主连击。", DRK.JobID, 0, "", "Take the plunge. All the way!")]
        DarkPlungeFeature = 5008,

        [ParentCombo(DeliriumFeature)]
        [CustomComboInfo("血乱延后特性", "在偶数分钟窗口使用血乱时将延迟两个GCD，而在奇数分钟窗口不进行延后。这将可以使技能更容易打进团辅。", DRK.JobID, 0)]
        DelayedDeliriumFeatureOption = 5010,

        [ParentCombo(DarkSouleaterCombo)]
        [CustomComboInfo("伤残特性", "当你离开近战范围时使用伤残替换主连击。", DRK.JobID, 0, "Ranged DPS job now, duh", "Stubby little arms, huh")]
        DarkRangedUptimeFeature = 5011,

        [CustomComboInfo("插言特性", "当目标可被打断时，使用插言替换掉下踢。", DRK.JobID, 0, "Lower blow", "Blow, but low.")]
        DarkKnightInterruptFeature = 5012,

        [ParentCombo(DarkStalwartSoulCombo)]
        [CustomComboInfo("吸血深渊特性", "当你的血量低于60%时，使用吸血深渊替换AOE连击。", DRK.JobID, 0, "", "Even the un-cool kids got heals these days")]
        DRKStalwartabyssalDrainFeature = 5013,

        [ParentCombo(DarkStalwartSoulCombo)]
        [CustomComboInfo("AOE暗影使者特性", "将暗影使者整合到AOE连击。", DRK.JobID, 0, "", "Wasn't this last expansion?")]
        DRKStalwartShadowbringerFeature = 5014,

        [ParentCombo(DarkStalwartSoulCombo)]
        [CustomComboInfo("暗影波动溢出特性", "当你的MP超过8500或暗黑即将结束（少于十秒）时，将暗影波动整合到AOE连击。", DRK.JobID, 0, "Something about mana", "You're basically a black mage! Well done!")]
        DarkManaOvercapAoEFeature = 5015,

        [ParentCombo(DarkSouleaterCombo)]
        [CustomComboInfo("暗血量表溢出特性", "当你的暗血不低于80时，将血溅整合到主连击。", DRK.JobID, 0, "", "Take the plunge. Or, just dip your toes in. Whatever.")]
        DarkBloodGaugeOvercapFeature = 5016,

        [ParentCombo(DarkSouleaterCombo)]
        [CustomComboInfo("90级暗黑骑士起手", "将90级的暗黑骑士起手整合到主连击。 \n起手将在进入战斗前并且使用了至黑之夜或者嗜血之后触发。", DRK.JobID, 0)]
        DarkOpenerFeature = 5017,

        [ParentCombo(DarkOpenerFeature)]
        [CustomComboInfo("嗜血战斗外特性", "如果在战斗外使用了至黑之夜，那么将嗜血整合到主连击作为起手。", DRK.JobID, 0)]
        DarkBloodWeaponOpener = 5018,

        [ParentCombo(DarkMainComboCDsGroup)]
        [CustomComboInfo("暗影使者特性", "当暗黑激活并且暗影使者所有层数都不在冷却时，将暗影使者整合到主连击。", DRK.JobID, 0)]
        DarkShBFeature = 5019,

        [ParentCombo(DarkManaOvercapFeature)]
        [CustomComboInfo("暗影锋爆发设置", "在偶数分的爆发窗口中持续使用暗影锋，直到MP到达设定值。", DRK.JobID, 0)]
        DarkEoSPoolOption = 5020,

        [ParentCombo(DarkShBFeature)]
        [CustomComboInfo("暗影使者爆发设置", "将暗影使者打入偶数分钟的爆发窗口中。", DRK.JobID, 0)]
        DarkBurstShBOption = 5021,

        [ParentCombo(DarkMainComboCDsGroup)]
        [CustomComboInfo("精雕怒斩特性", "当暗黑激活时将精雕怒斩整合到主连击。", DRK.JobID, 0)]
        DarkCnSFeature = 5022,

        [ParentCombo(DarkPlungeFeature)]
        [CustomComboInfo("跳斩爆发设置", "将跳斩打入每分钟的爆发窗口中。", DRK.JobID, 0)]
        DarkPlungeBurstOption = 5023,

        [ParentCombo(DarkMainComboCDsGroup)]
        [CustomComboInfo("腐秽大地特性", "当暗黑激活时将腐秽大地整合到主连击。将会在你解锁了腐秽黑暗之后生效。", DRK.JobID, 0)]
        DarkSaltedEarthFeature = 5024,

        [ParentCombo(DeliriumFeature)]
        [CustomComboInfo("血乱CD整合", "当暗黑激活时将血乱整合到主连击，并且会在你拥有50点以上暗血时在血乱将要完成冷却之前打出一个血溅以防暗血溢出。", DRK.JobID, 0)]
        DarkDeliriumOnCD = 5025,

        [ParentCombo(DarkMainComboBuffsGroup)]
        [CustomComboInfo("嗜血CD整合", "当暗黑激活时将嗜血整合到主连击。", DRK.JobID, 0)]
        DarkBloodWeaponOption = 5026,
        
        [CustomComboInfo("雪仇保护机制", "当目标已被赋予雪仇效果时，将雪仇替换为飞石", DRK.JobID)]
        DarkKnightReprisalProtection = 5030,

        #endregion
        // ====================================================================================
        #region DRAGOON

        [ConflictingCombos(DragoonSimple)]
        [CustomComboInfo("幻象冲整合设置", "处于幻象冲预备状态时，替换（高）跳跃为幻象冲。", DRG.JobID, 0, "跳跃 off map", "Oh no, muh double-weaves!")]
        DragoonJumpFeature = 6000,

        [ConflictingCombos(DragoonSimpleAoE)] [CustomComboInfo("一键山境酷刑连", "替换山境酷刑为相应连击。", DRG.JobID, 1, "", "枪出如龙!")]
        DragoonCoerthanTormentCombo = 6100,

        [ConflictingCombos(DragoonSimple)] [CustomComboInfo("一键樱花连", "替换樱花怒放为相应连击。", DRG.JobID, 2, "", "回马枪!")]
        DragoonChaosThrustCombo = 6200,

        [ParentCombo(DragoonChaosThrustCombo)]
        [CustomComboInfo("一键樱花连设置", "处于近战攻击范围外时，替换樱花怒放连击为贯穿尖。", DRG.JobID, 3, "", "Never stop hitting them.")]
        DragoonPiercingTalonChaosFeature = 6201,

        [ConflictingCombos(DragoonFullThrustComboPlus, DragoonSimple)]
        [CustomComboInfo("一键直刺连", "替换直刺为相应连击。", DRG.JobID, 4, "", "Wait... Three combo?")]
        DragoonFullThrustCombo = 6300,

        [ParentCombo(DragoonFullThrustCombo)]
        [CustomComboInfo("一键直刺连设置", "处于近战攻击范围外时，替换直刺连击为贯穿尖。", DRG.JobID, 5, "", "Never stop hitting them.")]
        DragoonPiercingTalonFullFeature = 6301,

        [ConflictingCombos(DragoonFullThrustCombo, DragoonSimple)]
        [CustomComboInfo("一键直刺连Plus", "替换直刺为相应连击。 (智能替换 开膛枪/樱花怒放/龙剑)。", DRG.JobID, 6, "", "F- Three combo MORE!")]
        DragoonFullThrustComboPlus = 6400,

        [ParentCombo(DragoonFullThrustComboPlus)]
        [CustomComboInfo("（高）跳跃 Plus 设置", "在樱花/直刺/山境酷刑连击中插入（高）跳跃", DRG.JobID, 7, "", "跳跃ing, but higher.")]
        DragoonHighJumpPlusFeature = 6401,

        [ParentCombo(DragoonHighJumpPlusFeature)]
        [CustomComboInfo("幻象冲 Plus 设置", "输出循环中加入幻象冲。", DRG.JobID, 8, "", "一点寒芒先到。")]
        DragoonMiragePlusFeature = 6402,

        [ParentCombo(DragoonFullThrustComboPlus)]
        [CustomComboInfo("龙剑 Plus 设置", "在樱花/直刺/山境酷刑连击中的合适时机和buff下加入龙剑。", DRG.JobID, 9, "", "Hit them harder still.")]
        DragoonLifeSurgePlusFeature = 6404,

        [ParentCombo(DragoonFullThrustComboPlus)]
        [CustomComboInfo("直刺连Plus 设置", "处于近战攻击距离外时，替换直刺为贯穿尖。", DRG.JobID, 10, "", "Never stop hitting them.")]
        DragoonPiercingTalonPlusFeature = 6403,

        [ConflictingCombos(DragoonFullThrustCombo, DragoonFullThrustComboPlus, DragoonJumpFeature,
            DragoonChaosThrustCombo, DragoonFangThrustFeature, DragoonFangAndClawFeature)]
        [CustomComboInfo("单体一键连击", "同一个按键，同一种龙骑。 与任何单体输出选项都有冲突。", DRG.JobID, 11, "", "谁能挡我！")]
        DragoonSimple = 6500,

        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("简易起手", "88级以上时，在连击中插入起手技能。需要自行选择最优目标。", DRG.JobID, 12, "",
            "Open with a backjump into the wall!")]
        DragoonOpenerFeature = 6501,

        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("天龙点睛特性", "在连击中加入天龙点睛。", DRG.JobID, 13, "", "Blue and red Dragons! Oh my.")]
        DragoonWyrmwindFeature = 6502,

        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("武神枪与死者之岸特性", "在连击中插入武神枪与死者之岸。", DRG.JobID, 18, "",
            "Let me chuck another dragon soul at them!")]
        DragoonGeirskogulNastrondFeature = 6503,

        [ConflictingCombos(DragoonLitanyDiveFeature, DragoonLanceDiveFeature, DragoonLifeLitanyDiveFeature)]
        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("跳跃类能力技设置", "单插技能貌似还行: 在连击中插入破碎冲、龙炎冲、坠星冲。", DRG.JobID, 14, "", "Don't jump to your death!")]
        DragoonDiveFeature = 6504,

        [ConflictingCombos(DragoonDiveFeature, DragoonLitanyDiveFeature, DragoonLifeLitanyDiveFeature)]
        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("舍身状态下跳跃类能力技设置", "单插还不错: 在舍身状态下的连击中插入破碎冲/龙炎冲，在红莲龙血状态下插入坠星冲。", DRG.JobID, 17, "",
            "Don't jump to your death!")]
        DragoonLanceDiveFeature = 6505,

        [ConflictingCombos(DragoonDiveFeature, DragoonLanceDiveFeature, DragoonLifeLitanyDiveFeature)]
        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("战斗连祷状态下跳跃类能力技设置", "需要双插: 在战斗连祷状态下的连击中插入破碎冲/龙炎冲,在红莲龙血状态下插入坠星冲。", DRG.JobID, 15, "",
            "Don't jump to your death!")]
        DragoonLitanyDiveFeature = 6506,

        [ConflictingCombos(DragoonDiveFeature, DragoonLanceDiveFeature, DragoonLitanyDiveFeature)]
        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("战斗连祷与红莲龙血状态下跳跃类能力技设置", "需要双插:在同时处于战斗连祷和红莲龙血状态下的连击中插入破碎冲/龙炎冲/坠星冲。", DRG.JobID, 16, "",
            "Don't jump to your death!")]
        DragoonLifeLitanyDiveFeature = 6507,

        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("高 跳跃设置", "在连击中插入 高 跳跃。", DRG.JobID, 19, "", "Jumping, but higher.")]
        DragoonHighJumpFeature = 6508,

        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("幻象冲设置", "在连击中插入幻象冲。", DRG.JobID, 20, "", "They thought they saw it coming.")]
        DragoonMirageFeature = 6509,

        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("Buff设置", "在连击中插入猛枪和战斗连祷。", DRG.JobID, 21, "", "This is why your team loves you.")]
        DragoonBuffsFeature = 6510,

        [ParentCombo(DragoonBuffsFeature)]
        [CustomComboInfo("巨龙视线 Sight Feature",
            "Includes Dragon Sight to the rotation. OPTIONAL: USE REACTION OR MOACTION FOR OPTIMAL TARGETING.",
            DRG.JobID, 22, "", "This is why your team loves you, too.")]
        DragoonDragonSightFeature = 6511,

        [ParentCombo(DragoonSimple)]
        // [CustomComboInfo("龙剑设置", "在连击中合适的状态和窗口内插入龙剑。", DRG.JobID, 23, "", "Hit them with more feeling!")]
        [CustomComboInfo("龙剑 Feature",
            "Includes Life Surge, while under proper buffs, onto proper GCDs, to the rotation.", DRG.JobID, 23, "",
            "Hit them with more feeling!")]
        DragoonLifeSurgeFeature = 6512,

        [ParentCombo(DragoonSimple)]
        [CustomComboInfo("贯穿尖设置", "处于近战攻击范围外时，替换直刺连击为贯穿尖。 非最优操作！", DRG.JobID, 24, "", "Never stop hitting them.")]
        DragoonSimplePiercingTalonFeature = 6513,

        [ConflictingCombos(DragoonCoerthanTormentCombo)]
        [CustomComboInfo("AOE一键连击", "横扫千军！", DRG.JobID, 25, "", "Never stop hitting them ALL.")]
        DragoonSimpleAoE = 6600,

        [ParentCombo(DragoonSimpleAoE)]
        [CustomComboInfo("天龙点睛设置", "在连击中插入天龙点睛。", DRG.JobID, 26, "", "Blue and red Dragons! Oh my.")]
        DragoonAoEWyrmwindFeature = 6601,

        [ParentCombo(DragoonSimpleAoE)]
        [CustomComboInfo("武神枪与死者之岸设置", "在连击中插入武神枪/死者之岸。", DRG.JobID, 27, "",
            "Let me chuck another dragon soul at them!")]
        DragoonAoEGeirskogulNastrondFeature = 6602,

        [ConflictingCombos(DragoonAoELitanyDiveFeature, DragoonAoELifeLitanyDiveFeature)]
        [ParentCombo(DragoonSimpleAoE)]
        [CustomComboInfo("跳跃类能力技设置", "在连击中插入破碎冲/龙炎冲/坠星冲。", DRG.JobID, 28, "", "Don't jump to your death!")]
        DragoonAoEDiveFeature = 6603,

        [ConflictingCombos(DragoonAoEDiveFeature, DragoonAoELitanyDiveFeature, DragoonAoELifeLitanyDiveFeature)]
        [ParentCombo(DragoonSimpleAoE)]
        [CustomComboInfo("舍身状态下 跳跃类能力技设置", "单插还不错:在舍身状态下连击中插入破碎冲/龙炎冲，同时在红莲龙血状态下插入坠星冲。", DRG.JobID, 29, "",
            "Don't jump to your death!")]
        DragoonAoELanceDiveFeature = 6604,

        [ConflictingCombos(DragoonAoEDiveFeature, DragoonAoELanceDiveFeature, DragoonAoELifeLitanyDiveFeature)]
        [ParentCombo(DragoonSimpleAoE)]
        [CustomComboInfo("战斗连祷中 跳跃类能力技设置", "在战斗连祷状态下的连击中插入破碎冲/龙炎冲,同时在红莲龙血状态下插入坠星冲。", DRG.JobID, 30, "",
            "Don't jump to your death!")]
        DragoonAoELitanyDiveFeature = 6605,

        [ConflictingCombos(DragoonAoEDiveFeature, DragoonAoELanceDiveFeature, DragoonAoELitanyDiveFeature)]
        [ParentCombo(DragoonSimpleAoE)]
        [CustomComboInfo("战斗连祷告与红莲龙血状态下 跳跃类能力技设置", "在战斗连祷告与红莲龙血状态下连击中插入破碎冲/龙炎冲，同时在红莲龙血状态下插入坠星冲。", DRG.JobID, 31, "",
            "Don't jump to your death!")]
        DragoonAoELifeLitanyDiveFeature = 6606,

        [ParentCombo(DragoonSimpleAoE)]
        [CustomComboInfo("（高）跳跃设置", "在连击中插入（高）跳跃。", DRG.JobID, 32, "", "跳跃ing, but higher.")]
        DragoonAoEHighJumpFeature = 6607,

        [ParentCombo(DragoonSimpleAoE)]
        [CustomComboInfo("幻象冲设置", "在连击中插入幻象冲。", DRG.JobID, 33, "", "They thought they saw it coming.")]
        DragoonAoEMirageFeature = 6608,

        [ParentCombo(DragoonSimpleAoE)]
        [CustomComboInfo("buff设置", "在连击中插入猛枪和战斗连祷。", DRG.JobID, 34, "", "This is why your team loves you.")]
        DragoonAoEBuffsFeature = 6609,

        [ParentCombo(DragoonAoEBuffsFeature)]
        [CustomComboInfo("巨龙视线设置Aoe", "在连击中插入巨龙视线。需要自行选择最优目标。", DRG.JobID, 35, "",
            "This is why your team loves you, too.")]
        DragoonAoEDragonSightFeature = 6610,

        [ParentCombo(DragoonSimpleAoE)]
        [CustomComboInfo("龙剑设置 AoE Feature", "在连击中合适的状态和窗口内插入龙剑。", DRG.JobID, 36, "", "Hit them with more feeling!")]
        DragoonAoELifeSurgeFeature = 6611,

        [ConflictingCombos(DragoonSimple)]
        [CustomComboInfo("龙尾大回旋/龙牙龙爪 选项", "在龙牙龙爪或龙尾大回旋预备状态下，替换樱花连为龙尾大回旋，替换直刺连为龙牙龙爪. 需要开启一键樱花连和直刺连。", DRG.JobID, 37,
            "ALL THE COMBOS", "Turns Wheeling Thrust into 火炎 IV when Hallowed or PvP options are active.")]
        DragoonFangThrustFeature = 6700,

        [ConflictingCombos(DragoonSimple)]
        [CustomComboInfo("龙尾大回旋/龙牙龙爪 特性", "在龙尾大回旋预备状态下，替换龙牙龙爪为龙尾大回旋。", DRG.JobID, 38, "Reinventing the Wheel",
            "Absolutely WHEELING, my guy")]
        DragoonFangAndClawFeature = 6701,

        #endregion
        // ====================================================================================
        #region GUNBREAKER

        [CustomComboInfo("迅连斩连击", "将迅连斩替换为迅连斩连击。 \n如果所有的次级选项都被勾选，那么将可以做到一键循环。（简单枪刃）", GNB.JobID, 0, "Floppy Barrel Combo", "Not so solid NOW, are ya?")]
        GunbreakerSolidBarrelCombo = 7000,

        [ParentCombo(GunbreakerSolidBarrelCombo)]
        [CustomComboInfo("烈牙与续剑整合到主连击", "将烈牙与续剑整合到主连击。烈牙必须被手动激活，之后会被替换到主连击。 \n 当烈牙需要空转的时候，可以只激活此选项。", GNB.JobID, 0, "Fashing Gnang", "Why grandma, what big teeth you have!")]
        GunbreakerGnashingFangOnMain = 7001,

        [ParentCombo(GunbreakerSolidBarrelCombo)]
        [CustomComboInfo("冷却技能整合到主连击", "当无情进入冷却时将各种拥有冷却时间的技能整合到主连击。", GNB.JobID, 0, "Gee Whiz!", "Mom, I can't manage my oGCDs!")]
        GunbreakerMainComboCDsGroup = 7002,

        [ParentCombo(GunbreakerMainComboCDsGroup)]
        [CustomComboInfo("危险领域/爆破领域整合到主连击", "将危险领域/爆破领域整合到整合到主连击。", GNB.JobID, 0)]
        GunbreakerDZOnMainComboFeature = 7005,

        [ParentCombo(GunbreakerMainComboCDsGroup)]
        [CustomComboInfo("倍攻整合到主连击", "当你拥有无情buff时，将倍攻整合到主连击。", GNB.JobID, 0, "ALL the deeps", "For when you're both feeling merciless and are stuffed full of powder. BANG!")]
        GunbreakerDDonMain = 7003,

        [ParentCombo(GunbreakerSolidBarrelCombo)]
        [CustomComboInfo("粗分斩设置", "当可用时将粗分斩整合到主连击。", GNB.JobID, 0, "Divide... Roughly", "Ayo pour one out for the homie Squall")]
        GunbreakerRoughDivideFeature = 7004,

        [ParentCombo(GunbreakerDemonSlaughterCombo)]
        [CustomComboInfo("弓形冲波整合到AOE连击", "当弓形冲波冷却完成时将其整合到AOE连击。推荐使用烈牙特性", GNB.JobID, 0, "AoE cattleprod enabler")]
        GunbreakerBowShockFeature = 7017,

        [CustomComboInfo("恶魔杀连击", "将恶魔杀替换为恶魔杀连击。", GNB.JobID, 0, "dEmOn SlAuGhTeR", "恶魔杀? Really? What is this, RPR?")]
        GunbreakerDemonSlaughterCombo = 7006,

        [CustomComboInfo("晶壤溢出特性", "当晶壤将要溢出时使用爆发击/命运之环替换掉单体/AOE连击。", GNB.JobID, 0, "Pew Pew Forever", "The whole nine yards")]
        GunbreakerAmmoOvercapFeature = 7007,

        [CustomComboInfo("烈牙续剑连击", "将续剑添加到烈牙.", GNB.JobID, 0, "More Mercy", "More, no wait, less, no wait, MORE Mercy! No, wait...")]
        GunbreakerGnashingFangCombo = 7008,

        [ParentCombo(GunbreakerGnashingFangCombo)]
        [CustomComboInfo("无情整合到烈牙", "当无情冷却结束时将其整合到烈牙.", GNB.JobID, 0, "Blue DPS be like", "This one comes with teeth.")]
        GunbreakerNoMercyonGF = 7009,

        [ParentCombo(GunbreakerGnashingFangCombo)]
        [CustomComboInfo("倍攻整合到烈牙", "当拥有无情BUFF时将倍攻整合到烈牙", GNB.JobID, 0, "My hotbars are a (tiny) mess!", "Thanks plogon!")]
        GunbreakerDDOnGF = 7010,

        [ParentCombo(GunbreakerGnashingFangCombo)]
        [CustomComboInfo("冷却技能整合到烈牙", "当拥有无情BUFF时将音速破/弓形冲波/爆破领域整合到烈牙。 \n当拥有无情BUFF时爆发击也将会被整合到烈牙。", GNB.JobID, 0, "More Teeth", "Gnashing fang, but like, if a shark did it. Or something.")]
        GunbreakerCDsOnGF = 7011,

        [CustomComboInfo("爆发击续剑", "将超音速添加到爆发击/主连击以及烈牙。", GNB.JobID, 0, "Swish, swoosh", "Now we're cooking with gas! Hyper!")]
        GunbreakerBurstStrikeConFeature = 7012,

        [CustomComboInfo("血壤替换爆发击特性", "当你没有晶壤可用时使用血壤替换爆发击。", GNB.JobID, 0, "P4S Vampire man 血壤 Feature", "Again with the edgelord names?\nTut, tut, Yoshi-P. Do better.")]
        GunbreakerBloodfestOvercapFeature = 7013,

        [ParentCombo(GunbreakerSolidBarrelCombo)]
        [CustomComboInfo("闪雷弹激活", "当你离开近战攻击范围时使用闪雷弹替换主连击。", GNB.JobID, 0, "Stubby-armed GNB", "Can't reach?")]
        GunbreakerRangedUptimeFeature = 7015,

        [CustomComboInfo("插言特性", "当目标可被打断时，使用插言替换掉下踢。", GNB.JobID, 0, "Lower blow", "Blow, but low.")]
        GunbreakerInterruptFeature = 7016,

        [ParentCombo(GunbreakerMainComboCDsGroup)]
        [CustomComboInfo("血壤整合到主连击", "当晶壤为0时将血壤整合到主连击。", GNB.JobID, 0)]
        GunbreakerBloodfestonST = 7014,

        [ParentCombo(GunbreakerSolidBarrelCombo)]
        [CustomComboInfo("无情整合到主连击", "当晶壤满时将无情整合到主连击。", GNB.JobID, 0)]
        GunbreakerNoMercyonST = 7018,

        [ParentCombo(GunbreakerGnashingFangOnMain)]
        [CustomComboInfo("烈牙启动", "将烈牙也整合到主连击。", GNB.JobID, 0)]
        GunbreakerGFStartonMain = 7019,

        [ParentCombo(GunbreakerMainComboCDsGroup)]
        [CustomComboInfo("弓形冲波整合到主连击", "将弓形冲波整合到主连击", GNB.JobID, 0)]
        GunbreakerBSOnMainComboFeature = 7020,

        [ParentCombo(GunbreakerMainComboCDsGroup)]
        [CustomComboInfo("音速破整合到主连击", "将音速破整合到主连击", GNB.JobID, 0)]
        GunbreakerSBOnMainComboFeature = 7021,

        [CustomComboInfo("音速破/弓形冲波整合到无情", "当无情进入CD时，将音速破/弓形冲波整合到无情。", GNB.JobID, 0)]
        GunbreakerCDsonNMFeature = 7022,

        [ParentCombo(GunbreakerMainComboCDsGroup)]
        [CustomComboInfo("爆发击整合到主连击", "当拥有无情BUFF并且结束烈牙连击之后，将爆发击整合到主连击", GNB.JobID, 0)]
        GunbreakerBSinNMFeature = 7023,
        
        [CustomComboInfo("血仇保护机制", "当目标已被赋予血仇时，将血仇替换为飞石。", GNB.JobID)]
        GunbreakerReprisalProtection = 7030,

        #endregion
        // ====================================================================================
         #region MACHINIST

        [CustomComboInfo("(Heated) Shot Combo", "替换狙击弹为连击循环中的任一技能", MCH.JobID, 0, "Alright, 热弹", "是否真的存在一击必杀呢？让我们试试看吧。")]
        MachinistMainCombo = 8000,

        [CustomComboInfo("Overdrive Feature", "在技能可用时，将车式浮空炮塔和后式自走人偶转换为超档车式炮塔和超档自走人偶", MCH.JobID, 0, "Drive (2011) Feature", "Insert synthwave soundtrack here.")]
        MachinistOverdriveFeature = 8002,

        [CustomComboInfo("虹吸弹 / Ricochet Feature", "将虹吸弹和弹射替换为一个或其他需要更多充电电能的技能.", MCH.JobID, 0, "Gatling feature", "It's just a lot of bullets, really.")]
        MachinistGaussRoundRicochetFeature = 8003,

        [CustomComboInfo("Drill / Air Anchor (热弹) Feature", "将钻头和热弹替换成一个或多个需要冷却时使用的技能", MCH.JobID, 0, "Multi-tool", "Why does MCH have a drill and a chainsaw? What is this, DoH?")]
        MachinistHotShotDrillChainsawFeature = 8004,

        [ParentCombo(MachinistMainCombo)]
        [ConflictingCombos(MachinistAlternateMainCombo)]
        [CustomComboInfo("钻头/Air/Chain Saw Feature On Main Combo", "如果使用整备，空气锚后的钻头将会被加入到连击循环.\nIf Air Anchor is on cooldown and you use 整备, Chainsaw will be added to main combo instead.", MCH.JobID, 0, "A bit of everything feature", "Don't rub your last two brain-cells together! We got you!")]
        MachinistDrillAirOnMainCombo = 8005,

        [CustomComboInfo("Single Button 热冲击", "将热冲击转换为充能.", MCH.JobID, 0, "So-called '热冲击'", "Basically a large hair-dryer.")]
        MachinistHeatblastGaussRicochetFeature = 8006,

        [CustomComboInfo("Single Button 自动弩", "将自动弩转换为超荷和虹吸弹/rico.", MCH.JobID, 0, "Laser Crossbow", "It's a crossbow, from the future!")]
        MachinistAutoCrossBowGaussRicochetFeature = 8018,

        [ParentCombo(MachinistMainCombo)]
        [ConflictingCombos(MachinistDrillAirOnMainCombo)]
        [CustomComboInfo("Alternate 钻头/Air Feature on Main Combo", "钻头/空气锚/热弹被添加到连击循环中（注意：只有当你在整备Buff\n时才会将它们添加到连击循环。（或整备在CD中时不会使用这些技能）\nOr Reasemble is on CD (Will do nothing if 整备 is OFF CD)", MCH.JobID, 0, "Gunbreaker feature", "Look at me - I'm the tank now.")]
        MachinistAlternateMainCombo = 8007,

        [ParentCombo(MachinistMainCombo)]
        [CustomComboInfo("Single Button HeatBlast On Main Combo Option", "当选项可用时，将单体开关热冲击添加到主连击循环中", MCH.JobID, 0, "Scalp burner", "Dry your hair even faster! Even hotter! Owch!")]
        MachinistHeatBlastOnMainCombo = 8008,

        [ParentCombo(MachinistMainCombo)]
        [CustomComboInfo("Battery Overcap Option", "为您提供电能过量时的电能保护，如果您的电量达到100，则会在您的过热循环中加入炮台/女王。", MCH.JobID, 0, "Battery", "Isn't battery like, assault? What's wrong with you?")]
        MachinistOverChargeOption = 8009,

        [ParentCombo(MachinistSpreadShotFeature)]
        [CustomComboInfo("Battery AOE Overcap Option", "为散射/霰弹枪增加了过电量保护。.", MCH.JobID, 0, "Battery", "Isn't battery like, assault? What's wrong with you?")]
        MachinistAoEOverChargeOption = 8010,

        [ParentCombo(MachinistSpreadShotFeature)]
        [CustomComboInfo("虹吸弹/弹射AOE Feature", "在超荷过程中为AOE连击循环增加虹吸弹/弹射", MCH.JobID, 0, "Ratchet and Clank Feature", "Bouncing off the walls")]
        MachinistAoEGaussRicochetFeature = 8011,

        [ParentCombo(MachinistAoEGaussRicochetFeature)]
        [CustomComboInfo("Always 虹吸弹/Ricochet on AoE Option", "在超荷窗口之外的AOE连击循环中加入虹吸弹/弹射.", MCH.JobID, 0, "Ratchet and Clank Feature", "Bouncing off the walls")]
        MachinistAoEGaussOption = 8012,

        [ConflictingCombos(MachinistRicochetGaussMainCombo)]
        [ParentCombo(MachinistMainCombo)]
        [CustomComboInfo("弹射 & 虹吸弹 Feature", "将弹射和虹吸弹添加到主连击循环中.这将使用所有电能.", MCH.JobID, 0, "Pew +1", "All the shots, ALL of the time.")]
        MachinistRicochetGaussChargesMainCombo = 8017,

        [ConflictingCombos(MachinistRicochetGaussChargesMainCombo)]
        [ParentCombo(MachinistMainCombo)]
        [CustomComboInfo("弹射 & 虹吸弹 overcap protection option", "将弹射和虹吸弹添加到主连击循环中.将各留下一个电能.", MCH.JobID, 0, "", "Never run out of bullets again! Or never have too many! Or something!\nBullets!")]
        MachinistRicochetGaussMainCombo = 8013,

        [ParentCombo(MachinistMainCombo)]
        [CustomComboInfo("枪管加热 drift protection feature", "如果热量在5-20之间，则将枪管加热添加到主连击循环中。", MCH.JobID, 0, "Third Leg", "Adds Barrel Stabiliser when you're in heat. You heard me.")]
        BarrelStabilizerDrift = 8014,

        [ParentCombo(MachinistHeatblastGaussRicochetFeature)]
        [CustomComboInfo("野火 Feature", "在单体目标热冲击功能中加入野火，如果你有足够的热量用于超荷而野火的cd已经转好，那么超荷将被野火取代。\nAlso weaves Ricochet/虹吸弹 on Heat Blast when necessary.", MCH.JobID, 0, "PEW", "PEW PEW PEW PEW, AND PEW.\nALSO WEAVES PEW AND PEW.")]
        MachinistWildfireFeature = 8015,

        [ParentCombo(MachinistSpreadShotFeature)]
        [CustomComboInfo("毒菌冲击 Feature", "在散射的功能中添加毒菌冲击", MCH.JobID, 0, "War crime feature", "This is like uh, some sort of anthrax? Jeez bro. Relax.\nBanned by the Geneva Convention.")]
        MachinistBioblasterFeature = 8016,

        [CustomComboInfo("枪管加热 Feature", "当热能低于50且处于冷却状态时，将枪管充能添加到单目标热冲击和单目标自动弩功能中。", MCH.JobID, 0, "Hot Cross Bow", "Now multi-purpose!")]
        MachinistAutoBarrel = 8019,

        [ConflictingCombos(MachinistMainCombo)]
        [CustomComboInfo("Simple Machinist", "Single button single target machinist, including buffs and overprotections.\nConflicts with other single target toggles!!\nMade to work optimally with a 2.5 GCD.", MCH.JobID, 0, "", "Goodbye, brain!")]
        MachinistSimpleFeature = 8020,

        [ParentCombo(MachinistSimpleFeature)]
        [CustomComboInfo("简单打断", "如果适用的话，在机工的简单循环中使用打断技能.", MCH.JobID, 0, "Simple Interr-", "Excuse me, I wa-")]
        MachinistSimpleInterrupt = 8021,

        [ParentCombo(MachinistSimpleFeature)]
        [CustomComboInfo("简单的机工召唤", "根据你目前的等级，增加超档车式炮塔和超档自走人偶的技能.\n试着在:55到:05窗口之间的最佳间隔期使用超档车式炮塔和超档自走人偶.", MCH.JobID, 0, "Gadgeteer", "My queen!")]
        MachinistSimpleGadget = 8022,

        [ParentCombo(MachinistSimpleFeature)]
        [CustomComboInfo("简单的组装", "整备将与下面的技能一起使用.\n在获得钻头之前，它将使用与狙击弹共用.", MCH.JobID, 0, "Megablox", "It's like Lego bricks! But worse!")]
        MachinistSimpleAssembling = 8023,

        [ParentCombo(MachinistSimpleFeature)]
        [CustomComboInfo("简单的虹吸弹和弹射", "将虹吸弹和弹射加入到循环中.", MCH.JobID, 0, "", "More bullets!")]
        MachinistSimpleGaussRicochet = 8024,

        [ParentCombo(MachinistSimpleFeature)]
        [CustomComboInfo("简单的野火和超荷", "将超荷和野火加入到循环中.\n它遵从钻头、空气锚和链锯的8秒规则.", MCH.JobID, 0, "Simply fantastic", "Is it really THAT simple?")]
        MachinistSimpleWildCharge = 8025,

        [ParentCombo(MachinistSimpleFeature)]
        [CustomComboInfo("枪管加热", "将枪管加热加入到循环中.\n当热量小于50并且野火不在CD或者野火CD快要转好了时使用.", MCH.JobID, 0, "Third Leg", "Reduces recoil when spraying and praying.")]
        MachinistSimpleStabilizer = 8026,

        [ParentCombo(MachinistSpreadShotFeature)]
        [CustomComboInfo("超荷", "将超荷加入到AOE循环中.", MCH.JobID, 0, "Sugar Rush", "I'm gonna slap @augporto for putting in so many damn MCH features.\nHow full of witty one-liners do you think I am?!")]
        MachinistAoEHyperchargeFeature = 8027,

        [CustomComboInfo("简单机工AOE", "当等级为82或者更高时将散射转换为霰弹枪, Both turn into 自动弩 when overheated\nand 毒菌冲击 is used first whenever it is off cooldown.", MCH.JobID, 0, "Dungeon go zzzz", "AoE, but you're just not here. Go make a coffee.")]
        MachinistSpreadShotFeature = 8028,

        [ParentCombo(MachinistSimpleAssembling)]
        [CustomComboInfo("钻头","当可以的时候整备与钻头一起使用.", MCH.JobID,0,"刺鱼aDrillBreaker","The 钻头 that will pierce the heavens!")]
        MachinistSimpleAssemblingDrill = 8029,

        [ParentCombo(MachinistSimpleAssembling)]
        [CustomComboInfo("空气锚", "当可以的时候整备与空气锚一起使用.", MCH.JobID, 0, "空气锚", "出卡 the tunes!")]
        MachinistSimpleAssemblingAirAnchor = 8030,

        [ParentCombo(MachinistSimpleAssembling)]
        [CustomComboInfo("回转飞锯", "当可以的时候整备与回转飞锯一起使用.", MCH.JobID, 0, "刺鱼a Sauce", "The secret to life.")]
        MachinistSimpleAssemblingChainSaw = 8031,

        [ParentCombo(MachinistSimpleAssemblingDrill)]
        [CustomComboInfo("只使用钻头...", "...当你整备充能是最大值时.", MCH.JobID, 0, "刺鱼aDrillBreaker MAX", "Mow pow in the pew!")]
        MachinistSimpleAssemblingDrillMaxCharges = 8032,

        [ParentCombo(MachinistSimpleAssemblingAirAnchor)]
        [CustomComboInfo("只使用空气锚...", "...当你整备充能是最大值时.", MCH.JobID, 0, "刺鱼aDrillBreaker MAX", "Mow pow in the pew!")]
        MachinistSimpleAssemblingAirAnchorMaxCharges = 8033,

        [ParentCombo(MachinistSimpleAssemblingChainSaw)]
        [CustomComboInfo("只使用回转飞锯...", "...当你整备充能是最大值时.", MCH.JobID, 0, "刺鱼aDrillBreaker MAX", "Mow pow in the pew!")]
        MachinistSimpleAssemblingChainSawMaxCharges = 8034,

        #endregion
        // ====================================================================================
        #region MONK

        [CustomComboInfo("AOE连击", "整合AOE连击到破坏神脚/破坏神冲。", MNK.JobID, 0, "", "更大范围的拳头，就像你拥有了其它的手一样！")]
        MnkAoECombo = 9000,

        [CustomComboInfo("连击特性", "在拥有连击效果提高时使用连击替换双龙脚。", MNK.JobID, 0, "", "闪亮的鞋子，对吧？")]
        MnkBootshineFeature = 9001,

        [CustomComboInfo("双掌打特性", "如果你功力buff不足六秒，使用双掌打替换正拳。", MNK.JobID, 0, "", "我他X的受够了这架他X的飞机上的蛇！")]
        MnkTwinSnakesFeature = 9011,

        [ConflictingCombos(MnkBootshineCombo)]
        [CustomComboInfo("基础循环", "整合一键基础循环", MNK.JobID, 0, "", "我按，我暗。")]
        MnkBasicCombo = 9002,

        [CustomComboInfo("震脚特性", "如果你拥有三档脉轮，那么使用必杀技替换震脚。", MNK.JobID, 0, "", "他们说生活就像是在走钢丝……")]
        MonkPerfectBalanceFeature = 9003,

        [CustomComboInfo("连击平衡特性", "如果你拥有三档脉轮，那么使用必杀技替换双龙脚。", MNK.JobID, 0, "The tin", "Does what it says on the tin")]
        MnkBootshineBalanceFeature = 9004,

        [CustomComboInfo("空鸣拳/万象斗气圈特性", "当你的斗气满时使用空鸣拳/万象斗气圈替换掉破坏神冲", MNK.JobID, 0, "", "Imagine using your fist to scream at someone. Welcome to MNK!\nEnjoy your stay.")]
        MonkHowlingFistMeditationFeature = 9005,

        [ConflictingCombos(MnkBasicCombo)]
        [CustomComboInfo("连击连击", "用基础循环替换掉连击 \n如果所有次级选项都被勾选，那么就会变成一键循环。（简单武僧）", MNK.JobID, 0, "", "They call it 'basic' for a reason, you donkey")]
        MnkBootshineCombo = 9006,

        [CustomComboInfo("震脚特性+", "当震脚激活时，使用可用的技能替换必杀技。", MNK.JobID, 0, "", "Try not to fall over, eh")]
        MnkPerfectBalancePlus = 9007,

        [ParentCombo(MnkBootshineCombo)]
        [CustomComboInfo("必杀技加入循环", "将必杀技加入循环", MNK.JobID, 0, "", "It's maths, but for your Single Target combo!")]
        MonkMasterfulBlitzOnMainCombo = 9008,

        [ParentCombo(MnkAoECombo)]
        [CustomComboInfo("必杀技加入AOE循环", "将必杀技加入AOE循环。", MNK.JobID, 0, "", "It's maths, but for your AoE combo!")]
        MonkMasterfullBlizOnAoECombo = 9009,

        [CustomComboInfo("斗气特性", "将万象斗气圈/阴阳斗气斩加入单体/AOE连击。现在测试中！", MNK.JobID, 0, "But, wait!", "Mom said no! It's forbidden!")]
        MonkForbiddenChakraFeature = 9010,

        [CustomComboInfo("红莲极意/义结金兰特性", "如果红莲极意进入冷却，那么使用义结金兰替换红莲极意。", MNK.JobID, 0, "", "Riddle me this, brotha'")]
        MnkRiddleOfFireBrotherhoodFeature = 9012,

        [ParentCombo(MnkBootshineCombo)]
        [CustomComboInfo("CD整合", "当红莲极意进入冷却时，将各种能力技整合进连击。", MNK.JobID, 0, "", "Been passed on for dance partner again? Don't worry, you have some buffs of your own.")]
        MnkCDsOnMainComboFeature = 9013,

        [ParentCombo(MnkCDsOnMainComboFeature)]
        [CustomComboInfo("疾风极意整合", "将疾风极意整合进连击。", MNK.JobID, 0, "", "Easy, breezy.")]
        MnkRiddleOfWindOnMainComboFeature = 9014,

        [ParentCombo(MnkCDsOnMainComboFeature)]
        [CustomComboInfo("震脚整合", "将震脚整合进连击。", MNK.JobID, 0, "", "")]
        MnkPerfectBalanceOnMainComboFeature = 9015,

        [ParentCombo(MnkCDsOnMainComboFeature)]
        [CustomComboInfo("义结金兰整合", "将义结金兰整合进连击。", MNK.JobID, 0, "", "")]
        MnkBrotherhoodOnMainComboFeature = 9016,

        [ParentCombo(MnkCDsOnMainComboFeature)]
        [CustomComboInfo("斗气整合", "将斗气整合进连击。", MNK.JobID, 0, "", "")]
        MnkMeditationOnMainComboFeature = 9017,

        [ParentCombo(MnkBootshineCombo)]
        [CustomComboInfo("阴阳开场", "将阴阳脉轮开场整合进连击。需要68级（习得红莲极意）。", MNK.JobID, 0, "", "")]
        MnkLunarSolarOpenerOnMainComboFeature = 9018,

        #endregion
        // ====================================================================================
        #region NINJA

        [ConflictingCombos(NinSimpleSingleTarget)]
        [CustomComboInfo("强甲破点突 Combo", "使用 强甲破点突 做为连击的起始技.", NIN.JobID, 0, "One, Two, Three", "It's a Ninja's life for me")]
        NinjaArmorCrushCombo = 10000,

        [ConflictingCombos(NinSimpleSingleTarget)]
        [CustomComboInfo("旋风刃 Combo", "使用 旋风刃 做为连击的起始技.", NIN.JobID, 0, "Edgy Edge Combo", "Knife go stab")]
        NinjaAeolianEdgeCombo = 10001,

        //[CustomComboInfo("Simple AoE", "Replaces 血雨飞花 with the AoE rotation.", NIN.JobID)]
        //NinjaHakkeMujinsatsuCombo = 10002,

        //[CustomComboInfo("Dream to 断绝", "Replace Dream Within a Dream with 断绝 when 断绝 Ready.", NIN.JobID)]
        //NinjaAssassinateFeature = 10003,

        [CustomComboInfo("生杀予夺 to 攻其不备", "影遁状态下或发动水遁之术后，使用 攻其不备 替换 生杀予夺.\n推荐同时使用冷却CD监视插件.", NIN.JobID, 0, "Katsu Curry to Trick", "This is how we eat at a restaurant and don't pay the bill.\nRUN!")]
        NinjaKassatsuTrickFeature = 10004,

        [CustomComboInfo("天地人 to 命水", "发动水遁之术 后，使用 命水 替换 天地人.\n推荐同时使用冷却CD监视插件.", NIN.JobID, 0, "天之印 Chin Scratches to Chop-Suey", "Does something, probably.\nHow do you deal with all these attack names?")]
        NinjaTCJMeisuiFeature = 10005,

        [CustomComboInfo("生杀予夺 地之印/人之印 Feature", "发动 生杀予夺 后，使用 人之印 替换 地之印.", NIN.JobID, 0, "", "Swaps your Katsu curry with a 地之印 地之印n-scratch.")]
        NinjaKassatsuChiJinFeature = 10006,

        [CustomComboInfo("隐遁 to 夺取", "战斗状态下，使用 夺取 替换 隐遁.", NIN.JobID, 0, "Stand and Deliver", "John Cena is a thief, now?")]
        NinjaHideMugFeature = 10007,

        //[CustomComboInfo("旋风刃 to 忍术 Feature", "此条效果无效：Replaces 旋风刃 (combo) with Ninjutsu if any Mudra are used.", NIN.JobID, 0, "Hand signs and all that", "Do the Naruto thing, I think.\nIdk I don't watch anime, sorry")]
        //NinjaNinjutsuFeature = 10008,

        [ConflictingCombos(NinSimpleSingleTarget)]
        [CustomComboInfo("GCDs to 忍术 Feature", "当忍术结印后，所有GCD连击技能都将被替换为 忍术.", NIN.JobID, 0, "Full-on Sign Language", "NOW you're really communicating with the party.")]
        NinjaGCDNinjutsuFeature = 10009,

        [CustomComboInfo("风来刃 / Raiju Feature", "当 月影雷兽爪 和 月影雷兽牙 可以使用时，替换 风来刃.", NIN.JobID, 0, "Pikachu / Raichu Feature", "Does something? Maybe? Evolutions? Combos? Probably.")]
        NinjaHuraijinRaijuFeature = 10010,

        [ParentCombo(NinjaHuraijinRaijuFeature)]
        [CustomComboInfo("风来刃 / Raiju Feature Option 1", "当 月影雷兽牙 可以使用时，替换 风来刃.", NIN.JobID, 0, "Pikachu / Raichu Option 1", "Does the same thing probably, who knows.")]
        NinjaHuraijinRaijuFeature1 = 10011,

        [ParentCombo(NinjaHuraijinRaijuFeature)]
        [CustomComboInfo("风来刃 / Raiju Feature Option 2", "当 月影雷兽爪 可以使用时，替换 风来刃.", NIN.JobID, 0, "Pikachu / Raichu Option 2", "What we DO know, is that all NIN mains just slam their heads on the keyboard to do combos, anyway.")]
        NinjaHuraijinRaijuFeature2 = 10012,

        [ParentCombo(NinjaAeolianEdgeCombo)]
        [CustomComboInfo("强甲破点突 Feature", "将 强甲破点突 加入到连击序列中.", NIN.JobID, 0, "", "Act like you can crush armor with your kitchen knives or whatever.")]
        NinjaArmorCrushOnMainCombo = 10013,

        [ParentCombo(NinjaAeolianEdgeCombo)]
        [CustomComboInfo("雷兽 Feature", "将 月影雷兽牙 加入到 旋风刃 连击序列中.", NIN.JobID, 0, "Raichu Feature", "闪雷bolt!")]
        NinjaFleetingRaijuFeature = 10014,

        [ParentCombo(NinjaAeolianEdgeCombo)]
        [CustomComboInfo("风来刃ToMainCombo", "当风遁buff持续时间不足时，将 风来刃 加入到连击序列中.", NIN.JobID, 0, "", "Smells like a hurricane. No idea.")]
        NinjaHuraijinFeature = 10015,

        [ParentCombo(NinjaAeolianEdgeCombo)]
        [CustomComboInfo("分身之术OnMainCombo", "当 分身之术 处于冷却状态且拥有足够的忍气时，将其加入到连击序列中.", NIN.JobID, 0, "What do you call a Viera that's been cut off at the knees?", "Bun-shin KEK")]
        NinjaBunshinFeature = 10016,

        [ParentCombo(NinjaAeolianEdgeCombo)]
        [CustomComboInfo("六道轮回OnMainCombo", "当拥有足够的忍气时，将 六道轮回 加入到连击序列中.", NIN.JobID, 0, "BAKLAVA!", "BAKLAVA!")]
        NinjaBhavacakraFeature = 10017,

        [ParentCombo(NinjaAeolianEdgeCombo)]
        [CustomComboInfo("飞刀 Uptime Feature", "当目标超出攻击范围时，将 旋风刃 替换为 飞刀.", NIN.JobID, 0, "", "Would probably make more sense for NIN to be a Ranged DPS, anyway.")]
        NinjaRangedUptimeFeature = 10018,

        [CustomComboInfo("Simple Mudras", "简化忍术结印的操作.", NIN.JobID, 0, "Simple Murder", "Murder, made simple. For the everyday user.")]
        NinjaSimpleMudras = 10020,

        [ParentCombo(NinjaTCJMeisuiFeature)]
        [CustomComboInfo("天地人 Feature", "发动 天地人 之后，将其依序变为 天之印, 地之印, 人之印.", NIN.JobID, 0, "", "Does literally nothing. Ever")]
        NinTCJFeature = 10021,

        [ConflictingCombos(NinjaArmorCrushCombo, NinjaAeolianEdgeCombo, NinjaGCDNinjutsuFeature)]
        [CustomComboInfo("Simple Ninja Single Target", "将 双刃旋 做为单目标一键连击的起始技i.\n在连击中使用忍术、攻其不备，并使用强甲破点突维持风遁buff.\n与很多其他功能相冲突，请只使用Simple Ninja Single Target与Simple Ninja AoE，并禁用其他所有选项.", NIN.JobID, 0, "", "")]
        NinSimpleSingleTarget = 10022,

        [CustomComboInfo("Simple Ninja AoE", "将 血雨飞花 做为多目标一键连击的起始技.\n只有在生杀予夺作用下、或是拥有两层结印时才会使用忍术，以确保更多的土遁覆盖时间.", NIN.JobID, 0, "Dote-on AoE", "Uses /dote on every target.")]
        NinSimpleAoE = 10023,

        [ParentCombo(NinSimpleSingleTarget)]
        [CustomComboInfo("Include 攻其不备", "在一键连击中允许或禁用 攻其不备.", NIN.JobID, 0, "Surprise!", "It's like the Uno Reverse card of XIV!")]
        NinSimpleTrickFeature = 10024,

        [ParentCombo(NinjaAeolianEdgeCombo)]
        [CustomComboInfo("断绝/梦幻三段 Feature", "将 断绝 和 梦幻三段 加入到连击序列中.", NIN.JobID, 0, "Fever Dream", "Frothinig at the mouth")]
        NinAeolianAssassinateFeature = 10025,

        [ParentCombo(NinjaAeolianEdgeCombo)]
        [CustomComboInfo("夺取 Feature", "将 夺取 加入到攻击序列中.", NIN.JobID, 0, "Petty Theft", "Embarrassing.")]
        NinAeolianMugFeature = 10026,

        [ParentCombo(NinSimpleTrickFeature)]
        [CustomComboInfo("生杀予夺 for 水遁 Feature", "当攻其不备冷却时，允许将生杀予夺用来优先使用水遁，水遁的优先级将高于冰晶乱流之术.", NIN.JobID)]
        NinSimpleTrickKassatsuFeature = 10027,

        [ParentCombo(NinSimpleAoE)]
        [CustomComboInfo("通灵之术·大虾蟆 Feature", "当拥有足够的忍气时，将 通灵之术·大虾蟆 加入到连击序列中.", NIN.JobID)]
        NinSimpleHellfrogFeature = 10028,

        [ParentCombo(NinSimpleAoE)]
        [CustomComboInfo("Mudra Feature", "将 土遁、火遁/劫火灭却之术 加入连击序列.", NIN.JobID)]
        NinSimpleAoeMudras = 10029,

        [ParentCombo(NinSimpleAoE)]
        [CustomComboInfo("分身之术 Feature", "将 分身之术 和 残影镰鼬 加入攻击序列.", NIN.JobID)]
        NinSimpleAoeBunshin = 10030,

        #endregion
        // ====================================================================================
        #region PALADIN

        [CustomComboInfo("沥血剑 Combo", "将 沥血剑 设为沥血连.", PLD.JobID, 0, "These aren't heals... huh?", "Just take the armour off and don a robe, we all know you're green on the inside.")]
        PaladinGoringBladeCombo = 11000,

        [CustomComboInfo("王权剑 Combo", "将 王权剑/战女神之怒. 设为一整套连招的起始.\n选中下列所有子选项激活一键连招.", PLD.JobID, 0, "", "Lmao, 'Authority'... If you say so, buddy.")]
        PaladinRoyalAuthorityCombo = 11001,

        [ParentCombo(PaladinRoyalAuthorityCombo)]
        [CustomComboInfo("赎罪剑 Feature", "在激活忠义之剑的状态下，将 王权剑 变为 赎罪剑.", PLD.JobID, 1, "", "赎罪剑 for what? Picking the weakest Tank?")]
        PaladinAtonementFeature = 11002,

        [CustomComboInfo("日珥斩 Combo", "将 日珥斩 设为AOE连击的起始.", PLD.JobID, 0, "Promenade feature", "Long walks on the promenade...")]
        PaladinProminenceCombo = 11003,

        [ParentCombo(PaladinReqMainComboFeature)]
        [CustomComboInfo("圣灵 Feature", "自身附加安魂祈祷状态时，将 圣灵 插入到连击中. ", PLD.JobID, 0, "Auto-PLD", "出卡s the whole job for you.\nJust stand there and take damage, right?")]
        PaladinRequiescatFeature = 11004,

        [ParentCombo(PaladinReqMainComboFeature)]
        [CustomComboInfo("悔罪 Combo Feature", "当MP高于2000，且仅剩一层安魂祈祷档数时，将 圣灵/圣环 替换为 悔罪. \n释放 悔罪后，依次变为 信念之剑/真理之剑/英勇之剑.", PLD.JobID, 0, "Confetti Feature", "This is gonna be a nightmare to clean up.")]
        PaladinConfiteorFeature = 11005,

        [CustomComboInfo("Scornful Spirits Feature", "将 深奥之灵 替换为 厄运流转 并优先显示最快冷却的那个.", PLD.JobID, 0, "", "Two for the price of one!")]
        PaladinScornfulSpiritsFeature = 11006,

        [ParentCombo(PaladinRoyalAuthorityCombo)]
        [CustomComboInfo("沥血剑 Feature", "在合适的时机插入沥血连.", PLD.JobID, 0, "", "")]
        PaladinRoyalGoringOption = 11007,

        [CustomComboInfo("Standalone 圣灵 Feature", "自身附加安魂祈祷状态时，将 圣灵 设为圣灵-悔罪-信念-真理-英勇连.", PLD.JobID, 0, "", "It's Christmas already?")]
        PaladinStandaloneHolySpiritFeature = 11008,

        [CustomComboInfo("Standalone 圣环 Feature", "自身附加安魂祈祷状态时，将 圣灵 设为圣环-悔罪-信念-真理-英勇连.", PLD.JobID, 0, "", "This is MY circle.")]
        PaladinStandaloneHolyCircleFeature = 11009,

        [ParentCombo(PaladinRoyalAuthorityCombo)]
        [CustomComboInfo("调停 Feature", "当 调停 可用时，将其插入到连击中.", PLD.JobID, 4, "", "It looks like a gap-closer. It smells like a gap-closer...")]
        PaladinInterveneFeature = 11010,

        [ParentCombo(PaladinRoyalAuthorityCombo)]
        [ConflictingCombos(PaladinRangedUptimeFeature2)]
        [CustomComboInfo("投盾 Uptime Feature", "超出攻击距离时，将 投盾 插入到连击中.", PLD.JobID, 4, "", "Don't throw your shield, you're not Captain America.\nJust get close!")]
        PaladinRangedUptimeFeature = 11012,

        [ParentCombo(PaladinFightOrFlightMainComboFeature)]
        [ConflictingCombos(PaladinFightOrFlightFeature)]
        [CustomComboInfo("战逃反应", "Adds FoF onto the main combo (Testing).", PLD.JobID, 0, "", "What is this, P3S?")]
        PaladinFightOrFlightMainComboFeature = 11013,

        [ParentCombo(PaladinRoyalAuthorityCombo)]
        [CustomComboInfo("安魂祈祷 Feature", "将 安魂祈祷 插入连击中 (测试中).", PLD.JobID, 2, "", "Just defend 4hed")]
        PaladinReqMainComboFeature = 11014,

        [ParentCombo(PaladinRoyalAuthorityCombo)]
        [ConflictingCombos(PaladinRangedUptimeFeature)]
        [CustomComboInfo("圣灵 Uptime Feature", "超出攻击距离时，将 王权剑/战女神之怒 连击替换为 圣灵.", PLD.JobID, 5, "(Un)神圣 Halone", "Who is Halone and why are they so angry?")]
        PaladinRangedUptimeFeature2 = 11016,

        [ParentCombo(PaladinRoyalAuthorityCombo)]
        [CustomComboInfo("战逃反应 Feature", "将 战逃反应 插入到连击中.", PLD.JobID, 2, "", "This feature hurts my brain. Yours too, no doubt")]
        PaladinFightOrFlightFeature = 11017,

        [CustomComboInfo("Interrupt Feature", "当目标可以被打断施法或眩晕且技能处于冷却时，将 盾牌猛击 替换为插言或下踢.", PLD.JobID, 0, "Lower blow", "Blow, but low.")]
        PaladinInterruptFeature = 11019,

        [ParentCombo(PaladinProminenceCombo)]
        [CustomComboInfo("圣环 Feature", "自身附加安魂祈祷状态时，将 圣环 插入到AOE连击中.", PLD.JobID, 1, "", "")]
        PaladinHolyCircleFeature = 11020,

        [ParentCombo(PaladinHolyCircleFeature)]
        [CustomComboInfo("AoE 悔罪 Feature", "Replaces AoE combo with Confiteor when 安魂祈祷 is active and appropiate.", PLD.JobID, 2, "", "")]
        PaladinAoEConfiteorFeature = 11021,

        [ParentCombo(PaladinHolyCircleFeature)]
        [CustomComboInfo("AoE 安魂祈祷 Feature", "当 安魂祈祷 处于冷却时，将其插入到AOE连击中.\n可能并不是最优解.", PLD.JobID, 0, "", "")]
        PaladinReqAoEComboFeature = 11022,

        [ParentCombo(PaladinRoyalAuthorityCombo)]
        [CustomComboInfo("Expiacion and 厄运流转 Feature", "在连击窗口期插入 偿赎剑 和 厄运流转.", PLD.JobID, 0, "", "")]
        PaladinExpiacionScornFeature = 11023,

        [ParentCombo(PaladinProminenceCombo)]
        [CustomComboInfo("AOE Expiacion / 厄运流转 Feature", "在连击窗口期插入 偿赎剑 和 厄运流转.", PLD.JobID, 0, "", "")]
        PaladinAoEExpiacionScornFeature = 11024,

        [CustomComboInfo("血仇保护机制", "当目标已被赋予血仇效果时，将 血仇 替换为 飞石", PLD.JobID)]
        PaladinReprisalProtection = 11030,

        #endregion
        // ====================================================================================
        #region REAPER

        // Single Target Combo Section
        [CustomComboInfo("切割连击", "将切割替换为切割连击。\n禁用此项将停用次级选项功能。", RPR.JobID, 0, "One, Two, Three", "It's a slicer's life for me~")]
        ReaperSliceCombo = 12000,

        [ParentCombo(ReaperSliceCombo)]
        [CustomComboInfo("灵魂切割设置", "当魂衣值低于50点，并且目标拥有死亡烙印Debuff时，使用灵魂切割替换切割。", RPR.JobID, 0, "", "FETCH ME THEIR SOULS...")]
        ReaperSoulSliceFeature = 12001,

        [ParentCombo(ReaperSliceCombo)]
        [CustomComboInfo("死亡之影设置", "当目标没有或者死亡烙印Debuff即将到期时，使用死亡之影替换切割。", RPR.JobID, 0, "DoB - Damage over Brain", "Already using all of your brain on positionals? Look no further!")]
        ReaperShadowOfDeathFeature = 12002,

        [ParentCombo(ReaperSliceCombo)]
        [CustomComboInfo("下踢设置", "当目标的施法可以被打断时，使用下踢替换切割。（以眩晕代替打断）", RPR.JobID, 0, "ZAP", "The male was too stunned to speak")]
        ReaperStunOption = 12003,

        [ParentCombo(ReaperSliceCombo)]
        [CustomComboInfo("回复设置", "在血量低于65%和40%时，使用浴血和内丹替换切割。", RPR.JobID, 0, "Pretend 出卡 Heals", "Look mom, I'm a White Mage!")]
        ReaperComboHealsOption = 12004,

        [ParentCombo(ReaperSliceCombo)]
        [CustomComboInfo("距离设置", "当超出近战范围时，使用勾刃（当可用时，使用收获月）替换切割。不会替换掉团契。", RPR.JobID, 0, "Stretch Armstrong", "Can't quite reach? Here.")]
        ReaperRangedFillerOption = 12005,


        // AoE Combo Section
        [CustomComboInfo("旋转钐割连击", "将旋转钐割替换为旋转钐割连击。\n禁用此项将停用次级选项功能。", RPR.JobID, 0, "One, Two, Th-", "Oh. It's barely a combo!")]
        ReaperScytheCombo = 12010,

        [ParentCombo(ReaperScytheCombo)]
        [CustomComboInfo("灵魂钐割设置", "当魂衣值低于50点，并且目标拥有死亡烙印Debuff时，使用灵魂钐割替换旋转钐割。", RPR.JobID, 0, "", "I've made the CoD Zombies joke too many times, but I'm too tired to think of anything else.\nSorry!")]
        ReaperSoulScytheFeature = 12011,

        [ParentCombo(ReaperScytheCombo)]
        [CustomComboInfo("死亡之涡设置", "当目标没有或者死亡烙印Debuff即将到期时，使用死亡之涡替换旋转钐割。", RPR.JobID, 0, "DoB - Damage over Brains", "Already using all of your brain-cells on positionals? Look no further!")]
        ReaperWhorlOfDeathFeature = 12012,


        // Gibbet, Gallows, Guillotine Combo Section
        [CustomComboInfo("[妖异技能特性]", "包括绞决, 缢杀和断首的设置。\n禁用此项 不会 停用次级选项功能。", RPR.JobID, 0, "Gubbins, Gibberish and Globular", "They all do the same thing, really.")]
        ReaperMenuUnveiledFeatures = 12020,

        [ParentCombo(ReaperMenuUnveiledFeatures)]
        [ConflictingCombos(ReaperGibbetGallowsInverseFeature)]
        [CustomComboInfo("绞决/缢杀特性", "在妖异之镰状态下使用绞决和缢杀分别替换切割与死亡之影。", RPR.JobID, 0, "Drown in FX!", "Now with even less buttons!")]
        ReaperGibbetGallowsFeature = 12021,

        [ParentCombo(ReaperGibbetGallowsFeature)]
        [CustomComboInfo("绞决/缢杀一键整合", "使用正确的绞决/缢杀替换切割，死亡之影不会被替换。", RPR.JobID, 0, "Gubbins/Gibberish One-Button Option", "Positionals were SO last patch, anyway.")]
        ReaperGibbetGallowsOption = 12022,

        [ParentCombo(ReaperMenuUnveiledFeatures)]
        [ConflictingCombos(ReaperGibbetGallowsFeature)]
        [CustomComboInfo("缢杀/绞决 (反转) 特性 - 损坏中 (目前与上一选项效果相同)", "在妖异之镰状态下使用绞决和缢杀分别替换死亡之影与切割。\n(与绞决/缢杀一键整合相比，会交换绞决和缢杀的位置)", RPR.JobID, 0, "BoRkEd", "Don't use this bruh, you KNOW it's broken.\nShit the bed!")]
        ReaperGibbetGallowsInverseFeature = 12023,

        [ParentCombo(ReaperMenuUnveiledFeatures)]
        [CustomComboInfo("断首特性", "在妖异之镰状态下使用断首替换旋转钐割。", RPR.JobID, 0, "", "As if this job wasn't the easiest Melee already. You're welcome, little sloth.")]
        ReaperGuillotineFeature = 12024,


        // Blood Stalk, Grim Swathe, Gluttony Combo Section
        [CustomComboInfo("[妖异之镰特性]", "包括束缚挥割，隐匿挥割与暴食的设置。\n禁用此项 不会 停用次级选项功能。", RPR.JobID, 0, "Grass Farmer Features", "oGCDs? You betcha")]
        ReaperMenuSoulReaverFeatures = 12030,

        [ParentCombo(ReaperMenuSoulReaverFeatures)]
        [ConflictingCombos(ReaperBloodStalkComboFeature, ReaperBloodStalkAlternateComboOption, ReaperGrimSwatheComboFeature)]
        [CustomComboInfo("隐匿挥割/束缚挥割特性", "当暴食不在冷却时，使用暴食替换隐匿挥割/束缚挥割。", RPR.JobID, 0, "Buttony", "It's like the normal buttons, but better! Double the fun!")]
        ReaperBloodSwatheFeature = 12031,

        [ParentCombo(ReaperMenuSoulReaverFeatures)]
        [ConflictingCombos(ReaperBloodSwatheFeature, ReaperBloodStalkAlternateComboOption)]
        [CustomComboInfo("隐匿挥割多重连击", "当暴食不在冷却时，使用暴食替换隐匿挥割，并且在夜游魂状态下使用附体连击替换隐匿挥割。", RPR.JobID, 0, "", "Play the job properly u stinker!")]
        ReaperBloodStalkComboFeature = 12032,

        [ParentCombo(ReaperMenuSoulReaverFeatures)]
        [ConflictingCombos(ReaperBloodSwatheFeature, ReaperBloodStalkComboFeature)]
        [CustomComboInfo("隐匿挥割多重连击优化 - 相似但是更好 (?)", "当暴食不在冷却时，使用暴食替换隐匿挥割，并且将绞决与缢杀整合到隐匿挥割。当然，也会在夜游魂状态下使用附体连击替换隐匿挥割。\n[需要更进一步的检查]", RPR.JobID, 0, "", "You heard me the first time!")]
        ReaperBloodStalkAlternateComboOption = 12033,

        [ParentCombo(ReaperMenuSoulReaverFeatures)]
        [ConflictingCombos(ReaperBloodSwatheFeature)]
        [CustomComboInfo("束缚挥割多重连击优化", "当暴食不在冷却时，使用暴食替换束缚挥割，并且在夜游魂状态下使用附体连击替换隐匿挥割。", RPR.JobID, 0, "", "I SAID - Play the job u stinker!!!!")]
        ReaperGrimSwatheComboFeature = 12034,


        // Enshroud Combo Section
        [CustomComboInfo("[夜游魂衣特性]", "夜游魂衣爆发设置\n禁用此项 不会 停用次级选项功能。", RPR.JobID, 0, "Edgelord mode", "Devil May Cry reboot when?")]
        ReaperMenuEnshroudFeatures = 12040,

        [ParentCombo(ReaperMenuEnshroudFeatures)]
        [CustomComboInfo("收割特性", "当你拥有两层虚无魂状态时，使用夜游魂切割替换缢杀/绞决，同时使用夜游魂钐割替换断首。", RPR.JobID, 0, "One-button farming burst", "Who is Lemure and what do they want?")]
        ReaperLemureFeature = 12041,

        [ParentCombo(ReaperMenuEnshroudFeatures)]
        [CustomComboInfo("团契特性", "当只剩下一层夜游魂时，使用团契替换缢杀/绞决与断首。", RPR.JobID, 0, "", "They say strong communio is the key to a healthy relationship")]
        ReaperComboCommunioFeature = 12042,

        [ParentCombo(ReaperMenuEnshroudFeatures)]
        // [ConflictingCombos(ReaperEnshroudComboFeature)]
        [CustomComboInfo("夜游魂衣团契特性", "当拥有夜游魂状态时，使用团契替换夜游魂衣。", RPR.JobID, 0, "", "Go on, press it as soon as you enter 夜游魂衣.\nI dare you. Dingus.")]
        ReaperEnshroudCommunioFeature = 12043,

        // [ParentCombo(ReaperMenuEnshroudFeatures)]
        // [ConflictingCombos(ReaperEnshroudCommunioFeature)]
        // [CustomComboInfo("夜游魂衣 One-Button Single Target Combo - DOESN'T EXIST YET", "Turns 夜游魂衣 into Void/Cross Reaping, weaves Lemure 切割 and finishes with Communio.", RPR.JobID)]
        // ReaperEnshroudComboFeature = 12044,


        // Miscellaneous Combo Section
        [CustomComboInfo("[额外特性]", "各种其它设置。\n禁用此项 不会 停用次级选项功能。", RPR.JobID, 0, "M O R E", "More? Haven't we made this job easy enough already?")]
        ReaperMenuExtraFeatures = 12050,

        [ParentCombo(ReaperMenuExtraFeatures)]
        [CustomComboInfo("神秘环大丰收特性。", "当你拥有死亡祭品层数时，使用大丰收替换神秘环。", RPR.JobID, 0, "Farming Simulator 2022", "You might as well buy a tractor at this point")]
        ReaperHarvestFeature = 12051,

        [ParentCombo(ReaperMenuExtraFeatures)]
        [CustomComboInfo("回退特性", "当你拥有回退预备状态时，使用回退同时替换地狱入境与地狱出境，而不是和你使用的位移技能相反的那个。", RPR.JobID, 0, "You're a DRG now, son", "GO WHENCE YOU CAME")]
        ReaperRegressFeature = 12052,

        [ParentCombo(ReaperMenuExtraFeatures)]
        [CustomComboInfo("勾刃播魂种特性", "在战斗外使用播魂种替换勾刃。当你拥有播魂种BUFF时，不进行替换。", RPR.JobID, 0, "", "Gotta have something to do before the pull, right?")]
        ReaperHarpeSoulsowFeature = 12053,

        [ParentCombo(ReaperMenuExtraFeatures)]
        [CustomComboInfo("勾刃收获月特性", "当你拥有播魂种BUFF时，使用收获月替换勾刃。", RPR.JobID, 0, "Dumb reaper be dumb", "Good luck finding the best place to use this kek")]
        ReaperHarpeHarvestMoonFeature = 12054,

        [ParentCombo(ReaperHarpeHarvestMoonFeature)]
        [CustomComboInfo("勾刃效果提高设置", "防止收获月在拥有勾刃效果提高时替换勾刃。\n这个选项也会影响切割连击设置中的距离设置。", RPR.JobID, 0, "", "Look at me, all SMN-like and that")]
        ReaperHarpeHarvestMoonEnhancedOption = 12055,

        [ParentCombo(ReaperHarpeHarvestMoonFeature)]
        [CustomComboInfo("战斗外勾刃设置", "防止收获月在战斗外替换勾刃。\n这个选项也会影响切割连击设置中的距离设置。", RPR.JobID, 0, "", "OoOoH uNaSpEcTeD dAmAgE")]
        ReaperHarpeHarvestMoonCombatOption = 12056,

        #endregion
        // ====================================================================================
        #region RED MAGE

        [ConflictingCombos(RedMageSmartcastAoECombo)]
        [CustomComboInfo("赤魔AOE连击", "若有连续咏唱或即刻咏唱状态,则替换赤烈风或赤震雷为冲击。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageAoECombo = 13000,

        [CustomComboInfo("魔连攻连击", "当黑白魔元满足条件时，替换连攻为魔连攻并可一键释放近战三连击。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageMeleeCombo = 13001,

        [CustomComboInfo("魔连攻连击特性", "根据黑白魔元平衡自动将赤神圣（核爆）与焦热接在近战三连击之后。 \n需先启用魔连攻连击。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageMeleeComboPlus = 13002,

        [ConflictingCombos(RedMageSmartSingleTargetCombo, RedMageJoltVerprocCombo)]
        [CustomComboInfo("赤飞石（火炎）替换摇荡", "没有任何触发时，替换赤飞石（火炎）为摇荡。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageVerprocCombo = 13003,

        [ConflictingCombos(RedMageSmartSingleTargetCombo)]
        [CustomComboInfo("赤飞石（火炎）替换摇荡特性1", "若有连续咏唱或即刻咏唱状态， 则将赤飞石（火炎）自动替换为赤疾风（闪雷）。\n需先启用赤飞石（火炎）替换震荡。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageVerprocComboPlus = 13004,

        [ConflictingCombos(RedMageSmartSingleTargetCombo)]
        [CustomComboInfo("赤飞石（火炎）替换摇荡特性2", "脱战后自动替换赤飞石（火炎）为赤疾风（闪雷）。\nR需先启用 赤飞石（火炎）替换震荡特性1。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageVerprocOpenerFeature = 13005,

        [CustomComboInfo("决断设置", "若决断可释放，则替换赤飞石（火炎）为决断。 ", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedmageResolutionFinisher = 13006,

        [CustomComboInfo("决断连击特性", "魔连攻连击后自动替换赤飞石（火炎）为赤神圣（核爆）、焦热、决断。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedmageResolutionFinisherMelee = 13007,

        [ConflictingCombos(RedMageAoECombo)]
        [CustomComboInfo("赤魔智能AOE连击", "释放赤震雷后根据黑白魔元平衡自动将赤震雷替换为赤烈风/冲击。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageSmartcastAoECombo = 13008,

        [ConflictingCombos(RedMageVerprocComboPlus, RedMageVerprocOpenerFeature, RedMageVerprocCombo, RedMageJoltVerprocCombo)]
        [CustomComboInfo("赤魔智能单体魔法循环", "以赤飞石（火炎）技能为基础的单体魔法循环（只包含震荡、赤飞石/火炎）、赤疾风/闪雷）。由PrincessRTFM制作", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageSmartSingleTargetCombo = 13009,

        [CustomComboInfo("飞刺、六分反击自动替换", "释放飞刺（六分反击）后会自动将其替换为六分反击（飞刺），且CD先结束的技能会自动替换仍处于CD的技能。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageOgcdCombo = 13010,

        [CustomComboInfo("赤魔智能单体起手", "以预读赤闪雷作为起手并尽量保持魔元平衡。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageVerprocOpenerSmartCastFeature = 13011,

        [ParentCombo(RedMageSmartcastAoECombo)]
        [CustomComboInfo("魔划圆斩连击整合", "使用三次魔划圆斩后会根据黑白魔元自动替换其为赤神圣或赤核爆，然后接焦热/决断。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageMeleeAoECombo = 13012,

        [CustomComboInfo("交剑设置", "当可用时，在所有近战三连中自动插入交剑。(仅测试用!)", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageEngagementFeature = 13013,

        [CustomComboInfo("赤魔单体一键输出", "一键智能整合所有单体输出技能到摇荡（回刺、交击斩、连攻、赤火炎/疾风、震荡均可），并在黑白魔元满足条件且在近战范围时自动变为魔六连爆发。(高延迟下待优化)", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        SimpleRedMage = 13014,

        [CustomComboInfo("赤魔AOE一键输出", "一键智能整合所有AOE输出技能到赤震雷（烈风）。(高延迟下待优化)", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        SimpleRedMageAoE = 13015,

        [CustomComboInfo("短兵相接、移转设置", "处于近战距离时自动替换短兵相接为移转。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageMovementFeature = 13016,

        [ParentCombo(RedMageOgcdCombo)]
        [CustomComboInfo("飞刺、六分反击自动插入", "当可用时，在所有智能连击中自动插入飞刺与六分反击。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageOgcdComboOnCombos = 13017,

        [ParentCombo(SimpleRedMage)]
        [CustomComboInfo("赤飞石（火炎）触发设置", "赤飞石（火炎）未被触发时，自动插入促进和即刻咏唱（优先插入促进）进行触发。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        SimpleRedMageFishing = 13018,

        [ParentCombo(SimpleRedMageFishing)]
        [CustomComboInfo("赤飞石（火炎）触发追加设置", "赤飞石（火炎）未被触发时，仅自动插入促进强制触发。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        SimpleRedMageAccelOnlyFishing = 13019,

        [ParentCombo(SimpleRedMage)]
        [ConflictingCombos(RedMageVerprocOpenerSmartCastFeature)]
        [CustomComboInfo("简单起手技能设置", "可以预读赤暴雷开展双触发起手。 [仅在90级时有效]", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        SimpleRedMageOpener = 13020,

        [ConflictingCombos(RedMageSmartSingleTargetCombo, RedMageVerprocCombo)]
        [CustomComboInfo("智能摇荡替换触发", "当有触发时自动替换摇荡为赤飞石（火炎），若会引起魔元失衡则不会替换。", RDM.JobID, 0, "/ac 即刻咏唱 然后 /ac 赤复活", "要什么输出，拉人才是你应该做的！")]
        RedMageJoltVerprocCombo = 13021,

        [CustomComboInfo("自定义醒梦使用", "当MP低于设定数值时自动替换赤疾风、赤闪雷和冲击为醒梦。", RDM.JobID, 0, "醒梦我也要自动插。", "emmm......你他娘的还真是个天才。")]
        RedMageLucidOnJolt = 13022,

        [CustomComboInfo("即刻赤复活", "使用即刻咏唱后替换即刻咏唱为赤复活。", RDM.JobID, 0, "Swifty 赤复活", "You're panicing right now, aren't you?")]
        RedMageSwiftVerraise = 13023,

        #endregion
        // ====================================================================================
        #region SAGE

        [CustomComboInfo("奶伴设置", "当未使用心关或拯救处于冷却状态时，替换拯救为心关。", SGE.JobID, 0, "Spoopy into Kpoopy", "Don't forget your danc- uh, heal partner!")]
        SageKardiaFeature = 14000,

        [CustomComboInfo("根素设置###SGENormal", "当蛇胆为0时，替换白牛清汁/灵橡清汁/寄生清汁/坚角清汁为根素。", SGE.JobID, 0, "Rhizomatato", "Can't quite manage that gauge? Neither can we.")]
        SageRhizomataFeature = 14001,

        [CustomComboInfo("单体治疗设置", "当白牛清汁处于冷却状态时，替换白牛清汁为灵橡清汁。", SGE.JobID, 0, "This for that", "They do the same thing, really. If you close your eyes.")]
        SageTauroDruoFeature = 14002,

        [CustomComboInfo("发炎设置", "单独启用此选项无任何效果，请根据需要开启相应子选项。", SGE.JobID, 0, "", "发炎balls.")]
        SagePhlegmaFeature = 14031,

            [ParentCombo(SagePhlegmaFeature)]
            [CustomComboInfo("发炎替换选项1", "当发炎或蛇刺消耗完毕时，替换发炎为箭毒。\n此选项优先级高于选项2。", SGE.JobID, 0, "", "Changes 发炎 to 箭毒, purely because the name is awful.")]
            SagePhlegmaToxikonFeature = 14003,

            [ParentCombo(SagePhlegmaFeature)]
            [CustomComboInfo("发炎替换选项2", "当发炎消耗完毕时，替换发炎为失衡。", SGE.JobID, 0, "", "Again, 发炎 is the worst skill name in the game. GET RID!")]
            SagePhlegmaDyskrasiaFeature = 14004,

        [CustomComboInfo("输出设置", "均衡注药整合为一键。", SGE.JobID, 0, "", "Oh look, you're basically WHM now!")]
        SageDPSFeature = 14005,

            [ParentCombo(SageDPSFeature)]
            [CustomComboInfo("均衡注药设置", "自定义设置用于自动判断是否需要使用dot的目标HP。", SGE.JobID, 0, "", "NERD")]
            SageDPSFeatureAdvTest = 14009,

            [ParentCombo(SageDPSFeature)]
            [CustomComboInfo("醒梦", "当mp低于设定值时自动插入醒梦。", SGE.JobID, 0, "Muh piety", "Never run out of steam!")]
            SageLucidFeature = 14006,

        [ConflictingCombos(SageAlternateEgeiroFeature)]
        [CustomComboInfo("即刻复苏1", "使用即刻咏唱后，替换即刻咏唱为复苏。", SGE.JobID, 0, "Swiftcast to Swiftcast", "GET BACK TO DOING DAMAGE")]
        SageEgeiroFeature = 14007,

        [ConflictingCombos(SageEgeiroFeature)]
        [CustomComboInfo("即刻复苏2", "当即刻咏唱可用时，替换复苏为即刻咏唱。", SGE.JobID, 0, "复活 to 复活", "Swaps your raise with WHM's raise.\nDoesn't work any more. You're welcome")]
        SageAlternateEgeiroFeature = 14008,

        [ConflictingCombos(SageRhizomataFeature, SageTauroDruoFeature)]
        [CustomComboInfo("单体治疗技能设置", "通过改变诊断技能。必须选中队友或自己以下设置才能生效。", SGE.JobID, 0)]
        SageSingleTargetHealFeature = 14011,

        [ConflictingCombos(SageRhizomataFeature, SageTauroDruoFeature)]
        [CustomComboInfo("群体治疗技能设置", "通过改变预后技能。自定义群体治疗技能。", SGE.JobID, 0)]
        SageAoEHealFeature = 14012,

        [ParentCombo(SageSingleTargetHealFeature)]
        [CustomComboInfo("心关", "如果心关从未使用过，则对选中目标使用心关。", SGE.JobID, 0)]
        AutoApplyKardia = 14013,

        [ParentCombo(SageSingleTargetHealFeature)]
        [CustomComboInfo("均衡诊断设置", "当所选目标没有盾值时，替换诊断为均衡诊断。", SGE.JobID, 0)]
        CustomEukrasianDiagnosisFeature = 14014,

        [ParentCombo(SageSingleTargetHealFeature)]
        [CustomComboInfo("自定义拯救设置", "当所选目标HP百分比在设定值以上时，对其使用拯救。", SGE.JobID, 0)]
        CustomSoteriaFeature = 14015,
        
        [ParentCombo(SageSingleTargetHealFeature)]
        [CustomComboInfo("自定义活化设置", "当所选目标HP百分比在设定值以上时，对其使用活化。", SGE.JobID, 0)]
        CustomZoeFeature = 14016,

        [ParentCombo(SageSingleTargetHealFeature)]
        [CustomComboInfo("自定义消化设置", "当所选目标存在盾值且HP百分比在设定值以上时，激活消化。", SGE.JobID, 0)]
        CustomPepsisFeature = 14017,

        [ParentCombo(SageSingleTargetHealFeature)]
        [CustomComboInfo("自定义白牛清汁设置", "当所选目标HP百分比在设定值以上时，插入白牛清汁。", SGE.JobID, 0)]
        CustomTaurocholeFeature = 14018,

        [ParentCombo(SageSingleTargetHealFeature)]
        [CustomComboInfo("自定义输血设置", "当所选目标HP百分比在设定值以上时，插入输血。", SGE.JobID, 0)]
        CustomHaimaFeature = 14019,

        [ParentCombo(SageSingleTargetHealFeature)]
        [CustomComboInfo("自定义根素设置###SGEST", "当蛇胆数为0时，插入根素。###SGEST", SGE.JobID, 0)]
        RhizomataFeature = 14020,
        
        [ParentCombo(SageSingleTargetHealFeature)]
        [CustomComboInfo("自定义混合设置", "当所选目标HP百分比在设定值以上时，对其使用混合。", SGE.JobID, 0)]
        CustomKrasisFeature = 14021,

        [ParentCombo(SageAoEHealFeature)]
        [CustomComboInfo("自生设置", "自动插入自生。", SGE.JobID, 0)]
        PhysisFeature = 14022,

        [ParentCombo(SageAoEHealFeature)]
        [CustomComboInfo("均衡预后设置", "当没有盾值时，替换预后为均衡预后。", SGE.JobID, 0)]
        EukrasianPrognosisFeature = 14023,

        [ParentCombo(SageAoEHealFeature)]
        [CustomComboInfo("整体论设置", "自动插入整体论。", SGE.JobID, 0)]
        HolosFeature = 14024,

        [ParentCombo(SageAoEHealFeature)]
        [CustomComboInfo("泛输血设置", "自动插入泛输血。", SGE.JobID, 0)]
        PanhaimaFeature = 14025,

        [ParentCombo(SageAoEHealFeature)]
        [CustomComboInfo("消化设置", "当盾值存在时激活消化。", SGE.JobID, 0)]
        PepsisFeature = 14026,

        [ParentCombo(SageAoEHealFeature)]
        [CustomComboInfo("寄生清汁设置", "自动插入寄生清汁。", SGE.JobID, 0)]
        IxocholeFeature = 14027,

        [ParentCombo(SageAoEHealFeature)]
        [CustomComboInfo("坚角清汁设置", "自动插入坚角清汁。", SGE.JobID, 0)]
        KeracholeFeature = 14028,

        [ParentCombo(SageAoEHealFeature)]
        [CustomComboInfo("根素设置###SGEAOE", "当蛇胆数为0时自动插入根素。###SGEAOE", SGE.JobID, 0)]
        RhizomataFeatureAoE = 14029,

        [ParentCombo(SageSingleTargetHealFeature)]
        [CustomComboInfo("灵橡清汁设置", "当所选目标HP百分比在设定值以上时，自动插入灵橡清汁。", SGE.JobID, 0)]
        CustomDruocholeFeature = 14030,
                
        #endregion
        // ====================================================================================
        #region SAMURAI

        [ConflictingCombos(SamuraiSimpleSamuraiFeature)]
        [CustomComboInfo("雪风连", "替换雪风为相应连击。", SAM.JobID, 0, "Yakuza Combo", "Gang affiliation? Surely not.")]
        SamuraiYukikazeCombo = 15000,

        [ConflictingCombos(SamuraiSimpleSamuraiFeature)]
        [CustomComboInfo("月光连", "替换月光为相应连击。", SAM.JobID, 0, "Geico Combo", "Fifteen minutes could save you 15% or more on car insurance!")]
        SamuraiGekkoCombo = 15001,

        [ConflictingCombos(SamuraiSimpleSamuraiFeature)]
        [CustomComboInfo("花车连", "替换花车为相应连击。", SAM.JobID, 0, "Cashman Combo", "Dolla dolla bill, y'all")]
        SamuraiKashaCombo = 15002,

        [CustomComboInfo("满月连", "替换满月为相应连击。", SAM.JobID, 0, "Mangetout Combo", "EAT IT ALL!")]
        SamuraiMangetsuCombo = 15003,

        [CustomComboInfo("樱花连", "替换樱花为相应连击。", SAM.JobID, 0, "Okeh Combo", "Okeh")]
        SamuraiOkaCombo = 15004,

        [CustomComboInfo("明镜止水", "根据自身buff需要将明镜止水替换为刃风/士风。", SAM.JobID, 0, "Jumpup/Sitdown", "Work those glutes.")]
        SamuraiJinpuShifuFeature = 15005,

        [ConflictingCombos(SamuraiIaijutsuTsubameGaeshiFeature)]
        [CustomComboInfo("居合术 替换 燕回返", "当没有任何闪时，替换燕回返为居合术。", SAM.JobID, 0, "", "You don't know the difference between this one and that one?")]
        SamuraiTsubameGaeshiIaijutsuFeature = 15006,

        [ConflictingCombos(SamuraiIaijutsuShohaFeature)]
        [CustomComboInfo("照破 替换 燕回返 ", "当剑压积累到3档时，替换燕回返为照破。", SAM.JobID, 0, "", "Don't worry, neither do we.")]
        SamuraiTsubameGaeshiShohaFeature = 15007,

        [ConflictingCombos(SamuraiTsubameGaeshiIaijutsuFeature)]
        [CustomComboInfo("燕回返 替换 居合术", "当没有任何闪时，替换居合术为燕回返。", SAM.JobID, 0, "", "Wait, there's more?")]
        SamuraiIaijutsuTsubameGaeshiFeature = 15008,

        [ConflictingCombos(SamuraiTsubameGaeshiShohaFeature)]
        [CustomComboInfo("照破 替换 居合术", "当剑压积累到3档时，替换居合术为照破。", SAM.JobID, 0, "", "You WHAT?!")]
        SamuraiIaijutsuShohaFeature = 15009,

        [CustomComboInfo("必杀剑·闪影 替换 必杀剑·震天", "当必杀剑·闪影冷却结束后，替换必杀剑·震天为必杀剑·闪影。", SAM.JobID, 0, "", "Kicks you in the shins if Senei is on cooldown")]
        SamuraiSeneiFeature = 15010,

        [CustomComboInfo("照破 替换 必杀剑·震天", "当剑压积累到3档时，替换必杀剑·震天为照破。", SAM.JobID, 0, "", "Kicks you in the shins if 照破 is on cooldown")]
        SamuraiShohaFeature = 15011,

        [CustomComboInfo("必杀剑·红莲 替换 必杀剑·九天", "当必杀剑·红莲冷却结束后，替换必杀剑·九天为必杀剑·红莲。", SAM.JobID, 0, "", "Hey Kyutie!")]
        SamuraiGurenFeature = 15012,

        [CustomComboInfo("无名照破 替换 必杀剑·九天", "当剑压积累到3档时，替换必杀剑·九天为无名照破。", SAM.JobID, 0, "", "Hey Kyutie 2, Electric Boogaloo!")]
        SamuraiShoha2Feature = 15013,

        [CustomComboInfo("意气冲天 奥义斩浪 连击特性", "当奥义斩浪可用时，替换意气冲天为奥义斩浪，随后变为回返斩浪。\n若同时剑压已积累3档，则意气冲天将变为照破。", SAM.JobID, 0, "Sticky-icky-shoten", "Wait, you guys use meditation?")]
        SamuraiIkishotenNamikiriFeature = 15014,

        [ConflictingCombos(SamuraiYukikazeCombo, SamuraiGekkoCombo, SamuraiKashaCombo)]
        [CustomComboInfo("武士一键单体连击", "以刃风整合一键单体输出循环。", SAM.JobID, 0, "Sleepy Samurai", "zzzz....wha?")]
        SamuraiSimpleSamuraiFeature = 15015,

        [CustomComboInfo("武士一键AOE连击", "以樱花整合AOE输出循环。非常感谢Stein121的帮助。", SAM.JobID, 0, "", "Hardly any buttons in the first place, but we got you. Lazy-ass.")]
        SamuraiSimpleSamuraiAoECombo = 15016,

        [CustomComboInfo("必杀剑·回天 特殊选项1", "当目标的彼岸花buff不足5s时，在打出彼岸花前插入必杀剑·回天。", SAM.JobID, 0, "", "MORE!")]
        SamuraiKaitenFeature1 = 15018,

        [CustomComboInfo("必杀剑·回天 特殊选项2", "在打出天下五剑前插入必杀剑·回天。", SAM.JobID, 0, "", "MORE!!")]
        SamuraiKaitenFeature2 = 15019,

        [CustomComboInfo("必杀剑·回天 特殊选项3", "在打出纷乱雪月花前插入必杀剑·回天。", SAM.JobID, 0, "", "MORE!!!")]
        SamuraiKaitenFeature3 = 15020,

        [CustomComboInfo("必杀剑·晓天 特性", "根据与目标的距离自动将必杀剑·晓天变为必杀剑·夜天/晓天。", SAM.JobID, 0, "Gyoza Feature", "Mm, tasty.")]
        SamuraiYatenFeature = 15021,

        [CustomComboInfo("必杀剑·回天 特殊选项4", "当奥义斩浪可用且剑气值在20以上时，在打出奥义斩浪前插入必杀剑·回天。", SAM.JobID, 0, "", "MORE!!!!")]
        SamuraiOgiNamikiriFeature = 15022,

        [ConflictingCombos(SamuraiOvercapFeature85)]
        [CustomComboInfo("单体剑气技能设置选项1", "当剑气值在75以上时，在单体一键输出循环中插入必杀剑·闪影与必杀剑·震天。", SAM.JobID, 0, "Wink emoji Overcap Feature 1", "Kinky.")]
        SamuraiOvercapFeature75 = 15023,

        [ConflictingCombos(SamuraiOvercapFeature75)]
        [CustomComboInfo("单体剑气技能设置选项2", "当剑气值在85以上时，在单体一键输出循环中插入必杀剑·闪影与必杀剑·震天。", SAM.JobID, 0, "Wink emoji Overcap Feature 2", "Even more kinky.")]
        SamuraiOvercapFeature85 = 15024,

        [ConflictingCombos(SamuraiOvercapFeatureAoe85)]
        [CustomComboInfo("AOE剑气技能设置选项1", "当剑气值在75以上时，在AOE一键输出循环中插入必杀剑·红莲与必杀剑·九天。", SAM.JobID, 0, "Wink emoji Overcap Feature 3", "Kinkier")]
        SamuraiOvercapFeatureAoe75 = 15025,

        [ConflictingCombos(SamuraiOvercapFeatureAoe75)]
        [CustomComboInfo("AOE剑气技能设置选项2", "当剑气值在85以上时，在AOE一键输出循环中插入必杀剑·红莲与必杀剑·九天。", SAM.JobID, 0, "Wink emoji Overcap Feature 4", "Kinkiest")]
        SamuraiOvercapFeatureAoe85 = 15026,

        [CustomComboInfo("意气冲天设置", "当剑气值在50以下时，在一键输出循环中插入意气冲天。", SAM.JobID, 0, "Gauge pls", "You heard me. Gauge pls")]
        SamuraiIkishotenonmaincombo = 15027,

        [ParentCombo(SamuraiIkishotenNamikiriFeature)]
        [CustomComboInfo("必杀剑·回天 强化 意气冲天", "在意气冲天变为奥义斩浪前插入必杀剑·回天。", SAM.JobID, 0, "", "MORE!!!!!")]
        SamuraiIkishotenKaitenNamikiriFeature = 15028,



        #endregion
        // ====================================================================================
        #region SCHOLAR

        [CustomComboInfo("Seraph 异想的祥光/慰藉", "炽天使同行状态下，将 异想的祥光 变为 慰藉.", SCH.JobID, 0, "", "Stupid little fairy thing")]
        ScholarSeraphConsolationFeature = 16000,

        [CustomComboInfo("ED 以太超流", "没有豆子的时候，将 能量吸收 变为 以太超流.", SCH.JobID, 0, "", "Stop trying to pretend you're a SMN. You're not fooling anyone")]
        ScholarEnergyDrainFeature = 16001,

        [ConflictingCombos(SCHAlternateRaiseFeature)]
        [CustomComboInfo("SCH 复活 Feature", "按下 即刻咏唱 后，使其变为 复生.", SCH.JobID, 0, "", "Well, at least PF wants you for something")]
        SchRaiseFeature = 16002,

        [ConflictingCombos(SchRaiseFeature)]
        [CustomComboInfo("SCH 复活 Alternate Feature", "当 即刻咏唱 处于可用状态时，将 复生 变为 即刻咏唱.", SCH.JobID, 0, "", "Well, this raise stuff and Peloton Extreme, I guess.")]
        SCHAlternateRaiseFeature = 16008,

        [CustomComboInfo("Fairy Feature", "未召唤出小仙女的状态下，技能栏上所有小仙女技能将变为 朝日召唤.", SCH.JobID, 0, "", "You're really gonna forget? Really?")]
        ScholarFairyFeature = 16004,

        [CustomComboInfo("DPS Feature", "战斗状态下，当目标身上没有debuff或即将到期时，将 极炎法/毁灭 变为 蛊毒法/毒菌.", SCH.JobID, 0, "", "Pretend something interesting is going on. DPS Routine!")]
        ScholarDPSFeature = 16005,

        [ParentCombo(ScholarDPSFeature)]
        [CustomComboInfo("DPS Feature Buff Option", "将 连环计 添加进 DPS Feature.", SCH.JobID, 0, "", "Raid buffs for everyone!")]
        ScholarDPSFeatureBuffOption = 16006,

        [ParentCombo(ScholarDPSFeature)]
        [CustomComboInfo("DPS Feature Lucid Dreaming Option", "当MP低于设定值时，将醒梦添加进 DPS feature.", SCH.JobID, 0, "", "Nobody's perfect. Maybe this'll help")]
        ScholarLucidDPSFeature = 16007,

        [CustomComboInfo("SCH Extra DPS Feature", "战斗状态下，将 毁坏 变为Dot法术，级别低于38级时将不会生效.", SCH.JobID, 0, "", "People still use 毁灭 2? Shouldn't you be healing or something?")]
        SCHDPSAlternateFeature = 16003,

        #endregion
        // ====================================================================================
        #region SUMMONER

        [ConflictingCombos(SummonerMainComboFeature, SummonerRuinIVMobilityFeature)]
        [CustomComboInfo("启动单体连击(毁灭)", "启动该特性后，毁灭将会被替换为其他合适的单体连击技能", SMN.JobID, 0, "毁灭 420 Feature", "毁灭ation is come")]
        SummonerMainComboFeatureRuin1 = 16999,

        [ConflictingCombos(SummonerMainComboFeatureRuin1)]
        [CustomComboInfo("启动单体连击(毁荡)", "启动该特性后，毁荡将会被替换为其他合适的单体连击技能", SMN.JobID, 0, "毁灭 7 Feature", "毁灭ation is come... again?")]
        SummonerMainComboFeature = 17000,

        [CustomComboInfo("启动群体连击", "启动该特性后，三重灾祸将会被替换为其他合适的群体连击技能", SMN.JobID, 0, "", "Can't deal with dungeons on your own? Fear not.")]
        SummonerAOEComboFeature = 17001,

        [CustomComboInfo("单体亚灵神（龙神/不死鸟）特性", "亚灵神显现期间，星极脉冲/灵泉之炎会在恰当的时刻替换为龙神迸发/不死鸟迸发/死星核爆/苏生之炎", SMN.JobID, 0, "Demi Dingus Feature", "不知道巴哈姆特和凤凰有啥区别？\n没关系，我知道。")]
        SummonerSingleTargetDemiFeature = 17002,

        [ParentCombo(SummonerAOEComboFeature)]
        [CustomComboInfo("单体亚灵神（龙神/不死鸟）特性 AOE", "亚灵神显现期间，星极核爆/炼狱之炎会在恰当的时刻替换为龙神迸发/不死鸟迸发/死星核爆/苏生之炎", SMN.JobID, 0, "BRRRR", "Upgrade!")]
        SummonerAOEDemiFeature = 17003,

        [CustomComboInfo("宝石耀特性", "在三神召唤期间，毁灭/毁荡/三重灾祸自动替换成宝石耀。只有启用了单体或群体连击后才有效（下同）。", SMN.JobID, 0, "Eggy-bread", "No idea when you're in burst phase?\nHint: It's all the time, really")]
        SummonerEgiAttacksFeature = 17004,

        [CustomComboInfo("螺旋气流特性", "在风神召唤时，毁灭/毁荡/三重灾祸自动替换为螺旋气流", SMN.JobID, 0, "螺旋气流", "2 Fast 2 Furious")]
        SummonerGarudaUniqueFeature = 17005,

        [CustomComboInfo("深红旋风/深红强袭特性", "在火神召唤时，毁灭/毁荡/三重灾祸自动替换为深红旋风/深红强袭", SMN.JobID, 0, "Fists of Fury", "Show MNK how it's done, will ya?")]
        SummonerIfritUniqueFeature = 17006,

        [CustomComboInfo("山崩特性", "在土神召唤时，毁灭/毁荡/三重灾祸自动替换为山崩", SMN.JobID, 0, "Mountain, BUSTA", "Bring the mountain to Mohammed, as they say")]
        SummonerTitanUniqueFeature = 17007,

        [CustomComboInfo("能量吸收+溃烂爆发合并特性", "在以太超流的档数为零时，溃烂爆发自动替换为能量吸收", SMN.JobID, 0, "溃烂爆发ing", "溃烂爆发ing? Go take a shower, bro")]
        SummonerEDFesterCombo = 17008,

        [CustomComboInfo("能量抽取+痛苦核爆合并特性", "在以太超流的档数为零时，痛苦核爆自动替换为能量抽取", SMN.JobID, 0, "Old age", "I sometimes get a painflare in my middle-back, too.")]
        SummonerESPainflareCombo = 17009,


        // BONUS TWEAKS
        [CustomComboInfo("宝石兽自动召唤", "在尚未召唤宝石兽时，将毁灭自动替换为召唤宝石兽。妈妈再也不用担心我开场放不出龙神了。", SMN.JobID, 0, "Rabbit Reminder", "C'mon. You had ONE job.")]
        SummonerCarbuncleSummonFeature = 17010,

        [CustomComboInfo("毁绝特性（单体）", "当所有召唤兽都在冷却时，将毁绝加入毁灭/毁荡的连击", SMN.JobID, 0, "毁灭 -> 毁灭 -> 毁灭", "毁灭 this, ruin that. Can't you see I'm busy ruining the plugin?!")]
        SummonerRuin4ToRuin3Feature = 17011,

        [CustomComboInfo("毁绝特性（群体）", "当所有召唤兽都在冷却时，将毁绝加入三重灾祸的连击", SMN.JobID, 0, "", "More 毁灭 this, more ruin that! Now in sharing size!")]
        SummonerRuin4ToTridisasterFeature = 17012,

        [ParentCombo(SummonerEDFesterCombo)]
        [CustomComboInfo("毁绝特性（溃烂爆发/痛苦核爆）", "当以太超流的档数为零，且能量吸收/能量抽取在CD，且毁绝触发时，将溃烂爆发/痛苦核爆自动替换为毁绝", SMN.JobID, 0, "溃烂爆发ing Painflare", "Just take some Advil for that, or see the doc?")]
        SummonerFesterPainflareRuinFeature = 17013,

        [ParentCombo(SummonerEDFesterCombo)]
        [CustomComboInfo("自动溃烂爆发特性", "将能量吸收/溃烂爆发加入到星极脉冲中。\n. 在不死鸟召唤期间，将只释放能量吸收，从而保留溃烂爆发到龙神召唤期间打爆发", SMN.JobID, 0, "I don't read descriptions -", "Why is 溃烂爆发 Not working properly?")]
        SummonerLazyFesterFeature = 17014,

        [ConflictingCombos(SimpleSummonerOption2)]
        [CustomComboInfo("一键单体循环", "将不死鸟召唤和三神召唤按照土神>风神>火神的顺序在合适的时机加入到毁灭/毁荡", SMN.JobID, 0, "", "We'll play the game for you. Shush, now")]
        SimpleSummoner = 17015,

        [CustomComboInfo("一键群体循环", "将不死鸟召唤和三神召唤按照土神>风神>火神的顺序在合适的时机加入到三重灾祸", SMN.JobID, 0, "", "We'll play the game for you. Shush, now")]
        SimpleAoESummoner = 17016,

        [ParentCombo(SimpleSummoner)]
        [CustomComboInfo("灼热之光加入循环（单体）", "将灼热之光加入到一键单体循环中", SMN.JobID, 0, "My eyes!", "I can't see!")]
        BuffOnSimpleSummoner = 17017,

        [ParentCombo(SimpleAoESummoner)]
        [CustomComboInfo("灼热之光加入循环（群体）", "将灼热之光加入到一键群体循环中", SMN.JobID, 0, "Our Eyes!", "Yay, we're all legally blind!")]
        BuffOnSimpleAoESummoner = 17018,

        [CustomComboInfo("亚灵神（龙神/不死鸟）自动召唤（单体）", "仅用亚灵神（龙神/不死鸟）召唤替换毁荡（这样你依然可以自己手动选择三神的召唤顺序，但永远都不会忘记召唤亚灵神）", SMN.JobID, 0, "Chad Kroeger Demi Feature", "This is how, you remind me, of what I really am")]
        SummonerDemiSummonsFeature = 17019,

        [CustomComboInfo("亚灵神（龙神/不死鸟）自动召唤（群体）", "仅用亚灵神（龙神/不死鸟）召唤三重灾祸（这样你依然可以自己手动选择三神的召唤顺序，但永远都不会忘记召唤亚灵神）", SMN.JobID, 0, "Nickelback Demi Feature", "Oh fuck, the whole band is here! Run!")]
        SummonerDemiAoESummonsFeature = 17020,

        [ConflictingCombos(SummonerMainComboFeatureRuin1)]
        [CustomComboInfo("毁荡机动型特性", "在没有毁绝的情况下，考虑到机动性的原因，允许你用毁绝释放毁荡。毁绝图标将变为毁灭。\n毁灭的连击会被破坏，毁绝的连击可能被破坏。", SMN.JobID, 0, "", "Don't you have enough mobility as it is?\nThis isn't Ranged DPS, y'know.")]
        SummonerRuinIVMobilityFeature = 17021,

        [ConflictingCombos(SummonerSwiftcastFeatureIfrit)]
        [CustomComboInfo("即刻风神特性", "在有即刻咏唱时，螺旋气流前将会自动插入即刻咏唱", SMN.JobID, 0, "No Raising for me!", "Always slips swiftstream, sometimes")]
        SummonerSwiftcastFeatureGaruda = 17022,

        [ConflictingCombos(SummonerSwiftcastFeatureGaruda)]
        [CustomComboInfo("即刻火神特性", "在有即刻咏唱时，第二个红宝石之仪前将会自动插入即刻咏唱", SMN.JobID, 0, "No Raising for you!", "Always cancels the Ifrit phase entirely, doing nothing instead.")]
        SummonerSwiftcastFeatureIfrit = 17023,

        [ConflictingCombos(SimpleSummoner)]
        [CustomComboInfo("一键循环特性2", "其他内容与一键循环均一致，只不过召唤顺序变为风神>土神>火神。", SMN.JobID, 0, "The Egi Shuffle", "You just had to be different, didn't you?")]
        SimpleSummonerOption2 = 17024,

        [CustomComboInfo("防止毁绝浪费特性", "如果毁绝预备效果即将过期，则将毁绝的优先级放到其他技能之前（除非现在是亚灵神显现期间）", SMN.JobID, 0, "", "Waste not, want not")]
        SummonerRuin4WastePrevention = 17025,

        [CustomComboInfo("苏生之炎特性", "将苏生之炎加入到毁灭/毁荡的连击中，需要开启其他特性才能生效。", SMN.JobID, 0, "Tinderbox", "Remember making those fires in runescape? Yeah. That.")]
        SummonerRekindlePhoenix = 17026,

        #endregion
        // ====================================================================================
        #region WARRIOR

        [CustomComboInfo("绿斩连击", "将暴风斩替换为主要连击技能/Path. \n如果下列选项以及裂石飞环/地毁人亡相关选项被勾选，暴风斩将变为一键循环", WAR.JobID, 0, "", "Follow the yellow-brick road.")]
        WarriorStormsPathCombo = 18000,

        [CustomComboInfo("红斩连击", "将暴风碎替换为红斩连击", WAR.JobID, 0, "", "Ow! My fucking eye!")]
        WarriorStormsEyeCombo = 18001,

        [CustomComboInfo("超压斧连击", "将超压斧替换为AOE连击", WAR.JobID, 0, "Underpower", "Bet you wish you had damage like DRK right now, huh")]
        WarriorMythrilTempestCombo = 18002,

        [ParentCombo(WarriorStormsPathCombo)]
        [CustomComboInfo("兽魂监控", "如果兽魂快溢出了，将消耗兽魂的技能加入到绿斩连击和AOE连击", WAR.JobID, 0, "", "Taming the beast... for now.")]
        WarriorGaugeOvercapFeature = 18003,

        [ParentCombo(WarriorStormsPathCombo)]
        [CustomComboInfo("原初的解放", "在原初的解放状态下，将绿斩连击替换为裂石飞环/地毁人亡", WAR.JobID, 0, "", "释放 your deepest thoughts and feelings upon the party. They'll love it!")]
        WarriorInnerReleaseFeature = 18004,

        [CustomComboInfo("原初的勇猛", "在同步到76级以下时，将原初的勇猛替换为原初的直觉", WAR.JobID, 0, "Nasty-ass Flash", "Jeez. Keep it to yourself.")]
        WarriorNascentFlashFeature = 18005,

        [ParentCombo(WarriorStormsPathCombo)]
        [CustomComboInfo("动乱", "在有红斩BUFF时，将动乱加入到绿斩连击", WAR.JobID, 0, "", "I use this feature when I'm moving house.")]
        WarriorUpheavalMainComboFeature = 18007,

        [ParentCombo(WarriorStormsPathCombo)]
        [CustomComboInfo("蛮荒崩裂", "在蛮荒崩裂预备状态下，将裂石飞环/地毁人亡替换为蛮荒崩裂", WAR.JobID, 0, "", "Going back to our roots. Let's get Primal!")]
        WarriorPrimalRendFeature = 18008,

        [ParentCombo(WarriorMythrilTempestCombo)]
        [CustomComboInfo("群山隆起", "在有红斩BUFF时，将群山隆起加入到AOE连击", WAR.JobID, 0, "Orange-y feature", "Orange flavour. Mm.")]
        WarriorOrogenyFeature = 18009,

        [ParentCombo(WarriorStormsPathCombo)]
        [ConflictingCombos(WarriorSpenderOption)]
        [CustomComboInfo("狂魂与混沌旋风", "在有原初的混沌BUFF时，将狂魂与混沌旋风分别加入到绿斩连击和AOE连击", WAR.JobID, 0, "", "THE EYE OF THE TIGERRRRR")]
        WarriorInnerChaosOption = 18010,

        [ParentCombo(WarriorStormsPathCombo)]
        [ConflictingCombos(WarriorInnerChaosOption)]
        [CustomComboInfo("裂石飞环/地毁人亡", "在兽魂超过50时，将裂石飞环/地毁人亡（或狂魂与混沌旋风）加入到绿斩连击和AOE连击\n为了让你可以随时按战壕不会溢出兽魂\n当原初的解放CD小于30s时将会倾泻资源", WAR.JobID, 0, "", "MORE CLEAVE!")]
        WarriorSpenderOption = 18011,

        [ParentCombo(WarriorStormsPathCombo)]
        [CustomComboInfo("猛攻", "在有红斩BUFF时，将猛攻加入到绿斩连击", WAR.JobID, 0, "", "猛攻! Full Power!")]
        WarriorOnslaughtFeature = 18012,

        [CustomComboInfo("战嚎保护机制", "在原初的解放状态下，将战壕替换为裂石飞环\n在有原初的混沌BUFF时，将战壕替换为狂魂", WAR.JobID, 0, "Cleave of annoyance", "Infuriating stuff, if you ask me. Truly chaotic.")]
        WarriorInfuriateFeature = 18015,

        [ParentCombo(WarriorStormsPathCombo)]
        [CustomComboInfo("飞斧", "在如果离boss太远，将飞斧加入到绿斩连击", WAR.JobID, 0, "飞斧!", "You heard me! 飞斧! Ka-chow!")]
        WARRangedUptimeFeature = 18016,

        [CustomComboInfo("打断", "当目标可以被打断施法时，替换下踢为插言", WAR.JobID, 0, "", "That's a low blow to my bro-bro, bro.")]
        WarriorInterruptFeature = 18017,

        [CustomComboInfo("战嚎 on Fell Cleave / Decimate", "Turns Fell Cleave and Decimate into 战嚎 if at or under set rage value", WAR.JobID)]
        WarriorInfuriateFellCleave = 18018,

        [CustomComboInfo("蛮荒崩裂", "在蛮荒崩裂预备状态下，将原初的解放替换为蛮荒崩裂.", WAR.JobID)]
        WarriorPrimalRendOnInnerRelease = 18019,
        
        [ParentCombo(WarriorStormsPathCombo)]
        [CustomComboInfo("原初的解放加入绿斩循环", "在没有原初的混沌BUFF时，将原初的解放加入到绿斩连击", WAR.JobID)]
        WarriorIRonST = 18020,

        [ParentCombo(WarriorStormsPathCombo)]
        [CustomComboInfo("战壕加入绿斩循环", "在没有原初的解放BUFF且不会溢出兽魂时，将战壕加入到绿斩连击", WAR.JobID)]
        WarriorInfuriateonST = 18021,

        [CustomComboInfo("雪仇保护机制", "如果目标已有雪仇，将锁定你的雪仇", WAR.JobID)]
        WarriorReprisalProtection = 18030,

        #endregion
        // ====================================================================================
        #region WHITE MAGE

        [CustomComboInfo("安慰之心-苦难之心", "当苦难之心可用时，将安慰之心替换为苦难之心", WHM.JobID, 0, "Misery", "I'd be miserable too if this were one of my DPS options.")]
        WhiteMageSolaceMiseryFeature = 19000,

        [CustomComboInfo("狂喜之心-苦难之心", "当苦难之心可用时，将狂喜之心替换为苦难之心", WHM.JobID, 0, "Misery, but with freinds", "Let's cry together!")]
        WhiteMageRaptureMiseryFeature = 19001,

        [CustomComboInfo("治疗技能等级同步", "当等级同步在30级以下时，将救疗替换为治疗", WHM.JobID, 0, "Weenie 治疗", "Bet you forgot 治疗 1 existed for a sec, huh")]
        WhiteMageCureFeature = 19002,

        [CustomComboInfo("治疗百合状态", "当拥有治疗百合时，将救疗替换为安慰之心，医治替换为狂喜之心", WHM.JobID, 0, "Inflatus Feature", "Pumps you full of air. Boing!")]
        WhiteMageAfflatusFeature = 19003,

        [ConflictingCombos(WHMAlternativeRaise)]
        [CustomComboInfo("白魔复活特性", "将即刻咏唱替换为复活", WHM.JobID, 0, "What you're really here for", "You're the best at this. You got this.")]
        WHMRaiseFeature = 19004,

        [ConflictingCombos(WHMRaiseFeature)]
        [CustomComboInfo("白魔复活特性替代", "当即刻咏唱可用时，将复活替换为即刻咏唱。如果启用，无中生有特性也适用于此。", WHM.JobID, 0, "Look up!", "It's this, but that.")]
        WHMAlternativeRaise = 19015,

        [CustomComboInfo("DoT-闪耀特性", "当你处在战斗状态，且目标没有Dot效果或效果即将到期时，将DoT替换至闪耀上(你仍然可以预读闪耀)", WHM.JobID, 0, "Weak", "WHM DPS rotation too much?")]
        WHMDotMainComboFeature = 19005,

        [ParentCombo(WHMDotMainComboFeature)]
        [CustomComboInfo("醒梦特性", "当魔力低于设定值时，将醒梦加入输出循环中", WHM.JobID, 0, "Dream within a Dream", "Awake, yet wholly asleep")]
        WHMLucidDreamingFeature = 19006,

        [CustomComboInfo("医治特性", "当你处于医济状态下时，将医济替换为医治", WHM.JobID, 0, "Big Brain AoE Heals", "God bless us all, eh")]
        WHMMedicaFeature = 19007,

        [ParentCombo(WHMDotMainComboFeature)]
        [CustomComboInfo("神速咏唱特性", "在闪耀中插入神速咏唱", WHM.JobID, 0, "", "This would imply you're actually paying attention.")]
        WHMPresenceOfMindFeature = 19008,

        [ParentCombo(WHMDotMainComboFeature)]
        [CustomComboInfo("法令特性", "在闪耀中插入法令", WHM.JobID, 0, "", "Size 'em up, knock 'em down")]
        WHMAssizeFeature = 19009,

        [ParentCombo(WHMMedicaFeature)]
        [CustomComboInfo("苦难之心-医济", "将医济替换为苦难之心", WHM.JobID, 0, "", "Ah, back to beinig miserable.")]
        WhiteMageAfflatusMiseryMedicaFeature = 19010,

        [ParentCombo(WHMMedicaFeature)]
        [CustomComboInfo("狂喜之心-医济", "将医济替换为狂喜之心", WHM.JobID, 0, "CRapture", "The final days are upon us!")]
        WhiteMageAfflatusRaptureMedicaFeature = 19011,

        [CustomComboInfo("苦难之心特性", "将救疗替换为苦难之心", WHM.JobID, 0, "", "治疗s? Who needs 'em?")]
        WhiteMageAfflatusMiseryCure2Feature = 19012,

        [ParentCombo(WHMDotMainComboFeature)]
        [CustomComboInfo("闪耀特性中移除DoT", "移除替换为DoT的特性", WHM.JobID, 0, "I'm an idiot", "Yes, one serving of less DPS, please.")]
        WHMRemoveDotFromDPSFeature = 19013,

        [CustomComboInfo("无中生有-复活特性", "为白魔复活特性/替代 添加插入无中生有", WHM.JobID, 0, "", "I can hardly breathe as it is!")]
        WHMThinAirFeature = 19014,

        #endregion
        // ====================================================================================
        #region DOH

        // [CustomComboInfo("Placeholder", "Placeholder.", DOH.JobID)]
        // DohPlaceholder = 50001,

        #endregion
        // ====================================================================================
        #region DOL

        [CustomComboInfo("理智同兴", "当理智同兴可用时，将农夫之智或石工之理替换为理智同兴", DOL.JobID)]
        DolEurekaFeature = 51001,

        [CustomComboInfo("抛竿", "正在钓鱼时，将抛竿替换为提钩", DOL.JobID)]
        DolCastHookFeature = 51002,

        [CustomComboInfo("刺鱼", "在水下时，将抛竿替换为刺鱼", DOL.JobID)]
        DolCastGigFeature = 51003,

        [CustomComboInfo("拍击水面", "在水下时，将拍击水面替换为熟练技巧", DOL.JobID)]
        DolSurfaceTradeFeature = 51004,

        [CustomComboInfo("大鱼猎手", "在水下时，将大鱼猎手替换为嘉惠", DOL.JobID)]
        DolPrizeBountyFeature = 51005,

        [CustomComboInfo("钓组", "在水下时，将钓组替换为打捞", DOL.JobID)]
        DolSnaggingSalvageFeature = 51006,

        [CustomComboInfo("垂钓之光", "在水下时，将垂钓之光替换为电水流", DOL.JobID)]
        DolCastLightElectricCurrentFeature = 51007,

        #endregion
        // ====================================================================================
        #region PvP Combos

        [SecretCustomCombo]
        [CustomComboInfo("BurstShotFeature", "Adds 影噬箭/EmpyArrow/PitchPerfect(3stacks)/SideWinder(When Target is low hp)/ApexArrow when gauge is 100 all on one button combo.", BRDPvP.JobID)]
        BurstShotFeaturePVP = 80000,

        [SecretCustomCombo]
        [CustomComboInfo("SongsFeature", "Replaces WanderersMinnuet and Peons song all on one button in an optimal order", BRDPvP.JobID)]
        SongsFeaturePVP = 80001,

        [SecretCustomCombo]
        [CustomComboInfo("噬魂斩ComboFeature", "Adds EoS as oGCD onto main combo and 血溅 when at 50 gauge or under delirium buff.", DRKPVP.JobID)]
        SouleaterComboFeature = 80002,

        [SecretCustomCombo]
        [CustomComboInfo("StalwartSoulComboFeature", "Adds FoS as oGCD onto main combo and 寂灭 when at 50 gauge or under delirium buff.", DRKPVP.JobID)]
        StalwartSoulComboFeature = 80003,

        [SecretCustomCombo]
        [CustomComboInfo("StormsPathComboFeature", "Replaces 暴风斩 Combo with FellCleave/IC when at 50 gauge or under IR", WARPVP.JobID)]
        StormsPathComboFeature = 80004,

        [SecretCustomCombo]
        [CustomComboInfo("SteelCycloneFeature", "Replaces 钢铁旋风 Combo with Decimate/CC when at 50 gauge or under IR", WARPVP.JobID)]
        SteelCycloneFeature = 80005,

        [SecretCustomCombo]
        [CustomComboInfo("RoyalAuthorityComboFeature", "Adds 神圣Spirit To the main combo", PLDPVP.JobID)]
        RoyalAuthorityComboFeature = 80006,

        [SecretCustomCombo]
        [CustomComboInfo("日珥斩ComboFeature", "Adds 神圣Circle to the main AoE Combo", PLDPVP.JobID)]
        ProminenceComboFeature = 80007,

        [SecretCustomCombo]
        [CustomComboInfo("GnashingFangComboFeature", "Adds BowShock(When target is meleeRange) and Burststrike at 2 ammo gauge to the main combo", GNBPVP.JobID)]
        SolidBarrelComboFeature = 80008,

        [SecretCustomCombo]
        [CustomComboInfo("DemonSlaughterComboFeature", "Adds BowShock(When target is meleeRange) and 命运之环 at 2 ammo gauge to the main AoE combo", GNBPVP.JobID)]
        DemonSlaughterComboFeature = 80009,

        [SecretCustomCombo]
        [CustomComboInfo("HeatedCleanShotFeature", "Adds Gauss/Rico weave to maincombo", MCHPVP.JobID)]
        HeatedCleanShotFeature = 80010,

        [SecretCustomCombo]
        [CustomComboInfo("野火BlankFeature", "Adds Blank To 野火 if you are in melee Range", MCHPVP.JobID)]
        WildfireBlankFeature = 80011,

        [SecretCustomCombo]
        [CustomComboInfo("Infernal切割ComboFeature", "Adds Gluttony/BloodStalk/Smite/EnshroudComboRotation on Infernal切割Combo", RPRPVP.JobID)]
        InfernalSliceComboFeature = 80012,

        [SecretCustomCombo]
        [CustomComboInfo("NightmareScytheComboFeature", "Adds 暴食/GrimSwathe/Smite/EnshroudComboRotation on InfernalScytheCombo", RPRPVP.JobID)]
        NightmareScytheComboFeature = 80013,

        [SecretCustomCombo]
        [CustomComboInfo("NinjaAeolianEdgePvpCombo", "Adds Cha/断绝/Smite on AeolianEdge combo", NINPVP.JobID)]
        NinjaAeolianEdgePvpCombo = 80014,

        [SecretCustomCombo]
        [CustomComboInfo("Mnk连击PvPFeature", "Adds Axekick/Smite/TornadoKick on main combo", MNKPVP.JobID)]
        MnkBootshinePvPFeature = 80015,

        [SecretCustomCombo]
        [CustomComboInfo("Black天语PVPFeature", "天语 Stance Switcher", BLMPVP.JobID)]
        BlackEnochianPVPFeature = 80016,


        #endregion
    }
}

