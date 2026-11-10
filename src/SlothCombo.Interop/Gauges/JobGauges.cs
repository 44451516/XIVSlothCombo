using System.Runtime.InteropServices;

namespace SlothCombo.Interop.Gauges;

/// <summary>The gauge of the astrologian.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public struct AstrologianGauge
{
    /// <summary>The three cards the astrologian holds.</summary>
    [FieldOffset(0x08)]
    public ushort CardsBacking;

    /// <summary>The card that was drawn last.</summary>
    [FieldOffset(0x0A)]
    public AstrologianDraw CurrentDraw;

    /// <summary>Gets the first card.</summary>
    public AstrologianCard Card1 => (AstrologianCard)BitField.GetBits(CardsBacking, 0, 4);

    /// <summary>Gets the second card.</summary>
    public AstrologianCard Card2 => (AstrologianCard)BitField.GetBits(CardsBacking, 4, 4);

    /// <summary>Gets the third card.</summary>
    public AstrologianCard Card3 => (AstrologianCard)BitField.GetBits(CardsBacking, 8, 4);

    /// <summary>Gets the arcana that is in effect.</summary>
    public AstrologianCard CurrentArcana => (AstrologianCard)BitField.GetBits(CardsBacking, 12, 4);
}

/// <summary>The gauge of the black mage.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public struct BlackMageGauge
{
    /// <summary>The time the enochian is left.</summary>
    [FieldOffset(0x08)]
    public short EnochianTimer;

    /// <summary>The stance of the element.</summary>
    [FieldOffset(0x0A)]
    public sbyte ElementStance;

    /// <summary>The umbral hearts.</summary>
    [FieldOffset(0x0B)]
    public byte UmbralHearts;

    /// <summary>The polyglot stacks.</summary>
    [FieldOffset(0x0C)]
    public byte PolyglotStacks;

    /// <summary>The flags of the enochian and the paradox.</summary>
    [FieldOffset(0x0D)]
    public byte FlagsBacking;

    /// <summary>Gets a value indicating whether the enochian is active.</summary>
    public bool EnochianActive => BitField.GetBit(FlagsBacking, 0);

    /// <summary>Gets a value indicating whether the paradox is active.</summary>
    public bool ParadoxActive => BitField.GetBit(FlagsBacking, 1);

    /// <summary>Gets the astral souls.</summary>
    public int AstralSoulStacks => BitField.GetIntBits(FlagsBacking, 2, 3);

    /// <summary>Gets the umbral stacks.</summary>
    public uint UmbralStacks => ElementStance >= 0 ? 0u : (uint)(ElementStance * -1);

    /// <summary>Gets the astral stacks.</summary>
    public uint AstralStacks => ElementStance <= 0 ? 0u : (uint)ElementStance;
}

/// <summary>The gauge of the summoner.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct SummonerGauge
{
    /// <summary>The time the summon is left.</summary>
    [FieldOffset(0x08)]
    public ushort SummonTimer;

    /// <summary>The time the attunement is left.</summary>
    [FieldOffset(0x0A)]
    public ushort AttunementTimer;

    /// <summary>The summon that is to be returned.</summary>
    [FieldOffset(0x0C)]
    public byte ReturnSummon;

    /// <summary>The glamour of the summon that is to be returned.</summary>
    [FieldOffset(0x0D)]
    public byte ReturnSummonGlam;

    /// <summary>The type and the count of the attunement.</summary>
    [FieldOffset(0x0E)]
    public byte AttunementBacking;

    /// <summary>The aethers that are ready.</summary>
    [FieldOffset(0x0F)]
    public AetherFlags AetherFlags;

    /// <summary>Gets the type of the attunement.</summary>
    public uint AttunementType => BitField.GetBits(AttunementBacking, 0, 2);

    /// <summary>Gets the count of the attunement.</summary>
    public byte AttunementCount => BitField.GetByteBits(AttunementBacking, 2, 6);
}

/// <summary>The gauge of the red mage.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x50)]
public struct RedMageGauge
{
    /// <summary>The white mana.</summary>
    [FieldOffset(0x08)]
    public byte WhiteMana;

    /// <summary>The black mana.</summary>
    [FieldOffset(0x09)]
    public byte BlackMana;

    /// <summary>The mana stacks.</summary>
    [FieldOffset(0x0A)]
    public byte ManaStacks;
}

