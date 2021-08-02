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
        private readonly DbConnection connectionNull = null;
        private DbConnection sqlConnection;

        //Methods
        [SetUp]
        public void SetUp()
        {
            sqlConnection = new SqlConnection();
        }

        [TearDown]
        public void TearDown()
        {
            sqlConnection?.Dispose();
            sqlConnection = null;
        }

        [Test]
        public void CreateDataAdapter_ConnectionIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => connectionNull.CreateDataAdapter();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void CreateDataAdapter_WhenCalled_ReturnSameTypeDbAdapter()
        {
            //Act
            var result = sqlConnection.CreateDataAdapter();

            //Assert
            result.Should().BeOfType<SqlDataAdapter>();
        }

        [Test]
        public void CreateParameter_ConnectionIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => connectionNull.CreateParameter("{name}", "Efthymios");

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
            var result = sqlConnection.CreateParameter(name, value);

            //Assert
            result.Should().BeOfType<SqlParameter>();
            result.ParameterName.Should().Be(name);
            result.Value.Should().Be(value);
        }

        [Test]
        public void OpenTransaction_ConnectionIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => connectionNull.OpenTransaction();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void IsOpen_ConnectionIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => connectionNull.IsOpen();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void IsAvailable_ConnectionIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => connectionNull.IsOpen();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}