using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Tests.SearchExtensionTests;

namespace NinjaNye.SearchExtensions.Tests.Fluent
{
    [TestFixture]
    public class FluentRankedTests
    {
        private List<TestData> testData = new List<TestData>();

        [SetUp]
        public void ClassSetup()
        {
            this.testData = new List<TestData>();
            this.BuildTestData();
        }

        private void BuildTestData()
        {
            this.testData.Add(new TestData {Name = "abcd", Description = "efgh", Number = 1});
            this.testData.Add(new TestData {Name = "ijkl", Description = "mnop", Number = 2});
            this.testData.Add(new TestData {Name = "qrst", Description = "uvwx", Number = 3});
            this.testData.Add(new TestData {Name = "yzab", Description = "cdef", Number = 4});
            this.testData.Add(new TestData {Name = "efgh", Description = "ijkl", Number = 5});
            this.testData.Add(new TestData {Name = "UPPER", Description = "CASE", Number = 6});
            this.testData.Add(new TestData {Name = "lower", Description = "case", Number = 7});
            this.testData.Add(new TestData {Name = "tweeter", Description = "cheese", Number = 8});
        }

        [Test]
        public void ToRanked_SearchedForData_RankedResultIsReturned()
        {
            //Arrange

            //Act
            var result = this.testData.Search(x => x.Description).Containing("c").ToRanked();

            //Assert
            Assert.IsInstanceOf<IEnumerable<IRanked<TestData>>>(result);
        }

        [Test]
        public void ToRanked_SearchOneColumn_RankIsCorrect()
        {
            //Arrange

            //Act
            var result = this.testData.Search(x => x.Name).Containing("e").ToRanked().ToList();

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
            var result = this.testData.Search(x => x.Name, x => x.Description)
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
            var result = this.testData.Search(x => x.Description)
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
            var result = this.testData.Search(x => x.Name)
                                      .Containing("e")
                                      .ToRanked()
                                      .Search(x => x.Item.Description)
                                      .Containing("k")
                                      .StartsWith("i");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().Hits);
        }
    }
}