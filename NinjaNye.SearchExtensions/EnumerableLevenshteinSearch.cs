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

        public EnumerableLevenshteinSearch(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties)
            : base(source, stringProperties)
        {
        }

        /// <summary>
        /// Define the term with which to compare string properties to
        /// </summary>
        /// <param name="stringProperties">The properties to perform the Levenshtein distance against</param>
        /// <returns></returns>
        public IEnumerable<ILevenshteinDistance<T>> ComparedTo(params Expression<Func<T, string>>[] stringProperties)
        {
            var targetProperties = stringProperties.Select(AlignParameter).ToArray();

            var levenshteinDistanceExpression = EnumerableExpressionHelper.CalculateLevenshteinDistance(Properties, targetProperties);
            var buildExpression = EnumerableExpressionHelper.ConstructLevenshteinResult<T>(levenshteinDistanceExpression, FirstParameter);
            var selectExpression = Expression.Lambda<Func<T, LevenshteinDistance<T>>>(buildExpression, FirstParameter).Compile();
            var convertedSource = Source.Select(selectExpression.Invoke);
            return new EnumerableLevenshteinCompare<ILevenshteinDistance<T>>(convertedSource);
        }

        /// <summary>
        /// Define the term with which to compare string properties to
        /// </summary>
        /// <param name="terms">The terms to perform the Levenshtein distance against</param>
        /// <returns></returns>
        public IEnumerable<ILevenshteinDistance<T>> ComparedTo(params string[] terms)
        {
            var levenshteinDistanceExpression = EnumerableExpressionHelper.CalculateLevenshteinDistances(Properties, terms);

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