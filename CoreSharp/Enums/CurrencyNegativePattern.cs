using System;
using System.Globalization;

namespace CoreSharp.Enums
{
    /// <summary>
    /// <see cref="Enum"/> used with <see cref="NumberFormatInfo.CurrencyNegativePattern"/>.
    /// Can be cast to <see cref="int"/>.
    /// </summary>
    public enum CurrencyNegativePattern
    {
        /// <summary>
        /// ($n)
        /// </summary>
        ParenthesisCurrencyNumber = 0,

        /// <summary>
        /// -$n
        /// </summary>
        SignCurrencyNumber = 1,

        /// <summary>
        /// $-n
        /// </summary>
        CurrencySignNumber = 2,

        /// <summary>
        /// $n-
        /// </summary>
        CurrencyNumberSign = 3,

        /// <summary>
        /// (n$)
        /// </summary>
        ParenthesisNumberCurrency = 4,

        /// <summary>
        /// -n$
        /// </summary>
        SignNumberCurrency = 5,

        /// <summary>
        /// n-$
        /// </summary>
        NumberSignCurrency = 6,

        /// <summary>
        /// n$-
        /// </summary>
        NumberCurrencySign = 7,

        /// <summary>
        /// -n $
        /// </summary>
        SignNumberSpaceCurrency = 8,

        /// <summary>
        /// -$ n
        /// </summary>
        SignCurrencySpaceNumber = 9,

        /// <summary>
        /// n $-
        /// </summary>
        NumberSpaceCurrencySign = 10,

        /// <summary>
        /// $ n-
        /// </summary>
        CurrencySpaceNumberSign = 11,

        /// <summary>
        /// $ -n
        /// </summary>
        CurrencySpaceSignNumber = 12,

        /// <summary>
        /// n- $
        /// </summary>
        NumberSignSpaceCurrency = 13,

        /// <summary>
        /// ($ n)
        /// </summary>
        ParenthesisCurrencySpaceNumber = 14,

        /// <summary>
        /// (n $)
        /// </summary>
        ParenthesisNumberSpaceCurrency = 15
    }
}
