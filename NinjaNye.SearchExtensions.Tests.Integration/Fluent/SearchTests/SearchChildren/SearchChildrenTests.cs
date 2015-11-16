using System;
using System.Linq;
using NinjaNye.SearchExtensions.Tests.Integration.Models;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [TestFixture]
    internal class SearchChildrenTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void SearchChild_SearchChildCollection_ReturnParentType()
        {
            //Arrange

            //Act
            var result = this._context.TestModels.SearchChildren(x => x.Children);

            //Assert
            CollectionAssert.AreEquivalent(this._context.TestModels, result);
        }

        [Test]
        public void SearchChild_SearchChildCollection_ResultIsQueryable()
        {
            //Arrange

            //Act
            var result = this._context.TestModels.SearchChildren(x => x.Children);

            //Assert
            Assert.That(result, Is.InstanceOf<IQueryable<TestModel>>());
        }

        [Test]
        public void SearchChild_SearchChildCollection_ProviderIsSet()
        {
            //Arrange

            //Act
            var result = this._context.TestModels.SearchChildren(x => x.Children);

            //Assert
            Assert.That(result.Provider, Is.EqualTo(((IQueryable) this._context.TestModels).Provider));
        }

        [Test]
        public void SearchChild_SearchChildCollection_ElementTypeIsSet()
        {
            //Arrange

            //Act
            var result = this._context.TestModels.SearchChildren(x => x.Children);

            //Assert
            Assert.That(result.ElementType, Is.EqualTo(((IQueryable) this._context.TestModels).ElementType));
        }

        [Test]
        public void SearchChild_SearchChildCollection_CanSelectChildParameters()
        {
            //Arrange

            //Act
            var result = this._context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .ToList();

            //Assert
            Assert.That(result, Is.EqualTo(this._context.TestModels));
        }

        [Test]
        public void SearchChild_SearchChildCollection_ReturnsCorrectRecords()
        {
            //Arrange

            //Act
            var query = this._context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .EqualTo(50);

            var result = query.ToList();
            //Assert

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
        }

        [Test]
        public void SearchChild_SearchChildCollection_MatchAgainstTwoValues()
        {
            //Arrange

            //Act
            var query = this._context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne)
                                                 .EqualTo(50, 1);

            var result = query.ToList();
            //Assert

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }

        [Test]
        public void SearchChild_SearchMultipleChildrensProperties_ResultMatchesAgainstAnyProperty()
        {
            //Arrange

            //Act
            var query = this._context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.IntegerOne, c => c.IntegerThree)
                                                 .EqualTo(1);

            var result = query.ToList();
            //Assert

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }

        [Test]
        public void SearchChildren_SearchMultipleChildCollections_ResultMatchString()
        {
            //Arrange

            //Act

            var result = this._context.TestModels.SearchChildren(x => x.Children, x => x.OtherChildren)
                                                 .With(c => c.StringOne)
                                                 .EqualTo("child test")
                                                 .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }

        [Test]
        public void SearchChildren_SearchMultipleChildCollections_ResultMatchIntegers()
        {
            //Arrange

            //Act

            var result = this._context.TestModels.SearchChildren(x => x.Children, x => x.OtherChildren)
                                                 .With(c => c.IntegerOne)
                                                 .EqualTo(3)
                                                 .ToList();

            //Assert
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