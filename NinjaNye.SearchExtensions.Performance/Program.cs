using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NinjaNye.SearchExtensions.Fluent;

namespace NinjaNye.SearchExtensions.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====================================");
            Console.WriteLine(" SearchExtensions performance tests ");
            Console.WriteLine("====================================");

            const int recordCount = 1000000;
            const StringComparison stringComparison = StringComparison.CurrentCulture;
            var stopwatch = new Stopwatch();


            Console.WriteLine("Building {0} records...", recordCount);
            List<string> enumerableData = new List<string>();
            for (int i = 0; i < recordCount; i++)
            {
                enumerableData.Add(Guid.NewGuid().ToString());
            }
            string[] searchTerms = new[] { "abc", "def", "ghi", "JKL", "mno", "pqr", "stu", "vwx" };

            Console.WriteLine("Begin search...");
            stopwatch.Start();
            var result = enumerableData.Search(searchTerms, s => s, stringComparison).ToList();
            stopwatch.Stop();
            Console.WriteLine("Record count: {0}", recordCount);
            Console.WriteLine("Results found: {0}", result.Count);
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Search complete...");
            Console.WriteLine();
            Console.WriteLine("==================================");
            Console.WriteLine();
            Console.WriteLine("Begin lamda...");
            stopwatch.Reset();
            stopwatch.Start();
            var lamdaResult = enumerableData.Where(s => s.IndexOf("abc", stringComparison) > -1
                                                     || s.IndexOf("def", stringComparison) > -1
                                                     || s.IndexOf("ghi", stringComparison) > -1
                                                     || s.IndexOf("jkl", stringComparison) > -1
                                                     || s.IndexOf("mno", stringComparison) > -1
                                                     || s.IndexOf("pqr", stringComparison) > -1
                                                     || s.IndexOf("stu", stringComparison) > -1
                                                     || s.IndexOf("vwx", stringComparison) > -1).ToList();

            stopwatch.Stop();
            Console.WriteLine("Results found: {0}", lamdaResult.Count);
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Lamda complete...");
            Console.WriteLine();
            Console.WriteLine("==================================");
            Console.WriteLine();

            Console.WriteLine("Begin .Any() search...");
            stopwatch.Reset();
            stopwatch.Start();
            var containsResult = enumerableData.Where(s => searchTerms.Any(st => s.IndexOf(st) > -1)).ToList();
            stopwatch.Stop();
            Console.WriteLine("Results found: {0}", containsResult.Count);
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Any() search complete...");
            Console.WriteLine();
            Console.WriteLine("==================================");
            Console.WriteLine();

            Console.WriteLine("Begin fluent search...");
            stopwatch.Reset();
            stopwatch.Start();
            var fluentResult = enumerableData.Search(s => s).Containing(searchTerms).ToList();
            stopwatch.Stop();
            Console.WriteLine("Record count: {0}", recordCount);
            Console.WriteLine("Results found: {0}", fluentResult.Count);
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Fluent search complete...");
            Console.WriteLine();
            Console.WriteLine("==================================");
            Console.ReadLine();
        }
    }
}
