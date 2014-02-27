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
        private StringComparison comparisonType;

        public FluentString(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties)
        {
            this.source = source;
            this.stringProperties = stringProperties;
            this.SetCulture(StringComparison.CurrentCulture);
            var firstProperty = stringProperties.FirstOrDefault();
            if (firstProperty != null)
            {
                this.firstParameter = firstProperty.Parameters[0];
            }
        }

        /// <summary>
        /// Set culture for string comparison
        /// </summary>
        public FluentString<T> SetCulture(StringComparison type)
        {
            this.comparisonType = type;
            return this;
        } 

        /// <summary>
        /// Only items where any property contains search term
        /// </summary>
        /// <param name="terms">Term to search for</param>
        public FluentString<T> Containing(params string[] terms)
        {
            var searchExpression = source.SearchExpression(terms, stringProperties, comparisonType);
            this.BuildExpression(searchExpression);
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

                var startsWithExpression = ExpressionHelper.BuildStartsWithExpression(swappedParamExpression, terms, comparisonType, false);
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
                var endsWithExpression = ExpressionHelper.BuildEndsWithExpression(swappedParamExpression, terms, comparisonType, false);
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
        public FluentString<T> IsEqual(params string[] terms)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in this.stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);
                var isEqualExpression = ExpressionHelper.BuildEqualsExpression(swappedParamExpression, terms, comparisonType);
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