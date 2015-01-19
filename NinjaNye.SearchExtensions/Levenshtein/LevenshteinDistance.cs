namespace NinjaNye.SearchExtensions.Levenshtein
{
    internal class LevenshteinDistance<T> : ILevenshteinDistance<T>
    {
        public int Distance { get; set; }
        public T Item { get; set; }
    }
}