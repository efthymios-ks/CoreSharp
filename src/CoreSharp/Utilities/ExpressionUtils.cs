using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="Expression"/> utilities.
/// </summary>
public static class ExpressionUtils
{
    /// <summary>
    /// Get <see cref="MemberInfo"/> from given <see cref="Expression{TDelegate}"/> selector.
    /// </summary>
    public static MemberInfo GetMemberInfo<TElement, TMember>(Expression<Func<TElement, TMember>> memberSelector)
    {
        ArgumentNullException.ThrowIfNull(memberSelector);

        if (memberSelector.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member;
        }

        throw new ArgumentException($"{memberSelector} does not point to a valid member.", nameof(memberSelector));

    }
}
