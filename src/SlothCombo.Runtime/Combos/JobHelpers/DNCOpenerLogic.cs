using SlothCombo.Runtime.Gauges;
using FFXIVClientStructs.FFXIV.Client.Game;
using SlothCombo.Combos.PvE;
using SlothCombo.Core;
using SlothCombo.CustomComboNS.Functions;
using SlothCombo.Extensions;

namespace SlothCombo.Combos.JobHelpers
{
    internal class DNCOpenerLogic : PvE.DNC
    {    
        
        private static uint OpenerLevel => 90;
        public bool DoFullOpener(ref uint actionID)
        {
            
            if (!CustomComboFunctions.LevelChecked(标准舞步StandardStep))
                return false;

            if (CustomComboFunctions.InCombat())
            {
                return false;
            }


            if (!CustomComboFunctions.InCombat())
            {

                DNCGauge? gauge = CustomComboFunctions.GetJobGauge<DNCGauge>();
                if (CustomComboFunctions.HasEffect(Buffs.标准舞步预备StandardStep))
                {
                    if (gauge.CompletedSteps < 2)
                    {
                        actionID = gauge.NextStep;
                        if (actionID.ActionReady())
                        {
                            unsafe
                            {
                                ActionManager.Instance()->UseAction(ActionType.Action, actionID);
                            }
                        }
                    }
                }



                if (Countdown.TimeRemaining() <= 15)
                {
                    if (标准舞步StandardStep.ActionReady())
                    {
                        actionID = 标准舞步StandardStep;
                        unsafe
                        {
                            ActionManager.Instance()->UseAction(ActionType.Action, actionID);
                        }
                    }
                }

                if (Countdown.TimeRemaining() is > 0.3f and < 0.5f)
                {
                    if (双色标准舞步结束StandardFinish2.ActionReady())
                    {
                        actionID = 双色标准舞步结束StandardFinish2;
                        unsafe
                        {
                            ActionManager.Instance()->UseAction(ActionType.Action, 双色标准舞步结束StandardFinish2);
                        }
                    }
                }

                if (标准舞步StandardStep.ActionReady())
                {
                    actionID = 标准舞步StandardStep;
                    return true;
                }

                return false;
            }

            return false;
        }
    }



}
