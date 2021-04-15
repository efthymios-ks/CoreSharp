namespace CoreSharp.Extensions
{
    /// <summary>
    /// Double extensions. 
    /// </summary>
    public static partial class DoubleExtensions
    {
        /// <summary>
        /// Re-maps a number from one range to another. 
        /// </summary>
        public static double Map(this double value, double fromLow, double fromHigh, double toLow, double toHigh)
        {
            var dValue = (decimal)value;
            var dFromLow = (decimal)fromLow;
            var dFromHigh = (decimal)fromHigh;
            var dToLow = (decimal)toLow;
            var dToHigh = (decimal)toHigh;
            var dResult = dValue.Map(dFromLow, dFromHigh, dToLow, dToHigh);
            return (double)dResult;
        }
    }
}
