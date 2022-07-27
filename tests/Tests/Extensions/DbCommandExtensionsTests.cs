namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class DbCommandExtensionsTests
{
    //Fields
    private readonly DbCommand _commandNull;
    private SqlCommand _sqlCommand;

    //Methods
    [SetUp]
    public void SetUp()
        => _sqlCommand = new();

    [TearDown]
    public void TearDown()
    {
        _sqlCommand?.Dispose();
        _sqlCommand = null;
    }

    [Test]
    public void CreateParameter_CommandIsNull_ThrowArgumentNullException()
    {
        //Act 
        Action action = () => _commandNull.CreateParameter("{name}", "Efthymios");

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void CreateParameter_WhenCalled_ReturnSameTypeDbParameterWithValues()
    {
        //Arrang 
        const string name = "{name}";
        const string value = "Efthymios";

        //Act 
        var result = _sqlCommand.CreateParameter(name, value);

        //Assert
        result.Should().BeOfType<SqlParameter>();
        result.ParameterName.Should().Be(name);
        result.Value.Should().Be(value);
    }

    [Test]
    public void AddParameter_CommandIsNull_ThrowArgumentNullException()
    {
        //Act 
        Action action = () => _commandNull.AddParameterWithValue("{name}", "Efthymios");

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void AddParameter_WhenCalled_AddsAndReturnSameTypeDbParameterWithValues()
    {
        //Arrange 
        const string name = "{name}";
        const string value = "Efthymios";

        //Act 
        var result = _sqlCommand.AddParameterWithValue(name, value);

        //Assert
        result.Should().BeOfType<SqlParameter>();
        result.ParameterName.Should().Be(name);
        result.Value.Should().Be(value);
        _sqlCommand.Parameters.Contains(result).Should().BeTrue();
    }
}
