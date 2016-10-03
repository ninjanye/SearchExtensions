using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Models;
using NinjaNye.SearchExtensions.Tests.Integration.Models;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [TestFixture]
    public class  FluentRankedTests
    {
        readonly TestContext _context = new TestContext();

        [Test]
        public void ToRanked_SearchedForData_RankedResultIsReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringTwo).Containing("c").ToRanked();

            //Assert
            Assert.IsInstanceOf<IQueryable<IRanked<TestModel>>>(result);
        }

        [Test]
        public void ToRanked_SearchOneColumn_RankIsCorrect()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Containing("g").ToRanked();

            //Assert
            Assert.AreEqual(4, result.Count());
            var first = result.OrderByDescending(r => r.Hits).ToList();
            Assert.AreEqual(2, first[0].Hits);
            Assert.AreEqual(1, first[1].Hits);
        }

        [Test]
        public void ToRanked_SearchMultipleColumns_RankIsCombined()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                                                .Containing("c")
                                                .ToRanked();

            //Assert
            var ordered = result.OrderByDescending(r => r.Hits).ToList();
            Assert.AreEqual(4, ordered[0].Hits);
        }

        [Test]
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
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().Hits);
        }

        [Test]
        public void ToRanked_SearchedForData_RankedStartsWithSearch()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                .StartsWith("t")
                .ToRanked();

            //Assert
            Assert.AreEqual(8, result.Count());
            Assert.AreEqual(3, result.First().Hits);
        }

        [Test]
        public void ToRanked_SearchedForData_RankedEndsWithSearch()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                .EndsWith("t")
                .ToRanked();

            //Assert
            Assert.AreEqual(14, result.Count());
        }

        [Test]
        public void ToRanked_SearchedForData_LeftWeightedRankedResults()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Containing("wordmatch")
                .ToRanked(RankedType.LeftWeighted);

            //Assert
            Assert.AreEqual(10, result.First().Hits);
        }
    }
}