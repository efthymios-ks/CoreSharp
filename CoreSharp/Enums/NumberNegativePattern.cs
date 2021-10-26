namespace CoreSharp.Enums
{
    /// <summary>
    /// Enum used with NumberFormatInfo.NumberNegativePattern.
    /// Just cast to int.
    /// </summary>
    public enum NumberNegativePattern
    {
        /// <summary>
        /// (n)
        /// </summary>
        ParenthesisNumber = 0,

        /// <summary>
        /// -n
        /// </summary>
        SignNumber = 1,

        /// <summary>
        /// - n
        /// </summary>
        SignSpaceNumber = 2,

        /// <summary>
        /// n-
        /// </summary>
        NumberSign = 3,

        /// <summary>
        /// n -
        /// </summary>
        NumberSpaceSign = 4
    }
}
