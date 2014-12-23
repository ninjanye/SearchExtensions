using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NinjaNye.SearchExtensions.Levenshtein;
using NinjaNye.SearchExtensions.Soundex;

namespace NinjaNye.SearchExtensions.Helpers
{
    internal static class EnumerableExpressionHelper
    {
        static readonly ConstantExpression EmptyStringExpression = Expression.Constant(string.Empty);
        static readonly ConstantExpression NullExpression = Expression.Constant(null);
        static readonly ConstantExpression ZeroConstantExpression = Expression.Constant(0);
        static readonly MethodInfo EqualsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(StringComparison) });
        static readonly MethodInfo IndexOfMethod = typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) });
        static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) });
        static readonly PropertyInfo StringLengthProperty = typeof(string).GetProperty("Length");
        static readonly MethodInfo ReplaceMethod = typeof(string).GetMethod("Replace", new[] { typeof(string), typeof(string) });
        static readonly MethodInfo ContainsMethod = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) });
        static readonly MethodInfo SoundexMethod = typeof(SoundexProcessor).GetMethod("ToSoundex");
        static readonly MethodInfo ReverseSoundexMethod = typeof(SoundexProcessor).GetMethod("ToReverseSoundex");
        static readonly MethodInfo LevensteinDistanceMethod = typeof(LevenshteinProcessor).GetMethod("LevenshteinDistance");
        static readonly MethodInfo CustomReplaceMethod = typeof(StringExtensionHelper).GetMethod("Replace");
        static readonly MethodInfo QuickReverseMethod = typeof(StringExtensionHelper).GetMethod("QuickReverse");

        /// <summary>
        /// Build a 'contains' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildContainsExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression)
        {
            return Expression.Call(stringProperty.Body, typeof(string).GetMethod("Contains"), searchTermExpression);
        }

        /// <summary>
        /// Build a 'contains' expression for a searching a property that 
        /// contains the value of another string property
        /// </summary>
        public static Expression BuildContainsExpression<T>(Expression<Func<T, string>> propertyToSearch, Expression<Func<T, string>> propertyToSearchFor)
        {
            var isNotNullExpression = ExpressionHelper.BuildNotNullExpression(propertyToSearch);
            var searchForIsNotNullExpression = ExpressionHelper.BuildNotNullExpression(propertyToSearchFor);
            var containsExpression = Expression.Call(propertyToSearch.Body, typeof(string).GetMethod("Contains"), propertyToSearchFor.Body);
            var fullNotNullExpression = Expression.AndAlso(isNotNullExpression, searchForIsNotNullExpression);
            return Expression.AndAlso(fullNotNullExpression, containsExpression);
        }

        /// <summary>
        /// Build a 'equals' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildEqualsExpression<TSource, TType>(Expression<Func<TSource, TType>> property, IEnumerable<string> terms, StringComparison comparisonType)
        {
            var comparisonTypeExpression = Expression.Constant(comparisonType);
            Expression completeExpression = null;
            foreach (var term in terms)
            {
                var searchTermExpression = Expression.Constant(term);
                var equalsExpression = Expression.Call(property.Body, EqualsMethod, searchTermExpression, comparisonTypeExpression);
                completeExpression = completeExpression == null ? equalsExpression 
                                         : ExpressionHelper.JoinOrExpression(completeExpression, equalsExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build a 'indexof() >= 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildIndexOfGreaterThanMinusOneExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            if (nullCheck)
            {
                var coalesceExpression = Expression.Coalesce(stringProperty.Body, EmptyStringExpression);
                var nullCheckExpresion = Expression.Call(coalesceExpression, IndexOfMethod, searchTermExpression, stringComparisonExpression);
                return Expression.GreaterThanOrEqual(nullCheckExpresion, ZeroConstantExpression);                
            }

            var indexOfCallExpresion = Expression.Call(stringProperty.Body, IndexOfMethod, searchTermExpression, stringComparisonExpression);
            return Expression.GreaterThanOrEqual(indexOfCallExpresion, ZeroConstantExpression);
        }

        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildStartsWithExpression<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms, StringComparison comparisonType, bool nullCheck = true)
        {
            var stringComparisonExpression = Expression.Constant(comparisonType);
            BinaryExpression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var startsWithExpression = BuildStartsWithExpression(stringProperty, searchTerm, stringComparisonExpression, nullCheck);
                completeExpression = completeExpression == null ? startsWithExpression
                                         : Expression.OrElse(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression BuildStartsWithExpression<T>(Expression<Func<T, string>> stringProperty, string searchTerm, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            var searchTermExpression = Expression.Constant(searchTerm);
            if (nullCheck)
            {
                var coalesceExpression = Expression.Coalesce(stringProperty.Body, EmptyStringExpression);
                var nullCheckExpresion = Expression.Call(coalesceExpression, IndexOfMethod, searchTermExpression, stringComparisonExpression);
                return Expression.Equal(nullCheckExpresion, ZeroConstantExpression);
            }

            var indexOfCallExpresion = Expression.Call(stringProperty.Body, IndexOfMethod, searchTermExpression, stringComparisonExpression);
            return Expression.Equal(indexOfCallExpresion, ZeroConstantExpression);            
        }

        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildStartsWithExpression<T>(Expression<Func<T, string>> stringProperty, IEnumerable<Expression<Func<T, string>>> propertiesToSearchFor, StringComparison comparisonType, bool nullCheck = true)
        {
            var stringComparisonExpression = Expression.Constant(comparisonType);
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var startsWithExpression = BuildStartsWithExpression(stringProperty, propertyToSearchFor, stringComparisonExpression, nullCheck);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression BuildStartsWithExpression<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>> propertyToSearchFor, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            var indexOfCallExpression = Expression.Call(stringProperty.Body, IndexOfMethod, propertyToSearchFor.Body, stringComparisonExpression);
            var indexOfIsZeroExpression = Expression.Equal(indexOfCallExpression, ZeroConstantExpression);
            if (nullCheck)
            {
                var stringPropertyNotNullExpression = Expression.NotEqual(stringProperty.Body, NullExpression);
                var searchForNotNullExpression = Expression.NotEqual(propertyToSearchFor.Body, NullExpression);
                var propertiesNotNullExpression = Expression.AndAlso(stringPropertyNotNullExpression, searchForNotNullExpression);
                return Expression.AndAlso(propertiesNotNullExpression, indexOfIsZeroExpression);
            }

            return indexOfIsZeroExpression;
        }

        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildEndsWithExpression<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms, StringComparison comparisonType, bool nullCheck = true)
        {
            var stringComparisonExpression = Expression.Constant(comparisonType);
            Expression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var endsWithExpression = BuildEndsWithExpression(stringProperty, searchTerm, stringComparisonExpression, nullCheck);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, endsWithExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build an 'EndsWith([searchTerm])' expression for a search term against a particular string property
        /// </summary>
        private static Expression BuildEndsWithExpression<T>(Expression<Func<T, string>> stringProperty, string searchTerm, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            var searchTermExpression = Expression.Constant(searchTerm);
            var endsWithExpresion = Expression.Call(stringProperty.Body, EndsWithMethod, searchTermExpression, stringComparisonExpression);
            return Expression.IsTrue(endsWithExpresion);
        }

        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildEndsWithExpression<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>>[] propertiesToSearchFor, StringComparison comparisonType, bool nullCheck = true)
        {
            var stringComparisonExpression = Expression.Constant(comparisonType);
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var endsWithExpression = BuildEndsWithExpression(stringProperty, propertyToSearchFor, stringComparisonExpression, nullCheck);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, endsWithExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build an 'EndsWith([searchTerm])' expression for a search term against a particular string property
        /// </summary>
        private static Expression BuildEndsWithExpression<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>> propertyToSearchFor, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            var endsWithExpresion = Expression.Call(stringProperty.Body, EndsWithMethod, propertyToSearchFor.Body, stringComparisonExpression);
            Expression finalExpression = null;
            if (nullCheck)
            {
                var notNullProperty = ExpressionHelper.BuildNotNullExpression(stringProperty);
                var notNullSearchFor = ExpressionHelper.BuildNotNullExpression(propertyToSearchFor);
                finalExpression = ExpressionHelper.JoinAndAlsoExpression(notNullProperty, notNullSearchFor);
            }
            finalExpression = ExpressionHelper.JoinAndAlsoExpression(finalExpression, endsWithExpresion);
            return Expression.IsTrue(finalExpression);
        }

        /// <summary>
        /// Build an 'soundexCodes.Contains(soundex(value))' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildSoundsLikeExpression<T>(Expression<Func<T, string>> stringProperty, IList<string> soundexCodes)
        {
            var soundexCallExpresion = Expression.Call(SoundexMethod, stringProperty.Body);
            var soundexCodesExpression = Expression.Constant(soundexCodes);
            var containsExpression = Expression.Call(soundexCodesExpression, ContainsMethod, soundexCallExpresion);
            var trueExpression = Expression.Constant(true);
            return Expression.Equal(containsExpression, trueExpression);
        }

        /// <summary>
        /// Build an 'soundexCodes.Contains(reverseSoundex(value))' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildReverseSoundexLikeExpression<T>(Expression<Func<T, string>> stringProperty, IList<string> soundexCodes)
        {
            var soundexCallExpresion = Expression.Call(ReverseSoundexMethod, stringProperty.Body);
            var soundexCodesExpression = Expression.Constant(soundexCodes);
            var containsExpression = Expression.Call(soundexCodesExpression, ContainsMethod, soundexCallExpresion);
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
            MemberExpression lengthExpression = Expression.Property(stringProperty.Body, StringLengthProperty);
            var replaceExpression = Expression.Call(stringProperty.Body, ReplaceMethod, 
                                                    searchTermExpression, EmptyStringExpression);
            var replacedLengthExpression = Expression.Property(replaceExpression, StringLengthProperty);
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
            MemberExpression lengthExpression = Expression.Property(stringProperty.Body, StringLengthProperty);
            var replaceExpression = Expression.Call(CustomReplaceMethod, stringProperty.Body, searchTermExpression, EmptyStringExpression, stringComparisonExpression);
            var replacedLengthExpression = Expression.Property(replaceExpression, StringLengthProperty);
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
            return Expression.Call(LevensteinDistanceMethod, stringProperty.Body, searchTermExpression);
        }

        /// <summary>
        /// Calculates the Levenshtein distance between a given property and a search term
        /// </summary>
        /// <returns>Expression equivalent to: LevenshteinProcessor.LevensteinDistance([stringProperty], [searchTerm])</returns>
        public static Expression CalculateLevenshteinDistance<T>(Expression<Func<T, string>> sourceProperty, Expression<Func<T, string>> targetProperty)
        {
            return Expression.Call(LevensteinDistanceMethod, sourceProperty.Body, targetProperty.Body);
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