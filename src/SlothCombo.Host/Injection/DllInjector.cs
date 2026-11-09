using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SlothCombo.Host.Injection;

/// <summary>
/// Loads a library into another process and starts it.
/// </summary>
/// <remarks>
/// <para>
/// The library is loaded by the same entry point the loader of the system
/// offers: the address of <c>LoadLibrary</c> is looked up in the host program
/// and a thread is started in the game that calls it with the path of the
/// library.
/// </para>
/// <para>
/// A library on its own does nothing, so the export that starts the work is
/// called afterwards. The address of the export in the game is the address of
/// the export in this program plus the place the library was loaded at in the
/// game.
/// </para>
/// <para>
/// This is the same mechanism the plugin host of the game uses, so nothing
/// here tries to hide the library from the game or from the operating
/// system.
/// </para>
/// </remarks>
internal static unsafe partial class DllInjector
{
    /// <summary>The right to create a thread in the process.</summary>
    private const uint ProcessCreateThread = 0x0002;

    /// <summary>The right to look at the process.</summary>
    private const uint ProcessQueryInformation = 0x0400;

    /// <summary>The right to change the memory of the process.</summary>
    private const uint ProcessVmOperation = 0x0008;

    /// <summary>The right to read the memory of the process.</summary>
    private const uint ProcessVmRead = 0x0010;

    /// <summary>The right to write the memory of the process.</summary>
    private const uint ProcessVmWrite = 0x0020;

    /// <summary>The flag that reserves memory for the path of the library.</summary>
    private const uint MemCommit = 0x1000;

    /// <summary>The flag that keeps the memory reserved.</summary>
    private const uint MemReserve = 0x2000;

    /// <summary>The protection that allows the memory to be written to.</summary>
    private const uint PageReadWrite = 0x04;

    /// <summary>The result of a wait that ran into its time limit.</summary>
    private const uint WaitTimeout = 0x102;

    /// <summary>The flag of the list of the modules of a process.</summary>
    private const uint SnapModule = 0x00000008;

    /// <summary>The flag of the list of the modules of a process of another kind.</summary>
    private const uint SnapModule32 = 0x00000010;

    /// <summary>How often the list of the modules is read again after a failure.</summary>
    private const int SnapshotAttempts = 10;

    /// <summary>How long the loader is given to load the library.</summary>
    private const int LoadTimeoutMilliseconds = 30_000;

    /// <summary>How long the export is given to do its work.</summary>
    private const int EntryTimeoutMilliseconds = 60_000;

    /// <summary>
    /// Loads the given library into the process and starts it.
    /// </summary>
    /// <param name="processId">The id of the process.</param>
    /// <param name="libraryPath">The full path of the library.</param>
    /// <param name="exportName">The name of the export that starts the work.</param>
    /// <exception cref="InjectionException">The library could not be loaded or started.</exception>
    public static void Inject(int processId, string libraryPath, string exportName)
    {
        if (!File.Exists(libraryPath))
        {
            throw new InjectionException($"The library '{libraryPath}' does not exist.");
        }

        var exportRva = PeExportReader.GetExportRva(libraryPath, exportName);

        var handle = OpenProcess(
            ProcessCreateThread | ProcessQueryInformation | ProcessVmOperation | ProcessVmRead | ProcessVmWrite,
            false,
            processId);

        if (handle == IntPtr.Zero)
        {
            throw new InjectionException(
                $"The game could not be opened (error {Marshal.GetLastWin32Error()}).");
        }

        try
        {
            Load(handle, libraryPath);

            var remote = FindRemoteModule(handle, processId, Path.GetFileName(libraryPath));
            if (remote == IntPtr.Zero)
            {
                throw new InjectionException("The library was not found in the game.");
            }

            Call(handle, new IntPtr(remote.ToInt64() + exportRva));
        }
        finally
        {
            CloseHandle(handle);
        }
    }

    /// <summary>
    /// Loads the library into the process that is already open.
    /// </summary>
    private static void Load(IntPtr process, string libraryPath)
    {
        var path = Encoding.Unicode.GetBytes(libraryPath + '\0');
        var size = (IntPtr)path.Length;

        var remote = VirtualAllocEx(process, IntPtr.Zero, size, MemCommit | MemReserve, PageReadWrite);
        if (remote == IntPtr.Zero)
        {
            throw new InjectionException(
                $"The memory for the path of the library could not be reserved (error {Marshal.GetLastWin32Error()}).");
        }

        try
        {
            if (!WriteProcessMemory(process, remote, path, size, out _))
            {
                throw new InjectionException(
                    $"The path of the library could not be written (error {Marshal.GetLastWin32Error()}).");
            }

            var loader = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryW");
            if (loader == IntPtr.Zero)
            {
                throw new InjectionException("The loader of the system could not be found.");
            }

            var thread = CreateRemoteThread(process, IntPtr.Zero, IntPtr.Zero, loader, remote, 0, out _);
            if (thread == IntPtr.Zero)
            {
                throw new InjectionException(
                    $"The loader could not be started (error {Marshal.GetLastWin32Error()}).");
            }

            try
            {
                var result = WaitForSingleObject(thread, LoadTimeoutMilliseconds);
                if (result != 0)
                {
                    throw new InjectionException("The library was not loaded in time.");
                }

                if (!GetExitCodeThread(thread, out var module) || module == 0)
                {
                    throw new InjectionException(
                        $"The library could not be loaded (error {Marshal.GetLastWin32Error()}).");
                }
            }
            finally
            {
                CloseHandle(thread);
            }
        }
        finally
        {
            VirtualFreeEx(process, remote, IntPtr.Zero, 0x8000);
        }
    }

