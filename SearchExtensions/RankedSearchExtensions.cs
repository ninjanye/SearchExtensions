using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public static class RankedSearchExtensions
    {
        #region IQueryable extensions
        /// <summary>
        /// Search a particular property for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="stringProperty">String property to search</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <returns>IQueryable, IRanked records where the 
        /// property contains the search term specified</returns>        
        public static IQueryable<IRanked<T>> RankedSearch<T>(this IQueryable<T> source, string searchTerm, Expression<Func<T, string>> stringProperty)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerm, "searchTerm");

            return RankedSearch(source, new[] { searchTerm }, new[] { stringProperty });
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
        public static IQueryable<IRanked<T>> RankedSearch<T>(this IQueryable<T> source, string[] searchTerms, Expression<Func<T, string>> stringProperty)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");

            return RankedSearch(source, searchTerms, new[] { stringProperty });
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
                    var hitCountExpression = ExpressionHelper.CalculateHitCount(nullSafeExpresion, searchTerm);
                    combinedHitExpression = ExpressionHelper.AddExpressions(combinedHitExpression, hitCountExpression);
                }
            }

            var rankedInitExpression = ExpressionHelper.ConstructRankedResult<T>(combinedHitExpression, singleParameter);
            var selectExpression = Expression.Lambda<Func<T, Ranked<T>>>(rankedInitExpression, singleParameter);       
            return source.Search(searchTerms, stringProperties)
                         .Select(selectExpression);
        }
        #endregion

        #region Enumerable extensions
        /// <summary>
        /// Search a particular property for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="stringProperty">String property to search</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <returns>IEnumerable, IRanked records where the 
        /// property contains the search term specified</returns>        
        public static IEnumerable<IRanked<T>> RankedSearch<T>(this IEnumerable<T> source, string searchTerm, Expression<Func<T, string>> stringProperty)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerm, "searchTerm");

            return RankedSearch(source, new[] { searchTerm }, new[] { stringProperty }, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Search a particular property for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="stringProperty">String property to search</param>
        /// <param name="stringComparison">Enumeration value that specifies how the strings will be compared.</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <returns>IEnumerable, IRanked records where the 
        /// property contains the search term specified</returns>        
        public static IEnumerable<IRanked<T>> RankedSearch<T>(this IEnumerable<T> source, string searchTerm, Expression<Func<T, string>> stringProperty, StringComparison stringComparison)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerm, "searchTerm");

            return RankedSearch(source, new[] { searchTerm }, new[] { stringProperty }, stringComparison);
        }

        /// <summary>
        /// Search multiple properties for a particular search term and return a ranked result.
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>IEnumerable of IRanked records where any property contains the search term</returns>
        public static IEnumerable<IRanked<T>> RankedSearch<T>(this IEnumerable<T> source, string searchTerm, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");
            Ensure.ArgumentNotNull(searchTerm, "searchTerm");

            return RankedSearch(source, new[] { searchTerm }, stringProperties, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Search multiple properties for a particular search term and return a ranked result.
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <param name="stringComparison">Enumeration value that specifies how the strings will be compared.</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>IEnumerable of IRanked records where any property contains the search term</returns>
        public static IEnumerable<IRanked<T>> RankedSearch<T>(this IEnumerable<T> source, string searchTerm, Expression<Func<T, string>>[] stringProperties, StringComparison stringComparison)
        {
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");
            Ensure.ArgumentNotNull(searchTerm, "searchTerm");

            return RankedSearch(source, new[] { searchTerm }, stringProperties, stringComparison);
        }

        /// <summary>
        /// Search a property for multiple search terms returning a ranked result.  
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search terms to find</param>
        /// <param name="stringProperty">properties to search against</param>
        /// <returns>IEnumerable records where the property contains any of the search terms</returns>
        public static IEnumerable<IRanked<T>> RankedSearch<T>(this IEnumerable<T> source, string[] searchTerms, Expression<Func<T, string>> stringProperty)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");

            return RankedSearch(source, searchTerms, new[] { stringProperty }, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Search a property for multiple search terms returning a ranked result.  
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search terms to find</param>
        /// <param name="stringComparison">Enumeration value that specifies how the strings will be compared.</param>
        /// <param name="stringProperty">properties to search against</param>
        /// <returns>IEnumerable records where the property contains any of the search terms</returns>
        public static IEnumerable<IRanked<T>> RankedSearch<T>(this IEnumerable<T> source, string[] searchTerms, Expression<Func<T, string>> stringProperty, StringComparison stringComparison)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");

            return RankedSearch(source, searchTerms, new[] { stringProperty }, stringComparison);
        }

        /// <summary>
        /// Search multiple properties for multiple search terms returning a ranked result
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>IEnumerable of IRanked records where any property contains any of the search terms</returns>
        public static IEnumerable<IRanked<T>> RankedSearch<T>(this IEnumerable<T> source, string[] searchTerms, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");

            return RankedSearch(source, searchTerms, stringProperties, StringComparison.CurrentCulture);
        }


        /// <summary>
        /// Search multiple properties for multiple search terms returning a ranked result
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <param name="stringComparison">Enumeration value that specifies how the strings will be compared.</param>
        /// <returns>IEnumerable of IRanked records where any property contains any of the search terms</returns>
        public static IEnumerable<IRanked<T>> RankedSearch<T>(this IEnumerable<T> source, string[] searchTerms, Expression<Func<T, string>>[] stringProperties, StringComparison stringComparison)
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
                    var hitCountExpression = ExpressionHelper.CalculateHitCount(nullSafeExpresion, searchTerm, stringComparison);
                    combinedHitExpression = ExpressionHelper.AddExpressions(combinedHitExpression, hitCountExpression);
                }
            }

            var rankedInitExpression = ExpressionHelper.ConstructRankedResult<T>(combinedHitExpression, singleParameter);
            var selectExpression = Expression.Lambda<Func<T, Ranked<T>>>(rankedInitExpression, singleParameter); 
            return source.Search(searchTerms, stringProperties, stringComparison)
                         .Select(x => selectExpression.Compile().Invoke(x));
        }
        #endregion

    }
}