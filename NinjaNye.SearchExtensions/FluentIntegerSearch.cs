using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;

namespace NinjaNye.SearchExtensions
{
    public static class FluentIntegerSearch
    {
        /// <summary>
        /// Search an Enumerable list of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="properties">
        /// Integer properties to search. If ommitted, a search 
        /// on all integer properties will be performed
        /// </param>
        public static EnumerableIntegerSearch<T> Search<T>(this IEnumerable<T> source, params Expression<Func<T, int>>[] properties)
        {
            return new EnumerableIntegerSearch<T>(source, properties);
        }

        /// <summary>
        /// Search a Queryable list of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="properties">
        /// Integer properties to search. If ommitted, a search 
        /// on all integer properties will be performed
        /// </param>
        public static QueryableIntegerSearch<T> Search<T>(this IQueryable<T> source, params Expression<Func<T, int>>[] properties)
        {
            return new QueryableIntegerSearch<T>(source, properties);
        }        
    }
}