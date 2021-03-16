using BenchmarkDotNet.Running;

namespace NinjaNye.SearchExtensions.Performance
{
    public class Program
    {
        static void Main(string[] args)
        {
            // BenchmarkRunner.Run<StringSearchTests>();
            BenchmarkRunner.Run<LevenshteinSearchTests>();
//            BenchmarkRunner.Run<SoundexSearchTests>();

            
        }
    }
}
