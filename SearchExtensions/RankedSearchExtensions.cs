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
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerm, "searchTerm");

            return RankedSearch(source, new[] {searchTerm}, new[] {stringProperty});
        }

        /// <summary>
        /// Search multiple properties for a particular search term and return a ranked result.
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>Queryable of IRanked records where any property contains the search term</returns>
        public static IQueryable<IRanked<T>> RankedSearch<T>(this IQueryable<T> source, string searchTerm, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");
            Ensure.ArgumentNotNull(searchTerm, "searchTerm");

            return RankedSearch(source, new[] { searchTerm }, stringProperties);
        }

        /// <summary>
        /// Search a property for multiple search terms returning a ranked result.  
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search terms to find</param>
        /// <param name="stringProperty">properties to search against</param>
        /// <returns>Queryable records where the property contains any of the search terms</returns>
        public static IQueryable<IRanked<T>> RankedSearch<T>(this IQueryable<T> source, Expression<Func<T, string>> stringProperty, params string[] searchTerms)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");

            return RankedSearch(source, searchTerms, new[] {stringProperty});
        }

        /// <summary>
        /// Search multiple properties for multiple search terms returning a ranked result
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>Queryable of IRanked records where any property contains any of the search terms</returns>
        public static IQueryable<IRanked<T>> RankedSearch<T>(this IQueryable<T> source, string[] searchTerms, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");

            var validSearchTerms = searchTerms.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray();
            if (!validSearchTerms.Any())
            {
                throw new ArgumentException("No valid search terms have been provided", "searchTerms");
            }

            var singleParameter = stringProperties[0].Parameters.Single();
            Expression combinedHitExpression = null;
            ConstantExpression emptyStringExpression = Expression.Constant("");
            foreach (var stringProperty in stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        singleParameter);

                foreach (var searchTerm in validSearchTerms)
                {
                    var nullSafeProperty = Expression.Coalesce(swappedParamExpression.Body, emptyStringExpression);
                    var nullSafeExpresion = Expression.Lambda<Func<T, string>>(nullSafeProperty, singleParameter);
                    var hitCountExpression = CalculateHitCount(nullSafeExpresion, searchTerm);
                    combinedHitExpression = AddExpressions(combinedHitExpression, hitCountExpression);
                }
            }

            var rankedInitExpression = ConstructRankedResult<T>(combinedHitExpression, singleParameter);
            var selectExpression = Expression.Lambda<Func<T, Ranked<T>>>(rankedInitExpression, singleParameter);
            return source.Search(validSearchTerms, stringProperties)
                         .Select(selectExpression);


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
            var rankedType = typeof(Ranked<T>);
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
            PropertyInfo stringLengthProperty = typeof(string).GetProperty("Length");
            var lengthExpression = Expression.Property(stringProperty.Body, stringLengthProperty);
            MethodInfo replaceMethod = typeof(string).GetMethod("Replace", new[] { typeof(string), typeof(string) });
            var replaceExpression = Expression.Call(stringProperty.Body, replaceMethod,
                                                    searchTermExpression, emptyStringExpression);
            var replacedLengthExpression = Expression.Property(replaceExpression, stringLengthProperty);
            var characterDiffExpression = Expression.Subtract(lengthExpression, replacedLengthExpression);
            var hitCountExpression = Expression.Divide(characterDiffExpression, searchTermLengthExpression);
            return hitCountExpression;
        }

        /// <summary>
        /// Join two expressions using the add operation
        /// </summary>
        /// <param name="existingExpression">Expression being summed</param>
        /// <param name="expressionToAdd">Expression to append</param>
        /// <returns>New expression containing both expressions joined using the Add operation</returns>
        private static Expression AddExpressions(Expression existingExpression, Expression expressionToAdd)
        {
            if (existingExpression == null)
            {
                return expressionToAdd;
            }

            return Expression.Add(existingExpression, expressionToAdd);
        }
    }
}