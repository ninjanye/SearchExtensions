using System;
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
            foreach (var value in values)
            {
                var equalExpression = DynamicExpression(properties, value, Expression.Equal);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, equalExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build a 'greater than' expression for a value against supplied properties
        /// </summary>
        public static Expression GreaterThanExpression<TSource, TType>(Expression<Func<TSource, TType>>[] properties, TType value)
        {
            return DynamicExpression(properties, value, Expression.GreaterThan);
        }

        /// <summary>
        /// Build a 'less than' expression for a value against supplied properties
        /// </summary>
        public static Expression LessThanExpression<TSource, TType>(Expression<Func<TSource, TType>>[] properties, TType value)
        {
            return DynamicExpression(properties, value, Expression.LessThan);
        }

        /// <summary>
        /// Build a 'less than or equal' expression for a value against supplied properties
        /// </summary>
        public static Expression LessThanOrEqualExpression<TSource, TType>(Expression<Func<TSource, TType>>[] properties, TType value)
        {
            return DynamicExpression(properties, value, Expression.LessThanOrEqual);
        }

        /// <summary>
        /// Build a 'greater than or equal' expression for a value against supplied properties
        /// </summary>
        public static Expression GreaterThanOrEqualExpression<TSource, TType>(Expression<Func<TSource, TType>>[] properties, TType value)
        {
            return DynamicExpression(properties, value, Expression.GreaterThanOrEqual);
        }

        public static Expression BetweenExpression<TSource, TType>(Expression<Func<TSource, TType>>[] properties, TType minValue, TType maxValue)
        {
            Expression completeExpression = null;
            var minValueExpression = Expression.Constant(minValue);
            var maxValueExpression = Expression.Constant(maxValue);
            foreach (var property in properties)
            {
                var greaterThanExpression = Expression.GreaterThan(property.Body, minValueExpression);
                var lessThanExpression = Expression.LessThan(property.Body, maxValueExpression);
                var betweenExpression = Expression.AndAlso(greaterThanExpression, lessThanExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, betweenExpression);
            }
            return completeExpression;
        }

        private static Expression DynamicExpression<TSource, TType>(Expression<Func<TSource, TType>>[] properties, TType value, Func<Expression, ConstantExpression, Expression> buildComparisonExpression)
        {
            Expression completeExpression = null;
            var valueExpression = Expression.Constant(value);
            foreach (var property in properties)
            {
                var expression = buildComparisonExpression(property.Body, valueExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, expression);
            }
            return completeExpression;
        }
        
    }
}