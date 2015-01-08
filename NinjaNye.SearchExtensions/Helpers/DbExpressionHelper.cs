using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
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
        public static Expression BuildContainsExpression<T>(Expression<Func<T, string>> propertyToSearch, ConstantExpression searchTermExpression)
        {
            Ensure.ArgumentNotNull(searchTermExpression, "searchTermExpression");
            return Expression.Call(propertyToSearch.Body, ContainsMethod, searchTermExpression);
        }

        /// <summary>
        /// Build a 'contains' expression to search a string property contained within another string property
        /// </summary>
        /// <param name="propertyToSearch">Property on which to perform search</param>
        /// <param name="propertyToSearchFor">Property containing the value to search for</param>
        /// <returns></returns>
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
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildStartsWithExpression<T>(Expression<Func<T, string>> stringProperty, params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var startsWithExpression = BuildStartsWithExpression(stringProperty, propertyToSearchFor);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildStartsWithExpression<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>> propertyToSearchFor)
        {
            var indexOfCallExpresion = Expression.Call(stringProperty.Body, IndexOfMethod, propertyToSearchFor.Body);
            return Expression.Equal(indexOfCallExpresion, ZeroConstantExpression);
        }

        /// <summary>
        /// Build an 'equals' expression for a search term against a particular string property
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

        /// <summary>
        /// Build an 'equals' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildEqualsExpression<TSource, TType>(Expression<Func<TSource, TType>> source, 
                                                                       IEnumerable<Expression<Func<TSource, TType>>> comparedTo)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in comparedTo)
            {
                var isEqualExpression = Expression.Equal(source.Body, propertyToSearchFor.Body);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }

            return completeExpression;
        }
    }
}