using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CoreSharp.Exceptions;

public class EntityNotFoundException : KeyNotFoundException
{
    // Constructors 
    public EntityNotFoundException(Type entityType, string propertyName, object propertyValue)
        : this(entityType.Name, propertyName, propertyValue)
    {
    }

    public EntityNotFoundException(string entityName, string propertyName, object propertyValue)
        : base($"{entityName} with {propertyName}=`{propertyValue}` not found.")
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
    public static EntityNotFoundException Create<TEntity, TKey>(Expression<Func<TEntity, TKey>> propertySelector, TKey targetValue)
    {
        _ = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));

        var entityType = typeof(TEntity);
        var propertyName = propertySelector.GetMemberName();

        return new EntityNotFoundException(entityType, propertyName, targetValue);
    }
}