using System;
using System.IO;
using SlothCombo.Host.Injection;

namespace SlothCombo.Tests;

/// <summary>
/// The tests of the reader of the exports of a program file.
/// </summary>
/// <remarks>
/// The injector has to know where the export of the boot library sits inside
/// the library, because the address of the export in the game is built from
/// the address of the library and that place inside it.
/// </remarks>
internal static class PeExportTest
{
    /// <summary>
    /// Finds an export of a library of the system.
    /// </summary>
    public static void FindsSystemExport()
    {
        var kernel32 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "kernel32.dll");
        Assert.True(File.Exists(kernel32), "The loader of the system was not found.");

        var rva = PeExportReader.GetExportRva(kernel32, "LoadLibraryW");
        Assert.True(rva > 0, "The export of the loader of the system was not found.");
    }

    /// <summary>
    /// Finds the export of the boot library.
    /// </summary>
    public static void FindsBootExport()
    {
        var distribution = Distribution.Find();
        if (distribution is null)
        {
            throw new SkipException("The installation was not found. Run build/publish.ps1 first.");
        }

        var library = Path.Combine(distribution, Distribution.BootLibraryName);
        var rva = PeExportReader.GetExportRva(library, "SlothComboBoot");
        Assert.True(rva > 0, "The export of the boot library was not found.");
    }

    /// <summary>
    /// Reports an export that is not there.
    /// </summary>
    public static void RejectsUnknownExport()
    {
        var kernel32 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "kernel32.dll");
        Assert.True(File.Exists(kernel32), "The loader of the system was not found.");

        var refused = false;
        try
        {
            PeExportReader.GetExportRva(kernel32, "SlothComboNoSuchExport");
        }
        catch (InvalidDataException)
        {
            refused = true;
        }

        Assert.True(refused, "An export that is not there was found.");
    }
}
