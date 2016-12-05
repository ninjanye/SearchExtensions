using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NinjaNye.SearchExtensions.Portable.Levenshtein;
using NinjaNye.SearchExtensions.Portable.Soundex;

namespace NinjaNye.SearchExtensions.Portable.Helpers.ExpressionBuilders
{
    public static class ExpressionMethods
    {
        #region Constants
        public static readonly ConstantExpression EmptyStringExpression = Expression.Constant(string.Empty);
        public static readonly ConstantExpression NullExpression = Expression.Constant(null);
        public static readonly ConstantExpression ZeroConstantExpression = Expression.Constant(0);
        public static readonly Type stringType = typeof(string);
        #endregion

        #region Properties
        public static readonly PropertyInfo StringLengthProperty = stringType.GetRuntimeProperty("Length");
        #endregion

        #region Methods

        public static readonly MethodInfo IndexOfMethod = typeof (string).GetRuntimeMethod("IndexOf", new[] { stringType });
        public static readonly MethodInfo IndexOfMethodWithComparison = stringType.GetRuntimeMethod("IndexOf", new[] { stringType, typeof(StringComparison) });
        public static readonly MethodInfo ReplaceMethod = stringType.GetRuntimeMethod("Replace", new[] { stringType, stringType });
        public static readonly MethodInfo EqualsMethod = stringType.GetRuntimeMethod("Equals", new[] { stringType, typeof(StringComparison) });
        public static readonly MethodInfo StartsWithMethod = stringType.GetRuntimeMethod("StartsWith", new[] { stringType });
        public static readonly MethodInfo StartsWithMethodWithComparison = stringType.GetRuntimeMethod("StartsWith", new[] { stringType, typeof(StringComparison) });
        public static readonly MethodInfo EndsWithMethod = stringType.GetRuntimeMethod("EndsWith", new[] { stringType });
        public static readonly MethodInfo EndsWithMethodWithComparison = stringType.GetRuntimeMethod("EndsWith", new[] { stringType, typeof(StringComparison) });
        public static readonly MethodInfo StringConcatMethod = stringType.GetRuntimeMethod("Concat", new[] { stringType, stringType });
        public static readonly MethodInfo StringContainsMethod = stringType.GetRuntimeMethod("Contains", new[] { stringType });
        public static readonly MethodInfo StringListContainsMethod = typeof(List<string>).GetRuntimeMethod("Contains", new[] { stringType });
        public static readonly MethodInfo SoundexMethod = typeof(SoundexProcessor).GetRuntimeMethod("ToSoundex", new [] { stringType });
        public static readonly MethodInfo ReverseSoundexMethod = typeof(SoundexProcessor).GetRuntimeMethod("ToReverseSoundex", new [] { stringType });
        public static readonly MethodInfo LevensteinDistanceMethod = typeof(LevenshteinProcessor).GetRuntimeMethod("LevenshteinDistance", new [] { stringType, stringType });
        public static readonly MethodInfo CustomReplaceMethod = typeof(StringExtensionHelper).GetRuntimeMethod("Replace", new[] { stringType, stringType, stringType, typeof(StringComparison) });
        public static readonly MethodInfo QuickReverseMethod = typeof(StringExtensionHelper).GetRuntimeMethod("QuickReverse", new [] { stringType });

        public static readonly MethodInfo AnyQueryableMethod = typeof(Enumerable).GetRuntimeMethods()
                                                                                 .Single(mi => mi.Name == "Any" 
                                                                                            && mi.GetParameters().Length == 2);

        #endregion
    }
}