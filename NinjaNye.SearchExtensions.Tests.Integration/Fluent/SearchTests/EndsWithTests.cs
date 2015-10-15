using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [TestFixture]
    internal class EndsWithTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void EndsWith_SearchPropertyMatchingWholeWord_MatchesWholeWordsOnly()
        {
            //Arrange

            //Act
            var result = this._context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .EndsWith(x => x.StringTwo);

            //Assert
            Assert.That(result.Select(x => x.Id), Is.Not.Contains(new Guid("A8AD8A4F-853B-417A-9C0C-0A2802560B8C")));
            Assert.That(result.Select(x => x.Id), Contains.Item(new Guid("CADA7A13-931A-4CF0-B4F4-49160A743251")));
        }

        [Test]
        public void EndsWith_SearchPropertyMatchingWholeWord_MatchesSingleWord()
        {
            //Arrange

            //Act
            var result = this._context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .EndsWith(x => x.StringThree);

            //Assert
            Assert.That(result.Select(x => x.Id), Contains.Item(new Guid("624CFA9C-4FA1-4680-880D-AAB6507A3014")));
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}