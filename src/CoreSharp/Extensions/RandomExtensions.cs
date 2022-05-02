using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="Random"/> extensions.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Return <see langword="true"/> or <see langword="false"/>.
        /// </summary>
        public static bool NextBool(this Random random)
        {
            _ = random ?? throw new ArgumentNullException(nameof(random));

            return random.Next(2) == 0;
        }

        /// <inheritdoc cref="OneOf{TElement}(Random, TElement[])"/>
        public static TElement OneOf<TElement>(this Random random, IEnumerable<TElement> source)
            => random.OneOf(source?.ToArray());

        /// <summary>
        /// Return random value from list of values.
        /// </summary>
        public static TElement OneOf<TElement>(this Random random, params TElement[] source)
        {
            _ = random ?? throw new ArgumentNullException(nameof(random));
            _ = source ?? throw new ArgumentNullException(nameof(source));
            if (source.Length == 0)
                throw new ArgumentException($"{nameof(source)} cannot be empty.", nameof(source));

            var topExclusive = source.Length;
            var index = random.Next(topExclusive);
            return source[index];
        }

        /// <inheritdoc cref="NextDouble(Random, double)"/>
        public static double NextDouble(this Random random, double maximum)
            => random.NextDouble(0, maximum);

        /// <summary>
        /// Get random double in given range.
        /// </summary>
        public static double NextDouble(this Random random, double minimum, double maximum)
        {
            _ = random ?? throw new ArgumentNullException(nameof(random));
            if (minimum > maximum)
                throw new ArgumentException($"{nameof(minimum)} ({minimum}) cannot be greater than {nameof(maximum)} ({maximum}).", nameof(minimum));

            var randomValue = random.NextDouble();
            return randomValue * (maximum - minimum) + minimum;
        }

        /// <inheritdoc cref="NextString(Random, int)"/>
        public static string NextString(this Random random)
        {
            _ = random ?? throw new ArgumentNullException(nameof(random));
            var randomSize = random.Next(0, byte.MaxValue);
            return random.NextString(randomSize);
        }

        /// <summary>
        /// Generate random string containing A-Z, a-z, 0-1.
        /// </summary>
        public static string NextString(this Random random, int size)
        {
            _ = random ?? throw new ArgumentNullException(nameof(random));
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size), $"{nameof(size)} has to be at least 1.");

            const string seed = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new char[size];

            for (var i = 0; i < result.Length; i++)
            {
                var randomIndex = random.Next(seed.Length);
                result[i] = seed[randomIndex];
            }

            return new string(result);
        }

        /// <summary>
        /// Check is percentage chance is greater than a given value.
        /// </summary>
        public static bool ChanceGreaterThan(this Random random, double percentage, bool includeEnd = true)
        {
            _ = random ?? throw new ArgumentNullException(nameof(random));
            if (percentage is < 0 or > 100)
                throw new ArgumentOutOfRangeException($"{nameof(percentage)} ({percentage}%) has to be between 0 and 100.");

            var chance = random.NextDouble(0, 100);

            if (includeEnd)
                return chance >= percentage;
            return chance > percentage;
        }

        /// <summary>
        /// Check is percentage chance is lower than a given value.
        /// </summary>
        public static bool ChanceLowerThan(this Random random, double percentage, bool includeEnd = true)
        {
            _ = random ?? throw new ArgumentNullException(nameof(random));
            if (percentage is < 0 or > 100)
                throw new ArgumentOutOfRangeException($"{nameof(percentage)} ({percentage}%) has to be between 0 and 100.");

            var chance = random.NextDouble(0, 100);

            if (includeEnd)
                return chance <= percentage;
            return chance < percentage;
        }

        /// <summary>
        /// Check is percentage chance is between two limits (including limits).
        /// </summary>
        public static bool ChanceBetween(this Random random, double percentageLeft, double percentageRight, bool includeEnds = true)
        {
            _ = random ?? throw new ArgumentNullException(nameof(random));
            if (percentageLeft < 0)
                throw new ArgumentOutOfRangeException($"{nameof(percentageLeft)} ({percentageLeft}%) has to be between 0 and 100.");
            else if (percentageRight > 100)
                throw new ArgumentOutOfRangeException($"{nameof(percentageRight)} ({percentageRight}%) has to be between 0 and 100.");
            else if (percentageLeft > percentageRight)
                throw new ArgumentException($"{nameof(percentageLeft)} ({percentageLeft}%) cannot be greater than {nameof(percentageRight)} ({percentageRight}%).");

            var chance = random.NextDouble(0, 100);
            if (includeEnds)
                return chance >= percentageLeft && chance <= percentageRight;
            else
                return chance > percentageLeft && chance < percentageRight;
        }

        /// <summary>
        /// Shuffle <see cref="IList{TElement}"/>.
        /// </summary>
        public static void Shuffle<TElement>(this Random random, IList<TElement> source)
        {
            _ = random ?? throw new ArgumentNullException(nameof(random));
            _ = source ?? throw new ArgumentNullException(nameof(source));

            for (var currentIndex = 0; currentIndex < source.Count - 1; currentIndex++)
            {
                var randomIndex = random.Next(currentIndex, source.Count);

                //Swap 
                (source[currentIndex], source[randomIndex]) = (source[randomIndex], source[currentIndex]);
            }
        }
    }
}
