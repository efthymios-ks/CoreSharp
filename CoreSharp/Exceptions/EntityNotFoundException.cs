using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CoreSharp.Exceptions
{
    //TODO: Add unit tests 
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
        public static EntityNotFoundException Create<TEntity, TKey>(Expression<Func<TEntity, TKey>> keySelector, TKey keyValue)
        {
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var entityType = typeof(TEntity);
            var memberName = keySelector.GetMemberName();

            return new EntityNotFoundException(entityType, memberName, keyValue);
        }

        public static void Throw<TEntity, TKey>(Expression<Func<TEntity, TKey>> keySelector, TKey keyValue)
            => throw Create(keySelector, keyValue);
    }
}
