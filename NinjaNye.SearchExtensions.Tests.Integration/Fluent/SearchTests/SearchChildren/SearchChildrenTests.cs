using System;
using System.Linq;
using NinjaNye.SearchExtensions.Tests.Integration.Models;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [Collection("Database tests")]
    public class SearchChildrenTests 
    {
        private readonly TestContext _context;

        public SearchChildrenTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void SearchChild_SearchChildCollection_ReturnParentType()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children);

            //Assert
            Assert.Equal(_context.TestModels, result.ToList());
        }

        [Fact]
        public void SearchChild_SearchChildCollection_ResultIsQueryable()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children);

            //Assert
            Assert.IsAssignableFrom<IQueryable<TestModel>>(result);
        }

        [Fact]
        public void SearchChild_SearchChildCollection_ProviderIsSet()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children);

            //Assert
            Assert.Equal(((IQueryable)_context.TestModels).Provider, result.Provider);
        }

        [Fact]
        public void SearchChild_SearchChildCollection_ElementTypeIsSet()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children);

            //Assert
            Assert.Equal(((IQueryable) _context.TestModels).ElementType, result.ElementType);
        }

        [Fact]
        public void SearchChild_SearchChildCollection_CanSelectChildParameters()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .ToList();

            //Assert
            Assert.Equal(_context.TestModels, result);
        }

        [Fact]
        public void SearchChild_SearchChildCollection_ReturnsCorrectRecords()
        {
            //Arrange

            //Act
            var query = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .EqualTo(50);

            var result = query.ToList();
            //Assert

            Assert.Single(result);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
        }

        [Fact]
        public void SearchChild_SearchChildCollection_MatchAgainstTwoValues()
        {
            //Arrange

            //Act
            var query = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .EqualTo(50, 1);

            var result = query.ToList();
            //Assert

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }

        [Fact]
        public void SearchChild_SearchMultipleChildrensProperties_ResultMatchesAgainstAnyProperty()
        {
            //Arrange

            //Act
            var query = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne, c => c.IntegerThree)
                                                 .EqualTo(1);

            var result = query.ToList();
            //Assert

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }

        [Fact]
        public void SearchChildren_SearchMultipleChildCollections_ResultMatchString()
        {
            //Arrange

            //Act

            var result = _context.TestModels.SearchChildren(x => x.Children, x => x.OtherChildren)
                                                 .With(c => c.StringOne)
                                                 .EqualTo("child test")
                                                 .ToList();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }

        [Fact]
        public void SearchChildren_SearchMultipleChildCollections_ResultMatchIntegers()
        {
            //Arrange

            //Act

            var result = _context.TestModels.SearchChildren(x => x.Children, x => x.OtherChildren)
                                                 .With(c => c.IntegerOne)
                                                 .EqualTo(3)
                                                 .ToList();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }
    }
}