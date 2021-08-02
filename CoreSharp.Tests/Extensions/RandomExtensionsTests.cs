using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class RandomExtensionsTests
    {
        //Fields
        private readonly Random rngNull = null;
        private readonly Random rng = new Random(DateTime.Now.Millisecond);
        private const int SampleCount = 5;

        [Test]
        public void CoinToss_RngIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => rngNull.CoinToss();

            //Act
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void OneOf_RngIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => rngNull.OneOf(1, 2, 3);

            //Act
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void OneOf_SourceIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => rng.OneOf<int>(null);

            //Act
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void OneOf_SourceIEmpty_ThrowArgumentException()
        {
            //Act
            Action action = () => rng.OneOf<int>();

            //Act
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void NextDouble_RngIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => rngNull.NextDouble(0, 100);

            //Act
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(0, 100, SampleCount)]
        public void NextDouble_WhenCalled_ReturnRandomValueInRange(double minimum, double maximum, int sampleCount)
        {
            //Arrange 
            var samples = new double[sampleCount];

            //Act 
            for (int i = 0; i < samples.Length; i++)
                samples[i] = rng.NextDouble(minimum, maximum);

            //Act
            samples.Should().OnlyContain(s => s >= minimum && s <= maximum);
        }

        [Test]
        public void ChanceGreaterThan_RngIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => rngNull.ChanceGreaterThan(50);

            //Act
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(-1)]
        [TestCase(101)]
        public void ChanceGreaterThan_PercentageOutOfRange_ThrowArgumentOutOfRangeException(double percentage)
        {
            //Act 
            Action action = () => rng.ChanceGreaterThan(percentage);

            //Act
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void ChanceLowerThan_RngIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => rngNull.ChanceLowerThan(50);

            //Act
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(-1)]
        [TestCase(101)]
        public void ChanceLowerThan_PercentageOutOfRange_ThrowArgumentOutOfRangeException(double percentage)
        {
            //Act 
            Action action = () => rng.ChanceLowerThan(percentage);

            //Act
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void ChanceBetween_RngIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => rngNull.ChanceBetween(25, 75);

            //Act
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(-1, 75)]
        [TestCase(25, 101)]
        public void ChanceLowerThan_EndsOutOfRange_ThrowArgumentOutOfRangeException(double percentageLeft, double percentageRight)
        {
            //Act 
            Action action = () => rng.ChanceBetween(percentageLeft, percentageRight);

            //Act
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        [TestCase(75, 25)]
        public void ChanceLowerThan_LeftGreaterThanRight_ThrowArgumentOutOfRangeException(double percentageLeft, double percentageRight)
        {
            //Act 
            Action action = () => rng.ChanceBetween(percentageLeft, percentageRight);

            //Act
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void Shuffle_RngIsNull_ThrowArgumentNullException()
        {
            //Arrange
            var source = new List<int>
            {
                1, 2, 3,4,5
            };

            //Act 
            Action action = () => rngNull.Shuffle(source);

            //Act
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Shuffle_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => rng.Shuffle<int>(null);

            //Act
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(SampleCount)]
        public void Shuffle_WhenCalled_ShufflesList(int sampleCount)
        {
            //Arrange
            var original = new List<int>
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };
            var shuffled = original.ToList();

            //Act 
            for (int i = 0; i < sampleCount; i++)
                rng.Shuffle(shuffled);

            //Act  
            shuffled.Should().NotEqual(original);
            shuffled.Should().OnlyContain(i => original.Any(o => o.Equals(i)));
        }
    }
}