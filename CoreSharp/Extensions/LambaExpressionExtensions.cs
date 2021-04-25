using System;
using System.Linq.Expressions;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// LambaExpression extensions. 
    /// </summary>
    public static partial class LambaExpressionExtensions
    {
        /// <summary>
        /// Get expression member name. 
        /// </summary> 
        public static string GetMemberName(this LambdaExpression memberExpression)
        {
            memberExpression = memberExpression ?? throw new ArgumentNullException(nameof(memberExpression));

            static string nameSelector(Expression e)
            {
                return e.NodeType switch
                {
                    ExpressionType.Parameter => (e as ParameterExpression)?.Name,
                    ExpressionType.MemberAccess => (e as MemberExpression)?.Member?.Name,
                    ExpressionType.Call => (e as MethodCallExpression)?.Method?.Name,
                    ExpressionType.Convert or ExpressionType.ConvertChecked => nameSelector((e as UnaryExpression)?.Operand),
                    ExpressionType.Invoke => nameSelector(((InvocationExpression)e).Expression),
                    ExpressionType.ArrayLength => "Length",
                    _ => throw new ArgumentException("Expression is not a proper member selector.", nameof(memberExpression)),
                };
            }

            return nameSelector(memberExpression.Body);
        }
    }
}
