using SlothCombo.Combos.PvE;
using SlothCombo.CustomComboNS.Functions;
using SlothCombo.Data;

namespace SlothCombo.Extensions
{
    internal static class UIntExtensions
    {
        internal static bool LevelChecked(this uint value) => CustomComboFunctions.LevelChecked(value);
        
        internal static bool TraitLevelChecked(this uint value) => CustomComboFunctions.TraitLevelChecked(value);

        internal static bool ActionReady(this  uint value) => CustomComboFunctions.ActionReady(value);
        internal static uint OriginalHook(this uint value) => CustomComboFunctions.OriginalHook(value);

        internal static string ActionName(this uint value) => ActionWatching.GetActionName(value);


        internal static bool GCDActionReady(this uint value)
        {
            if (!CustomComboFunctions.LevelChecked(value))
            {
                return false;
            }

            var gcdCooldownRemainingTime = CustomComboFunctions.GetCooldownRemainingTime(WAR.Tomahawk);
            var cooldownRemainingTime = CustomComboFunctions.GetCooldownRemainingTime(value);

            if (CustomComboFunctions.GetCooldownRemainingTime(value) <= 0 || cooldownRemainingTime - gcdCooldownRemainingTime <= 0)
            {
                return true;
            }
            
            if (CustomComboFunctions.GetMaxCharges(value) > 1)
            {
                if (cooldownRemainingTime - gcdCooldownRemainingTime <= CustomComboFunctions.GetCooldown(value).单次计时器 + 0.5f)
                {
                    return true;
                }
            }


            return false;
        }



        internal static bool GCDActionReady(this uint value, uint gcdActionId)
        {
            if (!CustomComboFunctions.LevelChecked(value))
            {
                return false;
            }

            if (CustomComboFunctions.GetCooldownRemainingTime(value) <= 0
                || (CustomComboFunctions.GetCooldownRemainingTime(value) <= CustomComboFunctions.GetCooldownRemainingTime(gcdActionId)
                    && CustomComboFunctions.GetCooldownRemainingTime(gcdActionId) < 1.5f))
            {
                return true;
            }
            return false;
        }


        internal static bool GCDActionReady(this uint value, uint gcdActionId, float RemainingTime)
        {
            if (!CustomComboFunctions.LevelChecked(value))
            {
                return false;
            }

            if (CustomComboFunctions.GetCooldownRemainingTime(value) <= 0
                || (CustomComboFunctions.GetCooldownRemainingTime(value) <= CustomComboFunctions.GetCooldownRemainingTime(gcdActionId)
                    && CustomComboFunctions.GetCooldownRemainingTime(gcdActionId) < RemainingTime))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 能力技好之前的最后一个gcd
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ActionId"></param>
        /// <returns></returns>
        internal static bool GCDActionPreReady(this uint value, uint ActionId)
        {
            if (!CustomComboFunctions.LevelChecked(value))
            {
                return false;
            }

            if ((CustomComboFunctions.GetCooldownRemainingTime(ActionId) > 0 && CustomComboFunctions.GetCooldownRemainingTime(ActionId) < 2.5f)
                && CustomComboFunctions.GetCooldownRemainingTime(value) < 2f
                && CustomComboFunctions.GetCooldownRemainingTime(value) < CustomComboFunctions.GetCooldownRemainingTime(ActionId))
            {
                return true;
            }
            return false;
        }
    }

    internal static class UShortExtensions
    {
        internal static string StatusName(this ushort value) => ActionWatching.GetStatusName(value);
    }
}