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
    public class QueryableChildStringSearch<TParent, TChild> : QueryableChildSearchBase<TParent, TChild, string>
    {
        private readonly SearchOptions _searchOptions = new SearchOptions();

        public QueryableChildStringSearch(IQueryable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, string>>[] properties)
            : base(parent, childProperties, properties, null, null)
        {
        }

        public QueryableChildStringSearch(IQueryable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, string>>[] properties, Expression completeExpression, ParameterExpression childParameter)
            : base(parent, childProperties, properties, completeExpression, childParameter)
        {
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are equal to any of the supplied <paramref name="values">values</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public QueryableChildStringSearch<TParent, TChild> EqualTo(params string[] values)
        {
            AppendExpression(ExpressionBuilder.EqualsExpression(Properties, values));
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// contain any of the supplied <paramref name="values">values</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public QueryableChildStringSearch<TParent, TChild> Containing(params string[] values)
        {
            AppendExpression(QueryableContainsExpressionBuilder.Build(Properties, values, _searchOptions.SearchType));
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// contain all of the supplied <paramref name="values">values</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public QueryableChildStringSearch<TParent, TChild> ContainingAll(params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Containing(values[i]);
            }

            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties starts 
        /// with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableChildStringSearch<TParent, TChild> StartsWith(params string[] terms)
        {
            AppendExpression(QueryableStartsWithExpressionBuilder.Build(Properties, terms, _searchOptions.SearchType));
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties ends 
        /// with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableChildStringSearch<TParent, TChild> EndsWith(params string[] terms)
        {
            AppendExpression(QueryableEndsWithExpressionBuilder.Build(Properties, terms, _searchOptions.SearchType));
            return this;
        }

        public QueryableChildStringSearch<TParent, TChild> Matching(SearchType searchType)
        {
            _searchOptions.SearchType = searchType;
            return this;
        }
    }
}