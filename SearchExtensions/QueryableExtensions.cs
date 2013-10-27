using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SearchExtensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Search a particular property for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="stringProperty">String property to search</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <returns>Collection of records matching the search term</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            //Create expression to represent x.[property] != null
            var isNotNullExpression = Expression.NotEqual(stringProperty.Body, Expression.Constant(null));

            //Create expression to represent x.[property].Contains(searchTerm)
            var searchTermExpression = Expression.Constant(searchTerm);
            var containsExpression = BuildContainsExpression(stringProperty, searchTermExpression);

            //Join not null and contains expressions
            var notNullAndContainsExpression = Expression.AndAlso(isNotNullExpression, containsExpression);

            var completeExpression = Expression.Lambda<Func<T, bool>>(notNullAndContainsExpression, stringProperty.Parameters);
            return source.Where(completeExpression);
        }

        /// <summary>
        /// Search multiple properties for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>Collection of records matching the search term</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string searchTerm, params Expression<Func<T, string>>[] stringProperties)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            // The below represents the following lamda:
            // source.Where(x => x.[property1].Contains(searchTerm)
            //                || x.[property2].Contains(searchTerm)
            //                || x.[property3].Contains(searchTerm)...)

            var searchTermExpression = Expression.Constant(searchTerm);

            //Variable to hold merged 'OR' expression
            Expression orExpression = null;
            //Retrieve first parameter to use accross all expressions
            var singleParameter = stringProperties[0].Parameters.Single();

            //Build a contains expression for each property
            foreach (var stringProperty in stringProperties)
            {
                //Syncronise single parameter accross each property
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty, stringProperty.Parameters.Single(), singleParameter);

                //Build expression to represent x.[propertyX].Contains(searchTerm)
                var containsExpression = BuildContainsExpression(swappedParamExpression, searchTermExpression);

                orExpression = BuildOrExpression(orExpression, containsExpression);
            }

            var completeExpression = Expression.Lambda<Func<T, bool>>(orExpression, singleParameter);
            return source.Where(completeExpression);
        }

        /// <summary>
        /// Search a property for multiple search terms 
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search terms to find</param>
        /// <param name="stringProperty">properties to search against</param>
        /// <returns>Collection of records matching any of the search terms</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, Expression<Func<T, string>> stringProperty, params string[] searchTerms)
        {
            if (!searchTerms.Any())
            {
                return source;
            }

            Expression orExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                //Create expression to represent x.[property].Contains(searchTerm)
                var searchTermExpression = Expression.Constant(searchTerm);
                var containsExpression = BuildContainsExpression(stringProperty, searchTermExpression);

                orExpression = BuildOrExpression(orExpression, containsExpression);
            }

            var completeExpression = Expression.Lambda<Func<T, bool>>(orExpression, stringProperty.Parameters);
            return source.Where(completeExpression);
        }

        /// <summary>
        /// Search multiple properties for multiple search terms
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerms">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        /// <returns>Collection of records matching the search term</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, IList<string> searchTerms, params Expression<Func<T, string>>[] stringProperties)
        {
            if (!searchTerms.Any())
            {
                return source;
            }

            // The below represents the following lamda:
            // source.Where(x => x.[property1].Contains(searchTerm1)
            //                || x.[property1].Contains(searchTerm2)
            //                || x.[property2].Contains(searchTerm1)
            //                || x.[property2].Contains(searchTerm2)
            //                || x.[property3].Contains(searchTerm1)
            //                || x.[property3].Contains(searchTerm2)...)

            //Variable to hold merged 'OR' expression
            Expression orExpression = null;
            //Retrieve first parameter to use accross all expressions
            var singleParameter = stringProperties[0].Parameters.Single();

            foreach (var searchTerm in searchTerms)
            {
                //Create expression to represent x.[property].Contains(searchTerm)
                ConstantExpression searchTermExpression = Expression.Constant(searchTerm);

                //Build a contains expression for each property
                foreach (var stringProperty in stringProperties)
                {
                    //Syncronise single parameter accross each property
                    var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty, stringProperty.Parameters.Single(), singleParameter);

                    //Build expression to represent x.[propertyX].Contains(searchTerm)
                    var containsExpression = BuildContainsExpression(swappedParamExpression, searchTermExpression);

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
