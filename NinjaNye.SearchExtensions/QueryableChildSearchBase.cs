using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    public abstract class QueryableChildSearchBase<TParent, TChild, TProperty> : IQueryable<TParent>
    {
        private readonly IQueryable<TParent> _parent;
        private readonly Expression<Func<TParent, IEnumerable<TChild>>>[] _childProperties;
        protected Expression<Func<TChild, TProperty>>[] Properties;
        private readonly ParameterExpression _childParameter = Expression.Parameter(typeof(TChild), "child");
        private Expression _completeExpression;

        protected QueryableChildSearchBase(IQueryable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties, Expression completeExpression, ParameterExpression childParamerter)
        {
            this._parent = parent;
            this._childProperties = childProperties;
            this._completeExpression = completeExpression;
            if (childParamerter != null) this._childParameter = childParamerter;

            this.Properties = this.AlignParameters(properties);
        }

        public Expression Expression
        {
            get { return _parent.Expression; }
        }

        public Type ElementType
        {
            get { return _parent.ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return _parent.Provider; }            
        }

        public QueryableChildSearch<TParent, TChild, TAnotherProperty> With<TAnotherProperty>(params Expression<Func<TChild, TAnotherProperty>>[] properties)
        {
            return new QueryableChildSearch<TParent, TChild, TAnotherProperty>(this._parent, this._childProperties, properties, this._completeExpression, this._childParameter);            
        }

        public QueryableChildStringSearch<TParent, TChild> With(params Expression<Func<TChild, string>>[] properties)
        {
            return new QueryableChildStringSearch<TParent, TChild>(this._parent, this._childProperties, properties, this._completeExpression, this._childParameter);            
        }

        protected void AppendExpression(Expression equalToExpression)
        {
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, equalToExpression);
        }

        private Expression<Func<TChild, TProperty>>[] AlignParameters(Expression<Func<TChild, TProperty>>[] properties)
        {
            var swappedProperties = new List<Expression<Func<TChild, TProperty>>>();
            foreach (var property in properties)
            {
                var swappedProperty = SwapExpressionVisitor.Swap(property, property.Parameters.Single(), this._childParameter);
                swappedProperties.Add(swappedProperty);
            }
            this.Properties = swappedProperties.ToArray();
            return swappedProperties.ToArray();
        }

        public IEnumerator<TParent> GetEnumerator()
        {
            return this.UpdatedSource().GetEnumerator();
        }

        private IQueryable<TParent> UpdatedSource()
        {
            if (this._completeExpression == null)
            {
                return this._parent;
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
    }
}