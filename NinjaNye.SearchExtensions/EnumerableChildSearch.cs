using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableChildSearch<TParent, TChild, TProperty> : EnumerableChildSearchBase<TParent, TChild, TProperty>
    {
        public EnumerableChildSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties)
            : base(parent, childProperties, properties)
        {
        }

        public EnumerableChildSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties, Expression completeExpression, ParameterExpression childParameter)
            : base(parent, childProperties, properties, completeExpression, childParameter)
        {
        }

        /// <summary>
        /// Identifies items where any of the defined properties
        /// are greater than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> GreaterThan(TProperty value)
        {
            var greaterThanExpression = ExpressionBuilder.GreaterThanExpression(this.Properties, value);
            this.JoinExpression(greaterThanExpression);
            return this;
        }

        /// <summary>
        /// Identifies items where any of the defined properties
        /// are greater than or equal to any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> GreaterThanOrEqualTo(TProperty value)
        {
            var greaterThanExpression = ExpressionBuilder.GreaterThanOrEqualExpression(this.Properties, value);
            this.JoinExpression(greaterThanExpression);
            return this;
        }

        /// <summary>
        /// Identifies items where any of the defined properties
        /// are less than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> LessThan(TProperty value)
        {
            var lessThanExpression = ExpressionBuilder.LessThanExpression(this.Properties, value);
            this.JoinExpression(lessThanExpression);
            return this;
        }

        /// <summary>
        /// Identifies items where any of the defined properties
        /// are less than or equal to any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> LessThanOrEqualTo(TProperty value)
        {
            var lessThanExpression = ExpressionBuilder.LessThanOrEqualExpression(this.Properties, value);
            this.JoinExpression(lessThanExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than <paramref name="minValue">minValue</paramref> 
        /// AND less than <paramref name="maxValue">maxValue</paramref> 
        /// </summary>
        public EnumerableChildSearch<TParent, TChild, TProperty> Between(TProperty minValue, TProperty maxValue)
        {
            var betweenExpression = ExpressionBuilder.BetweenExpression(this.Properties, minValue, maxValue);
            this.JoinExpression(betweenExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are equal to any of the supplied <paramref name="values">value</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> EqualTo(params TProperty[] values)
        {
            var equalToExpression = ExpressionBuilder.EqualsExpression(this.Properties, values);
            this.JoinExpression(equalToExpression);
            return this;
        }
    }
}