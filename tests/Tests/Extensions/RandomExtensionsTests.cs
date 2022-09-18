namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class RandomExtensionsTests
{
    // Fields
    private readonly Random _rngNull;
    private readonly Random _rng = new(DateTime.Now.Millisecond);
    private const int SampleCount = 5;

    [Test]
    public void NextBool_RngIsNull_ThrowArgumentNullException()
    {
        // Act
        Action action = () => _rngNull.NextBool();

        // Act
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void OneOf_RngIsNull_ThrowArgumentNullException()
    {
        // Act 
        Action action = () => _rngNull.OneOf(1, 2, 3);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void OneOf_SourceIsNull_ThrowArgumentNullException()
    {
        // Act
        Action action = () => _rng.OneOf<int>(null);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void OneOf_SourceIEmpty_ThrowArgumentException()
    {
        // Act
        Action action = () => _rng.OneOf<int>();

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void NextDouble_RngIsNull_ThrowArgumentNullException()
    {
        // Act 
        Action action = () => _rngNull.NextDouble(0, 100);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(0, 100, SampleCount)]
    public void NextDouble_WhenCalled_ReturnRandomValueInRange(double minimum, double maximum, int sampleCount)
    {
        // Arrange 
        var samples = new double[sampleCount];

        // Act 
        for (var i = 0; i < samples.Length; i++)
            samples[i] = _rng.NextDouble(minimum, maximum);

        // Assert
        samples.Should().OnlyContain(s => s >= minimum && s <= maximum);
    }

    [Test]
    public void ChanceGreaterThan_RngIsNull_ThrowArgumentNullException()
    {
        // Act 
        Action action = () => _rngNull.ChanceGreaterThan(50);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(-1)]
    [TestCase(101)]
    public void ChanceGreaterThan_PercentageOutOfRange_ThrowArgumentOutOfRangeException(double percentage)
    {
        // Act 
        Action action = () => _rng.ChanceGreaterThan(percentage);

        // Assert
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public void ChanceLowerThan_RngIsNull_ThrowArgumentNullException()
    {
        // Act 
        Action action = () => _rngNull.ChanceLowerThan(50);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(-1)]
    [TestCase(101)]
    public void ChanceLowerThan_PercentageOutOfRange_ThrowArgumentOutOfRangeException(double percentage)
    {
        // Act 
        Action action = () => _rng.ChanceLowerThan(percentage);

        // Assert
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public void ChanceBetween_RngIsNull_ThrowArgumentNullException()
    {
        // Act 
        Action action = () => _rngNull.ChanceBetween(25, 75);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(-1, 75)]
    [TestCase(25, 101)]
    public void ChanceLowerThan_EndsOutOfRange_ThrowArgumentOutOfRangeException(double percentageLeft, double percentageRight)
    {
        // Act 
        Action action = () => _rng.ChanceBetween(percentageLeft, percentageRight);

        // Assert
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    [TestCase(75, 25)]
    public void ChanceLowerThan_LeftGreaterThanRight_ThrowArgumentOutOfRangeException(double percentageLeft, double percentageRight)
    {
        // Act 
        Action action = () => _rng.ChanceBetween(percentageLeft, percentageRight);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void Shuffle_RngIsNull_ThrowArgumentNullException()
    {
        // Arrange
        var source = new List<int> { 1, 2, 3, 4, 5 };

        // Act 
        Action action = () => _rngNull.Shuffle(source);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Shuffle_SourceIsNull_ThrowArgumentNullException()
    {
        // Act 
        Action action = () => _rng.Shuffle<int>(null);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(SampleCount)]
    public void Shuffle_WhenCalled_ShufflesList(int sampleCount)
    {
        // Arrange
        var original = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var shuffled = original.ToList();

        // Act 
        for (var i = 0; i < sampleCount; i++)
            _rng.Shuffle(shuffled);

        // Assert  
        shuffled.Should().NotEqual(original);
        shuffled.Should().OnlyContain(i => original.Any(o => o.Equals(i)));
    }
}
