using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Data;
using System.Linq;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class DataTableExtensionsTests
    {
        //Methods
        [Test]
        public void GetColumnNames_DataTableIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            DataTable table = null;

            //Act 
            Action action = () => table.GetColumnNames();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetColumnNames_WhenCalled_ReturnsColumnNames()
        {
            //Arrange 
            var table = new DataTable();
            var columnNames = new[] { "Column 1", "Column 2", "Column3" };
            foreach (var name in columnNames)
                table.Columns.Add(name);

            //Act 
            var result = table.GetColumnNames();

            //Assert
            result.Should().Equal(columnNames);
        }

        [Test]
        public void ToEntities_DataTableIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            DataTable table = null;

            //Act 
            Action action = () => table.ToEntities<DummyClass>();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToEntities_IgnoreCaseIsFalse_MapCaseSensitiveColumnsToEntities()
        {
            //Arrange  
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("name", typeof(string));
            table.Rows.Add(1, "Red");
            table.Rows.Add(2, "Black");

            //Act 
            var result = table.ToEntities<DummyClass>().ToArray(); 
            var item1 = result[0];
            var item2 = result[1];

            //Assert
            result.Should().HaveCount(2);
            item1.Id.Should().Be(1);
            item1.Name.Should().NotBe("Red");
            item2.Id.Should().Be(2);
            item2.Name.Should().NotBe("Black");
        }

        [Test]
        public void ToEntities_IgnoreCaseIsTrue_MapCaseInsensitiveColumnsToEntity()
        {
            //Arrange  
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("name", typeof(string));
            table.Rows.Add(1, "Red");
            table.Rows.Add(2, "Black");

            //Act 
            var result = table.ToEntities<DummyClass>(true).ToArray(); 
            var item1 = result[0];
            var item2 = result[1];

            //Assert
            result.Should().HaveCount(2);
            item1.Id.Should().Be(1);
            item1.Name.Should().Be("Red");
            item2.Id.Should().Be(2);
            item2.Name.Should().Be("Black");
        }
    }
}
