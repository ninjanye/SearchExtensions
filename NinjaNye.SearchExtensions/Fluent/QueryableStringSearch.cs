using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;
using NinjaNye.SearchExtensions.Validation;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions.Fluent
{
    public class QueryableStringSearch<T> : IQueryable<T>
    {
        private bool expressionUpdated;
        private Expression completeExpression;
        private IQueryable<T> source;
        private readonly Expression<Func<T, string>>[] stringProperties;
        private readonly ParameterExpression firstParameter;
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

            if (!terms.Any() || !stringProperties.Any())
            {
                return null;
            }

            var validSearchTerms = terms.Where(s => !String.IsNullOrWhiteSpace(s)).ToList();
            if (!validSearchTerms.Any())
            {
                return null;
            }

            Expression orExpression = null;
            var singleParameter = stringProperties[0].Parameters.Single();

            Expression notNullExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        singleParameter);

                if (nullCheck)
                {
                    var propertyNotNullExpression = ExpressionHelper.BuildNotNullExpression(swappedParamExpression);
                    notNullExpression = ExpressionHelper.JoinOrExpression(notNullExpression, propertyNotNullExpression);                    
                }

                foreach (var searchTerm in validSearchTerms)
                {
                    ConstantExpression searchTermExpression = Expression.Constant(searchTerm);
                    Expression comparisonExpression = DbExpressionHelper.BuildContainsExpression(swappedParamExpression, searchTermExpression);
                    orExpression = ExpressionHelper.JoinOrExpression(orExpression, comparisonExpression);
                }
            }

            var jointExpression = ExpressionHelper.JoinAndAlsoExpression(notNullExpression, orExpression);

            this.AppendExpression(jointExpression);
            return this;
        }

        /// <summary>
        /// Only items where any property starts with the specified term
        /// </summary>
        /// <param name="terms">Term to search for</param>
        public QueryableStringSearch<T> StartsWith(params string[] terms)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in this.stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);

                var startsWithExpression = DbExpressionHelper.BuildStartsWithExpression(swappedParamExpression, terms, nullCheck);
                fullExpression = fullExpression == null ? startsWithExpression
                                                        : Expression.OrElse(fullExpression, startsWithExpression);
            }
            this.AppendExpression(fullExpression);
            return this;
        }

        //TODO: INVESTIGATE LINQ TO SQL SUPPORT
        ///// <summary>
        ///// Only items where any property ends with the specified term
        ///// </summary>
        ///// <param name="terms">Term to search for</param>
        //public QueryableStringSearch<T> EndsWith(params string[] terms)
        //{
        //    Expression fullExpression = null;
        //    foreach (var stringProperty in this.stringProperties)
        //    {
        //        var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
        //                                                                stringProperty.Parameters.Single(),
        //                                                                this.firstParameter);
        //        var endsWithExpression = DbExpressionHelper.BuildEndsWithExpression(swappedParamExpression, terms, nullCheck);
        //        fullExpression = ExpressionHelper.JoinOrExpression(fullExpression, endsWithExpression);
        //    }
        //    this.AppendExpression(fullExpression);
        //    return this;
        //}


        /// <summary>
        /// Only items where any property equals the specified term
        /// </summary>
        /// <param name="terms">Terms to search for</param>
        public QueryableStringSearch<T> IsEqual(params string[] terms)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in this.stringProperties)
            {
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

        private void AppendExpression(Expression expressionToJoin)
        {
            expressionUpdated = false;
            completeExpression = ExpressionHelper.JoinAndAlsoExpression(completeExpression, expressionToJoin);
        }

        private void UpdateSource()
        {
            if (this.completeExpression == null || expressionUpdated)
            {
                return;
            }

            expressionUpdated = true;
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
                return source.Expression;
            }
        }

        public Type ElementType { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }
}