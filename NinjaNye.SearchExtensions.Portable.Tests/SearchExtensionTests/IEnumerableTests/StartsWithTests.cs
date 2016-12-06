﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Portable.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class StartsWithTests
    {
        private List<TestData> _testData = new List<TestData>();

        [SetUp]
        public void ClassSetup()
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

        [Test]
        public void StartsWith_ComparedToAnExistingProperty_DoesNotThrowAnException()
        {
            //Arrange

            //Act
            //Assert
            Assert.DoesNotThrow(() => _testData.Search(x => x.Name).StartsWith(x => x.Description));
        }

        [Test]
        public void StartsWith_ComparedToAnExistingProperty_ResultIsNotNull()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).StartsWith(x => x.Description);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void StartsWith_ComparedToAnExistingProperty_ResultStartsWithExistingProperty()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).StartsWith(x => x.Description);

            //Assert
            Assert.IsTrue(result.Any(), "No records returned");
            Assert.IsTrue(result.All(x => x.Name.StartsWith(x.Description)));
        }

        [Test]
        public void StartsWith_ComparedToTwoExistingProperties_ResultStartsWithEitherOfExistingProperties()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).StartsWith(x => x.Description, x => x.Status);

            //Assert
            Assert.IsTrue(result.Any(), "No records returned");
            Assert.IsTrue(result.All(x => x.Name.StartsWith(x.Description) || x.Name.StartsWith(x.Status)));
        }

        [Test]
        public void StartsWith_ComparedToTwoExistingPropertiesWithNullValue_NullValueIsIgnored()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).StartsWith(x => x.Description, x => x.Status);

            //Assert
            Assert.IsTrue(result.Any(), "No records returned");
            Assert.IsTrue(result.All(x => x.Name.StartsWith(x.Description) || (x.Status != null && x.Name.StartsWith(x.Status))));
        }

        [Test]
        public void StartsWith_SearchTwoPropertiesComparedToAProperty_ResultsContainAllPermiatations()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name, x => x.Description).StartsWith(x => x.Status);

            //Assert
            Assert.IsTrue(result.Count() > 1, "Not enough records returned");
            Assert.IsTrue(result.All(x => x.Name.StartsWith(x.Status) || x.Description.StartsWith(x.Status)));
        }

        [Test]
        public void StartsWith_SearchPropertyWithIgnoreCaseCulture_ResultsAreCaseInsensitive()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).SetCulture(StringComparison.OrdinalIgnoreCase).StartsWith(x => x.Description);

            //Assert
            Assert.IsTrue(result.Any(t => t.Description == "TEsT"));
        }
    }
}