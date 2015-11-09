using System;
using System.Linq;
using NinjaNye.SearchExtensions.Tests.Integration.Models;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [TestFixture]
    internal class SearchChildrenWithChainingTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void SearchChildren_SearchStringAndInteger_ResultsMatchBothOccurences()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                            .With(c => c.IntegerOne)
                                            .EqualTo(101)
                                            .With(c => c.StringOne)
                                            .EqualTo("child test")
                                            .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
        }

        [Test]
        public void SearchChildren_SearchIntegerAndString_ResultsMatchBothOccurences()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                            .With(c => c.StringOne)
                                            .EqualTo("child test")
                                            .With(c => c.IntegerOne)
                                            .EqualTo(101)
                                            .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
        }


        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}