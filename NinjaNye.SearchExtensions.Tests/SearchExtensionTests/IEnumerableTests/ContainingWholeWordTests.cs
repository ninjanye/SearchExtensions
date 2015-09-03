using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class ContainingWholeWordTests
    {
        [Test]
        public void Containing_SearchWholeWordsOnly_CorrectResultReturned()
        {
            //Arrange
            var expected = new TestData{Description = "an expected result"};
            var unexpected = new TestData{Description = "an unexpected result"};
            var data = new List<TestData> {expected, unexpected};

            //Act
            var result = data.Search(x => x.Description)
                .Matching(SearchTypeEnum.WholeWords)
                .Containing("expected")
                .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(expected));
        }

        [Test]
        public void Containing_SearchWholeWordsOnly_ResultReturnedIfMatchingWordIsFirstWord()
        {
            //Arrange
            var expected = new TestData{Description = "expect result"};
            var unexpected = new TestData{Description = "expecting another result"};
            var data = new List<TestData> {expected, unexpected};

            //Act
            var result = data.Search(x => x.Description)
                .Matching(SearchTypeEnum.WholeWords)
                .Containing("expect")
                .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(expected));
        }

        [Test]
        public void Containing_SearchWholeWordsOnly_ResultReturnedIfMatchingWordIsLastWord()
        {
            //Arrange
            var expected = new TestData{Description = "result expected"};
            var unexpected = new TestData { Description = "result unexpected" };
            var data = new List<TestData> {expected, unexpected};

            //Act
            var result = data.Search(x => x.Description)
                .Matching(SearchTypeEnum.WholeWords)
                .Containing("expected")
                .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(expected));
        }

        [Test]
        public void Containing_ChangeMatchType_CorrectResultReturned()
        {
            //Arrange
            var expected = new TestData { Description = "an expected result" };
            var unexpected = new TestData { Description = "an not expected item" };
            var data = new List<TestData> { expected, unexpected };

            //Act
            var result = data.Search(x => x.Description)
                .Matching(SearchTypeEnum.WholeWords)
                .Containing("expected")
                .Matching(SearchTypeEnum.AnyOccurrence)
                .Containing("res")
                .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(expected));
        }

    }
}