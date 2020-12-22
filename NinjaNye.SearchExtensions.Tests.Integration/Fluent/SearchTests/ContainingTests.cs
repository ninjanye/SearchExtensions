using System;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [Collection("Database tests")]
    public class ContainingTests
    {
        private readonly TestContext _context;

        public ContainingTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void Containing_SearchWholeWordsOnly_ReturnsOnlyRecordsWithWholeWordMatches()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .Containing("word");

            //Assert
            Assert.Single(result);
        }

        [Fact]
        public void Containing_SearchWholeWordsOnly_ReturnsRecordsWithWholeWordAtTheStart()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .Containing("whole");

            //Assert
            Assert.Single(result);
        }

        [Fact]
        public void Containing_SearchWholeWordsOnly_ReturnsRecordsWithWholeWordAtTheEnd()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .Containing("match");

            //Assert
            Assert.Single(result);
            Assert.Contains(result, x => x.Id == new Guid("CADA7A13-931A-4CF0-B4F4-49160A743251"));
        }

        [Fact]
        public void Containing_SearchWholeWordsOnly_ReturnsRecordsWithSearchedWord()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .Containing("wholewordmatch");

            //Assert
            Assert.Single(result);
            Assert.Contains(result, x => x.Id == new Guid("A8AD8A4F-853B-417A-9C0C-0A2802560B8C"));
        }        
    }
}