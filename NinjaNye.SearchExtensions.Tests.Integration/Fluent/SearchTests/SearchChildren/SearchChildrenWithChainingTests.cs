using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [Collection("Database tests")]
    public class SearchChildrenWithChainingTests
    {
        private readonly TestContext _context;

        public SearchChildrenWithChainingTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void SearchChildren_SearchIntegerAndString_ResultsMatchBothOccurrences()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                            .With(c => c.IntegerOne)
                                            .EqualTo(101)
                                            .With(c => c.StringTwo)
                                            .Containing("two")
                                            .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
        }

        [Fact]
        public void SearchChildren_SearchIntegerAndInteger_ResultsMatchBothOccurrences()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                            .With(c => c.IntegerOne)
                                            .EqualTo(101)
                                            .With(c => c.IntegerThree)
                                            .EqualTo(200)
                                            .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }

        [Fact]
        public void SearchChildren_SearchStringAndInteger_ResultsMatchBothOccurrences()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                            .With(c => c.StringTwo)
                                            .EqualTo("child one")
                                            .With(c => c.IntegerOne)
                                            .EqualTo(101)
                                            .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }
    }
}