using System;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;

namespace NinjaNye.SearchExtensions
{
    public class QueryableStructSearch<TSource, TProperty> : QueryableSearchBase<TSource, TProperty>
        where TProperty : struct
    {
        public QueryableStructSearch(IQueryable<TSource> source, Expression<Func<TSource, TProperty>>[] properties)
            : base(source, properties)
        {
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// equal any of the supplied values
        /// </summary>
        /// <param name="values">Values to search for</param>
        public QueryableStructSearch<TSource, TProperty> EqualTo(params TProperty[] values)
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
        public QueryableStructSearch<TSource, TProperty> GreaterThan(TProperty value)
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
        public QueryableStructSearch<TSource, TProperty> LessThan(TProperty value)
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
        public QueryableStructSearch<TSource, TProperty> LessThanOrEqualTo(TProperty value)
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
        public QueryableStructSearch<TSource, TProperty> GreaterThanOrEqualTo(TProperty value)
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
        public QueryableStructSearch<TSource, TProperty> Between(TProperty minValue, TProperty maxValue)
        {
            var betweenExpression = ExpressionBuilder.BetweenExpression(this.Properties, minValue, maxValue);
            this.BuildExpression(betweenExpression);
            return this;
        }
    }
}