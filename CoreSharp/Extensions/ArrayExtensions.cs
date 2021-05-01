using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Array extensions. 
    /// </summary>
    public static partial class ArrayExtensions
    {
        /// <summary>
        /// Get row from 2D array. 
        /// </summary> 
        public static IEnumerable<T> GetRow<T>(this T[,] source, int row)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            int rowSize = source.GetLength(0);
            if (row < 0 || row >= rowSize)
                throw new ArgumentOutOfRangeException(nameof(row), $"{nameof(row)} has to be between 0 and {rowSize - 1}.");

            var columnSize = source.GetLength(1);
            return Enumerable
                        .Range(0, columnSize)
                        .Select(c => source[row, c]);
        }

        /// <summary>
        /// Get column from 2D array. 
        /// </summary> 
        public static IEnumerable<T> GetColumn<T>(this T[,] source, int column)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            int columnSize = source.GetLength(1);
            if (column < 0 || column >= columnSize)
                throw new ArgumentOutOfRangeException(nameof(column), $"{nameof(column)} has to be between 0 and {columnSize - 1}.");

            var rowSize = source.GetLength(0);
            return Enumerable
                        .Range(0, rowSize)
                        .Select(r => source[r, column]);
        }
    }
}
