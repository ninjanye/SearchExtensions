using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Soundex.Helpers;

namespace NinjaNye.SearchExtensions.Soundex
{
    public class QueryableSoundexSearch<T> : QueryableSearchBase<T, string>
    {
        public QueryableSoundexSearch(IQueryable<T> source, Expression<Func<T, string>>[] stringProperties)
            : base(source, stringProperties)
        {
        }

        public IQueryable<T> Include(string path)
        {
            QueryInclude(path);
            return this;
        }

        /// <summary>
        /// Returns Enumerable of records that match the Soundex code for
        /// any of the given terms across any of the defined properties
        /// </summary>
        /// <param name="terms">terms to search for</param>
        /// <returns>Enumerable of records where Soundex matches</returns>
        public IEnumerable<T> Matching(params string[] terms)
        {
            var firstCharacters = terms.Select(t => t.GetFirstCharacter())
                                       .Distinct()
                                       .ToArray();
            return Source.Search(Properties).StartsWith(firstCharacters).AsEnumerable()
                         .SoundexOf(Properties).Matching(terms);
        }
    }
}