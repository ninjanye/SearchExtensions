using System.Collections.Generic;

namespace NinjaNye.SearchExtensions.Levenshtein
{
    public interface ILevenshteinDistance<out T>
    {
        int Distance { get; }
        T Item { get; }
        int[] Distances { get; }
    }

    internal class LevenshteinDistance<T> : ILevenshteinDistance<T>
    {
        public LevenshteinDistance(T item, params int[] distances)
        {
            Item = item;
            Distances = distances;
            Distance = distances[0];
        }

        public int Distance { get; }
        public T Item { get; }
        public int[] Distances { get; }
    }
}