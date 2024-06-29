using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game.Character;

namespace XIVSlothComboX.Extensions
{
    internal static class BattleCharaExtensions
    {
        
        public unsafe static uint RawShieldValue(this IBattleChara chara)
        {
            FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara* baseVal = (FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara*)chara.Address;
            var value = baseVal->Character.CharacterData.ShieldValue;
            var rawValue = chara.CurrentHp / 100 * value;

            return rawValue;
        }

        public unsafe static byte ShieldPercentage(this IBattleChara chara)
        {
            BattleChara* baseVal = (BattleChara*)chara.Address;
            var value = baseVal->Character.CharacterData.ShieldValue;
            return value;
        }

        public static bool HasShield(this IBattleChara chara) => chara.RawShieldValue() > 0;
    }
}
