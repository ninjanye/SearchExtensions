using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NinjaNye.SearchExtensions
{
    public static class RankedSearchExtensions
    {
        /// <summary>
        /// Search a particular property for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="stringProperty">String property to search</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <returns>IQueryable, IRanked records where the 
        /// property contains the search term specified</returns>        
        public static IQueryable<IRanked<T>> RankedSearch<T>(this IQueryable<T> source, Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            var hitCountExpression = CalculateHitCount(stringProperty, searchTerm);
            var parameterExpression = stringProperty.Parameters[0];
            var rankedInitExpression = ConstructRankedResult<T>(hitCountExpression, parameterExpression);

            var selectExpression = Expression.Lambda<Func<T, Ranked<T>>>(rankedInitExpression, parameterExpression);
            return source.Search(stringProperty, searchTerm).Select(selectExpression);
        }

        /// <summary>
        /// Constructs a ranked result of type T
        /// </summary>
        /// <param name="hitCountExpression">Expression representing how to calculated search hits</param>
        /// <param name="parameterExpression">property parameter</param>
        /// <returns>Expression equivalent to: new Ranked{ Hits = [hitCountExpression], Item = x }</returns>
        private static Expression ConstructRankedResult<T>(Expression hitCountExpression, 
                                                           ParameterExpression parameterExpression)
        {
            var rankedType = typeof (Ranked<T>);
            var rankedCtor = Expression.New(rankedType);
            var hitProperty = rankedType.GetProperty("Hits");
            var hitValueAssignment = Expression.Bind(hitProperty, hitCountExpression);
            var itemProperty = rankedType.GetProperty("Item");
            var itemValueAssignment = Expression.Bind(itemProperty, parameterExpression);
            return Expression.MemberInit(rankedCtor, hitValueAssignment, itemValueAssignment);
        }

        /// <summary>
        /// Calculates how many search hits occured for a given property
        /// </summary>
        /// <param name="stringProperty">string property to analyse</param>
        /// <param name="searchTerm">search term to count</param>
        /// <returns>Expression equivalent to: [property].Length - ([property].Replace([searchTerm], "").Length) / [searchTerm].Length</returns>
        private static Expression CalculateHitCount<T>(Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            Expression searchTermExpression = Expression.Constant(searchTerm);
            Expression searchTermLengthExpression = Expression.Constant(searchTerm.Length);
            Expression emptyStringExpression = Expression.Constant("");
            PropertyInfo stringLengthProperty = typeof (string).GetProperty("Length");
            var lengthExpression = Expression.Property(stringProperty.Body, stringLengthProperty);
            MethodInfo replaceMethod = typeof (string).GetMethod("Replace", new[] {typeof (string), typeof (string)});
            var replaceExpression = Expression.Call(stringProperty.Body, replaceMethod, 
                                                    searchTermExpression, emptyStringExpression);
            var replacedLengthExpression = Expression.Property(replaceExpression, stringLengthProperty);
            var characterDiffExpression = Expression.Subtract(lengthExpression, replacedLengthExpression);
            var hitCountExpression = Expression.Divide(characterDiffExpression, searchTermLengthExpression);
            return hitCountExpression;
        }
    }
}