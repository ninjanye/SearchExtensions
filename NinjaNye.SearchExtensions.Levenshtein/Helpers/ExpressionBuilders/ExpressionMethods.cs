using System.Reflection;

namespace NinjaNye.SearchExtensions.Levenshtein.Helpers.ExpressionBuilders
{
    internal static class ExpressionMethods
    {
        #region Methods
#if NET45
        public static readonly MethodInfo LevensteinDistanceMethod = typeof(LevenshteinProcessor).GetMethod("LevenshteinDistance");
#else
        public static readonly MethodInfo LevensteinDistanceMethod = typeof(LevenshteinProcessor).GetRuntimeMethod("LevenshteinDistance", new[] { typeof(string), typeof(string) });
#endif
        #endregion
    }
}