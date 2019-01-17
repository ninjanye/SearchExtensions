using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    public class EndsWithTests : IDisposable
    {
        private readonly List<TestData> _testData;

        public EndsWithTests()
        {
            _testData = new List<TestData>();
        }
        
        public void Dispose()
        {
            _testData.Clear();
        }

        [Fact]
        public void EndsWith_ComparedToAnExistingProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try
            {
                _testData.Search(x => x.Name).EndsWith(x => x.Description);
            }
            catch (Exception)
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void EndsWith_ComparedToAnExistingProperty_ResultIsNotNull()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).EndsWith(x => x.Description);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Search_AllowEndsWithMethod_AllResultsEndWithSearchTerm()
        {
            //Arrange
            _testData.Add(new TestData { Name = "test" });
            _testData.Add(new TestData { Name = "false" });

            //Act
            var result = _testData.Search(x => x.Name).EndsWith("st");

            //Assert
            Assert.True(result.Any());
            Assert.True(result.All(x => x.Name.EndsWith("st")));
        }

        [Fact]
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
            Assert.Equal(2, result.Count());
            Assert.False(result.Contains(notPresent));
        }

        [Fact]
        public void EndsWith_ComparedToAnExistingProperty_ResultEndsWithExistingProperty()
        {
            //Arrange
            _testData.Add(new TestData { Name = "efgh", Description = "gh" });
            var notPresent = new TestData {Name = "no match", Description = "test"};
            _testData.Add(notPresent);

            //Act
            var result = _testData.Search(x => x.Name).EndsWith(x => x.Description);

            //Assert
            Assert.True(result.Any(), "No records returned");
            Assert.False(result.Contains(notPresent));
        }

        [Fact]
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
            Assert.Equal(2, result.Count());
            Assert.False(result.Contains(notPresent));
        }

        [Fact]
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
            Assert.True(result.Any(), "No records returned");
            Assert.True(result.All(x => x.Name.EndsWith(x.Description) || (x.Status != null && x.Name.EndsWith(x.Status))));
        }

        [Fact]
        public void EndsWith_SearchTwoPropertiesComparedToAProperty_ResultsContainAllPermiatations()
        {
            //Arrange
            _testData.Add(new TestData { Name = "abcd", Description = "efgh", Status = "cd" });
            _testData.Add(new TestData { Name = "zzzz", Description = "match", Status = "ch"});
            _testData.Add(new TestData { Name = "zzzz", Description = "yyyy", Status = "xxxx"});

            //Act
            var result = _testData.Search(x => x.Name, x => x.Description).EndsWith(x => x.Status);

            //Assert
            Assert.Equal(2, result.Count());
            Assert.True(result.All(x => x.Name.EndsWith(x.Status) || x.Description.EndsWith(x.Status)));
        }

        [Fact]
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
            Assert.Equal(1, result.Count());
            Assert.True(result.Any(t => t.Description == "test"));
        }

        [Fact]
        public void EndsWith_SearchOccursTwice_ReturnExpectedRecord()
        {
            //Arrange
            _testData.Add(new TestData { Name = "tastiest", Description = "two occurences of st", Number = 8 });

            //Act
            var result = _testData.Search(x => x.Name).EndsWith("st");

            //Assert
            Assert.True(result.Any(td => td.Number == 8));
        }

        [Fact]
        public void EndsWith_SerchAcrossTwoProperties_ResultEndsWithTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "test";
            _testData.Add(new TestData { Name = "a test", Description = "zzzz" });
            _testData.Add(new TestData { Name = "zzzz", Description = "another test" });

            //Act
            var result = _testData.Search(x => x.Name, x => x.Description).EndsWith(searchTerm);

            //Assert
            Assert.Equal(2, result.Count());
            Assert.True(result.All(x => x.Name.EndsWith(searchTerm) || x.Description.EndsWith(searchTerm)));
        }
    }
}