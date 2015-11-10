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
        private readonly Expression<Func<TParent, IEnumerable<TChild>>>[] _childProperties;
        private readonly Expression<Func<TChild, TProperty>>[] _properties;
        private readonly ParameterExpression _parentParameter;
        private readonly ParameterExpression _childParameter = Expression.Parameter(typeof(TChild), "child");
        private Expression _completeExpression;

        public EnumerableChildSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties)
            :this(parent, childProperties, properties, null, null)
        {
        }

        public EnumerableChildSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties, Expression completeExpression, ParameterExpression childParameter)
        {
            this._parent = parent;
            this._parentParameter = childProperties[0].Parameters[0];
            if (childParameter != null) this._childParameter = childParameter;
            _completeExpression = completeExpression;

            _childProperties = this.AlignParameters(childProperties, this._parentParameter).ToArray();
            _properties = this.AlignParameters(properties, this._childParameter).ToArray();
        }

        private IEnumerable<Expression<Func<TSource, TResult>>> AlignParameters<TSource, TResult>(Expression<Func<TSource, TResult>>[] properties, ParameterExpression parameterExpression)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                yield return SwapExpressionVisitor.Swap(property, property.Parameters.Single(), parameterExpression);
            }
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

        public EnumerableChildSearch<TParent, TChild, TAnotherProperty> With<TAnotherProperty>(params Expression<Func<TChild, TAnotherProperty>>[] properties)
        {
            return new EnumerableChildSearch<TParent, TChild, TAnotherProperty>(_parent, _childProperties, properties, _completeExpression, _childParameter);
        }

        public EnumerableChildStringSearch<TParent, TChild> With(params Expression<Func<TChild, string>>[] properties)
        {
            return new EnumerableChildStringSearch<TParent, TChild>(_parent, _childProperties, properties, _completeExpression, _childParameter);
        }

        public IEnumerator<TParent> GetEnumerator()
        {
            return this.UpdatedSource().GetEnumerator();
        }

        private IEnumerable<TParent> UpdatedSource()
        {
            if (_completeExpression == null)
            {
                return _parent;
            }

            var anyMethodInfo = ExpressionMethods.AnyQueryableMethod.MakeGenericMethod(typeof(TChild));
            Expression finalExpression = null;
            foreach (var childProperty in _childProperties)
            {
                var anyExpression = Expression.Lambda<Func<TChild, bool>>(this._completeExpression, this._childParameter);
                var anyChild = Expression.Call(null, anyMethodInfo, childProperty.Body, anyExpression);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, anyChild);
            }

            var final = Expression.Lambda<Func<TParent, bool>>(finalExpression, this._parentParameter).Compile();
            return this._parent.Where(final);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}