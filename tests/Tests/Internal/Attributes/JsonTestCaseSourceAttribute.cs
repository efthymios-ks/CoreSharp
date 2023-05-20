using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tests.Internal.Attributes.Common;

namespace Tests.Internal.Attributes;

/// <summary>
/// Uses json array as test case source. <br/>
/// Maps each entry to dto. <br/>
/// Dto must be record class. <br/>
/// <example>
/// <code>    
/// [TestFixture]
/// public class LoginTest 
/// {
///     // Methods 
///     [Test]
///     [JsonTestCaseSource("TestSources/LoginTest.Login")]
///     public void LoginWithDto(LoginDto login)
///     {
///     }
/// 
///     // Nested 
///     public record class LoginDto
///     {
///         // Properties
///         public Guid Id { get; set; }
///         public string Username { get; set; }
///         public string Password { get; set; }
///         public int Age { get; set; }
///     }
/// }
/// </code>
/// </example>
/// </summary>
public sealed class JsonTestCaseSourceAttribute : TestCaseBaseAttribute
{
    // Fields 
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        ReadCommentHandling = JsonCommentHandling.Skip
    };
    private readonly string _fileName;

    // Constructors 
    public JsonTestCaseSourceAttribute(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        _fileName = fileName;
    }

    // Methods  
    public override IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
    {
        var fileName = GetJsonFileName();

        if (!File.Exists(fileName))
        {
            var errorMessage = $"`{fileName}` was not found.";
            yield return BuildSkipMethod(method, suite, errorMessage);
            yield break;
        }

        var methodArguments = method.GetParameters();
        if (methodArguments.Length != 1)
        {
            var errorMessage = "The test method must be provided exactly 1 parameter.";
            yield return BuildSkipMethod(method, suite, errorMessage);
            yield break;
        }

        var entityType = GetEntityType(method);
        var items = GetItemsFromJson(fileName, entityType);
        foreach (var item in items)
        {
            yield return BuildRunMethod(method, suite, item);
        }
    }

    private string GetJsonFileName()
    {
        if (_fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            return _fileName;
        }
        else
        {
            return $"{_fileName}.json";
        }
    }

    private static Type GetEntityType(IMethodInfo method)
    {
        var methodArguments = method.GetParameters();
        return methodArguments.FirstOrDefault()?.ParameterType;
    }

    private object[] GetItemsFromJson(string fileName, Type entityType)
    {
        entityType = entityType.MakeArrayType();
        var jsonAsString = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize(jsonAsString, entityType, _jsonSerializerOptions) as object[];
    }
}

