using CoreSharp.Extensions;
using System;
using System.Linq.Expressions;

namespace CoreSharp.Exceptions;

public class EntityExistsException : Exception
{
    // Constructors
    public EntityExistsException(Type entityType, string propertyName, object propertyValue)
        : this(entityType.Name, propertyName, propertyValue)
    {
    }

    public EntityExistsException(string entityName, string propertyName, object propertyValue)
        : base($"{entityName} with {propertyName}=`{propertyValue}` already exists.")
    {
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
        _ = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));

        var entityType = typeof(TEntity);
        var propertyName = propertySelector.GetMemberName();

        return new EntityExistsException(entityType, propertyName, propertyValue);
    }
}
