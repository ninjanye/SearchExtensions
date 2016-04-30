using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NinjaNye.SearchExtensions
{
    internal interface ISearchTermCollection : IEnumerable<string>
    {
        void Add(string term);
        void Add(IEnumerable<string> terms);
        int Count { get; }
        string this[int index] { get; }
    }

    internal class SearchTermCollection : ISearchTermCollection
    {
        private readonly IList<string> _terms;

        public string this[int index] => _terms[index];
        /// <returns>
        /// Gets the number of elements contained in the SearchCollection
        /// </returns>
        public int Count => _terms.Count;

        public SearchTermCollection()
        {
            _terms = new List<string>();
        }

        public void Add(string term)
        {
            if (IsValid(term))
            {
                _terms.Add(term);
            }
        }

        private static bool IsValid(string term)
        {
            return !string.IsNullOrWhiteSpace(term);
        }

        public void Add(IEnumerable<string> terms)
        {
            foreach (var term in terms)
            {
                Add(term);
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _terms.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}