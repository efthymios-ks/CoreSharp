using FluentAssertions;
using NUnit.Framework;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class DbConnectionExtensionsTests
    {
        //Fields
        private readonly DbConnection _connectionNull = null;
        private DbConnection _sqlConnection;

        //Methods
        [SetUp]
        public void SetUp()
        {
            _sqlConnection = new SqlConnection();
        }

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
            string name = "{name}";
            string value = "Efthymios";

            //Act
            var result = _sqlConnection.CreateParameter(name, value);

            //Assert
            result.Should().BeOfType<SqlParameter>();
            result.ParameterName.Should().Be(name);
            result.Value.Should().Be(value);
        }

        [Test]
        public void OpenTransaction_ConnectionIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => _connectionNull.OpenTransaction();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
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
            Action action = () => _connectionNull.IsOpen();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}