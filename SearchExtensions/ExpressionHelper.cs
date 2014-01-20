using System;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    internal static class ExpressionHelper
    {
        /// <summary>
        /// Join two expressions using the conditional OR operation
        /// </summary>
        /// <param name="existingExpression">Expression being joined</param>
        /// <param name="expressionToJoin">Expression to append</param>
        /// <returns>New expression containing both expressions joined using the conditional OR operation</returns>
        public static Expression JoinOrExpression(Expression existingExpression, Expression expressionToJoin)
        {
            if (existingExpression == null)
            {
                return expressionToJoin;
            }

            return Expression.OrElse(existingExpression, expressionToJoin);
        }

        /// <summary>
        /// Build a 'contains' expression for a search term against a particular string property
        /// </summary>
        public static MethodCallExpression BuildContainsExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression)
        {
            ConstantExpression emptyString = Expression.Constant(string.Empty);
            var coalesceExpression = Expression.Coalesce(stringProperty.Body, emptyString);
            return Expression.Call(coalesceExpression, typeof(string).GetMethod("Contains"), searchTermExpression);
        }

        /// <summary>
        /// Build a 'indexof() >= 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildIndexOfExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression, StringComparison stringComparison)
        {
            ConstantExpression emptyString = Expression.Constant(string.Empty);
            var coalesceExpression = Expression.Coalesce(stringProperty.Body, emptyString);
            var indexOfMethod = typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) });
            var zeroConstantExpression = Expression.Constant(0);
            var stringComparisonExpression = Expression.Constant(stringComparison);
            var indexOfCallExpresion = Expression.Call(coalesceExpression, indexOfMethod, searchTermExpression, stringComparisonExpression);
            return Expression.GreaterThanOrEqual(indexOfCallExpresion, zeroConstantExpression);
        }

    }
}