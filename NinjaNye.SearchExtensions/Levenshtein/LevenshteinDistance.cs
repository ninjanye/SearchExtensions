using System.Collections.Generic;
using System.Linq;

namespace NinjaNye.SearchExtensions.Levenshtein
{
    public interface ILevenshteinDistance<out T>
    {
        int Distance { get; }
        T Item { get; }
        int[] Distances { get; }
        int MinimumDistance { get; }
        int MaximumDistance { get; }
    }

    internal class LevenshteinDistance<T> : ILevenshteinDistance<T>
    {
        public LevenshteinDistance(T item, params int[] distances)
        {
            Item = item;
            Distances = distances;
        }

        public int Distance => Distances.First();
        public T Item { get; }
        public int[] Distances { get; }

        public int MinimumDistance => Distances.Min();
        public int MaximumDistance => Distances.Max();
    }
}