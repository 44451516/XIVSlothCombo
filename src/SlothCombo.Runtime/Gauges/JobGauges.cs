using System;
using System.Collections.Generic;
using System.Reflection;
using SlothCombo.Interop.Game;
using JobGaugeManager = SlothCombo.Interop.Gauges.JobGaugeManager;
using SlothCombo.Runtime.Gauges.Enums;
using AstrologianGauge = SlothCombo.Interop.Gauges.AstrologianGauge;
using BardGauge = SlothCombo.Interop.Gauges.BardGauge;
using BlackMageGauge = SlothCombo.Interop.Gauges.BlackMageGauge;
using DancerGauge = SlothCombo.Interop.Gauges.DancerGauge;
using DarkKnightGauge = SlothCombo.Interop.Gauges.DarkKnightGauge;
using DragoonGauge = SlothCombo.Interop.Gauges.DragoonGauge;
using GunbreakerGauge = SlothCombo.Interop.Gauges.GunbreakerGauge;
using MachinistGauge = SlothCombo.Interop.Gauges.MachinistGauge;
using MonkGauge = SlothCombo.Interop.Gauges.MonkGauge;
using NinjaGauge = SlothCombo.Interop.Gauges.NinjaGauge;
using PaladinGauge = SlothCombo.Interop.Gauges.PaladinGauge;
using PictomancerGauge = SlothCombo.Interop.Gauges.PictomancerGauge;
using RedMageGauge = SlothCombo.Interop.Gauges.RedMageGauge;
using ReaperGauge = SlothCombo.Interop.Gauges.ReaperGauge;
using SageGauge = SlothCombo.Interop.Gauges.SageGauge;
using SamuraiGauge = SlothCombo.Interop.Gauges.SamuraiGauge;
using ScholarGauge = SlothCombo.Interop.Gauges.ScholarGauge;
using SummonerGauge = SlothCombo.Interop.Gauges.SummonerGauge;
using ViperGauge = SlothCombo.Interop.Gauges.ViperGauge;
using WarriorGauge = SlothCombo.Interop.Gauges.WarriorGauge;
using WhiteMageGauge = SlothCombo.Interop.Gauges.WhiteMageGauge;
using FcsAetherFlags = SlothCombo.Interop.Gauges.AetherFlags;
using FcsSongFlags = SlothCombo.Interop.Gauges.SongFlags;

namespace SlothCombo.Runtime.Gauges;

/// <summary>
/// The base of the job gauges.
/// </summary>
public abstract class JobGaugeBase
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal JobGaugeBase(IntPtr address) => Address = address;

    /// <summary>Gets the address of the gauge in memory.</summary>
    public IntPtr Address { get; }
}

/// <summary>
/// The base of the job gauges that read a native gauge struct.
/// </summary>
/// <typeparam name="T">The native gauge struct.</typeparam>
public unsafe class JobGaugeBase<T> : JobGaugeBase where T : unmanaged
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal JobGaugeBase(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets a pointer to the native gauge struct.</summary>
    private protected T* Struct => (T*)Address;
}

/// <summary>
/// Hands out the job gauges of the player.
/// </summary>
/// <remarks>
/// The game keeps every job gauge in the same union of the job gauge manager,
/// so the gauge of the current job is always read from the same address.
/// </remarks>
internal sealed unsafe class JobGaugeService
{
    private readonly Dictionary<Type, JobGaugeBase> _cache = [];

    /// <summary>Gets the gauge of the given type.</summary>
    /// <typeparam name="T">The type of the gauge.</typeparam>
    public T Get<T>() where T : JobGaugeBase
    {
        if (!_cache.TryGetValue(typeof(T), out var gauge))
        {
            var created = Activator.CreateInstance(
                typeof(T),
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                [Address],
                null);
            gauge = _cache[typeof(T)] = (T)created!;
        }

        return (T)gauge;
    }

    /// <summary>Gets the address of the job gauge union.</summary>
    public IntPtr Address
    {
        get
        {
            var manager = JobGaugeManager.Instance();
            return manager is null ? IntPtr.Zero : manager->SharedAddress;
        }
    }
}

