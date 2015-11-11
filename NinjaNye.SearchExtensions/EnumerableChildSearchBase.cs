using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableChildSearchBase<TParent, TChild, TProperty> : IEnumerable<TParent>
    {
        private readonly IEnumerable<TParent> _parent;
        private readonly Expression<Func<TParent, IEnumerable<TChild>>>[] _childProperties;
        protected readonly Expression<Func<TChild, TProperty>>[] Properties;
        private readonly ParameterExpression _parentParameter;
        private readonly ParameterExpression _childParameter = Expression.Parameter(typeof(TChild), "child");
        private Expression _completeExpression;

        protected EnumerableChildSearchBase(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties, Expression completeExpression, ParameterExpression childParameter)
        {
            this._parent = parent;
            this._parentParameter = childProperties[0].Parameters[0];
            if (childParameter != null) this._childParameter = childParameter;
            this._completeExpression = completeExpression;

            this._childProperties = this.AlignParameters(childProperties, this._parentParameter).ToArray();
            this.Properties = this.AlignParameters(properties, this._childParameter).ToArray();
        }

        /// <summary>
        /// Begin a search against a new property.
        /// </summary>
        public EnumerableChildSearch<TParent, TChild, TAnotherProperty> With<TAnotherProperty>(params Expression<Func<TChild, TAnotherProperty>>[] properties)
        {
            return new EnumerableChildSearch<TParent, TChild, TAnotherProperty>(this._parent, this._childProperties, properties, this._completeExpression, this._childParameter);
        }

        /// <summary>
        /// Begin a search against a new string property.
        /// </summary>
        public EnumerableChildStringSearch<TParent, TChild> With(params Expression<Func<TChild, string>>[] properties)
        {
            return new EnumerableChildStringSearch<TParent, TChild>(this._parent, this._childProperties, properties, this._completeExpression, this._childParameter);
        }

        private IEnumerable<Expression<Func<TSource, TResult>>> AlignParameters<TSource, TResult>(Expression<Func<TSource, TResult>>[] properties, ParameterExpression parameterExpression)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                yield return SwapExpressionVisitor.Swap(property, property.Parameters.Single(), parameterExpression);
            }
        }

        public IEnumerator<TParent> GetEnumerator()
        {
            return this.UpdatedSource().GetEnumerator();
        }

        private IEnumerable<TParent> UpdatedSource()
        {
            if (this._completeExpression == null)
            {
                return this._parent;
            }

            var anyMethodInfo = ExpressionMethods.AnyQueryableMethod.MakeGenericMethod(typeof(TChild));
            Expression finalExpression = null;
            foreach (var childProperty in this._childProperties)
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

        protected void JoinExpression(Expression equalToExpression)
        {
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, equalToExpression);
        }
    }
}