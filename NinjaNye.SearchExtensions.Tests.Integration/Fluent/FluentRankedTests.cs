using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Fluent;
using NinjaNye.SearchExtensions.Tests.Integration.Models;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [TestFixture]
    public class  FluentRankedTests
    {
        readonly TestContext context = new TestContext();

        [Test]
        public void ToRanked_SearchedForData_RankedResultIsReturned()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringTwo).Containing("c").ToRanked();

            //Assert
            Assert.IsInstanceOf<IQueryable<IRanked<TestModel>>>(result);
        }

        [Test]
        public void ToRanked_SearchOneColumn_RankIsCorrect()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).Containing("g").ToRanked();

            //Assert
            Assert.AreEqual(2, result.Count());
            var first = result.OrderByDescending(r => r.Hits).ToList();
            Assert.AreEqual(2, first[0].Hits);
            Assert.AreEqual(1, first[1].Hits);
        }

        [Test]
        public void ToRanked_SearchMultipleColumns_RankIsCombined()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                                                .Containing("c")
                                                .ToRanked();

            //Assert
            Assert.AreEqual(6, result.Count());
            var ordered = result.OrderByDescending(r => r.Hits).ToList();
            Assert.AreEqual(4, ordered[0].Hits);
        }

        [Test]
        public void ToRanked_SearchRankedSearch_OnlyRetrieveMatchingBothSearches()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne)
                                                .Containing("g")
                                                .ToRanked()
                                                .Search(x => x.Item.StringTwo)
                                                .Containing("k")
                                                .StartsWith("i");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().Hits);
        }
    }
}