    /// <summary>
    /// Calls a function of the game.
    /// </summary>
    private static void Call(IntPtr process, IntPtr address)
    {
        var thread = CreateRemoteThread(process, IntPtr.Zero, IntPtr.Zero, address, IntPtr.Zero, 0, out _);
        if (thread == IntPtr.Zero)
        {
            throw new InjectionException(
                $"The export could not be started (error {Marshal.GetLastWin32Error()}).");
        }

        try
        {
            var result = WaitForSingleObject(thread, EntryTimeoutMilliseconds);
            if (result != 0)
            {
                throw new InjectionException("The export did not finish in time.");
            }

            GetExitCodeThread(thread, out var exitCode);
            if (exitCode != 0)
            {
                throw new InjectionException($"The export reported a failure ({exitCode}).");
            }
        }
        finally
        {
            CloseHandle(thread);
        }
    }

    /// <summary>
    /// Finds where the library was loaded in the game.
    /// </summary>
    /// <remarks>
    /// The loader of the system hands the place of the library back as the
    /// result of its thread, which is too small to hold an address on a
    /// system of sixty four bits. The list of the modules of the game holds
    /// the whole address, so the library is looked up there instead.
    /// </remarks>
    private static IntPtr FindRemoteModule(IntPtr process, int processId, string moduleName)
    {
        for (var attempt = 0; attempt < SnapshotAttempts; attempt++)
        {
            var snapshot = CreateToolhelp32Snapshot(SnapModule | SnapModule32, (uint)processId);
            if (snapshot == IntPtr.Zero || snapshot == new IntPtr(-1))
            {
                ThreadSleep();
                continue;
            }

            try
            {
                var entry = new ModuleEntry32
                {
                    Size = (uint)sizeof(ModuleEntry32),
                };

                if (!Module32First(snapshot, ref entry))
                {
                    ThreadSleep();
                    continue;
                }

                do
                {
                    if (Matches(entry, moduleName))
                    {
                        return entry.BaseAddress;
                    }
                }
                while (Module32Next(snapshot, ref entry));
            }
            finally
            {
                CloseHandle(snapshot);
            }

            ThreadSleep();
        }

        return IntPtr.Zero;
    }

    /// <summary>
    /// Checks whether the module of the list is the one that was looked for.
    /// </summary>
    private static bool Matches(ModuleEntry32 entry, string moduleName)
    {
        var name = new string(entry.Name);
        return string.Equals(name, moduleName, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Waits a moment before the list of the modules is read again.
    /// </summary>
    private static void ThreadSleep() => System.Threading.Thread.Sleep(100);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial IntPtr OpenProcess(uint access, [MarshalAs(UnmanagedType.Bool)] bool inheritHandle, int processId);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CloseHandle(IntPtr handle);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial IntPtr VirtualAllocEx(
        IntPtr process,
        IntPtr address,
        IntPtr size,
        uint allocationType,
        uint protection);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool VirtualFreeEx(IntPtr process, IntPtr address, IntPtr size, uint freeType);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool WriteProcessMemory(
        IntPtr process,
        IntPtr address,
        byte[] buffer,
        IntPtr size,
        out IntPtr written);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial IntPtr CreateRemoteThread(
        IntPtr process,
        IntPtr attributes,
        IntPtr stackSize,
        IntPtr startAddress,
        IntPtr parameter,
        uint creationFlags,
        out uint threadId);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial uint WaitForSingleObject(IntPtr handle, uint milliseconds);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetExitCodeThread(IntPtr thread, out uint exitCode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial IntPtr CreateToolhelp32Snapshot(uint flags, uint processId);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool Module32First(IntPtr snapshot, ref ModuleEntry32 entry);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool Module32Next(IntPtr snapshot, ref ModuleEntry32 entry);

    [LibraryImport("kernel32.dll", StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr GetModuleHandle(string moduleName);

    [LibraryImport("kernel32.dll", StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr GetProcAddress(IntPtr module, string procedureName);

    /// <summary>One module of a process.</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct ModuleEntry32
    {
        /// <summary>Gets or sets the size of the entry.</summary>
        public uint Size;

        /// <summary>Gets or sets the number of the module.</summary>
        public uint ModuleId;

        /// <summary>Gets or sets the number of the process.</summary>
        public uint ProcessId;

        /// <summary>Gets or sets the number of the users of the module.</summary>
        public uint GlobalUsage;

        /// <summary>Gets or sets the number of the users of the module.</summary>
        public uint ProcessUsage;

        /// <summary>Gets or sets the place of the module in the game.</summary>
        public IntPtr BaseAddress;

        /// <summary>Gets or sets the size of the module.</summary>
        public uint BaseSize;

        /// <summary>Gets or sets the handle of the module.</summary>
        public IntPtr Module;

        /// <summary>Gets or sets the name of the module.</summary>
        public fixed char Name[256];

        /// <summary>Gets or sets the path of the module.</summary>
        public fixed char Path[260];
    }
}

/// <summary>The reason a library could not be loaded into the game.</summary>
internal sealed class InjectionException : Exception
{
    /// <summary>Creates the exception with the given message.</summary>
    /// <param name="message">The reason of the failure.</param>
    public InjectionException(string message) : base(message)
    {
    }

    /// <summary>Creates the exception with the given message and cause.</summary>
    /// <param name="message">The reason of the failure.</param>
    /// <param name="inner">The failure that caused this one.</param>
    public InjectionException(string message, Exception inner) : base(message, inner)
    {
    }
}
