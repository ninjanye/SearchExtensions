using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EndsWithExpressionBuilder
{
    internal static class EnumerableEndsWithExpressionBuilder
    {
        /// <summary>
        /// Build an 'EndsWith' expression for a collection of search terms against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var endsWithExpression = Build(stringProperty, searchTerm, searchOptions);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, endsWithExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build an 'EndsWith' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, string searchTerm, SearchOptions searchOptions)
        {
            var alteredTerm = searchOptions.SearchType == SearchType.WholeWords ? " " + searchTerm : searchTerm;
            var searchTermExpression = Expression.Constant(alteredTerm);
            var endsWithExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.EndsWithMethodWithComparison, searchTermExpression, searchOptions.ComparisonTypeExpression);
            return Expression.IsTrue(endsWithExpresion);
        }

        /// <summary>
        /// Build an 'EndsWith' expression for a collection of properties against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>>[] propertiesToSearchFor, SearchOptions searchOptions)
        {
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var endsWithExpression = Build(stringProperty, propertyToSearchFor, searchOptions);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, endsWithExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build an 'EndsWith' expression for a string property against a property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>> propertyToSearchFor, SearchOptions searchOptions)
        {
            var endsWithExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.EndsWithMethodWithComparison, propertyToSearchFor.Body, searchOptions.ComparisonTypeExpression);
            Expression finalExpression = null;
            if (searchOptions.NullCheck)
            {
                var notNullProperty = ExpressionHelper.BuildNotNullExpression(stringProperty);
                var notNullSearchFor = ExpressionHelper.BuildNotNullExpression(propertyToSearchFor);
                finalExpression = ExpressionHelper.JoinAndAlsoExpression(notNullProperty, notNullSearchFor);
            }
            finalExpression = ExpressionHelper.JoinAndAlsoExpression(finalExpression, endsWithExpresion);
            return Expression.IsTrue(finalExpression);
        }

        /// <summary>
        /// Build an 'EndsWith' expression comparing multiple string properties against other string properties
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, Expression<Func<T, string>>[] propertiesToSearchFor, SearchOptions searchOptions)
        {
            Expression finalExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var endsWithExpression = Build(stringProperty, propertiesToSearchFor, searchOptions);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, endsWithExpression);
            }
            return finalExpression;
        }

        /// <summary>
        /// Build an 'EndsWith([searchTerm])' expression comparing multiple string properties against string terms
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, string[] searchTerms, SearchOptions searchOptions)
        {
            Expression finalExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var endsWithExpression = Build(stringProperty, searchTerms, searchOptions);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, endsWithExpression);
            }
            return finalExpression;
        }
    }
}