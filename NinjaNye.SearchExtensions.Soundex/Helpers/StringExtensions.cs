using System;

namespace NinjaNye.SearchExtensions.Soundex.Helpers
{
    internal static class StringExtensionHelper
    {
        public static string GetFirstCharacter(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return text.TrimStart()[0].ToString();
        }

        public static string GetLastCharacter(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var trimmedText = text.TrimEnd();
            return trimmedText[trimmedText.Length -1].ToString();
        }

        public static string QuickReverse(this string value)
        {
            var charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}