using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public static class SearchEnumerableExtensions
    {
        /// <summary>
        /// Search ALL string properties for a particular search term in memory
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <returns>Queryable records where the any string property contains the search term</returns>
        public static IEnumerable<T> Search<T>(this IEnumerable<T> source, string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            var stringProperties = ExpressionHelper.GetStringProperties<T>();
            return source.Search(new[] { searchTerm }, stringProperties);
        }

        /// <summary>
        /// Search a particular property for a particular search term in memory.
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="stringProperty">String property to search</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <returns>Enumerable records where the property contains the search term</returns>
        public static IEnumerable<T> Search<T>(this IEnumerable<T> source, string searchTerm, Expression<Func<T, string>> stringProperty)
        {
            return source.Search(searchTerm, stringProperty, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Search a particular property for a particular search term in memory.
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="stringProperty">String property to search</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <param name="stringComparison">Enumeration value that specifies how the strings will be compared.</param>
        /// <returns>Enumerable records where the property contains the search term</returns>
        public static IEnumerable<T> Search<T>(this IEnumerable<T> source, string searchTerm, Expression<Func<T, string>> stringProperty, StringComparison stringComparison)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");

            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            ConstantExpression searchTermExpression = Expression.Constant(searchTerm);
            ConstantExpression stringComparisonExpression = Expression.Constant(stringComparison);
            var indexOfExpression = ExpressionHelper.BuildIndexOfExpression(stringProperty.Body, searchTermExpression, stringComparisonExpression);
            var completeExpression = Expression.Lambda<Func<T, bool>>(indexOfExpression, stringProperty.Parameters).Compile();
            return source.Where(completeExpression);
        }

        /// <summary>
        /// Search multiple properties for a particular search term in memory.
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>Enumerable records where any property contains the search term</returns>
        public static IEnumerable<T> Search<T>(this IEnumerable<T> source, string searchTerm, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");

            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            return source.Search(new[] { searchTerm }, stringProperties, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Search multiple properties for a particular search term in memory.
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <param name="stringComparison">Enumeration value that specifies how the strings will be compared.</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>Enumerable records where any property contains the search term</returns>
        public static IEnumerable<T> Search<T>(this IEnumerable<T> source, string searchTerm, Expression<Func<T, string>>[] stringProperties, StringComparison stringComparison)
        {
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");

            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            return source.Search(new[] { searchTerm }, stringProperties, stringComparison);
        }

        /// <summary>
        /// Search a property for multiple search terms in memory.  
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search terms to find</param>
        /// <param name="stringProperty">properties to search against</param>
        /// <returns>Enumerable records where the property contains any of the search terms</returns>
        public static IEnumerable<T> Search<T>(this IEnumerable<T> source, string[] searchTerms, Expression<Func<T, string>> stringProperty)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");

            return source.Search(searchTerms, new[] { stringProperty }, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Search a property for multiple search terms in memory.  
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="stringComparison">Enumeration value that specifies how the strings will be compared.</param>
        /// <param name="searchTerms">search terms to find</param>
        /// <param name="stringProperty">properties to search against</param>
        /// <returns>Enumerable records where the property contains any of the search terms</returns>
        public static IEnumerable<T> Search<T>(this IEnumerable<T> source, string[] searchTerms, Expression<Func<T, string>> stringProperty, StringComparison stringComparison)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");

            return source.Search(searchTerms, new[]{stringProperty}, stringComparison);
        }

        /// <summary>
        /// Search multiple properties for multiple search terms in memory
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>Enumerable records where any property contains any of the search terms</returns>
        public static IEnumerable<T> Search<T>(this IEnumerable<T> source, string[] searchTerms, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");

            return source.Search(searchTerms, stringProperties, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Search multiple properties for multiple search terms in memory
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <param name="stringComparison">Enumeration value that specifies how the strings will be compared.</param>
        /// <returns>Enumerable records where any property contains any of the search terms</returns>
        public static IEnumerable<T> Search<T>(this IEnumerable<T> source, string[] searchTerms, Expression<Func<T, string>>[] stringProperties, StringComparison stringComparison)
        {
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");

            if (!searchTerms.Any() || !stringProperties.Any())
            {
                return source;
            }

            var validSearchTerms = searchTerms.Where(s => !String.IsNullOrWhiteSpace(s)).ToList();
            if (!validSearchTerms.Any())
            {
                return source;
            }

            Expression orExpression = null;
            var singleParameter = stringProperties[0].Parameters.Single();
            var stringComparisonExpression = Expression.Constant(stringComparison);

            foreach (var stringProperty in stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        singleParameter);

                foreach (var searchTerm in validSearchTerms)
                {
                    ConstantExpression searchTermExpression = Expression.Constant(searchTerm);

                    var indexOfExpression = ExpressionHelper.BuildIndexOfExpression(swappedParamExpression.Body, searchTermExpression, stringComparisonExpression);
                    orExpression = ExpressionHelper.JoinOrExpression(orExpression, indexOfExpression);
                }
            }

            var completeExpression = Expression.Lambda<Func<T, bool>>(orExpression, singleParameter).Compile();
            return source.Where(completeExpression);
        }
    }
}