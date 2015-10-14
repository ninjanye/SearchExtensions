using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public static class FluentChildSearch
    {
        /// <summary>
        /// Identify a child collection to search on
        /// </summary>
        /// <typeparam name="TSource">Type of object to be searched</typeparam>
        /// <typeparam name="TProperty">Type of property to be searched</typeparam>
        /// <param name="property">Enumerable properties to search.</param>
        public static EnumerableChildSelector<TSource, TProperty> Search<TSource, TProperty>(this IEnumerable<TSource> source, Expression<Func<TSource, IEnumerable<TProperty>>> property)
        {
            return new EnumerableChildSelector<TSource, TProperty>(source, property);
        }
    }
}