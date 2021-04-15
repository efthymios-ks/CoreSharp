namespace CoreSharp.Extensions
{
    /// <summary>
    /// Float extensions. 
    /// </summary>
    public static partial class FloatExtensions
    {
        /// <summary>
        /// Re-maps a number from one range to another. 
        /// </summary>
        public static float Map(this float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            var dValue = (decimal)value;
            var dFromLow = (decimal)fromLow;
            var dFromHigh = (decimal)fromHigh;
            var dToLow = (decimal)toLow;
            var dToHigh = (decimal)toHigh;
            var dResult = dValue.Map(dFromLow, dFromHigh, dToLow, dToHigh);
            return (float)dResult;
        }
    }
}
