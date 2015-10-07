using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public static class FluentChildSearch
    {
        /// <summary>
        /// Identify a child collection to search
        /// </summary>
        /// <typeparam name="TSource">Type of object to be searched</typeparam>
        /// <typeparam name="TProperty">Type of property to be searched</typeparam>
        /// <param name="property">Enumerable properties to search.</param>
        public static EnumerableChildSearch<TSource, TProperty> Search<TSource, TProperty>(this IEnumerable<TSource> source, Expression<Func<TSource, IEnumerable<TProperty>>> property)
        {
            return new EnumerableChildSearch<TSource, TProperty>(source, new[] {property});
        }
    }

    public class EnumerableChildSearch<TSource, TProperty> : EnumerableSearchBase<TSource, IEnumerable<TProperty>>
    {
        public EnumerableChildSearch(IEnumerable<TSource> source, Expression<Func<TSource, IEnumerable<TProperty>>>[] properties) 
            : base(source, properties)
        {
        }

        public ExpressionStructSearch<TSource, TProperty, TNewProperty> With<TNewProperty>(params Expression<Func<TProperty, TNewProperty>>[] properties)
        {
            return new ExpressionStructSearch<TSource, TProperty, TNewProperty>(Source, Properties[0], properties);
        }

        public ExpressionStringSearch<TSource, TProperty> With(params Expression<Func<TProperty, string>>[] properties)
        {
            return new ExpressionStringSearch<TSource, TProperty>(Source, Properties[0], properties);
        }
    }

    public class ExpressionStructSearch<TBase, TSource, TProperty> : EnumerableStructSearch<TSource, TProperty>
    {
        public ExpressionStructSearch(IEnumerable<TBase> baseSource, Expression<Func<TBase, IEnumerable<TSource>>> source, Expression<Func<TSource, TProperty>>[] properties)
            : base(source.Compile().Invoke(baseSource.First()), properties)
        {
        }
    }

    public class ExpressionStringSearch<TBase, TSource> : EnumerableStringSearch<TSource>
    {
        public ExpressionStringSearch(IEnumerable<TBase> baseSource, Expression<Func<TBase, IEnumerable<TSource>>> source, Expression<Func<TSource, string>>[] properties)
            : base(source.Compile().Invoke(baseSource.First()), properties)
        {
        }
    }
}