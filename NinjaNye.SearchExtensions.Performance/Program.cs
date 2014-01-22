using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            const string searchTerm = "ab";
            const StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;

            Console.WriteLine("Building {0} records...", recordCount);
            List<string> enumerableData = new List<string>();
            for (int i = 0; i < recordCount; i++)
            {
                enumerableData.Add(Guid.NewGuid().ToString());
            }

            Console.WriteLine("Begin search...");
            var stopwatch = Stopwatch.StartNew();
            var result = enumerableData.Search(s => s, searchTerm, stringComparison).ToList();
            stopwatch.Stop();
            Console.WriteLine("Search complete...");
            Console.WriteLine("Record count: {0}", recordCount);
            Console.WriteLine();
            Console.WriteLine("Results found: {0}", result.Count);
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);

            Console.WriteLine("Begin lamda...");
            stopwatch.Reset();
            stopwatch.Start();
            var lamdaResult = enumerableData.Where(s => s.IndexOf(searchTerm, stringComparison) > -1).ToList();
            stopwatch.Stop();
            Console.WriteLine("Lamda complete...");
            Console.WriteLine("Results found: {0}", lamdaResult.Count);
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);

            Console.ReadLine();
        }
    }
}