/// <summary>The astrologian gauge.</summary>
public unsafe class ASTGauge : JobGaugeBase<AstrologianGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal ASTGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the cards the astrologian holds.</summary>
    public CardType[] DrawnCards =>
    [
        (CardType)Struct->Card1,
        (CardType)Struct->Card2,
        (CardType)Struct->Card3,
    ];

    /// <summary>Gets the first card.</summary>
    public CardType Card1 => (CardType)Struct->Card1;

    /// <summary>Gets the second card.</summary>
    public CardType Card2 => (CardType)Struct->Card2;

    /// <summary>Gets the third card.</summary>
    public CardType Card3 => (CardType)Struct->Card3;

    /// <summary>Gets the crown card.</summary>
    public CardType DrawnCrownCard => (CardType)Struct->CurrentArcana;

    /// <summary>Gets the active draw.</summary>
    public DrawType ActiveDraw => (DrawType)Struct->CurrentDraw;
}

/// <summary>The black mage gauge.</summary>
public unsafe class BLMGauge : JobGaugeBase<BlackMageGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal BLMGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the remaining time of the enochian.</summary>
    public short EnochianTimer => Struct->EnochianTimer;

    /// <summary>Gets the number of polyglot stacks.</summary>
    public byte PolyglotStacks => Struct->PolyglotStacks;

    /// <summary>Gets the number of umbral hearts.</summary>
    public byte UmbralHearts => Struct->UmbralHearts;

    /// <summary>Gets the number of umbral ice stacks.</summary>
    public byte UmbralIceStacks => (byte)(InUmbralIce ? -Struct->ElementStance : 0);

    /// <summary>Gets the number of astral fire stacks.</summary>
    public byte AstralFireStacks => (byte)(InAstralFire ? Struct->ElementStance : 0);

    /// <summary>Gets the number of astral souls.</summary>
    public int AstralSoulStacks => Struct->AstralSoulStacks;

    /// <summary>Gets a value indicating whether the black mage is in umbral ice.</summary>
    public bool InUmbralIce => Struct->ElementStance < 0;

    /// <summary>Gets a value indicating whether the black mage is in astral fire.</summary>
    public bool InAstralFire => Struct->ElementStance > 0;

    /// <summary>Gets a value indicating whether the enochian is active.</summary>
    public bool IsEnochianActive => Struct->EnochianActive;

    /// <summary>Gets a value indicating whether paradox is active.</summary>
    public bool IsParadoxActive => Struct->ParadoxActive;
}

/// <summary>The bard gauge.</summary>
public unsafe class BRDGauge : JobGaugeBase<BardGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal BRDGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the remaining time of the song.</summary>
    public ushort SongTimer => Struct->SongTimer;

    /// <summary>Gets the repertoire.</summary>
    public byte Repertoire => Struct->Repertoire;

    /// <summary>Gets the soul voice.</summary>
    public byte SoulVoice => Struct->SoulVoice;

    /// <summary>Gets the active song.</summary>
    public Song Song
    {
        get
        {
            var flags = Struct->SongFlags;
            if (flags.HasFlag(FcsSongFlags.WanderersMinuet))
            {
                return Song.WanderersMinuet;
            }

            if (flags.HasFlag(FcsSongFlags.ArmysPaeon))
            {
                return Song.ArmysPaeon;
            }

            if (flags.HasFlag(FcsSongFlags.MagesBallad))
            {
                return Song.MagesBallad;
            }

            return Song.None;
        }
    }

    /// <summary>Gets the song that was played last.</summary>
    public Song LastSong
    {
        get
        {
            var flags = Struct->SongFlags;
            if (flags.HasFlag(FcsSongFlags.WanderersMinuetLastPlayed))
            {
                return Song.WanderersMinuet;
            }

            if (flags.HasFlag(FcsSongFlags.ArmysPaeonLastPlayed))
            {
                return Song.ArmysPaeon;
            }

            if (flags.HasFlag(FcsSongFlags.MagesBalladLastPlayed))
            {
                return Song.MagesBallad;
            }

            return Song.None;
        }
    }

    /// <summary>Gets the coda of every song.</summary>
    public Song[] Coda =>
    [
        Struct->SongFlags.HasFlag(FcsSongFlags.MagesBalladCoda) ? Song.MagesBallad : Song.None,
        Struct->SongFlags.HasFlag(FcsSongFlags.ArmysPaeonCoda) ? Song.ArmysPaeon : Song.None,
        Struct->SongFlags.HasFlag(FcsSongFlags.WanderersMinuetCoda) ? Song.WanderersMinuet : Song.None,
    ];
}

