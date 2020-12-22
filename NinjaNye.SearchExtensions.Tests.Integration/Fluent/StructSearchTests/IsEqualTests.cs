using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
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
        public void IsEqual_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange

            //Act

            //Assert
            try
            {
                _context.TestModels.Search(x => x.IntegerOne).EqualTo(1);
            }
            catch (Exception)
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void IsEqual_CallWithValue_DoesNotReturnNull()
        {
            //Arrange

            //Act 
            var result = _context.TestModels.Search(x => x.IntegerOne).EqualTo(50);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsEqual_CallWithValue_AllRecordsHaveEqualPropertyValues()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne).EqualTo(101);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.IntegerOne == 101));
        }

        [Fact]
        public void IsEqual_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .EqualTo(3);

            //Assert
            Assert.True(result.All(x => x.IntegerOne == 3 || x.IntegerThree == 3));
        }

        [Fact]
        public void IsEqual_SearchMultipleAgainstMultipleValues_RecordsMatchingBothValuesReturned()
        {
            //Arrange

            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .EqualTo(3, 101);

            //Assert
            Assert.True(result.All(x => x.IntegerOne == 3 || x.IntegerThree == 3
                                       || x.IntegerOne == 101 || x.IntegerThree == 101));
        }
    }
}