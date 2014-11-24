namespace NinjaNye.SearchExtensions
{
    internal class LevenshteinDistance<T> : ILevenshteinDistance<T>
    {
        public int Distance { get; set; }
        public T Item { get; set; }
    }
}