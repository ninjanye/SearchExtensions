using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NinjaNye.SearchExtensions.Validation;

namespace NinjaNye.SearchExtensions.Helpers
{
    internal static class DbExpressionHelper
    {
        static readonly MethodInfo IndexOfMethod = typeof(string).GetMethod("IndexOf", new[] { typeof(string) });
        static readonly ConstantExpression ZeroConstantExpression = Expression.Constant(0);
        static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains");

        /// <summary>
        /// Build a 'contains' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildContainsExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression)
        {
            Ensure.ArgumentNotNull(searchTermExpression, "searchTermExpression");
            return Expression.Call(stringProperty.Body, ContainsMethod, searchTermExpression);
        }

        public static Expression BuildContainsExpression<T>(Expression<Func<T, string>> propertyToSearch, Expression<Func<T, string>> propertyToSearchFor)
        {
            return Expression.Call(propertyToSearch.Body, ContainsMethod, propertyToSearchFor.Body);
        }

        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildStartsWithExpression<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms)
        {
            Expression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var startsWithExpression = BuildStartsWithExpression(stringProperty, searchTerm);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildStartsWithExpression<T>(Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            var searchTermExpression = Expression.Constant(searchTerm);
            var indexOfCallExpresion = Expression.Call(stringProperty.Body, IndexOfMethod, searchTermExpression);
            return Expression.Equal(indexOfCallExpresion, ZeroConstantExpression);
        }

        /// <summary>
        /// Build a 'equals' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildEqualsExpression<TSource, TType>(Expression<Func<TSource, TType>> property, IEnumerable<string> terms)
        {
            Expression completeExpression = null;
            foreach (var term in terms)
            {
                var searchTermExpression = Expression.Constant(term);
                var equalsExpression = Expression.Equal(property.Body, searchTermExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, equalsExpression);
            }
            return completeExpression;
        }
    }
}