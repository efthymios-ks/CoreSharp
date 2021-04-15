namespace CoreSharp.Extensions
{
    /// <summary>
    /// Byte extensions. 
    /// </summary>
    public static partial class ByteExtensions
    {
        /// <summary>
        /// Re-maps a number from one range to another. 
        /// </summary>
        public static byte Map(this byte value, byte fromLow, byte fromHigh, byte toLow, byte toHigh)
        {
            var dValue = (decimal)value;
            var dFromLow = (decimal)fromLow;
            var dFromHigh = (decimal)fromHigh;
            var dToLow = (decimal)toLow;
            var dToHigh = (decimal)toHigh;
            var dResult = dValue.Map(dFromLow, dFromHigh, dToLow, dToHigh);
            return (byte)dResult;
        }
    }
}
