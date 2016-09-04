using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class EndsWithTests
    {
        private List<TestData> _testData = new List<TestData>();

        [SetUp]
        public void ClassSetup()
        {
            _testData = new List<TestData>();
        }

        [TearDown]
        public void TearDown()
        {
            _testData.Clear();
        }

        [Test]
        public void EndsWith_ComparedToAnExistingProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => _testData.Search(x => x.Name).EndsWith(x => x.Description));
        }

        [Test]
        public void EndsWith_ComparedToAnExistingProperty_ResultIsNotNull()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).EndsWith(x => x.Description);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Search_AllowEndsWithMethod_AllResultsEndWithSearchTerm()
        {
            //Arrange
            _testData.Add(new TestData { Name = "test" });
            _testData.Add(new TestData { Name = "false" });

            //Act
            var result = _testData.Search(x => x.Name).EndsWith("st");

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Name.EndsWith("st")));
        }

        [Test]
        public void Search_EndsWithMultipleTerms_SearchAgainstMultipleTerms()
        {
            //Arrange
            _testData.Add(new TestData { Name = "abcd" });
            _testData.Add(new TestData { Name = "efgh" });
            var notPresent = new TestData {Name = "not found"};
            _testData.Add(notPresent);

            //Act
            var result = _testData.Search(x => x.Name).EndsWith("cd", "gh");

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsFalse(result.Contains(notPresent));
        }

        [Test]
        public void EndsWith_ComparedToAnExistingProperty_ResultEndsWithExistingProperty()
        {
            //Arrange
            _testData.Add(new TestData { Name = "efgh", Description = "gh" });
            var notPresent = new TestData {Name = "no match", Description = "test"};
            _testData.Add(notPresent);

            //Act
            var result = _testData.Search(x => x.Name).EndsWith(x => x.Description);

            //Assert
            Assert.IsTrue(result.Any(), "No records returned");
            Assert.IsFalse(result.Contains(notPresent));
        }

        [Test]
        public void EndsWith_ComparedToTwoExistingProperties_ResultEndsWithEitherOfExistingProperties()
        {
            //Arrange
            _testData.Add(new TestData { Name = "abcd", Status = "cd" });
            _testData.Add(new TestData { Name = "efgh", Description = "gh" });
            var notPresent = new TestData { Name = "no match", Description = "test" };
            _testData.Add(notPresent);

            //Act
            var result = _testData.Search(x => x.Name).EndsWith(x => x.Description, x => x.Status);

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsFalse(result.Contains(notPresent));
        }

        [Test]
        public void EndsWith_ComparedToTwoExistingPropertiesWithNullValue_NullValueIsIgnored()
        {
            //Arrange
            _testData.Add(new TestData { Name = "abcd", Description = "no match", Status = "cd" });
            _testData.Add(new TestData { Name = "efgh", Description = "gh" });
            var notPresent = new TestData { Name = "no match", Description = "test" };
            _testData.Add(notPresent);

            //Act
            var result = _testData.Search(x => x.Name).EndsWith(x => x.Description, x => x.Status);

            //Assert
            Assert.IsTrue(result.Any(), "No records returned");
            Assert.IsTrue(result.All(x => x.Name.EndsWith(x.Description) || (x.Status != null && x.Name.EndsWith(x.Status))));
        }

        [Test]
        public void EndsWith_SearchTwoPropertiesComparedToAProperty_ResultsContainAllPermiatations()
        {
            //Arrange
            _testData.Add(new TestData { Name = "abcd", Description = "efgh", Status = "cd" });
            _testData.Add(new TestData { Name = "zzzz", Description = "match", Status = "ch"});
            _testData.Add(new TestData { Name = "zzzz", Description = "yyyy", Status = "xxxx"});

            //Act
            var result = _testData.Search(x => x.Name, x => x.Description).EndsWith(x => x.Status);

            //Assert
            Assert.AreEqual(2, result.Count(), "Not enough records returned");
            Assert.IsTrue(result.All(x => x.Name.EndsWith(x.Status) || x.Description.EndsWith(x.Status)));
        }

        [Test]
        public void EndsWith_SearchPropertyWithIgnoreCaseCulture_ResultsAreCaseInsensitive()
        {
            //Arrange
            _testData.Add(new TestData { Name = "a TEST", Description = "test" });
            _testData.Add(new TestData { Name = "zzzz", Description = "yyyy" });

            //Act
            var result = _testData.Search(x => x.Name)
                                      .SetCulture(StringComparison.OrdinalIgnoreCase)
                                      .EndsWith(x => x.Description);

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(t => t.Description == "test"));
        }

        [Test]
        public void EndsWith_SearchOccursTwice_ReturnExpectedRecord()
        {
            //Arrange
            _testData.Add(new TestData { Name = "tastiest", Description = "two occurences of st", Number = 8 });

            //Act
            var result = _testData.Search(x => x.Name).EndsWith("st");

            //Assert
            Assert.IsTrue(result.Any(td => td.Number == 8));
        }

        [Test]
        public void EndsWith_SerchAcrossTwoProperties_ResultEndsWithTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "test";
            _testData.Add(new TestData { Name = "a test", Description = "zzzz" });
            _testData.Add(new TestData { Name = "zzzz", Description = "another test" });

            //Act
            var result = _testData.Search(x => x.Name, x => x.Description).EndsWith(searchTerm);

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name.EndsWith(searchTerm) || x.Description.EndsWith(searchTerm)));
        }
    }
}