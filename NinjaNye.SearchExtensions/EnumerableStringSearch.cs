using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;
using NinjaNye.SearchExtensions.Validation;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableStringSearch<T> : IEnumerable<T>
    {
        private IEnumerable<T> source;
        private Expression completeExpression;
        private readonly Expression<Func<T, string>>[] stringProperties;
        private readonly ParameterExpression firstParameter;
        private readonly IList<string> searchTerms = new List<string>();
        private StringComparison comparisonType;

        public EnumerableStringSearch(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties)
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
        public EnumerableStringSearch<T> SetCulture(StringComparison type)
        {
            this.comparisonType = type;
            return this;
        } 

        /// <summary>
        /// Only items where any property contains search term
        /// </summary>
        /// <param name="terms">Term to search for</param>
        public EnumerableStringSearch<T> Containing(params string[] terms)
        {
            Ensure.ArgumentNotNull(terms, "searchTerms");

            if (!terms.Any() || !this.stringProperties.Any())
            {
                return null;
            }

            var validSearchTerms = terms.Where(s => !String.IsNullOrWhiteSpace(s)).ToList();
            if (!validSearchTerms.Any())
            {
                return null;
            }

            foreach (var validSearchTerm in validSearchTerms)
            {
                this.searchTerms.Add(validSearchTerm);
            }

            Expression orExpression = null;
            var singleParameter = this.stringProperties[0].Parameters.Single();
            var stringComparisonExpression = Expression.Constant(this.comparisonType);

            foreach (var stringProperty in this.stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        singleParameter);

                foreach (var searchTerm in validSearchTerms)
                {
                    ConstantExpression searchTermExpression = Expression.Constant(searchTerm);

                    var indexOfExpression = EnumerableExpressionHelper.BuildIndexOfGreaterThanMinusOneExpression(swappedParamExpression, searchTermExpression, stringComparisonExpression);
                    orExpression = ExpressionHelper.JoinOrExpression(orExpression, indexOfExpression);
                }
            }

            this.BuildExpression(orExpression);
            return this;
        }

        /// <summary>
        /// Only items where any property starts with the specified term
        /// </summary>
        /// <param name="terms">Term to search for</param>
        public EnumerableStringSearch<T> StartsWith(params string[] terms)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in this.stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);

                var startsWithExpression = EnumerableExpressionHelper.BuildStartsWithExpression(swappedParamExpression, terms, this.comparisonType, false);
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
        public EnumerableStringSearch<T> EndsWith(params string[] terms)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in this.stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);
                var endsWithExpression = EnumerableExpressionHelper.BuildEndsWithExpression(swappedParamExpression, terms, this.comparisonType, false);
                fullExpression = fullExpression == null ? endsWithExpression
                                                        : Expression.OrElse(fullExpression, endsWithExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Only items where any property equals the specified term
        /// </summary>
        /// <param name="terms">Terms to search for</param>
        public EnumerableStringSearch<T> IsEqual(params string[] terms)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in this.stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);
                var isEqualExpression = EnumerableExpressionHelper.BuildEqualsExpression(swappedParamExpression, terms, this.comparisonType);
                fullExpression = fullExpression == null ? isEqualExpression
                                     : Expression.OrElse(fullExpression, isEqualExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        public IEnumerable<IRanked<T>> ToRanked()
        {
            Expression combinedHitExpression = null;
            ConstantExpression emptyStringExpression = Expression.Constant("");
            foreach (var stringProperty in this.stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);

                foreach (var searchTerm in this.searchTerms)
                {
                    var nullSafeProperty = Expression.Coalesce(swappedParamExpression.Body, emptyStringExpression);
                    var nullSafeExpresion = Expression.Lambda<Func<T, string>>(nullSafeProperty, this.firstParameter);
                    var hitCountExpression = EnumerableExpressionHelper.CalculateHitCount(nullSafeExpresion, searchTerm, this.comparisonType);
                    combinedHitExpression = ExpressionHelper.AddExpressions(combinedHitExpression, hitCountExpression);
                }
            }

            var rankedInitExpression = EnumerableExpressionHelper.ConstructRankedResult<T>(combinedHitExpression, this.firstParameter);
            var selectExpression = Expression.Lambda<Func<T, Ranked<T>>>(rankedInitExpression, this.firstParameter).Compile();
            return this.Select(selectExpression.Invoke);
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