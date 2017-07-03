using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NetSteps.Common.Utility
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Taken from http://www.albahari.com/nutshell/predicatebuilder.aspx
    /// Created: 07-06-2010
    /// </summary>
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        // Added these methods for T to be implicitly discovered by the compiler to make it work more easily with anonymous types. - JHE
        public static Expression<Func<T, bool>> True<T>(IEnumerable<T> items) { return f => true; }
        public static Expression<Func<T, bool>> False<T>(IEnumerable<T> items) { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}