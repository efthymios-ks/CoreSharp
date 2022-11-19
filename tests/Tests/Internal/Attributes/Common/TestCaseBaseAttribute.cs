using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Tests.Internal.Attributes.Common;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public abstract class TestCaseBaseAttribute : NUnitAttribute, ITestBuilder
{
    // Fields
    private readonly NUnitTestCaseBuilder _nunitTestCaseBuilder = new();

    // Methods
    public abstract IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite);

    /// <summary>
    /// Build runnable test case.
    /// </summary>
    protected TestMethod BuildRunMethod(IMethodInfo method, Test suite, params object[] arguments)
    {
        var methodArgumentCount = method.GetParameters().Length;
        if (methodArgumentCount != arguments.Length)
        {
            var errorMessage = $"The test method has {methodArgumentCount} parameter(s), but {arguments.Length} argument(s) are supplied.";
            return BuildSkipMethod(method, suite, errorMessage);
        }

        var parameters = new TestCaseParameters(arguments);
        return BuildTestMethod(method, suite, parameters);
    }

    /// <summary>
    /// Build test case to be skipped with provided error message.
    /// </summary>
    protected TestMethod BuildSkipMethod(IMethodInfo method, Test suite, string skipReason)
    {
        var parameters = new TestCaseParameters { RunState = RunState.NotRunnable };
        parameters.Properties.Set(PropertyNames.SkipReason, skipReason);
        return BuildTestMethod(method, suite, parameters);
    }

    private TestMethod BuildTestMethod(IMethodInfo method, Test suite, TestCaseParameters parameters)
        => _nunitTestCaseBuilder.BuildTestMethod(method, suite, parameters);
}