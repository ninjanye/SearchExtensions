using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.ContainsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EndsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.StartsWithExpressionBuilder;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableChildStringSearch<TParent, TChild> : EnumerableChildSearchBase<TParent, TChild, string>
    {
        private readonly SearchOptions _searchOptions = new SearchOptions();

        public EnumerableChildStringSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, string>>[] properties)
            : base(parent, childProperties, properties, null, null)
        {
        }

        public EnumerableChildStringSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, string>>[] properties, Expression currentExpression, ParameterExpression childParameter) 
            : base(parent, childProperties, properties, currentExpression, childParameter)
        {
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are equal to any of the supplied <paramref name="values">value</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public EnumerableChildStringSearch<TParent, TChild> EqualTo(params string[] values)
        {
            var equalToExpression = EnumerableEqualsExpressionBuilder.Build(Properties, values, _searchOptions);
            AppendExpression(equalToExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableChildStringSearch<TParent, TChild> Containing(params string[] terms)
        {
            var validSearchTerms = terms.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray();
            if (validSearchTerms.Any())
            {
                var containingExpression = EnumerableContainsExpressionBuilder.Build(Properties, validSearchTerms, _searchOptions);
                AppendExpression(containingExpression);
            }
            return this;
        }

        /// <summary>
        /// Retrieve items where all of the defined terms are contained 
        /// within any of the defined properties
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableChildStringSearch<TParent, TChild> ContainingAll(params string[] terms)
        {
            for (int i = 0; i < terms.Length; i++)
            {
                Containing(terms[i]);
            }

            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties starts 
        /// with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableChildStringSearch<TParent, TChild> StartsWith(params string[] terms)
        {
            Expression startsWithExpression = EnumerableStartsWithExpressionBuilder.Build(Properties, terms, _searchOptions);
            AppendExpression(startsWithExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties starts 
        /// with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableChildStringSearch<TParent, TChild> EndsWith(params string[] terms)
        {
            Expression endsWithExpression = EnumerableEndsWithExpressionBuilder.Build(Properties, terms, _searchOptions);
            AppendExpression(endsWithExpression);
            return this;
        }

        /// <summary>
        /// Set the culture used for string comparisons
        /// </summary>
        public EnumerableChildStringSearch<TParent, TChild> SetCulture(StringComparison comparisonType)
        {
            _searchOptions.ComparisonType = comparisonType;
            return this;
        }

        /// <summary>
        /// Alter the matching algorithm between matching any occurence 
        /// and matching whole words only
        /// </summary>
        public EnumerableChildStringSearch<TParent, TChild> Matching(SearchType searchType)
        {
            _searchOptions.SearchType = searchType;
            return this;
        }
    }
}