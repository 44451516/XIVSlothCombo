namespace SlothCombo.Core
{
    /// <summary>
    /// The steps a slider can be moved by.
    /// </summary>
    public static class SliderIncrements
    {
        /// <summary> One step at a time. </summary>
        public const uint Ones = 1;

        /// <summary> Five steps at a time. </summary>
        public const uint Fives = 5;

        /// <summary> Ten steps at a time. </summary>
        public const uint Tens = 10;

        /// <summary> A hundred steps at a time. </summary>
        public const uint Hundreds = 100;

        /// <summary> A thousand steps at a time. </summary>
        public const uint Thousands = 1000;
    }
}