using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
            var ԭʼfancyName = fancyName;
            var ԭʼdescription = description;
            var �������� = true;
            var fancyName���ܷ��� = true;
            var description���ܷ��� = true;
            var saveWord = "�ȴ�����";


            // if (Service.Configuration != null)
            {
                // if (Service.Configuration.Language == "zh-CN")
                {
                    Dictionary<string, string> db = Translatezh_CN.db;
                    Dictionary<string, string> dbActionName = Translatezh_CN_DBActionName.dbActionName;

                    if (db.ContainsKey(ԭʼfancyName))
                    {
                        if (db[ԭʼfancyName] != saveWord)
                        {
                            fancyName = db[ԭʼfancyName];
                            �������� = false;
                            fancyName���ܷ��� = false;
                        }
                    }

                    if (fancyName���ܷ���)
                    {
                        ProcessingActionName(ԭʼfancyName, dbActionName, out fancyName);
                        if (fancyName != ԭʼfancyName)
                        {
                            db[ԭʼfancyName] = fancyName;
                            �������� = false;
                        }
                    }

                    

                    if (db.ContainsKey(ԭʼdescription))
                    {
                        if (db[ԭʼdescription]!= saveWord)
                        {
                            description = db[ԭʼdescription];
                            description���ܷ��� = false;
                            �������� = false;
                        }
                    }

                    if (description���ܷ���)
                    {
                        ProcessingActionName(ԭʼdescription, dbActionName, out description);

                        if (description != ԭʼdescription)
                        {
                            db[ԭʼdescription] = description;
                            �������� = false;
                            
                        }
                    }


                    if (��������)
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
            if (��������)
            {
                if (fancyName == saveWord)
                {
                    fancyName = ԭʼfancyName;
                }

                if (description == saveWord)
                {
                    description = ԭʼdescription;
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

        private static readonly Dictionary<uint, ClassJob> ClassJobs = Service.DataManager.GetExcelSheet<ClassJob>()!.ToDictionary(i => i.RowId, i => i);

        public static string JobIDToName(byte key) =>
            key switch
            {
                0 => "ð����(ͨ������)",
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
                18 => "������",
                19 => "��ʿ",
                20 => "��ɮ",
                21 => "սʿ",
                22 => "����ʿ",
                23 => "ʫ��",
                24 => "��ħ��ʦ",
                25 => "��ħ��ʦ",
                26 => "����ʦ",
                27 => "�ٻ�ʦ",
                28 => "ѧ��",
                29 => "˫��ʦ",
                30 => "����",
                31 => "����ʿ",
                32 => "������ʿ",
                33 => "ռ����ʿ",
                34 => "��ʿ",
                35 => "��ħ��ʦ",
                36 => "��ħ��ʦ",
                37 => "��ǹսʿ",
                38 => "����",
                39 => "������",
                40 => "����",
                99 => "Global",
                DOH.JobID => "Disciples of the Hand",
                DOL.JobID => "���ʹ��",
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
                18 => "������",
                19 => "��ʿ",
                20 => "��ɮ",
                21 => "սʿ",
                22 => "����ʿ",
                23 => "ʫ��",
                24 => "��ħ��ʦ",
                25 => "��ħ��ʦ",
                26 => "����ʦ",
                27 => "�ٻ�ʦ",
                28 => "ѧ��",
                29 => "˫��ʦ",
                30 => "����",
                31 => "����ʿ",
                32 => "������ʿ",
                33 => "ռ����ʿ",
                34 => "��ʿ",
                35 => "��ħ��ʦ",
                36 => "��ħ��ʦ",
                37 => "��ǹսʿ",
                38 => "����",
                39 => "������",
                40 => "����",
                99 => "Global",
                DOH.JobID => "Disciples of the Hand",
                DOL.JobID => "���ʹ��",
                _ => "Unknown",
            };
    }
}
