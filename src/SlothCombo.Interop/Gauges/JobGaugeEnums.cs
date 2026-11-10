using System.Runtime.InteropServices;

namespace SlothCombo.Interop.Gauges;

/// <summary>
/// The values the gauges of the jobs carry.
/// </summary>
/// <remarks>
/// The numbers are the ones of the game, so a value that is read from a gauge
/// can be compared with them directly.
/// </remarks>
public enum SongFlags
{
    /// <summary>No song.</summary>
    None = 0,

    /// <summary>The song of the mage.</summary>
    MagesBallad = 1,

    /// <summary>The song of the army.</summary>
    ArmysPaeon = 2,

    /// <summary>The song of the wanderer.</summary>
    WanderersMinuet = 3,

    /// <summary>The song that was played last is the song of the mage.</summary>
    MagesBalladLastPlayed = 4,

    /// <summary>The song that was played last is the song of the army.</summary>
    ArmysPaeonLastPlayed = 8,

    /// <summary>The song that was played last is the song of the wanderer.</summary>
    WanderersMinuetLastPlayed = 12,

    /// <summary>The coda of the song of the mage.</summary>
    MagesBalladCoda = 16,

    /// <summary>The coda of the song of the army.</summary>
    ArmysPaeonCoda = 32,

    /// <summary>The coda of the song of the wanderer.</summary>
    WanderersMinuetCoda = 64,
}

/// <summary>The states of the aether of the summoner.</summary>
[Flags]
public enum AetherFlags
{
    /// <summary>Nothing.</summary>
    None = 0,

    /// <summary>The first aether.</summary>
    Aetherflow1 = 1,

    /// <summary>The second aether.</summary>
    Aetherflow2 = 2,

    /// <summary>Both aethers.</summary>
    Aetherflow = 3,

    /// <summary>Phoenix is ready to be summoned.</summary>
    PhoenixPrimed = 4,

    /// <summary>The first aether of Bahamut.</summary>
    SolarBahamutFirstPrimed = 8,

    /// <summary>The second aether of Bahamut.</summary>
    SolarBahamutSecondPrimed = 12,

    /// <summary>Ifrit is ready to be summoned.</summary>
    IfritReady = 32,

    /// <summary>Titan is ready to be summoned.</summary>
    TitanReady = 64,

    /// <summary>Garuda is ready to be summoned.</summary>
    GarudaReady = 128,
}

/// <summary>The motifs the painter of the canvas has drawn.</summary>
[Flags]
public enum CanvasFlags
{
    /// <summary>Nothing.</summary>
    None = 0,

    /// <summary>The pom of a creature.</summary>
    Pom = 1,

    /// <summary>The wing of a creature.</summary>
    Wing = 2,

    /// <summary>The claw of a creature.</summary>
    Claw = 4,

    /// <summary>The maw of a creature.</summary>
    Maw = 8,

    /// <summary>The weapon of a creature.</summary>
    Weapon = 16,

    /// <summary>The landscape of a creature.</summary>
    Landscape = 32,
}

/// <summary>The portraits the painter has finished.</summary>
[Flags]
public enum CreatureFlags
{
    /// <summary>Nothing.</summary>
    None = 0,

    /// <summary>The pom of a creature.</summary>
    Pom = 1,

    /// <summary>The wing of a creature.</summary>
    Wings = 2,

    /// <summary>The claw of a creature.</summary>
    Claw = 4,

    /// <summary>The portrait of a moogle.</summary>
    MooglePortait = 16,

    /// <summary>The portrait of a madeen.</summary>
    MadeenPortrait = 32,
}

/// <summary>The steps of the combo of the viper.</summary>
public enum DreadCombo
{
    /// <summary>The step of the dreadwinder.</summary>
    Dreadwinder = 1,

    /// <summary>The step of the coil of the hunter.</summary>
    HuntersCoil = 2,

    /// <summary>The step of the coil of the swift skin.</summary>
    SwiftskinsCoil = 3,

    /// <summary>The step of the pit of dread.</summary>
    PitOfDread = 4,

