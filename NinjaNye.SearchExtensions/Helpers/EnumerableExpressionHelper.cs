using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.Enumerable;

namespace NinjaNye.SearchExtensions.Helpers
{
    internal static class EnumerableExpressionHelper
    {
        /// <summary>
        /// Build a 'contains' expression for a searching a property that 
        /// contains the value of another string property
        /// </summary>
        public static Expression BuildContainsExpression<T>(Expression<Func<T, string>> propertyToSearch, Expression<Func<T, string>> propertyToSearchFor)
        {
            var isNotNullExpression = ExpressionHelper.BuildNotNullExpression(propertyToSearch);
            var searchForIsNotNullExpression = ExpressionHelper.BuildNotNullExpression(propertyToSearchFor);
            var containsExpression = Expression.Call(propertyToSearch.Body, ExpressionMethods.StringContainsMethod, propertyToSearchFor.Body);
            var fullNotNullExpression = Expression.AndAlso(isNotNullExpression, searchForIsNotNullExpression);
            return Expression.AndAlso(fullNotNullExpression, containsExpression);
        }

        /// <summary>
        /// Build a 'indexof() >= 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildIndexOfGreaterThanMinusOneExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            if (nullCheck)
            {
                var coalesceExpression = Expression.Coalesce(stringProperty.Body, ExpressionMethods.EmptyStringExpression);
                var nullCheckExpresion = Expression.Call(coalesceExpression, ExpressionMethods.IndexOfMethod, searchTermExpression, stringComparisonExpression);
                return Expression.GreaterThanOrEqual(nullCheckExpresion, ExpressionMethods.ZeroConstantExpression);                
            }

