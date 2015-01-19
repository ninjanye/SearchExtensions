namespace NinjaNye.SearchExtensions.Models
{
    internal class Ranked<T> : IRanked<T>
    {
        public int Hits { get; set; }
        public T Item { get; set; }
    }
}