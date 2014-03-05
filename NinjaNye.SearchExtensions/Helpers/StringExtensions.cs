using System;

namespace NinjaNye.SearchExtensions.Helpers
{
    internal static class StringExtensionHelper
    {
        public static string Replace(this string text, string oldValue, string newValue, StringComparison stringComparison)
        {
            int position;
            while ((position = text.IndexOf(oldValue, stringComparison)) > -1)
            {
                text = text.Remove(position, oldValue.Length);
                text = text.Insert(position, newValue);
            }
            return text;
        }
    }
}