using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Levenshtein;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableLevenshteinSearch<T> : EnumerableSearchBase<T, string>
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
        public IEnumerable<ILevenshteinDistance<T>> ComparedTo(Expression<Func<T, string>> stringProperty)
        {
            var sourceProperty = Properties[0];
            var targetProperty = AlignParameter(stringProperty);

            var levenshteinDistanceExpression = EnumerableExpressionHelper.CalculateLevenshteinDistance(sourceProperty, targetProperty);
            var buildExpression = EnumerableExpressionHelper.ConstructLevenshteinResult<T>(levenshteinDistanceExpression, FirstParameter);
            var selectExpression = Expression.Lambda<Func<T, LevenshteinDistance<T>>>(buildExpression, FirstParameter).Compile();
            var convertedSource = Source.Select(selectExpression.Invoke);
            return new EnumerableLevenshteinCompare<ILevenshteinDistance<T>>(convertedSource);
        }

        /// <summary>
        /// Define the term with which to compare string properties to
        /// </summary>
        /// <param name="term">The term to perform the Levenshtein distance against</param>
        /// <returns></returns>
        public IEnumerable<ILevenshteinDistance<T>> ComparedTo(string term)
        {
            var stringProperty = Properties[0];
            var levenshteinDistanceExpression = EnumerableExpressionHelper.CalculateLevenshteinDistance(stringProperty, term);
            var buildExpression = EnumerableExpressionHelper.ConstructLevenshteinResult<T>(levenshteinDistanceExpression, FirstParameter);
            var selectExpression = Expression.Lambda<Func<T, LevenshteinDistance<T>>>(buildExpression, FirstParameter).Compile();
            var convertedSource = Source.Select(selectExpression.Invoke);
            return new EnumerableLevenshteinCompare<ILevenshteinDistance<T>>(convertedSource);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            throw new InvalidOperationException("Please use .ComparedTo() method to provide a value with which to build a Levenshtein Distance.");
        }
    }
}