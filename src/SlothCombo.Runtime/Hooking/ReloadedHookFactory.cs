using System;
using Reloaded.Hooks;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using SlothCombo.Abstractions;

namespace SlothCombo.Runtime.Hooking;

/// <summary>
/// A detour installed through the Reloaded hooking library.
/// </summary>
/// <typeparam name="TDelegate">The delegate type of the hooked function.</typeparam>
internal sealed class ReloadedNativeHook<TDelegate> : INativeHook<TDelegate> where TDelegate : Delegate
{
    private readonly IHook<TDelegate> _hook;

    /// <summary>Wraps the given hook.</summary>
    public ReloadedNativeHook(IHook<TDelegate> hook) => _hook = hook;

    /// <inheritdoc/>
    public IntPtr Address => _hook.OriginalFunctionAddress;

    /// <inheritdoc/>
    public bool IsEnabled => _hook.IsHookEnabled;

    /// <inheritdoc/>
    public TDelegate Original => _hook.OriginalFunction;

    /// <inheritdoc/>
    public void Enable() => _hook.Activate();

    /// <inheritdoc/>
    public void Disable() => _hook.Disable();

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_hook.IsHookEnabled)
        {
            _hook.Disable();
        }
    }
}

/// <summary>
/// Creates detours through the Reloaded hooking library.
/// </summary>
/// <remarks>
/// <para>
/// This is the replacement for the Dalamud game interop provider. The detour
/// has to be declared with the calling convention attribute so the library
/// knows how to move the arguments across the native boundary.
/// </para>
/// <para>
/// The hook is handed back disabled, matching the behaviour of the Dalamud
/// equivalent: the caller decides when the detour goes live.
/// </para>
/// </remarks>
internal sealed class ReloadedHookFactory : IHookFactory
{
    private readonly ReloadedHooks _hooks = ReloadedHooks.Instance;

    /// <inheritdoc/>
    public INativeHook<TDelegate> HookFromAddress<TDelegate>(IntPtr address, TDelegate detour)
        where TDelegate : Delegate
    {
        if (address == IntPtr.Zero)
        {
            throw new ArgumentException("The address of the function to hook is null.", nameof(address));
        }

        var hook = _hooks.CreateHook(detour, address.ToInt64());
        return new ReloadedNativeHook<TDelegate>(hook);
    }

    /// <inheritdoc/>
    public INativeHook<TDelegate> HookFromSignature<TDelegate>(string signature, TDelegate detour)
        where TDelegate : Delegate
    {
        // Signature based hooking is only used for the handful of addresses that
        // are not covered by the struct library. Those are resolved through the
        // scanner, which the runtime sets up before the first hook is created.
        var scanner = SignatureScanner.Current
                      ?? throw new InvalidOperationException("The signature scanner has not been initialised.");

        var address = scanner.ScanText(signature);
        if (address == IntPtr.Zero)
        {
            throw new InvalidOperationException($"The signature '{signature}' could not be resolved.");
        }

        return HookFromAddress(address, detour);
    }
}

/// <summary>
/// The calling convention attribute for a detour.
/// </summary>
/// <remarks>
/// The game is compiled for the Windows x64 calling convention, so every
/// detour needs the matching attribute for the library to marshal the
/// arguments correctly.
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
internal sealed class GameFunctionAttribute : Attribute
{
    /// <summary>Marks a method as a native game function.</summary>
    public GameFunctionAttribute()
    {
    }
}
