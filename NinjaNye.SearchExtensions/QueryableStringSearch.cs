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
    public class QueryableStringSearch<T> : IQueryable<T>
    {
        private bool expressionUpdated;
        private Expression completeExpression;
        private IQueryable<T> source;
        private readonly Expression<Func<T, string>>[] stringProperties;
        private readonly ParameterExpression firstParameter;
        private readonly IList<string> searchTerms = new List<string>();
        private bool nullCheck;

        public QueryableStringSearch(IQueryable<T> source, Expression<Func<T, string>>[] stringProperties)
        {
            this.source = source;
            this.ElementType = source.ElementType;
            this.Provider = source.Provider;
            this.stringProperties = stringProperties;
            var firstProperty = stringProperties.FirstOrDefault();
            if (firstProperty != null)
            {
                this.firstParameter = firstProperty.Parameters.FirstOrDefault();
            }
        }

        /// <summary>
        /// Only items where any property contains search term
        /// </summary>
        /// <param name="terms">Term to search for</param>
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
                this.searchTerms.Add(validSearchTerm);
            }

            Expression orExpression = null;
            var singleParameter = this.stringProperties[0].Parameters.Single();

            for (int i = 0; i < this.stringProperties.Length; i++)
            {
                var stringProperty = this.stringProperties[i];
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        singleParameter);

                Expression propertyNotNullExpression = null;
                if (this.nullCheck)
                {
                    propertyNotNullExpression = ExpressionHelper.BuildNotNullExpression(swappedParamExpression);
                }

                for (int j = 0; j < validSearchTerms.Count; j++)
                {
                    var searchTerm = validSearchTerms[j];
                    ConstantExpression searchTermExpression = Expression.Constant(searchTerm);
                    Expression comparisonExpression = DbExpressionHelper.BuildContainsExpression(swappedParamExpression, searchTermExpression);
                    if (this.nullCheck)
                    {
                        var nullCheckedExpression = ExpressionHelper.JoinAndAlsoExpression(propertyNotNullExpression, comparisonExpression);
                        orExpression = ExpressionHelper.JoinOrExpression(orExpression, nullCheckedExpression);
                    }
                    else
                    {
                        orExpression = ExpressionHelper.JoinOrExpression(orExpression, comparisonExpression);
                    }
                }
            }

            this.AppendExpression(orExpression);
            return this;
        }

        /// <summary>
        /// Only items where any property starts with the specified term
        /// </summary>
        /// <param name="terms">Term to search for</param>
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
        /// Only items where any property equals the specified term
        /// </summary>
        /// <param name="terms">Terms to search for</param>
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

        public QueryableStringSearch<T> CheckForNull(bool checkNull = true)
        {
            this.nullCheck = checkNull;
            return this;
        }
 
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

                for (int j = 0; j < this.searchTerms.Count; j++)
                {
                    var searchTerm = this.searchTerms[j];
                    var hitCountExpression = EnumerableExpressionHelper.CalculateHitCount(nullSafeExpression, searchTerm);
                    combinedHitExpression = ExpressionHelper.AddExpressions(combinedHitExpression, hitCountExpression);
                }
            }

            var rankedInitExpression = EnumerableExpressionHelper.ConstructRankedResult<T>(combinedHitExpression, this.firstParameter);
            var selectExpression = Expression.Lambda<Func<T, Ranked<T>>>(rankedInitExpression, this.firstParameter);
            return this.Select(selectExpression);
        } 

        private void AppendExpression(Expression expressionToJoin)
        {
            this.expressionUpdated = false;
            this.completeExpression = ExpressionHelper.JoinAndAlsoExpression(this.completeExpression, expressionToJoin);
        }

        private void UpdateSource()
        {
            if (this.completeExpression == null || this.expressionUpdated)
            {
                return;
            }

            this.expressionUpdated = true;
            var finalExpression = Expression.Lambda<Func<T, bool>>(this.completeExpression, this.firstParameter);
            this.source = this.source.Where(finalExpression);
        }

        public IEnumerator<T> GetEnumerator()
        {
            this.UpdateSource();
            return this.source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Expression Expression
        {
            get
            {
                this.UpdateSource();
                return this.source.Expression;
            }
        }

        public Type ElementType { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }
}