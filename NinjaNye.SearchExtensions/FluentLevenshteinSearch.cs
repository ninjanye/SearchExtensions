using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;

namespace NinjaNye.SearchExtensions
{
    public static class FluentLevenshteinSearch
    {
        /// <summary>
        /// Begin a Levenshtein comparison on Enumerable collection of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="stringProperty">
        /// String properties to search. If ommitted, a search 
        /// on all string properties will be performed
        /// </param>
        public static EnumerableLevenshteinSearch<T> LevenshteinDistanceOf<T>(this IEnumerable<T> source, Expression<Func<T, string>> stringProperty)
        {
            if (stringProperty == null)
            {
                throw new ArgumentNullException("stringProperty");
            }

            return new EnumerableLevenshteinSearch<T>(source, stringProperty);
        }        
    }
}