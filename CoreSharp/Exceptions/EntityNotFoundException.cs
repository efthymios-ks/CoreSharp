using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CoreSharp.Exceptions
{
    public class EntityNotFoundException : KeyNotFoundException
    {
        //Constructors
        public EntityNotFoundException(Type entityType, string keyName, object keyValue)
            : this(entityType.Name, keyName, keyValue)
        {
        }

        public EntityNotFoundException(string entityName, string keyName, object keyValue)
            : this($"{entityName} with {keyName}=`{keyValue}` not found.")
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        //Methods 
        public static void Throw<TEntity, TKey>(Expression<Func<TEntity, TKey>> keySelector, TKey keyValue)
        {
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var entityType = typeof(TEntity);
            var memberName = keySelector.GetMemberName();

            throw new EntityNotFoundException(entityType, memberName, keyValue);
        }
    }
}
