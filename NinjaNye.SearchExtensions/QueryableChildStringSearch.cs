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
    public class QueryableChildStringSearch<TParent, TChild> : IQueryable<TParent>
    {
        private readonly IQueryable<TParent> _parent;
        private readonly Expression<Func<TParent, IEnumerable<TChild>>>[] _childProperties;
        private Expression<Func<TChild, string>>[] _properties;
        private readonly ParameterExpression _childParameter = Expression.Parameter(typeof(TChild), "child");
        private Expression _completeExpression;
        private readonly SearchOptions _searchOptions = new SearchOptions();

        public QueryableChildStringSearch(IQueryable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, string>>[] properties)
        {
            //this.Expression = _parent.Expression;
            //this.ElementType = _parent.ElementType;
            //this.Provider = _parent.Provider;

            this._parent = parent;
            this._childProperties = childProperties;

            _properties = this.AlignParameters(properties);
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// are equal to any of the supplied <paramref name="values">values</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public QueryableChildStringSearch<TParent, TChild> EqualTo(params string[] values)
        {
            var equalToExpression = ExpressionBuilder.EqualsExpression(this._properties, values);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, equalToExpression);
            return this;
        }

        /// <summary>
        /// Retrieves items where any of the defined properties
        /// contain any of the supplied <paramref name="values">values</paramref> 
        /// </summary>
        /// <param name="values">A collection of values to match upon</param>
        public QueryableChildStringSearch<TParent, TChild> Containing(params string[] values)
        {
            var containsExpression = QueryableContainsExpressionBuilder.Build(this._properties, values, _searchOptions.SearchType);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, containsExpression);
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
                this.Containing(values[i]);
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
            Expression startsWithExpression = QueryableStartsWithExpressionBuilder.Build(_properties, terms, _searchOptions.SearchType);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, startsWithExpression);
            return this;
        }

        /// <summary>
        /// Retrieve items where any of the defined properties ends 
        /// with any of the defined search terms
        /// </summary>
        /// <param name="terms">Term or terms to search for</param>
        public QueryableChildStringSearch<TParent, TChild> EndsWith(params string[] terms)
        {
            Expression startsWithExpression = QueryableEndsWithExpressionBuilder.Build(_properties, terms, _searchOptions.SearchType);
            this._completeExpression = ExpressionHelper.JoinAndAlsoExpression(this._completeExpression, startsWithExpression);
            return this;
        }

        public QueryableChildStringSearch<TParent, TChild> Matching(SearchType searchType)
        {
            _searchOptions.SearchType = searchType;
            return this;
        }

        private Expression<Func<TChild, string>>[] AlignParameters(Expression<Func<TChild, string>>[] properties)
        {
            var swappedProperties = new List<Expression<Func<TChild, string>>>();
            foreach (var property in properties)
            {
                var swappedProperty = SwapExpressionVisitor.Swap(property, property.Parameters.Single(), this._childParameter);
                swappedProperties.Add(swappedProperty);
            }
            this._properties = swappedProperties.ToArray();
            return swappedProperties.ToArray();
        }

        public IEnumerator<TParent> GetEnumerator()
        {
            return this._completeExpression == null ? this._parent.GetEnumerator() 
                                                    : this.BuildEnumerator();
        }

        private IEnumerator<TParent> BuildEnumerator()
        {
            var childProperty = this._childProperties[0];

            var methodInfo = ExpressionMethods.AnyQueryableMethod.MakeGenericMethod(typeof (TChild));
            var anyExpression = Expression.Lambda<Func<TChild, bool>>(this._completeExpression, this._childParameter);

            var anyChild = Expression.Call(null, methodInfo, childProperty.Body, anyExpression);
            var finalExpression = Expression.Lambda<Func<TParent, bool>>(anyChild, childProperty.Parameters[0]);

            return this._parent.Where(finalExpression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Expression Expression { get; private set; }
        public Type ElementType { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }
}