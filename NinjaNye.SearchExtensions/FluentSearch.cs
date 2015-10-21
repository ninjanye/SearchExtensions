using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public static class FluentSearch
    {
        /// <summary>
        /// Search an Enumerable list of objects
        /// </summary>
        /// <typeparam name="TSource">Type of object to be searched</typeparam>
        /// <typeparam name="TProperty">Type of property to be searched</typeparam>
        /// <param name="source">source data on which to perform search</param>
        /// <param name="properties">
        /// Properties to search.</param>
        public static EnumerableStructSearch<TSource, TProperty> Search<TSource, TProperty>(this IEnumerable<TSource> source, params Expression<Func<TSource, TProperty>>[] properties) 
            where TProperty : struct
        {
            return new EnumerableStructSearch<TSource, TProperty>(source, properties);
        }

        /// <summary>
        /// Search a Queryable list of objects
        /// </summary>
        /// <typeparam name="TSource">Type of object to be searched</typeparam>
        /// <typeparam name="TProperty">Type of property to be searched</typeparam>
        /// <param name="source">source data on which to perform search</param>
        /// <param name="properties">Properties to search.</param>
        public static QueryableStructSearch<TSource, TProperty> Search<TSource, TProperty>(this IQueryable<TSource> source, params Expression<Func<TSource, TProperty>>[] properties) 
            where TProperty : struct
        {
            return new QueryableStructSearch<TSource, TProperty>(source, properties);
        }        
    }
}