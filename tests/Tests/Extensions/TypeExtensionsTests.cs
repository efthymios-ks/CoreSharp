namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class TypeExtensionsTests
{
    [Test]
    public void IsNumeric_TypeIsNull_ThrowArgumentNullException()
    {
        // Arrange
        Type typeNull = null;

        // Act
        Action action = () => typeNull.IsNumeric();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(typeof(byte), true)]
    [TestCase(typeof(byte?), true)]
    [TestCase(typeof(sbyte), true)]
    [TestCase(typeof(sbyte?), true)]
    [TestCase(typeof(int), true)]
    [TestCase(typeof(int?), true)]
    [TestCase(typeof(long), true)]
    [TestCase(typeof(long?), true)]
    [TestCase(typeof(float), true)]
    [TestCase(typeof(float?), true)]
    [TestCase(typeof(double), true)]
    [TestCase(typeof(double?), true)]
    [TestCase(typeof(decimal), true)]
    [TestCase(typeof(decimal?), true)]
    [TestCase(typeof(string), false)]
    public void IsNumeric_TypeIsNumeric_ReturnTrue(Type type, bool expectedIsNumeric)
    {
        // Act
        var isNumeric = type.IsNumeric();

        // Assert
        isNumeric.Should().Be(expectedIsNumeric);
    }

    [Test]
    public void IsDate_TypeIsNull_ThrowArgumentNullException()
    {
        // Arrange
        Type typeNull = null;

        // Act
        Action action = () => typeNull.IsDate();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(typeof(DateTime), true)]
    [TestCase(typeof(DateTime?), true)]
    [TestCase(typeof(DateTimeOffset), true)]
    [TestCase(typeof(DateTimeOffset?), true)]
    [TestCase(typeof(string), false)]
    public void IsDate_TypeIsDate_ReturnTrue(Type type, bool expectedIsDate)
    {
        // Act
        var isDate = type.IsDate();

        // Assert
        isDate.Should().Be(expectedIsDate);
    }

    [Test]
    [TestCase(typeof(int?), typeof(int))]
    [TestCase(typeof(int), typeof(int))]
    public void GetNullableBaseType_WhenCalled_ReturnBaseType(Type inputType, Type expectedBaseType)
    {
        // Act
        var baseType = inputType.GetNullableBaseType();

        // Assert
        baseType.Should().Be(expectedBaseType);
    }

    [Test]
    public void GetGenericTypeBase_TypeIsNull_ThrowArgumentNullException()
    {
        // Arrange
        Type type = null;

        // Act
        Action action = () => type.GetGenericTypeBase();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetDefault_TypeIsNull_ThrowArgumentNullException()
    {
        // Arrange
        Type type = null;

        // Act
        Action action = () => type.GetDefault();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(typeof(DummyClass), typeof(DummyClass))]
    [TestCase(typeof(DummyClass<int>), typeof(DummyClass<>))]
    public void GetGenericTypeBase_WhenCalled_ReturnGenericBaseType(Type inputType, Type expectedBaseType)
    {
        // Act
        var baseType = inputType.GetGenericTypeBase();

        // Assert
        baseType.Should().Be(expectedBaseType);
    }

    [Test]
    [TestCase(null, typeof(int))]
    [TestCase(typeof(int), null)]
    public void Implements_ArgIsNull_ThrowArgumentNullException(Type parentType, Type baseType)
    {
        // Act
        Action action = () => parentType.Implements(baseType);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(typeof(DummyClass<>), typeof(int), false, false)]
    [TestCase(typeof(DummyClass<>), typeof(DummyClass), false, true)]
    [TestCase(typeof(DummyClass<>), typeof(DummyClass<>), false, true)]
    [TestCase(typeof(DummyClass<>), typeof(DummyClass<int>), false, false)]
    [TestCase(typeof(DummyClass<>), typeof(DummyClass<int>), true, true)]
    public void Implements_WhenCalled_ReturnGenericBaseType(Type parentType, Type baseType, bool useGenericBaseType, bool expectedImplements)
    {
        // Act
        var implements = parentType.Implements(baseType, useGenericBaseType);

        // Assert
        implements.Should().Be(expectedImplements);
    }
}