/// <summary>The gauge of the pictomancer.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct PictomancerGauge
{
    /// <summary>The palette.</summary>
    [FieldOffset(0x08)]
    public byte PalleteGauge;

    /// <summary>The paint.</summary>
    [FieldOffset(0x0A)]
    public byte Paint;

    /// <summary>The motifs that are on the canvas.</summary>
    [FieldOffset(0x0B)]
    public CanvasFlags CanvasFlags;

    /// <summary>The portraits that are finished.</summary>
    [FieldOffset(0x0C)]
    public CreatureFlags CreatureFlags;

    /// <summary>Gets a value indicating whether a creature is drawn.</summary>
    public bool CreatureMotifDrawn =>
        (CanvasFlags & (CanvasFlags.Pom | CanvasFlags.Wing | CanvasFlags.Claw | CanvasFlags.Maw)) != 0;

    /// <summary>Gets a value indicating whether a weapon is drawn.</summary>
    public bool WeaponMotifDrawn => (CanvasFlags & CanvasFlags.Weapon) != 0;

    /// <summary>Gets a value indicating whether a landscape is drawn.</summary>
    public bool LandscapeMotifDrawn => (CanvasFlags & CanvasFlags.Landscape) != 0;

    /// <summary>Gets a value indicating whether the portrait of the moogle is ready.</summary>
    public bool MooglePortraitReady => (CreatureFlags & CreatureFlags.MooglePortait) != 0;

    /// <summary>Gets a value indicating whether the portrait of the madeen is ready.</summary>
    public bool MadeenPortraitReady => (CreatureFlags & CreatureFlags.MadeenPortrait) != 0;
}

/// <summary>The gauge of the white mage.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct WhiteMageGauge
{
    /// <summary>The time the lily is left.</summary>
    [FieldOffset(0x0A)]
    public short LilyTimer;

    /// <summary>The lilies.</summary>
    [FieldOffset(0x0C)]
    public byte Lily;

    /// <summary>The blood lilies.</summary>
    [FieldOffset(0x0D)]
    public byte BloodLily;
}

/// <summary>The gauge of the scholar.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct ScholarGauge
{
    /// <summary>The aetherflow.</summary>
    [FieldOffset(0x08)]
    public byte Aetherflow;

    /// <summary>The gauge of the fairy.</summary>
    [FieldOffset(0x09)]
    public byte FairyGauge;

    /// <summary>The time the seraph is left.</summary>
    [FieldOffset(0x0A)]
    public short SeraphTimer;

    /// <summary>A value that tells whether the fairy was dismissed.</summary>
    [FieldOffset(0x0C)]
    public byte DismissedFairy;
}

/// <summary>The gauge of the sage.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct SageGauge
{
    /// <summary>The time the addersgall is left.</summary>
    [FieldOffset(0x08)]
    public short AddersgallTimer;

    /// <summary>The addersgall.</summary>
    [FieldOffset(0x0A)]
    public byte Addersgall;

    /// <summary>The addersting.</summary>
    [FieldOffset(0x0B)]
    public byte Addersting;

    /// <summary>The eukrasia.</summary>
    [FieldOffset(0x0C)]
    public byte Eukrasia;

    /// <summary>Gets a value indicating whether the eukrasia is active.</summary>
    public bool EukrasiaActive => Eukrasia > 0;
}

/// <summary>The gauge of the bard.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct BardGauge
{
    /// <summary>The time the song is left.</summary>
    [FieldOffset(0x08)]
    public ushort SongTimer;

    /// <summary>The repertoire.</summary>
    [FieldOffset(0x0C)]
    public byte Repertoire;

    /// <summary>The soul voice.</summary>
    [FieldOffset(0x0D)]
    public byte SoulVoice;

    /// <summary>The time of the coda of the radiant finale.</summary>
    [FieldOffset(0x0E)]
    public byte RadiantFinaleCoda;

    /// <summary>The songs that are active.</summary>
    [FieldOffset(0x0F)]
    public SongFlags SongFlags;
}