            var indexOfCallExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.IndexOfMethod, searchTermExpression, stringComparisonExpression);
            return Expression.GreaterThanOrEqual(indexOfCallExpresion, ExpressionMethods.ZeroConstantExpression);
        }

        /// <summary>
        /// Build an 'soundexCodes.Contains(soundex(value))' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildSoundsLikeExpression<T>(Expression<Func<T, string>> stringProperty, IList<string> soundexCodes)
        {
            var soundexCallExpresion = Expression.Call(ExpressionMethods.SoundexMethod, stringProperty.Body);
            var soundexCodesExpression = Expression.Constant(soundexCodes);
            var containsExpression = Expression.Call(soundexCodesExpression, ExpressionMethods.StringListContainsMethod, soundexCallExpresion);
            var trueExpression = Expression.Constant(true);
            return Expression.Equal(containsExpression, trueExpression);
        }

        /// <summary>
        /// Build an 'soundexCodes.Contains(reverseSoundex(value))' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildReverseSoundexLikeExpression<T>(Expression<Func<T, string>> stringProperty, IList<string> soundexCodes)
        {
            var soundexCallExpresion = Expression.Call(ExpressionMethods.ReverseSoundexMethod, stringProperty.Body);
            var soundexCodesExpression = Expression.Constant(soundexCodes);
            var containsExpression = Expression.Call(soundexCodesExpression, ExpressionMethods.StringListContainsMethod, soundexCallExpresion);
            var trueExpression = Expression.Constant(true);
            return Expression.Equal(containsExpression, trueExpression);
        }


        /// <summary>
        /// Constructs a ranked result of type T
        /// </summary>
        /// <param name="hitCountExpression">Expression representing how to calculated search hits</param>
        /// <param name="parameterExpression">property parameter</param>
        /// <returns>Expression equivalent to: new Ranked{ Hits = [hitCountExpression], Item = x }</returns>
        public static Expression ConstructRankedResult<T>(Expression hitCountExpression,
                                                          ParameterExpression parameterExpression)
        {
            var rankedType = typeof(Ranked<T>);
            var rankedCtor = Expression.New(rankedType);
            PropertyInfo hitProperty = rankedType.GetProperty("Hits");
            PropertyInfo itemProperty = rankedType.GetProperty("Item");
            var hitValueAssignment = Expression.Bind(hitProperty, hitCountExpression);
            var itemValueAssignment = Expression.Bind(itemProperty, parameterExpression);
            return Expression.MemberInit(rankedCtor, hitValueAssignment, itemValueAssignment);
        }

        /// <summary>
        /// Constructs a ranked result of type T
        /// </summary>
        /// <param name="distanceExpression">Expression representing how to calculated distance</param>
        /// <param name="parameterExpression">property parameter</param>
        /// <returns>Expression equivalent to: new LevenshteinDistance{ Distance = [hitCountExpression], Item = x }</returns>
        public static Expression ConstructLevenshteinResult<T>(Expression distanceExpression,
                                                               ParameterExpression parameterExpression)
        {
            var distanceType = typeof(LevenshteinDistance<T>);
            var distanceCtor = Expression.New(distanceType);
            PropertyInfo distanceProperty = distanceType.GetProperty("Distance");
            PropertyInfo itemProperty = distanceType.GetProperty("Item");
            var distanceValueAssignment = Expression.Bind(distanceProperty, distanceExpression);
            var itemValueAssignment = Expression.Bind(itemProperty, parameterExpression);
            return Expression.MemberInit(distanceCtor, distanceValueAssignment, itemValueAssignment);
        }

        /// <summary>
        /// Calculates how many search hits occured for a given property
        /// </summary>
        /// <param name="stringProperty">string property to analyse</param>
        /// <param name="searchTerm">search term to count</param>
        /// <returns>Expression equivalent to: [property].Length - ([property].Replace([searchTerm], "").Length) / [searchTerm].Length</returns>
        public static Expression CalculateHitCount<T>(Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            Expression searchTermExpression = Expression.Constant(searchTerm);
            Expression searchTermLengthExpression = Expression.Constant(searchTerm.Length);
            MemberExpression lengthExpression = Expression.Property(stringProperty.Body, ExpressionMethods.StringLengthProperty);
            var replaceExpression = Expression.Call(stringProperty.Body, ExpressionMethods.ReplaceMethod,
                                                    searchTermExpression, ExpressionMethods.EmptyStringExpression);
            var replacedLengthExpression = Expression.Property(replaceExpression, ExpressionMethods.StringLengthProperty);
            var characterDiffExpression = Expression.Subtract(lengthExpression, replacedLengthExpression);
            var hitCountExpression = Expression.Divide(characterDiffExpression, searchTermLengthExpression);
            return hitCountExpression;
        }

        /// <summary>
        /// Calculates how many search hits occured for a given property
        /// </summary>
        /// <returns>Expression equivalent to: [property].Length - ([property].Replace([searchTerm], "").Length) / [searchTerm].Length</returns>
        public static Expression CalculateHitCount<T>(Expression<Func<T, string>> stringProperty, string searchTerm, StringComparison stringComparison)
        {
            Expression searchTermExpression = Expression.Constant(searchTerm);
            Expression searchTermLengthExpression = Expression.Constant(searchTerm.Length);
            Expression stringComparisonExpression = Expression.Constant(stringComparison);
            MemberExpression lengthExpression = Expression.Property(stringProperty.Body, ExpressionMethods.StringLengthProperty);
            var replaceExpression = Expression.Call(ExpressionMethods.CustomReplaceMethod, stringProperty.Body, searchTermExpression, ExpressionMethods.EmptyStringExpression, stringComparisonExpression);
            var replacedLengthExpression = Expression.Property(replaceExpression, ExpressionMethods.StringLengthProperty);
            var characterDiffExpression = Expression.Subtract(lengthExpression, replacedLengthExpression);
            var hitCountExpression = Expression.Divide(characterDiffExpression, searchTermLengthExpression);
            return hitCountExpression;
        }

        /// <summary>
        /// Calculates the Levenshtein distance between a given property and a search term
        /// </summary>
        /// <returns>Expression equivalent to: LevenshteinProcessor.LevensteinDistance([stringProperty], [searchTerm])</returns>
        public static Expression CalculateLevenshteinDistance<T>(Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            Expression searchTermExpression = Expression.Constant(searchTerm);
            return Expression.Call(ExpressionMethods.LevensteinDistanceMethod, stringProperty.Body, searchTermExpression);
        }

        /// <summary>
        /// Calculates the Levenshtein distance between a given property and a search term
        /// </summary>
        /// <returns>Expression equivalent to: LevenshteinProcessor.LevensteinDistance([stringProperty], [searchTerm])</returns>
        public static Expression CalculateLevenshteinDistance<T>(Expression<Func<T, string>> sourceProperty, Expression<Func<T, string>> targetProperty)
        {
            return Expression.Call(ExpressionMethods.LevensteinDistanceMethod, sourceProperty.Body, targetProperty.Body);
        }


        /// <summary>
        /// Builds an array of expressions that map to every TType property on an object
        /// </summary>
        /// <typeparam name="TSource">Type of object to retrieve properties from</typeparam>
        /// <typeparam name="TType">The type of property to locate</typeparam>
        /// <returns>An array of expressions pointing to each TType property</returns>
        public static Expression<Func<TSource, TType>>[] GetProperties<TSource, TType>()
        {
            var parameter = Expression.Parameter(typeof(TSource));
            var stringProperties = typeof(TSource).GetProperties()
                                                  .Where(property => property.CanRead
                                                                  && property.PropertyType == typeof(TType));

            var result = new List<Expression<Func<TSource, TType>>>();
            foreach (var property in stringProperties)
            {
                Expression body = Expression.Property(parameter, property);
                result.Add(Expression.Lambda<Func<TSource, TType>>(body, parameter));
            }
            return result.ToArray();
        }
    }
}