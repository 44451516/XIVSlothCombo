using System;
using System.Runtime.InteropServices;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.Gauge;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Data;

public unsafe class TmpSCHGauge
{
    public byte Aetherflow => Struct->Aetherflow;

    public byte FairyGauge => Struct->FairyGauge;

    public short SeraphTimer => Struct->SeraphTimer;

    public DismissedFairy DismissedFairy => (DismissedFairy)Struct->DismissedFairy;

    private protected TmpScholarGauge* Struct;

    public TmpSCHGauge()
    {
        Struct = (TmpScholarGauge*)Service.JobGauges.Get<SCHGauge>().Address;
    }
}
public unsafe class TmpVPRGauge
{
    public byte RattlingCoilStacks => Struct->RattlingCoilStacks;

    public byte SerpentsOfferings => Struct->SerpentsOfferings;

    public byte AnguineTribute => Struct->AnguineTribute;

    public bool DreadwinderReady => Struct->DreadwinderReady;

    public bool HuntersCoilReady => Struct->HuntersCoilReady;

    public bool SwiftskinsCoilReady => Struct->SwiftskinsCoilReady;

    public bool PitOfDreadReady => Struct->PitOfDreadReady;

    public bool HuntersDenReady => Struct->HuntersDenReady;

    public bool SwiftskinsDenReady => Struct->SwiftskinsDenReady;

    private protected ViperGauge* Struct;

    public byte GetOffset(int offset)
    {
        var val = IntPtr.Add(Address, offset);
        return Marshal.ReadByte(val);
    }

    private nint Address;
    public TmpVPRGauge()
    {
        Address = Svc.SigScanner.GetStaticAddressFromSig("48 8B 3D ?? ?? ?? ?? 33 ED") + 0x8;
        Struct = (ViperGauge*)Address;
    }


}

public unsafe class TmpPCTGauge
{
    private nint Address;
    private protected PictomancerGauge* Struct;
    public uint 豆子 => Struct->Paint;
    
    public uint 能量 => Struct->PalleteGauge;
    public CanvasFlags CanvasFlags => Struct->CanvasFlags;
    public CreatureFlags CreatureFlags => Struct->CreatureFlags;
    
    public bool 生物画 => Struct->CreatureMotifDrawn;
    public bool 武器画 => Struct->WeaponMotifDrawn;
    public bool 风景画 => Struct->LandscapeMotifDrawn;
    public bool 莫古准备 => Struct->MooglePortraitReady;
    public bool 蔬菜准备 => Struct->MadeenPortraitReady;
    
    
    
    public TmpPCTGauge()
    {
        Address = Svc.SigScanner.GetStaticAddressFromSig("48 8B 3D ?? ?? ?? ?? 33 ED") + 0x8;
        Struct = (PictomancerGauge*)Address;
    }
}

[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct TmpScholarGauge
{
    [FieldOffset(0x08)] public byte Aetherflow;
    [FieldOffset(0x09)] public byte FairyGauge;
    [FieldOffset(0x0A)] public short SeraphTimer;
    [FieldOffset(0x0C)] public byte DismissedFairy;
}

[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct ViperGauge
{
    [FieldOffset(0x08)] public byte RattlingCoilStacks;
    [FieldOffset(0x0A)] public byte SerpentsOfferings;
    [FieldOffset(0x09)] public byte AnguineTribute;
    [FieldOffset(0x0B)] public DreadwinderPitFlags DreadwinderPitCombo;

    public bool DreadwinderReady => DreadwinderPitCombo.HasFlag(DreadwinderPitFlags.Dreadwinder);
    public bool HuntersCoilReady => DreadwinderPitCombo.HasFlag(DreadwinderPitFlags.HuntersCoil);
    public bool SwiftskinsCoilReady => DreadwinderPitCombo.HasFlag(DreadwinderPitFlags.SwiftskinsCoil);
    public bool PitOfDreadReady => DreadwinderPitCombo.HasFlag(DreadwinderPitFlags.PitOfDread);
    public bool HuntersDenReady => DreadwinderPitCombo.HasFlag(DreadwinderPitFlags.HuntersDen);
    public bool SwiftskinsDenReady => DreadwinderPitCombo.HasFlag(DreadwinderPitFlags.SwiftskinsDen);
    

}

[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct PictomancerGauge {
    [FieldOffset(0x08)] public byte PalleteGauge;
    [FieldOffset(0x0A)] public byte Paint;
    [FieldOffset(0x0B)] public CanvasFlags CanvasFlags;
    [FieldOffset(0x0C)] public CreatureFlags CreatureFlags;

    public bool CreatureMotifDrawn => CanvasFlags.HasFlag(CanvasFlags.Pom) || CanvasFlags.HasFlag(CanvasFlags.Wing) || CanvasFlags.HasFlag(CanvasFlags.Claw) || CanvasFlags.HasFlag(CanvasFlags.Maw);
    public bool WeaponMotifDrawn => CanvasFlags.HasFlag(CanvasFlags.Weapon);
    public bool LandscapeMotifDrawn => CanvasFlags.HasFlag(CanvasFlags.Landscape);
    public bool MooglePortraitReady => CreatureFlags.HasFlag(CreatureFlags.MooglePortait);
    public bool MadeenPortraitReady => CreatureFlags.HasFlag(CreatureFlags.MadeenPortrait);
}

[Flags]
public enum DreadwinderPitFlags : byte
{
    Dreadwinder = 1,
    HuntersCoil = 2,
    SwiftskinsCoil = 3,
    PitOfDread = 4,
    HuntersDen = 5,
    SwiftskinsDen = 6
}