using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders
{
    internal static class SoundexExpressionBuilder
    {
        /// <summary>
        /// Build an 'soundexCodes.Contains(soundex(value))' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildSoundsLikeExpression<T>(Expression<Func<T, string>> stringProperty, IList<string> soundexCodes)
        {
            var soundexCallExpresion = Expression.Call(ExpressionMethods.SoundexMethod, stringProperty.Body);
            var soundexCodesExpression = Expression.Constant(soundexCodes);
            var containsExpression = Expression.Call(soundexCodesExpression, ExpressionMethods.StringListContainsMethod, soundexCallExpresion);
            var trueExpression = Expression.Constant(true);
            return Expression.Equal(containsExpression, trueExpression);
        }

        /// <summary>
        /// Build an 'soundexCodes.Contains(reverseSoundex(value))' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildReverseSoundexLikeExpression<T>(Expression<Func<T, string>> stringProperty, IList<string> soundexCodes)
        {
            var soundexCallExpresion = Expression.Call(ExpressionMethods.ReverseSoundexMethod, stringProperty.Body);
            var soundexCodesExpression = Expression.Constant(soundexCodes);
            var containsExpression = Expression.Call(soundexCodesExpression, ExpressionMethods.StringListContainsMethod, soundexCallExpresion);
            var trueExpression = Expression.Constant(true);
            return Expression.Equal(containsExpression, trueExpression);
        }
    }
}