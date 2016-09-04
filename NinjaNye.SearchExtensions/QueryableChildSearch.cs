using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;

namespace NinjaNye.SearchExtensions
{
    public class QueryableChildSearch<TParent, TChild, TProperty> : QueryableChildSearchBase<TParent, TChild, TProperty>
    {
        public QueryableChildSearch(IQueryable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties) 
            : base(parent, childProperties, properties, null, null)
        {
        }

        public QueryableChildSearch(IQueryable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties, Expression completeExpression, ParameterExpression childParameter)
            : base(parent, childProperties, properties, completeExpression, childParameter)
        {
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are equal to any of the supplied <paramref name="values">values</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public QueryableChildSearch<TParent, TChild, TProperty> EqualTo(params TProperty[] values)
        {
            AppendExpression(ExpressionBuilder.EqualsExpression(Properties, values));
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied <paramref name="value">value</paramref> 
        /// </summary>
        /// <param name="value">A collection of values to match upon</param>
        public QueryableChildSearch<TParent, TChild, TProperty> GreaterThan(TProperty value)
        {
            AppendExpression(ExpressionBuilder.GreaterThanExpression(Properties, value));
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than or equal to any of the supplied <paramref name="value">value</paramref> 
        /// </summary>
        /// <param name="value">A collection of values to match upon</param>
        public QueryableChildSearch<TParent, TChild, TProperty> GreaterThanOrEqualTo(TProperty value)
        {
            AppendExpression(ExpressionBuilder.GreaterThanOrEqualExpression(Properties, value));
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied <paramref name="value">value</paramref> 
        /// </summary>
        /// <param name="value">A collection of values to match upon</param>
        public QueryableChildSearch<TParent, TChild, TProperty> LessThan(TProperty value)
        {
            AppendExpression(ExpressionBuilder.LessThanExpression(Properties, value));
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied <paramref name="value">value</paramref> 
        /// </summary>
        /// <param name="value">A collection of values to match upon</param>
        public QueryableChildSearch<TParent, TChild, TProperty> LessThanOrEqualTo(TProperty value)
        {
            AppendExpression(ExpressionBuilder.LessThanOrEqualExpression(Properties, value));
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied <paramref name="value">value</paramref> 
        /// </summary>
        public QueryableChildSearch<TParent, TChild, TProperty> Between(TProperty minvalue, TProperty maxValue)
        {
            AppendExpression(ExpressionBuilder.BetweenExpression(Properties, minvalue, maxValue));
            return this;
        }
    }
}