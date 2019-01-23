using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Soundex
{
    public static class FluentSoundexSearch
    {
        /// <summary>
        /// Begin a Soundex comparison on Enumerable collection of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="source">source data on which to perform search</param>
        /// <param name="stringProperties">String property to search.</param>
        public static EnumerableSoundexSearch<T> SoundexOf<T>(this IEnumerable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            if (stringProperties == null)
            {
                throw new ArgumentNullException(nameof(stringProperties));
            }
            return new EnumerableSoundexSearch<T>(source, stringProperties);
        }

        /// <summary>
        /// Begin a Soundex comparison on Queryable collection of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="source">source data on which to perform search</param>
        /// <param name="stringProperties">String property to search.</param>
        public static QueryableSoundexSearch<T> SoundexOf<T>(this IQueryable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            if (stringProperties == null)
            {
                throw new ArgumentNullException(nameof(stringProperties));
            }
            return new QueryableSoundexSearch<T>(source, stringProperties);
        }

        /// <summary>
        /// Begin a Soundex comparison on Enumerable collection of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="source">source data on which to perform search</param>
        /// <param name="stringProperties">String property to search.</param>
        public static EnumerableReverseSoundexSearch<T> ReverseSoundexOf<T>(this IEnumerable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            if (stringProperties == null)
            {
                throw new ArgumentNullException(nameof(stringProperties));
            }
            return new EnumerableReverseSoundexSearch<T>(source, stringProperties);
        }

        /// <summary>
        /// Begin a Soundex comparison on Enumerable collection of objects
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="source">source data on which to perform search</param>
        /// <param name="stringProperties">String property to search.</param>
        public static QueryableReverseSoundexSearch<T> ReverseSoundexOf<T>(this IQueryable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            if (stringProperties == null)
            {
                throw new ArgumentNullException(nameof(stringProperties));
            }
            return new QueryableReverseSoundexSearch<T>(source, stringProperties);
        }

    }
}