namespace NinjaNye.SearchExtensions.Models
{
    public interface IRanked<out T>
    {
        int Hits { get; }
        T Item { get; }
    }

    public class Ranked<T> : IRanked<T>
    {
        public int Hits { get; set; }
        public T Item { get; set; }
    }
}
