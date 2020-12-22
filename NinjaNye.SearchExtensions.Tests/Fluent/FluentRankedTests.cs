using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using NinjaNye.SearchExtensions.Models;
using NinjaNye.SearchExtensions.Tests.SearchExtensionTests;
// ReSharper disable StringLiteralTypo

namespace NinjaNye.SearchExtensions.Tests.Fluent
{    
    public class FluentRankedTests
    {
        private readonly List<TestData> _testData;
        
        public FluentRankedTests()
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

        [Fact]
        public void ToRanked_SearchedForData_RankedResultIsReturned()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Description).Containing("c").ToRanked();

            //Assert
            Assert.IsAssignableFrom<IEnumerable<IRanked<TestData>>>(result);
        }

        [Fact]
        public void ToRanked_CorrectRankReturned()
        {
            var result = _testData.Search(x => x.Name).SetCulture(StringComparison.OrdinalIgnoreCase)
                                                      .ContainingAll("p").ToRanked();
            var first = result.OrderByDescending(r => r.Hits).First();
            Assert.Equal(2, first.Hits);
            Assert.Equal("UPPER", first.Item.Name);
        }

        [Fact]
        public void ToRanked_SearchOneColumn_RankIsCorrect()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).Containing("e").ToRanked().ToList();

            //Assert
            Assert.Equal(3, result.Count);
            var first = result.OrderByDescending(r => r.Hits).ToList();
            Assert.Equal(3, first[0].Hits);
            Assert.Equal(1, first[1].Hits);
        }

        [Fact]
        public void ToRanked_SearchMultipleColumns_RankIsCombined()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name, x => x.Description)
                             .Containing("c")
                             .ToRanked()
                             .ToList();

            //Assert
            Assert.Equal(4, result.Count);
            var ordered = result.OrderByDescending(r => r.Hits).ToList();
            Assert.Equal(1, ordered[0].Hits);
        }

        [Fact]
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
            Assert.Equal(4, result.Count);
            Assert.True(result.All(r => r.Hits == 1));
        }

        [Fact]
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
            Assert.Single(result);
            Assert.Equal(1, result.First().Hits);
        }

        [Fact]
        public void ToRanked_SearchedForDataStartingWith_RankedResultIsReturned()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Description)
                                       .SetCulture(StringComparison.OrdinalIgnoreCase)
                                       .StartsWith("c").ToRanked().ToList();

            //Assert
            Assert.Equal(4, result.Count());
            Assert.Equal(1, result.First().Hits);
        }

        [Fact]
        public void ToRanked_SearchedForDataEndsWith_RankedResultIsReturned()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Description)
                                       .EndsWith("e")
                                       .ToRanked()
                                       .ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(1, result.First().Hits);
        }
    }
}