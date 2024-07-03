using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NinjaNye.SearchExtensions.Soundex;

namespace NinjaNye.SearchExtensions.Performance
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [SimpleJob(RuntimeMoniker.Net50)]
    [BenchmarkCategory("Soundex")]
    public class SoundexSearchTests
    {
        [Benchmark(OperationsPerInvoke = 1000)]
        public void SoundexSearch() => Guid.NewGuid().ToString().ToSoundex();

        [Benchmark(OperationsPerInvoke = 1000)]
        public void ReverseSoundexSearch() => Guid.NewGuid().ToString().ToReverseSoundex();
    }
}