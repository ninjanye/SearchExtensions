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
        protected readonly IEnumerable<TParent> Parent;
        protected readonly Expression<Func<TParent, IEnumerable<TChild>>>[] ChildProperties;
        protected readonly Expression<Func<TChild, TProperty>>[] Properties;
        protected readonly ParameterExpression ParentParameter;
        protected readonly ParameterExpression ChildParameter = Expression.Parameter(typeof(TChild), "child");
        protected Expression CompleteExpression;

        protected EnumerableChildSearchBase(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties)
            :this(parent, childProperties, properties, null, null)
        {
        }

        protected EnumerableChildSearchBase(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties, Expression completeExpression, ParameterExpression childParameter)
        {
            this.Parent = parent;
            this.ParentParameter = childProperties[0].Parameters[0];
            if (childParameter != null) this.ChildParameter = childParameter;
            this.CompleteExpression = completeExpression;

            this.ChildProperties = this.AlignParameters(childProperties, this.ParentParameter).ToArray();
            this.Properties = this.AlignParameters(properties, this.ChildParameter).ToArray();
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
            if (this.CompleteExpression == null)
            {
                return this.Parent;
            }

            var anyMethodInfo = ExpressionMethods.AnyQueryableMethod.MakeGenericMethod(typeof(TChild));
            Expression finalExpression = null;
            foreach (var childProperty in this.ChildProperties)
            {
                var anyExpression = Expression.Lambda<Func<TChild, bool>>(this.CompleteExpression, this.ChildParameter);
                var anyChild = Expression.Call(null, anyMethodInfo, childProperty.Body, anyExpression);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, anyChild);
            }

            var final = Expression.Lambda<Func<TParent, bool>>(finalExpression, this.ParentParameter).Compile();
            return this.Parent.Where(final);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}