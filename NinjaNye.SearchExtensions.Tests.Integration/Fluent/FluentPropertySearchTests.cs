using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [TestFixture]
    public class FluentPropertySearchTests : IDisposable
    {
        private readonly TestContext context = new TestContext();

        [Test]
        public void Containing_CompareAgainstAnotherProperty_DoesNotThrowAnException()
        {
            //Arrange

            //Act
            context.TestModels.Search(x => x.StringOne).Containing(x => x.StringTwo);

            //Assert
            Assert.Pass("No exception thrown");
        }

        [Test]
        public void Containing_CompareAgainstAnotherProperty_ResultHasRecords()
        {
            //Arrange

            //Act
            var result = context.TestModels.Search(x => x.StringOne).Containing(x => x.StringTwo);

            //Assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void Containing_CompareAgainstAnotherProperty_ResultDoesNotContainIncorrectRecords()
        {
            //Arrange

            //Act
            var result = context.TestModels.Search(x => x.StringOne).Containing(x => x.StringTwo);

            //Assert
            Assert.IsTrue(result.All(x => x.StringOne.Contains(x.StringTwo)));
        }

        [Test]
        public void Containing_SearchTwoProperties_ReturnsRecordWithMatchedDataInSecondProperty()
        {
            //Arrange
            var expected = this.context.TestModels.FirstOrDefault(x => x.Id == new Guid("2F75BE28-CEC8-46D8-852E-E6DAE5C8F0A3"));

            //Act
            var result = context.TestModels.Search(x => x.StringOne, x => x.StringTwo).Containing(x => x.StringThree).ToList();

            //Assert
            CollectionAssert.Contains(result, expected);
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}