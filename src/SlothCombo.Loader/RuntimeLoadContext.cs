using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace SlothCombo.Loader;

/// <summary>
/// The load context the runtime of the game lives in.
/// </summary>
/// <remarks>
/// The context is collectible, which is what makes a replacement possible:
/// unloading it is the only way to let go of the assembly and of the files it
/// was read from. Every dependency of the runtime is taken from the folder
/// the runtime sits in, so a build carries its own set of assemblies and
/// nothing is left behind in the default context.
/// </remarks>
internal sealed class RuntimeLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver? _resolver;

    /// <summary>Creates the context for the assembly of the given runtime.</summary>
    /// <param name="runtimeAssemblyPath">The path of the runtime of the game.</param>
    /// <param name="name">The name the context is known by.</param>
    /// <remarks>
    /// A build of the runtime always carries the file that lists its
    /// dependencies. An assembly without dependencies may not have one, and
    /// is then served by the default context alone.
    /// </remarks>
    public RuntimeLoadContext(string runtimeAssemblyPath, string name)
        : base(name, isCollectible: true)
    {
        var dependencies = Path.ChangeExtension(runtimeAssemblyPath, ".deps.json");
        _resolver = File.Exists(dependencies)
            ? new AssemblyDependencyResolver(runtimeAssemblyPath)
            : null;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// A dependency that is not part of the runtime is handed back to the
    /// default context, which owns the runtime of the system and everything
    /// that comes with it.
    /// </remarks>
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var path = _resolver?.ResolveAssemblyToPath(assemblyName);
        return path is null ? null : LoadFromAssemblyPath(path);
    }

    /// <inheritdoc/>
    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var path = _resolver?.ResolveUnmanagedDllToPath(unmanagedDllName);
        return path is null ? IntPtr.Zero : LoadUnmanagedDllFromPath(path);
    }
}
