using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EndsWithExpressionBuilder
{
    internal static class QueryableEndsWithExpressionBuilder
    {
        /// <summary>
        /// Build an 'EndsWith' expression for a collection of search terms against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms, SearchType searchType)
        {
            Expression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var endsWithExpression = Build(stringProperty, searchTerm, searchType);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, endsWithExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build an 'EndsWith' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, string searchTerm, SearchType searchType)
        {
            var alteredSearchTerm = searchType == SearchType.WholeWords ? " " + searchTerm : searchTerm;
            var searchTermExpression = Expression.Constant(alteredSearchTerm);
            return Expression.Call(stringProperty.Body, ExpressionMethods.EndsWithMethod, searchTermExpression);
        }

        /// <summary>
        /// Build an 'EndsWith' expression for a collection of properties against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>>[] propertiesToSearchFor, SearchType searchType)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var endsWithExpression = Build(stringProperty, propertyToSearchFor, searchType);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, endsWithExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build an 'EndsWith' expression for a string property against multiple properties
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>> propertyToSearchFor, SearchType searchType)
        {
            var paddedTerm = propertyToSearchFor.Body;
            if (searchType == SearchType.WholeWords)
            {
                var seperator = Expression.Constant(" ");
                paddedTerm = Expression.Call(ExpressionMethods.StringConcatMethod, seperator, propertyToSearchFor.Body);
            }

            var result = Expression.Call(stringProperty.Body, ExpressionMethods.EndsWithMethod, paddedTerm);
            if (searchType == SearchType.WholeWords)
            {
                var isEqualExpression = QueryableEqualsExpressionBuilder.Build(stringProperty, propertyToSearchFor);
                return ExpressionHelper.JoinOrExpression(result, isEqualExpression);
            }
            return result;
        }

        /// <summary>
        /// Build an 'EndsWith' expression comparing multiple string properties against other string properties
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, Expression<Func<T, string>>[] propertiesToSearchFor, SearchType searchType)
        {
            Expression finalExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var endsWithExpression = Build(stringProperty, propertiesToSearchFor, searchType);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, endsWithExpression);
            }
            return finalExpression;
        }

        /// <summary>
        /// Build an 'EndsWith([searchTerm])' expression comparing multiple string properties against string terms
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, string[] searchTerms, SearchType searchType)
        {
            Expression finalExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var endsWithExpression = Build(stringProperty, searchTerms, searchType);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, endsWithExpression);
            }
            return finalExpression;
        }
    }
}