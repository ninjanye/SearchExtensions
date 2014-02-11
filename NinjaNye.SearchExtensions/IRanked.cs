namespace NinjaNye.SearchExtensions
{
    public interface IRanked<out T>
    {
        int Hits { get; }
        T Item { get; }
    }
}