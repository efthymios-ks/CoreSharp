using FluentAssertions;
using NUnit.Framework;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class DbCommandExtensionsTests
    {
        //Fields
        private readonly DbCommand _commandNull = null;
        private SqlCommand _sqlCommand;

        //Methods
        [SetUp]
        public void SetUp()
        {
            _sqlCommand = new SqlCommand();
        }

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
            string name = "{name}";
            string value = "Efthymios";

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
            //Arrang 
            string name = "{name}";
            string value = "Efthymios";

            //Act 
            var result = _sqlCommand.AddParameterWithValue(name, value);

            //Assert
            result.Should().BeOfType<SqlParameter>();
            result.ParameterName.Should().Be(name);
            result.Value.Should().Be(value);
            _sqlCommand.Parameters.Should().Contain(result);
        }
    }
}