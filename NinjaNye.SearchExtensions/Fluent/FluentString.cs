using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Fluent
{
    public class FluentString<T> : IEnumerable<T>
    {
        private IEnumerable<T> source;
        private Expression completeExpression;
        private readonly Expression<Func<T, string>>[] stringProperties;
        private readonly ParameterExpression firstParameter;

        public FluentString(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties)
        {
            this.source = source;
            this.stringProperties = stringProperties;
            var firstProperty = stringProperties.FirstOrDefault();
            if (firstProperty != null)
            {
                this.firstParameter = firstProperty.Parameters[0];
            }
        }

        /// <summary>
        /// Only items where any property contains search term
        /// </summary>
        /// <param name="terms">Term to search for</param>
        public FluentString<T> Containing(params string[] terms)
        {
            this.BuildExpression(source.SearchExpression(terms, stringProperties));
            return this;
        }

        /// <summary>
        /// Only items where any property starts with the specified term
        /// </summary>
        /// <param name="terms">Term to search for</param>
        public FluentString<T> StartsWith(params string[] terms)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in this.stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);

                var startsWithExpression = ExpressionHelper.BuildStartsWithExpression(swappedParamExpression, terms, false);
                fullExpression = fullExpression == null ? startsWithExpression 
                                     : Expression.OrElse(fullExpression, startsWithExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Only items where any property ends with the specified term
        /// </summary>
        /// <param name="terms">Term to search for</param>
        public FluentString<T> EndsWith(params string[] terms)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in this.stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);
                var endsWithExpression = ExpressionHelper.BuildEndsWithExpression(swappedParamExpression, terms, false);
                fullExpression = fullExpression == null ? endsWithExpression
                                     : Expression.OrElse(fullExpression, endsWithExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Only items where any property equals the specified term
        /// </summary>
        /// <param name="term">Term to search for</param>
        public FluentString<T> IsEqual(string term)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in this.stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);
                var termExpression = Expression.Constant(term);
                var isEqualExpression = ExpressionHelper.BuildEqualsExpression(swappedParamExpression, termExpression);
                fullExpression = fullExpression == null ? isEqualExpression
                                     : Expression.OrElse(fullExpression, isEqualExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.completeExpression != null)
            {
                var finalExpression = Expression.Lambda<Func<T, bool>>(this.completeExpression, this.firstParameter).Compile();
                this.source = this.source.Where(finalExpression);
            }
            return this.source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void BuildExpression(Expression expressionToJoin)
        {
            if (this.completeExpression == null)
            {
                this.completeExpression = expressionToJoin;
            }
            else
            {
                this.completeExpression = Expression.AndAlso(this.completeExpression, expressionToJoin);
            }
        }
    }
}