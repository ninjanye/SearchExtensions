using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [Collection("Database tests")]
    public class FluentSearchTests
    {
        private readonly TestContext _context;

        public FluentSearchTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void Search_SearchWithoutActionHasNoAffectOnTheResults_ResultsAreUnchanged()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).ToList();

            //Assert
            Assert.Equal(_context.TestModels, result);
        }

        [Fact]
        public void Search_Contains_OnlyResultsContainingTermAreReturned()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Containing("abc").ToList();

            //Assert
            Assert.Single(result);
            Assert.True(result.All(x => x.StringOne.Contains("abc")));
        }

        [Fact]
        public void Search_ContainingNoValidSearchTerms_ReturnsAllRecords()
        {
            //Arrange
            int expectedCount = _context.TestModels.Count();

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Containing("").ToList();

            //Assert
            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public void Search_ContainsThenStartsWith_OnlyResultsThatContainTextAndStartWithTextAreReturned()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Containing("abc").StartsWith("a").ToList();

            //Assert
            Assert.Single(result);
            Assert.True(result.All(x => x.StringOne.Contains("abc") && x.StringOne.StartsWith("a")));
        }

        [Fact]
        public void Search_ContainsThenEndsWith_OnlyResultsThatContainTextAndEndWithTextAreReturned()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Containing("abc").EndsWith("d").ToList();

            //Assert
            Assert.Single(result);
            Assert.True(result.All(x => x.StringOne.Contains("abc") && x.StringOne.EndsWith("d")));
        }

        [Fact]
        public void Search_Equals_DefinedPropertyEqualsSearchResult()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("abcd").ToList();

            //Assert
            Assert.Single(result);
            Assert.True(result.All(x => x.StringOne == "abcd"));
        }

        [Fact]
        public void SearchMultiple_ResultContainsAcrossTwoProperties_ResultContainsTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "cd";

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo).Containing(searchTerm).ToList();

            //Assert
            Assert.Equal(3, result.Count());
            Assert.True(result.All(x => x.StringOne.Contains(searchTerm) || x.StringTwo.Contains(searchTerm)));
        }

        [Fact]
        public void SearchMultiple_ResultStartsWithAcrossTwoProperties_ResultStartsWithTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "ef";

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo).StartsWith(searchTerm).ToList();
            
            //Assert
            Assert.Equal(2, result.Count());
            Assert.True(result.All(x => x.StringOne.StartsWith(searchTerm) || x.StringTwo.StartsWith(searchTerm)));
        }

        [Fact]
        public void SearchMultiple_ResultEndsWithAcrossTwoProperties_ResultEndsWithTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "gh";

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                                                .EndsWith(searchTerm).ToList();
            
            //Assert
            Assert.Equal(3, result.Count());
            Assert.True(result.All(x => x.StringOne.EndsWith(searchTerm) || x.StringTwo.EndsWith(searchTerm)));
        }

        [Fact]
        public void SearchAll_NoPropertiesDefined_AllPropertiesAreSearched()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search().Containing("cd").ToList();

            //Assert
            Assert.Equal(3, result.Count());
            Assert.True(result.All(x => x.StringOne.Contains("cd") || x.StringTwo.Contains("cd")));
        }

        [Fact]
        public void Search_ContainingMultipleTerms_SearchAgainstMultipleTerms()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Containing("ab", "jk").ToList();

            //Assert
            Assert.Equal(3, result.Count());
            Assert.True(result.All(x => x.StringOne.Contains("ab") || x.StringOne.Contains("jk")));
        }

        [Fact]
        public void Search_StartsWithMultipleTerms_SearchAgainstMultipleTerms()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).StartsWith("ab", "ef").ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.True(result.All(x => x.StringOne.StartsWith("ab") || x.StringOne.StartsWith("ef")));

        }

        [Fact]
        public void Search_EndsWithMultipleTerms_SearchAgainstMultipleTerms()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).EndsWith("cd", "gh").ToList();

            //Assert
            Assert.Equal(3, result.Count());
            Assert.True(result.All(x => x.StringOne.EndsWith("cd") || x.StringOne.EndsWith("gh")));

        }

        [Fact]
        public void Search_SearchManyPropertiesContainingManyTerms_AllResultsHaveASearchTermWithin()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo).Containing("cd", "jk").ToList();

            //Assert
            Assert.Equal(5, result.Count());
            Assert.True(result.All(x => x.StringOne.Contains("cd") || x.StringOne.Contains("jk")
                                       || x.StringTwo.Contains("cd") || x.StringTwo.Contains("jk")));
        }

        [Fact]
        public void Search_SearchManyPropertiesStartingWithManyTerms_AllResultsHaveAPropertyStartingWithASpecifiedTerm()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo).StartsWith("cd", "ef").ToList();

            //Assert
            Assert.Equal(3, result.Count());
            Assert.True(result.All(x => x.StringOne.StartsWith("cd") || x.StringOne.StartsWith("ef")
                                       || x.StringTwo.StartsWith("cd") || x.StringTwo.StartsWith("ef")));
        }

        [Fact]
        public void Search_SearchManyPropertiesEndingWithManyTerms_AllResultsHaveAPropertyEndingWithASpecifiedTerm()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo).EndsWith("cd", "gh").ToList();

            //Assert
            Assert.Equal(3, result.Count());
            Assert.True(result.All(x => x.StringOne.EndsWith("cd") || x.StringOne.EndsWith("gh")
                                       || x.StringTwo.EndsWith("cd") || x.StringTwo.EndsWith("gh")));
        }

        [Fact]
        public void Search_SearchContainsIgnoreCase_CaseIsIgnored()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Containing("AB", "jk").ToList();

            //Assert
            Assert.Equal(3, result.Count());
            Assert.True(result.All(x => x.StringOne.Contains("jk") || x.StringOne.Contains("ab")));
        }

        [Fact]
        public void Search_SearchStartsWithIgnoreCase_CaseIsIgnored()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringTwo).StartsWith("C").ToList();

            //Assert
            Assert.True(result.All(x => x.StringTwo.StartsWith("c", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void Search_SearchEndsWithIgnoreCase_CaseIsIgnored()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringTwo).EndsWith("E").ToList();

            //Assert
            Assert.True(result.All(x => x.StringTwo.EndsWith("e", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void Search_SearchIsEqual_CaseIsIgnored()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringTwo).EqualTo("CASE").ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.True(result.All(x => x.StringTwo.Equals("case", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void Search_SearchManyTermsAreEqual_ResultsMatchAnyTerm()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("abcd", "efgh").ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.True(result.All(x => x.StringOne == "abcd" || x.StringOne == "efgh"));
        }

        [Fact]
        public void Search_SearchManyTermsAreEqual_ResultsMatchAnyTermInAnyCase()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("ABCD", "EFGH");

            //Assert
            Assert.Equal(2, result.Count());
            Assert.True(result.All(x => x.StringOne == "abcd" || x.StringOne == "efgh"));
        }

        [Fact]
        public void Search_IncludeAfterSearch_ReturnsResults()
        {
            var result = _context.TestModels.Search(x => x.StringOne)
                                            .ContainingAll("parent", "test")
                                            .Include(x => x.Children);

            var model = result.First();
            Assert.NotNull(model.Children);
        }

        [Fact]
        public void Search_IncludeAfterSearchForInt_ReturnsResults()
        {
            var result = _context.TestModels.Search(x => x.IntegerOne)
                                            .GreaterThan(5)
                                            .Include(x => x.Children);

            var model = result.First();
            Assert.NotNull(model.Children);
        }
    }
}
