using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    /// <summary>
    /// Summary description for PerformanceTests
    /// </summary>
    [TestFixture]
    public class PerformanceTests
    {
        private readonly List<string> enumerableData = new List<string>();
            
        [TestFixtureSetUp]
        public void ClassSetup()
        {
            for (int i = 0; i < 1000000; i++)
            {
                Guid guid = Guid.NewGuid();
                enumerableData.Add(guid.ToString());
            }
        }

        [Test]
        public void SearchAMillionRecords_SearchShouldTakeNoLongerThan1second()
        {
            //Arrange
            var stopwatch = Stopwatch.StartNew();

            //Act
            var result = enumerableData.Search("abc", s => s, StringComparison.OrdinalIgnoreCase).ToList();

            //Assert
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Found: " + result.Count);
            Assert.IsTrue(1 > stopwatch.Elapsed.Seconds);
        }

        [Test]
        public void SearchAMillionRecords_SearchShouldBeWithin20pcOfEquivalentLambdaSearch()
        {
            //Arrange
            var stopwatch = Stopwatch.StartNew();

            //Act
            var lambdaResult = enumerableData.Where(s => s.IndexOf("abc", StringComparison.OrdinalIgnoreCase) > -1).ToList();
            stopwatch.Stop();
            var lambdaTime = stopwatch.Elapsed;
            stopwatch.Restart();
            var result = enumerableData.Search("abc", s => s, StringComparison.OrdinalIgnoreCase).ToList();
            stopwatch.Stop();   
            var searchTime = stopwatch.Elapsed;

            //Assert
            Console.WriteLine("Lambda time taken: {0}", lambdaTime);
            Console.WriteLine("Search time taken: {0}", searchTime);
            double threshold = lambdaTime.Ticks + (lambdaTime.Ticks*0.15);
            Console.WriteLine("Time threshhold: {0}", new TimeSpan((int) threshold));
            Assert.LessOrEqual(searchTime.Ticks, threshold);
        }
    }
}
