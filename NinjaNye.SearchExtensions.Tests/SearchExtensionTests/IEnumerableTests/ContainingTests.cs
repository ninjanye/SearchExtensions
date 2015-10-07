using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class SearchChildrenTests
    {
        [Test]
        public void SearchChild_SearchChildCollection_ReturnParentType()
        {
            //Arrange
            var parent = new ParentTestData
            {
                Children = new List<TestData>
                {
                    new TestData {Name = "child", Description = "child data", Number = 3},
                    new TestData {Name = "child", Description = "child data", Number = 4},
                    new TestData {Name = "child", Description = "child data", Number = 5},
                    new TestData {Name = "child", Description = "child data", Number = 6}
                }
            };
            var testData = new List<ParentTestData> {parent};

            //Act
            var result = testData.Search(p => p.Children)
                                 .With(c => c.Number)
                                 .Between(3,6);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        public void SearchChild_SearchChildCollectionForStringMatch_ReturnParentType()
        {
            //Arrange
            var expected1 = new TestData {Name = "chris", Description = "child data", Number = 3};
            var expected2 = new TestData {Name = "josh", Description = "child data", Number = 6};
            var parent = new ParentTestData
            {
                Children = new List<TestData>
                {
                    expected1,
                    new TestData {Name = "fred", Description = "child data", Number = 4},
                    new TestData {Name = "teddy", Description = "child data", Number = 5},
                    expected2
                }
            };
            var testData = new List<ParentTestData> { parent };

            //Act
            var result = testData.Search(p => p.Children)
                                 .With(c => c.Name)
                                 .Containing("s");

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(result, expected1);
            CollectionAssert.Contains(result, expected2);
        }
    }


    [TestFixture]
    public class ContainingTests
    {
        private List<TestData> _testData = new List<TestData>();
        private readonly TestData _matchingItem1 = new TestData { Name = "searching this", Description = "chin" };
        private readonly TestData _matchingItem2 = new TestData { Name = "searching this", Description = "sea" };
        private readonly TestData _matchingItem3 = new TestData { Name = "look here", Description = "sea", Status = "chelsea"};
        private readonly TestData _matchingItem4 = new TestData { Name = "in status", Description = "miss", Status = "status"};
        private readonly TestData _unmatchingItem = new TestData { Name = "searching this", Description = "no match", Number = 1 };
        private readonly TestData _nullItem = new TestData();

        [SetUp]
        public void ClassSetup()
        {
            this._testData = new List<TestData>();
            this.BuildTestData();
        }

        [TearDown]
        public void TearDown()
        {
            this._testData.Clear();
        }

        private void BuildTestData()
        {
            this._testData.Add(this._matchingItem1);
            this._testData.Add(this._unmatchingItem);
            this._testData.Add(this._matchingItem2);
            this._testData.Add(this._matchingItem3);
            this._testData.Add(this._nullItem);
            this._testData.Add(this._matchingItem4);
        }

        [Test]
        public void Containing_CompareAgainstAnotherProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act
            this._testData.Search(x => x.Name).Containing(x => x.Description);

            //Assert
            Assert.Pass("No exception thrown");
        }

        [Test]
        public void Containing_ComapareAgainstAnotherProperty_DoesNotReturnUnmatchedData()
        {
            //Arrange
            
            //Act
            var result = this._testData.Search(x => x.Name).Containing(x => x.Description);

            //Assert
            CollectionAssert.DoesNotContain(result, this._unmatchingItem);
        }

        [Test]
        public void Containing_ComapareAgainstAnotherProperty_ReturnsAllMatchedData()
        {
            //Arrange
            
            //Act
            var result = this._testData.Search(x => x.Name).Containing(x => x.Description).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.Contains(result, this._matchingItem2);
        }

        [Test]
        public void Containing_SearchTwoProperties_ReturnsRecordWithMatchedDataInSecondProperty()
        {
            //Arrange
            
            //Act
            var result = this._testData.Search(x => x.Name, x => x.Status).Containing(x => x.Description).ToList();

            //Assert
            CollectionAssert.Contains(result, this._matchingItem3);
        }

        [Test]
        public void Containing_SearchAgainstMultipleProperties_ReturnMatchingItemOnSecondProperty()
        {
            //Arrange
            
            //Act
            var result = this._testData.Search(x => x.Name).Containing(x => x.Description, x => x.Status).ToList();

            //Assert
            CollectionAssert.Contains(result, this._matchingItem4);
        }
    }
}