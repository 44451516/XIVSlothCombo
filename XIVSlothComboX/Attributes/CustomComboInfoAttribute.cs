using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Dalamud.Game;
using ECommons.DalamudServices;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Attributes
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
                        if (db[原始description] != saveWord)
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

        /// <summary> Gets the job role. </summary>
        public int Role => JobIDToRole(JobID);

        private int JobIDToRole(byte jobID)
        {
            if (Service.DataManager.GetExcelSheet<ClassJob>().HasRow(jobID))
                return Service.DataManager.GetExcelSheet<ClassJob>().GetRow(jobID).Role;

            return 0;
        }
        
        /// <summary> Gets the display order. </summary>
        public int Order { get; }

        /// <summary> Gets the job name. </summary>
        public string JobName => JobIDToName(JobID);

        public string JobShorthand => JobIDToShorthand(JobID);

        private string JobIDToShorthand(byte key)
        {
            if (ClassJobs.TryGetValue(key, out var job))
            {
                return job.Abbreviation.RawString;
            }
            else
            {
                return "";
            }
        }


        private static readonly Dictionary<uint, ClassJob> ClassJobs = Service.DataManager.GetExcelSheet<ClassJob>()!.ToDictionary(i => i.RowId, i => i);

        public static string JobIDToName(byte key)
        {
            if (key == 0)
                return "通用职业";

            if (key == 51)
            {
                ;
                ;
                ;
            }

            //Override DOH/DOL
            if (key is DOH.JobID) 
                key = 08; //Set to Carpenter
            if (key is DOL.JobID) 
                key = 16; //Set to Miner
            if (ClassJobs.TryGetValue(key, out ClassJob? job))
            {
                //Grab Category name for DOH/DOL, else the normal Name for the rest
                // string jobname = key is 08 or 16 ? job.ClassJobCategory.Value.Name : job.Name;
                string jobname = key is 08 or 16 ? "大地使者" : job.Name;
                //Job names are all lowercase by default. This capitalizes based on regional rules
                string cultureID = Service.ClientState.ClientLanguage switch
                {
                    ClientLanguage.French => "fr-FR",
                    ClientLanguage.Japanese => "ja-JP",
                    ClientLanguage.German => "de-DE",
                    _ => "en-us",
                };
                TextInfo textInfo = new CultureInfo(cultureID, false).TextInfo;
                jobname = textInfo.ToTitleCase(jobname);
                //if (key is 0) jobname = " " + jobname; //Adding space to the front of Global moves it to the top. Shit hack but works
                return jobname;

            } //Misc or unknown
            else return key == 99 ? "Global" : "Unknown";
        }

        /// <summary> Gets the meme job name. </summary>
        public string MemeJobName => MemeJobIDToName(JobID);

        private static string MemeJobIDToName(byte key) => key switch
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
            18 => "Fisher",
            19 => "Paladin",
            20 => "Monk",
            21 => "Warrior",
            22 => "Dragoon",
            23 => "Bard",
            24 => "White Mage",
            25 => "Black Mage",
            26 => "Arcanist",
            27 => "Summoner",
            28 => "Scholar",
            29 => "Rogue",
            30 => "Ninja",
            31 => "Machinist",
            32 => "Dark Knight",
            33 => "Astrologian",
            34 => "Samurai",
            35 => "Red Mage",
            36 => "Blue Mage",
            37 => "Gunbreaker",
            38 => "Dancer",
            39 => "Reaper",
            40 => "Sage",
            99 => "Global",
            DOH.JobID => "Disciples of the 手工艺",
            DOL.JobID => "Disciples of the 大地使者",
            _ => "Unknown",
        };

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
    }
}