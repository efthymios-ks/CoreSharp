using System;
using System.Linq.Expressions;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="LambdaExpression"/> extensions.
    /// </summary>
    public static class LambdaExpressionExtensions
    {
        /// <summary>
        /// Get expression member name.
        /// </summary>
        public static string GetMemberName(this LambdaExpression memberExpression)
        {
            _ = memberExpression ?? throw new ArgumentNullException(nameof(memberExpression));

            static string NameSelector(Expression expression)
            {
                return expression.NodeType switch
                {
                    ExpressionType.Parameter => (expression as ParameterExpression)?.Name,
                    ExpressionType.MemberAccess => (expression as MemberExpression)?.Member?.Name,
                    ExpressionType.Call => (expression as MethodCallExpression)?.Method?.Name,
                    ExpressionType.Convert or ExpressionType.ConvertChecked => NameSelector((expression as UnaryExpression)?.Operand),
                    ExpressionType.Invoke => NameSelector(((InvocationExpression)expression).Expression),
                    ExpressionType.ArrayLength => nameof(Array.Length),
                    _ => throw new ArgumentException("Expression is not a proper member selector.", nameof(memberExpression))
                };
            }

            return NameSelector(memberExpression.Body);
        }
    }
}
