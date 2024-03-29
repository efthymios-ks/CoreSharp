﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="Array"/> extensions.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Get row from 2D array.
    /// </summary>
    public static IEnumerable<TElement> GetRow<TElement>(this TElement[,] source, int row)
    {
        ArgumentNullException.ThrowIfNull(source);
        var rowSize = source.GetLength(0);
        if (row < 0 || row >= rowSize)
        {
            throw new ArgumentOutOfRangeException(nameof(row), $"{nameof(row)} has to be between 0 and {rowSize - 1}.");
        }

        var columnSize = source.GetLength(1);
        return Enumerable.Range(0, columnSize).Select(c => source[row, c]);
    }

    /// <summary>
    /// Get column from 2D array.
    /// </summary>
    public static IEnumerable<TElement> GetColumn<TElement>(this TElement[,] source, int column)
    {
        ArgumentNullException.ThrowIfNull(source);
        var columnSize = source.GetLength(1);
        if (column < 0 || column >= columnSize)
        {
            throw new ArgumentOutOfRangeException(nameof(column), $"{nameof(column)} has to be between 0 and {columnSize - 1}.");
        }

        var rowSize = source.GetLength(0);
        return Enumerable.Range(0, rowSize)
                         .Select(r => source[r, column]);
    }

    /// <inheritdoc cref="Slice{TElement}(TElement[], int, int)"/>
    public static TElement[] Slice<TElement>(this TElement[] source, int offset)
        => source.Slice(offset, (source?.Length ?? 0) - offset);

    /// <summary>
    /// Get sub-array given offset and length.
    /// </summary>
    public static TElement[] Slice<TElement>(this TElement[] source, int offset, int length)
    {
        var result = new TElement[length];
        Array.Copy(source, offset, result, 0, length);
        return result;
    }

    /// <summary>
    /// Return <see cref="Array.Empty{TElement}"/> if source is null.
    /// </summary>
    public static TElement[] OrEmpty<TElement>(this TElement[] source)
        => source ?? Array.Empty<TElement>();
}
