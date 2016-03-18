using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.ContainsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EndsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.StartsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Models;
using NinjaNye.SearchExtensions.Validation;

namespace NinjaNye.SearchExtensions
{
    public class QueryableStringSearch<T> : QueryableSearchBase<T, string>
    {
        private IList<string> _containingSearchTerms = new List<string>();
        private IList<string> _startsWithSearchTerms = new List<string>();
        private SearchType _searchType;

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
            var validSearchTerms = terms.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray();
            _containingSearchTerms = _containingSearchTerms.Union(validSearchTerms).ToList();

            var orExpression = QueryableContainsExpressionBuilder.Build(this.Properties, validSearchTerms, _searchType);
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
            foreach (var propertyToSearch in this.Properties)
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
            this._startsWithSearchTerms = _startsWithSearchTerms.Union(terms).ToList();
            Expression completeExpression = null;
            foreach (var propertyToSearch in this.Properties)
            {
                var startsWithExpression = QueryableStartsWithExpressionBuilder.Build(propertyToSearch, terms, _searchType);
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
            var completeExpression = QueryableStartsWithExpressionBuilder.Build(this.Properties, propertiesToSearch, _searchType);
            this.BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// ends with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableStringSearch<T> EndsWith(params string[] terms)
        {
            var endsWithExpression = QueryableEndsWithExpressionBuilder.Build(this.Properties, terms, _searchType);
            this.BuildExpression(endsWithExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// ends with any of the defined terms
        /// </summary>
        /// <param name="propertiesToSearchFor">properties defining the terms to search for</param>
        public QueryableStringSearch<T> EndsWith(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            var propertiesToSearch = propertiesToSearchFor.Select(AlignParameter).ToArray();
            var endsWithExpression = QueryableEndsWithExpressionBuilder.Build(this.Properties, propertiesToSearch, this._searchType);
            this.BuildExpression(endsWithExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// are equal to any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableStringSearch<T> EqualTo(params string[] terms)
        {
            Expression completeExpression = QueryableEqualsExpressionBuilder.Build(Properties, terms);
            this.BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined search properties 
        /// are equal to any of the properties supplied
        /// </summary>
        /// <param name="propertiesToSearchFor">Properties to match against</param>
        public QueryableStringSearch<T> EqualTo(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearch in this.Properties)
            {
                var alignedProperties = propertiesToSearchFor.Select(AlignParameter).ToArray();
                var isEqualExpression = QueryableEqualsExpressionBuilder.Build(propertyToSearch, alignedProperties);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
            }

            this.BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// are equal to any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        [Obsolete("This method has been renamed.  Please use .EqualTo() instead.")]
        public QueryableStringSearch<T> IsEqual(params string[] terms)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearch in this.Properties)
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
        [Obsolete("This method has been renamed.  Please use .EqualTo() instead.")]
        public QueryableStringSearch<T> IsEqual(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearch in this.Properties)
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
                       .Search(this.Properties).Soundex(terms);
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
            var searchTerms = this._containingSearchTerms.Any() ? this._containingSearchTerms : this._startsWithSearchTerms;
            foreach (var propertyToSearch in this.Properties)
            {
                var nullSafeExpression = this.BuildNullSafeExpression(propertyToSearch);
                for (int j = 0; j < searchTerms.Count; j++)
                {
                    var searchTerm = searchTerms[j];
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

        public QueryableStringSearch<T> Matching(SearchType searchType)
        {
            _searchType = searchType;
            return this;
        }
    }
}