﻿using FluentAssertions;
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
        private readonly DbProviderFactory _factoryNull = null;
        private DbProviderFactory _sqlFactory;

        //Methods 
        [SetUp]
        public void SetUp()
        {
            using (var connection = new SqlConnection())
                _sqlFactory = DbProviderFactories.GetFactory(connection);
        }

        [Test]
        public void CreateParameter_FactoryIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => _factoryNull.CreateParameter("{name}", "Efthymios");

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
            var result = _sqlFactory.CreateParameter(name, value);

            //Assert
            result.Should().BeOfType<SqlParameter>();
            result.ParameterName.Should().Be(name);
            result.Value.Should().Be(value);
        }
    }
}