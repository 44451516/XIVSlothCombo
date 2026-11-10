using System;

namespace SlothCombo.Abstractions;

/// <summary>
/// A detour installed on a native function.
/// </summary>
/// <remarks>
/// The legacy plugin used <c>Dalamud.Hooking.Hook&lt;T&gt;</c> obtained from
/// <c>Service.GameInteropProvider</c>. This interface exposes the same shape so
/// the hook sites in the combo logic stay unchanged, while the standalone
/// runtime implements it on top of Reloaded.Hooks.
/// </remarks>
/// <typeparam name="TDelegate">The delegate type of the hooked function.</typeparam>
public interface INativeHook<TDelegate> : IDisposable where TDelegate : Delegate
{
    /// <summary>Gets the address of the hooked function.</summary>
    IntPtr Address { get; }

    /// <summary>Gets a value indicating whether the detour is currently installed.</summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Gets the original function, which is called to let the game continue
    /// with the action it wanted to take.
    /// </summary>
    TDelegate Original { get; }

    /// <summary>Installs the detour.</summary>
    void Enable();

    /// <summary>Removes the detour.</summary>
    void Disable();
}

/// <summary>
/// Creates detours on native functions.
/// </summary>
/// <remarks>
/// Implementations must be usable for the full lifetime of the process: a detour
/// that is installed while the game is calling the hooked function is the most
/// common source of crashes, so the implementation is expected to install
/// detours on a stable, already-warm code path.
/// </remarks>
public interface IHookFactory
{
    /// <summary>Hooks the native function at the given address.</summary>
    /// <typeparam name="TDelegate">The delegate type of the hooked function.</typeparam>
    /// <param name="address">The address of the function to hook.</param>
    /// <param name="detour">The detour function.</param>
    /// <returns>The created hook.</returns>
    INativeHook<TDelegate> HookFromAddress<TDelegate>(IntPtr address, TDelegate detour) where TDelegate : Delegate;

    /// <summary>Hooks the native function matching the given signature.</summary>
    /// <typeparam name="TDelegate">The delegate type of the hooked function.</typeparam>
    /// <param name="signature">The signature to scan for.</param>
    /// <param name="detour">The detour function.</param>
    /// <returns>The created hook.</returns>
    INativeHook<TDelegate> HookFromSignature<TDelegate>(string signature, TDelegate detour) where TDelegate : Delegate;
}

/// <summary>Signature scanning, used to locate the few addresses that are not covered by the struct library.</summary>
public interface ISignatureScanner
{
    /// <summary>Scans the main module for the given signature.</summary>
    /// <param name="signature">The signature to scan for.</param>
    /// <returns>The address of the match, or <c>IntPtr.Zero</c>.</returns>
    IntPtr ScanText(string signature);

    /// <summary>
    /// Scans the main module for the given signature and returns the place the
    /// pattern was found, without resolving the call it may start with.
    /// </summary>
    /// <param name="signature">The signature to scan for.</param>
    /// <returns>The address of the match, or <c>IntPtr.Zero</c>.</returns>
    /// <remarks>
    /// A pattern that starts with a call stands for the function the call goes
    /// to, which <see cref="ScanText"/> resolves on its own. The layer that
    /// knows the layout of the game needs the place the pattern was found
    /// instead, because the distance that leads to the address is read there.
    /// </remarks>
    IntPtr ScanTextRaw(string signature);

    /// <summary>Scans the main module for the given signature and returns the address of a static.</summary>
    /// <param name="signature">The signature to scan for.</param>
    /// <param name="offset">The offset added to the matched address.</param>
    /// <param name="isPointer">Whether the matched address holds a pointer to the static.</param>
    /// <returns>The address of the static, or <c>IntPtr.Zero</c>.</returns>
    IntPtr ScanText(string signature, int offset, bool isPointer = false);
}
