using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [Collection("Database tests")]
    public class LessThanTests 
    {
        private readonly TestContext _context;

        public LessThanTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void SearchChild_SearchChildrenLessThan_ResultMatchesAgainstAnyProperty()
        {
            //Arrange

            //Act
            var query = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne, c => c.IntegerTwo)
                                                 .LessThan(5);

            var result = query.ToList();
            //Assert

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }

        [Fact]
        public void SearchChild_SearchChildrenLessThanOrEqualTo()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .LessThanOrEqualTo(0)
                                                 .ToList();

            //Assert

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }
    }
}