using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Models;
using NinjaNye.SearchExtensions.Tests.SearchExtensionTests;

namespace NinjaNye.SearchExtensions.Tests.Fluent
{
    [TestFixture]
    public class FluentRankedTests
    {
        private List<TestData> _testData = new List<TestData>();

        [SetUp]
        public void ClassSetup()
        {
            _testData = new List<TestData>();
            BuildTestData();
        }

        private void BuildTestData()
        {
            _testData.Add(new TestData { Name = "abcd", Description = "efgh", Number = 1 });
            _testData.Add(new TestData { Name = "ijkl", Description = "mnop", Number = 2 });
            _testData.Add(new TestData { Name = "qrst", Description = "uvwx", Number = 3 });
            _testData.Add(new TestData { Name = "yzab", Description = "cdef", Number = 4 });
            _testData.Add(new TestData { Name = "efgh", Description = "ijkl", Number = 5 });
            _testData.Add(new TestData { Name = "UPPER", Description = "CASE", Number = 6 });
            _testData.Add(new TestData { Name = "lower", Description = "case", Number = 7 });
            _testData.Add(new TestData { Name = "tweeter", Description = "cheese", Number = 8 });
        }

        [Test]
        public void ToRanked_SearchedForData_RankedResultIsReturned()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Description).Containing("c").ToRanked();

            //Assert
            Assert.IsInstanceOf<IEnumerable<IRanked<TestData>>>(result);
        }

        [Test]
        public void ToRanked_CorrectRankReturned()
        {
            var result = _testData.Search(x => x.Name).ContainingAll("wee").ToLeftWeightedRanked();
            var first = result.OrderByDescending(r => r.Hits).First();
            var check = first.Hits;
            //as 'wee' is one char into string, it should add (7 - 1) to the hit count. - should add 6
            Assert.AreEqual(7, first.Hits);

            result = _testData.Search(x => x.Name).ContainingAll("ete").ToLeftWeightedRanked();
            first = result.OrderByDescending(r => r.Hits).First();

            //as 'ete' is three char into string, it should add (7 - 3) to the hit count. - should add 4
            Assert.AreEqual(5, first.Hits);
        }


        [Test]
        public void ToRanked_SearchOneColumn_RankIsCorrect()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).Containing("e").ToRanked().ToList();

            //Assert
            Assert.AreEqual(3, result.Count);
            var first = result.OrderByDescending(r => r.Hits).ToList();
            Assert.AreEqual(3, first[0].Hits);
            Assert.AreEqual(1, first[1].Hits);
        }

        [Test]
        public void ToRanked_SearchMultipleColumns_RankIsCombined()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name, x => x.Description)
                             .Containing("c")
                             .ToRanked()
                             .ToList();

            //Assert
            Assert.AreEqual(4, result.Count);
            var ordered = result.OrderByDescending(r => r.Hits).ToList();
            Assert.AreEqual(1, ordered[0].Hits);
        }

        [Test]
        public void ToRanked_CultureSetToIgnoreCase_RankIgnoresCase()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Description)
                                      .SetCulture(StringComparison.OrdinalIgnoreCase)
                                      .Containing("c")
                                      .ToRanked()
                                      .ToList();

            //Assert
            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result.All(r => r.Hits == 1));
        }

        [Test]
        public void ToRanked_SearchRankedSearch_OnlyRetrieveMatchingBothSearches()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name)
                                      .Containing("e")
                                      .ToRanked()
                                      .Search(x => x.Item.Description)
                                      .Containing("k")
                                      .StartsWith("i");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().Hits);
        }

        [Test]
        public void ToRanked_SearchedForDataStartingWith_RankedResultIsReturned()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Description)
                                       .SetCulture(StringComparison.OrdinalIgnoreCase)
                                       .StartsWith("c").ToRanked();

            //Assert
            Assert.AreEqual(4, result.Count());
            Assert.AreEqual(1, result.First().Hits);
        }

        [Test]
        public void ToRanked_SearchedForDataEndsWith_RankedResultIsReturned()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Description)
                                       .EndsWith("e")
                                       .ToRanked();

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(1, result.First().Hits);
        }
    }
}