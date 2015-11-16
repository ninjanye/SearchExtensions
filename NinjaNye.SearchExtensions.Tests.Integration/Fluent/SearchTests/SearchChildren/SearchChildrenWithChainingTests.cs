using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [TestFixture]
    internal class SearchChildrenWithChainingTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
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
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
        }

        [Test]
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
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }

        [Test]
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
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }


        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}