﻿using System.Linq.Expressions;

namespace CoreSharp.Exceptions.Tests;

[TestFixture]
public sealed class EntityExistsExceptionTests
{
    [Test]
    public void Constructor_WhenEntityTypeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type entityType = null;
        var propertyName = "PropertyName";
        var propertyValue = "PropertyValue";

        // Act
        Action action = () => _ = new EntityExistsException(entityType, propertyName, propertyValue);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void Constructor_WhenEntityNameNameIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        string entityName = null;
        var propertyName = nameof(TestEntity.Name);
        var propertyValue = "PropertyValue";

        // Act
        Action action = () => _ = new EntityExistsException(entityName, propertyName, propertyValue);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void Constructor_WhenPropertyNameIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        var entityName = nameof(TestEntity);
        string propertyName = null;
        var propertyValue = "PropertyValue";

        // Act
        Action action = () => _ = new EntityExistsException(entityName, propertyName, propertyValue);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void Constructor_WhenPropertyValueIsNull_ShouldNotThrowException()
    {
        // Arrange
        var entityType = typeof(TestEntity);
        var propertyName = "PropertyName";
        object propertyValue = null;

        // Act
        Action action = () => _ = new EntityExistsException(entityType, propertyName, propertyValue);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    public void Constructor_WhenArgumentsAreValid_ShouldSetPropertiesAndMessage()
    {
        // Arrange
        var entityType = typeof(TestEntity);
        var propertyName = "PropertyName";
        object propertyValue = "PropertyValue";

        // Act
        var exception = new EntityExistsException(entityType, propertyName, propertyValue);

        // Assert
        exception.EntityName.Should().Be(entityType.Name);
        exception.PropertyName.Should().Be(propertyName);
        exception.PropertyValue.Should().Be(propertyValue);
        exception.Message.Should().Be($"{entityType.Name} with {propertyName}=`{propertyValue}` already exists.");
    }

    [Test]
    public void Create_WhenPropertySelectorIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Expression<Func<TestEntity, string>> propertySelector = null;
        var propertyValue = "PropertyValue";

        // Act
        Action action = () => EntityExistsException.Create(propertySelector, propertyValue);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Create_WhenArgumentsAreValid_ShouldReturnEntityExistsException()
    {
        // Arrange
        Expression<Func<TestEntity, string>> propertySelector = e => e.Name;
        var propertyValue = "PropertyValue";

        // Act
        var exception = EntityExistsException.Create(propertySelector, propertyValue);

        // Assert
        exception.Should().BeOfType<EntityExistsException>();
        exception.EntityName.Should().Be(nameof(TestEntity));
        exception.PropertyName.Should().Be(nameof(TestEntity.Name));
        exception.PropertyValue.Should().Be(propertyValue);
        exception.Message.Should().Be($"{nameof(TestEntity)} with {nameof(TestEntity.Name)}=`{propertyValue}` already exists.");
    }

    private sealed class TestEntity
    {
        public string Name { get; set; }
    }
}
