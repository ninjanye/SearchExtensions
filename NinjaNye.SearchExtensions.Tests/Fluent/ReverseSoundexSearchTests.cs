using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Soundex;
using NinjaNye.SearchExtensions.Tests.SearchExtensionTests;

namespace NinjaNye.SearchExtensions.Tests.Fluent
{
    [TestFixture]
    public class ReverseSoundexSearchTests
    {
        private List<TestData> testData = new List<TestData>();

        [SetUp]
        public void ClassSetup()
        {
            this.testData = new List<TestData>();
            this.BuildTestData();
        }

        private void BuildTestData()
        {
            this.testData.Add(new TestData { Name = "arrange", Description = "", Number = 1 });
            this.testData.Add(new TestData { Name = "estrange", Description = "", Number = 2 });
            this.testData.Add(new TestData { Name = "Taint", Description = "", Number = 3 });
            this.testData.Add(new TestData { Name = "Paint", Description = "", Number = 4 });
        }

        [Test]
        public void SoundsLike_SearchSingleWord_ReturnsMatchingRecord()
        {
            //Arrange

            //Act
            var result = this.testData.Search(x => x.Name).ReverseSoundex("range");

            //Assert
            CollectionAssert.Contains(result, this.testData[0]);
        }

        [Test]
        public void SoundsLike_SearchSingleWord_DoesNotReturnsNonMatchingRecords()
        {
            //Arrange

            //Act
            var result = this.testData.Search(x => x.Name).ReverseSoundex("range");

            //Assert
            CollectionAssert.DoesNotContain(result, this.testData[3]);
        }

        [Test]
        public void SoundsLike_SearchMultipleWords_ReturnsAllMatchingRecords()
        {
            //Arrange
            var names = new[] { "range", "point" };
            var soundexCodes = names.Select(w => w.ToReverseSoundex());
            var expected = this.testData.Where(td => soundexCodes.Contains(td.Name.ToReverseSoundex()));

            //Act
            var result = this.testData.Search(x => x.Name).ReverseSoundex(names);

            //Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}