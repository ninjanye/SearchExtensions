using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.ContainsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EndsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.StartsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Models;
using NinjaNye.SearchExtensions.Validation;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableStringSearch<T> : EnumerableSearchBase<T, string>
    {
        private readonly SearchTermCollection _searchTerms = new SearchTermCollection();
        private readonly SearchOptions _searchOptions;

        public EnumerableStringSearch(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties)
            : base(source, stringProperties)
        {
            _searchOptions = new SearchOptions();
            SetCulture(StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Set culture for string comparison
        /// </summary>
        public EnumerableStringSearch<T> SetCulture(StringComparison type)
        {
            _searchOptions.ComparisonType = type;
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableStringSearch<T> Containing(params string[] terms)
        {
            Ensure.ArgumentNotNull(terms, "terms");

            if (!terms.Any() || !Properties.Any())
            {
                return this;
            }


            var validSearchTerms = terms.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray();
            if (!validSearchTerms.Any())
            {
                return this;
            }

            _searchTerms.Add(validSearchTerms);
            Expression orExpression = EnumerableContainsExpressionBuilder.Build(Properties, validSearchTerms, _searchOptions);

            BuildExpression(orExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="propertiesToSearchFor">Property or properties to search against</param>
        public EnumerableStringSearch<T> Containing(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            Expression finalExpression = null;
            foreach (var stringProperty in propertiesToSearchFor)
            {
                var containsProperty = AlignParameter(stringProperty);
                foreach (var propertyToSearch in Properties)
                {
                    var containsExpression = EnumerableContainsExpressionBuilder.Build(propertyToSearch, containsProperty);
                    finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, containsExpression);
                }
            }

            BuildExpression(finalExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where all of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableStringSearch<T> ContainingAll(params string[] terms)
        {
            for (int i = 0; i < terms.Length; i++)
            {
                Containing(terms[i]);
            }

            return this;
        }

        /// <summary>
        /// Retrieve items where all of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="propertiesToSearchFor">Properties containg text to search for</param>
        public EnumerableStringSearch<T> ContainingAll(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            var result = this;
            foreach (var propertyToSearch in propertiesToSearchFor)
            {
                result = result.Containing(propertyToSearch);
            }
            return result;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties starts 
        /// with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableStringSearch<T> StartsWith(params string[] terms)
        {
            _searchTerms.Add(terms);
            Expression fullExpression = EnumerableStartsWithExpressionBuilder.Build(Properties, terms, _searchOptions);
            BuildExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties starts 
        /// with any of the defined properties
        /// </summary>
        /// <param name="propertiesToSearchFor">Term or terms to search for</param>
        public EnumerableStringSearch<T> StartsWith(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            var propertiesToSearch = propertiesToSearchFor.Select(AlignParameter).ToArray();
            Expression completeExpression = EnumerableStartsWithExpressionBuilder.Build(Properties, propertiesToSearch, _searchOptions);
            BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// ends with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableStringSearch<T> EndsWith(params string[] terms)
        {
            _searchTerms.Add(terms);
            Expression fullExpression = EnumerableEndsWithExpressionBuilder.Build(Properties, terms, _searchOptions);
            BuildExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// ends with any of the defined search terms
        /// </summary>
        /// <param name="propertiesToSearchFor">Properties containing terms to search for</param>
        public EnumerableStringSearch<T> EndsWith(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            var propertiesToSearch = propertiesToSearchFor.Select(AlignParameter).ToArray();
            Expression finalExpression = EnumerableEndsWithExpressionBuilder.Build(Properties, propertiesToSearch, _searchOptions);
            BuildExpression(finalExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// are equal to any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableStringSearch<T> EqualTo(params string[] terms)
        {
            Expression fullExpression = EnumerableEqualsExpressionBuilder.Build(Properties, terms, _searchOptions);
            BuildExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// are equal to any of the defined properties to search for
        /// </summary>
        /// <param name="propertiesToSearchFor">Properties to search for</param>
        public EnumerableStringSearch<T> EqualTo(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            propertiesToSearchFor = propertiesToSearchFor.Select(AlignParameter).ToArray();
            Expression completeExpression = EnumerableEqualsExpressionBuilder.Build(Properties, propertiesToSearchFor, _searchOptions);
            BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// are equal to any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        [Obsolete("This method has been renamed.  Please use .EqualTo() instead.")]
        public EnumerableStringSearch<T> IsEqual(params string[] terms)
        {
            var fullExpression = EnumerableEqualsExpressionBuilder.Build(Properties, terms, _searchOptions);
            BuildExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// are equal to any of the defined properties to search for
        /// </summary>
        /// <param name="propertiesToSearchFor">Properties to search for</param>
        [Obsolete("This method has been renamed.  Please use .EqualTo() instead.")]
        public EnumerableStringSearch<T> IsEqual(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            propertiesToSearchFor = propertiesToSearchFor.Select(AlignParameter).ToArray();
            var completeExpression = EnumerableEqualsExpressionBuilder.Build(Properties, propertiesToSearchFor, _searchOptions);
            BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Return records that match the Soundex code for any of the given terms
        /// </summary>
        /// <param name="terms">terms to search for</param>
        /// <returns>Enumerable of records where Soundex matches</returns>
        public IEnumerable<T> Soundex(params string[] terms)
        {
            return new EnumerableSoundexSearch<T>(Source, Properties).American(terms);
        }

        /// <summary>
        /// Return records that match the Soundex code for any of the given terms
        /// </summary>
        /// <param name="terms">terms to search for</param>
        /// <returns>Enumerable of records where Soundex matches</returns>
        public IEnumerable<T> ReverseSoundex(params string[] terms)
        {
            return new EnumerableSoundexSearch<T>(Source, Properties).Reverse(terms);
        }

        /// <summary>
        /// Rank the filtered items based on the matched occurences
        /// </summary>
        /// <returns>
        /// Enumerable of ranked items.  Each item will contain 
        /// the amount of hits found across the defined properties
        /// </returns>
        public IEnumerable<IRanked<T>> ToRanked(RankedType type = RankedType.Default)
        {
            Expression combinedHitExpression = null;
            foreach (var propertyToSearch in Properties)
            {
                for (int j = 0; j < _searchTerms.Count; j++)
                {
                    var searchTerm = _searchTerms[j];
                    var nullSafeExpresion = BuildNullSafeExpresion(propertyToSearch);
                    var hitCountExpression = type == RankedType.Default ? EnumerableExpressionHelper.CalculateHitCount(nullSafeExpresion, searchTerm, _searchOptions)
                                                                        : EnumerableExpressionHelper.CalculateHitCount_LeftWeighted(nullSafeExpresion, searchTerm, _searchOptions);
                    combinedHitExpression = ExpressionHelper.AddExpressions(combinedHitExpression, hitCountExpression);
                }
            }

            var rankedInitExpression = EnumerableExpressionHelper.ConstructRankedResult<T>(combinedHitExpression, FirstParameter);
            var selectExpression = Expression.Lambda<Func<T, Ranked<T>>>(rankedInitExpression, FirstParameter).Compile();
            return this.Select(selectExpression.Invoke);
        }

        private Expression<Func<T, string>> BuildNullSafeExpresion(Expression<Func<T, string>> propertyToSearch)
        {
            var nullSafeProperty = Expression.Coalesce(propertyToSearch.Body, ExpressionMethods.EmptyStringExpression);
            var nullSafeExpresion = Expression.Lambda<Func<T, string>>(nullSafeProperty, FirstParameter);
            return nullSafeExpresion;
        }

        public EnumerableStringSearch<T> Matching(SearchType searchType)
        {
            _searchOptions.SearchType = searchType;
            return this;
        }
    }
}