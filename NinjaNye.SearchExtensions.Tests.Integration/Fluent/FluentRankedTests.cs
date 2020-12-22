using System.Linq;
using NinjaNye.SearchExtensions.Models;
using NinjaNye.SearchExtensions.Tests.Integration.Models;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [Collection("Database tests")]
    public class  FluentRankedTests 
    {
        private readonly TestContext _context;

        public FluentRankedTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void ToRanked_SearchedForData_RankedResultIsReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringTwo).Containing("c").ToRanked();

            //Assert
            Assert.IsAssignableFrom<IQueryable<IRanked<TestModel>>>(result);
        }

        [Fact]
        public void ToRanked_SearchOneColumn_RankIsCorrect()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Containing("g").ToRanked();

            //Assert
            Assert.Equal(4, result.Count());
            var first = result.OrderByDescending(r => r.Hits).ToList();
            Assert.Equal(2, first[0].Hits);
            Assert.Equal(1, first[1].Hits);
        }

        [Fact]
        public void ToRanked_SearchMultipleColumns_RankIsCombined()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                                                .Containing("c")
                                                .ToRanked();

            //Assert
            var ordered = result.OrderByDescending(r => r.Hits).ToList();
            Assert.Equal(4, ordered[0].Hits);
        }

        [Fact]
        public void ToRanked_SearchRankedSearch_OnlyRetrieveMatchingBothSearches()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                                                .Containing("g")
                                                .ToRanked()
                                                .Search(x => x.Item.StringTwo)
                                                .Containing("k")
                                                .StartsWith("i");

            //Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Hits);
        }

        [Fact]
        public void ToRanked_SearchedForData_RankedStartsWithSearch()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                .StartsWith("t")
                .ToRanked();

            //Assert
            Assert.Equal(8, result.Count());
            Assert.Equal(3, result.First().Hits);
        }

        [Fact]
        public void ToRanked_SearchedForData_RankedEndsWithSearch()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                .EndsWith("t")
                .ToRanked();

            //Assert
            Assert.Equal(14, result.Count());
        }
    }
}