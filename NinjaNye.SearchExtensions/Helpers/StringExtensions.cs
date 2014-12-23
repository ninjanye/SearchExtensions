using System;
using System.Globalization;

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

        public static string GetFirstCharacter(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return text.TrimStart()[0].ToString(CultureInfo.InvariantCulture);
        }

        public static string QuickReverse(this string value)
        {
            var charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}