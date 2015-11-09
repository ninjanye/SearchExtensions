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
    public class QueryableChildSearch<TParent, TChild, TProperty> : IQueryable<TParent>
    {
        private IQueryable<TParent> _parent;
        private readonly Expression<Func<TParent, IEnumerable<TChild>>>[] _childProperties;
        private Expression<Func<TChild, TProperty>>[] _properties;
        private readonly ParameterExpression _childParameter = Expression.Parameter(typeof(TChild), "child");
        private Expression _completeExpression;

        public QueryableChildSearch(IQueryable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties)
        {
            this._parent = parent;
            this._childProperties = childProperties;

            _properties = this.AlignParameters(properties);
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are equal to any of the supplied <paramref name="values">values</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public QueryableChildSearch<TParent, TChild, TProperty> EqualTo(params TProperty[] values)
        {
            var equalToExpression = ExpressionBuilder.EqualsExpression(this._properties, values);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, equalToExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied <paramref name="value">value</paramref> 
        /// </summary>
        /// <param name="value">A collection of values to match upon</param>
        public QueryableChildSearch<TParent, TChild, TProperty> GreaterThan(TProperty value)
        {
            var greaterThanExpression = ExpressionBuilder.GreaterThanExpression(this._properties, value);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, greaterThanExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than or equal to any of the supplied <paramref name="value">value</paramref> 
        /// </summary>
        /// <param name="value">A collection of values to match upon</param>
        public QueryableChildSearch<TParent, TChild, TProperty> GreaterThanOrEqualTo(TProperty value)
        {
            var greaterThanExpression = ExpressionBuilder.GreaterThanOrEqualExpression(this._properties, value);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, greaterThanExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied <paramref name="value">value</paramref> 
        /// </summary>
        /// <param name="value">A collection of values to match upon</param>
        public QueryableChildSearch<TParent, TChild, TProperty> LessThan(TProperty value)
        {
            var lessThanExpression = ExpressionBuilder.LessThanExpression(this._properties, value);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, lessThanExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied <paramref name="value">value</paramref> 
        /// </summary>
        /// <param name="value">A collection of values to match upon</param>
        public QueryableChildSearch<TParent, TChild, TProperty> LessThanOrEqualTo(TProperty value)
        {
            var lessThanExpression = ExpressionBuilder.LessThanOrEqualExpression(this._properties, value);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, lessThanExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are greater than any of the supplied <paramref name="value">value</paramref> 
        /// </summary>
        public QueryableChildSearch<TParent, TChild, TProperty> Between(TProperty minvalue, TProperty maxValue)
        {
            var betweenExpression = ExpressionBuilder.BetweenExpression(this._properties, minvalue, maxValue);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, betweenExpression);
            return this;
        }

        public QueryableChildSearch<TParent, TChild, TAnotherProperty> With<TAnotherProperty>(params Expression<Func<TChild, TAnotherProperty>>[] properties)
        {
            return new QueryableChildSearch<TParent, TChild, TAnotherProperty>(this.UpdatedSource(), _childProperties, properties);            
        }

        private Expression<Func<TChild, TProperty>>[] AlignParameters(Expression<Func<TChild, TProperty>>[] properties)
        {
            var swappedProperties = new List<Expression<Func<TChild, TProperty>>>();
            foreach (var property in properties)
            {
                var swappedProperty = SwapExpressionVisitor.Swap(property, property.Parameters.Single(), this._childParameter);
                swappedProperties.Add(swappedProperty);
            }
            this._properties = swappedProperties.ToArray();
            return swappedProperties.ToArray();
        }

        public IEnumerator<TParent> GetEnumerator()
        {
            return this.UpdatedSource().GetEnumerator();
        }

        private IQueryable<TParent> UpdatedSource()
        {
            if (_completeExpression == null)
            {
                return _parent;
            }

            var childProperty = this._childProperties[0];
            var methodInfo = ExpressionMethods.AnyQueryableMethod.MakeGenericMethod(typeof (TChild));
            var anyExpression = Expression.Lambda<Func<TChild, bool>>(this._completeExpression, this._childParameter);

            var anyChild = Expression.Call(null, methodInfo, childProperty.Body, anyExpression);
            var finalExpression = Expression.Lambda<Func<TParent, bool>>(anyChild, childProperty.Parameters[0]);
            return this._parent.Where(finalExpression);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Expression Expression { get; private set; }
        public Type ElementType { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }
}