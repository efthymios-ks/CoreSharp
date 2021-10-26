using System;
using System.Data;
using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class DataRowExtensionsTests
    {
        //Methods
        [Test]
        public void GetColumnNames_DataRowIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            DataRow row = null;

            //Act 
            Action action = () => row.GetColumnNames();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetColumnNames_WhenCalled_ReturnsColumnNames()
        {
            //Arrange 
            var columnNames = new[] { "Column 1", "Column 2", "Column3" };
            var table = new DataTable();
            table.Rows.Add();
            foreach (var name in columnNames)
                table.Columns.Add(name);
            var row = table.Rows[0];

            //Act 
            var result = row.GetColumnNames();

            //Assert
            result.Should().Equal(columnNames);
        }

        [Test]
        public void GetColumnValues_DataRowIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            DataRow row = null;

            //Act 
            Action action = () => row.GetColumnValues();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetColumnValues_WhenCalled_ReturnsColumnValues()
        {
            //Arrange  
            var columnValues = new object[] { "Efthymios", 26 };
            var table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Age", typeof(int));
            table.Rows.Add(columnValues);
            var row = table.Rows[0];

            //Act 
            var result = row.GetColumnValues();

            //Assert
            result.Should().Equal(columnValues);
        }

        [Test]
        public void ToEntity_DataRowIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            DataRow row = null;

            //Act 
            Action action = () => row.ToEntity<DummyClass>();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToEntity_IgnoreCaseIsFalse_MapCaseSensitiveColumnsToEntity()
        {
            //Arrange  
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("name", typeof(string));
            var row = table.Rows.Add(1, "Red");

            //Act 
            var result = row.ToEntity<DummyClass>();

            //Assert
            result.Id.Should().Be(1);
            result.Name.Should().NotBe("Red");
        }

        [Test]
        public void ToEntity_IgnoreCaseIsTrue_MapCaseInsensitiveColumnsToEntity()
        {
            //Arrange 
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("name", typeof(string));
            var row = table.Rows.Add(1, "Red");

            //Act 
            var result = row.ToEntity<DummyClass>(true);

            //Assert
            result.Id.Should().Be(1);
            result.Name.Should().Be("Red");
        }
    }
}
