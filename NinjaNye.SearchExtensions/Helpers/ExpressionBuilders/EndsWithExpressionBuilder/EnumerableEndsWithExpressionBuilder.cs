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
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, IEnumerable<string> searchTerms, StringComparison comparisonType, bool nullCheck = true)
        {
            var stringComparisonExpression = Expression.Constant(comparisonType);
            Expression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var endsWithExpression = Build(stringProperty, searchTerm, stringComparisonExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, endsWithExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build an 'EndsWith' expression for a search term against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, string searchTerm, ConstantExpression stringComparisonExpression)
        {
            var searchTermExpression = Expression.Constant(searchTerm);
            var endsWithExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.EndsWithMethod, searchTermExpression, stringComparisonExpression);
            return Expression.IsTrue(endsWithExpresion);
        }

        /// <summary>
        /// Build an 'EndsWith' expression for a collection of properties against a particular string property
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>>[] propertiesToSearchFor, StringComparison comparisonType, bool nullCheck = true)
        {
            var stringComparisonExpression = Expression.Constant(comparisonType);
            Expression completeExpression = null;
            foreach (var propertyToSearchFor in propertiesToSearchFor)
            {
                var endsWithExpression = Build(stringProperty, propertyToSearchFor, stringComparisonExpression, nullCheck);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, endsWithExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build an 'EndsWith' expression for a string property against multiple properties
        /// </summary>
        private static Expression Build<T>(Expression<Func<T, string>> stringProperty, Expression<Func<T, string>> propertyToSearchFor, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            var endsWithExpresion = Expression.Call(stringProperty.Body, ExpressionMethods.EndsWithMethod, propertyToSearchFor.Body, stringComparisonExpression);
            Expression finalExpression = null;
            if (nullCheck)
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
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, Expression<Func<T, string>>[] propertiesToSearchFor, StringComparison stringComparison)
        {
            Expression finalExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var endsWithExpression = Build(stringProperty, propertiesToSearchFor, stringComparison);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, endsWithExpression);
            }
            return finalExpression;
        }

        /// <summary>
        /// Build an 'EndsWith([searchTerm])' expression comparing multiple string properties against string terms
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>>[] stringProperties, string[] searchTerms, StringComparison stringComparison)
        {
            Expression finalExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var endsWithExpression = Build(stringProperty, searchTerms, stringComparison);
                finalExpression = ExpressionHelper.JoinOrExpression(finalExpression, endsWithExpression);
            }
            return finalExpression;
        }
    }
}