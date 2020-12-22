using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [Collection("Database tests")]
    public class StringTests
    {
        private readonly TestContext _context;

        public StringTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void SearchChildren_SearchForMatchingString_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .EqualTo("child test")
                                                 .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
        }

        [Fact]
        public void SearchChildren_SearchForStringContaining_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .Containing("hil")
                                                 .ToList();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));

        }

        [Fact]
        public void SearchChildren_SearchForAnyMatchingString_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .Containing("test", "child")
                                                 .ToList();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));

        }

        [Fact]
        public void SearchChildren_SearchForPropertiesContainingAllStrings_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .ContainingAll("child", "another")
                                                 .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));

        }

        [Fact]
        public void SearchChildren_SearchForAnyMatchingWholeWords_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .Matching(SearchType.WholeWords)
                                                 .Containing("st")
                                                 .ToList();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void SearchChildren_SearchForRecordsStartingWith_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .StartsWith("ano")
                                                 .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }

        [Fact]
        public void SearchChildren_SearchForRecordsStartingWithMultiple_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .StartsWith("ano", "tes")
                                                 .ToList();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
        }

        [Fact]
        public void SearchChildren_SearchForRecordsEndsWith_CorrectResultsAreReturned()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .EndsWith("est")
                                                 .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
        }

        [Fact]
        public void SearchChildren_SearchForRecordsEndingWithAny_CorrectResultsAreReturned()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .EndsWith("est", "ild")
                                                 .ToList();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }
    }
}