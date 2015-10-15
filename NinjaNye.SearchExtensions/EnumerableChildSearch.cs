using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableChildSearch<TParent, TChild, TProperty> : IEnumerable<TParent>
    {
        private readonly IEnumerable<TParent> _parent;
        private readonly Expression<Func<TParent, IEnumerable<TChild>>> _child;
        private readonly Expression<Func<TChild, TProperty>>[] _properties;
        private Expression _completeExpression;
        private readonly ParameterExpression _childParameter = Expression.Parameter(typeof(TChild), "child");

        public EnumerableChildSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>> child, Expression<Func<TChild, TProperty>>[] properties)
        {
            this._parent = parent;
            this._child = child;

            var swappedProperties = new List<Expression<Func<TChild, TProperty>>>();
            foreach (var property in properties)
            {
                var swappedProperty = SwapExpressionVisitor.Swap(property, property.Parameters.Single(), this._childParameter);
                swappedProperties.Add(swappedProperty);
            }

            _properties = swappedProperties.ToArray();
        }

        /// <summary>
        /// Identifies items where any of the defined properties
        /// are greater than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> GreaterThan(TProperty value)
        {
            var greaterThanExpression = ExpressionBuilder.GreaterThanExpression(this._properties, value);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, greaterThanExpression);
            return this;
        }

        /// <summary>
        /// Identifies items where any of the defined properties
        /// are greater than or equal to any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> GreaterThanOrEqualTo(TProperty value)
        {
            var greaterThanExpression = ExpressionBuilder.GreaterThanOrEqualExpression(this._properties, value);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, greaterThanExpression);
            return this;
        }

        /// <summary>
        /// Identifies items where any of the defined properties
        /// are less than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> LessThan(TProperty value)
        {
            var lessThanExpression = ExpressionBuilder.LessThanExpression(this._properties, value);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, lessThanExpression);
            return this;
        }

        /// <summary>
        /// Identifies items where any of the defined properties
        /// are less than or equal to any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> LessThanOrEqualTo(TProperty value)
        {
            var lessThanExpression = ExpressionBuilder.LessThanOrEqualExpression(this._properties, value);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, lessThanExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than <paramref name="minValue">minValue</paramref> 
        /// AND less than <paramref name="maxValue">maxValue</paramref> 
        /// </summary>
        public EnumerableChildSearch<TParent, TChild, TProperty> Between(TProperty minValue, TProperty maxValue)
        {
            var betweenExpression = ExpressionBuilder.BetweenExpression(this._properties, minValue, maxValue);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, betweenExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are equal to any of the supplied <paramref name="values">value</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> EqualTo(params TProperty[] values)
        {
            var equalToExpression = ExpressionBuilder.EqualsExpression(this._properties, values);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, equalToExpression);
            return this;
        }


        public IEnumerator<TParent> GetEnumerator()
        {
            return this._completeExpression == null ? this._parent.GetEnumerator() 
                                                    : this.GetEnueratorWithoutChecks();
        }

        private IEnumerator<TParent> GetEnueratorWithoutChecks()
        {
            var finalExpression = Expression.Lambda<Func<TChild, bool>>(this._completeExpression, this._childParameter).Compile();
            foreach (var parent in this._parent)
            {
                var children = this._child.Compile().Invoke(parent);
                var isMatch = children.Any(c => finalExpression.Invoke(c));
                if (isMatch)
                {
                    yield return parent;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}