    /// <summary>The step of the den of the hunter.</summary>
    HuntersDen = 5,

    /// <summary>The step of the den of the swift skin.</summary>
    SwiftskinsDen = 6,
}

/// <summary>The steps of the combo of the serpent.</summary>
public enum SerpentCombo
{
    /// <summary>The step of the death rattle.</summary>
    DeathRattle = 1,

    /// <summary>The step of the last lash.</summary>
    LastLash = 2,

    /// <summary>The step of the first legacy.</summary>
    FirstLegacy = 3,

    /// <summary>The step of the second legacy.</summary>
    SecondLegacy = 4,

    /// <summary>The step of the third legacy.</summary>
    ThirdLegacy = 5,

    /// <summary>The step of the fourth legacy.</summary>
    FourthLegacy = 6,
}

/// <summary>The nadi the monk has open.</summary>
[Flags]
public enum NadiFlags
{
    /// <summary>None.</summary>
    None = 0,

    /// <summary>The lunar nadi.</summary>
    Lunar = 1,

    /// <summary>The solar nadi.</summary>
    Solar = 2,
}

/// <summary>The sen the samurai has ready.</summary>
[Flags]
public enum SenFlags
{
    /// <summary>None.</summary>
    None = 0,

    /// <summary>The sen of setsu.</summary>
    Setsu = 1,

    /// <summary>The sen of getsu.</summary>
    Getsu = 2,

    /// <summary>The sen of ka.</summary>
    Ka = 4,
}

/// <summary>The action the samurai is about to use.</summary>
public enum KaeshiAction
{
    /// <summary>The higanbana.</summary>
    Higanbana = 1,

    /// <summary>The goken.</summary>
    Goken = 2,

    /// <summary>The setsugekka.</summary>
    Setsugekka = 3,

    /// <summary>The namikiri.</summary>
    Namikiri = 4,
}

/// <summary>The chakras of the beast of the monk.</summary>
public enum BeastChakraType
{
    /// <summary>None.</summary>
    None = 0,

    /// <summary>The opo opo.</summary>
    OpoOpo = 1,

    /// <summary>The raptor.</summary>
    Raptor = 2,

    /// <summary>The coeurl.</summary>
    Coeurl = 3,
}

/// <summary>The card the astrologian draws.</summary>
public enum AstrologianDraw
{
    /// <summary>The astral card.</summary>
    Astral = 0,

    /// <summary>The umbral card.</summary>
    Umbral = 1,
}

/// <summary>The cards of the astrologian.</summary>
public enum AstrologianCard
{
    /// <summary>No card.</summary>
    None = 0,

    /// <summary>The balance.</summary>
    Balance = 1,

    /// <summary>The bole.</summary>
    Bole = 2,

    /// <summary>The arrow.</summary>
    Arrow = 3,

    /// <summary>The spear.</summary>
    Spear = 4,

    /// <summary>The ewer.</summary>
    Ewer = 5,

    /// <summary>The spire.</summary>
    Spire = 6,

    /// <summary>The lord.</summary>
    Lord = 7,

    /// <summary>The lady.</summary>
    Lady = 8,
}

/// <summary>The steps of the dance.</summary>
public enum DanceStep
{
    /// <summary>The step is finished.</summary>
    Finish = 0,

    /// <summary>The emboite.</summary>
    Emboite = 1,

    /// <summary>The entrechat.</summary>
    Entrechat = 2,

    /// <summary>The jete.</summary>
    Jete = 3,

    /// <summary>The pirouette.</summary>
    Pirouette = 4,
}

/// <summary>The base of every gauge of a job.</summary>
/// <remarks>
/// The gauge of a job is read through a pointer to this structure, and the
/// fields of the job start at the offset 0x08. The first eight bytes hold the
/// table of the virtual functions of the gauge, which the program does not
/// call.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public unsafe struct JobGauge
{
    /// <summary>The table of the virtual functions of the gauge.</summary>
    [FieldOffset(0x00)]
    public void* VTable;
}
