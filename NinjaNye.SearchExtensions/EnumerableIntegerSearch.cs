using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableIntegerSearch<T> : EnumerableSearchBase<T, int>
    {
        public EnumerableIntegerSearch(IEnumerable<T> source, Expression<Func<T, int>>[] properties) 
            : base(source, properties)
        {
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// equal any of the supplied values
        /// </summary>
        /// <param name="values">Values to search for</param>
        public EnumerableIntegerSearch<T> IsEqual(params int[] values)
        {
            var equalsExpression = ExpressionBuilder.EqualsExpression(Properties, values);
            this.BuildExpression(equalsExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableIntegerSearch<T> GreaterThan(int value)
        {
            var greaterThanExpression = ExpressionBuilder.GreaterThanExpression(Properties, value);
            BuildExpression(greaterThanExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are less than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableIntegerSearch<T> LessThan(int value)
        {
            var greaterThanExpression = ExpressionBuilder.LessThanExpression(Properties, value);
            BuildExpression(greaterThanExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are less than or equal to any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableIntegerSearch<T> LessThanOrEqual(int value)
        {
            var lessThanOrEqualExpression = ExpressionBuilder.LessThanOrEqualExpression(Properties, value);
            BuildExpression(lessThanOrEqualExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than or equal to any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableIntegerSearch<T> GreaterThanOrEqual(int value)
        {
            var greaterThanOrEqualExpression = ExpressionBuilder.GreaterThanOrEqualExpression(Properties, value);
            BuildExpression(greaterThanOrEqualExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than <paramref name="minValue">minValue</paramref> 
        /// AND less than <paramref name="maxValue">maxValue</paramref> 
        /// </summary>
        public EnumerableIntegerSearch<T> Between(int minValue, int maxValue)
        {
            var betweenExpression = ExpressionBuilder.BetweenExpression(Properties, minValue, maxValue);
            BuildExpression(betweenExpression);
            return this;
        } 
    }
}