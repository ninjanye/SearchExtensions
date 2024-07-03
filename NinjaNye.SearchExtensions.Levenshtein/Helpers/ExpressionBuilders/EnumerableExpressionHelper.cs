using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
// ReSharper disable once RedundantUsingDirective
using System.Reflection;

namespace NinjaNye.SearchExtensions.Levenshtein.Helpers.ExpressionBuilders
{
    internal static class EnumerableExpressionHelper
    {
        /// <summary>
        /// Constructs a ranked result of type T
        /// </summary>
        /// <param name="distanceExpressions">Expression representing how to calculated distance</param>
        /// <param name="parameterExpression">property parameter</param>
        /// <returns>Expression equivalent to: new LevenshteinDistance{ Distance = [hitCountExpression], Item = x }</returns>
        public static Expression ConstructLevenshteinResult<T>(IEnumerable<Expression> distanceExpressions,
                                                               ParameterExpression parameterExpression)
        {
            var distanceType = typeof(LevenshteinDistance<T>);
#if NET45
            var constructor = distanceType.GetConstructor(new [] {typeof(T), typeof(int[])});
#else
            var constructor = distanceType.GetTypeInfo().DeclaredConstructors.First();
#endif
            var distanceArray = Expression.NewArrayInit(typeof(int), distanceExpressions);
            var distanceCtor = Expression.New(constructor, parameterExpression, distanceArray);
            return distanceCtor;
        }

        /// <summary>
        /// Calculates the Levenshtein distance between a given property and a search term
        /// </summary>
        /// <returns>Expression equivalent to: LevenshteinProcessor.LevenshteinDistance([stringProperty], [searchTerm])</returns>
        public static IEnumerable<Expression> CalculateLevenshteinDistances<T>(Expression<Func<T, string>>[] stringProperties, params string[] searchTerms)
        {
            var searchTermExpressions = searchTerms.Select(Expression.Constant).ToList();
            return from searchTerm in searchTermExpressions
                   from sourceProperty in stringProperties
                   select Expression.Call(ExpressionMethods.LevensteinDistanceMethod, sourceProperty.Body, searchTerm);
        }

        /// <summary>
        /// Calculates the Levenshtein distance between a given property and a search term
        /// </summary>
        /// <returns>Expression equivalent to: LevenshteinProcessor.LevenshteinDistance([stringProperty], [searchTerm])</returns>
        public static IEnumerable<Expression> CalculateLevenshteinDistance<T>(
            Expression<Func<T, string>>[] sourceProperties, params Expression<Func<T, string>>[] targetProperties)
        {
            return from targetProperty in targetProperties
                   from sourceProperty in sourceProperties
                   select Expression.Call(ExpressionMethods.LevensteinDistanceMethod, sourceProperty.Body, targetProperty.Body);
        }
    }
}