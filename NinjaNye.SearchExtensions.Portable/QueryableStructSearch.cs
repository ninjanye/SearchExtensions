using System;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Portable.Helpers.ExpressionBuilders.EqualsExpressionBuilder;

namespace NinjaNye.SearchExtensions.Portable
{
    public class QueryableStructSearch<TSource, TProperty> : QueryableSearchBase<TSource, TProperty>
        where TProperty : struct
    {
        public QueryableStructSearch(IQueryable<TSource> source, Expression<Func<TSource, TProperty>>[] properties)
            : base(source, properties)
        {
        }

        public IQueryable<TSource> Include(string path)
        {
            QueryInclude(path);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// equal any of the supplied values
        /// </summary>
        /// <param name="values">Values to search for</param>
        public QueryableStructSearch<TSource, TProperty> EqualTo(params TProperty[] values)
        {
            var equalsExpression = ExpressionBuilder.EqualsExpression(Properties, values);
            BuildExpression(equalsExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public QueryableStructSearch<TSource, TProperty> GreaterThan(TProperty value)
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
        public QueryableStructSearch<TSource, TProperty> LessThan(TProperty value)
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
        public QueryableStructSearch<TSource, TProperty> LessThanOrEqualTo(TProperty value)
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
        public QueryableStructSearch<TSource, TProperty> GreaterThanOrEqualTo(TProperty value)
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
        public QueryableStructSearch<TSource, TProperty> Between(TProperty minValue, TProperty maxValue)
        {
            var betweenExpression = ExpressionBuilder.BetweenExpression(Properties, minValue, maxValue);
            BuildExpression(betweenExpression);
            return this;
        }
    }
}