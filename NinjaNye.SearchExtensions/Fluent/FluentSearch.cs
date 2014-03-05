using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Fluent
{
    public static class FluentSearch
    {
        public static EnumerableStringSearch<T> Search<T>(this IEnumerable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");
            return new EnumerableStringSearch<T>(source, stringProperties);
        }

        public static QueryableStringSearch<T> Search<T>(this IQueryable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");
            return new QueryableStringSearch<T>(source, stringProperties);
        }

        public static EnumerableStringSearch<TSource> SearchAll<TSource>(this IEnumerable<TSource> source)
        {
            var properties = EnumerableHelper.GetProperties<TSource, string>();
            return new EnumerableStringSearch<TSource>(source, properties);
        }

        public static QueryableStringSearch<TSource> SearchAll<TSource>(this IQueryable<TSource> source)
        {
            var properties = EnumerableHelper.GetProperties<TSource, string>();
            return new QueryableStringSearch<TSource>(source, properties);
        }
    }
}
