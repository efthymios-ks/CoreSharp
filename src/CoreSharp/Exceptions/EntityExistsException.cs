using CoreSharp.Extensions;
using System;
using System.Linq.Expressions;

namespace CoreSharp.Exceptions;

public class EntityExistsException : Exception
{
    // Constructors
    public EntityExistsException(Type entityType, string propertyName, object propertyValue)
        : this(entityType?.Name, propertyName, propertyValue)
    {
    }

    public EntityExistsException(string entityName, string propertyName, object propertyValue)
        : base($"{entityName} with {propertyName}=`{propertyValue}` already exists.")
    {
        if (string.IsNullOrWhiteSpace(entityName))
        {
            throw new ArgumentException("Can't be empty.", nameof(entityName));
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("Can't be empty.", nameof(propertyName));
        }

        EntityName = entityName;
        PropertyName = propertyName;
        PropertyValue = propertyValue;
    }

    // Properties 
    public string EntityName { get; }
    public string PropertyName { get; }
    public object PropertyValue { get; }

    // Methods 
    public static EntityExistsException Create<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, TProperty propertyValue)
    {
        ArgumentNullException.ThrowIfNull(propertySelector);

        var entityType = typeof(TEntity);
        var propertyName = propertySelector.GetMemberName();

        return new EntityExistsException(entityType, propertyName, propertyValue);
    }
}
