using System;
using System.Linq;
using System.Linq.Expressions;

namespace WLX.Utils.WebUtils
{
    /// <summary>
    /// 描述：Expression扩展 and 和 or 方法
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// 函数应用True时：单个AND有效，多个AND有效；单个OR无效，多个OR无效；混应时写在AND后的OR有效 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// 函数应用False时：单个AND无效，多个AND无效；单个OR有效，多个OR有效；混应时写在OR后面的AND有效 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
            (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
            (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
