using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    public abstract class SearchBase<TSource, TType> where TSource : IEnumerable<TType>
    {
        protected TSource Source;
        protected Expression CompleteExpression;
        protected readonly Expression<Func<TType, string>>[] StringProperties;
        protected readonly ParameterExpression FirstParameter;

        protected SearchBase(TSource source, Expression<Func<TType, string>>[] stringProperties)
        {
            this.Source = source;
            var firstProperty = stringProperties.FirstOrDefault();
            if (firstProperty != null)
            {
                this.FirstParameter = firstProperty.Parameters[0];
            }
            this.StringProperties = stringProperties.Select(AlignParameter).ToArray();
        }

        /// <summary>
        /// Appends expressionToJoin to CompleteExpression using an Expression.AndAlso join
        /// </summary>
        protected virtual void BuildExpression(Expression expressionToJoin)
        {
            this.CompleteExpression = ExpressionHelper.JoinAndAlsoExpression(this.CompleteExpression, expressionToJoin);
        }

        /// <summary>
        /// Align the lambda parameter to that of the first string property
        /// </summary>
        protected Expression<TProperty> AlignParameter<TProperty>(Expression<TProperty> lambda)
        {            
            return SwapExpressionVisitor.Swap(lambda, lambda.Parameters.Single(), this.FirstParameter);
        }
    }
}