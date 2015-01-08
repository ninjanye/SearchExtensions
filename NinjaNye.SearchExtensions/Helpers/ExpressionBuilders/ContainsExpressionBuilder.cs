using System;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Validation;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders
{
    internal static class ContainsExpressionBuilder
    {
        internal static class Enumerable
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
            public static BinaryExpression Build<T>(Expression<Func<T, string>> propertyToSearch, ConstantExpression searchTermExpression, ConstantExpression stringComparisonExpression, bool nullCheck = true)
            {
                var coalesceExpression = Expression.Coalesce(propertyToSearch.Body, ExpressionMethods.EmptyStringExpression);
                var nullCheckExpresion = Expression.Call(coalesceExpression, ExpressionMethods.IndexOfMethodWithComparison, searchTermExpression, stringComparisonExpression);
                return Expression.GreaterThanOrEqual(nullCheckExpresion, ExpressionMethods.ZeroConstantExpression);
            }
        }

        internal static class Queryable
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
}