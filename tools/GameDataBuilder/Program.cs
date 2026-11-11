using System.Net.Http;

namespace SlothCombo.GameDataBuilder;

/// <summary>
/// The program that turns the published data of the game into the tables the
/// program reads.
/// </summary>
/// <remarks>
/// <para>
/// The data of the game is published as a file per sheet and per language.
/// The tool reads those files and writes the tables of the program, which
/// hold only the columns the program has a use for.
/// </para>
/// <para>
/// The tool is run by hand whenever the data of the game changes. It is not
/// part of the installation of the program.
/// </para>
/// </remarks>
internal static class Program
{
    /// <summary>The place the data of the game is published at.</summary>
    private const string SourceRoot =
        "https://raw.githubusercontent.com/Souma-Sumire/ffxiv-datamining-hexcode-mixed/main";

    /// <summary>The languages the tables are written for.</summary>
    private static readonly string[] Languages = ["chs", "en", "ja", "de", "fr", "ko"];

    /// <summary>The sheets the program reads.</summary>
    private static readonly SheetPlan[] Plans =
    [
        new("Action", SheetColumns.Action),
        new("ActionCategory", SheetColumns.ActionCategory),
        new("Status", SheetColumns.Status),
        new("Item", SheetColumns.Item),
        new("ItemAction", SheetColumns.ItemAction),
        new("ClassJob", SheetColumns.ClassJob),
        new("ClassJobCategory", SheetColumns.ClassJobCategory, "jobs"),
        new("Trait", SheetColumns.Trait),
        new("Quest", SheetColumns.Quest),
        new("TerritoryType", SheetColumns.TerritoryType),
        new("PlaceName", SheetColumns.PlaceName),
        new("LogMessage", SheetColumns.LogMessage),
        new("BNpcBase", SheetColumns.BNpcBase),
        new("AozAction", SheetColumns.AozAction),
        new("AozActionTransient", SheetColumns.AozActionTransient),
        new("ItemFood", SheetColumns.ItemFood),
        new("EventItem", SheetColumns.EventItem),
    ];

    /// <summary>Runs the tool.</summary>
    /// <param name="args">The folder to write the tables to, and the languages to write them for.</param>
    /// <returns>The code the program ends with.</returns>
    public static async Task<int> Main(string[] args)
    {
        var outputFolder = args.Length > 0 ? args[0] : "GameData";
        var languages = args.Length > 1 ? args[1..] : Languages;

        var cacheFolder = Path.Combine(Path.GetTempPath(), "sloth-gamedata-cache");
        Directory.CreateDirectory(cacheFolder);

        using var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };

        foreach (var language in languages)
        {
            var folder = Path.Combine(outputFolder, language);
            Directory.CreateDirectory(folder);

            foreach (var plan in Plans)
            {
                var source = Path.Combine(cacheFolder, $"{language}-{plan.Name}.csv");

                if (!File.Exists(source))
                {
                    Console.WriteLine($"Downloading {plan.Name} of {language}...");
                    var url = $"{SourceRoot}/{language}/{plan.Name}.csv";
                    var bytes = await client.GetByteArrayAsync(url).ConfigureAwait(false);
                    await File.WriteAllBytesAsync(source, bytes).ConfigureAwait(false);
                }
                else
                {
                    Console.WriteLine($"Using the copy of {plan.Name} of {language}.");
                }

                var lines = await File.ReadAllLinesAsync(source).ConfigureAwait(false);
                var sheet = SheetParser.Parse(lines);

                var target = Path.Combine(folder, $"{plan.Name}.json.gz");
                SheetWriter.Write(sheet, plan, target);

                // The table is read back right away, so that a table the
                // runtime would not be able to read never leaves the tool.
                var count = TableChecker.Check(plan.Name, target);

                Console.WriteLine($"  {plan.Name}: {count} rows.");
            }
        }

        Console.WriteLine($"The tables are in '{outputFolder}'.");
        return 0;
    }
}
