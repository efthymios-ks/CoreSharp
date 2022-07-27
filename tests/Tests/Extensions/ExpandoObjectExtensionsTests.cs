using System.Dynamic;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class ExpandoObjectExtensionsTests
{
    [Test]
    public void ToDictionary_InputIsNull_ThrowNewArgumentNullException()
    {
        //Arrange 
        ExpandoObject expando = null;

        //Act 
        Action action = () => expando.ToDictionary();

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void ToDictionary_WhenCalled_ReturnDictionary()
    {
        //Arrange 
        var source = new Dictionary<string, object>
        {
            { "id", 1 },
            { "color", "black" }
        };
        IDictionary<string, object> expando = new ExpandoObject();
        foreach (var item in source)
            expando.Add(item);

        //Act 
        var dictionary = ((ExpandoObject)expando).ToDictionary();

        //Assert
        dictionary.Should().Equal(source);
    }
}
