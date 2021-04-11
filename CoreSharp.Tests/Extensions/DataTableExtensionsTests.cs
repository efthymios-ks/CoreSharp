using FluentAssertions;
using NUnit.Framework;
using System;
using System.Data;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class DataTableExtensionsTests
    {
        //Methods
        [Test]
        public void GetColumnNames_WhenDataTableIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            DataTable table = null;

            //Act 
            Action action = () => table.GetColumnNames();

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GetColumnNames_WhenCalled_ReturnsColumnNames()
        {
            //Arrange 
            var columnNames = new[] { "Column 1", "Column 2", "Column3" };
            var table = new DataTable();
            foreach (var name in columnNames)
                table.Columns.Add(name);

            //Act 
            var result = table.GetColumnNames();

            //Assert
            result.Should().Equal(columnNames);
        }
    }
}