using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;
using NinjaNye.SearchExtensions.Soundex;
using NinjaNye.SearchExtensions.Visitors;

namespace NinjaNye.SearchExtensions
{
    internal class EnumerableSoundexSearch<T> : EnumerableSearchBase<T>
    {
        public EnumerableSoundexSearch(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties) 
            : base(source, stringProperties)
        {
        }

        /// <summary>
        /// Perform a soundex search for a particular collection of terms 
        /// based on the American Soundex algorythm 
        /// as defined on http://en.wikipedia.org/wiki/Soundex
        /// </summary>
        /// <param name="terms">Term or terms results should sound similar to</param>
        /// <returns>Returns only the items that match the soundex codes for the terms supplied</returns>
        public IEnumerable<T> American(params string[] terms)
        {
            Expression fullExpression = null;
            var soundexCodes = terms.Select(t => t.ToSoundex()).ToList();
            for (int i = 0; i < this.StringProperties.Length; i++)
            {
                var stringProperty = this.StringProperties[i];
                var swappedParamExpression = AlignParameter(stringProperty);
                var soundsLikeExpression = EnumerableExpressionHelper.BuildSoundsLikeExpression(swappedParamExpression, soundexCodes);
                fullExpression = fullExpression == null ? soundsLikeExpression
                                                        : Expression.OrElse(fullExpression, soundsLikeExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        /// <summary>
        /// Perform a reverse soundex search for a particular collection of terms 
        /// based on the American Soundex algorythm with the words reversed 
        /// as defined on http://en.wikipedia.org/wiki/Soundex
        /// </summary>
        /// <param name="terms">Term or terms results should sound similar to</param>
        /// <returns>Returns only the items that match the soundex codes for the terms supplied</returns>
        public IEnumerable<T> Reverse(params string[] terms)
        {
            Expression fullExpression = null;
            var soundexCodes = terms.Select(t => t.ToReverseSoundex()).ToList();
            for (int i = 0; i < this.StringProperties.Length; i++)
            {
                var stringProperty = this.StringProperties[i];
                var swappedParamExpression = this.AlignParameter(stringProperty);
                var soundsLikeExpression = EnumerableExpressionHelper.BuildReverseSoundexLikeExpression(swappedParamExpression, soundexCodes);
                fullExpression = fullExpression == null ? soundsLikeExpression
                                     : Expression.OrElse(fullExpression, soundsLikeExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }
    }
}