/// <summary>The gauge of the machinist.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct MachinistGauge
{
    /// <summary>The time the overheat is left.</summary>
    [FieldOffset(0x08)]
    public short OverheatTimeRemaining;

    /// <summary>The time the summon is left.</summary>
    [FieldOffset(0x0A)]
    public short SummonTimeRemaining;

    /// <summary>The heat.</summary>
    [FieldOffset(0x0C)]
    public byte Heat;

    /// <summary>The battery.</summary>
    [FieldOffset(0x0D)]
    public byte Battery;

    /// <summary>The power of the battery of the last summon.</summary>
    [FieldOffset(0x0E)]
    public byte LastSummonBatteryPower;

    /// <summary>The flags of the timers.</summary>
    [FieldOffset(0x0F)]
    public byte TimerActive;

    /// <summary>Gets a value indicating whether the timer of the overheat runs.</summary>
    public bool OverheatTimerActive => BitField.GetBit(TimerActive, 0);

    /// <summary>Gets a value indicating whether the timer of the summon runs.</summary>
    public bool SummonTimerActive => BitField.GetBit(TimerActive, 1);
}

/// <summary>The gauge of the dancer.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct DancerGauge
{
    /// <summary>The feathers.</summary>
    [FieldOffset(0x08)]
    public byte Feathers;

    /// <summary>The esprit.</summary>
    [FieldOffset(0x09)]
    public byte Esprit;

    /// <summary>The four steps of the dance.</summary>
    [FieldOffset(0x0A)]
    public fixed byte DanceSteps[4];

    /// <summary>The step the dancer is at.</summary>
    [FieldOffset(0x0E)]
    public byte StepIndex;

    /// <summary>Gets the step the dancer is at.</summary>
    public DanceStep CurrentStep => StepIndex >= 4 ? DanceStep.Finish : (DanceStep)DanceSteps[StepIndex];
}

/// <summary>The gauge of the monk.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct MonkGauge
{
    /// <summary>The chakra.</summary>
    [FieldOffset(0x08)]
    public byte Chakra;

    /// <summary>The first chakra of the beast.</summary>
    [FieldOffset(0x09)]
    public BeastChakraType BeastChakra1;

    /// <summary>The second chakra of the beast.</summary>
    [FieldOffset(0x0A)]
    public BeastChakraType BeastChakra2;

    /// <summary>The third chakra of the beast.</summary>
    [FieldOffset(0x0B)]
    public BeastChakraType BeastChakra3;

    /// <summary>The stacks of the three kinds of beast.</summary>
    [FieldOffset(0x0C)]
    public byte BeastChakraStacks;

    /// <summary>The nadi that are open.</summary>
    [FieldOffset(0x0D)]
    public NadiFlags Nadi;

    /// <summary>The time the blitz is left.</summary>
    [FieldOffset(0x0E)]
    public ushort BlitzTimeRemaining;

    /// <summary>Gets the stacks of the opo opo.</summary>
    public int OpoOpoStacks => BitField.GetIntBits(BeastChakraStacks, 0, 2);

    /// <summary>Gets the stacks of the raptor.</summary>
    public int RaptorStacks => BitField.GetIntBits(BeastChakraStacks, 2, 2);

    /// <summary>Gets the stacks of the coeurl.</summary>
    public int CoeurlStacks => BitField.GetIntBits(BeastChakraStacks, 4, 2);
}

/// <summary>The gauge of the dragoon.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DragoonGauge
{
    /// <summary>The time the leap of faith is left.</summary>
    [FieldOffset(0x08)]
    public short LotdTimer;

    /// <summary>The state of the leap of faith.</summary>
    [FieldOffset(0x0A)]
    public byte LotdState;

    /// <summary>The eyes of the dragon.</summary>
    [FieldOffset(0x0B)]
    public byte EyeCount;

    /// <summary>The focus of the first mind.</summary>
    [FieldOffset(0x0C)]
    public byte FirstmindsFocusCount;
}

/// <summary>The gauge of the ninja.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct NinjaGauge
{
    /// <summary>The ninki.</summary>
    [FieldOffset(0x08)]
    public byte Ninki;

    /// <summary>The kazematoi.</summary>
    [FieldOffset(0x0A)]
    public byte Kazematoi;
}

/// <summary>The gauge of the samurai.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct SamuraiGauge
{
    /// <summary>The action the samurai is about to use.</summary>
    [FieldOffset(0x0A)]
    public KaeshiAction Kaeshi;

    /// <summary>The kenki.</summary>
    [FieldOffset(0x0B)]
    public byte Kenki;

    /// <summary>The meditation stacks.</summary>
    [FieldOffset(0x0C)]
    public byte MeditationStacks;

    /// <summary>The sen that are ready.</summary>
    [FieldOffset(0x0D)]
    public SenFlags SenFlags;
}

