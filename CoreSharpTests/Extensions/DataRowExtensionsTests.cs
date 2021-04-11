using NUnit.Framework;
using System;
using System.Data;
using FluentAssertions;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class DataRowExtensionsTests
    {
        [Test]
        public void GetColumnNames_WhenDataRowIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            DataRow row = null;

            //Act 
            Action action = () => DataRowExtensions.GetColumnNames(row);

            //Assert
            action.Should().Throw<ArgumentNullException>();
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
        public void GetColumnValues_WhenDataRowIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            DataRow row = null;

            //Act 
            Action action = () => DataRowExtensions.GetColumnValues(row);

            //Assert
            action.Should().Throw<ArgumentNullException>();
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
    }
}