/// <summary>The dancer gauge.</summary>
public unsafe class DNCGauge : JobGaugeBase<DancerGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal DNCGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the number of feathers.</summary>
    public byte Feathers => Struct->Feathers;

    /// <summary>Gets the esprit.</summary>
    public byte Esprit => Struct->Esprit;

    /// <summary>Gets the number of completed steps.</summary>
    public byte CompletedSteps => Struct->StepIndex;

    /// <summary>Gets the actions of the four steps.</summary>
    public uint[] Steps
    {
        get
        {
            var arr = new uint[4];
            for (var i = 0; i < 4; i++)
            {
                arr[i] = Struct->DanceSteps[i] + 15999u - 1;
            }

            return arr;
        }
    }

    /// <summary>Gets the action of the next step.</summary>
    public uint NextStep =>
        Struct->StepIndex >= 4 ? 0 : 15999u + Struct->DanceSteps[Struct->StepIndex] - 1;

    /// <summary>Gets a value indicating whether the dancer is dancing.</summary>
    public bool IsDancing => Struct->DanceSteps[0] != 0;
}

/// <summary>The dragoon gauge.</summary>
public unsafe class DRGGauge : JobGaugeBase<DragoonGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal DRGGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the remaining time of the life of the dragon.</summary>
    public short LOTDTimer => Struct->LotdTimer;

    /// <summary>Gets a value indicating whether the life of the dragon is active.</summary>
    public bool IsLOTDActive => Struct->LotdState == 2;

    /// <summary>Gets the number of eyes.</summary>
    public byte EyeCount => Struct->EyeCount;

    /// <summary>Gets the number of firstminds focus.</summary>
    public byte FirstmindsFocusCount => Struct->FirstmindsFocusCount;
}

/// <summary>The dark knight gauge.</summary>
public unsafe class DRKGauge : JobGaugeBase<DarkKnightGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal DRKGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the blood gauge.</summary>
    public byte Blood => Struct->Blood;

    /// <summary>Gets the remaining time of dark side.</summary>
    public ushort DarksideTimeRemaining => Struct->DarksideTimer;

    /// <summary>Gets the remaining time of shadow.</summary>
    public ushort ShadowTimeRemaining => Struct->ShadowTimer;

    /// <summary>Gets a value indicating whether dark arts is available.</summary>
    public bool HasDarkArts => Struct->DarkArtsState > 0;

    /// <summary>Gets the step of the delirium.</summary>
    public DeliriumStep DeliriumComboStep => (DeliriumStep)Struct->DeliriumStep;
}

/// <summary>The gunbreaker gauge.</summary>
public unsafe class GNBGauge : JobGaugeBase<GunbreakerGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal GNBGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the ammunition.</summary>
    public byte Ammo => Struct->Ammo;

    /// <summary>Gets the maximum duration of the timer.</summary>
    public short MaxTimerDuration => Struct->MaxTimerDuration;

    /// <summary>Gets the step of the ammunition combo.</summary>
    public byte AmmoComboStep => Struct->AmmoComboStep;
}

/// <summary>The machinist gauge.</summary>
public unsafe class MCHGauge : JobGaugeBase<MachinistGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal MCHGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the remaining time of the overheat.</summary>
    public short OverheatTimeRemaining => Struct->OverheatTimeRemaining;

    /// <summary>Gets the remaining time of the summon.</summary>
    public short SummonTimeRemaining => Struct->SummonTimeRemaining;

    /// <summary>Gets the heat.</summary>
    public byte Heat => Struct->Heat;

    /// <summary>Gets the battery.</summary>
    public byte Battery => Struct->Battery;

    /// <summary>Gets the battery power of the last summon.</summary>
    public byte LastSummonBatteryPower => Struct->LastSummonBatteryPower;

    /// <summary>Gets a value indicating whether the machinist is overheated.</summary>
    public bool IsOverheated => (Struct->TimerActive & 1) != 0;

    /// <summary>Gets a value indicating whether the robot is active.</summary>
    public bool IsRobotActive => (Struct->TimerActive & 2) != 0;
}

