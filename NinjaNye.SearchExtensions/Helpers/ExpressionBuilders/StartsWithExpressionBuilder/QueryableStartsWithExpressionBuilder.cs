using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.StartsWithExpressionBuilder
{
    internal static class QueryableStartsWithExpressionBuilder
    {
        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms)
        {
            Expression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var startsWithExpression = Build(stringProperty, searchTerm);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression Build<T>(Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            var searchTermExpression = Expression.Constant(searchTerm);
            var indexOfCallExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.IndexOfMethod, searchTermExpression);
            return Expression.Equal(indexOfCallExpresion, ExpressionMethods.ZeroConstantExpression);
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>> stringProperty, params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var startsWithExpression = Build(stringProperty, propertyToSearchFor);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>> propertyToSearchFor)
        {
            var indexOfCallExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.IndexOfMethod, propertyToSearchFor.Body);
            return Expression.Equal(indexOfCallExpresion, ExpressionMethods.ZeroConstantExpression);
        }
    }
}