/// <summary>The gauge of the reaper.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public struct ReaperGauge
{
    /// <summary>The soul.</summary>
    [FieldOffset(0x08)]
    public byte Soul;

    /// <summary>The shroud.</summary>
    [FieldOffset(0x09)]
    public byte Shroud;

    /// <summary>The time the shroud is left.</summary>
    [FieldOffset(0x0A)]
    public ushort EnshroudedTimeRemaining;

    /// <summary>The lemure shroud.</summary>
    [FieldOffset(0x0C)]
    public byte LemureShroud;

    /// <summary>The void shroud.</summary>
    [FieldOffset(0x0D)]
    public byte VoidShroud;
}

/// <summary>The gauge of the viper.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x18)]
public struct ViperGauge
{
    /// <summary>The rattling coil stacks.</summary>
    [FieldOffset(0x08)]
    public byte RattlingCoilStacks;

    /// <summary>The anguine tribute.</summary>
    [FieldOffset(0x09)]
    public byte AnguineTribute;

    /// <summary>The serpent offering.</summary>
    [FieldOffset(0x0A)]
    public byte SerpentOffering;

    /// <summary>The step of the combo of the viper.</summary>
    [FieldOffset(0x0B)]
    public DreadCombo DreadCombo;

    /// <summary>The time the reawakened is left.</summary>
    [FieldOffset(0x0E)]
    public ushort ReawakenedTimer;

    /// <summary>The state of the combo of the serpent.</summary>
    [FieldOffset(0x10)]
    public byte SerpentComboState;

    /// <summary>Gets the step of the combo of the serpent.</summary>
    public SerpentCombo SerpentCombo => (SerpentCombo)BitField.GetBits(SerpentComboState, 2, 6);
}

/// <summary>The gauge of the dark knight.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x18)]
public struct DarkKnightGauge
{
    /// <summary>The blood.</summary>
    [FieldOffset(0x08)]
    public byte Blood;

    /// <summary>The state of the dark arts.</summary>
    [FieldOffset(0x09)]
    public byte DarkArtsState;

    /// <summary>The time the darkside is left.</summary>
    [FieldOffset(0x0A)]
    public ushort DarksideTimer;

    /// <summary>The time the shadow is left.</summary>
    [FieldOffset(0x0C)]
    public ushort ShadowTimer;

    /// <summary>The step of the delirium.</summary>
    [FieldOffset(0x10)]
    public ushort DeliriumStep;
}

/// <summary>The gauge of the paladin.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct PaladinGauge
{
    /// <summary>The oath gauge.</summary>
    [FieldOffset(0x08)]
    public byte OathGauge;

    /// <summary>The time the combo of the confiteor is left.</summary>
    [FieldOffset(0x0A)]
    public ushort ConfiteorComboTimer;

    /// <summary>The step of the combo of the confiteor.</summary>
    [FieldOffset(0x0C)]
    public byte ConfiteorComboStep;
}

/// <summary>The gauge of the warrior.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct WarriorGauge
{
    /// <summary>The beast gauge.</summary>
    [FieldOffset(0x08)]
    public byte BeastGauge;
}

/// <summary>The gauge of the gunbreaker.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct GunbreakerGauge
{
    /// <summary>The ammo.</summary>
    [FieldOffset(0x08)]
    public byte Ammo;

    /// <summary>The time the combo is left.</summary>
    [FieldOffset(0x0A)]
    public short MaxTimerDuration;

    /// <summary>The step of the combo.</summary>
    [FieldOffset(0x0C)]
    public byte AmmoComboStep;
}

/// <summary>The gauges of the jobs.</summary>
/// <remarks>
/// The game keeps the gauges of all the jobs in one place. Only the gauge of
/// the job the player is in carries a value, so the gauges share the same
/// place in memory.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x60)]
public unsafe struct JobGaugeManager
{
    /// <summary>The place where the gauges of all the jobs share.</summary>
    private const int SharedGaugesOffset = 0x08;

    /// <summary>The gauge of the job the player is in.</summary>
    [FieldOffset(0x00)]
    public JobGauge* CurrentGauge;

    /// <summary>The place the gauges of all the jobs share.</summary>
    [FieldOffset(0x08)]
    public fixed byte Gauges[0x50];

    /// <summary>The class of the job the player is in.</summary>
    [FieldOffset(0x58)]
    public byte ClassJobId;

