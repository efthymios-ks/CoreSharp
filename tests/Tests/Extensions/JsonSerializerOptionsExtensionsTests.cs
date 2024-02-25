using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class JsonSerializerOptionsExtensionsTests
{
    // Methods
    [Test]
    public void MapTo_WhenFromIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        JsonSerializerOptions from = null;
        var to = new JsonSerializerOptions();

        // Act
        Action action = () => from.MapTo(to);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void MapTo_WhenToIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var from = new JsonSerializerOptions();
        JsonSerializerOptions to = null;

        // Act
        Action action = () => from.MapTo(to);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void MapTo_WhenCalled_ShouldOverrideOptions()
    {
        // Arrange
        var from = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.Strict,
            WriteIndented = true
        };
        var to = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            WriteIndented = false
        };

        // Act
        from.MapTo(to);

        // Assert
        to.NumberHandling.Should().Be(JsonNumberHandling.Strict);
        to.WriteIndented.Should().BeTrue();
    }

    [Test]
    public void MapTo_WhenCalled_ShouldCopyConverters()
    {
        // Arrange
        var converter1 = new DummyJsonConverter();
        var converter2 = new DummyJsonConverter();
        var from = new JsonSerializerOptions();
        from.Converters.Add(converter1);
        from.Converters.Add(converter2);

        var to = new JsonSerializerOptions();

        // Act
        from.MapTo(to);

        // Assert
        to.Converters.Should().ContainInOrder(converter1, converter2);
    }

    // Nested
    private sealed class DummyJsonConverter : JsonConverter<object>
    {
        // Methods
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
            => throw new NotImplementedException();
    }
}