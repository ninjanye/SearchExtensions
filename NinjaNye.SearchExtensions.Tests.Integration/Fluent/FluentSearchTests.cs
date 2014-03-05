using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Fluent;
using NinjaNye.SearchExtensions.Tests.Integration.Models;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [TestFixture]
    public class FluentSearchTests : IDisposable
    {
        readonly TestContext context = new TestContext();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Search_SearchWithoutActionHasNoAffectOnTheResults_ResultsAreUnchanged()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).ToList();

            //Assert
            CollectionAssert.AreEqual(this.context.TestModels, result);
        }

        [Test]
        public void Search_Contains_OnlyResultsContainingTermAreReturned()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).Containing("abc").ToList();

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.Contains("abc")));
        }

        [Test]
        public void Search_ContainsThenStartsWith_OnlyResultsThatContainTextAndStartWithTextAreReturned()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).Containing("abc").StartsWith("a").ToList();

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.Contains("abc") && x.StringOne.StartsWith("a")));
        }

        [Test]
        public void Search_Equals_DefinedPropertyEqualsSearchResult()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).IsEqual("abcd").ToList();

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne == "abcd"));
        }

        [Test]
        public void Search_EndsWith_AllResultsEndWithSearchTerm()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).EndsWith("st").ToList();

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.EndsWith("st")));
        }

        [Test]
        public void Search_AllowEndsWithAndContainsMethod_AllResultsEndWithSearchTermAndContainSearch()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).EndsWith("st").Containing("qr").ToList();

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.EndsWith("st") && x.StringOne.Contains("qr")));
        }

        [Test]
        public void SearchMultiple_ResultContainsAcrossTwoProperties_ResultContainsTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "cd";

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne, x => x.StringTwo).Containing(searchTerm).ToList();

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.Contains(searchTerm) || x.StringTwo.Contains(searchTerm)));
        }

        [Test]
        public void SearchMultiple_ResultEndsWithAcrossTwoProperties_ResultEndsWithTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "gh";

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne, x => x.StringTwo).EndsWith(searchTerm).ToList();

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.EndsWith(searchTerm) || x.StringTwo.EndsWith(searchTerm)));
        }

        [Test]
        public void SearchMultiple_ResultStartsWithAcrossTwoProperties_ResultStartsWithTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "ef";

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne, x => x.StringTwo).StartsWith(searchTerm).ToList();
            
            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.StartsWith(searchTerm) || x.StringTwo.StartsWith(searchTerm)));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SearchMultiple_PassNull_ExceptionThrown()
        {
            //Arrange

            //Act
            this.context.TestModels.Search(null as Expression<Func<TestModel, string>>[]).Containing("cd").ToList();

            //Assert
            Assert.Fail("ArgumentNullException should be thrown");
        }

        [Test]
        public void SearchAll_NoPropertiesDefined_AllPropertiesAreSearched()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.SearchAll().Containing("cd").ToList();

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.Contains("cd") || x.StringTwo.Contains("cd")));
        }

        [Test]
        public void Search_ContainingMultipleTerms_SearchAgainstMultipleTerms()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).Containing("ab", "jk").ToList();

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.Contains("ab") || x.StringOne.Contains("jk")));
        }

        [Test]
        public void Search_StartsWithMultipleTerms_SearchAgainstMultipleTerms()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).StartsWith("ab", "ef").ToList();

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.StartsWith("ab") || x.StringOne.StartsWith("ef")));

        }

        [Test]
        public void Search_EndsWithMultipleTerms_SearchAgainstMultipleTerms()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).EndsWith("cd", "gh").ToList();

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.EndsWith("cd") || x.StringOne.EndsWith("gh")));
        }

        [Test]
        public void Search_SearchManyPropertiesContainingManyTerms_AllResultsHaveASearchTermWithin()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne, x => x.StringTwo).Containing("cd", "jk").ToList();

            //Assert
            Assert.AreEqual(4, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.Contains("cd") || x.StringOne.Contains("jk")
                                       || x.StringTwo.Contains("cd") || x.StringTwo.Contains("jk")));
        }

        [Test]
        public void Search_SearchManyPropertiesStartingWithManyTerms_AllResultsHaveAPropertyStartingWithASpecifiedTerm()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne, x => x.StringTwo).StartsWith("cd", "ef").ToList();

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.StartsWith("cd") || x.StringOne.StartsWith("ef")
                                       || x.StringTwo.StartsWith("cd") || x.StringTwo.StartsWith("ef")));
        }

        [Test]
        public void Search_SearchManyPropertiesEndingWithManyTerms_AllResultsHaveAPropertyEndingWithASpecifiedTerm()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne, x => x.StringTwo).EndsWith("kl", "ef").ToList();

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.EndsWith("kl") || x.StringOne.EndsWith("ef")
                                       || x.StringTwo.EndsWith("kl") || x.StringTwo.EndsWith("ef")));
        }

        [Test]
        public void Search_SearchContainsIgnoreCase_CaseIsIgnored()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).Containing("AB", "jk").ToList();

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne.Contains("jk") || x.StringOne.Contains("ab")));
        }

        [Test]
        public void Search_SearchStartsWithIgnoreCase_CaseIsIgnored()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringTwo).StartsWith("C").ToList();

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(x => x.StringTwo.StartsWith("c", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public void Search_SearchEndsWith_CaseIsIgnored()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringTwo).EndsWith("SE").ToList();

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.StringTwo.EndsWith("se", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public void Search_SearchIsEqual_CaseIsIgnored()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringTwo).IsEqual("CASE").ToList();

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.StringTwo.Equals("case", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public void Search_SearchManyTermsAreEqual_ResultsMatchAnyTerm()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).IsEqual("abcd", "efgh").ToList();

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne == "abcd" || x.StringOne == "efgh"));
        }

        [Test]
        public void Search_SearchManyTermsAreEqual_ResultsMatchAnyTermInAnyCase()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).IsEqual("ABCD", "EFGH");

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.StringOne == "abcd" || x.StringOne == "efgh"));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
