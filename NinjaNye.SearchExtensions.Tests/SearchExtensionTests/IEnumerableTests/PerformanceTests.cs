using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    /// <summary>
    /// Summary description for PerformanceTests
    /// </summary>
    [TestFixture]
    public class PerformanceTests
    {
        private List<string> enumerableData = new List<string>();
            
        [TestFixtureSetUp]
        public void ClassSetup()
        {
            var random = new Random();
            for (int i = 0; i < 1000000; i++)
            {
                enumerableData.Add(random.Next(1,1000).ToString());
            }
        }

        [Test]
        public void SearchAMillionRecords_SearchShouldTakeNoLongerThan1second()
        {
            //Arrange
            var stopwatch = Stopwatch.StartNew();

            //Act
            var result = enumerableData.Search(s => s, "12", StringComparison.OrdinalIgnoreCase).ToList();

            //Assert
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Assert.IsTrue(1 > stopwatch.Elapsed.Seconds);
        }
    }
}
