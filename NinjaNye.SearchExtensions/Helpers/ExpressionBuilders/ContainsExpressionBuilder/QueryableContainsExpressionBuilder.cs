using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EndsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.StartsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Validation;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.ContainsExpressionBuilder
{
    internal static class QueryableContainsExpressionBuilder
    {
        public static Expression Build<T>(Expression<Func<T, string>>[] propertiesToSearch, ICollection<string> searchTerms, SearchType searchType)
        {
            Expression result = null;
            foreach (var propertyToSearch in propertiesToSearch)
            {
                var containsExpression = Build(propertyToSearch, searchTerms, searchType);
                result = ExpressionHelper.JoinOrExpression(result, containsExpression);
            }

            if (searchType == SearchType.WholeWords)
            {
                var terms = searchTerms.ToArray();
                var startsWithExpression = QueryableStartsWithExpressionBuilder.Build(propertiesToSearch, terms, searchType);
                result = ExpressionHelper.JoinOrExpression(result, startsWithExpression);
                var endsWithExpression = QueryableEndsWithExpressionBuilder.Build(propertiesToSearch, terms, searchType);
                result = ExpressionHelper.JoinOrExpression(result, endsWithExpression);

                var equalsExpression = QueryableEqualsExpressionBuilder.Build(propertiesToSearch, terms);
                result = ExpressionHelper.JoinOrExpression(result, equalsExpression);
            }

            return result;
        }

        private static Expression Build<T>(Expression<Func<T, string>> propertyToSearch, ICollection<string> searchTerms, SearchType searchType)
        {
            Expression result = null;
            foreach (var searchTerm in searchTerms)
            {
                Expression comparisonExpression = Build(propertyToSearch, searchTerm, searchType);
                result = ExpressionHelper.JoinOrExpression(result, comparisonExpression);
            }

            return result;
        }

        /// <summary>
        /// Build a 'contains' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> propertyToSearch, string searchTerm, SearchType searchType)
        {
            Ensure.ArgumentNotNull(searchTerm, "searchTerm");
            if (searchType == SearchType.WholeWords)
            {
                searchTerm = " " + searchTerm + " ";
            }
            ConstantExpression searchTermExpression = Expression.Constant(searchTerm);
            return Expression.Call(propertyToSearch.Body, ExpressionMethods.StringContainsMethod, searchTermExpression);
        }

        /// <summary>
        /// Build a 'contains' expression to search a string property contained within another string property
        /// </summary>
        /// <param name="propertyToSearch">Property on which to perform search</param>
        /// <param name="propertyToSearchFor">Property containing the value to search for</param>
        /// <returns></returns>
        public static Expression Build<T>(Expression<Func<T, string>> propertyToSearch, Expression<Func<T, string>> propertyToSearchFor)
        {
            return Expression.Call(propertyToSearch.Body, ExpressionMethods.StringContainsMethod, propertyToSearchFor.Body);
        }
    }
}