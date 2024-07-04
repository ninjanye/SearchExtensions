using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [Collection("Database tests")]
    public class FluentPropertySearchTests(DatabaseIntegrationTests @base)
    {
        private readonly TestContext _context = @base.Context;

        [Fact]
        public void Containing_CompareAgainstAnotherProperty_DoesNotThrowAnException()
        {
            //Arrange

            //Act
            _context.TestModels.Search(x => x.StringOne).Containing(x => x.StringTwo);

            //Assert
            Assert.True(true, "No exception thrown");
        }

        [Fact]
        public void Containing_CompareAgainstAnotherProperty_ResultHasRecords()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Containing(x => x.StringTwo);

            //Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Containing_CompareAgainstAnotherProperty_ResultDoesNotContainIncorrectRecords()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Containing(x => x.StringTwo);

            //Assert
            Assert.True(result.All(x => x.StringOne.Contains(x.StringTwo)));
        }

        [Fact]
        public void Containing_SearchTwoProperties_ReturnsRecordWithMatchedDataInSecondProperty()
        {
            //Arrange
            var expected = _context.TestModels.FirstOrDefault(x => x.Id == new Guid("2F75BE28-CEC8-46D8-852E-E6DAE5C8F0A3"));

            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                .Containing(x => x.StringThree)
                .ToList();

            //Assert
            Assert.Contains(expected, result);
        }
    }
}
