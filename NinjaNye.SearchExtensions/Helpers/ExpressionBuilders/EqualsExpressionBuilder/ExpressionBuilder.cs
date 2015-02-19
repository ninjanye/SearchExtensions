using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder
{
    internal static class ExpressionBuilder
    {
        /// <summary>
        /// Build an 'equals' expression for a value against supplied properties
        /// </summary>
        public static Expression EqualsExpression<TSource, TType>(Expression<Func<TSource, TType>>[] properties, TType[] values)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearch in properties)
            {
                var isEqualExpression = EqualsExpression(propertyToSearch, values);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'equals' expression for a value against a particular property
        /// </summary>
        private static Expression EqualsExpression<TSource, TType>(Expression<Func<TSource, TType>> property, IEnumerable<TType> values)
        {
            Expression completeExpression = null;
            foreach (var value in values)
            {
                var valueExpression = Expression.Constant(value);
                var equalsExpression = Expression.Equal(property.Body, valueExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, equalsExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build a 'greater than' expression for a value against supplied properties
        /// </summary>
        public static Expression GreaterThanExpression<TSource, TType>(Expression<Func<TSource, TType>>[] properties, TType value)
        {
            Expression completeExpression = null;
            var valueExpression = Expression.Constant(value);
            foreach (var property in properties)
            {
                var isEqualExpression = Expression.GreaterThan(property.Body, valueExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build a 'less than' expression for a value against supplied properties
        /// </summary>
        public static Expression LessThanExpression<TSource, TType>(Expression<Func<TSource, TType>>[] properties, TType value)
        {
            Expression completeExpression = null;
            var valueExpression = Expression.Constant(value);
            foreach (var property in properties)
            {
                var isEqualExpression = Expression.LessThan(property.Body, valueExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }
            return completeExpression;
        }
        
    }
}