using System.Collections.Generic;
using System.Reflection;

namespace NinjaNye.SearchExtensions.Soundex.Helpers.ExpressionBuilders
{
    internal static class ExpressionMethods
    {
        #region Methods
#if NET45
        public static readonly MethodInfo SoundexMethod = typeof(SoundexProcessor).GetMethod("ToSoundex");
        public static readonly MethodInfo ReverseSoundexMethod = typeof(SoundexProcessor).GetMethod("ToReverseSoundex");
        public static readonly MethodInfo StringListContainsMethod = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) });
#else
        public static readonly MethodInfo SoundexMethod = typeof(SoundexProcessor).GetRuntimeMethod("ToSoundex", new[] { typeof(string) });
        public static readonly MethodInfo ReverseSoundexMethod = typeof(SoundexProcessor).GetRuntimeMethod("ToReverseSoundex", new[]{typeof(string)});
        public static readonly MethodInfo StringListContainsMethod = typeof(List<string>).GetRuntimeMethod("Contains", new[] { typeof(string) });
#endif
        #endregion
    }
}