    /// <summary>Gets the manager of the gauges.</summary>
    /// <returns>The manager, or <c>null</c> when the game is not ready.</returns>
    public static JobGaugeManager* Instance()
        => (JobGaugeManager*)GameFunctions.Get(GameAddresses.JobGaugeManager);

    /// <summary>Gets the address of the place the gauges of all the jobs share.</summary>
    /// <remarks>
    /// The gauge of the job the player is in is read from this place, which is
    /// why it is the same for every job.
    /// </remarks>
    public IntPtr SharedAddress
        => (IntPtr)((byte*)StructPtr.Of(ref this) + SharedGaugesOffset);

    /// <summary>Gets the gauge of the astrologian.</summary>
    public AstrologianGauge Astrologian => Read<AstrologianGauge>(0x00);

    /// <summary>Gets the gauge of the black mage.</summary>
    public BlackMageGauge BlackMage => Read<BlackMageGauge>(0x00);

    /// <summary>Gets the gauge of the bard.</summary>
    public BardGauge Bard => Read<BardGauge>(0x00);

    /// <summary>Gets the gauge of the dancer.</summary>
    public DancerGauge Dancer => Read<DancerGauge>(0x00);

    /// <summary>Gets the gauge of the dragoon.</summary>
    public DragoonGauge Dragoon => Read<DragoonGauge>(0x00);

    /// <summary>Gets the gauge of the dark knight.</summary>
    public DarkKnightGauge DarkKnight => Read<DarkKnightGauge>(0x00);

    /// <summary>Gets the gauge of the gunbreaker.</summary>
    public GunbreakerGauge Gunbreaker => Read<GunbreakerGauge>(0x00);

    /// <summary>Gets the gauge of the machinist.</summary>
    public MachinistGauge Machinist => Read<MachinistGauge>(0x00);

    /// <summary>Gets the gauge of the monk.</summary>
    public MonkGauge Monk => Read<MonkGauge>(0x00);

    /// <summary>Gets the gauge of the ninja.</summary>
    public NinjaGauge Ninja => Read<NinjaGauge>(0x00);

    /// <summary>Gets the gauge of the paladin.</summary>
    public PaladinGauge Paladin => Read<PaladinGauge>(0x00);

    /// <summary>Gets the gauge of the pictomancer.</summary>
    public PictomancerGauge Pictomancer => Read<PictomancerGauge>(0x00);

    /// <summary>Gets the gauge of the red mage.</summary>
    public RedMageGauge RedMage => Read<RedMageGauge>(0x00);

    /// <summary>Gets the gauge of the reaper.</summary>
    public ReaperGauge Reaper => Read<ReaperGauge>(0x00);

    /// <summary>Gets the gauge of the samurai.</summary>
    public SamuraiGauge Samurai => Read<SamuraiGauge>(0x00);

    /// <summary>Gets the gauge of the scholar.</summary>
    public ScholarGauge Scholar => Read<ScholarGauge>(0x00);

    /// <summary>Gets the gauge of the sage.</summary>
    public SageGauge Sage => Read<SageGauge>(0x00);

    /// <summary>Gets the gauge of the summoner.</summary>
    public SummonerGauge Summoner => Read<SummonerGauge>(0x00);

    /// <summary>Gets the gauge of the viper.</summary>
    public ViperGauge Viper => Read<ViperGauge>(0x00);

    /// <summary>Gets the gauge of the warrior.</summary>
    public WarriorGauge Warrior => Read<WarriorGauge>(0x00);

    /// <summary>Gets the gauge of the white mage.</summary>
    public WhiteMageGauge WhiteMage => Read<WhiteMageGauge>(0x00);

    /// <summary>
    /// Reads a gauge out of the place the gauges share.
    /// </summary>
    /// <typeparam name="T">The kind of the gauge.</typeparam>
    /// <param name="offset">The place of the gauge inside the shared place.</param>
    /// <returns>The gauge.</returns>
    private T Read<T>(int offset)
        where T : struct
    {
        fixed (byte* bytes = Gauges)
        {
            return MemoryMarshal.Read<T>(new ReadOnlySpan<byte>(bytes + offset, SizeOf<T>()));
        }
    }

    /// <summary>
    /// The number of bytes a gauge takes.
    /// </summary>
    /// <typeparam name="T">The kind of the gauge.</typeparam>
    /// <returns>The number of bytes.</returns>
    private static int SizeOf<T>()
        where T : struct
        => System.Runtime.CompilerServices.Unsafe.SizeOf<T>();
}
