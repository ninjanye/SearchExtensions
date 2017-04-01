using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    public class StartsWithTests
    {
        private List<TestData> _testData = new List<TestData>();

        public StartsWithTests()
        {
            _testData = new List<TestData>();
            BuildTestData();
        }

        private void BuildTestData()
        {
            _testData.Add(new TestData { Name = "abcd", Description = "efgh", Status = "status" });
            _testData.Add(new TestData { Name = "mnop", Description = "m", Status = "status" });
            _testData.Add(new TestData { Name = "yesterday", Description = "no", Status = "yes" });
            _testData.Add(new TestData { Name = "another day", Description = "null status" });
            _testData.Add(new TestData { Name = "test", Description = "description", Status = "desc" });
            _testData.Add(new TestData { Name = "teSt cAsE iNsEnSiTiViTy", Description = "TEsT" });
        }

        [Fact]
        public void StartsWith_ComparedToAnExistingProperty_DoesNotThrowAnException()
        {
            //Arrange

            //Act
            //Assert
            try { _testData.Search(x => x.Name).StartsWith(x => x.Description); } catch (Exception) { Assert.False(true); }
        }

        [Fact]
        public void StartsWith_ComparedToAnExistingProperty_ResultIsNotNull()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).StartsWith(x => x.Description);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void StartsWith_ComparedToAnExistingProperty_ResultStartsWithExistingProperty()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).StartsWith(x => x.Description);

            //Assert
            Assert.True(result.Any(), "No records returned");
            Assert.True(result.All(x => x.Name.StartsWith(x.Description)));
        }

        [Fact]
        public void StartsWith_ComparedToTwoExistingProperties_ResultStartsWithEitherOfExistingProperties()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).StartsWith(x => x.Description, x => x.Status);

            //Assert
            Assert.True(result.Any(), "No records returned");
            Assert.True(result.All(x => x.Name.StartsWith(x.Description) || x.Name.StartsWith(x.Status)));
        }

        [Fact]
        public void StartsWith_ComparedToTwoExistingPropertiesWithNullValue_NullValueIsIgnored()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).StartsWith(x => x.Description, x => x.Status);

            //Assert
            Assert.True(result.Any(), "No records returned");
            Assert.True(result.All(x => x.Name.StartsWith(x.Description) || (x.Status != null && x.Name.StartsWith(x.Status))));
        }

        [Fact]
        public void StartsWith_SearchTwoPropertiesComparedToAProperty_ResultsContainAllPermiatations()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name, x => x.Description).StartsWith(x => x.Status);

            //Assert
            Assert.True(result.Count() > 1, "Not enough records returned");
            Assert.True(result.All(x => x.Name.StartsWith(x.Status) || x.Description.StartsWith(x.Status)));
        }

        [Fact]
        public void StartsWith_SearchPropertyWithIgnoreCaseCulture_ResultsAreCaseInsensitive()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).SetCulture(StringComparison.OrdinalIgnoreCase).StartsWith(x => x.Description);

            //Assert
            Assert.True(result.Any(t => t.Description == "TEsT"));
        }
    }
}