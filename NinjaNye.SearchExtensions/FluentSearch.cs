using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;

namespace NinjaNye.SearchExtensions
{
    public static class FluentSearch
    {
        public static EnumerableStringSearch<T> Search<T>(this IEnumerable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            if (stringProperties == null || stringProperties.All(sp => sp == null))
            {
                return source.SearchAll();
            }

            return new EnumerableStringSearch<T>(source, stringProperties);
        }

        public static QueryableStringSearch<T> Search<T>(this IQueryable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            if (stringProperties == null || stringProperties.All(sp => sp == null))
            {
                return source.SearchAll();
            }

            return new QueryableStringSearch<T>(source, stringProperties);
        }

        private static EnumerableStringSearch<TSource> SearchAll<TSource>(this IEnumerable<TSource> source)
        {
            var properties = EnumerableExpressionHelper.GetProperties<TSource, string>();
            return new EnumerableStringSearch<TSource>(source, properties);
        }

        private static QueryableStringSearch<TSource> SearchAll<TSource>(this IQueryable<TSource> source)
        {
            var properties = EnumerableExpressionHelper.GetProperties<TSource, string>();
            return new QueryableStringSearch<TSource>(source, properties);
        }
    }
}
