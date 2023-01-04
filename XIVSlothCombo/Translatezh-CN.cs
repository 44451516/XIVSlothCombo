using System.Collections.Generic;

namespace XIVSlothCombo;

public class Translatezh_CN
{
    public static Dictionary<string, string> db = new()
    {
        #region PvE Combos

        #region Misc

        #endregion

        #region GLOBAL FEATURES

        #region Global Tank Features

        { "Global Tank Features", "全局防护职业功能" },
        { "Features and options involving shared role actions for Tanks.\nCollapsing this category does NOT disable the features inside.", "防护职业的通用功能和选项\n取消勾选这个选项不会禁用里面的功能。" },
        { "Tank: Interrupt Feature", "防护：打断功能" },
        { "Replaces Low Blow (Stun) with Interject (Interrupt) when the target can be interrupted.\nPLDs can slot Shield Bash to have the feature to work with Shield Bash.", "当目标可被打断时，使用插言替换掉下踢。\n骑士可以额外配置盾牌猛击的使用。" },
        { "Tank: Double Reprisal Protection", "防护：血仇防顶" },
        { "Prevents the use of Reprisal when target already has the effect by replacing it with Stone.", "当目标已被赋予雪仇效果时，将雪仇替换为飞石" },

        #endregion

        #region Global Healer Features

        { "Global Healer Features", "全局治疗职业功能" },
        { "Features and options involving shared role actions for Healers.\nCollapsing this category does NOT disable the features inside.", "治疗职业的通用功能和选项\n取消勾选这个选项不会禁用里面的功能。" },
        { "Healer: Raise Feature", "治疗：复活功能" },
        { "Changes the class' Raise Ability into Swiftcast.", "将复活替换为即刻咏唱" },

        #endregion

        #region Global Magical Ranged Features

        { "Global Magical Ranged Features", "全局远程魔法进攻职业功能" },
        { "Features and options involving shared role actions for Magical Ranged DPS.\nCollapsing this category does NOT disable the features inside.", "远程魔法进攻职业的通用功能和选项\n取消勾选这个选项不会禁用里面的功能。" },
        { "Magical Ranged DPS: Double Addle Protection", "远程魔法进攻: 昏乱防顶" },
        { "Prevents the use of Addle when target already has the effect by replacing it with Fell Cleave.", "当目标已被赋予昏乱效果时，将昏乱替换为裂石飞环" },
        { "Magical Ranged DPS: Raise Feature", "远程魔法进攻：复活功能" },
        { "Changes the class' Raise Ability into Swiftcast or Dualcast in the case of RDM.", "将复活替换为即刻咏唱（赤复活替换为连续咏唱）" },

        #endregion

        #region Global Melee Features

        { "Global Melee DPS Features", "全局近战进攻职业功能" },
        { "Features and options involving shared role actions for Melee DPS.\nCollapsing this category does NOT disable the features inside.", "近战进攻职业功能的通用功能和选项\n取消勾选这个选项不会禁用里面的功能。" },
        { "Melee DPS: Double Feint Protection", "近战进攻：牵制防顶" },
        { "Prevents the use of Feint when target already has the effect by replacing it with Fire.", "当目标已被赋予牵制效果时，将牵制替换为火炎" },
        { "Melee DPS: True North Protection", "近战：真北保护" },
        { "Prevents the use of True North when its buff is already active by replacing it with Fire.", "当自身拥有真北时，将真北替换为火炎" },

        #endregion

        #region Global Ranged Physical Features

        { "Global Physical Ranged Features", "全局物理远程职业功能" },
        { "Features and options involving shared role actions for Physical Ranged DPS.\nCollapsing this category does NOT disable the features inside.", "物理远程职业功能的通用功能和选项\n取消勾选这个选项不会禁用里面的功能。" },
        { "Physical Ranged DPS: Double Mitigation Protection", "物理远程进攻：减伤防顶" },
        { "Prevents the use of Tactician/Troubadour/Shield Samba when target already has one of those three effects by replacing it with Stardiver.", "当目标已被赋予行吟/策动/防守之桑巴其中之一效果时，将自己的对应技能变为坠星冲。" },
        { "Physical Ranged DPS: Ranged Interrupt Feature", "物理远程进攻：打断选项" },
        { "Replaces Foot Graze with Head Graze when target can be interrupted.", "当目标可被打断时，使用伤头代替伤足" },

        #endregion

        { "Output Combat Log", "输出战斗日志" },
        { "Outputs your performed actions to the chat.", "将你使用的技能输出到聊天框" },

        #endregion

        #region ASTROLOGIAN

        #region DPS

        { "DPS Feature", "续dot" },
        { "Replaces Malefic or Combust with options below", "用以下选项替换煞星或烧灼" },
        { "Combust Uptime Option", "烧灼上线选项" },
        { "Adds Combust to the DPS feature if it's not present on current target, or is about to expire.", "将烧灼（续DoT）加入输出循环。" },
        { "AoE DPS Feature", "AoE DPS连击" },
        { "Every option below (Lucid/AutoDraws/Astrodyne/etc) will also be added to Gravity", "下面的每个选项（醒梦/自动抽卡/星力……）也将添加到 重力aoe循环 " },
        { "Lightspeed Weave Option", "光速选项" },
        { "Adds Lightspeed", "循环加入光速" },
        { "Lucid Dreaming Weave Option", "醒梦选项" },
        { "Adds Lucid Dreaming when MP drops below slider value", "当 MP 低于滑动条的值时在循环中加入醒梦" },
        { "Divination Weave Option", "占卜选项" },
        { "Adds Divination", "循环加入占卜" },
        { "Card Draw Weave Option", "抽卡选项" },
        { "Draws your card", "抽卡" },
        { "Astrodyne Weave Option", "星力选项" },
        { "Adds Astrodyne when you have 3 seals", "当集齐三个星标时将星力加入循环" },
        { "Crown Card Draw Weave Option", "小奥秘卡（抽王冠卡）选项" },
        { "Adds Auto Crown Card Draw", "循环加入小奥秘卡" },
        { "Lord of Crowns Weave Option", "王冠之领主选项" },
        { "Adds Lord Of Crowns", "循环加入王冠之领主" },

        #endregion

        #region Healing

        { "Simple Heals (Single Target)", "简易治疗（单目标）" },
        { "Essential Dignity Feature", "先天禀赋" },
        { "Essential Dignity will be added when the target is at or below the value set", "当目标体力低于你设定的值时，自动添加先天禀赋进入单体治疗连击" },
        { "Celestial Intersection Feature", "天星交错" },
        { "Adds Celestial Intersection.", "添加天星交错进入单体治疗连击" },
        { "Aspected Benefic Feature", "吉星相位" },
        { "Adds Aspected Benefic & refreshes it if needed.", "在目标身上没有吉星相位或快要结束时，自动更换福星为吉星相位" },
        { "Exaltation Feature", "擢升设置" },
        { "Adds Exaltation.", "添加擢升进入治疗连击" },
        { "Aspected Helios Feature", "阳星相位" },
        { "Replaces Aspected Helios whenever you are under Aspected Helios regen with Helios", "当你处于阳星相位hot时，用阳星来替换阳星相位" },
        { "Celestial Opposition Feature", "天星冲日" },
        { "Adds Celestial Opposition", "添加天星冲日进入AOE治疗连击" },
        { "Lazy Lady Feature", "王冠之贵妇" },
        { "Adds Lady of Crowns, if the card is drawn", "当抽取到王冠之贵妇时，自动加入治疗连击" },
        { "Horoscope Feature", "天宫图" },
        { "Adds Horoscope.", "添加天宫图进入治疗连击" },
        { "Benefic 2 Downgrade", "福星降级" },
        { "Changes Benefic 2 to Benefic when Benefic 2 is not unlocked or available.", "如果福星还没学会或用不了，将福星替换为吉星" },

        #endregion

        #region Utility

        { "Alternative Raise Feature", "替代性的复活功能" },
        { "Changes Swiftcast to Ascend", "自动把即刻咏唱改为生辰" },

        #endregion

        #region Cards

        { "Draw on Play", "用出卡替换抽卡" },
        { "Play turns into Draw when no card is drawn, as well as the usual Play behavior.", "当没有抽取到奥秘卡时，自动将出卡替换为抽卡。" },
        { "Quick Target Cards", "快速目标发牌" },
        { "Grabs a suitable target from the party list when you draw a card and targets them for you.", "抽卡时自动选中一个合适的队友。" },
        { "Keep Target Locked", "保持目标锁定" },
        { "Keeps your target locked until you play the card", "保持目标锁定直到你使用发牌" },
        { "Add Tanks/Healers to Auto-Target", "将坦克和奶妈加入自动发牌目标选择" },
        { "Targets a tank or healer if no DPS remain for quick target selection", "如果没有 DPS 可选时选择坦克和奶妈" },
        { "Redraw Feature", "重抽功能" },
        { "Sets Draw to Redraw if you pull a card with a seal you already have and you can use Redraw.", "当你抽到星标重复的卡且重抽技能可用时，将抽卡替换为重抽" },
        { "Target Previous Feature", "选择先前目标功能" },
        { "Once you've played your card, switch back to your previously manually selected target. (May also be who you played the card on)", "出卡后，切换回你之前手动选择的目标。(也可以是你出牌的对象）。" },
        { "Target Focus Feature", "选择焦点目标功能" },
        { "Once you've played your card, switch back to your focus target.", "出卡后，切换回你的焦点目标。" },
        { "Crown Play to Minor Arcana", "用出王冠卡替换小奥秘卡" },
        { "Changes Crown Play to Minor Arcana when a card is not drawn or has Lord Or Lady Buff.", "当没有抽取到小奥秘卡时，自动将出王冠卡替换为小奥秘卡。" },
        { "Astrodyne on Play", "用星力替换出卡" },
        { "Play becomes Astrodyne when you have 3 seals.", "当你拥有三颗星星时自动替换出卡为星力" },

        #endregion

        #endregion

        #region BLACK MAGE

        { "Umbral Soul/Transpose Feature", "灵极魂/星灵移位功能" },
        { "Replaces Transpose with Umbral Soul when Umbral Soul is available.", "当灵极魂可用时将星灵移位替换为灵极魂。" },
        { "Between the Ley Lines Feature", "魔纹步功能" },
        { "Replaces Ley Lines with Between the Ley Lines when Ley Lines is active.", "使用黑魔纹后将其替换为魔纹步。" },
        { "Blizzard 1/2/3 Feature", "冰1/2/3" },
        { "Replaces Blizzard 1 with Blizzard 3 when out of Umbral Ice. Replaces Freeze with Blizzard 2 when synced.", "当自身没有灵极冰状态时替换冰结为冰封，根据自身等级自动替换玄冰为冰冻。" },
        { "Xenoglossy Feature", "异言功能" },
        { "Replaces Scathe with Xenoglossy when available.", "当异言可用时替换崩溃为异言。" },
        { "Fire 1/3 Feature", "火炎/爆炎功能" },
        { "Replaces Fire 1 with Fire 3 outside of Astral Fire or when Firestarter proc is up.", "当自身没有星极火状态或使用火起手时，用爆炎替换火炎。" },
        { "Thundercloud Option", "雷云功能" },
        { "Adds Thunder 1/3 when the debuff isn't present or expiring and Thundercloud is available to Simple BLM.", "当自身存在雷云状态且目标身上没有dot存在或dot即将结束时，自动插入闪雷/暴雷." },
        { "Simple AoE Feature", "简易 AoE 功能" },
        { "Replaces Flare with a full one button rotation.", "将核爆整合为一键AoE循环。" },
        { "Thunder 1/3 Option", "闪雷/暴雷功能" },
        { "Adds Thunder 1/3 when the debuff isn't present or expiring to Simple BLM.", "当目标身上没有dot存在或dot即将结束时，自动插入闪雷/暴雷." },
        { "Simple BLM Feature", "标准循环" },
        { "Replaces Scathe with a full one button rotation.", "将崩溃整合为一键单体标准循环。" },
        { "Cooldowns Option", "冷却选项" },
        { "Adds Manafont, Sharpcast, Amplifier onto the Simple BLM feature.", "加入魔泉, 激情咏唱至一键循环." },
        { "Ley Lines Option", "黑魔纹选项" },
        { "Adds Ley Lines onto the Simple BLM feature.", "加入黑魔纹至一键循环." },
        { "Triplecast / Swiftcast Option", "瞬发选项" },
        { "Adds Triplecast/Swiftcast onto the Simple BLM feature.", "加入三连咏唱至一键循环." },
        { "Pool Triplecast / Swiftcast Option", "保留瞬发选项" },
        { "Keep one triplecast usage and swiftcast for movement in the Simple BLM feature.", "保留即刻咏唱和一层三连咏唱，用于在移动时瞬发技能。" },
        { "Pool Xenoglossy Option", "保留异言选项" },
        { "Keep one xenoglossy usage for movement in the Simple BLM feature.", "一键循环保留一个异言便于走位" },
        { "Fire 3 Opener", "火起手" },
        { "Adds the Fire 3 Opener to Simple BLM.", "使用火起手。" },
        { "Fire 3 Opener - 1 Triplecast", "火起手+单三连咏唱" },
        { "Modifies the Simple Fire 3 Opener to only use 1 Triplecast.", "在火起手中只使用一层三连咏唱." },
        { "Foul / Manafont Flare Option", "秽浊/魔泉 核爆 功能" },
        { "Adds Foul when available during Astral Fire. Weaves Manafont after Foul for additional Flare", "当自身处于星极火状态且秽浊可用时插入秽浊，在秽浊后插入魔泉以便再使用一次核爆。" },
        { "Advanced BLM Feature", "星灵循环" },
        { "Replaces Scathe with a full one button rotation that uses Transpose. Requires level 90.", "将崩溃整合为一键星灵循环（需要90级）。" },
        { "Pool Triplecast Option", "保留三连咏唱选项" },
        { "Keep one triplecast usage for movement in the Advanced BLM feature.", "保留一层三连咏唱用于移动时瞬发技能." },
        { "Paradox BLM Feature", "悖论循环" },
        { "Replaces Scathe with a full one button rotation that has minimal casts (~9-13%% less damage than Simple BLM). Requires level 90.", "将崩溃整合为悖论循环（需要90级）。\n它的读条数很少，但比标准循环的伤害低 9-13%%。" },
        { "Adds Ley Lines onto the Advanced BLM feature.", "在星灵循环中插入黑魔纹。" },
        { "Adds Ley Lines onto the Paradox BLM feature.", "在悖论循环中插入黑魔纹。" },
        { "Swiftcast/Triplecast Moving Option", "瞬发走位选项" },
        { "Use Swiftcast/Triplecast when moving.", "移动时自动插入即刻咏唱/三连咏唱瞬发技能。" },
        { "Xenoglossy Moving Option", "异言走位选项" },
        { "Use Xenoglossy when moving.", "移动时自动插入异言。" },
        { "Scathe Moving Option", "崩溃走位选项" },
        { "Use Scathe when moving.", "移动时自动插入崩溃。" },
        { "Adds Thunder 1/3 when the debuff isn't present or expiring and Thundercloud is available to Advanced BLM.", "当自身存在雷云状态且目标身上没有dot存在或dot即将结束时，自动插入闪雷/暴雷." },
        { "Adds Thunder 1/3 when the debuff isn't present or expiring to Advanced BLM.", "当目标身上没有dot存在或dot即将结束时，自动插入闪雷/暴雷." },

        #endregion

        #region BLUE MAGE

        { "Buffed Song of Torment", "苦闷之歌buff强化" },
        { "Turns Song of Torment into Bristle so Song of Torment is buffed.", "将苦闷之歌用怒发冲冠代替，使苦闷之歌处于怒发冲冠的buff下" },
        { "Moon Flute Opener", "月之笛起手连击" },
        { "Puts the Full Moon Flute Opener on Moon Flute or Whistle.", "月之笛起手连击放置于月之笛或口笛上" },
        { "Final Sting Combo Feature", "终极针连击" },
        { "Turns Final Sting into the buff combo of: Moon Flute, Tingle, Whistle, Final Sting. Will use any primals off cooldown before casting Final Sting.", "将最终极针变成月之笛、哔哩哔哩、口笛、最终极针的BUFF组合。在施放终极极针之前，将使用任何冷却完毕的蛮神魔法。" },
        { "Off-cooldown Primal Additions", "蛮神技能冷却选项" },
        { "Adds any Primals that are off cooldown to the Final Sting Combo Feature.", "将任何已经冷却完毕的蛮神技能加入终极针连击中" },
        { "Ram's Voice into Ultravibration", "寒冰咆哮-超振动" },
        { "Turns Ultravibration into Ram's Voice if Deep Freeze isn't on the target. Will swiftcast Ultravibration if available.", "如果目标未被冰冻，则将超震动更改为寒冰咆哮。如果目标已被冰冻，则会即刻咏唱超震动。" },
        { "Tank Debuff Feature", "坦克Debuff特性" },
        { "Puts Devour, Off-Guard, Lucid Dreaming, and Bad Breath into one button when under Tank Mimicry.", "在【以太复制：防护】下，将捕食、破防、醒梦、臭气合并为一个按钮。" },
        { "Addle/Magic Hammer Debuff Feature", "昏乱/魔法锤Debuff特性" },
        { "Turns Magic Hammer into Addle when off CD.", "当冷却完毕时，用魔法锤代替昏乱" },
        { "Primal Feature", "蛮神技特性" },
        { "Turns Feather Rain into any Primals that are off CD. \nWill cause primals to desync from Moon Flute burst phases if used on CD.", "在冷却结束后，将所有蛮神技能整合至飞翎羽。\n如果在冷却未结束的时候使用，将导致月之笛爆发不完整。" },
        { "Knight's Tour Feature", "骑士之旅特性" },
        { "Turns Black Knight's Tour or White Knight's Tour into its counterpart when the enemy is under the effect of the spell's debuff.", "在敌人处于“止步”或“减速”状态下时，自动将在黑/白骑士之旅间切换。" },
        { "Peripheral Synthesis into Mustard Bomb", "生成外设-芥末爆弹" },
        { "Turns Peripheral Synthesis into Mustard Bomb when target is under the effect of Lightheaded.", "当目标受到头晕的影响时，将生成外设变为芥末爆弹。" },
        { "Solo Mode", "单挑模式" },
        { "Uses Basic Instinct if you're in an instance and on your own.", "如果你在单挑副本，使用斗争本能。" },
        { "Hydro Pull Setup", "水力吸引设置" },
        { "Uses Hydro Pull before using Ram's Voice.", "在使用寒冰咆哮之前先用水力吸引。" },
        { "J Kick Option", "正义飞踢选项" },
        { "Adds J Kick to the Primal Feature.", "将正义飞踢加入蛮神技能选项" },
        { "Perpetual Ray into Sharpened Knife Feature", "永恒射线-锋利菜刀" },
        { "Turns Perpetual Ray into Sharpened Knife when target is stunned and in melee range.", "当目标是眩晕且在近战范围内时，将永恒射线转为锋利菜刀。" },
        { "Moon Flute Burst Pooling Option", "月之笛爆发选项" },
        { "Holds spells if Moon Flute burst is about to occur and spells are off cooldown.", "如果技能已经冷却完毕而月之笛即将冷却结束，则保留冷却完毕的技能至月之笛爆发期使用。" },
        { "Sonic Boom Melee Feature", "音爆选项" },
        { "Turns Sonic Boom into Sharpened Knife when in melee range.", "在近战范围内，使用锋利菜刀替代音爆。" },

        #endregion

        #region BARD

        { "Heavy Shot into Straight Shot", "强力射击设置" },
        { "Replaces Heavy Shot/Burst Shot with Straight Shot/Refulgent Arrow when procced.", "触发直线射击预备状态时，替换强力射击/爆发射击为直线射击/辉煌箭。" },
        { "DoT Maintenance Option", "Dot选项" },
        { "Enabling this option will make Heavy Shot into Straight Shot refresh your DoTs on your current.", "开启此选项可适时插入毒/风箭。" },
        { "Iron Jaws Feature", "伶牙俐齿续dot模式A" },
        { "Iron Jaws is replaced with Caustic Bite/Stormbite if one or both are not up.\nAlternates between the two if Iron Jaws isn't available.", "当目标身上没有毒/风dot时，替换伶牙俐齿为毒/风箭。\n当还未习得伶牙俐齿时会在毒/风箭之间自动切换。" },
        { "Iron Jaws Alternate Feature", "伶牙俐齿续dot模式B" },
        { "Iron Jaws is replaced with Caustic Bite/Stormbite if one or both are not up.\nIron Jaws will only show up when debuffs are about to expire.", "当目标身上没有毒/风dot时，替换伶牙俐齿为毒/风箭。 \n伶牙俐齿仅会在风/毒dot即将结束时复现。" },
        { "Burst Shot/Quick Nock into Apex Arrow", "绝峰箭设置" },
        { "Replaces Burst Shot and Quick Nock with Apex Arrow when gauge is full and Blast Arrow when you are Blast Arrow ready.", "当灵魂之声蓄满时，替换爆发射击/连珠箭为绝峰箭，触发爆破箭预备状态时替换为爆破箭。" },
        { "Single Target oGCD Feature", "单目标能力技插入选项" },
        { "All oGCD's on Bloodletter (+ Songs rotation) depending on their CD.", "在三歌循环中根据cd时间替换失血箭为其他能力技。" },
        { "AoE oGCD Feature", "AOE能力技插入选项" },
        { "All AoE oGCD's on Rain of Death depending on their CD.", "在三歌循环中根据cd时间替换死亡箭雨为其他能力技。" },
        { "AoE Combo Feature", "AOE连击" },
        { "Replaces Quick Nock/Ladonsbite with Shadowbite when ready", "在影噬箭预备状态下替换连珠箭/百首龙牙箭为影噬箭。" },
        { "Simple Bard", "单体简易一键连击" },
        { "Adds every single target ability to one button,\nIf there are DoTs on target Simple Bard will try to maintain their uptime.", "一键整合所有单体攻击技能。\n如果目标身上有dots，会同时尝试保持dots不断。" },
        { "Simple Bard DoTs", "简易诗人Dots" },
        { "This option will make Simple Bard apply DoTs if none are present on the target.", "如果目标身上不存在风/毒dot，开启此选项会在连击中加入风/毒箭。" },
        { "Simple Bard Songs", "简易诗人唱歌" },
        { "This option adds the bards songs to the Simple Bard feature.", "在连击中自动加入三首歌循环。" },
        { "Song Feature", "AOE连击唱歌设置" },
        { "Adds Songs onto AoE oGCD Feature.", "在AOE连击中加入三首歌循环。" },
        { "Bard Buffs Feature", "Buff技能设置" },
        { "Adds Raging Strikes and Battle Voice onto Barrage.", "将猛者强击/战斗之声整合至纷乱箭。" },
        { "One Button Songs", "一键唱歌" },
        { "Add Mage's Ballad and Army's Paeon to Wanderer's Minuet depending on cooldowns", "将贤者的叙事谣/军神的赞美歌整合至放浪神的小步舞曲，并根据cd时间自动切换。" },
        { "Simple AoE Bard", "AOE简易一键连击" },
        { "Weaves oGCDs onto Quick Nock/Ladonsbite", "在连珠箭/百首龙牙箭连打时插入能力技。" },
        { "Simple AoE Bard Song", "简易诗人唱歌(AOE)" },
        { "Weave songs on the Simple AoE", "自动插入三首歌循环。" },
        { "Simple Buffs", "简易诗人Buffs" },
        { "Adds buffs onto the Simple Bard feature.", "自动插入Buff技能。" },
        { "Simple Buffs - Radiant", "光明神的最终乐章替换设置" },
        { "Adds Radiant Finale to the Simple Buffs feature.", "当可用时自动插入光明神的最终乐章。" },
        { "Simple No Waste Mode", "简易无浪费模式" },
        { "Adds enemy health checking on mobs for buffs, dots and songs.\nThey will not be reapplied if less than specified.", "在使用buff，dots和唱歌时，加入敌人血量检测。\n如果血量少于指定值，则不会再次重新应用。" },
        { "Simple Interrupt", "单体连击打断技能设置" },
        { "Uses interrupt during simple bard rotation if applicable", "在合适时机插入打断技能。" },
        { "Disable Apex Arrow", "禁止绝峰箭自动替换" },
        { "Removes Apex Arrow from Simple Bard and AoE Feature.", "不在一键连击中自动替换插入绝峰箭。" },
        { "Simple Opener", "单体简易起手" },
        { "Adds the optimum opener to simple bard.\nThis conflicts with pretty much everything outside of simple bard options due to the nature of the opener.", "在单体一键连击中加入最佳起手技能。\n此选项与其它绝大部分类似选项均有冲突。" },
        { "Simple Pooling", "一键循环异言设置" },
        { "Pools Bloodletter charges to allow for optimum burst phases.", "将失血箭集中于最佳爆发期" },
        { "Iron Jaws Apex", "伶牙俐齿替换绝峰箭" },
        { "Adds Apex and Blast Arrow to Iron Jaws when available", "当绝峰箭/爆破箭可用时，替换伶牙俐齿为绝峰箭/爆破箭。" },
        { "Simple Raging Jaws", "简易猛者中续伶牙俐齿" },
        { "Enable the snapshotting of DoTs, within the remaining time of Raging Strikes below:", "在猛者强击持续时间低于设定值时，自动替换插入dot技能。" },
        { "Opener Only", "只有起手" },
        { "Until the first auto-refresh you can DoT new targets automatically.", "你可以自动对新目标释放Dots直到第一次自动刷新(伶牙俐齿)\n即启用本选项后，在第一次自动刷新(伶牙俐齿)后不会对新目标上Dot(包括同一目标上天断Dot后)" },
        { "Exclude Wanderer's Minuet", "除外放浪神的小步舞曲" },
        { "Dont use Wanderer's Minuet.", "不使用放浪神的小步舞曲" },

        #endregion

        #region DANCER

        #region Single Target Multibutton

        { "Single Target Multibutton Feature", "单目标多按钮" },
        { "Single target combo with Fan Dances and Esprit use.", "使用扇舞与伶俐值的单目标连击" },
        { "ST Esprit Overcap Option", "单体目标伶俐防溢出设置" },
        { "Adds Saber Dance above the set Esprit threshold.", "伶俐超过下面设置的数值后，将剑舞加入循环" },
        { "Fan Dance Overcap Protection Option", "幻扇溢出保护" },
        { "Adds Fan Dance 1 when Fourfold Feathers are full.", "当量谱上幻扇满了后，将扇舞序加入循环" },
        { "Fan Dance Option", "扇舞选项" },
        { "Adds Fan Dance 3/4 when available.", "当扇舞急·扇舞终可用时，加入循环。" },

        #endregion

        #region AoE Multibutton

        { "AoE Multibutton Feature", "AOE多按钮" },
        { "AoE combo with Fan Dances and Esprit use.", "使用扇舞与伶俐值的AOE连击" },
        { "AoE Esprit Overcap Option", "AoE伶俐防溢出设置" },
        { "AoE Fan Dance Overcap Protection Option", "AoE幻扇溢出保护" },
        { "Adds Fan Dance 2 when Fourfold Feathers are full.", "当量谱上幻扇满了后，将扇舞破加入循环" },
        { "AoE Fan Dance Option", "AOE幻扇选项" },

        #endregion

        #region Dance Features

        { "Dance Features", "跳舞功能" },
        { "Features and options involving Standard Step and Technical Step.\nCollapsing this category does NOT disable the features inside.", "标准舞步和技巧舞步相关功能与设置\n折叠此选项不会关闭其中的功能。" },

        #region Combined Dance Feature

        { "Combined Dance Feature", "舞步整合" },
        { "Standard And Technical Dance on one button (SS).\nStandard > Technical.\nThis combos out into Tillana and Starfall Dance.", "将标准舞步与机巧舞步整合为一个技能。标准舞步>技巧舞步。 此连击后接续提拉纳和流星舞." },
        { "Devilment Plus Option", "进攻之探戈附加设置" },
        { "Adds Devilment right after Technical finish.", "在技巧舞步结束后接续进攻之探戈。" },
        { "Flourish Plus Option", "百花争艳附加设置" },
        { "Adds Flourish to the Combined Dance Feature.", "将百花争艳加入到舞步整合。" },

        #endregion

        { "Custom Dance Step Feature", "自定义舞步功能" },
        { "Change custom actions into dance steps while dancing.\nThis helps ensure you can still dance with combos on, without using auto dance.\nYou can change the respective actions by inputting action IDs below for each dance step.\nThe defaults are Cascade, Flourish, Fan Dance and Fan Dance II. If set to 0, they will reset to these actions.\nYou can get Action IDs with Garland Tools by searching for the action and clicking the cog.", "自定义在跳舞时变为舞步技能的技能。\n此功能可以让你开启了其他连击选项时仍然可以自己跳舞。\n你可以通过输入技能ID来改变对应的舞步技能。\n默认替换为瀑泻,百花争艳,扇舞·序,扇舞·破. 设置为0时会重置为这些技能。\n你可以在Garland Tools找到技能的ID。" },

        #endregion

        #region Flourishing Features

        { "Flourishing Features", "百花争艳期间功能" },
        { "Features and options involving Fourfold Feathers and Flourish.\nCollapsing this category does NOT disable the features inside.", "幻扇和百花争艳相关功能与设置\n折叠此选项不会关闭其中的功能。" },
        { "Flourishing Fan Dance Feature", "百花齐放中扇舞相关设置" },
        { "Replace Flourish with Fan Dance 3 & 4 during weave-windows, when Flourish is on cooldown.", "在使用百花齐放后的窗口期内，将百花齐放替换为扇舞急·扇舞终" },

        #endregion

        #region Fan Dance Combo Features

        { "Fan Dance Combo Feature", "扇舞连击" },
        { "Options for Fan Dance combos.\nFan Dance 3 takes priority over Fan Dance 4.", "扇舞连击相关选项。扇舞急优先于扇舞终" },
        { "Fan Dance 1 -> 3 Option", "扇舞·序 -> 扇舞·急" },
        { "Changes Fan Dance 1 to Fan Dance 3 when available.", "当可用时，将扇舞·序替换为扇舞·急。" },
        { "Fan Dance 1 -> 4 Option", "扇舞·序 -> 扇舞·终" },
        { "Changes Fan Dance 1 to Fan Dance 4 when available.", "当可用时，将扇舞·序替换为扇舞·终。" },
        { "Fan Dance 2 -> 3 Option", "扇舞·破 -> 扇舞·急" },
        { "Changes Fan Dance 2 to Fan Dance 3 when available.", "当可用时，将扇舞·破替换为扇舞·急。" },
        { "Fan Dance 2 -> 4 Option", "扇舞·破 -> 扇舞·终" },
        { "Changes Fan Dance 2 to Fan Dance 4 when available.", "当可用时，将扇舞·破替换为扇舞·终。" },

        #endregion

        { "Devilment to Starfall Feature", "进攻之探戈 -> 流星舞" },
        { "Change Devilment into Starfall Dance after use.", "使用完进攻之探戈后将其替换为流星舞。" },
        { "Dance Step Combo Feature", "舞步连击相关" },
        { "Change Standard Step and Technical Step into each dance step while dancing.\nWorks with Simple Dancer and Simple Dancer AoE.", "跳舞时将标准舞步和技巧舞步替换为对应的舞步技能。\n与'单体一键连击'和'AoE一键连击'兼容" },

        #region Simple Dancer (Single Target)

        { "Simple Dancer (Single Target) Feature", "简易舞者（单目标）" },
        { "Single button, single target. Includes songs, flourishes and overprotections.\nConflicts with all other non-simple toggles, except 'Dance Step Combo'.", "舞者单目标一键连击。包含唱，跳，Rap，打篮球，百花齐放和溢出保护。几乎与所有其他非一键选项冲突，除了上面的舞步连击" },
        { "Simple Interrupt Option", "单体连击打断技能设置" },
        { "Includes an interrupt in the rotation (if applicable to your current target).", "循环加入打断技能" },
        { "Simple Dancer (Double Targets) Feature.", "简易舞步(双目标)" },
        { "Single button, double targets. Includes songs, flourishes and overprotections.\nConflicts with all other non-simple toggles, except 'Dance Step Combo'.", "舞者双目标一键连击。包含唱歌，百花齐放和溢出保护。\n几乎与所有其他非一键选项冲突，除了上面的舞步连击" },
        { "Simple Standard Dance Option", "简易标准舞步选项" },
        { "Includes Standard Step (and all steps) in the rotation.", "循环加入标准舞步" },
        { "Simple Standard Fill Option", "简易标准舞步选项" },
        { "Adds ONLY Standard dance steps and Standard Finish to the rotation.\nStandard Step itself must be initiated manually when using this option.", "只将标准舞步和标准舞步结束加入循环\n开启后必须手动跳标准舞步" },
        { "Simple Technical Dance Option", "简易技巧舞步选项" },
        { "Includes Technical Step, all dance steps and Technical Finish in the rotation.", "循环加入技巧舞步" },
        { "Simple Tech Fill Option", "简易技巧舞步选项" },
        { "Adds ONLY Technical dance steps and Technical Finish to the rotation.\nTechnical Step itself must be initiated manually when using this option.", "只将简易技巧舞步和简易技巧舞步结束加入循环\n开启后必须手动跳简易技巧舞步" },
        { "Simple Tech Devilment Option", "简易技巧进攻之探戈" },
        { "Includes Devilment in the rotation.\nWill activate only during Technical Finish if you are Lv70 or above.", "70级以上时，循环在技巧舞步结束后加入进攻之探戈" },
        { "Simple Flourish Option", "简易百花争艳" },
        { "Includes Flourish in the rotation.", "将百花争艳加入循环。" },
        { "Simple Feathers Option", "简易幻扇" },
        { "Includes Feather usage in the rotation.", "将幻扇的使用加入循环。" },
        { "Simple Feather Pooling Option", "简易幻扇囤积" },
        { "Expends a feather in the next available weave window when capped.\nWeaves feathers where possible during Technical Finish.\nWeaves feathers outside of burst when target is below set HP percentage.", "幻扇将要溢出时使用一次\n技巧舞步后倾泄幻扇\n目标血量低于设定值时倾泄幻扇" },
        { "Simple Panic Heals Option", "简易紧急恢复" },
        { "Includes Curing Waltz and Second Wind in the rotation when available and your HP is below the set percentages.", "自身血量低于设定值时使用治疗华尔兹、内丹" },
        { "Simple Improvisation Option", "简易即兴表演" },
        { "Includes Improvisation in the rotation when available.", "当即兴表演可用时将其加入循环。" },
        { "Simple Peloton Opener Option", "简易速行起手" },
        { "Uses Peloton when you are out of combat, do not already have the Peloton buff and are performing Standard Step with greater than 5s remaining of your dance.\nWill not override Dance Step Combo Feature.", "当脱战时使用速行，不会一直保持速行buff存在，并且在跳舞剩余时间少于5秒后释放舞蹈\n不会覆盖舞步连击设置。" },

        #endregion

        #region Simple Dancer (AoE)

        { "Simple Dancer (AoE) Feature", "简易舞者（AOE）" },
        { "Single button, AoE. Includes songs, flourishes and overprotections.\nConflicts with all other non-simple toggles, except 'Dance Step Combo'.", "舞者AOE一键连击。包含唱，跳，Rap，打篮球，百花齐放和溢出保护。几乎与所有其他非一键选项冲突，除了上面的舞步连击" },
        { "Simple AoE Interrupt Option", "简易AoE中断" },
        { "Includes an interrupt in the AoE rotation (if your current target can be interrupted).", "在AoE循环中加入中断(伤头)(如果当前目标可被打断)。" },
        { "Simple AoE Standard Dance Option", "简易标准舞步选项（AOE）" },
        { "Includes Standard Step (and all steps) in the AoE rotation.", "循环加入标准舞步" },
        { "Simple AoE Standard Fill Option", "简易标准舞步选项（AOE）" },
        { "Adds ONLY Standard dance steps and Standard Finish to the AoE rotation.\nStandard Step itself must be initiated manually when using this option.", "只将标准舞步和标准舞步结束加入循环\n开启后必须手动跳标准舞步" },
        { "Simple AoE Technical Dance Option", "简易技巧舞步选项（AOE）" },
        { "Includes Technical Step, all dance steps and Technical Finish in the AoE rotation.", "循环加入技巧舞步" },
        { "Simple AoE Tech Fill Option", "简易技巧舞步选项（AOE）" },
        { "Adds ONLY Technical dance steps and Technical Finish to the AoE rotation.\nTechnical Step itself must be initiated manually when using this option.", "只将简易技巧舞步和简易技巧舞步结束加入循环\n开启后必须手动跳简易技巧舞步" },
        { "Simple AoE Tech Devilment Option", "简易AoE技巧进攻之探戈" },
        { "Includes Devilment in the AoE rotation.\nWill activate only during Technical Finish if you Lv70 or above.", "70级以上时，循环在技巧舞步结束后加入进攻之探戈" },
        { "Simple AoE Flourish Option", "简易AoE百花争艳" },
        { "Includes Flourish in the AoE rotation.", "将百花争艳加入AoE循环。" },
        { "Simple AoE Feathers Option", "简易AoE幻扇" },
        { "Includes feather usage in the AoE rotation.", "将幻扇的使用加入AoE循环。" },
        { "Simple AoE Feather Pooling Option", "简易AoE幻扇囤积" },
        { "Expends a feather in the next available weave window when capped.", "幻扇将要溢出时使用一次" },
        { "Simple AoE Panic Heals Option", "简易AoE紧急恢复" },
        { "Includes Curing Waltz and Second Wind in the AoE rotation when available and your HP is below the set percentages.", "自身血量低于设定值时使用治疗华尔兹、内丹" },
        { "Simple AoE Improvisation Option", "简易AoE即兴表演" },
        { "Includes Improvisation in the AoE rotation when available.", "当即兴表演可用时将其加入AoE循环。" },

        #endregion

        #endregion

        #region DARK KNIGHT

        { "Buffs on Main Combo", "主连击Buff整合" },
        { "Collection of buffs to add to the main combo", "将所有增益技能整合至噬魂斩连击中." },
        { "Cooldowns on Main Combo", "暗黑骑士能力技整合 设置" },
        { "GNB Cooldowns on Main Combo", "GNB能力技整合 设置" },
        { "Collection of cooldowns to add to the main combo", "将所有能力技整合至噬魂斩连击中." },
        { "Souleater Combo", "噬魂斩连击" },
        { "Replace Souleater with its combo chain. \nIf all sub options are selected will turn into a full one button rotation (Advanced Dark Knight)", "用基础循环替换掉噬魂斩。 \n如果所有的次级选项都被开启，那么就可以进行一键循环(简单黑骑)" },
        { "Stalwart Soul Combo", "刚魂连击" },
        { "Replace Stalwart Soul with its combo chain.", "用基础循环替换掉刚魂。" },
        { "Bloodspiller Feature", "血溅 设置" },
        { "Replace Souleater and Stalwart Soul with Bloodspiller and Quietus when Delirium is active.", "当血乱激活时，使用血溅和寂灭替换掉噬魂战和刚魂。" },
        { "Dark Knight Gauge Overcap Feature", "暗血量表溢出特性 SoulCombo" },
        { "Replace AoE combo with gauge spender if you are about to overcap.", "当你的暗血即将溢出时，使用寂灭替换掉刚魂。" },
        { "Living Shadow Feature", "掠影示现特性" },
        { "Living Shadow will now be on main combo if its not on cooldown and you have gauge for it.", "当掠影示现未在冷却中且暗血值足够时将其插入主连击中." },
        { "Edge of Shadow Overcap Feature", "暗影锋防覆盖 设置" },
        { "Uses Edge of Shadow if you are above 8,500 mana or Darkside is about to expire (10sec or less)", "当你的mp高于8500或暗黑状态即将结束时(剩余持续时间不超过10秒)自动插入暗影锋." },
        { "oGCD Feature", "能力技特性" },
        { "Adds Living Shadow > Salted Earth > Carve And Spit > Salt And Darkness to Carve And Spit and Abysal Drain", "按照掠影示现 > 腐秽大地 > 精雕怒斩 > 腐秽黑暗的顺序将能力技整合到精雕怒斩与吸血深渊。" },
        { "Shadowbringer oGCD Feature", "暗影使者 设置" },
        { "Adds Shadowbringer to oGCD Feature", "将暗影使者整合到能力技特性。" },
        { "Plunge Feature", "跳斩特性" },
        { "Adds Plunge onto main combo whenever its available and Darkside is up.", "当暗黑激活时将跳斩添加到主连击。" },
        { "Delayed Bloodspiller Feature", "延后使用血溅 设置" },
        { "Delays Bloodspiller by 2 GCDs when Delirium is used during even windows, uses it regularly during odd windows. Useful for feeding into raid buffs at level 90.", "在偶数分钟窗口使用血乱时将延迟两个GCD，而在奇数分钟窗口不进行延后。这将可以使技能更容易打进团辅。" },
        { "Unmend Uptime Feature", "伤残特性" },
        { "Replace Souleater Combo Feature with Unmend when you are out of range.", "当你离开近战范围时使用伤残替换主连击。" },
        { "Abyssal Drain Feature", "吸血深渊特性" },
        { "Adds abyssal drain to the AoE combo when you fall below 60 percent hp.", "当你的血量低于60%时，在刚魂连击中插入吸血深渊." },
        { "AoE Shadowbringer Feature", "AOE暗影使者特性" },
        { "Adds Shadowbringer to the AoE combo.", "在刚魂连击中插入暗影使者." },
        { "Flood of Shadow Overcap Feature", "暗影波动防溢出 设置" },
        { "Uses Flood of Shadow if you are above 8.5k mana or Darkside is about to expire (10sec or less)", "当你的mp值高于8500或暗影状态即将结束时,将暗影波动自动插入到刚魂连击中使用." },
        { "Blood Gauge Overcap Feature", "暗血量表溢出特性" },
        { "Adds Bloodspiller onto main combo when at 80 blood gauge or higher", "当你的暗血不低于80时，将血溅整合到主连击。" },
        { "Shadowbringer Feature", "暗影使者能力技特性" },
        { "Adds Shadowbringer on main combo while Darkside is up. Will use all stacks on cooldown.", "当自身存在暗影状态时插入暗影使者至主连击中.会使用所有可用层数." },
        { "Edge of Shadow Burst Option", "暗影锋爆发 设置" },
        { "Uses Edge of Shadow until chosen MP limit is reached during minute window bursts.", "在爆发期直到MP到达设定值之前都会一直插入暗影锋." },
        { "Shadowbringer Burst Option", "暗影使者爆发设置" },
        { "Pools Shadowbringer to use during even minute window bursts.", "将暗影使者打入偶数分钟的爆发窗口中。" },
        { "Carve and Spit Feature", "精雕怒斩特性" },
        { "Adds Carve and Spit on main combo while Darkside is up.", "当自身存在暗影状态时插入精雕怒斩至主连击中." },
        { "Melee Plunge Option", "近战时使用跳斩 设置" },
        { "Uses Plunge when under Darkside and in the target ring (1 yalm).\nWill use as many stacks as selected in the above slider.", "当你在目标圈内且自身存在暗影状态时在主连击中自动插入跳斩.\n会使用上方选择的所有跳斩层数." },
        { "Salted Earth Feature", "腐秽大地特性" },
        { "Adds Salted Earth on main combo while Darkside is up, will use Salt and Darkness if unlocked.", "当自身存在暗影状态时插入腐秽大地至主连击中,习得腐秽黑暗后在可用时也会同样插入" },
        { "Delirium on Cooldown", "血乱整合 设置" },
        { "Adds Delirium to main combo on cooldown and when Darkside is up. Will also spend 50 blood gauge if Delirium is nearly ready to protect from overcap.", "当自身存在暗影状态且血乱可用时插入血乱.同样当血乱即将结束冷却时会根据自身暗血值先插入血溅/寂灭以防止暗血值溢出." },
        { "Blood Weapon on Cooldown", "嗜血整合 设置" },
        { "Adds Blood Weapon to main combo on cooldown and when Darkside is up.", "若自身存在暗影状态且嗜血未在冷却中,在主连击中插入嗜血." },
        { "Blood Weapon Option", "嗜血 设置" },
        { "Adds Blood Weapon to AoE combo on cooldown and when Darkside is up.", "若自身存在暗影状态且嗜血未在冷却中,在刚魂连击中插入嗜血." },
        { "Delirium Option", "血乱 设置" },
        { "Adds Deliriun to AoE combo on cooldown and when Darkside is up.", "若自身存在暗影状态且嗜血未在冷却中,在刚魂连击中插入血乱." },
        { "Salted Earth Option", "腐秽大地 设置" },
        { "Adds Salted Earth and Salt and Darkness to AoE on cooldown and when Darkside is up.", "若自身存在暗影状态且腐秽大地未在冷却中,在刚魂连击中插入腐秽大地与腐秽黑暗." },
        { "Living Shadow Option", "掠影示现 设置" },
        { "Adds Living Shadow to AoE on cooldown and when Darkside is up.", "若自身存在暗影状态且掠影示现未在冷却中,在刚魂连击中插入掠影示现." },

        #endregion

        #region DRAGOON

        #region Advanced Dragoon

        { "Advanced Dragoon", "高级龙骑" },
        { "Replaces Full Thrust with the entire ST combo chain.", "将精准刺替换为相应连击" },
        { "Level 88+ Opener", "88级起手" },
        { "Adds opener to the rotation.\nActivates when Battle Litany and Lance Charge are off cooldown and when True North is used outside of combat. OPTIONAL: USE REACTION OR MOACTION FOR OPTIMAL TARGETING.", "当。注意：需要自行选择最优目标" },
        { "CDs on Main Combo", "主连击CD整合" },
        { "Collection of CD features on Main Combo.", "cd技能循环整合" },
        { "Collection of Buff features on Main Combo.", "buff技能循环整合" },
        { "Wyrmwind Thrust Option", "天龙点睛选项" },
        { "Includes Wyrmwind Thrust to the rotation.", "将天龙点睛加入循环" },
        { "Geirskogul and Nastrond Option", "武神枪和死者之岸选项" },
        { "Includes Geirskogul and Nastrond to the rotation.", "将武神枪和死者之岸加入循环" },
        { "Dives Option", "跳跃选项" },
        { "Adds Spineshatter Dive, Dragonfire Dive, and Stardiver to the rotation.\n Select options below for when to use dives.", "将破碎冲，龙炎冲和坠星冲加入循环，用以下选项确定释放时机" },
        { "High Jump Option", "高跳选项" },
        { "Includes High Jump/Jump to the rotation.", "将高跳/跳跃加入循环" },
        { "Mirage Option", "幻象冲选项" },
        { "Includes Mirage Dive to the rotation.", "将幻象冲加入循环" },
        { "Lance Charge Option", "猛枪选项" },
        { "Includes Lance Charge to the rotation.", "猛枪加入循环" },
        { "Dragon Sight Option", "巨龙视线选项" },
        { "Includes Dragon Sight to the rotation. OPTIONAL: USE REACTION OR MOACTION FOR OPTIMAL TARGETING.", "巨龙视线设置。注意：需要自行选择最优目标" },
        { "Battle Litany Option", "战斗连祷选项" },
        { "Includes Battle Litany to the rotation.", "将战斗连祷加入循环" },
        { "Life Surge Option", "龙剑选项" },
        { "Includes Life Surge, while under proper buffs, onto proper GCDs, to the rotation.", "在樱花/直刺/山境酷刑连击中的合适时机和buff下插入龙剑。" },
        { "Ranged Uptime Option", "超出近战范围选项" },
        { "Replaces Main Combo with Piercing Talon when you are out of melee range.", "超出近战范围时，将主要AOE连击替换为贯穿尖" },
        { "Melee Dives Option", "冲冲冲设置" },
        { "Uses Spineshatter Dive, Dragonfire Dive, and Stardiver when in the target's target ring (1 yalm) and closer.", "当在目标的目标圈内（1米）时使用破碎冲，龙炎冲和坠星冲" },

        #endregion

        #region Advanced Dragoon AoE

        { "Advanced Dragoon AoE", "高级龙骑AOE" },
        { "Replaces Coerthan Torment with its combo chain", "用基础循环替换掉山境酷刑" },
        { "Wyrmwind Thrust AoE Feature", "天龙点睛设置" },
        { "Includes Wyrmwind Thrust to the AoE rotation.", "将天龙点睛插入AOE循环" },
        { "Geirskogul and Nastrond AoE Feature", "武神枪与死者之岸设置" },
        { "Includes Geirskogul and Nastrond to the AoE rotation.", "将武神枪与死者之岸插入AOE循环" },
        { "Dives AoE Feature", "跳跃类能力技设置" },
        { "Includes Spineshatter Dive, Dragonfire Dive and Stardiver to the AoE rotation.", "在aoe连击中插入破碎冲，龙炎冲，坠星冲。" },
        { "High Jump AoE Feature", "高跳设置" },
        { "Includes High Jump to the AoE rotation.", "在连击中插入高跳。" },
        { "Mirage AoE Feature", "幻象冲设置" },
        { "Includes Mirage to the AoE rotation.", "在AOE连击中插入幻象冲" },

        #region Buffs AoE Feature

        { "Buffs AoE Feature", "buff设置" },
        { "Includes Lance Charge and Battle Litany to the AoE rotation.", "在连击中插入猛枪和战斗连祷。" },
        { "Dragon Sight AoE Feature", "巨龙视线设置Aoe" },
        { "Includes Dragon Sight to the AoE rotation. OPTIONAL: USE REACTION OR MOACTION FOR OPTIMAL TARGETING.", "在连击中插入巨龙视线。需要自行选择最优目标。" },

        #endregion

        { "Life Surge AoE Feature", "龙剑设置 AoE Feature" },
        { "Includes Life Surge, while under proper buffs, onto proper GCDs, to the AoE rotation.", "在连击中合适的状态和窗口内插入龙剑。" },
        { "Replaces Main AoE Combo with Piercing Talon when you are out of melee range.", "超出近战范围时，将主要AOE连击替换为贯穿尖" },

        #endregion

        { "Stardiver Feature", "坠星冲设置" },
        { "Turns Stardiver into Nastrond during Life of the Dragon, and Geirskogul outside of Life of the Dragon.", "等待翻译" },
        { "Lance Charge to Battle Litany Feature", "猛枪整合到战斗连祷" },
        { "Turns Lance Charge into Battle Litany when the former is on cooldown.", "猛枪冷却完毕后整合至战斗连祷" },
        { "Adds Dragon Sight to Lance Charge, will take precedence over Battle Litany.", "将巨龙视线整合至猛枪（优先于战斗连祷）" },

        #endregion

        #region GUNBREAKER

        { "Solid Barrel Combo", "迅连斩连击" },
        { "Replace Solid Barrel with its combo chain. \nIf all sub options are selected will turn into a full one button rotation (Advanced Gunbreaker)", "整合攻击技能至迅连斩. \n如果开启下列全部子选项将会变成绝枪战士一键单体连击." },
        { "Gnashing Fang and Continuation on Main Combo", "烈牙与续剑整合到主连击" },
        { "Adds Gnashing Fang to the main combo. Gnashing Fang must be started manually and the combo will finish it off.\n Useful for when Gnashing Fang needs to be help due to downtime.", "将烈牙与续剑整合到主连击。烈牙必须被手动激活，之后会被替换到主连击。 \n 当烈牙需要空转的时候，可以只激活此选项。" },
        { "Adds various cooldowns to the main combo when under No Mercy or when No Mercy is on cooldown", "选择是否在无情状态下将各种能力技整合至主连击中" },
        { "Double Down on Main Combo", "倍攻整合到主连击" },
        { "Adds Double Down to the main combo when under No Mercy buff", "在无情状态下才在主连击中插入倍攻." },
        { "Rough Divide Option", "粗分斩设置" },
        { "Adds Rough Divide to the main combo whenever it's available.", "当可用时在主连击中插入粗分斩." },
        { "Danger Zone/Blasting Zone on Main Combo", "危险领域/爆破领域整合到主连击" },
        { "Adds Danger Zone/Blasting Zone to the main combo", "在主连击中插入危险领域/爆破领域." },
        { "Demon Slaughter Combo", "恶魔杀连击" },
        { "Replace Demon Slaughter with its combo chain.", "将恶魔杀替换为恶魔杀连击。" },
        { "Ammo Overcap Feature", "晶壤溢出特性" },
        { "Uses Burst Strike/Fated Circle on the respective ST/AoE combos when ammo is about to overcap.", "当晶壤将要溢出时使用爆发击/命运之环替换掉单体/AOE连击。" },
        { "Gnashing Fang Continuation Combo", "烈牙续剑连击" },
        { "Adds Continuation to Gnashing Fang.", "将续剑添加到烈牙." },
        { "No Mercy on Gnashing Fang", "无情整合到烈牙" },
        { "Adds No Mercy to Gnashing Fang when it's ready.", "当无情冷却结束时将其整合到烈牙." },
        { "Double Down on Gnashing Fang", "倍攻整合到烈牙" },
        { "Adds Double Down to Gnashing Fang when No Mercy buff is up.", "当拥有无情BUFF时将倍攻整合到烈牙" },
        { "CDs on Gnashing Fang", "冷却技能整合到烈牙" },
        { "Adds Sonic Break/Bow Shock/Blasting Zone on Gnashing Fang, order dependent on No Mercy buff. \nBurst Strike and Hypervelocity added if there's charges while No Mercy buff is up.", "当自身处于无情状态下将音速破/弓形冲波/爆破领域整合至烈牙,并根据无情状态剩余时间安排插入顺序. \n同时也会整合爆发击与超高速." },
        { "Burst Strike Continuation", "爆发击续剑" },
        { "Adds Hypervelocity on Burst Strike.", "超高速可以使用的时候，超高速替换爆发击" },
        { "Burst Strike to Bloodfest Feature", "血壤替换爆发击特性" },
        { "Replace Burst Strike with Bloodfest if you have no powder gauge.", "当你没有晶壤可用时使用血壤替换爆发击。" },
        { "Bloodfest on Main Combo", "血壤整合到主连击" },
        { "Adds Bloodfest to the main combo when ammo is 0.", "当晶壤为0时在主连击中插入血壤." },
        { "Lightning Shot Uptime", "闪雷弹激活" },
        { "Adds Lightning Shot to the main combo when you are out of range.", "当自身与所选目标在近战距离外时，插入闪雷弹." },
        { "No Mercy AoE Option", "无情整合至AoE连击" },
        { "Adds No Mercy to AoE combo when it's available.", "当可用时在AoE连击中插入无情." },
        { "Bow Shock on AoE Feature", "弓形冲波整合到AOE连击" },
        { "Adds Bow Shock onto the AoE combo when it's off cooldown.", "当可用时在AoE连击中插入弓形冲波." },
        { "No Mercy on Main Combo", "无情整合到主连击" },
        { "Adds No Mercy to the main combo when at full ammo.", "当晶壤积累满时在主连击中插入无情." },
        { "Gnashing Fang Starter", "烈牙启动" },
        { "Begins Gnashing Fang to the main combo.", "先从烈牙开始倾泻晶壤." },
        { "Bow Shock on Main Combo", "弓形冲波整合到主连击" },
        { "Adds Bow Shock to the main combo.", "弓形冲波整合到主连击" },
        { "Sonic Break on Main Combo", "音速破整合到主连击" },
        { "Adds Sonic Break to the main combo.", "在主连击中插入音速破." },
        { "Sonic Break/Bow Shock on No Mercy", "无情技能整合 设置" },
        { "Adds Sonic Break and Bow Shock to No Mercy when it is on cooldown.", "当无情在冷却中替换其为音速破与弓形冲波." },
        { "Burst Strike on Main Combo", "爆发击整合到主连击" },
        { "Adds Burst Strike and Hypervelocity (when available) to the main combo when under No Mercy and Gnashing Fang is over.", "当在无情状态下且已使用过烈牙后，在主连击中插入爆发击和超高速(若可用)." },
        { "Bloodfest AoE Option", "血壤整合至AoE连击" },
        { "Adds Bloodfest to AoE combo when it's available. Will dump Ammo through Fated Circle to prepare for Bloodfest.", "当可用时在AoE连击中插入血壤.会先使用命运之环以确保使用血壤不会使晶壤溢出." },
        { "Double Down AoE Option", "倍攻整合至AoE连击" },
        { "Adds Double Down to AoE combo when it's available and there is 2 or more ammo.", "当倍攻可用且晶壤有2个以上时在AoE连击中插入倍攻." },
        { "Double Down on Burst Strike Feature", "爆发击整合 设置" },
        { "Adds Double Down to Burst Strike when under No Mercy and ammo is above 2.", "无情状态下且有三颗晶壤时，用倍攻代替爆发击" },
        { "Melee Rough Divide Option", "近战时插入粗分斩 设置" },
        { "Uses Rough Divide when under No Mercy, burst cooldowns when available, and in the target ring (1 yalm).\nWill use as many stacks as selected in the above slider.", "当自身处于无情状态下，且在目标圈内时，插入粗分斩..\n会使用选择的所有粗分斩层数." },
        { "Aurora Protection Feature", "极光保护机制" },
        { "Turns Aurora into Nascent Flash if Aurora's effect is on the player.", "当选择目标身上已有极光状态时，将极光变为原初的勇猛.\n 译者注:有bug，是自身身上有极光，将极光变为原初的勇猛" },
        { "Danger Zone on AoE Feature", "危险领域整合至AoE连击" },
        { "Adds Danger Zone to the AoE combo when it's off cooldown.", "当可用时在AoE连击中插入危险领域." },
        { "Sonic Break on AoE Feature", "音速破整合至AoE连击" },
        { "Adds Sonic Break to the AoE combo when it's off cooldown.", "当可用时在AoE连击中插入音速破." },

        #endregion

        #region MACHINIST

        { "(Heated) Shot Combo Feature", "(热)狙击弹连击一键整合" },
        { "Replace either form of Clean Shot with its combo chain.", "替换热弹为连击循环中的任一技能" },
        { "Overdrive Feature", "超档炮塔/人偶替换设置" },
        { "Replace Rook Autoturret and Automaton Queen with Overdrive while active.", "在技能可用时，将车式浮空炮塔和后式自走人偶转换为超档车式炮塔和超档自走人偶" },
        { "Gauss Round / Ricochet Feature", "虹吸弹 / 弹射设置" },
        { "Replace Gauss Round and Ricochet with one or the other depending on which has more charges.", "将虹吸弹和弹射替换为一个或其他需要更多充电电能的技能." },
        { "Drill / Air Anchor (Hot Shot) Feature", "钻头与空气锚/热弹按键整合" },
        { "Replace Drill and Air Anchor (Hot Shot) with one or the other (or Chain Saw) depending on which is on cooldown.", "钻头、空气锚 (热弹)、回转飞锯根据cd时间互相替换." },
        { "Drill/Air/Chain Saw on Main Combo Feature", "钻头/空气锚/回转飞锯整合 设置" },
        { "Air Anchor followed by Drill is added onto main combo if you use Reassemble.\nIf Air Anchor is on cooldown and you use Reassemble, Chain Saw will be added to main combo instead.", "当使用整备后，将先插入空气锚，后插入钻头至主连击中.\n若此时空气锚正在冷却中，则先插入回转飞锯." },
        { "Single Button Heat Blast", "热冲击一键整合" },
        { "Switches Heat Blast to Hypercharge.", "整合超荷至热冲击." },
        { "Single Button Auto Crossbow", "自动弩一键整合" },
        { "Switches Auto Crossbow to Hypercharge and weaves gauss/rico.", "整合超荷至自动弩，同时在连击中插入虹吸弹/弹射." },
        { "Alternate Drill/Air Feature on Main Combo", "交替插入钻头/空气锚/热弹被添加到连击循环中" },
        { "Drill/Air/Hotshot Feature is added onto main combo (Note: It will add them onto main combo ONLY if you are under Reassemble Buff\nOr Reasemble is on CD (will do nothing if Reassemble is OFF CD)", "将钻头/空气锚/热弹插入主连击循环中(注意:只有当你在整备Buff中\n或整备正在冷却中该选项才会生效。(若整备可用但却没有使用时则不会插入这些技能)" },
        { "Single Button HeatBlast On Main Combo Option", "添加热冲击至主连击循环" },
        { "Adds Single Button Heatblast onto the main combo when the option is enabled.", "当选项可用时，将单体开关热冲击添加到主连击循环中" },
        { "Battery Overcap Option", "电量溢出后自动替换炮台/人偶" },
        { "Overcharge protection for your Battery, If you are at 100 battery charge rook/queen will be added to your (Heated) Shot Combo.", "为您提供电能过量时的电能保护，如果您的电量达到100，则会在您的过热循环中加入炮台/女王。" },
        { "Battery AoE Overcap Option", "防止AoE连击电量溢出 设置" },
        { "Adds overcharge protection to Spread Shot/Scattergun.", "为散射/霰弹枪增加了过电量保护。." },
        { "Gauss Round Ricochet on AoE Feature", "虹吸弹/弹射整合 设置" },
        { "Adds Gauss Round/Ricochet to the AoE combo during Hypercharge.", "在超荷过程中为AOE连击循环增加虹吸弹/弹射" },
        { "Always Gauss Round/Ricochet on AoE Option", "不限制虹吸弹/弹射的插入时机" },
        { "Adds Gauss Round/Ricochet to the AoE combo outside of Hypercharge windows.", "在超荷窗口之外的AOE连击循环中也会插入可用的虹吸弹/弹射." },
        { "Ricochet & Gauss Round Feature", "弹射 & 虹吸弹 设置" },
        { "Adds Ricochet and Gauss Round to main combo. Will use all charges.", "将弹射和虹吸弹添加到主连击循环中.这将使用所有电能." },
        { "Ricochet & Gauss Round overcap protection option", "弹射 & 虹吸弹防溢出 设置" },
        { "Adds Ricochet and Gauss Round to main combo. Will leave 1 charge of each.", "将弹射和虹吸弹添加到主连击循环中.将各留下一个电能." },
        { "Barrel Stabilizer drift protection feature", "枪管加热避免延后浪费 设置" },
        { "Adds Barrel Stabilizer onto the main combo if heat is between 5-20.", "如果热量在5-20之间，则将枪管加热添加到主连击循环中。" },
        { "Wildfire Feature", "野火 设置" },
        { "Adds Wildfire to the Single Button Heat Blast Feature if Wildfire is off cooldown and you have enough heat for Hypercharge then Hypercharge will be replaced with Wildfire.\nAlso weaves Ricochet/Gauss Round on Heat Blast when necessary.", "在热冲击一键连击中加入野火，如果你有足够的热量用于超荷而野火的cd已经转好，那么超荷将被野火取代。\n同样也会在连击中插入可用的弹射/虹吸弹." },
        { "BioBlaster Feature", "毒菌冲击整合 设置" },
        { "Adds Bioblaster to the Spreadshot feature", "在散射的功能中添加毒菌冲击" },
        { "Barrel Feature", "枪管加热 设置" },
        { "Adds Barrel Stabalizer to Single Button Heat Blast and Single Button Auto Crossbow Features when below 50 heat and is off cooldown", "当热能低于50且处于冷却状态时，将枪管充能添加到热冲击和自动弩一键连中。" },
        { "Simple Machinist", "一键简单机工循环" },
        { "Single button single target machinist, including buffs and overcap protections.\nConflicts with other single target toggles!\nMade to work optimally with a 2.5 GCD.\nThe use of latency mitigation tools is recommended due to XIV's network handling.", "整合机工士单体输出技能至(热)分裂弹，包括各种buff技能和防溢出功能.\n与其他所有单体目标输出设置均有冲突!\n该选项的推荐GCD为2.5秒.\n建议同时使用缓解游戏网络延迟的手段或工具." },
        { "Uses interrupt during simple machinist rotation, if applicable.", "如果适用的话，在机工的简单循环中使用打断技能." },
        { "Simple Gadget", "简单的机工召唤" },
        { "Adds Queen or Rook uses to the feature, based on your current level.\nTry to use Queen at optimal intervals between :55 to :05 windows.", "根据你目前的等级，增加超档车式炮塔和超档自走人偶的技能.\n试着在:55到:05窗口之间的最佳间隔期使用超档车式炮塔和超档自走人偶." },
        { "Simple Assembling", "简单的组装" },
        { "Pairs reassemble uses with the following skills.\nBefore acquiring Drill it will be used with Clean Shot.", "整备将与下面的技能一起使用.\n在获得钻头之前，它将使用与狙击弹共用." },
        { "Simple Gauss Ricochet", "简单的虹吸弹和弹射" },
        { "Adds Gauss Round and Ricochet uses to the feature.", "将虹吸弹和弹射加入到循环中." },
        { "Simple Wildcharge", "简单的野火和超荷" },
        { "Adds Hypercharge and Wildfire uses to the feature.\nIt respects the 8 second rule of Drill, AirAnchor and Chain Saw.", "将超荷和野火插入一键连击中.\n该选项遵循钻头、空气锚和回转飞锯的8秒原则(?)." },
        { "Simple Stabilizer", "枪管加热" },
        { "Adds Barrel Stabilizer to the feature.\nWhen heat < 50 and Wildfire is off CD or about to come off CD.", "将枪管加热加入到循环中.\n当热量小于50并且野火不在CD或者野火CD快要转好了时使用." },
        { "Hypercharge", "超荷" },
        { "Adds hypercharge to the AoE.", "将超荷加入到AOE循环中." },
        { "Simple Machinist AoE", "机工简易AoE一键连击" },
        { "Spread Shot turns into Scattergun when Lv82 or higher. Both turn into Auto Crossbow when Overheated.\nBioblaster is used first whenever it is off cooldown.", "整合AoE技能至散射，当到达82级以上时将整合至霰弹枪.在过热状态下会替换为自动弩.\n当毒菌冲击未在冷却时总会首先替换为毒菌冲击." },
        { "Drill", "钻头" },
        { "Use Reassemble with Drill when available.", "当可以的时候整备与钻头一起使用." },
        { "Air Anchor", "空气锚" },
        { "Use Reassemble with Air Anchor when available.", "当可以的时候整备与空气锚一起使用." },
        { "Chain Saw", "回转飞锯" },
        { "Use Reassemble with Chain Saw when available.", "当可以的时候整备与回转飞锯一起使用." },
        { "Only use Drill...", "只使用钻头..." },
        { "...when you have max charges of reassemble.", "...当你整备充能是最大值时." },
        { "Only use Air Anchor...", "只使用空气锚..." },
        { "Only use Chain Saw...", "只使用回转飞锯..." },
        { "Wildfire Only", "在特定时机插入" },
        { "Only use it to prepare for Wildfire use.", "仅为野火使用枪管加热." },
        { "High Ping Mode", "高延迟模式" },
        { "A high ping friendly mode.\nIt limits the uses of Gauss/Ricochet inside Hypercharge windows.\nThere will be a little dps loss.", "一种在高延迟网络条件下表现较好的模式.\n它将限制在过热状态下使用弹射/虹吸弹的次数.\n这会丢失一些dps." },

        #endregion

        #region MONK

        { "Arm of the Destroyer Combo", "破坏神冲 Combo" },
        { "Replaces Arm Of The Destroyer with its combo chain.", "将破坏神冲替换为其 Combo 链" },
        { "Dragon Kick --> Bootshine Feature", "双龙脚 --> 连击 功能" },
        { "Replaces Dragon Kick with Bootshine if both a form and Leaden Fist are up.", "在拥有连击效果提高时使用连击替换双龙脚。" },
        { "Twin Snakes Feature", "双掌打特性" },
        { "Replaces True Strike with Twin Snakes if Disciplined Fist is not applied or is less than 6 seconds from falling off.", "如果你功力buff不足六秒，使用双掌打替换正拳。" },
        { "Basic Rotation", "基础循环" },
        { "Basic Monk Combo on one button", "整合一键基础循环" },
        { "Perfect Balance Feature", "震脚特性" },
        { "Perfect Balance becomes Masterful Blitz while you have 3 Beast Chakra.", "如果你拥有三档脉轮，那么使用必杀技替换震脚。" },
        { "Bootshine Balance Feature", "连击平衡特性" },
        { "Replaces Dragon Kick with Masterful Blitz if you have 3 Beast Chakra.", "如果你拥有三档脉轮，那么使用必杀技替换双龙脚。" },
        { "Howling Fist/Meditation Feature", "空鸣拳/万象斗气圈特性" },
        { "Replaces Howling Fist/Enlightenment with Meditation when the Fifth Chakra is not open.", "当你的斗气满时使用空鸣拳/万象斗气圈替换掉破坏神冲" },
        { "Bootshine Combo", "连击连击" },
        { "Replace Bootshine with its combo chain. \nIf all sub options are selected will turn into a full one button rotation (Simple Monk). Slider values can be used to control Disciplined Fist + Demolish uptime.", "将连击替换为其 Combo 链。\n如果启用所有子选项，将变成武僧的一键循环。滑块的值可以用来控制功力 buff 和破碎拳 dot 的正常运行时间。" },
        { "Perfect Balance Feature Plus", "震脚特性+" },
        { "All of the (optimal?) Blitz combos on Masterful Blitz when Perfect Balance is active", "当震脚激活时，使用可用的技能替换必杀技。" },
        { "Masterful Blitz on Main Combo", "必杀技加入循环" },
        { "Adds Masterful Blitz to the main combo", "将必杀技加入循环" },
        { "Masterful Blitz to AoE Combo", "必杀技加入AOE循环" },
        { "Adds Masterful Blitz to the AoE combo.", "将必杀技添加进AoE循环。" },
        { "Riddle of Fire/Brotherhood Feature", "红莲极意/义结金兰特性" },
        { "Replaces Riddle of Fire with Brotherhood when Riddle of Fire is on cooldown.", "如果红莲极意进入冷却，那么使用义结金兰替换红莲极意。" },
        { "Adds various CDs to the main combo when under Riddle of Fire or when Riddle of Fire is on cooldown.", "当红莲极意进入冷却时，将各种能力技整合进连击。" },
        { "Riddle of Wind on Main Combo", "疾风极意整合" },
        { "Adds Riddle of Wind to the main combo.", "将疾风极意整合进连击。" },
        { "Perfect Balance on Main Combo", "震脚整合" },
        { "Adds Perfect Balance to the main combo.", "将震脚整合进连击。" },
        { "Brotherhood on Main Combo", "义结金兰整合" },
        { "Adds Brotherhood to the main combo.", "将义结金兰整合进连击。" },
        { "Meditation on Main Combo", "斗气整合" },
        { "Adds Meditation to the main combo.", "将斗气整合进连击。" },
        { "Lunar Solar Opener", "阴阳开场" },
        { "Start with the Lunar Solar Opener on the main combo. Requires level 68 for Riddle of Fire.\nA 1.93/1.94 GCD is highly recommended.", "在一键循环中使用阴阳开场。使用红莲极意需要在 68 级以上。\n强烈建议使用 1.93/1.94 GCD 配装。" },
        { "CDs on AoE Combo", "AoE 循环中的能力技使用" },
        { "Adds various CDs to the AoE combo when under Riddle of Fire or when Riddle of Fire is on cooldown.", "AoE循环下，在红莲极意中插入各种能力技。" },
        { "Riddle of Wind on AoE Combo", "疾风极意应用于 AoE 循环" },
        { "Adds Riddle of Wind to the AoE combo.", "将疾风极意添加进一键 AoE 循环。" },
        { "Perfect Balance on AoE Combo", "震脚应用于 AoE 循环" },
        { "Adds Perfect Balance to the AoE combo.", "将震脚添加进一键 AoE 循环" },
        { "Brotherhood on AoE Combo", "义结金兰应用于 AoE 循环" },
        { "Adds Brotherhood to the AoE combo.", "将义结金兰添加进一键 AoE 循环。" },
        { "Meditation on AoE Combo", "斗气应用于 AoE 循环" },
        { "Adds Meditation to the AoE combo.", "将斗气添加进一键 AoE 循环。" },
        { "Thunderclap on AoE Combo", "轻身步法应用于 AoE 循环" },
        { "Adds Thunderclap when out of combat to the AoE combo.", "将轻身步法添加进一键 AoE 循环" },
        { "Thunderclap on Main Combo", "轻身步法应用于单体循环" },
        { "Adds Thunderclap when out of combat to the main combo.", "将轻身步法添加进一键单体循环。" },

        #endregion

#region NINJA

        { "Simple Mode - Single Target", "下忍模式 - 单目标" },
        { "Replaces Spinning Edge with a one-button full single target rotation.\nThis is the ideal option for newcomers to the job.", "将双刃旋替换为一键单目标连击。\n一键输出，下忍的理想之选。" },
        { "Balance Opener Feature", "起手爆发" },
        { "Starts with the Balance opener.\nDoes pre-pull first, if you enter combat before hiding the opener will fail.\nLikewise, moving during TCJ will cause the opener to fail too.\nRequires you to be out of combat with majority of your cooldowns available for it to work.", "一键循环从起手爆发开始。\n做好战斗前准备，如果你在隐遁前进入了战斗，那么起手爆发将会失败。\n同样的，如果你在TCJ期间移动，也会导致起手爆发失败。\n失败后需要你退出战斗，并且所有技能冷却完毕。" },
        { "Simple Mode - AoE", "下忍模式 - AoE" },
        { "Turns Death Blossom into a one-button full AoE rotation.", "将血雨飞花替换为一键AOE连击" },
        { "Advanced Mode - Single Target", "上忍模式 - 单目标" },
        { "Replace Spinning Edge with a one-button full single target rotation.\nThese features are ideal if you want to customize the rotation.", "将双刃旋替换为一键单目标连击。\n自定义循环，上忍的理想之选。" },
        { "Throwing Dagger Uptime Feature", "飞刀 开关" },
        { "Adds Throwing Dagger to Advanced Mode if out of melee range.", "如果在近战攻击距离以外，将飞刀加入循环。" },
        { "Mug Feature", "夺取 开关 " },
        { "Adds Mug to Advanced Mode.", "将 夺取 加入一键循环" },
        { "Align Mug with Trick Attack", "对齐 夺取 和 攻其不备" },
        { "Only uses Mug whilst the target has Trick Attack, otherwise will use on cooldown.", "当目标存在 攻其不备 时使用 夺取，或在冷却完毕时使用" },
        { "Use Mug before Trick Attack", "在 攻其不备 前使用 夺取" },
        { "Aligns Mug with Trick Attack but weaves it at least 1 GCD before Trick Attack.", "对齐 夺取 与 攻其不备，但 夺取 至少早 攻其不备 1个GCD" },
        { "Trick Attack Feature", "攻其不备 开关" },
        { "Adds Trick Attack to Advanced Mode.", "将 攻其不备 加入一键循环" },
        { "Save Cooldowns Before Trick Attack", "在 攻其不备 前保留CD" },
        { "Stops using abilities with longer cooldowns up to 15 seconds before Trick Attack comes off cooldown.", "在 攻其不备 冷却结束前，停止使用CD超过15s的能力技" },
        { "Delayed Trick Attack Feature", "延后 攻其不备" },
        { "Waits at least 8 seconds into combat before using Trick Attack.", "进入战斗至少8s后再使用 攻其不备" },
        { "Ninjitsu Feature", "忍术 开关" },
        { "Adds Ninjitsus to Advanced Mode.", "将 忍术 加入一键循环" },
        { "Hold 1 Charge", "保留1次忍术" },
        { "Prevent using both charges of Mudra.", "防止一次性使用两个忍术" },
        { "Use Fuma Shuriken", "使用 风魔手里剑" },
        { "Spends Mudra charges on Fuma Shuriken (only before Raiton is available).", "使用忍术释放 风魔手里剑（仅在雷遁获取前生效）" },
        { "Use Raiton", "使用 雷遁" },
        { "Spends Mudra charges on Raiton.", "使用忍术释放 雷遁" },
        { "Use Suiton", "使用 水遁" },
        { "Spends Mudra charges on Suiton.", "使用忍术释放 水遁" },
        { "Use Huton", "使用 风遁" },
        { "Spends Mudra charges on Huton.", "使用忍术释放 风遁" },
        { "Assassinate/Dream Within a Dream Feature", "断绝/梦幻三段 开关" },
        { "Adds Assassinate and Dream Within a Dream to Advanced Mode.", "将 断绝/梦幻三段 加入一键循环" },
        { "Kassatsu Feature", "生杀予夺 开关" },
        { "Adds Kassatsu to Advanced Mode.", "将 生杀予夺 加入一键循环" },
        { "Use Hyosho Ranryu", "使用 冰晶乱流之术" },
        { "Spends Kassatsu on Hyosho Ranryu.", "使用 生杀予夺 释放 冰晶乱流之术" },
        { "Armor Crush Feature", "强甲破点突 开关" },
        { "Adds Armor Crush to Advanced Mode.", "将 强甲破点突 加入一键循环" },
        { "Huraijin Feature", "风来刃 开关" },
        { "Adds Huraijin to Advanced Mode.", "将 风来刃 加入一键循环" },
        { "Bhavacakra Feature", "六道轮回 开关" },
        { "Adds Bhavacakra to Advanced Mode.", " 将 六道轮回 加入一键循环" },
        { "Ten Chi Jin Feature", "天地人 开关" },
        { "Adds Ten Chi Jin (the cooldown) to Advanced Mode.", "将 天地人 加入一键循环" },
        { "Meisui Feature", "命水 开关" },
        { "Adds Meisui to Advanced Mode.", "将 命水 加入一键循环" },
        { "Bunshin Feature", "分身之术 开关" },
        { "Adds Bunshin to Advanced Mode.", "将 分身之术 加入一键循环" },
        { "Phantom Kamaitachi Feature", "残影镰鼬 开关" },
        { "Adds Phantom Kamaitachi to Advanced Mode.", "将 残影镰鼬 加入一键循环" },
        { "Raiju Feature", "雷兽 开关" },
        { "Adds Fleeting/Forked Raiju to Advanced Mode.", "将 月影雷兽爪/月影雷兽牙 加入一键循环" },
        { "Forked Raiju Gapcloser Feature", "月影雷兽爪 开关" },
        { "Uses Forked Raiju when out of range.", "当处于攻击范围外时，使用 月影雷兽爪" },
        { "True North Feature", "真北 开关" },
        { "Adds True North to Advanced Mode.", "将 真北 加入一键循环" },
        { "Use Before Armor Crush Only", "仅在 强甲破点突 前使用" },
        { "Only triggers the use of True North before Armor Crush.", "仅在 强甲破点突 前使用 真北" },
        { "Second Wind Feature", "内丹 开关" },
        { "Adds Second Wind to Advanced Mode.", "将 内丹 加入一键循环" },
        { "Shade Shift Feature", "残影 开关" },
        { "Adds Shade Shift to Advanced Mode.", "将 残影 加入一键循环" },
        { "Bloodbath Feature", "浴血 开关" },
        { "Adds Bloodbath to Advanced Mode.", "将 浴血 加入一键循环" },
        { "Advanced Mode - AoE", "上忍模式 - AoE" },
        { "Replace Death Blossom with a one-button full AoE rotation.\nThese features are ideal if you want to customize the rotation.", "将血雨飞花替换为一键群体连击。\n自定义循环，上忍的理想之选。" },
        { "Adds Assassinate/Dream Within a Dream to Advanced Mode.", "将 断绝/梦幻三段 加入一键循环" },
        { "Ninjitsus Feature", "忍术 开关" },
        { "Use Katon", "使用 火遁" },
        { "Spends Mudra charges on Katon.", "使用忍术释放 火遁" },
        { "Use Doton", "使用 土遁" },
        { "Spends Mudra charges on Doton.", "使用忍术释放 土遁" },
        { "Goka Mekkyaku Feature", "劫火灭却之术 开关" },
        { "Adds Goka Mekkyaku to Advanced Mode.", "将 劫火灭却之术 加入一键循环" },
        { "Hellfrog Medium Feature", "通灵之术·大虾蟆 开关" },
        { "Adds Hellfrog Medium to Advanced Mode.", "将 通灵之术·大虾蟆 加入一键循环" },
        { "Armor Crush Combo", "强甲破点突 Combo" },
        { "Replace Armor Crush with its combo chain.", "使用 强甲破点突 做为连击的起始技." },
        { "Kassatsu to Trick", "生杀予夺 替换为 攻其不备" },
        { "Replaces Kassatsu with Trick Attack while Suiton or Hidden is up.\nCooldown tracking plugin recommended.", "隐遁状态下或发动水遁之术后，使用 攻其不备 替换 生杀予夺.\n推荐同时使用冷却CD监视插件." },
        { "Ten Chi Jin to Meisui", "天地人 替换为 命水" },
        { "Replaces Ten Chi Jin (the move) with Meisui while Suiton is up.\nCooldown tracking plugin recommended.", "发动水遁之术 后，使用 命水 替换 天地人.\n推荐同时使用冷却CD监视插件." },
        { "Kassatsu Chi/Jin Feature", "生杀予夺 地之印/人之印 开关" },
        { "Replaces Chi with Jin while Kassatsu is up if you have Enhanced Kassatsu.", "发动 生杀予夺 后，使用 人之印 替换 地之印." },
        { "Hide to Mug/Trick Attack", "隐遁 替换为 夺取/攻其不备" },
        { "Replaces Hide with Mug while in combat and Trick Attack whilst Hidden.", "战斗状态下用 夺取 替换 隐遁，隐遁状态下用 攻其不备 替换 隐遁" },
        { "Aeolian to Ninjutsu Feature", "旋风刃 替换为 忍术 " },
        { "Replaces Aeolian Edge (combo) with Ninjutsu if any Mudra are used.", "此功能效果无效：当使用任意结印时，将 旋风刃 Combo 替换为 忍术。" },
        { "Huraijin / Raiju Feature", "风来刃 替换为 雷兽" },
        { "Replaces Huraijin with Forked and Fleeting Raiju when available.", "当 月影雷兽爪 和 月影雷兽牙 可以使用时，替换 风来刃." },
        { "Huraijin / Raiju Feature Option 1", "风来刃 替换为 月影雷兽牙" },
        { "Replaces Huraijin with Fleeting Raiju when available.", "当 月影雷兽牙 可以使用时，替换 风来刃." },
        { "Huraijin / Raiju Feature Option 2", "风来刃 替换为 月影雷兽爪" },
        { "Replaces Huraijin with Forked Raiju when available.", "当 月影雷兽爪 可以使用时，替换 风来刃." },
        { "Simple Mudras", "简化结印" },
        { "Simplify the mudra casting to avoid failing.", "简化忍术结印的操作." },
        { "Turns Ten Chi Jin (the move) into Ten, Chi, and Jin.", "发动 天地人 之后，将其依序变为 天之印, 地之印, 人之印." },
        { "Huraijin / Armor Crush Combo", "风来刃 替换为 强甲破点突 Combo" },
        { "Replace Huraijin with Armor Crush after using Gust Slash.", "使用 绝风 后，用 强甲破点突 替换 风来刃" },

        #endregion

        #region PALADIN

        { "Goring Blade Combo", "沥血剑 Combo" },
        { "Replace Goring Blade with its combo chain.", "将 沥血剑 设为沥血连." },
        { "Royal Authority Combo", "王权剑 Combo" },
        { "All-in-one main combo adds Royal Authority/Rage of Halone.\nToggle all sub-options on to make this a 1 button rotation", "将 王权剑/战女神之怒. 设为一整套连招的起始.\n选中下列所有子选项激活一键连招." },
        { "Atonement drop Feature", "赎罪剑层数处理" },
        { "Will drop the last Atonement charge right before FoF comes back off cooldown.\nPlease note that this assumes you use both FoF and Req according to the full FoF opener and standard loop\nRequires a GCD speed of 2.40-2.45s", "在 战逃反应 冷却结束前使用最后一层 赎罪剑 。\n请注意：这是假设你是以完整的 战逃反应 起手，并以标准循环使用 战逃反应 以及 安魂祈祷 。\n要求GCD为2.40-2.45s。" },
        { "Prominence Combo", "日珥斩 Combo" },
        { "Replace Prominence with its combo chain.", "将 日珥斩 设为AOE连击的起始." },
        { "Holy Spirit Feature", "圣灵 Feature" },
        { "Replaces Royal Authority combo with Holy Spirit if you don't have the Fight or Flight buff", "等待翻译" },
        { "Confiteor Combo Feature", "悔罪 Combo Feature" },
        { "Replace Holy Spirit/Circle with Confiteor when Requiescat is up and MP is under 2000 or only one stack remains \nand adds Faith/Truth/Valor Combo after Confiteor.", "当MP高于2000，且仅剩一层安魂祈祷档数时，将 圣灵/圣环 替换为 悔罪. \n释放 悔罪后，依次变为 信念之剑/真理之剑/英勇之剑." },
        { "Scornful Spirits Feature", "深奥之灵 厄运流转同一键位" },
        { "Replace Spirits Within and Circle of Scorn with whichever is available soonest.", "将 深奥之灵 替换为 厄运流转 并优先显示最快冷却的那个." },
        { "Goring Blade Feature", "沥血剑 Feature" },
        { "Insert Goring Blade into the main combo when appropriate.", "在合适的时机插入沥血连." },
        { "Standalone Holy Spirit Feature", "安魂圣灵单体连" },
        { "Replaces Holy Spirit with Confiteor and Confiteor combo", "自身附加 安魂祈祷 状态时，将 圣灵 设为圣灵-悔罪-信念-真理-英勇连." },
        { "Standalone Holy Circle Feature", "安魂圣环aoe连" },
        { "Replaces Holy Circle with Confiteor and Confiteor combo", "自身附加 安魂祈祷 状态时，将 圣灵 设为圣环-悔罪-信念-真理-英勇连." },
        { "Intervene Feature", "调停加入连击" },
        { "Adds Intervene onto main combo whenever it's available.", "启用后，自动将 调停 加入连击" },
        { "Uptime Feature", "等待翻译" },
        { "Replace main combo with Shield Lob/Holy Spirit when out of range.\nWill use Holy Spirit if you're not moving or have the Requiescat buff\nOtherwise will use Shield lob when moving and if you don't have the Requiescat buff.", "超出范围时用投盾/圣灵代替主要组合。\n如果你不移动或者处于安魂祈祷状态，就会使用圣灵。\n否则，当你移动时和未处于安魂祈祷状态，将使用投盾。" },
        { "Fight or Flight", "战逃反应" },
        { "Adds Fight or Flight onto the main combo (Testing).", "将战逃反应加入循环（测试功能）" },
        { "Requiescat Feature", "安魂祈祷 功能" },
        { "Requiescat gets added onto the main combo when the Fight or Flight buff has 17 seconds remaining or less.", "将在战逃反应剩余时间不足17s时，将安魂祈祷加入连击" },
        { "Fight or Flight Feature", "战逃反应 功能" },
        { "Adds Fight or Flight onto the main combo with a delayed weave.", "将战逃反应稍作延迟插入循环，以覆盖更多的物理伤害。" },
        { "Holy Circle Feature", "圣环 功能" },
        { "Replaces AoE combo with Holy Circle when Requiescat is active.", "自身附加安魂祈祷状态时，将 圣环 插入到AOE连击中。" },
        { "AoE Confiteor Feature", "AoE 悔罪 功能" },
        { "Replaces AoE combo with Confiteor when Requiescat is active and appropiate.", "当自身拥有安魂祈祷状态时，在合适时机将悔罪加入AOE循环" },
        { "AoE Requiescat Feature", "AoE 安魂祈祷 功能" },
        { "Replaces AoE combo with Requiescat when it's off cooldown.", "当安魂祈祷冷却完毕后，在AOE循环中插入安魂祈祷" },
        { "Expiacion and Circle of Scorn Feature", "厄运流转加入连击" },
        { "Adds Expiacion and Circle of Scorn onto the main combo during weave windows", "在连击窗口期插入 偿赎剑 和 厄运流转." },
        { "AoE Expiacion / Circle of Scorn Feature", "AoE 偿赎剑 / 厄运流转 特质" },
        { "Adds Expiacion and Circle of Scorn onto the main AoE combo during weave windows", "在AOE连击窗口期插入 偿赎剑 和 厄运流转" },
        { "Atonement Feature", "赎罪剑特质" },
        { "Replace Royal Authority with Atonement when under the effect of Sword Oath.", "在激活忠义之剑的状态下，将 王权剑 变为 赎罪剑." },
        { "Melee Intervene Option", "调停选项" },
        { "Uses Intervene when under Fight or Flight and in the target ring (1 yalm).\nWill use as many stacks as selected in the above slider.", "在战逃反应下并在目标圈内（1米）时使用调停。\n将使用上述滑块中选择的层数。" },
        { "Expiacion and Circle of Scorn Option", "偿赎剑和厄运流转选项" },
        { "Uses Circle of Scorn and Expiacion when under Fight or Flight or when Fight or Flight is on cooldown", "在战逃反应下或战逃反应冷却时，使用厄运流转和偿赎剑。" },
        { "FoF Opener Feature", "战逃反应起手选项" },
        { "Adds the FoF opener to the main combo.\nWill execute the full FoF opener by default, but if FoF is activated 18s pre-pull, the 18s pre-pull opener will be executed. Requires Lv68.", "将战逃反应起手爆发添加到主连击中。\n默认情况下会执行完整的战逃反应起手爆发，但如果在开战18s倒计时前使用了战逃反应，择使用18s倒计时的开场。\n需要Lv68。" },
        { "Intervene Option", "调停选项" },
        { "Adds Intervene to the Fight or Flight opener.", "将调停加入战逃反应的起手爆发" },

        #endregion

        #region REAPER

        { "Positional Preference", "身位喜好" },
        { "Choose positional order for all positional related features.\nSupports turning Slice/Shadow of Death into all positionals or Slice and Shadow of Death being two separate positionals.", "为身位相关的功能选择身位喜好,支持为切割和死亡之影统一或分开设置" },

        #region Single Target (Slice) Combo Section

        { "Slice Combo Feature", "切割连击" },
        { "Replace Slice with its combo chain.\nIf all sub options are toggled will turn into a full one button rotation (Advanced Reaper)", "将切割替换为切割连击。如果启用了所有子选项将变为一键循环(Advanced 镰刀)" },
        { "Soul Slice Option", "灵魂切割设置" },
        { "Adds Soul Slice to Slice Combo when Soul Gauge is 50 or less and when current target is afflicted with Death's Design.", "当魂衣值低于50点，并且目标拥有死亡烙印Debuff时，将灵魂切割加入连击。" },
        { "Shadow Of Death Option", "死亡之影设置" },
        { "Adds Shadow of Death to Slice Combo if Death's Design is not present on current target, or is about to expire.", "当目标没有或者死亡烙印Debuff即将到期时，将死亡之影加入连击。" },
        { "Double SoD Enshroud Option", "夜游魂衣双死亡之影设置" },
        { "Uses Shadow of Death twice during the first of the two Enshroud Bursts during the 2-minute windows (Double Enshroud Burst).", "前两次的夜游魂衣的2分钟爆发期时使用两次死亡之影(双夜游魂衣爆发)。" },
        { "Stun Option", "下踢设置" },
        { "Adds Leg Sweep to main combo when target is performing an interruptible cast.", "当目标的施法可以被打断时，将下踢加入连击。" },
        { "Combo Heals Option", "回复设置" },
        { "Adds Bloodbath and Second Wind to the combo at 65%% and 40%% HP, respectively.", "在血量低于65%和40%时，将浴血和内丹加入连击。" },
        { "Ranged Filler Option", "距离设置" },
        { "Replaces the combo chain with Harpe (or Harvest Moon, if available) when outside of melee range. Will not override Communio.", "当超出近战范围时，使用勾刃（当可用时，使用收获月）替换切割。不会替换掉团契。" },
        { "Enshroud Option", "夜游魂衣设置" },
        { "Adds Enshroud to the combo when at 50 Shroud or greater and when current target is afflicted with Death's Design.", "当目标有死亡烙印Debuff并且魂衣大于等于50时将夜游魂衣加入连击。" },
        { "Enshroud Burst (Double Enshroud) Option", "夜游魂衣爆发(双夜游魂衣)设置" },
        { "Uses Enshroud at 50 Shroud during Arcane Circle (mimics the 2-minute Double Enshroud window), but will pool Shroud outside of burst windows.\nBelow level 88, will use Enshroud at 50 gauge.", "在神秘环持续期间会在50魂衣时使用夜游魂衣(模拟2分钟双夜游魂衣爆发期)，但是会在爆发期外囤积魂衣。\n低于88级时，会在50魂衣时使用夜游魂衣。" },
        { "Lemure's Slice Option", "夜游魂切割设置" },
        { "Adds Lemure's Slice to the combo when there are 2 Void Shroud charges.", "当拥有2层虚无魂时将夜游魂切割加入连击。" },
        { "Communio Finisher Option", "团契设置" },
        { "Adds Communio to the combo when there is 1 charge of Lemure Shroud left.", "当剩余1层夜游魂时将团契加入连击。" },
        { "Arcane Circle Option", "神秘环设置" },
        { "Adds Arcane Circle to the combo when available and when current target is afflicted with Death's Design.", "当神秘环可用并且目标有死亡烙印Debuff时将其加入连击。" },
        { "Plentiful Harvest Option", "大丰收设置" },
        { "Adds Plentiful Harvest to the combo when available.", "当大丰收可用时将其加入连击。" },
        { "Gibbet and Gallows Option", "绞决/缢杀设置" },
        { "Adds Gibbet and Gallows to the combo when current target is afflicted with Death's Design.\nWill use Void/Cross Reaping during Enshroud.", "当目标有死亡烙印Debuff时将绞决/缢杀加入连击。\n在夜游魂状态下会使用虚无收割/交错收割。" },
        { "Gibbet and Gallows on SoD Option", "绞决/缢杀替换死亡之影设置" },
        { "Adds Gibbet and Gallows to Shadow of Death as well if chosen in positional preferences.", "如果在身位喜好里设置了，将死亡之影替换为绞决/缢杀" },
        { "Gluttony and Blood Stalk Option", "暴食与隐匿挥割设置" },
        { "Adds Gluttony and Blood Stalk to the combo when target is afflicted with Death's Design, and the skills are off cooldown and < 50 soul.", "当目标有死亡烙印Debuff并且灵魂小于50并且暴食/隐匿挥割不在冷却时，将其加入连击。" },
        { "Communio Movement Option", "移动时团契" },
        { "Uses Shadow of Death instead of Communio when moving.", "当移动时使用死亡之影替换团契" },

        #endregion

        #region AoE (Scythe) Combo Section

        { "Scythe Combo Feature", "旋转钐割连击" },
        { "Replace Spinning Scythe with its combo chain.\nIf all sub options are toggled will turn into a full one button rotation (Simple AoE)", "旋转钐割加入循环。\n如果启用所有子选项则变为一键循环（简单AOE）" },
        { "Soul Scythe Option", "灵魂钐割设置" },
        { "Adds Soul Scythe to AoE combo when Soul Gauge is 50 or less and current target is afflicted with Death's Design.", "当目标有死亡烙印Debuff并且灵魂小于50 将灵魂钐割加入AOE连击" },
        { "Whorl Of Death Option", "死亡之涡设置" },
        { "Adds Whorl of Death to AoE combo if Death's Design is not present on current target, or is about to expire.", "当目标的死亡烙印Debuff不存在或即将消失时 将死亡之涡加入AOE连击" },
        { "Guillotine Option", "断首设置" },
        { "Adds Guillotine to AoE combo when under Soul Reaver and when current target is afflicted with Death's Design.\nWill use Grim Reaping during Enshroud.", "当目标有死亡烙印Debuff且处于妖异之镰状态下时将断首加入AOE连击。\n在夜游魂衣时会重复使用阴冷收割。" },
        { "Adds Arcane Circle to AoE combo when off cooldown.", "当神秘环不在冷却时将其加入AOE连击。" },
        { "Adds Plentiful Harvest to AoE combo when off cooldown and ready.", "当大丰收不在冷却且准备好时将其加入AOE连击。" },
        { "Adds Enshroud to the AoE combo when at 50 Shroud and greater and when current target is afflicted with Death's Design.", "当目标有死亡烙印Debuff且魂衣大于等于50时将夜游魂衣加入AOE连击。" },
        { "Adds Lemure's Slice to the AoE combo when there are 2 Void Shrouds.", "当拥有2层虚无魂时将夜游魂钐割加入AOE连击。" },
        { "Adds Communio to the AoE combo when there is 1 Lemure Shroud left.", "当剩余1层夜游魂时将团契加入AOE连击。" },
        { "Gluttony and Grim Swathe Option", "暴食与束缚挥割设置" },
        { "Adds Gluttony and Grim Swathe to the AoE combo when current target is afflicted with Death's Design and Soul Gauge < 50.", "当目标有死亡烙印Debuff并且灵魂小于50时将暴食与束缚挥割加入AOE连击。" },

        #endregion

        #region Blood Stalk/Grim Swathe Combo Section

        { "Gluttony on Blood Stalk/Grim Swathe Feature", "暴食替换隐匿挥割/束缚挥割功能" },
        { "Blood Stalk and Grim Swathe will turn into Gluttony when it is available.", "当暴食可用时将其替换到隐匿挥割/束缚挥割上。" },
        { "Gibbet and Gallows/Guillotine on Blood Stalk/Grim Swathe Feature", "绞决/缢杀/断首替换隐匿挥割/束缚挥割功能" },
        { "Adds Gibbet and Gallows on Blood Stalk.\nAdds Guillotine on Grim Swathe.", "将隐匿挥割替换为绞决/缢杀。\n将束缚挥割替换为断首。" },
        { "Enshroud Combo Option", "夜游魂衣连击设置" },
        { "Adds Enshroud combo (Void/Cross Reaping, Communio, and Lemure's Slice) on Blood Stalk and Grim Swathe.", "将魂衣连击（交错/虚无收割，团契，夜游魂切割）替换至隐匿挥割和束缚挥割上" },

        #endregion

        #region Miscellaneous

        { "Arcane Circle Harvest Feature", "神秘环大丰收特性。" },
        { "Replaces Arcane Circle with Plentiful Harvest when you have stacks of Immortal Sacrifice.", "当你拥有死亡祭品层数时，使用大丰收替换神秘环。" },
        { "Regress Feature", "回退特性" },
        { "Changes both Hell's Ingress and Hell's Egress turn into Regress when Threshold is active.", "当你拥有回退预备状态时，将地狱入境与地狱出境替换为回退。" },
        { "Soulsow Reminder Feature", "播魂种提醒功能" },
        { "Adds Soulsow to the skills selected below when out of combat. \nWill also add Soulsow to Harpe when in combat and no target is selected.", "脱战时将魂播种替换至下列选择的技能上\n在战斗中如果没有选择目标也会将播魂种替换至勾刃上" },
        { "Harpe Harvest Moon Feature", "勾刃收获月特性" },
        { "Replaces Harpe with Harvest Moon when you are in combat with Soulsow active.", "当你拥有播魂种BUFF时，使用收获月替换勾刃。" },
        { "Enhanced Harpe Option", "勾刃效果提高设置" },
        { "Prevent Harvest Moon replacing Harpe when Enhanced Harpe is active.", "防止收获月在拥有勾刃效果提高时替换勾刃。" },
        { "Combat Harpe Option", "战斗外勾刃设置" },
        { "Prevent Harvest Moon replacing Harpe when you are not in combat.", "防止收获月在非战斗状态下替换勾刃。" },
        { "Enshroud Protection Feature", "夜游魂衣保护功能" },
        { "Turns Enshroud into Gibbet/Gallows to protect Soul Reaver waste.", "将夜游魂衣替换为绞决/缢杀来防止妖异之镰浪费。" },
        { "Communio on Gibbet/Gallows and Guillotine Feature", "团契替换绞决/缢杀/断首功能" },
        { "Adds Communio to Gibbet/Gallows and Guillotine.", "将团契加入到绞决/缢杀/断首" },
        { "Lemure's Slice/Scythe Option", "夜游魂切割/钐割设置" },
        { "Adds Lemure's Slice to Gibbet/Gallows and Lemure's Scythe to Guillotine.", "将夜游魂切割加入到绞决/缢杀,将夜游魂钐割加入到断首。" },
        { "Enshroud to Communio Feature", "夜游魂衣替换团契功能" },
        { "Turns Enshroud to Communio when available to use.", "当夜游魂衣可用时替换团契。" },
        { "Adds True North to Slice, Shadow of Death, Enshroud, and Blood Stalk when under Gluttony and if Gibbet/Gallows options are selected to replace those skills.", "如果绞决/缢杀设置替换了切割/死亡之影/夜游魂衣/隐匿挥割,在暴食后将真北加入这些技能。" },
        { "Soulsow Reminder during Combat", "战斗中播魂种提醒" },
        { "Adds Soulsow to Harpe during combat when no target is selected.", "没有目标选择时 将播魂种替换至勾刃上" },

        #endregion

        #endregion

        #region RED MAGE

        #region Section 1 - Openers

        { "Balance Opener Feature [Lv.90]", "黑白魔元平衡起手(需达到90级)" },
        { "Replaces Jolt with the Balance opener ending with Resolution\n**Must move into melee range before melee combo**", "将起手技能整合至摇荡并以决断结束.\n**使用魔三连时必须处于近战距离内.**" },
        { "Use Opener at any Mana Option", "无视魔元选项" },
        { "Removes 0/0 Mana reqirement to reset opener\n**All other actions must be off cooldown**", "无视魔元量整合起手技能.\n**但所有技能都必须处于可用状态.**" },

        #endregion

        #region Sections 2 to 3 - Rotation

        { "Verthunder/Veraero Feature", "赤闪雷/赤疾风 设置" },
        { "Replace Jolt with Verthunder and Veraero", "使用摇荡后自动根据黑白魔元量插入赤闪雷/赤疾风." },
        { "Single Target Acceleration Option", "对单目标时使用促进 设置" },
        { "Add Acceleration when no Verfire/Verstone proc is available", "当赤火炎/赤飞石未被触发时自动插入促进." },
        { "Include Swiftcast Option", "对单目标时使用即刻咏唱 设置" },
        { "Add Swiftcast when all Acceleration charges are used", "当促进全部使用完后插入即刻咏唱瞬发赤闪雷/赤疾风." },
        { "Verfire/Verstone Feature", "赤火炎/赤飞石 设置" },
        { "Replace Jolt with Verfire and Verstone", "触发赤火炎/赤飞石后自动将摇荡替换." },
        { "Weave oGCD Damage Feature", "伤害能力技插入 设置" },
        { "Use oGCD actions on specified action(s)", "在可用窗口内自动插入伤害能力技." },
        { "Fleche Option", "飞刺 设置" },
        { "Use Fleche on above specified action(s)", "在可用窗口内自动插入飞刺." },
        { "Contra Sixte Option", "六分反击 设置" },
        { "Use Contre Sixte on above specified action(s)", "在可用窗口内自动插入六分反击." },
        { "Engagement Option", "交剑 设置" },
        { "Use Engagement on above specified action(s) when in melee range", "当处于近战距离时在可用窗口内自动插入交剑." },
        { "Hold one charge Option", "保留位移技能 设置" },
        { "Pool one charge of Engagement/Displacement for manual use", "保留一层交剑/移转备用." },
        { "Corps-a-corps Option", "短兵相接 设置" },
        { "Use Corps-a-corps on above specified action(s)", "在可用窗口内自动插入短兵相接." },
        { "Only in Melee Range Option", "短兵相接距离检测" },
        { "Use Corps-a-corps only when in melee range", "仅在近战距离内插入短兵相接." },
        { "Pool one charge of Corp-a-corps for manual use", "保留一层短兵相接备用." },
        { "Verthunder II/Veraero II Feature", "赤震雷/赤烈风 设置" },
        { "Replace Scatter/Impact with Verthunder II or Veraero II", "使用赤震雷/赤烈风后自动将其替换为散碎/冲击." },
        { "AoE Acceleration Feature", "AoE攻击时使用促进 设置" },
        { "Use Acceleration on Scatter/Impact for increased damage", "使用促进瞬发散碎/冲击." },
        { "Add Swiftcast when all Acceleration charges are used or when below level 50", "50级以下时，当促进全部使用完后插入即刻咏唱瞬发散碎/冲击." },
        { "Weave Acceleration Option", "使用促进 设置" },
        { "Only use acceleration during weave windows", "仅使用促进瞬发技能." },

        #endregion

        # region Sections 4 to 5 - Melee

        { "Single Target Melee Combo Feature", "单目标魔六连一键连击 设置" },
        { "Stack Reposte combo on specified action(s)\n**Must be in melee range or have Gap close with Corps-a-corps enabled**", "等待翻译" },
        { "Use Manafication and Embolden Option", "倍增与鼓励使用 设置" },
        { "Add Manafication and Embolden on specified action(s)\n**Must be in melee range or have Gap close with Corps-a-corps enabled**", "整合倍增与鼓励至魔六连选择的技能中\n**必须处于近战攻击距离内或可用短兵相接接近目标**" },
        { "Hold for Double Melee Combo Option [Lv.90]", "倍增与鼓励延后使用 设置" },
        { "Hold both actions until you can perform a double melee combo", "延后使用倍增与鼓励以便可连续打出两次魔六连." },
        { "AoE Melee Combo Feature", "AoE魔六连一键连击 设置" },
        { "Use Moulinet on Scatter/Impact when over 60/60 mana", "当黑白魔元均超过60时，在最后一次散碎/冲击后插入魔划圆斩." },
        { "Add Manafication and Embolden to Scatter/Impact\n**Must be in range of Moulinet**", "在散碎/冲击后插入倍增与鼓励.\n**必须处于魔划圆斩可攻击到目标时**" },
        { "Gap close with Corps-a-corps Option", "使用短兵相接接近目标 设置" },
        { "Use Corp-a-corps when out of melee range and you have enough mana to start the melee combo", "当处于近战距离外且有足够魔元打出魔三连时使用短兵相接靠近近目标." },
        { "Unbalance Mana Option", "不平衡魔元处理 设置" },
        { "Use Acceleration to unbalance mana prior to starting melee combo", "使用促进触发黑白魔元中较低的一方以便使用魔三连." },
        { "Melee Finisher Feature", "魔六连终结 设置" },
        { "Add Verflare/Verholy and other finishing moves to specified action(s)", "整合魔六连的后三连到下列选项之一" },

        #endregion

        #region Sections 6 to 7 - QoL

        { "Lucid Dreaming Feature", "醒梦" },
        { "Use Lucid Dreaming on Jolt 1/2, Veraero 1/2/3, Verthunder 1/2/3, and Scatter/Impact when below threshold.", "当mp低于设定值时，在可用窗口中插入醒梦." },
        { "Verraise Feature", "赤复活 设置" },
        { "Changes Swiftcast to Verraise when under the effect of Swiftcast or Dualcast.", "当存在连续咏唱状态或使用即刻咏唱后，将即刻咏唱替换为赤复活." },

        #endregion

        #region Sections 8 to 9 - Miscellaneous

        { "Displacement <> Corps-a-corps Feature", "移转与短兵相接替换 设置" },
        { "Replace Displacement with Corps-a-corps when out of range.", "当距离过远时替换移转为短兵相接." },
        { "Embolden to Manafication Feature", "鼓励与倍增替换 设置" },
        { "Changes Embolden to Manafication when on cooldown.", "当鼓励在冷却时替换其为倍增." },
        { "Magick Barrier to Addle Feature", "抗死与昏乱替换 设置" },
        { "Changes Magick Barrier to Addle when on cooldown.", "当抗死在冷却时替换其为昏乱." },

        #endregion

        #endregion

        #region SAGE

        #region Single Target DPS Feature

        { "Single Target DPS Feature", "整合单体输出技能" },
        { "Adds Lucid Dreaming to Dosis when MP drops below slider value.", "等待翻译" },
        { "Eukrasian Dosis Option", "均衡注药 设置" },
        { "Automatic DoT Uptime", "自动插入均衡注药续Dot" },
        { "Toxikon Movement Option", "移动时使用箭毒 设置" },
        { "Use Toxikon when you have Addersting charges and are moving.", "等待翻译" },

        #endregion

        #region AoE DPS Feature

        { "Toxikon - No Phlegma Charges Option", "箭毒 无发炎层数时选项" },
        { "Use Toxikon when out of Phlegma charges\nTakes priority over Dyskrasia.", "当没有发炎层数时使用箭毒，优先于失衡" },
        { "Toxikon - Out of Phlegma Range Option", "箭毒 超出发炎范围时选项" },
        { "Use Toxikon when out of Phlemga's Range\nTakes priority over Dyskrasia.", "当超出发炎范围时使用箭毒，优先于失衡" },
        { "Dyskrasia - No Phlegma Charges Option", "失衡 无发炎层数时选项" },
        { "Use Dyskrasia when out of Phlegma charges.", "当超出发炎范围时使用失衡" },
        { "Dyskrasia - No-Target Option", "失衡 无目标锁定选项" },
        { "Use Dyskrasia when no target is selected.", "当无目标锁定时使用失衡" },
        { "Adds Lucid Dreaming to Phlegma when MP drops below slider value.", "当MP小于滑块值时，将醒梦整合至发炎" },

        #endregion

        #region Diagnosis Simple Single Target Heal

        { "Single Target Heal Feature", "单目标治疗功能" },
        { "Supports soft-targeting.\nOptions below are in priority order", "支持弱选中.\n下列选项按照优先级从高到低依次排列." },
        { "Apply Kardia Option", "心关" },
        { "Applies Kardia to your target if it's not applied to anyone else.", "如果心关从未使用过，则对选中目标使用心关。" },
        { "Eukrasian Diagnosis Option", "均衡诊断选项" },
        { "Diagnosis becomes Eukrasian Diagnosis if the shield is not applied to the target.", "当所选目标没有盾值时，替换诊断为均衡诊断。" },
        { "Soteria Option", "拯救选项" },
        { "Applies Soteria.", "插入拯救." },
        { "Zoe Option", "活化选项" },
        { "Applies Zoe.", "插入活化." },
        { "Pepsis Option", "消化选项" },
        { "Triggers Pepsis if a shield is present.", "当盾值存在时激活消化。" },
        { "Taurochole Option", "白牛清汁选项" },
        { "Adds Taurochole.", "插入白牛清汁." },
        { "Haima Option", "输血选项" },
        { "Applies Haima.", "插入输血." },
        { "Rhizomata Option", "根素选项" },
        { "Adds Rhizomata when Addersgall is 0.", "当没有蛇胆时插入根素." },
        { "Krasis Option", "混合选项" },
        { "Applies Krasis.", "插入混合." },
        { "Druochole Option", "灵橡清汁选项" },
        { "Applies Druochole.", "插入灵橡清汁." },

        #endregion

        #region Sage Simple AoE Heal

        { "AoE Heal Feature", "群体治疗技能一键整合至预后" },
        { "Customize your AoE healing to your liking.", "按照个人喜好制定群体治疗技能一键整合方式." },
        { "Physis Option", "自生" },
        { "Adds Physis.", "自动插入自生。" },
        { "Eukrasian Prognosis Option", "均衡预后" },
        { "Prognosis becomes Eukrasian Prognosis if the shield is not applied.", "当没有盾值时，替换预后为均衡预后。" },
        { "Holos Option", "整体论" },
        { "Adds Holos.", "自动插入整体论。" },
        { "Panhaima Option", "泛输血" },
        { "Adds Panhaima.", "自动插入泛输血。" },
        { "Ixochole Option", "寄生清汁" },
        { "Adds Ixochole.", "整合寄生清汁." },
        { "Kerachole Option", "坚角清汁" },
        { "Adds Kerachole.", "整合坚角清汁." },

        #endregion

        #region Misc Healing

        { "Rhizomata Feature", "根素 设置" },
        { "Replaces Addersgall skills with Rhizomata when empty.", "蛇胆不满时插入根素" },
        { "Druochole to Taurochole Feature", "替换灵橡清汁为白牛清汁 设置" },
        { "Upgrades Druochole to Taurochole when Taurochole is available.", "当白牛青汁可用时，替换灵橡清汁为白牛清汁." },
        { "Zoe Pneuma Feature", "将活化整合至魂灵风息 设置" },
        { "Places Zoe on top of Pneuma when both actions are on cooldown.", "魂灵风息变为活化，使用后变回魂灵风息." },

        #endregion

        #region Utility

        { "Swiftcast Raise Feature", "即刻复活 设置" },
        { "Changes Swiftcast to Egeiro while Swiftcast is on cooldown.", "整合即可咏唱至复苏" },
        { "Soteria to Kardia Feature", "替换拯救为心关 设置" },
        { "Soteria turns into Kardia when not active or Soteria is on-cooldown.", "当未使用心关或拯救处于冷却状态时，替换拯救为心关。" },
        { "Eukrasia Feature", "均衡技能整合 设置" },
        { "Eukrasia turns into the selected Eukrasian-type action when active.", "使用均衡后将其替换为下列选择的技能之一." },

        #endregion

        #endregion

        #region SAMURAI

        #region Overcap Features

        {"Samurai Overcap Feature","武士单体技能替代选项"},
        {"Adds Shinten onto main combo when Kenki is at the selected amount or more","当剑气达到或超过所选数量时，将必杀剑·震天添加到单体连击中。"},
        {"Samurai AoE Overcap Feature","武士AOE技能替代选项"},
        {"Adds Kyuten onto main AoE combos when Kenki is at the selected amount or more","当剑气达到或超过所选数量时，将必杀剑·九天添加到主要AOE连击中。"},

        #endregion

        #region Main Combo (Gekko) Features

        { "Gekko Combo", "月光连" },
        { "Replace Gekko with its combo chain.\nIf all sub options are selected will turn into a full one button rotation (Advanced Samurai)", "等待翻译" },
        {"Enpi Uptime Feature","燕飞特性"},
        {"Replace main combo with Enpi when you are out of range.","当你离开射程时，使用燕飞。"},
        {"Yukikaze Combo on Main Combo","雪风连击加入循环"},
        {"Adds Yukikaze Combo to main combo. Will add Yukikaze during Meikyo Shisui as well","将雪风连击加入主循环。在明镜止水期间也将使用雪风连击。"},
        {"Kasha Combo on Main Combo","花车连击加入循环"},
        {"Adds Kasha combo to nain combo. Will add Kasha during Meikyo Shisui as well.","将花车连击加入主循环。在明镜止水期间也将使用花车连击。"},
        {"Level 90 Samurai Opener","90级武士棋手爆发"},
        {"Adds the Level 90 Opener to the main combo.\nOpener triggered by using Meikyo Shisui before combat. If you have any Sen, Hagakure will be used to clear them.\nWill work at any levels of Kenki, requires 2 charges of Meikyo Shisui and all CDs ready. If conditions aren't met it will skip into the regular rotation. \nIf the Opener is interrupted, it will exit the opener via a Goken and a Kaeshi: Goken at the end or via the last Yukikaze. If the latter, CDs will be used on cooldown regardless of burst options.","在循环中加入90级的开场爆发。\n在战斗前使用明镜止水触发开场爆发。如果你有任何闪，叶隐将被用来清除它们。\n在存有2层明镜止水和所有技能都转好了cd的情况下，无论存有多少剑气，都可以正常工作。如果不满足上述条件，将自动进入正常的循环。\n如果起手爆发被打断，它会使用天下五剑+回返五剑的组合或是一个雪风来退出起手爆发。 如果是以雪风退出，将在冷却完成后立即使用，而不考虑爆发选项。"},
        {"Filler Combo Feature","填充技能选项"},
        {"Adds selected Filler combos to main combo at the appropriate time.\nChoose Skill Speed tier with Fuka buff below.\nWill disable if you die or if you don't activate the opener.","在适当的时候将选定的填充技能组合添加到输出循环中。选择技能速度与下面的Fuka buff。\n在你死亡或不使用开场爆发的情况下失效。"},

        #region CDs on Main Combo

        { "Collection of CD features on main combo.", "将CD技能加入循环" },
        { "Ikishoten on Main Combo", "将意气冲天加入循环" },
        { "Adds Ikishoten when at or below 50 Kenki.\nWill dump Kenki at 10 seconds left to allow Ikishoten to be used.", "当剑气达到或低于50时，可将意气冲天加入到月光和满月连击中。\n将在义气冲天还剩10秒时消耗剑气，防止剑气溢出。" },
        { "Iaijutsu on Main Combo", "将居合术加入循环" },
        { "Adds Midare: Setsugekka, Higanbana, and Kaeshi: Setsugekka when ready and when you're not moving to main combo.", "在未移动和技能冷却完毕的情况下，将纷乱雪月花、彼岸花和回返雪月花加入循环。" },

        #region Ogi Namikiri on Main Combo

        {"Ogi Namikiri on Main Combo","将奥义斩浪加入循环"},
        {"Ogi Namikiri and Kaeshi: Namikiri when ready and when you're not moving to main combo.","在未移动和技能冷却完毕的情况下，将奥义斩浪和回返斩浪加入循环。"},
        {"Ogi Namikiri Burst Feature","奥义斩浪爆发选项"},
        {"Saves Ogi Namikiri for even minute burst windows.\nIf you don't activate the opener or die, Ogi Namikiri will instead be used on CD.","保留奥义斩浪直至爆发循环。\n如果你未使用开场爆发或死亡，则会在冷却完毕时使用。"},

        #endregion

        {"Meikyo Shisui on Main Combo","将明镜止水加入循环"},
        {"Adds Meikyo Shisui to main combo when off cooldown.","当冷却完毕时将明镜止水加入循环"},

        #region Meikyo Shisui on Main Combo

        {"Meikyo Shisui Burst Feature","明镜止水爆发选项"},
        {"Saves Meikyo Shisui for burst windows.\nIf you don't activate the opener or die, Meikyo Shisui will instead be used on CD.","保留明镜止水直至爆发循环。\n如果你未使用开场爆发或死亡，则会在冷却完毕时使用。"},

        #endregion

        {"Shoha on Main Combo","将照破加入循环"},
        {"Adds Shoha to main combo when there are three meditation stacks.","当拥有三档剑压时，将照破加入循环"},
        {"Senei on Main Combo","将必杀剑·闪影加入循环"},
        {"Adds Senei to main combo when off cooldown and above 25 Kenki.","当必杀剑·闪影冷却完毕且剑气超过25时，将必杀剑·闪影加入循环"},
        {"Senei Burst Feature","必杀剑·闪影爆发选项"},
        {"Saves Senei for even minute burst windows.\nIf you don't activate the opener or die, Senei will instead be used on CD.","保留必杀剑·闪影直至爆发循环。\n如果你未使用开场爆发或死亡，则会在冷却完毕时使用。"},


        #endregion

        #endregion

        #region Yukikaze/Kasha Combos

        { "Yukikaze Combo", "雪风连" },
        { "Replace Yukikaze with its combo chain.", "替换雪风为相应连击。" },
        { "Kasha Combo", "花车连" },
        { "Replace Kasha with its combo chain.", "替换花车为相应连击。" },

        #endregion

        #region AoE Combos

        { "Mangetsu Combo", "满月连" },
        { "Replace Mangetsu with its combo chain.\nIf all sub options are toggled will turn into a full one button AoE rotation.", "用满月连击的组合代替满月。\n如果所有子选项都被选中，将变成一个完整的一键循环（高级武士）。" },
        { "Oka to Mangetsu Combo", "等待翻译" },
        { "Adds Oka combo after Mangetsu combo loop.\nWill add Oka if needed during Meikyo Shisui.", "将樱花连击加如到AOE连击中。\n樱花连击中将试情况使用明镜止水" },
        { "Iaijutsu on Mangetsu Combo", "将居合术加入AOE连击" },
        { "Adds Tenka Goken, Midare: Setsugekka, and Kaeshi: Goken when ready and when you're not moving to Mangetsu combo.", "等待翻译" },
        { "Ogi Namikiri on Mangetsu Combo", "将奥义斩浪加入AOE连击" },
        { "Adds Ogi Namikiri and Kaeshi: Namikiri when ready and when you're not moving to Mangetsu combo.", "在未移动和技能冷却完毕的情况下，将奥义斩浪加入循环。" },
        { "Shoha 2 on Mangetsu Combo", "将无明照破加入AOE连击" },
        { "Adds Shoha 2 when you have 3 meditation stacks to Mangetsu combo.", "当拥有三档剑压时，将无明照破加入AOE循环" },
        { "Guren on Mangetsu Combo", "将必杀剑·红莲加入AOE连击" },
        { "Adds Guren when it's off cooldown and you have 25 Kenki to Mangetsu combo.", "当必杀剑·红莲冷却完毕且剑气超过25时，将必必杀剑·红莲加入循环" },
        { "Meikyo Shisui on Mangetsu Combo", "等待翻译" },
        { "Adds Meikyo Shisui to Mangetsu combo.", "等待翻译" },
        { "Oka Combo", "樱花连" },
        { "Replace Oka with its combo chain.", "替换樱花为相应连击。" },
        { "Oka Two Target Rotation Feature", "樱花双目标功能" },
        { "Adds the Yukikaze combo, Mangetsu combo, Senei, Shinten, and Shoha to Oka combo.\nUsed for two targets only and when Lv86 and above.", "增加雪风连击、满月连击、必杀剑-闪影、必杀剑-震天、照破到樱花连击。只适用于两个目标，并且在Lv86以上时使用。" },

        #endregion

        #region CD Features

        { "Jinpu/Shifu Feature", "明镜止水" },
        { "Replace Meikyo Shisui with Jinpu, Shifu, and Yukikaze depending on what is needed.", "根据需要，用阵风、士风和雪风代替明镜止水。" },

        #endregion

        #region Iaijutsu Features

        { "Iaijutsu Features", "居合术功能" },
        { "Collection of Iaijutsu Features.", "居合术整合" },
        { "Iaijutsu to Tsubame-Gaeshi", "使用燕回返代替居合术" },
        { "Replace Iaijutsu with  Tsubame-gaeshi when Sen is empty.", "当闪为空时，用燕回返代替居合术。" },
        { "Iaijutsu to Shoha", "照破 替换 居合术" },
        { "Replace Iaijutsu with Shoha when meditation is 3.", "当剑压积累到3档时，替换居合术为照破。" },
        { "Iaijutsu to Ogi Namikiri", "使用奥义斩浪代替居合术" },
        { "Replace Iaijutsu with Ogi Namikiri and Kaeshi: Namikiri when buffed with Ogi Namikiri Ready.", "处于奥义斩浪预备的时候，使用回奥义斩浪和返斩浪代替居合术。" },

        #endregion

        #region Shinten Features

        { "Shinten to Shoha", "照破 替换 必杀剑·震天" },
        { "Replace Hissatsu: Shinten with Shoha when Meditation is full.", "当剑压积累到3档时，替换必杀剑·震天为照破。" },
        { "Shinten to Senei", "必杀剑·闪影 替换 必杀剑·震天" },
        { "Replace Hissatsu: Shinten with Senei when its cooldown is up.", "当必杀剑·闪影冷却结束后，替换必杀剑·震天为必杀剑·闪影。" },

        #endregion

        #region Kyuten Features

        { "Kyuten to Shoha II", "无名照破 替换 必杀剑·九天" },
        { "Replace Hissatsu: Kyuten with Shoha II when Meditation is full.", "当剑压积累到3档时，替换必杀剑·九天为无名照破。" },
        { "Kyuten to Guren", "必杀剑·红莲 替换 必杀剑·九天" },
        { "Replace Hissatsu: Kyuten with Guren when its cooldown is up.", "当必杀剑·红莲冷却结束后，替换必杀剑·九天为必杀剑·红莲。" },

        #endregion

        #region Other

        { "Gyoten Feature", "必杀剑·晓天 特性" },
        { "Hissatsu: Gyoten becomes Yaten/Gyoten depending on the distance from your target.", "根据与目标的距离自动将必杀剑·晓天变为必杀剑·夜天/晓天。" },
        { "Ikishoten Namikiri Feature", "意气冲天 奥义斩浪 连击特性" },
        { "Replace Ikishoten with Ogi Namikiri and then Kaeshi Namikiri when available.\nIf you have full Meditation stacks, Ikishoten becomes Shoha while you have Ogi Namikiri ready.", "当奥义斩浪可用时，替换意气冲天为奥义斩浪，随后变为回返斩浪。\n若同时剑压已积累3档，则意气冲天将变为照破。" },
        { "Adds True North on all single target combos if Meikyo Shisui's buff is on you.", "处于明镜止水状态时，将真北插入所有的单体连击" },

        #endregion

        #endregion

        #region SCHOLAR

        #region DPS

        { "Replace Ruin I / Broils or Bios with options below", "等待翻译" },
        { "Adds Lucid Dreaming when MP drops below slider value:", "当 MP 低于滑动条的值时在循环中加入醒梦" },
        { "Chain Stratagem Weave Option", "连环计" },
        { "Adds Chain Stratagem on Cooldown with overlap protection", "等待翻译" },
        { "Aetherflow Weave Option", "以太超流" },
        { "Use Aetherflow when out of aetherflow stacks", "当自身没有以太超流层数时使用以太超流技能补充" },
        { "Ruin II Moving Option", "移动毁荡" },
        { "Use Ruin 2 when you have to move", "使用毁荡当你在移动时" },
        { "Bio / Biolysis Option", "毒菌/蛊毒法" },
        { "Automatic DoT Uptime.", "自动插入均衡注药续Dot." },

        #endregion

        #region Healing

        { "Fey Blessing to Seraph's Consolation Feature", "异想的祥光 替换 慰藉" },
        { "Change Fey Blessing into Consolation when Seraph is out.", "炽天使同行状态下，将 异想的祥光 变为 慰藉." },

        #endregion

        #region Utilities

        { "Aetherflow Helper Feature", "以太超流" },
        { "Change Aetherflow-using skills to Aetherflow, Recitation, or Dissipation as selected", "等待翻译" },
        { "Recitation Option", "秘策" },
        { "Prioritizes Recitation usage on Excogitation or Indominability", "等待翻译" },
        { "On Excogitation Option", "深谋远虑之策" },
        { "On Indominability Option", "不屈不挠之策" },
        { "Dissipation Option", "转化" },
        { "If Aetherflow itself is on cooldown, show Dissipation instead", "等待翻译" },
        { "Swiftcast Raise Combo Feature", "即刻复活设置" },
        { "Changes Swiftcast to Resurrection while Swiftcast is on cooldown", "当即刻冷却完毕 替换 复生" },
        { "Fairy Feature", "小仙女" },
        { "Change all fairy actions into Fairy Summons if you do not have a fairy summoned.", "当没有召唤小仙女时，将所有小仙女行动技能替换为召唤小仙女" },
        { "Deployment Tactics Feature", "展开战术" },
        { "Deployment Tactics idles as Adloquium until the Party Member has the Galvanize Buff", "将展开战术替换为鼓舞激励之策，直到所有队友都拥有鼓舞盾" },
        { "Adds Recitation when off cooldown to force a critical Galvanize Buff on the Party Member.", "等待翻译" },

        #endregion

        #endregion

        #region SUMMONER

        { "Advanced Summoner Feature", "高级自定义循环开关" },
        { "Advanced combo features for a greater degree of customisation.\nAccommodates SpS builds.\nRuin III is left unchanged for mobility purposes.", "高级自定义循环开关 \n适配咏速套装.\n手动毁荡" },
        { "Demi Attacks Combo Option", "亚灵神替换" },
        { "Adds Deathflare, Ahk Morn and Revelation to the single target and AoE combos.", "当 龙神迸发/不死鸟迸发/死星核爆 可用时替换 毁荡和三重灾祸" },
        { "Egi Attacks Combo Option", "宝石耀&宝石辉替换" },
        { "Adds Gemshine and Precious Brilliance to the single target and AoE combos, respectively.", "宝石耀 宝石辉 可用时替换 成对应的单体群体技能" },
        { "Garuda Slipstream Option", "螺旋气流-迦楼罗替换" },
        { "Adds Slipstream to the single target and AoE combos.", "螺旋气流 可用时替换 毁荡和三重灾祸" },
        { "Ifrit Cyclone Option", "深红旋风-伊芙利特替换" },
        { "Adds Crimson Cyclone and Crimson Strike to the single target and AoE combos.", "深红旋风 深红强袭 可用时替换 毁荡和三重灾祸" },
        { "Titan Mountain Buster Option", "山崩-泰坦替换" },
        { "Adds Mountain Buster to the single target and AoE combos.", "山崩 可用时替换 毁荡和三重灾祸" },
        { "Energy Drain to Fester Feature", "能量吸收替换" },
        { "Change Fester into Energy Drain when out of Aetherflow stacks.", "当自身没有以太超流时 用能量吸收替换溃烂爆发。" },
        { "Energy Siphon to Painflare Feature", "能量抽取替换" },
        { "Change Painflare into Energy Siphon when out of Aetherflow stacks.", "当自身没有以太超流时 用能量抽取替换痛苦核爆" },
        { "Carbuncle Reminder Feature", "宝石兽自动召唤" },
        { "Replaces most offensive actions with Summon Carbuncle when it is not summoned.", "当没有宝石兽跟随时，大部分技能变为召唤宝石兽" },
        { "Ruin IV Combo Option", "循环加入毁绝" },
        { "Adds Ruin IV to the single target and AoE combos.\nUses when moving during Garuda Phase and you have no attunement, when moving during Ifrit phase, or when you have no active Egi or Demi summon.", "在循环和AOE循环中加入毁绝技能 仅在没有其他瞬发技能时使用" },
        { "Ruin IV Fester Option", "毁绝替换" },
        { "Changes Fester to Ruin IV when out of Aetherflow stacks, Energy Drain is on cooldown, and Ruin IV is available.", "当没有以太超流层数时 自身处于毁绝准备中 并且能量吸收进入冷却，溃烂爆发技能变为毁绝，" },
        { "Energy Attacks Combo Option", "循环加入能量吸收和能量抽取" },
        { "Adds Energy Drain and Fester to the single target combo.\nAdds Energy Siphon and Painflare to the AoE combo.\nWill be used on cooldown.", "将能量系技能在冷却完毕后加入到循环和AOE循环中 " },
        { "Egi Summons Combo Option", "循环加入三神召唤" },
        { "Adds Egi summons to the single target and AoE combos.\nWill prioritise the Egi selected below.\nIf no option is selected, the feature will default to summoning Titan first.", "将三神召唤加入到循环和AOE循环中\n可选择召唤顺序.\n默认采用泰坦优先." },
        { "Searing Light Combo Option", "循环加入灼热之光" },
        { "Adds Searing Light to the single target and AoE combos.\nWill be used on cooldown.", "在循环和AOE循环中加入灼热之光\n冷却完毕立刻使用" },
        { "Searing Light Burst Option", "灼热之光爆发设置" },
        { "Casts Searing Light only during Demi phases.\nReflects Demi choice selected under 'Pooled oGCDs Option'.\nNot recommended for SpS Builds.", "只在亚神召唤中使用灼热之光\n进阶选项在 高级能力技选项下\n不适用咏速套装" },
        { "Demi Summons Combo Option", "循环中加入亚神召唤" },
        { "Adds Demi summons to the single target and AoE combos.", "循环和AOE循环中加入 龙神召唤 不死鸟召唤" },
        { "Swiftcast Egi Ability Option", "即刻咏唱三神技能选项" },
        { "Uses Swiftcast during the selected Egi summon.", "使用三神技能时自动使用即刻" },
        { "Astral Flow/Enkindle on Demis Feature", "亚神技能替换亚神召唤" },
        { "Adds Enkindle Bahamut, Enkindle Phoenix and Astral Flow to their relevant summons.", "龙神迸发 不死鸟迸发 死星核爆 可用时替换掉 对应的亚神召唤" },
        { "Pooled oGCDs Option", "高级能力技选项" },
        { "Pools damage oGCDs for use inside the selected Demi phase while under the Searing Light buff.", "只在选定条件下以及灼热之光buff中使用 能力技" },
        { "Changes Swiftcast to Raise when on cooldown.", "当即刻冷却完毕 替换 复生" },
        { "Rekindle Combo Option", "循环加入苏生之炎" },
        { "Adds Rekindle to the single target and AoE combos.", "循环 AOE循环中 加入苏生之炎" },
        { "Ruin III Mobility Feature", "毁荡走位设置" },
        { "Puts Ruin III on Ruin IV when you don't have Further Ruin.", "当你没有毁绝预备buff时，用毁荡替换毁绝。" },
        { "Lucid Dreaming Option", "循环加入醒梦" },
        { "Adds Lucid Dreaming to the single target combo when MP falls below the set value.", "当MP低于设定值时，将醒梦加入到循环中" },
        { "Egi Abilities on Summons Feature", "三神星极超流技替换" },
        { "Adds Egi Abilities (Astral Flow) to Egi summons when ready.\nEgi abilities will appear on their respective Egi summon ability, as well as Titan.", "三神星极超流技可用时 替换 三神召唤技能 " },
        { "Use only on Single Target combo", "只在单循环中使用" },
        { "Prevent this feature from applying to the AoE combo.", "禁止此功能在AOE循环中使用" },
        { "Ruin IV Painflare Option", "毁绝替换痛苦核爆" },
        { "Changes Painflare to Ruin IV when out of Aetherflow stacks, Energy Siphon is on cooldown, and Ruin IV is up.", "能量抽取CD中自身没有以太档数 并且毁绝准备中时 将痛苦核爆替换为毁绝" },
        { "Crimson Cyclone Option", "深红旋风进阶" },
        { "Only uses Crimson Cyclone if you are not moving, or have no remaining Ifrit Attunement charges.", "只在未移动或者没有火神对应技能的情况下使用" },
        { "Simple Summoner Feature", "简易召唤" },
        { "General purpose one-button combo.\nBursts on Bahamut phase.\nSummons Titan, Garuda, then Ifrit.\nSwiftcasts on Slipstream unless drifted.", "一键循环.\n巴哈阶段开爆发.\n召唤顺序 泰坦>迦楼罗>伊芙利特.\n即刻用来释放 螺旋气流 .." },
        { "Burst Delay Option", "爆发延迟开关" },
        { "Only follows Burst Delay settings for the opener burst.\nThis Option is for high SPS builds.", "自定义爆发延迟设置\n适用于咏速套装" },

        #endregion

        #region WARRIOR

        { "Storms Path Combo", "暴风斩连击" },
        { "All in one main combo feature adds Storm's Eye/Path. \nIf all sub options and Fell Cleave/Decimate Options are toggled will turn into a full one button rotation (Advanced Warrior)", "等待翻译" },
        { "Storms Eye Combo", "暴风碎连击" },
        { "Replace Storms Eye with its combo chain", "将暴风碎替换为暴风碎连击" },
        { "Overpower Combo", "超压斧连击" },
        { "Add combos to Overpower", "将超压斧替换为AOE连击" },
        { "Warrior Gauge Overcap Feature", "兽魂监控" },
        { "Replace Single target or AoE combo with gauge spender if you are about to overcap and are before a step of a combo that would generate beast gauge", "如果兽魂快溢出了，将消耗兽魂的技能加入到暴风斩连击和AOE连击" },
        { "Nascent Flash Feature", "原初的勇猛" },
        { "Replace Nascent Flash with Raw intuition when level synced below 76", "在同步到76级以下时，将原初的勇猛替换为原初的直觉" },
        { "Upheaval Feature", "动乱" },
        { "Adds Upheaval into maincombo if you have Surging Tempest", "在有暴风碎BUFF时，将动乱加入到暴风斩连击" },
        { "Primal Rend Feature", "蛮荒崩裂Main combo" },
        { "Replace Inner Beast and Steel Cyclone with Primal Rend when available (Also added onto Main AoE combo)", "在蛮荒崩裂预备状态下，将裂石飞环/地毁人亡替换为蛮荒崩裂" },
        { "Orogeny Feature", "群山隆起" },
        { "Adds Orogeny onto main AoE combo when you are buffed with Surging Tempest", "在有暴风碎BUFF时，将群山隆起加入到AOE连击" },
        { "Fell Cleave/Decimate Option", "裂石飞环/地毁人亡" },
        { "Adds Fell Cleave to main combo when gauge is at 50 or more and adds Decimate to the AoE combo .\nWill use Inner Chaos/Chaotic Cyclone if Infuriate is used and Fell Cleave/Steel Cyclone during Inner Release.\nWill begin pooling resources when Inner Release is under 30s", "等待翻译" },
        { "Onslaught Feature", "猛攻" },
        { "Adds Onslaught to Storm's Path feature combo if you are under Surging Tempest Buff", "在有暴风碎BUFF时，将猛攻加入到暴风斩连击" },
        { "Infuriate AoE Feature", "战嚎 AoE Feature" },
        { "Adds Infuriate to AoE combo when gauge is below 50 and not under Inner Release.", "等待翻译" },
        { "Inner Release AoE Feature", "原初的解放 AoE Feature" },
        { "Adds Inner Release to Storm's Path combo.", "将原初的解放加入到暴风斩连击" },
        { "Tomahawk Uptime Feature", "飞斧" },
        { "Replace Storm's Path Combo Feature with Tomahawk when you are out of range.", "在如果离boss太远，将飞斧加入到暴风斩连击" },
        { "Infuriate on Fell Cleave / Decimate", "战嚎 on Fell Cleave / Decimate" },
        { "Turns Fell Cleave and Decimate into Infuriate if at or under set rage value", "Turns Fell Cleave and Decimate into 战嚎 if at or under set rage value" },
        { "Primal Rend Option", "蛮荒崩裂" },
        { "Turns Inner Release into Primal Rend on use.", "在蛮荒崩裂预备状态下，将原初的解放替换为蛮荒崩裂." },
        { "Inner Release on Storm's Path", "原初的解放加入暴风斩循环" },
        { "Infuriate on Storm's Path", "战壕加入暴风斩循环" },
        { "Adds Infuriate to Storm's Path combo when gauge is below 50 and not under Inner Release.", "等待翻译" },
        { "Use Inner Release Stacks First", "等待翻译" },
        { "Prevents the use of Infuriate while you have Inner Release stacks available.", "等待翻译" },
        { "Primal Rend Melee Feature", "蛮荒崩裂 Melee Feature" },
        { "Uses Primal Rend when in the target's target ring (1 yalm) and closer otherwise will use it when buff is less than 10 seconds.", "当处于目标的目标圈内（1米）并且蛮荒崩裂预备小于10s时使用蛮荒崩裂" },
        { "Melee Onslaught Option", "猛攻" },
        { "Uses Onslaught when under Surging Tempest and in the target ring (1 yalm).\nWill use as many stacks as selected in the above slider.", "拥有战场风暴并且处于目标的目标圈内（1米）时使用猛攻\n将使用滑条选择的层数" },

        #endregion

        #region WHITE MAGE

        #region Single Target DPS Feature

        { "Collection of cooldowns and spell features on Glare/Stone.", "咏唱闪耀/飞石后插入能力技." },
        { "Glare III Opener Option", "开场技能整合至闪灼 设置" },
        { "Delays all oGCDs until after third Glare III cast.\nOnly works with Glare III.", "将所有的能力技延迟到第三发闪灼释放之后，仅当闪灼可用时生效" },
        { "Aero/Dia Uptime Option", "保持dot不断 设置" },
        { "Adds Aero/Dia to the single target combo if the debuff is not present on current target, or is about to expire.", "当目标身上不存在dot或dot即将结束时，在单目标输出循环中插入疾风/烈风/天辉." },
        { "Assize Option", "法令 设置" },
        { "Adds Assize to the single target combo.", "将法令插入单目标输出循环中." },
        { "Afflatus Misery Option", "苦难之心 设置" },
        { "Adds Afflatus Misery to the single target combo when it is ready to be used.", "当苦难之心可用时将其插入单目标输出循环中." },
        { "Lily Overcap Protection Option", "百合保护" },
        { "Adds Afflatus Rapture to the single target combo when at three Lilies.", "当有三档治疗百合时在单目标输出循环中插入苦难之心." },
        { "Presence of Mind Option", "神速咏唱 设置" },
        { "Adds Presence of Mind to the single target combo.", "将神速咏唱插入单目标输出循环中." },
        { "Adds Lucid Dreaming to the single target combo when below set MP value.", "当MP低于设定值时将醒梦插入单目标输出循环中." },

        #endregion

        #region AoE DPS Feature

        { "Collection of cooldowns and spell features on Holy/Holy III.", "整合AoE技能到神圣/豪圣." },
        { "Adds Assize to the AoE combo.", "将法令整合AoE循环中." },
        { "Adds Afflatus Misery to the AoE combo when it is ready to be used.", "当苦难之心可用时将其插入AoE循环中." },
        { "Adds Afflatus Rapture to the AoE combo when at three Lilies.", "当有三档治疗百合时在AoE循环中插入苦难之心." },
        { "Adds Presence of Mind to the AoE combo.", "将神速咏唱插入AoE循环中" },
        { "Adds Lucid Dreaming to the AoE combo when below set MP value.", "当MP低于设定值时将醒梦插入AoE循环中." },

        #endregion

        { "Solace into Misery Feature", "安慰之心与苦难之心整合 设置" },
        { "Replaces Afflatus Solace with Afflatus Misery when it is ready to be used.", "当苦难之心可用时，替换安慰之心为苦难之心." },
        { "Rapture into Misery Feature", "狂喜之心与苦难之心整合 设置" },
        { "Replaces Afflatus Rapture with Afflatus Misery when it is ready to be used.", "当苦难之心可用时，替换狂喜之心为苦难之心." },

        #region Afflatus Feature

        { "Afflatus Feature", "治疗百合状态" },
        { "Changes Cure II into Afflatus Solace when Lilies are up.\nChanges Medica into Afflatus Rapture when Lilies are up.", "当有可用百合时替换救疗为苦难之心，将医济替换为狂喜之心\n" },
        { "Afflatus Misery Heal Feature", "治疗时使用苦难之心 设置" },
        { "Changes Cure II into Afflatus Misery when Misery is ready to be used.", "当苦难之心可用时，替换救疗为苦难之心." },

        #region oGCD Heals/Shields Option

        { "oGCD Heals/Shields Option", "瞬发治疗/护盾" },
        { "Adds oGCD Healing and Shields to Cure II.", "整合治疗和减伤能力技至救疗" },
        { "Use Tetragrammaton on oGCD Option", "插入神名 设置." },
        { "Only shows Tetragrammaton during oGCD weave window when HP conditions are met.", "等待校对 满足 HP 条件时在能力技窗口重点显示神名" },
        { "Use Tetragrammaton on GCD Option", "优先使用神名 设置" },
        { "Shows Tetragrammaton when HP conditions are met.", "等待校对 满足 HP 条件时优先显示神名" },
        { "Use Divine Benison on oGCD Option", "插入神祝祷 设置" },
        { "Only shows Divine Benison during oGCD weave window when target is not already under the effect.", "仅在目标身上没有防护罩效果时在治疗魔法GCD窗口中插入神祝祷." },
        { "Use Divine Benison on GCD Option", "优先使用神祝祷 设置" },
        { "Shows Divine Benison when target is not already under the effect.", "当目标身上没有防护罩效果时替换救疗为神祝祷." },
        { "Prioritize oGCDs Option", "神名与神祝祷排序使用 设置" },
        { "Displays oGCD Heals/Shields over Afflatus\n(only applies to GCD options for Tetragrammaton and Divine Benison).", "  在百合上显示能力技治疗/护盾。\n（仅适用于 神名 和 神祝祷 的 GCD 选项" },

        #endregion

        #endregion

        #region Medica Feature

        { "Medica Feature", "医治特性" },
        { "Replaces Medica II with Medica whenever you are under Medica II's regen.", "当你处于医济的hot效果时，替换医济为医治." },
        { "Afflatus Rapture on Medica Option", "整合狂喜之心 设置" },
        { "Adds Afflatus Rapture to the Medica Feature when available.", "当狂喜之心可用时替换为狂喜之心." },
        { "Afflatus Misery on Medica Option", "整合苦难之心 设置" },
        { "Adds Afflatus Misery to the Medica Feature when Misery is ready to be used.", "当苦难之心可用时替换为苦难之心." },
        { "Thin Air on Medica Option", "整合无中生有 设置" },
        { "Adds Thin Air to the Medica Feature when available.", "当无中生有可用时替换为无中生有." },

        #endregion

        { "Cure II Sync Feature", "救疗同步 设置" },
        { "Changes Cure II to Cure when synced below Lv.30.", "当等级同步至30级以下时替换救疗为治疗." },
        { "Changes Swiftcast to Raise.", "整合即刻咏唱至复活." },
        { "Thin Air Raise Feature", "无中生有-复活特性" },
        { "Adds Thin Air to the Global Raise Feature/Alternative Raise Feature.", "在即刻复活前插入无中生有." },

        #endregion

        #region DOH

        { "Placeholder", "Placeholder" },
        { "Placeholder.", "Placeholder." },

        #endregion

        #region DOL

        { "Eureka Feature", "理智同兴" },
        { "Replace Ageless Words and Solid Reason with Wise to the World when available.", "当理智同兴可用时，将农夫之智或石工之理替换为理智同兴" },
        { "Cast / Hook Feature", "抛竿" },
        { "Replace Cast with Hook when fishing.", "正在钓鱼时，将抛竿替换为提钩" },
        { "Cast / Gig Feature", "刺鱼" },
        { "Replace Cast with Gig when underwater.", "在水下时，将抛竿替换为刺鱼" },
        { "Surface Slap / Veteran Trade Feature", "拍击水面" },
        { "Replace Surface Slap with Veteran Trade when underwater.", "在水下时，将拍击水面替换为熟练技巧" },
        { "Prize Catch / Nature's Bounty Feature", "大鱼猎手" },
        { "Replace Prize Catch with Nature's Bounty when underwater.", "在水下时，将大鱼猎手替换为嘉惠" },
        { "Snagging / Salvage Feature", "钓组" },
        { "Replace Snagging with Salvage when underwater.", "在水下时，将钓组替换为打捞" },
        { "Cast Light / Electric Current Feature", "垂钓之光" },
        { "Replace Cast Light with Electric Current when underwater.", "在水下时，将垂钓之光替换为电水流" },

        #endregion

        #endregion

        #region PvP Combos

        #region ASTROLOGIAN

        #endregion

        #region MACHINIST

        { "Burst Mode", "爆发模式" },
        { "Turns Blast Charge into an all-in-one damage button.", "把蓄力冲击改为伤害combo按键" },
        { "Alternate Drill Mode", "等待翻译" },
        { "Saves drill for use after wildfire.", "保留钻头次数并在野火后使用" },
        { "Alternate Analysis Mode", "等待翻译" },
        { "Uses analysis with Air Anchor instead of Chain Saw.", "分析后使用空气锚，不使用回转飞锯" },

        #endregion

        #region BARD

        { "Turns Powerful Shot into an all-in-one damage button.", "把强劲射击改为伤害combo按键" },

        #endregion

        #region RED MAGE

        { "Turns Verstone/Verfire into an all-in-one damage button.", "赤飞石/赤火炎改为伤害combo按键" },

        #endregion

        #region WARRIOR

        { "Turns Heavy Swing into an all-in-one damage button.", "把重劈改为伤害combo按键" },
        { "Bloodwhetting Option", "血气选项" },
        { "Allows usage of bloodwhetting anytime, not just inbetween GCDs.", "允许随时使用原初的血气，而不是必须在GCD之间插入" },
        { "Blota Option", "献身选项" },
        { "Removes blota from main combo if Primal Rend has 5 seconds or less on its cooldown.", "如果蛮荒崩裂的冷却时间为 5 秒或更短，则从主要连击中移除献身" },

        #endregion

        #region NINJA

        { "Turns Aeolian Edge Combo into an all-in-one damage button.", "把旋风刃连击改为伤害combo按键" },
        { "AoE Burst Mode", "AOE爆发模式" },
        { "Turns Fuma Shuriken into an all-in-one AoE damage button.", "把风魔手里剑改为AOE伤害combo按键" },
        { "Uses Three Mudra on Meisui when HP under a certain threshold.", "等待翻译" },

        #endregion

        #region SAGE

        { "Turns Dosis III into an all-in-one damage button.", "把注药 III 改成伤害combo按键" },

        #endregion

        #region DANCER

        { "Turns Fountain Combo into an all-in-one damage button.", "把喷泉连改成伤害combo按键" },
        { "Honing Dance Option", "刃舞选项" },
        { "Adds Honing Dance to the main combo when in melee range (for pack pushing, respects global offset).\nThis option prevents early use of Honing Ovation!\nKeep Honing Dance bound to another key if you want to end early.", "在近战范围内将刃舞添加到combo。此选项可防止提早使用刃舞·终！如果您想提前结束，请将刃舞绑定到另一个键" },
        { "Curing Waltz Option", "治疗之华尔兹选项" },
        { "Adds Curing Waltz to the combo when available, and your HP is at or below the set percentage.", "等待翻译" },

        #endregion

        #region SAMURAI

        #region Burst Mode

        { "Adds Meikyo Shisui, Midare:Setsugekka, Ogi Namikiri, Kaeshi: Namikiri and Soten to Meikyo Shisui.\nWill only cast Midare and Ogi Namikiri when you're not moving.\nWill not use if target is guarding.", "加入明镜止水，纷乱雪月花，奥义斩浪，回返斩浪，必杀剑·早天进明镜止水。 \n只会在你不移动时使用纷乱雪月花和奥义斩浪。\n如果目标处于防御状态，将不会使用。" },
        { "Add Chiten", "添加必杀剑·地天" },
        { "Adds Chiten to the Burst Mode when in combat and HP is below 95%.", "HP低于95%时的爆发模式加入必杀剑·地天" },
        { "Add Mineuchi", "添加刀背击打" },
        { "Adds Mineuchi to the Burst Mode.", "添加刀背击打进爆发模式" },
        { "Burst Mode on Kasha Combo", "花车连击的爆发模式" },
        { "Adds Burst Mode to Kasha Combo instead.", "爆发模式加入花车连击" },

        #endregion

        #region Kasha Features

        { "PvP Features for Kasha Combo", "花车连PVP特性" },
        { "Collection of Features for Kasha Combo.", "花车连的功能集合" },
        { "Soten Gap Closer Option", "使用必杀剑·早天拉近距离" },
        { "Adds Soten when outside melee range to the Kasha Combo.", "花车连击时目标在近战范围外时使用必杀剑·早天" },
        { "AoE Melee Protection", "AOE范围近战保护" },
        { "Makes the AoE combos unusable if not in melee range of target.", "在超出近战范围时，将AOE连击变成不可用状态" },

        #endregion

        #endregion

        #region BLACK MAGE

        { "Turns Fire and Blizzard into all-in-one damage buttons.", "把火炎和冰结改为伤害combo按键" },
        { "Add Night Wing", "添加夜翼" },
        { "Adds Night Wing to the Burst Mode", "添加夜翼到爆发模式" },
        { "Add Aetherial Manipulation", "添加以太步" },
        { "Uses Aetherial Manipulation to gap close if Burst is off cooldown.", "爆发CD没好的时候，使用以太步拉近距离" },

        #endregion

        #region REAPER

        { "Turns Slice Combo into an all-in-one damage button.\nAdds Soul Slice to the main combo.", "把切割改为伤害combo按键 \n添加灵魂切割到主连击 " },
        { "Death Warrant Option", "斩首令选项" },
        { "Adds Death Warrant onto the main combo when Plentiful Harvest is ready to use, or when Plentiful Harvest's cooldown is longer than Death Warrant's.\nRespects Immortal Sacrifice Pooling Option.", "当大丰收可以使用时，或者当大丰收的冷却时间比死亡令的冷却时间长时，将斩首令添加到主连击中 \n 可以和死亡祭品的选项一同使用" },
        { "Plentiful Harvest Opener Option", "大丰收开启选项" },
        { "Starts combat with Plentiful Harvest to immediately begin Limit Break generation.", "等待翻译" },
        { "Plentiful Harvest + Immortal Sacrifice Pooling Option - BETA", "等待翻译" },
        { "Pools stacks of Immortal Sacrifice before using Plentiful Harvest.\nAlso holds Plentiful Harvest if Death Warrant is on cooldown.\nSet the value to 3 or below to use Plentiful as soon as it's available.", "等待翻译" },
        { "Enshrouded Burst Option", "等待翻译" },
        { "Puts Lemure's Slice on the main combo during Enshrouded Burst Phase.\nContains burst options.", "等待翻译" },

        #region RPR Enshrouded Option

        { "Enshrouded Death Warrant Option", "等待翻译" },
        { "Adds Death Warrant onto the main combo during the Enshroud burst when available.", "等待翻译" },
        { "Adds Communio onto the main combo when you have 1 stack of Enshroud remaining.\nWill not trigger if you are moving.", "等待翻译" },

        #endregion

        { "Ranged Harvest Moon Option", "等待翻译" },
        { "Puts Harvest Moon onto the main combo when you're out of melee range, the GCD is not rolling and it is available for use.", "等待翻译" },
        { "Adds Arcane Circle to the main combo when under a set HP perecentage.", "等待翻译" },

        #endregion

        #region MONK

        { "Turns Phantom Rush Combo into all-in-one damage button.", "将六合星导脚、凤凰舞插入连击" },
        { "Add Thunderclap", "轻声步法" },
        { "Adds Thunderclap to jump to Enemy Target when not buffed with Wind Resonance.", "没有极风神髓状态，将轻声步法替换连击" },
        { "Add Riddle of Earth", "等待翻译" },
        { "Adds Riddle of Earth and Earth's Reply to the Burst Mode when in combat.", "等待翻译" },

        #endregion

        #region DARK KNIGHT

        #endregion

        #region DRAGOON

        #endregion

        #region GUNBREAKER

        #endregion

        #region PALADIN

        #endregion

        #region SCHOLAR

        #endregion

        #region SUMMONER

        { "Turns Ruin III into an all-in-one damage button.\nOnly uses Crimson Cyclone when in melee range.", "等待翻译" },
        { "Radiant Aegis Option", "等待翻译" },
        { "Adds Radiant Aegis to the combo when available, and your HP is at or below the set percentage.", "等待翻译" },

        #endregion

        #region WARRIOR

        #endregion

        #region WHITE MAGE

        #endregion

        #endregion

        #region PvPGlobals // Pending insertion into above region

        { "Emergency Heals", "紧急自愈" },
        { "Uses Recuperate when your HP is under a certain threshold and you have the MP.", "有蓝且血量低于阈值时使用自愈" },
        { "Emergency Guard", "紧急防御" },
        { "Uses Guard when your HP is under a certain threshold.", "血量低于阈值时使用防御" },
        { "Quick Purify", "快速净化" },
        { "Uses Purify when afflicted with any selected debuff.", "受到选定的debuff时使用净化" },
        { "Prevent Mash Cancelling", "防止取消防御" },
        { "Stops you cancelling your guard if you're mashing buttons", "阻止你取消防御" },

        #endregion
    };
}