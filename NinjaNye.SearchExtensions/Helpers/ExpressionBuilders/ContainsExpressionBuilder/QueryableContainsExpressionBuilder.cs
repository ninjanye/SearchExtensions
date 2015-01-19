using System;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Validation;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.ContainsExpressionBuilder
{
    internal static class QueryableContainsExpressionBuilder
    {
        /// <summary>
        /// Build a 'contains' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>> propertyToSearch, ConstantExpression searchTermExpression)
        {
            Ensure.ArgumentNotNull(searchTermExpression, "searchTermExpression");
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