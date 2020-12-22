using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [Collection("Database tests")]
    public class EndsWithTests
    {
        private readonly TestContext _context;

        public EndsWithTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void EndsWith_SearchPropertyMatchingWholeWord_MatchesWholeWordsOnly()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .EndsWith(x => x.StringTwo);

            //Assert
            IQueryable<Guid> guids = result.Select(x => x.Id);
            Assert.False(guids.Contains(new Guid("A8AD8A4F-853B-417A-9C0C-0A2802560B8C")));
            Assert.True(guids.Contains(new Guid("CADA7A13-931A-4CF0-B4F4-49160A743251")));
        }

        [Fact]
        public void EndsWith_SearchPropertyMatchingWholeWord_MatchesSingleWord()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .EndsWith(x => x.StringThree);

            //Assert
            Assert.True(result.Select(x => x.Id).Contains(new Guid("624CFA9C-4FA1-4680-880D-AAB6507A3014")));
        }
    }
}