/// <summary>The monk gauge.</summary>
public unsafe class MNKGauge : JobGaugeBase<MonkGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal MNKGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the number of chakras.</summary>
    public byte Chakra => Struct->Chakra;

    /// <summary>Gets the beast chakras.</summary>
    public BeastChakra[] BeastChakra =>
    [
        (BeastChakra)Struct->BeastChakra1,
        (BeastChakra)Struct->BeastChakra2,
        (BeastChakra)Struct->BeastChakra3,
    ];

    /// <summary>Gets the first beast chakra.</summary>
    public BeastChakra BeastChakra1 => (BeastChakra)Struct->BeastChakra1;

    /// <summary>Gets the second beast chakra.</summary>
    public BeastChakra BeastChakra2 => (BeastChakra)Struct->BeastChakra2;

    /// <summary>Gets the third beast chakra.</summary>
    public BeastChakra BeastChakra3 => (BeastChakra)Struct->BeastChakra3;

    /// <summary>Gets the nadi.</summary>
    public Nadi Nadi => (Nadi)Struct->Nadi;

    /// <summary>Gets the number of opo opo furies.</summary>
    public int OpoOpoFury => Struct->OpoOpoStacks;

    /// <summary>Gets the number of raptor furies.</summary>
    public int RaptorFury => Struct->RaptorStacks;

    /// <summary>Gets the number of coeurl furies.</summary>
    public int CoeurlFury => Struct->CoeurlStacks;

    /// <summary>Gets the remaining time of the blitz.</summary>
    public ushort BlitzTimeRemaining => Struct->BlitzTimeRemaining;
}

/// <summary>The ninja gauge.</summary>
public unsafe class NINGauge : JobGaugeBase<NinjaGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal NINGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the ninki.</summary>
    public byte Ninki => Struct->Ninki;

    /// <summary>Gets the kazematoi.</summary>
    public byte Kazematoi => Struct->Kazematoi;
}

/// <summary>The paladin gauge.</summary>
public unsafe class PLDGauge : JobGaugeBase<PaladinGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal PLDGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the oath gauge.</summary>
    public byte OathGauge => Struct->OathGauge;
}

/// <summary>The red mage gauge.</summary>
public unsafe class RDMGauge : JobGaugeBase<RedMageGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal RDMGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the white mana.</summary>
    public byte WhiteMana => Struct->WhiteMana;

    /// <summary>Gets the black mana.</summary>
    public byte BlackMana => Struct->BlackMana;

    /// <summary>Gets the number of mana stacks.</summary>
    public byte ManaStacks => Struct->ManaStacks;
}

/// <summary>The reaper gauge.</summary>
public unsafe class RPRGauge : JobGaugeBase<ReaperGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal RPRGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the soul.</summary>
    public byte Soul => Struct->Soul;

    /// <summary>Gets the shroud.</summary>
    public byte Shroud => Struct->Shroud;

    /// <summary>Gets the remaining time of the shroud.</summary>
    public ushort EnshroudedTimeRemaining => Struct->EnshroudedTimeRemaining;

    /// <summary>Gets the lemure shroud.</summary>
    public byte LemureShroud => Struct->LemureShroud;

    /// <summary>Gets the void shroud.</summary>
    public byte VoidShroud => Struct->VoidShroud;
}

