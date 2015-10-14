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
        private ParentTestData _otherParent;

        [SetUp]
        public void SetUp()
        {
            this._dataOne = new TestData { Name = "chris", Description = "child data", Number = 1 };
            this._dataTwo = new TestData { Name = "fred", Description = "child data", Number = 6 };
            this._dataThree = new TestData { Name = "teddy", Description = "child data", Number = 2 };
            this._dataFour = new TestData { Name = "josh", Description = "child data", Number = 20 };
            this._parent = new ParentTestData
            {
                Children = new List<TestData> {this._dataOne, this._dataTwo}
            };
            this._otherParent = new ParentTestData
            {
                Children = new List<TestData> {this._dataThree, this._dataFour}
            };
            this._testData = new List<ParentTestData> { this._parent, this._otherParent };            
        }

        [Test]
        public void SearchChild_SearchChildCollectionWithoutProperty_ReturnParent()
        {
            //Arrange

            //Act
            var result = this._testData.Search(p => p.Children);

            //Assert
            CollectionAssert.AreEqual(_testData, result);
        }

        [Test]
        public void SearchChild_SearchChildCollection_ReturnParentType()
        {
            //Arrange

            //Act
            var result = this._testData.Search(p => p.Children)
                                       .With(c => c.Number);

            //Assert
            Assert.That(result is IEnumerable<ParentTestData>, Is.True);
            CollectionAssert.AreEqual(_testData, result);
        }

        [Test]
        public void SearchChild_SearchChildCollection_ReturnOnlyParentWhosChildNumberIsGreaterThanTen()
        {
            //Arrange
            
            //Act
            var result = this._testData.Search(p => p.Children)
                                       .With(c => c.Number)
                                       .GreaterThan(10)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.All(p => p.Children.Any(c => c.Number > 10)), Is.True);
        }

        [Test]
        public void SearchChildren_SearchChildCollectionWithPropertyGreaterThan_ReturnOnlyParentWhosChildNumberIsGreaterThan4()
        {
            //Arrange
            
            //Act
            var result = this._testData.Search(p => p.Children)
                                       .With(c => c.Number)
                                       .GreaterThan(4)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, _parent);
            CollectionAssert.Contains(result, _otherParent);
            Assert.That(result.All(p => p.Children.Any(c => c.Number > 4)), Is.True);
        }

        //[Test]
        //public void SearchChild_SearchChildCollection_ReturnParentType()
        //{
        //    //Arrange

        //    //Act
        //    var result = this._testData.Search(p => p.Children)
        //        .With(c => c.Number)
        //        .Between(3,6);

        //    //Assert
        //    Assert.That(result.Count(), Is.EqualTo(2));
        //    CollectionAssert.Contains(result, this._parent);
        //    CollectionAssert.Contains(result, this._otherParent);
        //}

        //public void SearchChild_SearchChildCollectionForStringMatch_ReturnParentType()
        //{
        //    //Arrange

        //    //Act
        //    var result = this._testData.Search(p => p.Children)
        //        .With(c => c.Name)
        //        .Containing("s");

        //    //Assert
        //    Assert.That(result.Count(), Is.EqualTo(2));
        //    CollectionAssert.Contains(result, this._dataOne);
        //    CollectionAssert.Contains(result, this._dataFour);
        //}
    }
}