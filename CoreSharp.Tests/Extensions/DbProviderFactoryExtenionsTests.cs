using FluentAssertions;
using NUnit.Framework;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class DbProviderFactoryExtenionsTests
    {
        //Fields
        private const string SqlProviderName = "Microsoft.Data.SqlClient";
        private readonly DbProviderFactory factoryNull = null;
        private DbProviderFactory sqlFactory;

        //Methods 
        [SetUp]
        public void SetUp()
        {
            using (var connection = new SqlConnection())
                sqlFactory = DbProviderFactories.GetFactory(connection);
        }

        [Test]
        public void CreateParameter_FactoryIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => factoryNull.CreateParameter("{name}", "Efthymios");

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
            var result = sqlFactory.CreateParameter(name, value);

            //Assert
            result.Should().BeOfType<SqlParameter>();
            result.ParameterName.Should().Be(name);
            result.Value.Should().Be(value);
        }
    }
}