using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder
{
    internal static class QueryableEqualsExpressionBuilder
    {
        /// <summary>
        /// Build an 'equals' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<TSource, TType>(Expression<Func<TSource, TType>>[] properties, string[] terms)
        {
            Expression completeExpression = null;
            foreach (var property in properties)
            {
                var equalsExpression = Build(property, terms);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, equalsExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'equals' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<TSource, TType>(Expression<Func<TSource, TType>> property, IEnumerable<string> terms)
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
        public static Expression Build<TSource, TType>(Expression<Func<TSource, TType>> source,
                                                       IEnumerable<Expression<Func<TSource, TType>>> comparedTo)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in comparedTo)
            {
                var isEqualExpression = Build(source, propertyToSearchFor);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build an 'equals' expression for a search term against a particular string property
        /// </summary>
        public static Expression Build<TSource, TType>(Expression<Func<TSource, TType>> source,
                                                       Expression<Func<TSource, TType>> comparedTo)
        {
            return Expression.Equal(source.Body, comparedTo.Body);
        }             
    }
}