using System;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Tests.Integration.Models;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [TestFixture]
    internal class StartsWithTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void StartsWith_SearchStartsWith_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => _context.TestModels.Search(x => x.StringOne).StartsWith(x => x.StringTwo));
        }

        [Test]
        public void StartsWith_SearchStartsWith_ReturnsQueryableStringSearch()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).StartsWith(x => x.StringTwo);

            //Assert
            Assert.IsInstanceOf<QueryableStringSearch<TestModel>>(result);
        }

        [Test]
        public void StartsWith_SearchStartsWithProperty_AllRecordsStartWithStringTwo()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).StartsWith(x => x.StringTwo);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.StringOne.StartsWith(x.StringTwo)));
        }

        [Test]
        public void StartsWith_SearchStartsWithAgainstTwoProperties_AllRecordsStartWithEitherProperty()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                                                .StartsWith(x => x.StringTwo, x => x.StringThree);

            //Assert
            Assert.IsTrue(result.Any(x => x.StringOne.StartsWith(x.StringThree)));
        }

        [Test]
        public void StartsWith_SearchTwoPropertiesStartsWithOneProperty_AnyRecordMatchesSecondSearchedProperty()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo).StartsWith(x => x.StringThree);

            //Assert
            Assert.IsTrue(result.Any(x => x.StringTwo.StartsWith(x.StringThree)));
        }

        [Test]
        public void StartsWith_SearchPropertyMatchingWholeWord_MatchesSingleWord()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                                                .Matching(SearchType.WholeWords)
                                                .StartsWith(x => x.StringThree);

            //Assert
            Assert.That(result.Select(x => x.Id), Contains.Item(new Guid("624CFA9C-4FA1-4680-880D-AAB6507A3014")));
        }

        [Test]
        public void StartsWith_SearchPropertyMatchingWholeWord_MatchesWholeWordsOnly()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne)
                                                .Matching(SearchType.WholeWords)
                                                .StartsWith(x => x.StringThree);

            //Assert
            Assert.That(result.Select(x => x.Id), Is.Not.Contains(new Guid("A8AD8A4F-853B-417A-9C0C-0A2802560B8C")));
            Assert.That(result.Select(x => x.Id), Contains.Item(new Guid("CADA7A13-931A-4CF0-B4F4-49160A743251")));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}