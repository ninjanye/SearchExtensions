using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
#if NET45
        public static readonly PropertyInfo StringLengthProperty = typeof(string).GetTypeInfo().GetProperty("Length");
#else
        public static readonly PropertyInfo StringLengthProperty = typeof(string).GetRuntimeProperty("Length");
#endif
        #endregion

        #region Methods

#if NET45
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
        public static readonly MethodInfo CustomReplaceMethod = typeof(StringExtensionHelper).GetMethod("Replace");
        public static readonly MethodInfo QuickReverseMethod = typeof(StringExtensionHelper).GetMethod("QuickReverse");

        public static readonly MethodInfo AnyQueryableMethod = typeof(Enumerable).GetMethods()
                                                                                 .Single(mi => mi.Name == "Any" 
                                                                                            && mi.GetParameters().Length == 2);
#else
        public static readonly MethodInfo IndexOfMethod = typeof(string).GetRuntimeMethod("IndexOf", new[] { typeof(string) });
        public static readonly MethodInfo IndexOfMethodWithComparison = typeof(string).GetRuntimeMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) });
        public static readonly MethodInfo ReplaceMethod = typeof(string).GetRuntimeMethod("Replace", new[] { typeof(string), typeof(string) });
        public static readonly MethodInfo EqualsMethod = typeof(string).GetRuntimeMethod("Equals", new[] { typeof(string), typeof(StringComparison) });
        public static readonly MethodInfo StartsWithMethod = typeof(string).GetRuntimeMethod("StartsWith", new[] { typeof(string) });
        public static readonly MethodInfo StartsWithMethodWithComparison = typeof(string).GetRuntimeMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) });
        public static readonly MethodInfo EndsWithMethod = typeof(string).GetRuntimeMethod("EndsWith", new[] { typeof(string) });
        public static readonly MethodInfo EndsWithMethodWithComparison = typeof(string).GetRuntimeMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) });
        public static readonly MethodInfo StringConcatMethod = typeof(string).GetRuntimeMethod("Concat", new[] { typeof(string), typeof(string) });
        public static readonly MethodInfo StringContainsMethod = typeof(string).GetRuntimeMethod("Contains", new[] { typeof(string) });
        public static readonly MethodInfo CustomReplaceMethod = typeof(StringExtensionHelper).GetRuntimeMethod("Replace", new[] { typeof(string), typeof(string), typeof(string), typeof(StringComparison)});
        public static readonly MethodInfo QuickReverseMethod = typeof(StringExtensionHelper).GetRuntimeMethod("QuickReverse", new[] {typeof(string)});

        public static readonly MethodInfo AnyQueryableMethod = typeof(Enumerable).GetRuntimeMethods()
                                                                                 .Single(mi => mi.Name == "Any"
                                                                                            && mi.GetParameters().Length == 2);
#endif


#endregion
    }
}