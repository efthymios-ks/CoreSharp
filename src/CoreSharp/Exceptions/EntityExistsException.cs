using CoreSharp.Extensions;
using System;
using System.Linq.Expressions;

namespace CoreSharp.Exceptions
{
    public class EntityExistsException : Exception
    {
        //Constructors
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

        //Properties 
        public string EntityName { get; }
        public string PropertyName { get; }
        public object PropertyValue { get; }

        //Methods 
        public static EntityExistsException Create<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, TProperty propertyValue)
        {
            _ = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));

            var entityType = typeof(TEntity);
            var propertyName = propertySelector.GetMemberName();

            return new EntityExistsException(entityType, propertyName, propertyValue);
        }

        public static void Throw<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, TProperty propertyValue)
            => throw Create(propertySelector, propertyValue);

        /// <summary>
        /// Throw new <see cref="EntityExistsException"/>
        /// when provided entity property matches with target value.
        /// </summary>
        public static void ThrowIf<TEntity, TKey>(TEntity entity, Expression<Func<TEntity, TKey>> propertySelector, TKey targetValue)
        {
            _ = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));

            //If entity is null, do not throw.
            if (entity is null)
                return;

            //If values differ, do not throw.
            var actualValue = propertySelector.Compile().Invoke(entity);
            if (!Equals(actualValue, targetValue))
                return;

            //Else throw
            Throw(propertySelector, targetValue);
        }
    }
}
