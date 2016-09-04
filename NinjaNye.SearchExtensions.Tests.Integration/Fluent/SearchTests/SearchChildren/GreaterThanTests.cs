using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [TestFixture]
    internal class GreaterThanTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void SearchChild_SearchChildCollection_ReturnsCorrectRecords()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .GreaterThan(50)
                                                 .ToList();
            //Assert

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }

        [Test]
        public void SearchChild_SearchChildCollection_ReturnsRecordsGreaterThanOrEqualTo()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .GreaterThanOrEqualTo(101)
                                                 .ToList();
            //Assert

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}