/// <summary>The samurai gauge.</summary>
public unsafe class SAMGauge : JobGaugeBase<SamuraiGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal SAMGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the kaeshi.</summary>
    public Kaeshi Kaeshi => (Kaeshi)Struct->Kaeshi;

    /// <summary>Gets the kenki.</summary>
    public byte Kenki => Struct->Kenki;

    /// <summary>Gets the number of meditation stacks.</summary>
    public byte MeditationStacks => Struct->MeditationStacks;

    /// <summary>Gets the sen.</summary>
    public Sen Sen => (Sen)Struct->SenFlags;

    /// <summary>Gets a value indicating whether the samurai holds the setsu sen.</summary>
    public bool HasSetsu => (Sen & Sen.Setsu) != 0;

    /// <summary>Gets a value indicating whether the samurai holds the getsu sen.</summary>
    public bool HasGetsu => (Sen & Sen.Getsu) != 0;

    /// <summary>Gets a value indicating whether the samurai holds the ka sen.</summary>
    public bool HasKa => (Sen & Sen.Ka) != 0;
}

/// <summary>The scholar gauge.</summary>
public unsafe class SCHGauge : JobGaugeBase<ScholarGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal SCHGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the aetherflow.</summary>
    public byte Aetherflow => Struct->Aetherflow;

    /// <summary>Gets the fairy gauge.</summary>
    public byte FairyGauge => Struct->FairyGauge;

    /// <summary>Gets the remaining time of the seraph.</summary>
    public short SeraphTimer => Struct->SeraphTimer;

    /// <summary>Gets the dismissed fairy.</summary>
    public DismissedFairy DismissedFairy => (DismissedFairy)Struct->DismissedFairy;
}

/// <summary>The sage gauge.</summary>
public unsafe class SGEGauge : JobGaugeBase<SageGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal SGEGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the remaining time of the addersgall.</summary>
    public short AddersgallTimer => Struct->AddersgallTimer;

    /// <summary>Gets the addersgall.</summary>
    public byte Addersgall => Struct->Addersgall;

    /// <summary>Gets the addersting.</summary>
    public byte Addersting => Struct->Addersting;

    /// <summary>Gets a value indicating whether eukrasia is active.</summary>
    public bool Eukrasia => Struct->Eukrasia == 1;
}

/// <summary>The summoner gauge.</summary>
public unsafe class SMNGauge : JobGaugeBase<SummonerGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal SMNGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the remaining time of the summon.</summary>
    public ushort SummonTimerRemaining => Struct->SummonTimer;

    /// <summary>Gets the remaining time of the attunement.</summary>
    public ushort AttunementTimerRemaining => Struct->AttunementTimer;

    /// <summary>Gets the pet the summoner returns to.</summary>
    public SummonPet ReturnSummon => (SummonPet)Struct->ReturnSummon;

    /// <summary>Gets the glamour of the pet the summoner returns to.</summary>
    public PetGlam ReturnSummonGlam => (PetGlam)Struct->ReturnSummonGlam;

    /// <summary>Gets the attunement of the summon.</summary>
    /// <remarks>
    /// The game packs the type of the summon and the number of charges into a
    /// single byte. The two are put back together here so the value keeps the
    /// shape the plugin expects.
    /// </remarks>
    public byte Attunement => (byte)((byte)AttunementType | (byte)(AttunementCount << 2));

    /// <summary>Gets the attunement count.</summary>
    public byte AttunementCount => Struct->AttunementCount;

    /// <summary>Gets the type of the attunement.</summary>
    public SummonAttunement AttunementType => (SummonAttunement)Struct->AttunementType;

    /// <summary>Gets the aether flags.</summary>
    public AetherFlags AetherFlags => (AetherFlags)Struct->AetherFlags;

    /// <summary>Gets a value indicating whether bahamut is ready.</summary>
    public bool IsBahamutReady => !AetherFlags.HasFlag(AetherFlags.PhoenixPrimed);

    /// <summary>Gets a value indicating whether phoenix is ready.</summary>
    public bool IsPhoenixReady => AetherFlags.HasFlag(AetherFlags.PhoenixPrimed);

    /// <summary>Gets a value indicating whether ifrit is ready.</summary>
    public bool IsIfritReady => AetherFlags.HasFlag(AetherFlags.IfritReady);

    /// <summary>Gets a value indicating whether titan is ready.</summary>
    public bool IsTitanReady => AetherFlags.HasFlag(AetherFlags.TitanReady);

    /// <summary>Gets a value indicating whether garuda is ready.</summary>
    public bool IsGarudaReady => AetherFlags.HasFlag(AetherFlags.GarudaReady);

    /// <summary>Gets a value indicating whether ifrit is attuned.</summary>
    public bool IsIfritAttuned => AttunementType == SummonAttunement.Ifrit;

    /// <summary>Gets a value indicating whether titan is attuned.</summary>
    public bool IsTitanAttuned => AttunementType == SummonAttunement.Titan;

    /// <summary>Gets a value indicating whether garuda is attuned.</summary>
    public bool IsGarudaAttuned => AttunementType == SummonAttunement.Garuda;

    /// <summary>Gets a value indicating whether there are aetherflow stacks.</summary>
    public bool HasAetherflowStacks => AetherflowStacks > 0;

    /// <summary>Gets the number of aetherflow stacks.</summary>
    public byte AetherflowStacks => (byte)(Struct->AetherFlags & FcsAetherFlags.Aetherflow);
}

