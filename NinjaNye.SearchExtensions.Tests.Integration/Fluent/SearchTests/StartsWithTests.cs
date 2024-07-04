using System;
using System.Linq;
using Xunit;
using NinjaNye.SearchExtensions.Tests.Integration.Models;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [Collection("Database tests")]
    public class StartsWithTests(DatabaseIntegrationTests @base)
    {
        private readonly TestContext _context = @base.Context;

        [Fact]
        public void StartsWith_SearchStartsWith_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try
            {
                _context.TestModels.Search(x => x.StringOne).StartsWith(x => x.StringTwo);
            }
            catch (Exception)
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void StartsWith_SearchStartsWith_ReturnsQueryableStringSearch()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).StartsWith(x => x.StringTwo);

            //Assert
            Assert.IsType<QueryableStringSearch<TestModel>>(result);
        }

        [Fact]
        public void StartsWith_SearchStartsWithProperty_AllRecordsStartWithStringTwo()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).StartsWith(x => x.StringTwo);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.StringOne.StartsWith(x.StringTwo)));
        }

        [Fact]
        public void StartsWith_SearchStartsWithAgainstTwoProperties_AllRecordsStartWithEitherProperty()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).StartsWith(x => x.StringTwo, x => x.StringThree);

            //Assert
            Assert.Contains(result, x =>  x.StringOne.StartsWith(x.StringThree ?? ""));
        }

        [Fact]
        public void StartsWith_SearchTwoPropertiesStartsWithOneProperty_AnyRecordMatchesSecondSearchedProperty()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo).StartsWith(x => x.StringThree);

            //Assert
            Assert.Contains(result, x => x.StringTwo.StartsWith(x.StringThree));
        }

        [Fact]
        public void StartsWith_SearchPropertyMatchingWholeWord_MatchesSingleWord()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .StartsWith(x => x.StringThree);

            //Assert
            Assert.True(result.Select(x => x.Id).Contains(new Guid("624CFA9C-4FA1-4680-880D-AAB6507A3014")));
        }

        [Fact]
        public void StartsWith_SearchPropertyMatchingWholeWord_MatchesWholeWordsOnly()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .StartsWith(x => x.StringThree);
            
            //Assert
            var guids = result.Select(x => x.Id).ToList();
            Assert.DoesNotContain(guids, x => x == new Guid("A8AD8A4F-853B-417A-9C0C-0A2802560B8C"));
            Assert.Contains(guids, x => x == new Guid("CADA7A13-931A-4CF0-B4F4-49160A743251"));
        }
    }
}