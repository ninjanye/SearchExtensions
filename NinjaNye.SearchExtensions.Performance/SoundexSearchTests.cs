using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NinjaNye.SearchExtensions.Soundex;

namespace NinjaNye.SearchExtensions.Performance
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    public class SoundexSearchTests
    {
        [Benchmark(OperationsPerInvoke = 1000)]
        public void SoundexSearch() => Guid.NewGuid().ToString().ToSoundex();

        [Benchmark(OperationsPerInvoke = 1000)]
        public void ReverseSoundexSearch() => Guid.NewGuid().ToString().ToReverseSoundex();
    }
}