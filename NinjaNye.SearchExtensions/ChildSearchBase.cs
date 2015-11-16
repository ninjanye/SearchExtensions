using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    public class ChildSearchBase<TParent, TChild, TProperty>
    {
        protected Expression<Func<TParent, IEnumerable<TChild>>>[] _childProperties;
        protected Expression<Func<TChild, TProperty>>[] Properties;
        private ParameterExpression _parentParameter;
        protected ParameterExpression _childParameter = Expression.Parameter(typeof(TChild), "child");
        protected Expression _completeExpression;

        protected ChildSearchBase(Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties, Expression completeExpression, ParameterExpression childParameter)
        {
            this._parentParameter = childProperties[0].Parameters[0];
            if (childParameter != null) this._childParameter = childParameter;

            this._childProperties = this.AlignParameters(childProperties, this._parentParameter).ToArray();
            this.Properties = this.AlignParameters(properties, this._childParameter).ToArray();
            this._completeExpression = completeExpression;
        }

        private IEnumerable<Expression<Func<TSource, TResult>>> AlignParameters<TSource, TResult>(Expression<Func<TSource, TResult>>[] properties, ParameterExpression parameterExpression)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                yield return SwapExpressionVisitor.Swap(property, property.Parameters.Single(), parameterExpression);
            }
        }

        protected void AppendExpression(Expression equalToExpression)
        {
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, equalToExpression);
        }

        protected Expression<Func<TParent, bool>> BuildFinalExpression()
        {
            if (_completeExpression == null)
            {
                return Expression.Lambda<Func<TParent, bool>>(Expression.Constant(true), _parentParameter);
            }
            var anyMethodInfo = ExpressionMethods.AnyQueryableMethod.MakeGenericMethod(typeof (TChild));
            Expression finalExpression = null;
            foreach (var childProperty in this._childProperties)
            {
                var anyExpression = Expression.Lambda<Func<TChild, bool>>(this._completeExpression, this._childParameter);
                var anyChild = Expression.Call(null, anyMethodInfo, childProperty.Body, anyExpression);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, anyChild);
            }

            var final = Expression.Lambda<Func<TParent, bool>>(finalExpression, this._parentParameter);
            return final;
        }
    }
}