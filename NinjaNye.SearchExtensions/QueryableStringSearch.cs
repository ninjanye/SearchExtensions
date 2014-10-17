using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;
using NinjaNye.SearchExtensions.Validation;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    public class QueryableStringSearch<T> : QueryableStringSearchBase<T>
    {
        private readonly IList<string> containingSearchTerms = new List<string>();

        public QueryableStringSearch(IQueryable<T> source, Expression<Func<T, string>>[] stringProperties) 
            : base(source, stringProperties)
        {
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// contains any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableStringSearch<T> Containing(params string[] terms)
        {
            Ensure.ArgumentNotNull(terms, "terms");

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
                this.containingSearchTerms.Add(validSearchTerm);
            }

            Expression orExpression = null;
            var singleParameter = this.stringProperties[0].Parameters.Single();

            for (int i = 0; i < this.stringProperties.Length; i++)
            {
                var stringProperty = this.stringProperties[i];
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        singleParameter);

                for (int j = 0; j < validSearchTerms.Count; j++)
                {
                    var searchTerm = validSearchTerms[j];
                    ConstantExpression searchTermExpression = Expression.Constant(searchTerm);
                    Expression comparisonExpression = DbExpressionHelper.BuildContainsExpression(swappedParamExpression, searchTermExpression);
                    orExpression = ExpressionHelper.JoinOrExpression(orExpression, comparisonExpression);
                }
            }

            this.AppendExpression(orExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// starts with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableStringSearch<T> StartsWith(params string[] terms)
        {
            Expression fullExpression = null;
            for (int i = 0; i < this.stringProperties.Length; i++)
            {
                var stringProperty = this.stringProperties[i];
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);

                var startsWithExpression = DbExpressionHelper.BuildStartsWithExpression(swappedParamExpression, terms);
                fullExpression = ExpressionHelper.JoinOrExpression(fullExpression, startsWithExpression);
            }
            this.AppendExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// are equal to any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableStringSearch<T> IsEqual(params string[] terms)
        {
            Expression fullExpression = null;
            for (int i = 0; i < this.stringProperties.Length; i++)
            {
                var stringProperty = this.stringProperties[i];
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);
                var isEqualExpression = DbExpressionHelper.BuildEqualsExpression(swappedParamExpression, terms);
                fullExpression = ExpressionHelper.JoinOrExpression(fullExpression, isEqualExpression);
            }
            this.AppendExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Rank the filtered items based on the matched occurences
        /// </summary>
        /// <returns>
        /// Enumerable of ranked items.  Each item will contain 
        /// the amount of hits found across the defined properties
        /// </returns>
        public IQueryable<IRanked<T>> ToRanked()
        {
            Expression combinedHitExpression = null;
            ConstantExpression emptyStringExpression = Expression.Constant("");
            for (int i = 0; i < this.stringProperties.Length; i++)
            {
                var stringProperty = this.stringProperties[i];
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);

                var nullSafeProperty = Expression.Coalesce(swappedParamExpression.Body, emptyStringExpression);
                var nullSafeExpression = Expression.Lambda<Func<T, string>>(nullSafeProperty, this.firstParameter);

                for (int j = 0; j < this.containingSearchTerms.Count; j++)
                {
                    var searchTerm = this.containingSearchTerms[j];
                    var hitCountExpression = EnumerableExpressionHelper.CalculateHitCount(nullSafeExpression, searchTerm);
                    combinedHitExpression = ExpressionHelper.AddExpressions(combinedHitExpression, hitCountExpression);
                }
            }

            var rankedInitExpression = EnumerableExpressionHelper.ConstructRankedResult<T>(combinedHitExpression, this.firstParameter);
            var selectExpression = Expression.Lambda<Func<T, Ranked<T>>>(rankedInitExpression, this.firstParameter);
            return this.Select(selectExpression);
        }
    }
}