using System.Linq;

namespace NinjaNye.SearchExtensions.Levenshtein
{
    public interface ILevenshteinDistance<out T>
    {
        int Distance { get; }

        /// <summary>
        /// The queried item
        /// </summary>
        T Item { get; }
        
        /// <summary>
        /// A collection of all distances calculated
        /// </summary>
        int[] Distances { get; }

        /// <summary>
        /// The minimum distance of all levenshtein calculations
        /// </summary>
        int MinimumDistance { get; }

        /// <summary>
        /// The maximum distance of all levenshtein calculations
        /// </summary>
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