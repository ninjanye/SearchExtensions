using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Validation;

namespace NinjaNye.SearchExtensions
{
    public static class FluentLevenshteinSearch
    {
        /// <summary>
        /// Begin a Levenshtein comparison on Enumerable collection of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="source">source data on which to perform search</param>
        /// <param name="stringProperty">String property to search.</param>
        public static EnumerableLevenshteinSearch<T> LevenshteinDistanceOf<T>(this IEnumerable<T> source, Expression<Func<T, string>> stringProperty)
        {
            Ensure.ArgumentNotNull(stringProperty, "stringProperty");
            return new EnumerableLevenshteinSearch<T>(source, stringProperty);
        }        
    }
}