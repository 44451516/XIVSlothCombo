using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dalamud.Logging;
using XIVSlothCombo.Combos.PvE;
using XIVSlothCombo.Services;

namespace XIVSlothCombo.Attributes
{
    /// <summary> Attribute documenting additional information for each combo. </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class CustomComboInfoAttribute : Attribute
    {
        /// <summary> Initializes a new instance of the <see cref="CustomComboInfoAttribute"/> class. </summary>
        /// <param name="fancyName"> Display name. </param>
        /// <param name="description"> Combo description. </param>
        /// <param name="jobID"> Associated job ID. </param>
        /// <param name="order"> Display order. </param>
        /// <param name="memeName"> Display meme name </param>
        /// <param name="memeDescription"> Meme description. </param>
        internal CustomComboInfoAttribute(string fancyName, string description, byte jobID, [CallerLineNumber] int order = 0, string memeName = "", string memeDescription = "")
        {
            var 原始fancyName = fancyName;
            var 原始description = description;
            var 增加搜索 = true;
            var fancyName技能翻译 = true;
            var description技能翻译 = true;
            var saveWord = "等待翻译";


            // if (Service.Configuration != null)
            {
                // if (Service.Configuration.Language == "zh-CN")
                {
                    Dictionary<string, string> db = Translatezh_CN.db;
                    Dictionary<string, string> dbActionName = Translatezh_CN_DBActionName.dbActionName;

                    if (db.ContainsKey(原始fancyName))
                    {
                        if (db[原始fancyName] != saveWord)
                        {
                            fancyName = db[原始fancyName];
                            增加搜索 = false;
                            fancyName技能翻译 = false;
                        }
                    }

                    if (fancyName技能翻译)
                    {
                        ProcessingActionName(原始fancyName, dbActionName, out fancyName);
                        if (fancyName != 原始fancyName)
                        {
                            db[原始fancyName] = fancyName;
                            增加搜索 = false;
                        }
                    }

                    

                    if (db.ContainsKey(原始description))
                    {
                        if (db[原始description]!= saveWord)
                        {
                            description = db[原始description];
                            description技能翻译 = false;
                            增加搜索 = false;
                        }
                    }

                    if (description技能翻译)
                    {
                        ProcessingActionName(原始description, dbActionName, out description);

                        if (description != 原始description)
                        {
                            db[原始description] = description;
                            增加搜索 = false;
                            
                        }
                    }


                    if (增加搜索)
                    {
                        try
                        {
                            var replaceOption = fancyName.Replace(" Option", "");

                            if (db.ContainsKey($"{replaceOption}"))
                            {
                                fancyName = db[$"{replaceOption}"];
                            }

                            if (db.ContainsKey($"{replaceOption}"))
                            {
                                description = db[$"{replaceOption}"];
                            }
                        }
                        catch (Exception e)
                        {
                            // PluginLog.Information($"log fancyName:{fancyName} description:{description} {e.Message}");

                            // Console.WriteLine(e);
                            // throw;
                        }
                    }
                }
            }

            // if (fancyName .Equals("No Mercy AOE Option.") )
            // {
            //     fancyName = "adsadas";
            // }
            if (增加搜索)
            {
                if (fancyName == saveWord)
                {
                    fancyName = 原始fancyName;
                }

                if (description == saveWord)
                {
                    description = 原始description;
                }
            }


            FancyName = fancyName;
            Description = description;
            JobID = jobID;
            Order = order;
            MemeName = memeName;
            MemeDescription = memeDescription;
        }

        public static void ProcessingActionName(string sentence, Dictionary<string, string> dbActionName, out string output)
        {
            output = sentence;
            var split_sentence = sentence.Replace('\n', ' ').Split(' ');
            for (int i = 0; i < split_sentence.Length; i++)
            {
                if (split_sentence[i].Contains("/"))
                {
                    if (dbActionName.ContainsKey(split_sentence[i]))
                    {
                        split_sentence[i] = dbActionName[split_sentence[i]];
                    }
                }

                if (i < split_sentence.Length - 2)
                {
                    var new_word = split_sentence[i] + " " + split_sentence[i + 1] + " " + split_sentence[i + 2];
                    if (dbActionName.ContainsKey(new_word))
                    {
                        output = output.Replace(new_word, dbActionName[new_word]);
                        continue;
                    }
                }

                if (i < split_sentence.Length - 1)
                {
                    var new_word = split_sentence[i] + " " + split_sentence[i + 1];
                    if (dbActionName.ContainsKey(new_word))
                    {
                        output = output.Replace(new_word, dbActionName[new_word]);
                        continue;
                    }
                }

                if (dbActionName.ContainsKey(split_sentence[i]))
                {
                    output = output.Replace(split_sentence[i], dbActionName[split_sentence[i]]);
                }
            }
        }

        /// <summary> Gets the display name. </summary>
        public string FancyName { get; }

        ///<summary> Gets the meme name. </summary>
        public string MemeName { get; }

        /// <summary> Gets the description. </summary>
        public string Description { get; }

        /// <summary> Gets the meme description. </summary>
        public string MemeDescription { get; }

        /// <summary> Gets the job ID. </summary>
        public byte JobID { get; }

        /// <summary> Gets the display order. </summary>
        public int Order { get; }

        /// <summary> Gets the job name. </summary>
        public string JobName => JobIDToName(JobID);

        /// <summary> Gets the meme job name. </summary>
        public string MemeJobName => MemeJobIDToName(JobID);

        private static string JobIDToName(byte key) =>
            key switch
            {
                0 => "冒险者(通用设置)",
                1 => "Gladiator",
                2 => "Pugilist",
                3 => "Marauder",
                4 => "Lancer",
                5 => "Archer",
                6 => "Conjurer",
                7 => "Thaumaturge",
                8 => "Carpenter",
                9 => "Blacksmith",
                10 => "Armorer",
                11 => "Goldsmith",
                12 => "Leatherworker",
                13 => "Weaver",
                14 => "Alchemist",
                15 => "Culinarian",
                16 => "Miner",
                17 => "Botanist",
                18 => "捕鱼人",
                19 => "骑士",
                20 => "武僧",
                21 => "战士",
                22 => "龙骑士",
                23 => "诗人",
                24 => "白魔法师",
                25 => "黑魔法师",
                26 => "秘术师",
                27 => "召唤师",
                28 => "学者",
                29 => "双剑师",
                30 => "忍者",
                31 => "机工士",
                32 => "暗黑骑士",
                33 => "占星术士",
                34 => "武士",
                35 => "赤魔法师",
                36 => "青魔法师",
                37 => "绝枪战士",
                38 => "舞者",
                39 => "钐镰客",
                40 => "贤者",
                99 => "Global",
                DOH.JobID => "Disciples of the Hand",
                DoL.JobID => "大地使者",
                _ => "Unknown",
            };

        private static string MemeJobIDToName(byte key) =>
            key switch
            {
                0 => "Adventurer",
                1 => "Gladiator",
                2 => "Pugilist",
                3 => "Marauder",
                4 => "Lancer",
                5 => "Archer",
                6 => "Conjurer",
                7 => "Thaumaturge",
                8 => "Carpenter",
                9 => "Blacksmith",
                10 => "Armorer",
                11 => "Goldsmith",
                12 => "Leatherworker",
                13 => "Weaver",
                14 => "Alchemist",
                15 => "Culinarian",
                16 => "Miner",
                17 => "Botanist",
                18 => "捕鱼人",
                19 => "骑士",
                20 => "武僧",
                21 => "战士",
                22 => "龙骑士",
                23 => "诗人",
                24 => "白魔法师",
                25 => "黑魔法师",
                26 => "秘术师",
                27 => "召唤师",
                28 => "学者",
                29 => "双剑师",
                30 => "忍者",
                31 => "机工士",
                32 => "暗黑骑士",
                33 => "占星术士",
                34 => "武士",
                35 => "赤魔法师",
                36 => "青魔法师",
                37 => "绝枪战士",
                38 => "舞者",
                39 => "钐镰客",
                40 => "贤者",
                99 => "Global",
                DOH.JobID => "Disciples of the Hand",
                DoL.JobID => "大地使者",
                _ => "Unknown",
            };
    }
}