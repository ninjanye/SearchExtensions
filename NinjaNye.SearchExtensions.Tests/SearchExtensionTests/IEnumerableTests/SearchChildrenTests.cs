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
            this._dataOne = new TestData { Name = "chris", Description = "child data", Number = 1, Age = 20};
            this._dataTwo = new TestData { Name = "fred", Description = "child data", Number = 6, Age = 30 };
            this._dataThree = new TestData { Name = "teddy", Description = "child data", Number = 2, Age = 40 };
            this._dataFour = new TestData { Name = "josh", Description = "child data", Number = 20, Age = 50 };
            this._parent = new ParentTestData
            {
                Children = new List<TestData> {this._dataOne, this._dataTwo},
                OtherChildren = new List<TestData> { this._dataThree, this._dataFour }
            };
            this._otherParent = new ParentTestData
            {
                Children = new List<TestData> {this._dataThree, this._dataFour},
                OtherChildren = new List<TestData> { this._dataOne, this._dataTwo }
            };
            this._testData = new List<ParentTestData> { this._parent, this._otherParent };            
        }

        [Test]
        public void SearchChild_SearchChildCollectionWithoutProperty_ReturnParent()
        {
            //Arrange

            //Act
            var result = this._testData.SearchChildren(p => p.Children);

            //Assert
            CollectionAssert.AreEqual(_testData, result);
        }

        [Test]
        public void SearchChild_SearchChildCollection_ReturnOnlyParentWhosChildNumberIsGreaterThanTen()
        {
            //Arrange
            
            //Act
            var result = this._testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .GreaterThan(10)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.All(p => p.Children.Any(c => c.Number > 10)), Is.True);
        }

        [Test]
        public void SearchChildren_SearchChildCollectionWithPropertyGreaterThan()
        {
            //Arrange
            
            //Act
            var result = this._testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .GreaterThan(4)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, _parent);
            CollectionAssert.Contains(result, _otherParent);
            Assert.That(result.All(p => p.Children.Any(c => c.Number > 4)), Is.True);
        }

        [Test]
        public void SearchChildren_SearchChildCollectionWithPropertyGreaterThanOrEqualTo()
        {
            //Arrange
            
            //Act
            var result = this._testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .GreaterThanOrEqualTo(6)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, _parent);
            CollectionAssert.Contains(result, _otherParent);
            Assert.That(result.All(p => p.Children.Any(c => c.Number >= 6)), Is.True);
        }

        [Test]
        public void SearchChildren_SearchChildCollectionWithPropertyLessThan()
        {
            //Arrange
            
            //Act
            var result = this._testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .LessThan(2)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            CollectionAssert.Contains(result, _parent);
        }

        [Test]
        public void SearchChildren_SearchChildCollectionWithPropertyLessThanOrEqualTo()
        {
            //Arrange
            
            //Act
            var result = this._testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .LessThanOrEqualTo(2)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, _parent);
            CollectionAssert.Contains(result, _otherParent);
        }

        [Test]
        public void SearchChildren_SearchChildCollectionWithPropertyLessThanAndGreaterThan()
        {
            //Arrange
            
            //Act
            var result = this._testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .LessThan(10)
                                       .GreaterThan(2)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            CollectionAssert.Contains(result, _parent);
        }

        [Test]
        public void SearchChildren_SearchChildCollectionWithPropertyBetween()
        {
            //Arrange
            
            //Act
            var result = this._testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .Between(2, 10)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            CollectionAssert.Contains(result, _parent);
        }

        [Test]
        public void SearchChildren_SearchChildCollectionWithPropertyEqualTo()
        {
            //Arrange
            
            //Act
            var result = this._testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .EqualTo(2)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            CollectionAssert.Contains(result, _otherParent);
        }

        [Test]
        public void SearchChildren_SearchChildCollectionWithPropertyEqualToAnyOneOfMultiple()
        {
            //Arrange
            
            //Act
            var result = this._testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .EqualTo(2, 6)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, _parent);
            CollectionAssert.Contains(result, _otherParent);
        }

        [Test]
        public void SearchChildren_SearchChildCollectionWithMultiplePropertiesEqualTo()
        {
            //Arrange
            
            //Act
            var result = this._testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number, c => c.Age)
                                       .EqualTo(20)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, _parent);
            CollectionAssert.Contains(result, _otherParent);
        }

        [Test]
        public void SearchChildren_SearchMultipleChildCollectionsWithPropertyEqualTo()
        {
            //Arrange

            //Act
            var result = this._testData.SearchChildren(p => p.Children, p => p.OtherChildren)
                                       .With(c => c.Number)
                                       .EqualTo(20)
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, _parent);
            CollectionAssert.Contains(result, _otherParent);
        }

        [Test]
        public void SearchChildren_SearchMultipleChildCollectionsWithStringPropertyEqualTo()
        {
            //Arrange

            //Act
            var result = this._testData.SearchChildren(p => p.Children, p => p.OtherChildren)
                                       .With(c => c.Name)
                                       .EqualTo("chris")
                                       .ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, _parent);
            CollectionAssert.Contains(result, _otherParent);
        }
    }
}