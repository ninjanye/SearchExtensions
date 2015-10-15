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
        private readonly Expression<Func<TChild, TProperty>> _property;
        private Expression _completeExpression;
        private readonly ParameterExpression _childParameter = Expression.Parameter(typeof(TChild), "child");

        public EnumerableChildSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>> child, Expression<Func<TChild, TProperty>> property)
        {
            this._parent = parent;
            this._child = child;
            this._property = SwapExpressionVisitor.Swap(property, property.Parameters.Single(), this._childParameter);
        }

        /// <summary>
        /// Identifies items where any of the defined properties
        /// are greater than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> GreaterThan(TProperty value)
        {
            var greaterThanExpression = ExpressionBuilder.GreaterThanExpression(new []{ this._property }, value);
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
            var lessThanExpression = ExpressionBuilder.LessThanExpression(new []{ this._property }, value);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, lessThanExpression);
            return this;
        }

        /// <summary>
        /// Identifies items where any of the defined properties
        /// are less than any of the supplied value
        /// </summary>
        /// <param name="value">Value to search for</param>
        public EnumerableChildSearch<TParent, TChild, TProperty> Between(TProperty minValue, TProperty maxValue)
        {
            var betweenExpression = ExpressionBuilder.BetweenExpression(new[] { this._property }, minValue, maxValue);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, betweenExpression);
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