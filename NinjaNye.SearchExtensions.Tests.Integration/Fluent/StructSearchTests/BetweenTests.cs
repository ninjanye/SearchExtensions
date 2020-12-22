using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
{
    [Collection("Database tests")]
    public class BetweenTests 
    {
        private readonly TestContext _context;

        public BetweenTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void Between_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try { _context.TestModels.Search(x => x.IntegerOne).Between(1,3); } catch (Exception) { Assert.False(true); }
        }

        [Fact]
        public void Between_CallWithValue_DoesNotReturnNull()
        {
            //Arrange
            
            //Act 
            var result = _context.TestModels.Search(x => x.IntegerOne).Between(50, 150);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Between_CallWithValue_AllRecordsHaveEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne).Between(10, 100);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.IntegerOne > 10 && x.IntegerOne < 100));
        }

        [Fact]
        public void Between_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .Between(3, 200);

            //Assert
            Assert.True(result.All(x => (x.IntegerOne > 3 && x.IntegerOne < 200) 
                                       || (x.IntegerThree > 3 && x.IntegerThree < 200)));
        }
    }
}