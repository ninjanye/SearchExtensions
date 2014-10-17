using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;

namespace NinjaNye.SearchExtensions
{
    public static class FluentSearch
    {
        /// <summary>
        /// Search an Enumerable list of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="stringProperties">
        /// String properties to search. If ommitted, a search 
        /// on all string properties will be performed
        /// </param>
        public static EnumerableStringSearch<T> Search<T>(this IEnumerable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            if (stringProperties == null || stringProperties.All(sp => sp == null))
            {
                stringProperties = EnumerableExpressionHelper.GetProperties<T, string>();
            }

            return new EnumerableStringSearch<T>(source, stringProperties);
        }

        /// <summary>
        /// Soundex search an Enumerable list of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="stringProperties">
        /// String properties to search. If ommitted, a search 
        /// on all string properties will be performed
        /// </param>
        public static EnumerableSoundexSearch<T> SoundexSearch<T>(this IEnumerable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            if (stringProperties == null || stringProperties.All(sp => sp == null))
            {
                stringProperties = EnumerableExpressionHelper.GetProperties<T, string>();
            }

            return new EnumerableSoundexSearch<T>(source, stringProperties);
        }

        /// <summary>
        /// Search a Queryable collection
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="stringProperties">
        /// String properties to search. If ommitted, a search 
        /// on all string properties will be performed
        /// </param>
        public static QueryableStringSearch<T> Search<T>(this IQueryable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            if (stringProperties == null || stringProperties.All(sp => sp == null))
            {
                stringProperties = EnumerableExpressionHelper.GetProperties<T, string>();
            }

            return new QueryableStringSearch<T>(source, stringProperties);
        }

    }
}
