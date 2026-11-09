namespace SlothCombo.Runtime.Gauges.Enums;

/// <summary>The card an astrologian holds.</summary>
public enum CardType
{
    /// <summary>No card.</summary>
    None = 0,

    /// <summary>The balance card.</summary>
    Balance = 1,

    /// <summary>The bole card.</summary>
    Bole = 2,

    /// <summary>The arrow card.</summary>
    Arrow = 3,

    /// <summary>The spear card.</summary>
    Spear = 4,

    /// <summary>The ewer card.</summary>
    Ewer = 5,

    /// <summary>The spire card.</summary>
    Spire = 6,

    /// <summary>The lord card.</summary>
    Lord = 7,

    /// <summary>The lady card.</summary>
    Lady = 8,
}

/// <summary>The draw of the astrologian.</summary>
public enum DrawType
{
    /// <summary>The astral draw.</summary>
    Astral = 0,

    /// <summary>The umbral draw.</summary>
    Umbral = 1,
}

/// <summary>The song a bard plays.</summary>
public enum Song
{
    /// <summary>No song.</summary>
    None = 0,

    /// <summary>Mage's ballad.</summary>
    MagesBallad = 1,

    /// <summary>Army's paeon.</summary>
    ArmysPaeon = 2,

    /// <summary>Wanderer's minuet.</summary>
    WanderersMinuet = 3,
}

/// <summary>The sen a samurai holds.</summary>
[System.Flags]
public enum Sen
{
    /// <summary>No sen.</summary>
    None = 0,

    /// <summary>The setsu sen.</summary>
    Setsu = 1 << 0,

    /// <summary>The getsu sen.</summary>
    Getsu = 1 << 1,

    /// <summary>The ka sen.</summary>
    Ka = 1 << 2,
}

/// <summary>The kaeshi a samurai prepares.</summary>
public enum Kaeshi
{
    /// <summary>Higanbana.</summary>
    Higanbana = 1,

    /// <summary>Goken.</summary>
    Goken = 2,

    /// <summary>Setsugekka.</summary>
    Setsugekka = 3,

    /// <summary>Namikiri.</summary>
    Namikiri = 4,
}

/// <summary>The nadi a monk channels.</summary>
[System.Flags]
public enum Nadi
{
    /// <summary>No nadi.</summary>
    None = 0,

    /// <summary>The lunar nadi.</summary>
    Lunar = 1 << 0,

    /// <summary>The solar nadi.</summary>
    Solar = 1 << 1,
}

/// <summary>The beast chakra a monk holds.</summary>
public enum BeastChakra
{
    /// <summary>No chakra.</summary>
    None = 0,

    /// <summary>The opo opo chakra.</summary>
    OpoOpo = 1,

    /// <summary>The raptor chakra.</summary>
    Raptor = 2,

    /// <summary>The coeurl chakra.</summary>
    Coeurl = 3,
}

/// <summary>The dread combo of a dragoon.</summary>
public enum DreadCombo
{
    /// <summary>No combo.</summary>
    None = 0,

    /// <summary>Dreadwinder.</summary>
    Dreadwinder = 1,

    /// <summary>Hunters coil.</summary>
    HuntersCoil = 2,

    /// <summary>Swiftskins coil.</summary>
    SwiftskinsCoil = 3,

    /// <summary>Pit of dread.</summary>
    PitOfDread = 4,

    /// <summary>Hunters den.</summary>
    HuntersDen = 5,

    /// <summary>Swiftskins den.</summary>
    SwiftskinsDen = 6,
}

/// <summary>The serpent combo of a viper.</summary>
public enum SerpentCombo
{
    /// <summary>No combo.</summary>
    None = 0,

    /// <summary>Death rattle.</summary>
    DeathRattle = 1,

    /// <summary>Last lash.</summary>
    LastLash = 2,

    /// <summary>First legacy.</summary>
    FirstLegacy = 3,

    /// <summary>Second legacy.</summary>
    SecondLegacy = 4,

    /// <summary>Third legacy.</summary>
    ThirdLegacy = 5,

    /// <summary>Fourth legacy.</summary>
    FourthLegacy = 6,
}

/// <summary>The canvas of a pictomancer.</summary>
[System.Flags]
public enum CanvasFlags
{
    /// <summary>An empty canvas.</summary>
    None = 0,

