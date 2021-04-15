namespace CoreSharp.Extensions
{
    /// <summary>
    /// Int extensions. 
    /// </summary>
    public static partial class IntExtensions
    {
        /// <summary>
        /// Re-maps a number from one range to another. 
        /// </summary>
        public static int Map(this int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            var dValue = (decimal)value;
            var dFromLow = (decimal)fromLow;
            var dFromHigh = (decimal)fromHigh;
            var dToLow = (decimal)toLow;
            var dToHigh = (decimal)toHigh;
            var dResult = dValue.Map(dFromLow, dFromHigh, dToLow, dToHigh);
            return (int)dResult;
        }
    }
}
