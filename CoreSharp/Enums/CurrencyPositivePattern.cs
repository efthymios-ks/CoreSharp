namespace CoreSharp.Enums
{
    /// <summary>
    /// Enum used with NumberFormatInfo.CurrencyPositivePattern. 
    /// Just cast to int. 
    /// </summary>
    public enum CurrencyPositivePattern
    {
        /// <summary>
        /// $n
        /// </summary>
        CurrencyNumber = 0,

        /// <summary>
        /// n$
        /// </summary>
        NumberCurrency = 1,

        /// <summary>
        /// $ n
        /// </summary>
        CurrencySpaceNumber = 2,

        /// <summary>
        ///  n $
        /// </summary>
        NumberSpaceCurrency = 3
    }
}
