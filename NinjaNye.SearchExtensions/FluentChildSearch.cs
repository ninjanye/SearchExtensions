using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public static class FluentChildSearch
    {
        /// <summary>
        /// Identify a child collection to search on
        /// </summary>
        /// <remarks>
        /// Initially wanted to have this method named `Search` as an 
        /// overload to the normal search methods, however due to the constraints
        /// of the .Net method resolution...
        /// (http://stackoverflow.com/questions/33231636/making-use-of-polymorphism-in-a-generic-parameter)
        /// this was not possible.
        /// </remarks>
        /// <typeparam name="TSource">Type of object to be searched</typeparam>
        /// <typeparam name="TProperty">Type of property to be searched</typeparam>
        /// <param name="source">source data on which to search</param>
        /// <param name="properties">Enumerable properties to search.</param>
        public static EnumerableChildSelector<TSource, TProperty> SearchChildren<TSource, TProperty>(this IEnumerable<TSource> source, params Expression<Func<TSource, IEnumerable<TProperty>>>[] properties)
        {
            return new EnumerableChildSelector<TSource, TProperty>(source, properties);
        }

        /// <summary>
        /// Identify a child collection to search on
        /// </summary>
        /// <typeparam name="TSource">Type of object to be searched</typeparam>
        /// <typeparam name="TProperty">Type of property to be searched</typeparam>
        /// <param name="source">source data on which to search</param>
        /// <param name="properties">Enumerable properties to search.</param>
        public static QueryableChildSelector<TSource, TProperty> SearchChildren<TSource, TProperty>(this IQueryable<TSource> source, params Expression<Func<TSource, IEnumerable<TProperty>>>[] properties)
        {
            return new QueryableChildSelector<TSource, TProperty>(source, properties);
        }
    }
}