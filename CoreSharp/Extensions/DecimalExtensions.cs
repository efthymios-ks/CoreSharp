namespace CoreSharp.Extensions
{
    /// <summary>
    /// Decimal extensions. 
    /// </summary>
    public static partial class DecimalExtensions
    {
        /// <summary>
        /// Re-maps a number from one range to another. 
        /// </summary>
        public static decimal Map(this decimal value, decimal fromLow, decimal fromHigh, decimal toLow, decimal toHigh)
        {
            return (value - fromLow) * ((toHigh - toLow) / (fromHigh - fromLow)) + toLow;
        }
    }
}
