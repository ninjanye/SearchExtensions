using System;
using System.Linq;
using NinjaNye.SearchExtensions.Tests.Integration.Models;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests.SearchChildren
{
    [TestFixture]
    internal class StringTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void SearchChildren_SearchForMatchingString_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .EqualTo("child test")
                                                 .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
        }

        [Test]
        public void SearchChildren_SearchForStringContaining_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .Containing("hil")
                                                 .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));

        }

        [Test]
        public void SearchChildren_SearchForAnyMatchingString_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .Containing("test", "child")
                                                 .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));

        }

        [Test]
        public void SearchChildren_SearchForPropertiesContainingAllStrings_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .ContainingAll("child", "another")
                                                 .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));

        }

        [Test]
        public void SearchChildren_SearchForAnyMatchingWholeWords_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .Matching(SearchType.WholeWords)
                                                 .Containing("st")
                                                 .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void SearchChildren_SearchForRecordsStartingWith_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .StartsWith("ano")
                                                 .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
        }

        [Test]
        public void SearchChildren_SearchForRecordsStartingWithMultiple_CorrectResultsAreReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .StartsWith("ano", "tes")
                                                 .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("24726ECC-953E-4F95-88AA-91E0C0B52D00")));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
        }

        [Test]
        public void SearchChildren_SearchForRecordsEndsWith_CorrectResultsAreReturned()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .EndsWith("est")
                                                 .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Any(tm => tm.Id == Guid.Parse("F672552D-2787-468D-8D2E-DE1E88F83E21")));
        }

        [Test]
        public void SearchChildren_SearchForRecordsEndingWithAny_CorrectResultsAreReturned()
        {
            //Arrange

            //Act
            var result = _context.TestModels.SearchChildren(x => x.Children)
                                                 .With(c => c.StringOne)
                                                 .EndsWith("est", "ild")
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