/// <summary>The viper gauge.</summary>
public unsafe class VPRGauge : JobGaugeBase<ViperGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal VPRGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the number of rattling coil stacks.</summary>
    public byte RattlingCoilStacks => Struct->RattlingCoilStacks;

    /// <summary>Gets the serpent offering.</summary>
    public byte SerpentOffering => Struct->SerpentOffering;

    /// <summary>Gets the anguine tribute.</summary>
    public byte AnguineTribute => Struct->AnguineTribute;

    /// <summary>Gets the dread combo.</summary>
    public DreadCombo DreadCombo => (DreadCombo)Struct->DreadCombo;

    /// <summary>Gets the serpent combo.</summary>
    public SerpentCombo SerpentCombo => (SerpentCombo)Struct->SerpentCombo;
}

/// <summary>The warrior gauge.</summary>
public unsafe class WARGauge : JobGaugeBase<WarriorGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal WARGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the beast gauge.</summary>
    public byte BeastGauge => Struct->BeastGauge;
}

/// <summary>The white mage gauge.</summary>
public unsafe class WHMGauge : JobGaugeBase<WhiteMageGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal WHMGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the remaining time of the lily.</summary>
    public short LilyTimer => Struct->LilyTimer;

    /// <summary>Gets the lily.</summary>
    public byte Lily => Struct->Lily;

    /// <summary>Gets the blood lily.</summary>
    public byte BloodLily => Struct->BloodLily;
}

/// <summary>The pictomancer gauge.</summary>
public unsafe class PCTGauge : JobGaugeBase<PictomancerGauge>
{
    /// <summary>Creates the gauge at the given address.</summary>
    /// <param name="address">The address of the gauge in memory.</param>
    internal PCTGauge(IntPtr address)
        : base(address)
    {
    }

    /// <summary>Gets the use of the subjective palette.</summary>
    public byte PalleteGauge => Struct->PalleteGauge;

    /// <summary>Gets the amount of paint.</summary>
    public byte Paint => Struct->Paint;

    /// <summary>Gets a value indicating whether a creature motif is drawn.</summary>
    public bool CreatureMotifDrawn => Struct->CreatureMotifDrawn;

    /// <summary>Gets a value indicating whether a weapon motif is drawn.</summary>
    public bool WeaponMotifDrawn => Struct->WeaponMotifDrawn;

    /// <summary>Gets a value indicating whether a landscape motif is drawn.</summary>
    public bool LandscapeMotifDrawn => Struct->LandscapeMotifDrawn;

    /// <summary>Gets a value indicating whether the moogle portrait is ready.</summary>
    public bool MooglePortraitReady => Struct->MooglePortraitReady;

    /// <summary>Gets a value indicating whether the madeen portrait is ready.</summary>
    public bool MadeenPortraitReady => Struct->MadeenPortraitReady;

    /// <summary>Gets the creature flags.</summary>
    public CreatureFlags CreatureFlags => (CreatureFlags)Struct->CreatureFlags;

    /// <summary>Gets the canvas flags.</summary>
    public CanvasFlags CanvasFlags => (CanvasFlags)Struct->CanvasFlags;
}
