using System;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public static class SearchQueryableExtensions
    {
        /// <summary>   
        /// Search ALL string properties for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <returns>Queryable records where the any string property contains the search term</returns>
        [Obsolete("This method has been superseded by the fluent api. Please use the Fluent API http://jnye.co/fluent")]
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            var stringProperties = EnumerableHelper.GetProperties<T, string>();
            return source.Search(new[] {searchTerm}, stringProperties);
        }

        /// <summary>
        /// Search ALL string properties for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search term to look for</param>
        /// <returns>Queryable records where the any string property contains the search term</returns>
        [Obsolete("This method has been superseded by the fluent api. Please use the Fluent API http://jnye.co/fluent")]
        public static IQueryable<T> Search<T>(this IQueryable<T> source, params string[] searchTerms)
        {
            Ensure.ArgumentNotNull(searchTerms, "searchTerms");

            var stringProperties = EnumerableHelper.GetProperties<T, string>();
            return source.Search(searchTerms, stringProperties);
        }

        /// <summary>
        /// Search a particular property for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="stringProperty">String property to search</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <returns>Queryable records where the property contains the search term</returns>
        [Obsolete("This method has been superseded by the fluent api. Please use the Fluent API http://jnye.co/fluent")]
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string searchTerm, Expression<Func<T, string>> stringProperty)
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
        [Obsolete("This method has been superseded by the fluent api. Please use the Fluent API http://jnye.co/fluent")]
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
        [Obsolete("This method has been superseded by the fluent api. Please use the Fluent API http://jnye.co/fluent")]
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string[] searchTerms, Expression<Func<T, string>> stringProperty)
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
        [Obsolete("This method has been superseded by the fluent api. Please use the Fluent API http://jnye.co/fluent")]
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string[] searchTerms, params Expression<Func<T, string>>[] stringProperties)
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
            bool isDbProvider = source.Provider.GetType().Name == "DbQueryProvider";

            Expression notNullExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        singleParameter);

                var propertyNotNullExpression = ExpressionHelper.BuildNotNullExpression(swappedParamExpression);
                notNullExpression = ExpressionHelper.JoinOrExpression(notNullExpression, propertyNotNullExpression);

                foreach (var searchTerm in validSearchTerms)
                {
                    ConstantExpression searchTermExpression = Expression.Constant(searchTerm);
                    Expression comparisonExpression = isDbProvider ? EnumerableHelper.BuildContainsExpression(swappedParamExpression, searchTermExpression)
                                                                   : EnumerableHelper.BuildIndexOfGreaterThanMinusOneExpression(swappedParamExpression, searchTermExpression, false);
                    orExpression = ExpressionHelper.JoinOrExpression(orExpression, comparisonExpression);
                }
            }

            var jointExpression = ExpressionHelper.JoinAndAlsoExpression(notNullExpression, orExpression);
            var completeExpression = Expression.Lambda<Func<T, bool>>(jointExpression, singleParameter);
            return source.Where(completeExpression);
        }
    }
}
