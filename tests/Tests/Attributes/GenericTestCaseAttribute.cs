using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Tests.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class GenericTestCaseAttribute : TestCaseAttribute, ITestBuilder
{
    // Constructors
    public GenericTestCaseAttribute(params object[] arguments)
        : base(arguments)
    {
    }

    // Properties
    public Type[] TypeArguments { get; set; }

    // Methods
    IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test suite)
    {
        if (!method.IsGenericMethodDefinition)
            return BuildFrom(method, suite);

        // If not all generic types are defined
        var genericArgumentsCount = method.GetGenericArguments().Length;
        if (TypeArguments?.Length != genericArgumentsCount)
        {
            var message = $"`{nameof(TypeArguments)}` should have {genericArgumentsCount} element(s).";
            var testParameters = new TestCaseParameters { RunState = RunState.NotRunnable };
            testParameters.Properties.Set("_SKIPREASON", message);
            var testMethod = new NUnitTestCaseBuilder().BuildTestMethod(method, suite, testParameters);
            return new[] { testMethod };
        }

        // Return property test method
        var genericMethod = method.MakeGenericMethod(TypeArguments);
        return BuildFrom(genericMethod, suite);
    }
}
