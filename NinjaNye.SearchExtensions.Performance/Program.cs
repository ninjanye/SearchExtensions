using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            const StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;

            Console.WriteLine("Building {0} records...", recordCount);
            List<string> enumerableData = new List<string>();
            for (int i = 0; i < recordCount; i++)
            {
                enumerableData.Add(Guid.NewGuid().ToString());
            }

            Console.WriteLine("Begin search...");
            var stopwatch = Stopwatch.StartNew();
            string[] searchTerms = new[] {"abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx"};
            var result = enumerableData.Search(searchTerms, s => s, stringComparison).ToList();
            stopwatch.Stop();
            Console.WriteLine("Search complete...");
            Console.WriteLine("Record count: {0}", recordCount);
            Console.WriteLine();
            Console.WriteLine("Results found: {0}", result.Count);
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);

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
            Console.WriteLine("Lamda complete...");
            Console.WriteLine("Results found: {0}", lamdaResult.Count);
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);

            stopwatch.Reset();
            stopwatch.Start();
            var containsResult = enumerableData.Where(s => searchTerms.Any(st => s.IndexOf(st) > -1)).ToList();
            stopwatch.Stop();
            Console.WriteLine("Contains complete...");
            Console.WriteLine("Results found: {0}", containsResult.Count);
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);

            Console.ReadLine();
        }
    }
}
