namespace NinjaNye.SearchExtensions
{
    public interface ILevenshteinDistance<out T>
    {
        int Distance { get; }
        T Item { get; }
    }
}