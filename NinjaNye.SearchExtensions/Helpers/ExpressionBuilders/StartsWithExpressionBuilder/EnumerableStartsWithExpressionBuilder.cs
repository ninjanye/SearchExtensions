using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.StartsWithExpressionBuilder
{
    internal static class EnumerableStartsWithExpressionBuilder
    {
        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var startsWithExpression = Build(stringProperty, searchTerm, searchOptions);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression Build<T>(Expression<Func<T, string>> stringProperty, string searchTerm, SearchOptions searchOptions)
        {
            var alteredSearchTerm = searchOptions.SearchType == SearchTypeEnum.WholeWords ? searchTerm + " " : searchTerm;
            var searchTermExpression = Expression.Constant(alteredSearchTerm);
            if (searchOptions.NullCheck)
            {
                var coalesceExpression = Expression.Coalesce(stringProperty.Body, ExpressionMethods.EmptyStringExpression);
                var nullCheckExpresion = Expression.Call(coalesceExpression, ExpressionMethods.IndexOfMethodWithComparison, searchTermExpression, searchOptions.ComparisonTypeExpression);
                return Expression.Equal(nullCheckExpresion, ExpressionMethods.ZeroConstantExpression);
            }

            var indexOfCallExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.IndexOfMethodWithComparison, searchTermExpression, searchOptions.ComparisonTypeExpression);
            return Expression.Equal(indexOfCallExpresion, ExpressionMethods.ZeroConstantExpression);
        }

        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, IEnumerable<Expression<Func<T, string>>> propertiesToSearchFor, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var startsWithExpression = Build(stringProperty, propertyToSearchFor, searchOptions.ComparisonTypeExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>> propertyToSearchFor, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            var indexOfCallExpression = Expression.Call(stringProperty.Body, ExpressionMethods.IndexOfMethodWithComparison, propertyToSearchFor.Body, stringComparisonExpression);
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
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, string[] searchTerms, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var startsWithExpression = Build(stringProperty, searchTerms, searchOptions);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, Expression<Func<T, string>>[] propertiesToSearchFor, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var startsWithExpression = Build(stringProperty, propertiesToSearchFor, searchOptions);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }
    }
}