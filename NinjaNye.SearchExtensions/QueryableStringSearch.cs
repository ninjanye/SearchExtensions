using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.ContainsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.StartsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Models;
using NinjaNye.SearchExtensions.Validation;

namespace NinjaNye.SearchExtensions
{
    public sealed class QueryableStringSearch<T> : QueryableStringSearchBase<T>
    {
        private readonly IList<string> containingSearchTerms = new List<string>();

        public QueryableStringSearch(IQueryable<T> source, Expression<Func<T, string>>[] stringProperties) 
            : base(source, stringProperties)
        {
        }

        /// <summary>
        /// Retrieve items where any of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableStringSearch<T> Containing(params string[] terms)
        {
            Ensure.ArgumentNotNull(terms, "terms");
            var validSearchTerms = terms.Where(s => !String.IsNullOrWhiteSpace(s)).ToList();

            foreach (var validSearchTerm in validSearchTerms)
            {
                this.containingSearchTerms.Add(validSearchTerm);
            }

            Expression orExpression = null;
            foreach (var propertyToSearch in StringProperties)
            {
                foreach (var validSearchTerm in validSearchTerms)
                {
                    ConstantExpression searchTermExpression = Expression.Constant(validSearchTerm);
                    Expression comparisonExpression =  QueryableContainsExpressionBuilder.Build(propertyToSearch, searchTermExpression);
                    orExpression = ExpressionHelper.JoinOrExpression(orExpression, comparisonExpression);
                }
            }

            this.BuildExpression(orExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableStringSearch<T> Containing(params Expression<Func<T, string>>[] terms)
        {
            Expression finalExpression = null;
            foreach (var propertyToSearch in StringProperties)
            {
                var searchTermProperty = AlignParameter(terms[0]);
                Expression comparisonExpression = QueryableContainsExpressionBuilder.Build(propertyToSearch, searchTermProperty);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, comparisonExpression);
            }
            this.BuildExpression(finalExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where *all* of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableStringSearch<T> ContainingAll(params string[] terms)
        {
            var result = this;
            foreach (var term in terms)
            {
                result = result.Containing(term);
            }

            return result;
        }

        /// <summary>
        /// Retrieve items where *all* of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="propertiesToSearchFor">Term or terms to search for</param>
        public QueryableStringSearch<T> ContainingAll(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            var result = this;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                result = result.Containing(propertyToSearchFor);   
            }
            return result;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// starts with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableStringSearch<T> StartsWith(params string[] terms)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearch in StringProperties)
            {
                var startsWithExpression = QueryableStartsWithExpressionBuilder.Build(propertyToSearch, terms);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            this.BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// starts with any of the defined terms
        /// </summary>
        /// <param name="propertiesToSearchFor">properties defining the terms to search for</param>
        public QueryableStringSearch<T> StartsWith(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            var propertiesToSearch = propertiesToSearchFor.Select(AlignParameter).ToArray();
            Expression completeExpression = null;
            foreach (var stringProperty in StringProperties)
            {
                var startsWithExpression = QueryableStartsWithExpressionBuilder.Build(stringProperty, propertiesToSearch);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }

            this.BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// are equal to any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableStringSearch<T> IsEqual(params string[] terms)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearch in StringProperties)
            {
                var isEqualExpression = QueryableEqualsExpressionBuilder.Build(propertyToSearch, terms);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }
            this.BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined search properties 
        /// are equal to any of the properties supplied
        /// </summary>
        /// <param name="propertiesToSearchFor">Properties to match against</param>
        public QueryableStringSearch<T> IsEqual(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearch in StringProperties)
            {
                var alignedProperties = propertiesToSearchFor.Select(AlignParameter).ToArray();
                var isEqualExpression = QueryableEqualsExpressionBuilder.Build(propertyToSearch, alignedProperties);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }

            this.BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Returns Enumerable of records that match the Soundex code for
        /// any of the given terms across any of the defined properties
        /// </summary>
        /// <param name="terms">terms to search for</param>
        /// <returns>Enumerable of records where Soundex matches</returns>
        public IEnumerable<T> Soundex(params string[] terms)
        {
            var firstCharacters = terms.Select(t => t.GetFirstCharacter())
                                       .Distinct()
                                       .ToArray();
            return this.StartsWith(firstCharacters).AsEnumerable()
                       .Search(StringProperties).Soundex(terms);
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
            foreach (var propertyToSearch in StringProperties)
            {
                var nullSafeExpression = this.BuildNullSafeExpression(propertyToSearch);
                for (int j = 0; j < this.containingSearchTerms.Count; j++)
                {
                    var searchTerm = this.containingSearchTerms[j];
                    var hitCountExpression = EnumerableExpressionHelper.CalculateHitCount(nullSafeExpression, searchTerm);
                    combinedHitExpression = ExpressionHelper.AddExpressions(combinedHitExpression, hitCountExpression);
                }
            }

            var rankedInitExpression = EnumerableExpressionHelper.ConstructRankedResult<T>(combinedHitExpression, this.FirstParameter);
            var selectExpression = Expression.Lambda<Func<T, Ranked<T>>>(rankedInitExpression, this.FirstParameter);
            return this.Select(selectExpression);
        }

        private Expression<Func<T, string>> BuildNullSafeExpression(Expression<Func<T, string>> propertyToSearch)
        {
            var nullSafeProperty = Expression.Coalesce(propertyToSearch.Body, ExpressionMethods.EmptyStringExpression);
            var nullSafeExpression = Expression.Lambda<Func<T, string>>(nullSafeProperty, this.FirstParameter);
            return nullSafeExpression;
        }
    }
}