using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;
using NinjaNye.SearchExtensions.Validation;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableStringSearch<T> : EnumerableSearchBase<T>
    {
        private readonly IList<string> containingSearchTerms = new List<string>();
        private StringComparison comparisonType;

        public EnumerableStringSearch(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties) 
            : base(source, stringProperties)
        {
            this.SetCulture(StringComparison.CurrentCulture);
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
        /// Retrieve items where any of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableStringSearch<T> Containing(params string[] terms)
        {
            Ensure.ArgumentNotNull(terms, "searchTerms");

            if (!terms.Any() || !this.StringProperties.Any())
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
            var stringComparisonExpression = Expression.Constant(this.comparisonType);

            foreach (var propertyToSearch in StringProperties)
            {
                foreach (var validSearchTerm in validSearchTerms)
                {
                    ConstantExpression searchTermExpression = Expression.Constant(validSearchTerm);
                    var indexOfExpression = EnumerableExpressionHelper.BuildIndexOfGreaterThanMinusOneExpression(propertyToSearch, searchTermExpression, stringComparisonExpression);
                    orExpression = ExpressionHelper.JoinOrExpression(orExpression, indexOfExpression);
                }
            }

            this.BuildExpression(orExpression);
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
                foreach (var propertyToSearch in StringProperties)
                {
                    var containsExpression = EnumerableExpressionHelper.BuildContainsExpression(propertyToSearch, containsProperty);
                    finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, containsExpression);
                }
            }

            this.BuildExpression(finalExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where all of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableStringSearch<T> ContainingAll(params string[] terms)
        {
            var result = this;
            for (int i = 0; i < terms.Length; i++)
            {
                result = result.Containing(terms[i]);
            }

            return result;
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
            Expression fullExpression = null;
            foreach (var propertyToSearch in StringProperties)
            {
                var startsWithExpression = EnumerableExpressionHelper.BuildStartsWithExpression(propertyToSearch, terms, this.comparisonType, false);
                fullExpression = ExpressionHelper.JoinOrExpression(fullExpression, startsWithExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties starts 
        /// with any of the defined properties
        /// </summary>
        /// <param name="propertiesToSearchFor">Term or terms to search for</param>
        public EnumerableStringSearch<T> StartsWith(params Expression<Func<T, string>>[] propertiesToSearchFor)
        {
            var propertiesToSearch = propertiesToSearchFor.Select(AlignParameter).ToList();
            Expression completeExpression = null;
            foreach (var stringProperty in StringProperties)
            {
                var startsWithExpression = EnumerableExpressionHelper.BuildStartsWithExpression(stringProperty, propertiesToSearch, this.comparisonType);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            this.BuildExpression(completeExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// ends with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableStringSearch<T> EndsWith(params string[] terms)
        {
            Expression fullExpression = null;
            foreach (var propertyToSearch in StringProperties)
            {
                var endsWithExpression = EnumerableExpressionHelper.BuildEndsWithExpression(propertyToSearch, terms, this.comparisonType, false);
                fullExpression = ExpressionHelper.JoinOrExpression(fullExpression, endsWithExpression);
            }
            this.BuildExpression(fullExpression);
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
            Expression finalExpression = null;
            foreach (var stringProperty in StringProperties)
            {
                var endsWithExpression = EnumerableExpressionHelper.BuildEndsWithExpression(stringProperty, propertiesToSearch, comparisonType);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, endsWithExpression);
            }
            this.BuildExpression(finalExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties 
        /// is equal to any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableStringSearch<T> IsEqual(params string[] terms)
        {
            Expression fullExpression = null;
            foreach (var propertyToSearch in StringProperties)
            {
                var isEqualExpression = EnumerableExpressionHelper.BuildEqualsExpression(propertyToSearch, terms, this.comparisonType);
                fullExpression = ExpressionHelper.JoinOrExpression(fullExpression, isEqualExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Return records that match the Soundex code for any of the given terms
        /// </summary>
        /// <param name="terms">terms to search for</param>
        /// <returns>Enumerable of records where Soundex matches</returns>
        public IEnumerable<T> Soundex(params string[] terms)
        {
            return new EnumerableSoundexSearch<T>(Source, StringProperties).American(terms);
        }

        /// <summary>
        /// Return records that match the Soundex code for any of the given terms
        /// </summary>
        /// <param name="terms">terms to search for</param>
        /// <returns>Enumerable of records where Soundex matches</returns>
        public IEnumerable<T> ReverseSoundex(params string[] terms)
        {
            return new EnumerableSoundexSearch<T>(Source, StringProperties).Reverse(terms);
        }

        /// <summary>
        /// Rank the filtered items based on the matched occurences
        /// </summary>
        /// <returns>
        /// Enumerable of ranked items.  Each item will contain 
        /// the amount of hits found across the defined properties
        /// </returns>
        public IEnumerable<IRanked<T>> ToRanked()
        {
            Expression combinedHitExpression = null;
            ConstantExpression emptyStringExpression = Expression.Constant("");
            foreach (var propertyToSearch in StringProperties)
            {
                for (int j = 0; j < this.containingSearchTerms.Count; j++)
                {
                    var searchTerm = this.containingSearchTerms[j];
                    var nullSafeProperty = Expression.Coalesce(propertyToSearch.Body, emptyStringExpression);
                    var nullSafeExpresion = Expression.Lambda<Func<T, string>>(nullSafeProperty, this.FirstParameter);
                    var hitCountExpression = EnumerableExpressionHelper.CalculateHitCount(nullSafeExpresion, searchTerm, this.comparisonType);
                    combinedHitExpression = ExpressionHelper.AddExpressions(combinedHitExpression, hitCountExpression);
                }
            }

            var rankedInitExpression = EnumerableExpressionHelper.ConstructRankedResult<T>(combinedHitExpression, this.FirstParameter);
            var selectExpression = Expression.Lambda<Func<T, Ranked<T>>>(rankedInitExpression, this.FirstParameter).Compile();
            return this.Select(selectExpression.Invoke);
        }
    }
}