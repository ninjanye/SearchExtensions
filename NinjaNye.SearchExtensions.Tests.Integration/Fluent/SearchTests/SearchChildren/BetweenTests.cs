using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [Collection("Database tests")]
    public class BetweenTests
    {
        private readonly TestContext _context;

        public BetweenTests(DatabaseIntegrationTests @base)
        {
            _context = @base._context;
        }

        [Fact]
        public void SearchChild_SearchChildCollection_ReturnsCorrectRecords()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .Between(100, 200)
                                                 .ToList();
            //Assert

            Assert.Equal(result.Count, 2);
            Assert.True(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
            Assert.True(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }
    }
}