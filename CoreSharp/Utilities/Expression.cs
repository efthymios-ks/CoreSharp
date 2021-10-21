using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="System.Linq.Expressions.Expression"/> utilities.
    /// </summary>
    public static class Expression
    {
        /// <summary>
        /// Get <see cref="MemberInfo"/> from given <see cref="Expression{TDelegate}"/> selector.
        /// </summary>
        public static MemberInfo GetMemberInfo<TItem, TMember>(Expression<Func<TItem, TMember>> memberSelector)
        {
            _ = memberSelector ?? throw new ArgumentNullException(nameof(memberSelector));

            var memberExpression = memberSelector.Body as MemberExpression;
            if (memberExpression is not null)
                return memberExpression.Member;
            else
                throw new ArgumentException($"{memberSelector} does not point to a valid member.", nameof(memberSelector));
        }
    }
}
