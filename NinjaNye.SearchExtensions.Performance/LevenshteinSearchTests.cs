using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NinjaNye.SearchExtensions.Levenshtein;

namespace NinjaNye.SearchExtensions.Performance
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [SimpleJob(RuntimeMoniker.Net50)]
    [BenchmarkCategory("Levenshtein")]
    public class LevenshteinSearchTests
    {
        [Benchmark(OperationsPerInvoke = 1000)]
        public void LevenshteinSearch_SearchExtensions() => LevenshteinProcessor.LevenshteinDistance(Guid.NewGuid().ToString(), "searchString");
    }
}