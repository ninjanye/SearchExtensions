using System;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Tests.Integration.Models;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [TestFixture]
    internal class StartsWithTests : IDisposable
    {
        private readonly TestContext context = new TestContext();

        [Test]
        public void StartsWith_SearchStartsWith_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => this.context.TestModels.Search(x => x.StringOne).StartsWith(x => x.StringTwo));
        }

        [Test]
        public void StartsWith_SearchStartsWith_ReturnsQueryableStringSearch()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).StartsWith(x => x.StringTwo);

            //Assert
            Assert.IsInstanceOf<QueryableSearch<TestModel>>(result);
        }

        [Test]
        public void StartsWith_SearchStartsWithProperty_AllRecordsStartWithStringTwo()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).StartsWith(x => x.StringTwo);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.StringOne.StartsWith(x.StringTwo)));
        }

        [Test]
        public void StartsWith_SearchStartsWithAgainstTwoProperties_AllRecordsStartWithEitherProperty()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne)
                                                .StartsWith(x => x.StringTwo, x => x.StringThree);

            //Assert
            Assert.IsTrue(result.Any(x => x.StringOne.StartsWith(x.StringThree)));
        }

        [Test]
        public void StartsWith_SearchTwoPropertiesStartsWithOneProperty_AnyRecordMatchesSecondSearchedProperty()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne, x => x.StringTwo).StartsWith(x => x.StringThree);

            //Assert
            Assert.IsTrue(result.Any(x => x.StringTwo.StartsWith(x.StringThree)));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}