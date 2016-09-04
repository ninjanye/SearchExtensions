using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    public abstract class SearchBase<TSource, TType, TPropertyType> where TSource : IEnumerable<TType>
    {
        protected TSource Source;
        protected Expression CompleteExpression;
        protected readonly Expression<Func<TType, TPropertyType>>[] Properties;
        protected readonly ParameterExpression FirstParameter;

        protected SearchBase(TSource source, Expression<Func<TType, TPropertyType>>[] properties)
        {
            Source = source;
            var firstProperty = properties.FirstOrDefault();
            if (firstProperty != null)
            {
                FirstParameter = firstProperty.Parameters[0];
            }
            Properties = properties.Select(AlignParameter).ToArray();
        }

        /// <summary>
        /// Appends expressionToJoin to CompleteExpression using an Expression.AndAlso join
        /// </summary>
        protected virtual void BuildExpression(Expression expressionToJoin)
        {
            CompleteExpression = ExpressionHelper.JoinAndAlsoExpression(CompleteExpression, expressionToJoin);
        }

        /// <summary>
        /// Align the lambda parameter to that of the first string property
        /// </summary>
        protected Expression<TProperty> AlignParameter<TProperty>(Expression<TProperty> lambda)
        {
            return SwapExpressionVisitor.Swap(lambda, lambda.Parameters.Single(), FirstParameter);
        }
    }

}