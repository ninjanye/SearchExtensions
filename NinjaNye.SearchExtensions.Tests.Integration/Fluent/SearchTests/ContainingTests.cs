using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [TestFixture]
    internal class ContainingTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void Containing_SearchWholeWordsOnly_ReturnsOnlyRecordsWithWholeWordMatches()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .Containing("word");

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Containing_SearchWholeWordsOnly_ReturnsRecordsWithWholeWordAtTheStart()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .Containing("whole");

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Containing_SearchWholeWordsOnly_ReturnsRecordsWithWholeWordAtTheEnd()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .Containing("match");

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.Any(x => x.Id == new Guid("CADA7A13-931A-4CF0-B4F4-49160A743251")), Is.True);
        }

        [Test]
        public void Containing_SearchWholeWordsOnly_ReturnsRecordsWithSearchedWord()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                .Matching(SearchType.WholeWords)
                .Containing("wholewordmatch");

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.Any(x => x.Id == new Guid("A8AD8A4F-853B-417A-9C0C-0A2802560B8C")), Is.True);
        }

        public void Dispose()
        {
            _context.Dispose();
        }        
    }
}