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
        /// <returns>Collection of records where the property contains the search term</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            if (stringProperty == null) throw new ArgumentNullException("stringProperty");

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
        /// <returns>Collection of records where any property contains the search term</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string searchTerm, params Expression<Func<T, string>>[] stringProperties)
        {
            if (stringProperties == null) throw new ArgumentNullException("stringProperties");

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
        /// <returns>Collection of records where the property contains any of the search terms</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, Expression<Func<T, string>> stringProperty, params string[] searchTerms)
        {
            if (stringProperty == null) throw new ArgumentNullException("stringProperty");
            if (searchTerms == null)
            {
                throw new ArgumentNullException("searchTerms");
            }

            return source.Search(searchTerms, new[] {stringProperty});
        }

        /// <summary>
        /// Search multiple properties for multiple search terms
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>Collection of records where any property contains any of the search terms</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string[] searchTerms, params Expression<Func<T, string>>[] stringProperties)
        {
            if (searchTerms == null)
            {
                throw new ArgumentNullException("searchTerms");
            }

            if (stringProperties == null)
            {
                throw new ArgumentNullException("stringProperties");
            }

            if (searchTerms.Length == 0 || stringProperties.Length == 0)
            {
                return source;
            }

            if (searchTerms.All(String.IsNullOrEmpty))
            {
                return source;
            }

            // The below represents the following lamda:
            // source.Where(x => x.[property1].Contains(searchTerm1)
            //                || x.[property2].Contains(searchTerm1)
            //                || x.[property1].Contains(searchTerm2)
            //                || x.[property2].Contains(searchTerm2)...)

            //Variable to hold merged 'OR' expression
            Expression orExpression = null;
            //Retrieve first parameter to use accross all expressions
            var singleParameter = stringProperties[0].Parameters.Single();

            foreach (var searchTerm in searchTerms)
            {
                //Create a constant to represent the search term
                ConstantExpression searchTermExpression = Expression.Constant(searchTerm);

                //Build a contains expression for each property
                foreach (var stringProperty in stringProperties)
                {
                    //Syncronise single parameter accross each property
                    var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty, stringProperty.Parameters.Single(), singleParameter);

                    //Build expression to represent x.[propertyX].Contains(searchTerm)
                    var containsExpression = BuildContainsExpression(swappedParamExpression, searchTermExpression);

                    //Add contains expresion to the existing expression
                    orExpression = BuildOrExpression(orExpression, containsExpression);
                }
            }

            var completeExpression = Expression.Lambda<Func<T, bool>>(orExpression, singleParameter);
            return source.Where(completeExpression);
        }

        /// <summary>
        /// Connect to expressions using the OrElse expression
        /// </summary>
        private static Expression BuildOrExpression(Expression existingExpression, Expression expressionToAdd)
        {
            if (existingExpression == null)
            {
                return expressionToAdd;
            }

            //Build 'OR' expression for each property
            return Expression.OrElse(existingExpression, expressionToAdd);
        }

        /// <summary>
        /// Build 'contains' expression for a particular search property against a search term expression
        /// </summary>
        private static MethodCallExpression BuildContainsExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression)
        {
            return Expression.Call(stringProperty.Body, typeof(string).GetMethod("Contains"), searchTermExpression);
        }
    }
}
