using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.StartsWithExpressionBuilder
{
    internal static class QueryableStartsWithExpressionBuilder
    {
        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, string[] searchTerms, SearchType searchType)
        {
            Expression completeExpression = null;
            foreach (var property in stringProperties)
            {
                var startsWithExpression = Build(property, searchTerms, searchType);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms, SearchType searchType)
        {
            Expression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var startsWithExpression = Build(stringProperty, searchTerm, searchType);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, string searchTerm, SearchType searchType)
        {
            var paddedTerm = searchType == SearchType.WholeWords ? searchTerm + " " : searchTerm;
            var searchTermExpression = Expression.Constant(paddedTerm);
            return Expression.Call(stringProperty.Body, ExpressionMethods.StartsWithMethod, searchTermExpression);
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, Expression<Func<T, string>>[] propertiesToSearchFor, SearchType searchType)
        {
            Expression completeExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var startsWithExpression = Build(stringProperty, propertiesToSearchFor, searchType);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>>[] propertiesToSearchFor, SearchType searchType)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var startsWithExpression = Build(stringProperty, propertyToSearchFor, searchType);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>> propertyToSearchFor, SearchType searchType)
        {
            var paddedTerm = propertyToSearchFor.Body;
            if (searchType == SearchType.WholeWords)
            {
                var seperator = Expression.Constant(" ");
                paddedTerm = Expression.Call(ExpressionMethods.StringConcatMethod, propertyToSearchFor.Body, seperator);
            }

            var result = Expression.Call(stringProperty.Body, ExpressionMethods.StartsWithMethod, paddedTerm);
            if (searchType == SearchType.WholeWords)
            {
                var isEqualExpression = QueryableEqualsExpressionBuilder.Build(stringProperty, propertyToSearchFor);
                return ExpressionHelper.JoinOrExpression(result, isEqualExpression);
            }

            return result;
        }
    }
}