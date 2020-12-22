using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [Collection("Database tests")]
    public class ContainingAllTests 
    {
        private readonly TestContext _context;

        public ContainingAllTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void SearchContainingAll_OneTermSupplied_ReturnsOnlyRecordsContainingSearchTerm()
        {
            //Arrange
            const string searchTerm = "test";

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).ContainingAll(searchTerm);

            //Assert
            Assert.NotEmpty(result);
            Assert.All(result, t => Assert.True(t.StringOne.IndexOf(searchTerm, StringComparison.Ordinal) >= 0));
        }

        [Fact]
        public void SearchContainingAll_TwoTermsSupplied_ReturnsRecordsContainingBothSearchTerms()
        {
            //Arrange
            const string searchTerm1 = "test";
            const string searchTerm2 = "search";

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).ContainingAll(searchTerm1, searchTerm2).ToList();

            //Assert
            Assert.NotEmpty(result);
            var hasBothTerms = result.All(t => t.StringOne.IndexOf(searchTerm1, StringComparison.Ordinal) >= 0 &&
                                               t.StringOne.IndexOf(searchTerm2, StringComparison.Ordinal) >= 0);
            Assert.True(hasBothTerms);
        }

        [Fact]
        public void SearchContainingAll_TwoPropertiesAndTwoTermsSupplied_ReturnsDataWhereAllTermsAreMatched()
        {
            //Arrange
            const string searchTerm1 = "test";
            const string searchTerm2 = "search";
            const string searchTerm3 = "windmill";

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                .ContainingAll(searchTerm1, searchTerm2, searchTerm3)
                .ToList();

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(t => (t.StringOne.Contains(searchTerm1) || t.StringTwo.Contains(searchTerm1))
                                     && (t.StringOne.Contains(searchTerm2) || t.StringTwo.Contains(searchTerm2))
                                     && (t.StringOne.Contains(searchTerm3) || t.StringTwo.Contains(searchTerm3))
                              ));
        }

        [Fact]
        public void ContainingAll_SearchAgainstOneProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try
            {
                _context.TestModels.Search(x => x.StringOne).ContainingAll(x => x.StringTwo);
            }
            catch (Exception)
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void ContainingAll_SearchAgainstOneProperty_DoesNotReturnNull()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).ContainingAll(x => x.StringTwo);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ContainingAll_SearchAgainstOneProperty_AllResultsHaveAStringTwoInStringOne()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).ContainingAll(x => x.StringTwo).ToList();

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.StringOne.Contains(x.StringTwo)));
        }

        [Fact]
        public void ContainingAll_SearchAgainstTwoProperties_AllResultsHaveBothProperties()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                                                .ContainingAll(x => x.StringTwo, x => x.StringThree)
                                                .ToList();

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.StringOne.Contains(x.StringTwo) && x.StringOne.Contains(x.StringThree)));
        }
    }
}