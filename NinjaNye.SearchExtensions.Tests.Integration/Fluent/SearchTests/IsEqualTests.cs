using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [Collection("Database tests")]
    public class IsEqualTests
    {
        private readonly TestContext _context;

        public IsEqualTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void IsEqual_CallWithProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try { _context.TestModels.Search(x => x.StringOne).EqualTo(x => x.StringTwo); } catch (Exception) { Assert.False(true); }
        }

        [Fact]
        public void IsEqual_CallWithProperty_DoesNotReturnNull()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo(x => x.StringTwo);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsEqual_CallWithProperty_AllRecordsHaveEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo(x => x.StringTwo);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.StringOne == x.StringTwo));
        }

        [Fact]
        public void IsEqual_SearchMultipleProperties_RecordsFromSecondPropertyMatchReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne, x => x.StringTwo).EqualTo(x => x.StringThree);

            //Assert
            Assert.Contains(result, x => x.StringTwo == x.StringThree);
        }

        [Fact]
        public void IsEqual_SearchForMultipleProperties_RecordsFromSecondPropertyMatchReturned()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo(x => x.StringTwo, x => x.StringThree);

            //Assert
            Assert.Contains(result, x => x.StringOne == x.StringThree);
        }
    }
}