namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="decimal"/> extensions.
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Re-maps a number from one range to another.
        /// </summary>
        public static decimal Map(this decimal value, decimal fromLow, decimal fromHigh, decimal toLow, decimal toHigh)
            => ((value - fromLow) * ((toHigh - toLow) / (fromHigh - fromLow))) + toLow;
    }
}
