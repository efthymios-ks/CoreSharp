using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Random extensions. 
    /// </summary>
    public static partial class RandomExtensions
    {
        /// <summary>
        /// Return true or false. 
        /// </summary>
        public static bool CoinToss(this Random RNG)
        {
            _ = RNG ?? throw new ArgumentNullException(nameof(RNG));

            return RNG.Next(2) == 0;
        }

        /// <inheritdoc cref="OneOf{T}(Random, T[])"/>
        public static T OneOf<T>(this Random RNG, IEnumerable<T> source)
            => RNG.OneOf(source?.ToArray());

        /// <summary>
        /// Return random value from list of values. 
        /// </summary>
        public static T OneOf<T>(this Random RNG, params T[] source)
        {
            _ = RNG ?? throw new ArgumentNullException(nameof(RNG));
            _ = source ?? throw new ArgumentNullException(nameof(source));
            if (source.Length == 0)
                throw new ArgumentException($"{nameof(source)} cannot be empty.", nameof(source));

            int topExclusive = source.Length;
            int index = RNG.Next(topExclusive);
            return source[index];
        }

        /// <inheritdoc cref="NextDouble(Random, double)"/>
        public static double NextDouble(this Random RNG, double minimum, double maximum)
        {
            _ = RNG ?? throw new ArgumentNullException(nameof(RNG));
            if (minimum > maximum)
                throw new ArgumentException($"{nameof(minimum)} ({minimum}) cannot be greater than {nameof(maximum)} ({maximum}).", nameof(minimum));

            double randomValue = RNG.NextDouble();
            return randomValue * (maximum - minimum) + minimum;
        }

        /// <summary>
        /// Get random double in given range. 
        /// </summary>
        public static double NextDouble(this Random RNG, double maximum) => RNG.NextDouble(0, maximum);

        /// <summary>
        /// Check is percentage chance is greater than a given value. 
        /// </summary>
        public static bool ChanceGreaterThan(this Random RNG, double percentage, bool includeEnds = true)
        {
            _ = RNG ?? throw new ArgumentNullException(nameof(RNG));
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException($"{nameof(percentage)} ({percentage}%) has to be between 0 and 100.");

            double chance = RNG.NextDouble(0, 100);

            if (includeEnds)
                return chance >= percentage;
            else
                return chance > percentage;
        }

        /// <summary>
        /// Check is percentage chance is lower than a given value. 
        /// </summary>
        public static bool ChanceLowerThan(this Random RNG, double percentage, bool includeEnds = true)
        {
            _ = RNG ?? throw new ArgumentNullException(nameof(RNG));
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException($"{nameof(percentage)} ({percentage}%) has to be between 0 and 100.");

            double chance = RNG.NextDouble(0, 100);

            if (includeEnds)
                return chance <= percentage;
            else
                return chance < percentage;
        }

        /// <summary>
        /// Check is percentage chance is between two limits (including limits). 
        /// </summary>
        public static bool ChanceBetween(this Random RNG, double percentageLeft, double percentageRight, bool includeEnds = true)
        {
            _ = RNG ?? throw new ArgumentNullException(nameof(RNG));
            if (percentageLeft < 0)
                throw new ArgumentOutOfRangeException($"{nameof(percentageLeft)} ({percentageLeft}%) has to be between 0 and 100.");
            else if (percentageRight > 100)
                throw new ArgumentOutOfRangeException($"{nameof(percentageRight)} ({percentageRight}%) has to be between 0 and 100.");
            else if (percentageLeft > percentageRight)
                throw new ArgumentException($"{nameof(percentageLeft)} ({percentageLeft}%) cannot be greater than {nameof(percentageRight)} ({percentageRight}%).");

            double chance = RNG.NextDouble(0, 100);
            if (includeEnds)
                return chance >= percentageLeft && chance <= percentageRight;
            else
                return chance > percentageLeft && chance < percentageRight;
        }

        /// <summary>
        /// Shuffle IList. 
        /// </summary>
        public static void Shuffle<T>(this Random RNG, IList<T> source)
        {
            _ = RNG ?? throw new ArgumentNullException(nameof(RNG));
            _ = source ?? throw new ArgumentNullException(nameof(source));

            for (int currentIndex = 0; currentIndex < (source.Count - 1); currentIndex++)
            {
                var randomIndex = RNG.Next(currentIndex, source.Count);

                var tmp = source[currentIndex];
                source[currentIndex] = source[randomIndex];
                source[randomIndex] = tmp;
            }
        }
    }
}
