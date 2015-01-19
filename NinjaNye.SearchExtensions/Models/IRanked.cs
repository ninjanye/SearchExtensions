namespace NinjaNye.SearchExtensions.Models
{
    public interface IRanked<out T>
    {
        int Hits { get; }
        T Item { get; }
    }
}