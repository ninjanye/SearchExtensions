using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EndsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.StartsWithExpressionBuilder;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.ContainsExpressionBuilder
{
    internal static class EnumerableContainsExpressionBuilder
    {
        /// <summary>
        /// Build a 'contains' expression for a searching a property that 
        /// contains the value of another string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>> propertyToSearch, Expression<Func<T, string>> propertyToSearchFor)
        {
            var isNotNullExpression = ExpressionHelper.BuildNotNullExpression(propertyToSearch);
            var searchForIsNotNullExpression = ExpressionHelper.BuildNotNullExpression(propertyToSearchFor);
            var containsExpression = Expression.Call(propertyToSearch.Body, ExpressionMethods.StringContainsMethod, propertyToSearchFor.Body);
            var fullNotNullExpression = Expression.AndAlso(isNotNullExpression, searchForIsNotNullExpression);
            return Expression.AndAlso(fullNotNullExpression, containsExpression);
        }

        /// <summary>
        /// Build a 'indexof() >= 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression Build<T>(Expression<Func<T, string>> propertyToSearch, ConstantExpression searchTermExpression, ConstantExpression stringComparisonExpression)
        {
            var coalesceExpression = Expression.Coalesce(propertyToSearch.Body, ExpressionMethods.EmptyStringExpression);
            var nullCheckExpresion = Expression.Call(coalesceExpression, ExpressionMethods.IndexOfMethodWithComparison, searchTermExpression, stringComparisonExpression);
            return Expression.GreaterThanOrEqual(nullCheckExpresion, ExpressionMethods.ZeroConstantExpression);
        }

        private static Expression Build<T>(Expression<Func<T, string>> propertyToSearch, IEnumerable<string> searchTerms, ConstantExpression stringComparisonExpression, SearchType searchType)
        {
            Expression completeExpression = null;
            bool isWholeWordSearch = searchType == SearchType.WholeWords;
            foreach (var searchTerm in searchTerms)
            {
                var searchTermExpression = Expression.Constant(isWholeWordSearch ? " " + searchTerm + " " : searchTerm);
                var containsExpression = Build(propertyToSearch, searchTermExpression, stringComparisonExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, containsExpression);
            }

            return completeExpression;
        }

        public static Expression Build<T>(Expression<Func<T, string>>[] properties, string[] searchTerms, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            var comparisonTypeExpression = Expression.Constant(searchOptions.ComparisonType);
            foreach (var stringProperty in properties)
            {
                var containsExpression = Build(stringProperty, searchTerms, comparisonTypeExpression, searchOptions.SearchType);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, containsExpression);
            }

            if (searchOptions.SearchType == SearchType.WholeWords)
            {
                var startsWithExpression = EnumerableStartsWithExpressionBuilder.Build(properties, searchTerms, searchOptions);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);

                var endsWithExpression = EnumerableEndsWithExpressionBuilder.Build(properties, searchTerms, searchOptions);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, endsWithExpression);

                var equalsExpression = EnumerableEqualsExpressionBuilder.Build(properties, searchTerms, searchOptions);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, equalsExpression);
            }
            return completeExpression;
        }

    }
}