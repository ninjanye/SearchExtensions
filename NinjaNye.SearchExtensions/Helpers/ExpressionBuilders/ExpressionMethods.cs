using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NinjaNye.SearchExtensions.Levenshtein;
using NinjaNye.SearchExtensions.Soundex;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders
{
    internal static class ExpressionMethods
    {
        #region Contants
        public static readonly ConstantExpression EmptyStringExpression = Expression.Constant(string.Empty);
        public static readonly ConstantExpression NullExpression = Expression.Constant(null);
        public static readonly ConstantExpression ZeroConstantExpression = Expression.Constant(0);
        #endregion

        #region Properties
        public static readonly PropertyInfo StringLengthProperty = typeof(string).GetProperty("Length");
        #endregion

        #region Methods

        public static readonly MethodInfo IndexOfMethod = typeof (string).GetMethod("IndexOf", new[] {typeof (string)});
        public static readonly MethodInfo IndexOfMethodWithComparison = typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) });
        public static readonly MethodInfo ReplaceMethod = typeof(string).GetMethod("Replace", new[] { typeof(string), typeof(string) });
        public static readonly MethodInfo EqualsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(StringComparison) });
        public static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        public static readonly MethodInfo StartsWithMethodWithComparison = typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) });
        public static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        public static readonly MethodInfo EndsWithMethodWithComparison = typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) });
        public static readonly MethodInfo StringConcatMethod = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });
        public static readonly MethodInfo StringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        public static readonly MethodInfo StringListContainsMethod = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) });
        public static readonly MethodInfo SoundexMethod = typeof(SoundexProcessor).GetMethod("ToSoundex");
        public static readonly MethodInfo ReverseSoundexMethod = typeof(SoundexProcessor).GetMethod("ToReverseSoundex");
        public static readonly MethodInfo LevensteinDistanceMethod = typeof(LevenshteinProcessor).GetMethod("LevenshteinDistance");
        public static readonly MethodInfo CustomReplaceMethod = typeof(StringExtensionHelper).GetMethod("Replace");
        public static readonly MethodInfo QuickReverseMethod = typeof(StringExtensionHelper).GetMethod("QuickReverse");

        public static readonly MethodInfo AnyQueryableMethod = typeof(Enumerable).GetMethods()
                                                                                 .Single(mi => mi.Name == "Any" 
                                                                                            && mi.GetParameters().Length == 2);

        #endregion
    }
}