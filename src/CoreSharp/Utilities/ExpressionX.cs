using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="Expression"/> utilities.
/// </summary>
public static class ExpressionX
{
    /// <summary>
    /// Get <see cref="MemberInfo"/> from given <see cref="Expression{TDelegate}"/> selector.
    /// </summary>
    public static MemberInfo GetMemberInfo<TElement, TMember>(Expression<Func<TElement, TMember>> memberSelector)
    {
        _ = memberSelector ?? throw new ArgumentNullException(nameof(memberSelector));

        if (memberSelector.Body is MemberExpression memberExpression)
            return memberExpression.Member;
        else
            throw new ArgumentException($"{memberSelector} does not point to a valid member.", nameof(memberSelector));
    }
}
