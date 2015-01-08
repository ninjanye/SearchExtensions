using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.Enumerable
{
    internal static class StartsWithExpressionBuilder
    {
        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression Build<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            BinaryExpression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var startsWithExpression = Build(stringProperty, searchTerm, stringComparisonExpression, nullCheck);
                completeExpression = completeExpression == null ? startsWithExpression
                                         : Expression.OrElse(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression Build<T>(Expression<Func<T, string>> stringProperty, string searchTerm, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            var searchTermExpression = Expression.Constant(searchTerm);
            if (nullCheck)
            {
                var coalesceExpression = Expression.Coalesce(stringProperty.Body, ExpressionMethods.EmptyStringExpression);
                var nullCheckExpresion = Expression.Call(coalesceExpression, ExpressionMethods.IndexOfMethod, searchTermExpression, stringComparisonExpression);
                return Expression.Equal(nullCheckExpresion, ExpressionMethods.ZeroConstantExpression);
            }

            var indexOfCallExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.IndexOfMethod, searchTermExpression, stringComparisonExpression);
            return Expression.Equal(indexOfCallExpresion, ExpressionMethods.ZeroConstantExpression);            
        }

        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, IEnumerable<Expression<Func<T, string>>> propertiesToSearchFor, ConstantExpression stringComparisonExpression)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var startsWithExpression = Build(stringProperty, propertyToSearchFor, stringComparisonExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>> propertyToSearchFor, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            var indexOfCallExpression = Expression.Call(stringProperty.Body, ExpressionMethods.IndexOfMethod, propertyToSearchFor.Body, stringComparisonExpression);
            var indexOfIsZeroExpression = Expression.Equal(indexOfCallExpression, ExpressionMethods.ZeroConstantExpression);
            if (nullCheck)
            {
                var stringPropertyNotNullExpression = Expression.NotEqual(stringProperty.Body, ExpressionMethods.NullExpression);
                var searchForNotNullExpression = Expression.NotEqual(propertyToSearchFor.Body, ExpressionMethods.NullExpression);
                var propertiesNotNullExpression = Expression.AndAlso(stringPropertyNotNullExpression, searchForNotNullExpression);
                return Expression.AndAlso(propertiesNotNullExpression, indexOfIsZeroExpression);
            }
            return indexOfIsZeroExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, string[] searchTerms, StringComparison comparisonType)
        {
            Expression completeExpression = null;
            var comparisonTypeExpression = Expression.Constant(comparisonType);
            foreach (var stringProperty in stringProperties)
            {
                var startsWithExpression = Build(stringProperty, searchTerms, comparisonTypeExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, Expression<Func<T, string>>[] propertiesToSearchFor, StringComparison comparisonType)
        {
            Expression completeExpression = null;
            var comparisonTypeExpression = Expression.Constant(comparisonType);
            foreach (var stringProperty in stringProperties)
            {
                var startsWithExpression = Build(stringProperty, propertiesToSearchFor, comparisonTypeExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }
    }
}