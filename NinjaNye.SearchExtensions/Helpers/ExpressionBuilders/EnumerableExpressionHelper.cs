using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NinjaNye.SearchExtensions.Models;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders
{
    internal static class EnumerableExpressionHelper
    {
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
            PropertyInfo hitProperty = rankedType.GetRuntimeProperty("Hits");
            PropertyInfo itemProperty = rankedType.GetRuntimeProperty("Item");
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
        public static Expression CalculateHitCount<T>(Expression<Func<T, string>> stringProperty, string searchTerm, SearchOptions searchOptions)
        {
            Expression searchTermExpression = Expression.Constant(searchTerm);
            Expression searchTermLengthExpression = Expression.Constant(searchTerm.Length);
            MemberExpression lengthExpression = Expression.Property(stringProperty.Body, ExpressionMethods.StringLengthProperty);
            var replaceExpression = Expression.Call(ExpressionMethods.CustomReplaceMethod, stringProperty.Body, searchTermExpression, ExpressionMethods.EmptyStringExpression, searchOptions.ComparisonTypeExpression);
            var replacedLengthExpression = Expression.Property(replaceExpression, ExpressionMethods.StringLengthProperty);
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
#if NET45
                var stringProperties = typeof(TSource).GetTypeInfo().GetProperties()
                                                  .Where(property => property.CanRead
                                                                  && property.PropertyType == typeof(TType));
#else
            var stringProperties = typeof(TSource).GetRuntimeProperties()
                                              .Where(property => property.CanRead
                                                              && property.PropertyType == typeof(TType));
#endif

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