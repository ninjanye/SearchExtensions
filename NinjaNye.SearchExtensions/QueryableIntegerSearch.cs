using System;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;

namespace NinjaNye.SearchExtensions
{
    public class QueryableIntegerSearch<T> : QueryableSearchBase<T, int>
    {
        public QueryableIntegerSearch(IQueryable<T> source, Expression<Func<T, int>>[] properties)
            : base(source, properties)
        {
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// equal any of the supplied values
        /// </summary>
        /// <param name="values">Values to search for</param>
        public QueryableIntegerSearch<T> IsEqual(params int[] values)
        {
            var equalsExpression = ExpressionBuilder.EqualsExpression(this.Properties, values);
            this.BuildExpression(equalsExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public QueryableIntegerSearch<T> GreaterThan(int value)
        {
            var greaterThanExpression = ExpressionBuilder.GreaterThanExpression(this.Properties, value);
            this.BuildExpression(greaterThanExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are less than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public QueryableIntegerSearch<T> LessThan(int value)
        {
            var greaterThanExpression = ExpressionBuilder.LessThanExpression(this.Properties, value);
            this.BuildExpression(greaterThanExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are less than or equal to any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public QueryableIntegerSearch<T> LessThanOrEqual(int value)
        {
            var lessThanOrEqualExpression = ExpressionBuilder.LessThanOrEqualExpression(this.Properties, value);
            this.BuildExpression(lessThanOrEqualExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than or equal to any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public QueryableIntegerSearch<T> GreaterThanOrEqual(int value)
        {
            var greaterThanOrEqualExpression = ExpressionBuilder.GreaterThanOrEqualExpression(this.Properties, value);
            this.BuildExpression(greaterThanOrEqualExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than <paramref name="minValue">minValue</paramref> 
        /// AND less than <paramref name="maxValue">maxValue</paramref> 
        /// </summary>
        public QueryableIntegerSearch<T> Between(int minValue, int maxValue)
        {
            var betweenExpression = ExpressionBuilder.BetweenExpression(this.Properties, minValue, maxValue);
            this.BuildExpression(betweenExpression);
            return this;
        }
    }
}