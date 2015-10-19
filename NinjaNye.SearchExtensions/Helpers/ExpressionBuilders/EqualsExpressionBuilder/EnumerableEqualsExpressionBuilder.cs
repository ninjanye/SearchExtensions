using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder
{
    internal static class EnumerableEqualsExpressionBuilder
    {
        /// <summary>
        /// Build an 'equals' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<TSource, TType>(Expression<Func<TSource, TType>>[] properties, string[] terms, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearch in properties)
            {
                var isEqualExpression = Build(propertyToSearch, terms, searchOptions);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'equals' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<TSource, TType>(Expression<Func<TSource, TType>> property, IEnumerable<string> terms, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            foreach (var term in terms)
            {
                var searchTermExpression = Expression.Constant(term);
                var equalsExpression = Expression.Call(property.Body, ExpressionMethods.EqualsMethod, searchTermExpression, searchOptions.ComparisonTypeExpression);
                completeExpression = completeExpression == null ? equalsExpression
                    : ExpressionHelper.JoinOrExpression(completeExpression, equalsExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'equals' expression for one string property against another string property
        /// </summary>
        public static Expression Build<TSource, TType>(Expression<Func<TSource, TType>>[] propertiesToSearch, Expression<Func<TSource, TType>>[] propertiesToSearchFor, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearch in propertiesToSearch)
            {
                var isEqualExpression = Build(propertyToSearch, propertiesToSearchFor, searchOptions);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'equals' expression for one string property against another string property
        /// </summary>
        private static Expression Build<TSource, TType>(Expression<Func<TSource, TType>> propertyToSearch, IEnumerable<Expression<Func<TSource, TType>>> propertiesToSearchFor, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var isEqualExpression = Expression.Call(propertyToSearch.Body, ExpressionMethods.EqualsMethod, propertyToSearchFor.Body, searchOptions.ComparisonTypeExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }
            return completeExpression;
        }
    }
}