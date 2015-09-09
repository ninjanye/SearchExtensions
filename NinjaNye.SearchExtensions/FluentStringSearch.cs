using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders;

namespace NinjaNye.SearchExtensions
{
    public static class FluentStringSearch
    {
        /// <summary>
        /// Search an Enumerable list of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        public static EnumerableStringSearch<T> Search<T>(this IEnumerable<T> source)
        {
            var stringProperties = EnumerableExpressionHelper.GetProperties<T, string>();
            return new EnumerableStringSearch<T>(source, stringProperties);
        }

        /// <summary>
        /// Search an Enumerable list of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="source"></param>
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
        /// Search a Queryable collection
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        public static QueryableStringSearch<T> Search<T>(this IQueryable<T> source)
        {
            var properties = EnumerableExpressionHelper.GetProperties<T, string>();
            return new QueryableStringSearch<T>(source, properties);
        }

        /// <summary>
        /// Search a Queryable collection
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="source"></param>
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
