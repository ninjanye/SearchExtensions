using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [TestFixture]
    internal class IsEqualTests : IDisposable
    {
        private readonly TestContext context = new TestContext();

        [Test]
        public void IsEqual_CallWithProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => context.TestModels.Search(x => x.StringOne).IsEqual(x => x.StringTwo));
        }

        [Test]
        public void IsEqual_CallWithProperty_DoesNotReturnNull()
        {
            //Arrange
            
            //Act
            var result = context.TestModels.Search(x => x.StringOne).IsEqual(x => x.StringTwo);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void IsEqual_CallWithProperty_AllRecordsHaveEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = context.TestModels.Search(x => x.StringOne).IsEqual(x => x.StringTwo);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.StringOne == x.StringTwo));
        }

        [Test]
        public void IsEqual_SearchMultipleProperties_RecordsFromSecondPropertyMatchReturned()
        {
            //Arrange
            
            //Act
            var result = context.TestModels.Search(x => x.StringOne, x => x.StringTwo).IsEqual(x => x.StringThree);

            //Assert
            Assert.IsTrue(result.Any(x => x.StringTwo == x.StringThree));
        }

        [Test]
        public void IsEqual_SearchForMultipleProperties_RecordsFromSecondPropertyMatchReturned()
        {
            //Arrange
            
            //Act
            var result = context.TestModels.Search(x => x.StringOne).IsEqual(x => x.StringTwo, x => x.StringThree);

            //Assert
            Assert.IsTrue(result.Any(x => x.StringOne == x.StringThree));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}