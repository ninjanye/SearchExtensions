using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [TestFixture]
    internal class LessThanTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void SearchChild_SearchChildrenLessThan_ResultMatchesAgainstAnyProperty()
        {
            //Arrange

            //Act
            var query = this._context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne, c => c.IntegerTwo)
                                                 .LessThan(5);

            var result = query.ToList();
            //Assert

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }

        [Test]
        public void SearchChild_SearchChildrenLessThanOrEqualTo()
        {
            //Arrange

            //Act
            var result = this._context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .LessThanOrEqualTo(0)
                                                 .ToList();

            //Assert

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}