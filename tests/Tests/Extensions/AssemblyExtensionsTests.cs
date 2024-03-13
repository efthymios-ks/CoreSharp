using System.Reflection;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public sealed class AssemblyExtensionsTests
{
    [Test]
    public void LoadReferencedAssemblies_ShouldReturnAllReferencedAssemblies()
    {
        // Arrange
        var executingAssembly = Assembly.GetExecutingAssembly();

        // Act
        var result = executingAssembly.LoadReferencedAssemblies();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain(assembly => assembly.FullName.StartsWith("CoreSharp", StringComparison.OrdinalIgnoreCase));
        result.Should().Contain(assembly => assembly.FullName.StartsWith("System.Runtime", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public void LoadReferencedAssemblies_WhenAssemblyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Assembly assembly = null;
        static bool Filter(AssemblyName _)
            => true;

        // Act
        Action action = () => assembly.LoadReferencedAssemblies(Filter);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void LoadReferencedAssemblies_WhenFilterIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        Func<AssemblyName, bool> filter = null;

        // Act
        Action action = () => assembly.LoadReferencedAssemblies(filter);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void LoadReferencedAssemblies_WhenNoAssemblyReferencesMatchFilter_ShouldReturnEmptyEnumerable()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        static bool Filter(AssemblyName _)
            => false;

        // Act
        var result = assembly.LoadReferencedAssemblies(Filter);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void LoadReferencedAssemblies_WhenSomeAssemblyReferencesMatchFilter_ShouldReturnMatchingAssemblies()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        static bool Filter(AssemblyName assemblyName)
            => assemblyName.Name.StartsWith("CoreSharp", StringComparison.OrdinalIgnoreCase);

        // Act
        var result = assembly.LoadReferencedAssemblies(Filter);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain(assembly => assembly.FullName.StartsWith("CoreSharp", StringComparison.OrdinalIgnoreCase));
    }
}