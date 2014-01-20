using System;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Search a particular property for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="stringProperty">String property to search</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <returns>Queryable records where the property contains the search term</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");

            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            return source.Search(new[] {searchTerm}, new[] {stringProperty});
        }

        /// <summary>
        /// Search multiple properties for a particular search term.
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>Queryable records where any property contains the search term</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string searchTerm, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");

            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            return source.Search(new[] {searchTerm}, stringProperties);
        }

        /// <summary>
        /// Search a property for multiple search terms.  
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search terms to find</param>
        /// <param name="stringProperty">properties to search against</param>
        /// <returns>Queryable records where the property contains any of the search terms</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, Expression<Func<T, string>> stringProperty, params string[] searchTerms)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");

            return source.Search(searchTerms, new[] {stringProperty});
        }

        /// <summary>
        /// Search multiple properties for multiple search terms
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>Queryable records where any property contains any of the search terms</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string[] searchTerms, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");

            if (searchTerms.Length == 0 || stringProperties.Length == 0)
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

            foreach (var searchTerm in validSearchTerms)
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    continue;
                }

                ConstantExpression searchTermExpression = Expression.Constant(searchTerm);
                foreach (var stringProperty in stringProperties)
                {
                    var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty, 
                                                                            stringProperty.Parameters.Single(), 
                                                                            singleParameter);

                    var containsExpression = ExpressionHelper.BuildContainsExpression(swappedParamExpression, searchTermExpression);
                    orExpression = ExpressionHelper.JoinOrExpression(orExpression, containsExpression);
                }
            }

            var completeExpression = Expression.Lambda<Func<T, bool>>(orExpression, singleParameter);
            return source.Where(completeExpression);
        }
    }
}