    /// <summary>The pom motif.</summary>
    Pom = 1 << 0,

    /// <summary>The wing motif.</summary>
    Wing = 1 << 1,

    /// <summary>The claw motif.</summary>
    Claw = 1 << 2,

    /// <summary>The maw motif.</summary>
    Maw = 1 << 3,

    /// <summary>The weapon motif.</summary>
    Weapon = 1 << 4,

    /// <summary>The landscape motif.</summary>
    Landscape = 1 << 5,
}

/// <summary>The creature of a pictomancer.</summary>
[System.Flags]
public enum CreatureFlags
{
    /// <summary>No creature.</summary>
    None = 0,

    /// <summary>The pom creature.</summary>
    Pom = 1 << 0,

    /// <summary>The wing creature.</summary>
    Wings = 1 << 1,

    /// <summary>The claw creature.</summary>
    Claw = 1 << 2,

    /// <summary>The moogle portrait.</summary>
    MooglePortait = 1 << 4,

    /// <summary>The madeen portrait.</summary>
    MadeenPortrait = 1 << 5,
}

/// <summary>The aether of a summoner.</summary>
[System.Flags]
public enum AetherFlags
{
    /// <summary>No aether.</summary>
    None = 0,

    /// <summary>The first aetherflow stack.</summary>
    Aetherflow1 = 1 << 0,

    /// <summary>The second aetherflow stack.</summary>
    Aetherflow2 = 1 << 1,

    /// <summary>Both aetherflow stacks.</summary>
    Aetherflow = Aetherflow1 | Aetherflow2,

    /// <summary>Phoenix is primed.</summary>
    PhoenixPrimed = 1 << 2,

    /// <summary>Solar bahamut is primed for the first time.</summary>
    SolarBahamutFirstPrimed = 1 << 3,

    /// <summary>Solar bahamut is primed.</summary>
    SolarBahamutSecondPrimed = PhoenixPrimed | SolarBahamutFirstPrimed,

    /// <summary>Ifrit is ready.</summary>
    IfritReady = 1 << 5,

    /// <summary>Titan is ready.</summary>
    TitanReady = 1 << 6,

    /// <summary>Garuda is ready.</summary>
    GarudaReady = 1 << 7,
}

/// <summary>The step of the delirium of a dark knight.</summary>
public enum DeliriumStep
{
    /// <summary>Scarlet delirium.</summary>
    ScarletDelirium = 0,

    /// <summary>Comeuppance.</summary>
    Comeuppance = 1,

    /// <summary>Torcleaver.</summary>
    Torcleaver = 2,
}

/// <summary>The fairy a scholar dismissed.</summary>
public enum DismissedFairy : byte
{
    /// <summary>Eos.</summary>
    Eos = 6,

    /// <summary>Selene.</summary>
    Selene = 7,
}

/// <summary>The mudra of a monk.</summary>
public enum Mudras : byte
{
    /// <summary>Ten.</summary>
    Ten = 1,

    /// <summary>Chi.</summary>
    Chi = 2,

    /// <summary>Jin.</summary>
    Jin = 3,
}

/// <summary>The pet a summoner returns to.</summary>
public enum SummonPet : byte
{
    /// <summary>No pet.</summary>
    None = 0,

    /// <summary>Carbuncle.</summary>
    Carbuncle = 23,
}

/// <summary>The glamour of a pet.</summary>
public enum PetGlam : byte
{
    /// <summary>No glamour.</summary>
    None = 0,

    /// <summary>Emerald.</summary>
    Emerald = 1,

    /// <summary>Topaz.</summary>
    Topaz = 2,

    /// <summary>Ruby.</summary>
    Ruby = 3,

    /// <summary>Carbuncle.</summary>
    Carbuncle = 4,

    /// <summary>Ifrit.</summary>
    Ifrit = 5,

    /// <summary>Titan.</summary>
    Titan = 6,

    /// <summary>Garuda.</summary>
    Garuda = 7,
}

/// <summary>The summon a summoner is attuned to.</summary>
public enum SummonAttunement
{
    /// <summary>No summon.</summary>
    None = 0,

    /// <summary>Ifrit.</summary>
    Ifrit = 1,

    /// <summary>Titan.</summary>
    Titan = 2,

    /// <summary>Garuda.</summary>
    Garuda = 3,
}
