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
            _context = @base._context;
        }

        [Fact]
        public void SearchChildren_SearchIntegerAndString_ResultsMatchBothOccurences()
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
            Assert.Equal(1, result.Count);
            Assert.True(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
        }

        [Fact]
        public void SearchChildren_SearchIntergerAndInteger_ResultsMatchBothOccurences()
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
            Assert.Equal(1, result.Count);
            Assert.True(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }

        [Fact]
        public void SearchChildren_SearchStringAndInteger_ResultsMatchBothOccurences()
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
            Assert.Equal(1, result.Count);
            Assert.True(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}