using System;
using SlothCombo.Interop.UI;
using SlothCombo.Runtime.Services;

namespace SlothCombo.CustomComboNS.Functions
{
    internal abstract partial class CustomComboFunctions
    {
        
        private static DateTime? movementStarted;
     
        
        
        public static unsafe bool IsMoving()
        {
            bool isMoving = AgentMap.Instance() is not null && AgentMap.Instance()->IsPlayerMoving;

            if (isMoving && movementStarted is null)
                movementStarted = DateTime.Now;

            if (!isMoving)
                movementStarted = null;

            return isMoving && (TimeMoving.TotalMilliseconds / 1000f) >= Service.Configuration.MovementLeeway;
        }

        public static TimeSpan TimeMoving => movementStarted is null ? TimeSpan.Zero : (DateTime.Now - movementStarted.Value);

    }
}