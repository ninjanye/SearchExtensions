using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Fluent
{
    public static class FluentSearch
    {
        public static FluentString<T> Search<T>(this IEnumerable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");
            return new FluentString<T>(source, stringProperties);
        }

        public static FluentString<TSource> SearchAll<TSource>(this IEnumerable<TSource> source)
        {
            var properties = ExpressionHelper.GetProperties<TSource, string>();
            return new FluentString<TSource>(source, properties);
        }
    }
}
