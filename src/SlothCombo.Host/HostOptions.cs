using System;
using System.Collections.Generic;
using System.IO;

namespace SlothCombo.Host;

/// <summary>
/// The settings the program is started with.
/// </summary>
internal sealed class HostOptions
{
    /// <summary>Gets or sets the process of the game to attach to.</summary>
    public int? ProcessId { get; set; }

    /// <summary>Gets or sets the folder the runtime keeps its data in.</summary>
    public string? DataFolder { get; set; }

    /// <summary>Gets or sets the installation directory of the game.</summary>
    public string? GameInstallPath { get; set; }

    /// <summary>Gets or sets a value indicating whether the help was asked for.</summary>
    public bool ShowHelp { get; set; }

    /// <summary>Gets or sets a value indicating whether the browser stays closed.</summary>
    public bool NoBrowser { get; set; }

    /// <summary>
    /// Reads the settings from the given arguments.
    /// </summary>
    /// <param name="args">The arguments of the program.</param>
    /// <returns>The settings.</returns>
    public static HostOptions Parse(string[] args)
    {
        var options = new HostOptions();

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--pid":
                    options.ProcessId = int.Parse(ReadValue(args, ref i, "--pid"));
                    break;

                case "--data":
                    options.DataFolder = ReadValue(args, ref i, "--data");
                    break;

                case "--game":
                    options.GameInstallPath = ReadValue(args, ref i, "--game");
                    break;

                case "--help" or "-h" or "/?":
                    options.ShowHelp = true;
                    break;

                case "--no-browser":
                    options.NoBrowser = true;
                    break;

                default:
                    throw new ArgumentException($"'{args[i]}' is not an option of the program.");
            }
        }

        return options;
    }

    /// <summary>
    /// Reads the value that belongs to an option.
    /// </summary>
    private static string ReadValue(string[] args, ref int index, string name)
    {
        if (index + 1 >= args.Length)
        {
            throw new ArgumentException($"The option '{name}' needs a value.");
        }

        index++;
        return args[index];
    }

    /// <summary>
    /// Writes the help of the program to the output.
    /// </summary>
    public static void WriteHelp()
    {
        Console.WriteLine("SlothCombo - the standalone helper of XIVSlothComboX");
        Console.WriteLine();
        Console.WriteLine("Usage: SlothCombo.Host [options]");
        Console.WriteLine();
        Console.WriteLine("  --pid <id>    the process of the game to attach to");
        Console.WriteLine("  --data <dir>  the folder the runtime keeps its data in");
        Console.WriteLine("  --game <dir>  the installation directory of the game");
        Console.WriteLine("  --no-browser  do not open the settings page in the browser");
        Console.WriteLine("  --help        show this text");
    }
}
