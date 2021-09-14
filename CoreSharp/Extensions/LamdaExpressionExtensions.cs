using System;
using System.Linq.Expressions;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// LambdaExpression extensions.
    /// </summary>
    public static class LambdaExpressionExtensions
    {
        /// <summary>
        /// Get expression member name.
        /// </summary>
        public static string GetMemberName(this LambdaExpression memberExpression)
        {
            _ = memberExpression ?? throw new ArgumentNullException(nameof(memberExpression));

            static string NameSelector(Expression e)
            {
                return e.NodeType switch
                {
                    ExpressionType.Parameter => (e as ParameterExpression)?.Name,
                    ExpressionType.MemberAccess => (e as MemberExpression)?.Member?.Name,
                    ExpressionType.Call => (e as MethodCallExpression)?.Method?.Name,
                    ExpressionType.Convert or ExpressionType.ConvertChecked => NameSelector((e as UnaryExpression)?.Operand),
                    ExpressionType.Invoke => NameSelector(((InvocationExpression)e).Expression),
                    ExpressionType.ArrayLength => "Length",
                    _ => throw new ArgumentException("Expression is not a proper member selector.", nameof(memberExpression)),
                };
            }

            return NameSelector(memberExpression.Body);
        }
    }
}
