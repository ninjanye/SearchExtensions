using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.ContainsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EndsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.StartsWithExpressionBuilder;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableChildStringSearch<TParent, TChild> : IEnumerable<TParent>
    {
        private readonly IEnumerable<TParent> _parent;
        private readonly Expression<Func<TParent, IEnumerable<TChild>>>[] _childProperties;
        private readonly Expression<Func<TChild, string>>[] _properties;
        private Expression _completeExpression;
        private readonly ParameterExpression _childParameter = Expression.Parameter(typeof(TChild), "child");
        private readonly SearchOptions _searchOptions;

        public EnumerableChildStringSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, string>>[] properties)
        {
            this._parent = parent;
            this._childProperties = childProperties;

            var swappedProperties = new List<Expression<Func<TChild, string>>>();
            foreach (var property in properties)
            {
                var swappedProperty = SwapExpressionVisitor.Swap(property, property.Parameters.Single(), this._childParameter);
                swappedProperties.Add(swappedProperty);
            }

            _properties = swappedProperties.ToArray();
            _searchOptions = new SearchOptions();
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are equal to any of the supplied <paramref name="values">value</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public EnumerableChildStringSearch<TParent, TChild> EqualTo(params string[] values)
        {
            var equalToExpression = EnumerableEqualsExpressionBuilder.Build(this._properties, values, this._searchOptions);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, equalToExpression);
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
                var containingExpression = EnumerableContainsExpressionBuilder.Build(_properties, validSearchTerms, _searchOptions);
                this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, containingExpression);
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
                this.Containing(terms[i]);
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
            Expression startsWithExpression = EnumerableStartsWithExpressionBuilder.Build(_properties, terms, _searchOptions);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, startsWithExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties starts 
        /// with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public EnumerableChildStringSearch<TParent, TChild> EndsWith(params string[] terms)
        {
            Expression endsWithExpression = EnumerableEndsWithExpressionBuilder.Build(_properties, terms, _searchOptions);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, endsWithExpression);
            return this;
        }

        public IEnumerator<TParent> GetEnumerator()
        {
            return this._completeExpression == null ? this._parent.GetEnumerator() 
                                                    : this.GetEnueratorWithoutChecks();
        }

        private IEnumerator<TParent> GetEnueratorWithoutChecks()
        {
            var finalExpression = Expression.Lambda<Func<TChild, bool>>(this._completeExpression, this._childParameter).Compile();
            foreach (var parent in this._parent)
            {
                foreach (var childProperty in _childProperties)
                {
                    var children = childProperty.Compile().Invoke(parent);
                    var isMatch = children.Any(c => finalExpression.Invoke(c));
                    if (isMatch)
                    {
                        yield return parent;
                        break;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public EnumerableChildStringSearch<TParent, TChild> SetCulture(StringComparison comparisonType)
        {
            _searchOptions.ComparisonType = comparisonType;
            return this;
        }

        public EnumerableChildStringSearch<TParent, TChild> Matching(SearchType searchType)
        {
            _searchOptions.SearchType = searchType;
            return this;
        }
    }
}