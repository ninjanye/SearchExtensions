using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NinjaNye.SearchExtensions.Levenshtein;

namespace NinjaNye.SearchExtensions.Performance
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    public class LevenshteinSearchTests
    {
        [Benchmark(OperationsPerInvoke = 1000)]
        public void LevenshteinSearch_SearchExtensions() => LevenshteinProcessor.LevenshteinDistance(Guid.NewGuid().ToString(), "searchString");
    }
}