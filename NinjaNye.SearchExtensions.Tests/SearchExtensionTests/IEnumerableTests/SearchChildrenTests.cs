using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class SearchChildrenTests
    {
        private ParentTestData _parent;
        private List<ParentTestData> _testData;
        private TestData _dataOne;
        private TestData _dataFour;
        private TestData _dataTwo;
        private TestData _dataThree;

        [SetUp]
        public void SetUp()
        {
            this._dataOne = new TestData { Name = "chris", Description = "child data", Number = 3 };
            this._dataTwo = new TestData { Name = "fred", Description = "child data", Number = 4 };
            this._dataThree = new TestData { Name = "teddy", Description = "child data", Number = 5 };
            this._dataFour = new TestData { Name = "josh", Description = "child data", Number = 6 };
            this._parent = new ParentTestData
            {
                Children = new List<TestData> {this._dataOne, this._dataTwo, this._dataThree, this._dataFour}
            };
            this._testData = new List<ParentTestData> { this._parent };            
        }

        [Test]
        public void SearchChild_SearchChildCollection_ReturnParentType()
        {
            //Arrange

            //Act
            var result = this._testData.Search(p => p.Children)
                .With(c => c.Number)
                .Between(3,6);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, this._dataTwo);
            CollectionAssert.Contains(result, this._dataThree);
        }

        public void SearchChild_SearchChildCollectionForStringMatch_ReturnParentType()
        {
            //Arrange

            //Act
            var result = this._testData.Search(p => p.Children)
                .With(c => c.Name)
                .Containing("s");

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, this._dataOne);
            CollectionAssert.Contains(result, this._dataFour);
        }
    }
}