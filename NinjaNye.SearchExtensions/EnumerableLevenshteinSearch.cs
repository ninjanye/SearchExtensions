using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableLevenshteinSearch<T> : EnumerableSearchBase<T>
    {
        public EnumerableLevenshteinSearch(IEnumerable<T> source, Expression<Func<T, string>> stringProperty)
            : base(source, new[]{stringProperty})
        {
        }

        /// <summary>
        /// Define the term with which to compare string properties to
        /// </summary>
        /// <param name="stringProperty">The property to perform the Levenshtein distance against</param>
        /// <returns></returns>
        public EnumerableLevenshteinCompare<ILevenshteinDistance<T>> ComparedTo(Expression<Func<T, string>> stringProperty)
        {
            var sourceProperty = this.StringProperties[0];
            var targetProperty = SwapExpressionVisitor.Swap(stringProperty,
                                                            stringProperty.Parameters.Single(),
                                                            this.FirstParameter);

            var levenshteinDistanceExpression = EnumerableExpressionHelper.CalculateLevenshteinDistance(sourceProperty, targetProperty);
            var buildExpression = EnumerableExpressionHelper.ConstructLevenshteinResult<T>(levenshteinDistanceExpression, this.FirstParameter);
            var selectExpression = Expression.Lambda<Func<T, LevenshteinDistance<T>>>(buildExpression, this.FirstParameter).Compile();
            var convertedSource = this.Source.Select(selectExpression.Invoke);
            return new EnumerableLevenshteinCompare<ILevenshteinDistance<T>>(convertedSource);
        }

        /// <summary>
        /// Define the term with which to compare string properties to
        /// </summary>
        /// <param name="term">The term to perform the Levenshtein distance against</param>
        /// <returns></returns>
        public EnumerableLevenshteinCompare<ILevenshteinDistance<T>> ComparedTo(string term)
        {
            var stringProperty = this.StringProperties[0];
            var levenshteinDistanceExpression = EnumerableExpressionHelper.CalculateLevenshteinDistance(stringProperty, term);
            var buildExpression = EnumerableExpressionHelper.ConstructLevenshteinResult<T>(levenshteinDistanceExpression, this.FirstParameter);
            var selectExpression = Expression.Lambda<Func<T, LevenshteinDistance<T>>>(buildExpression, this.FirstParameter).Compile();
            var convertedSource = this.Source.Select(selectExpression.Invoke);
            return new EnumerableLevenshteinCompare<ILevenshteinDistance<T>>(convertedSource);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            throw new InvalidOperationException("Please use .ComparedTo() method to provide a value with which to build a Levenshtein Distance.");
        }
    }
}