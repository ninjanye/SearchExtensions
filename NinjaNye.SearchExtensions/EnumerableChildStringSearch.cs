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
        private readonly ParameterExpression _parentParameter;
        private readonly ParameterExpression _childParameter = Expression.Parameter(typeof(TChild), "child");
        private readonly SearchOptions _searchOptions;

        public EnumerableChildStringSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, string>>[] properties)
            : this(parent, childProperties, properties, null, null)
        {
        }

        public EnumerableChildStringSearch(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, string>>[] properties, Expression currentExpression, ParameterExpression childParameter)
        {
            this._parent = parent;
            this._parentParameter = childProperties[0].Parameters[0];
            _searchOptions = new SearchOptions();

            _completeExpression = currentExpression;
            if (childParameter != null) this._childParameter = childParameter;

            _childProperties = this.AlignParameters(childProperties, this._parentParameter).ToArray();
            _properties = this.AlignParameters(properties, this._childParameter).ToArray();
        }

        private IEnumerable<Expression<Func<TSource, TResult>>> AlignParameters<TSource, TResult>(Expression<Func<TSource, TResult>>[] properties, ParameterExpression parameterExpression)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                yield return SwapExpressionVisitor.Swap(property, property.Parameters.Single(), parameterExpression);
            }
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

        public EnumerableChildSearch<TParent, TChild, TAnotherProperty> With<TAnotherProperty>(params Expression<Func<TChild, TAnotherProperty>>[] properties)
        {
            return new EnumerableChildSearch<TParent, TChild, TAnotherProperty>(_parent, _childProperties, properties, _completeExpression, _childParameter);
        }

        public EnumerableChildStringSearch<TParent, TChild> With(params Expression<Func<TChild, string>>[] properties)
        {
            return new EnumerableChildStringSearch<TParent, TChild>(_parent, _childProperties, properties, _completeExpression, _childParameter);
        }

        public IEnumerator<TParent> GetEnumerator()
        {
            return this.UpdatedSource().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private IEnumerable<TParent> UpdatedSource()
        {
            if (_completeExpression == null)
            {
                return _parent;
            }

            var anyMethodInfo = ExpressionMethods.AnyQueryableMethod.MakeGenericMethod(typeof(TChild));
            Expression finalExpression = null;
            foreach (var childProperty in _childProperties)
            {
                var anyExpression = Expression.Lambda<Func<TChild, bool>>(this._completeExpression, this._childParameter);
                var anyChild = Expression.Call(null, anyMethodInfo, childProperty.Body, anyExpression);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, anyChild);
            }

            var final = Expression.Lambda<Func<TParent, bool>>(finalExpression, this._parentParameter).Compile();
            return this._parent.Where(final);
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