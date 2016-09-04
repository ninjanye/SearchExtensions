using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class SearchChildrenChaingingTests
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
            _dataOne = new TestData {Name = "chris", Description = "child data", Number = 1, Age = 60};
            _dataTwo = new TestData {Name = "fred", Description = "child", Number = 20, Age = 30};
            _dataThree = new TestData {Name = "teddy", Description = "data", Number = 2, Age = 40};
            _dataFour = new TestData {Name = "josh", Description = "child data", Number = 20, Age = 50};
            _parent = new ParentTestData
            {
                Children = new List<TestData> {_dataOne, _dataTwo},
            };
            _otherParent = new ParentTestData
            {
                Children = new List<TestData> {_dataThree, _dataFour},
            };
            _testData = new List<ParentTestData> {_parent, _otherParent};
        }

        [Test]
        public void SearchChildren_SearchStringAndInteger_ResultsMatchBothOccurences()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(x => x.Children)
                                            .With(c => c.Name)
                                            .Containing("ed")
                                            .With(c => c.Number)
                                            .EqualTo(20)
                                            .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            CollectionAssert.Contains(result, _parent);
        }

        [Test]
        public void SearchChildren_SearchStringAndString_ResultsMatchBothOccurences()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(x => x.Children)
                                            .With(c => c.Name)
                                            .Containing("ed")
                                            .With(c => c.Description)
                                            .Containing("child")
                                            .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            CollectionAssert.Contains(result, _parent);
        }

        [Test]
        public void SearchChildren_SearchIntegerAndString_ResultsMatchBothOccurences()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(x => x.Children)
                                            .With(c => c.Number)
                                            .EqualTo(20)
                                            .With(c => c.Name)
                                            .Containing("ed")
                                            .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            CollectionAssert.Contains(result, _parent);
        }

        [Test]
        public void SearchChildren_SearchIntegerAndInteger_ResultsMatchBothOccurences()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(x => x.Children)
                                            .With(c => c.Number)
                                            .EqualTo(20)
                                            .With(c => c.Age)
                                            .GreaterThan(40)
                                            .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            CollectionAssert.Contains(result, _otherParent);
        }
        
    }
}