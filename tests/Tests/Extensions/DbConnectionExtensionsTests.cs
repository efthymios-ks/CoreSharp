using FluentAssertions;
using NUnit.Framework;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class DbConnectionExtensionsTests
{
    //Fields
    private readonly DbConnection _connectionNull;
    private DbConnection _sqlConnection;

    //Methods
    [SetUp]
    public void SetUp()
        => _sqlConnection = new SqlConnection();

    [TearDown]
    public void TearDown()
    {
        _sqlConnection?.Dispose();
        _sqlConnection = null;
    }

    [Test]
    public void CreateDataAdapter_ConnectionIsNull_ThrowArgumentNullException()
    {
        //Act
        Action action = () => _connectionNull.CreateDataAdapter();

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void CreateDataAdapter_WhenCalled_ReturnSameTypeDbAdapter()
    {
        //Act
        var result = _sqlConnection.CreateDataAdapter();

        //Assert
        result.Should().BeOfType<SqlDataAdapter>();
    }

    [Test]
    public void CreateParameter_ConnectionIsNull_ThrowArgumentNullException()
    {
        //Act
        Action action = () => _connectionNull.CreateParameter("{name}", "Efthymios");

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void CreateParameter_WhenCalled_ReturnSameTypeDbParameterWithValues()
    {
        //Arrange
        const string name = "{name}";
        const string value = "Efthymios";

        //Act
        var result = _sqlConnection.CreateParameter(name, value);

        //Assert
        result.Should().BeOfType<SqlParameter>();
        result.ParameterName.Should().Be(name);
        result.Value.Should().Be(value);
    }

    [Test]
    public void IsOpen_ConnectionIsNull_ThrowArgumentNullException()
    {
        //Act
        Action action = () => _connectionNull.IsOpen();

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void IsAvailable_ConnectionIsNull_ThrowArgumentNullException()
    {
        //Act
        Func<Task<bool>> action = () => _connectionNull.IsAvailableAsync();

        //Assert
        action.Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
