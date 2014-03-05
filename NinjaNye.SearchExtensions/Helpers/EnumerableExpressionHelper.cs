using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NinjaNye.SearchExtensions.Helpers
{
    internal static class EnumerableExpressionHelper
    {
        static readonly ConstantExpression EmptyStringExpression = Expression.Constant(string.Empty);
        static readonly ConstantExpression ZeroConstantExpression = Expression.Constant(0);
        static readonly MethodInfo EqualsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(StringComparison) });
        static readonly MethodInfo IndexOfMethod = typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) });
        static readonly PropertyInfo StringLengthProperty = typeof(string).GetProperty("Length");
        static readonly MethodInfo ReplaceMethod = typeof(string).GetMethod("Replace", new[] { typeof(string), typeof(string) });
        static readonly MethodInfo CustomReplaceMethod = typeof(StringExtensionHelper).GetMethod("Replace");

        /// <summary>
        /// Build a 'contains' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildContainsExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression)
        {
            return Expression.Call(stringProperty.Body, typeof(string).GetMethod("Contains"), searchTermExpression);
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
        public static BinaryExpression BuildIndexOfGreaterThanMinusOneExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression, bool nullCheck = true)
        {
            var stringComparisonExpression = Expression.Constant(StringComparison.OrdinalIgnoreCase);
            return BuildIndexOfGreaterThanMinusOneExpression(stringProperty, searchTermExpression, stringComparisonExpression, nullCheck);
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
        public static BinaryExpression BuildEndsWithExpression<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms, StringComparison comparisonType, bool nullCheck = true)
        {
            var stringComparisonExpression = Expression.Constant(comparisonType);
            BinaryExpression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var endsWithExpression = BuildEndsWithExpression(stringProperty, searchTerm, stringComparisonExpression, nullCheck);
                completeExpression = completeExpression == null ? endsWithExpression
                                         : Expression.OrElse(completeExpression, endsWithExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        private static BinaryExpression BuildEndsWithExpression<T>(Expression<Func<T, string>> stringProperty, string searchTerm, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            int expectedIndexOf = searchTerm.Length;
            var lengthExpression = Expression.Constant(expectedIndexOf);
            var searchTermExpression = Expression.Constant(searchTerm);
            if (nullCheck)
            {
                var coalesceExpression = Expression.Coalesce(stringProperty.Body, EmptyStringExpression);
                var nullCheckExpresion = Expression.Call(coalesceExpression, IndexOfMethod, searchTermExpression, stringComparisonExpression);
                return Expression.Equal(nullCheckExpresion, lengthExpression);
            }

            var indexOfCallExpresion = Expression.Call(stringProperty.Body, IndexOfMethod, searchTermExpression, stringComparisonExpression);
            var lengthPropertyExpression = Expression.Property(stringProperty.Body, StringLengthProperty);
            var expectedLengthExpression = Expression.Subtract(lengthPropertyExpression, lengthExpression);
            return Expression.Equal(indexOfCallExpresion, expectedLengthExpression);
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