using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Random extensions.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Return true or false.
        /// </summary>
        public static bool CoinToss(this Random rng)
        {
            _ = rng ?? throw new ArgumentNullException(nameof(rng));

            return rng.Next(2) == 0;
        }

        /// <inheritdoc cref="OneOf{T}(Random, T[])"/>
        public static T OneOf<T>(this Random rng, IEnumerable<T> source)
            => rng.OneOf(source?.ToArray());

        /// <summary>
        /// Return random value from list of values.
        /// </summary>
        public static T OneOf<T>(this Random rng, params T[] source)
        {
            _ = rng ?? throw new ArgumentNullException(nameof(rng));
            _ = source ?? throw new ArgumentNullException(nameof(source));
            if (source.Length == 0)
                throw new ArgumentException($"{nameof(source)} cannot be empty.", nameof(source));

            var topExclusive = source.Length;
            var index = rng.Next(topExclusive);
            return source[index];
        }

        /// <inheritdoc cref="NextDouble(Random, double)"/>
        public static double NextDouble(this Random rng, double minimum, double maximum)
        {
            _ = rng ?? throw new ArgumentNullException(nameof(rng));
            if (minimum > maximum)
                throw new ArgumentException($"{nameof(minimum)} ({minimum}) cannot be greater than {nameof(maximum)} ({maximum}).", nameof(minimum));

            var randomValue = rng.NextDouble();
            return (randomValue * (maximum - minimum)) + minimum;
        }

        /// <summary>
        /// Get random double in given range.
        /// </summary>
        public static double NextDouble(this Random rng, double maximum) => rng.NextDouble(0, maximum);

        /// <summary>
        /// Check is percentage chance is greater than a given value.
        /// </summary>
        public static bool ChanceGreaterThan(this Random rng, double percentage, bool includeEnds = true)
        {
            _ = rng ?? throw new ArgumentNullException(nameof(rng));
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException($"{nameof(percentage)} ({percentage}%) has to be between 0 and 100.");

            var chance = rng.NextDouble(0, 100);

            if (includeEnds)
                return chance >= percentage;
            else
                return chance > percentage;
        }

        /// <summary>
        /// Check is percentage chance is lower than a given value.
        /// </summary>
        public static bool ChanceLowerThan(this Random rng, double percentage, bool includeEnds = true)
        {
            _ = rng ?? throw new ArgumentNullException(nameof(rng));
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException($"{nameof(percentage)} ({percentage}%) has to be between 0 and 100.");

            var chance = rng.NextDouble(0, 100);

            if (includeEnds)
                return chance <= percentage;
            else
                return chance < percentage;
        }

        /// <summary>
        /// Check is percentage chance is between two limits (including limits).
        /// </summary>
        public static bool ChanceBetween(this Random rng, double percentageLeft, double percentageRight, bool includeEnds = true)
        {
            _ = rng ?? throw new ArgumentNullException(nameof(rng));
            if (percentageLeft < 0)
                throw new ArgumentOutOfRangeException($"{nameof(percentageLeft)} ({percentageLeft}%) has to be between 0 and 100.");
            else if (percentageRight > 100)
                throw new ArgumentOutOfRangeException($"{nameof(percentageRight)} ({percentageRight}%) has to be between 0 and 100.");
            else if (percentageLeft > percentageRight)
                throw new ArgumentException($"{nameof(percentageLeft)} ({percentageLeft}%) cannot be greater than {nameof(percentageRight)} ({percentageRight}%).");

            var chance = rng.NextDouble(0, 100);
            if (includeEnds)
                return chance >= percentageLeft && chance <= percentageRight;
            else
                return chance > percentageLeft && chance < percentageRight;
        }

        /// <summary>
        /// Shuffle IList.
        /// </summary>
        public static void Shuffle<T>(this Random rng, IList<T> source)
        {
            _ = rng ?? throw new ArgumentNullException(nameof(rng));
            _ = source ?? throw new ArgumentNullException(nameof(source));

            for (var currentIndex = 0; currentIndex < (source.Count - 1); currentIndex++)
            {
                var randomIndex = rng.Next(currentIndex, source.Count);
                (source[currentIndex], source[randomIndex]) = (source[randomIndex], source[currentIndex]);
            }
        }
    }
}
