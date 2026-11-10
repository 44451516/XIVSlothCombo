using SlothCombo.Abstractions;
using SlothCombo.Interop.Game;

namespace SlothCombo.Extensions
{
    internal static class BattleCharaExtensions
    {
        
        public unsafe static uint RawShieldValue(this IBattleChara chara)
        {
            SlothCombo.Interop.Game.BattleChara* baseVal = (SlothCombo.Interop.Game.BattleChara*)chara.Address;
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
