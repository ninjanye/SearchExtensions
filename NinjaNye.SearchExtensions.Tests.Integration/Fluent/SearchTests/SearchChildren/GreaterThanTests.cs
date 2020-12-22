using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [Collection("Database tests")]
    public class GreaterThanTests
    {
        private readonly TestContext _context;

        public GreaterThanTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void SearchChild_SearchChildCollection_ReturnsCorrectRecords()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .GreaterThan(50)
                                                 .ToList();
            //Assert

            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }

        [Fact]
        public void SearchChild_SearchChildCollection_ReturnsRecordsGreaterThanOrEqualTo()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .GreaterThanOrEqualTo(101)
                                                 .ToList();
            //Assert

            Assert.Equal(2, result.Count);
            Assert.Contains(result, tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21"));
            Assert.Contains(result, tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00"));
        }
    }
}