using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace NinjaNye.SearchExtensions.Performance
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [SimpleJob(RuntimeMoniker.Net50)]
    [BenchmarkCategory("StringSearch")]
    public class StringSearchTests
    {
        [Params(1000)] 
        public static int WordsToSearch;
        private static List<string> _data;
        private static string[] _searchTerms;
        private const StringComparison STRING_COMPARISON = StringComparison.CurrentCulture;

        [GlobalSetup]
        public static void BuildWords()
        {
            Console.WriteLine("Building {0} records....", WordsToSearch);
            _data = BuildData(WordsToSearch);
            _searchTerms = new[] { "abc", "def", "ghi", "JKL", "mno", "pqr", "stu", "vwx" };
        }        

        [Benchmark]
        public void StringSearch_SearchExtensions()
        {
            var _ = _data.Search(s => s)
                .Containing(_searchTerms)
                .ToList();
        }

        [Benchmark(Baseline = true)]
        public void StringSearch_Linq()
        {
            var _ = _data.Where(s => s.IndexOf("abc", STRING_COMPARISON) > -1
                                     || s.IndexOf("def", STRING_COMPARISON) > -1
                                     || s.IndexOf("ghi", STRING_COMPARISON) > -1
                                     || s.IndexOf("jkl", STRING_COMPARISON) > -1
                                     || s.IndexOf("mno", STRING_COMPARISON) > -1
                                     || s.IndexOf("pqr", STRING_COMPARISON) > -1
                                     || s.IndexOf("stu", STRING_COMPARISON) > -1
                                     || s.IndexOf("vwx", STRING_COMPARISON) > -1).ToList();

        }
        
        [Benchmark]
        public void StringSearch_Any()
        {
            var _ = _data.Where(s => _searchTerms.Any(st => s.IndexOf(st, STRING_COMPARISON) > -1)).ToList();
        }

        private static List<string> BuildData(int recordCount)
        {
            var enumerableData = new List<string>();
            for (var i = 0; i < recordCount; i++)
            {
                enumerableData.Add(Guid.NewGuid().ToString());
            }

            return enumerableData;
        }

    }
}