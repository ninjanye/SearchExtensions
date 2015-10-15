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
        private static BinaryExpression Build<T>(Expression<Func<T, string>> stringProperty, string searchTerm, SearchType searchType)
        {
            var alteredSearchTerm = searchType == SearchType.WholeWords ? searchTerm + " " : searchTerm;
            var searchTermExpression = Expression.Constant(alteredSearchTerm);
            var indexOfCallExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.IndexOfMethod, searchTermExpression);
            return Expression.Equal(indexOfCallExpresion, ExpressionMethods.ZeroConstantExpression);
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
            var seperator = Expression.Constant(" ");
            var paddedTerm = propertyToSearchFor.Body;
            if (searchType == SearchType.WholeWords)
            {
                paddedTerm = Expression.Call(ExpressionMethods.StringConcatMethod, propertyToSearchFor.Body, seperator);
            }
            var indexOfCallExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.IndexOfMethod, paddedTerm);
            var result = Expression.Equal(indexOfCallExpresion, ExpressionMethods.ZeroConstantExpression);
            if (searchType == SearchType.WholeWords)
            {
                var isEqualExpression = QueryableEqualsExpressionBuilder.Build(stringProperty, propertyToSearchFor);
                return ExpressionHelper.JoinOrExpression(result, isEqualExpression);
            }

            return result;
        }
    }
}