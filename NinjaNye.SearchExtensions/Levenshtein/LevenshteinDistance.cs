namespace NinjaNye.SearchExtensions.Levenshtein
{
    public interface ILevenshteinDistance<out T>
    {
        int Distance { get; }
        T Item { get; }
    }

    internal class LevenshteinDistance<T> : ILevenshteinDistance<T>
    {
        public int Distance { get; set; }
        public T Item { get; set; }
    }
}