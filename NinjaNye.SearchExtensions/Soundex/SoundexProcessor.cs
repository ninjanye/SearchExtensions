using System.Globalization;
using System.Text;
using NinjaNye.SearchExtensions.Helpers;

namespace NinjaNye.SearchExtensions.Soundex
{
    public static class SoundexProcessor
    {
        private const int maxSoundexLength = 4;

        /// <summary>
        /// Retrieve the Soundex value for a given string
        /// Soundex used is American Soundex as defined
        /// on http://en.wikipedia.org/wiki/Soundex  
        /// </summary>
        /// <param name="value">string to transform into soundex code</param>
        /// <returns>Soundex code for the given string</returns>
        public static string ToSoundex(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var sb = BuildRawSoundex(value);
            ValidateLength(sb);
            return sb.ToString();
        }

        /// <summary>
        /// Retrieve the Soundex value for a given string.
        /// Soundex used is Reverse Soundex which produces 
        /// a soundex code on the reverse of the supplied string
        /// </summary>
        /// <param name="value">string to transform into reverse soundex code</param>
        /// <returns>Reverse Soundex code for the given string</returns>
        public static string ToReverseSoundex(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            value = value.QuickReverse();
            var sb = BuildRawSoundex(value);
            ValidateLength(sb);
            return sb.ToString();
        }

        private static StringBuilder BuildRawSoundex(string value)
        {
            char firstCharacter = CultureInfo.InvariantCulture.TextInfo.ToUpper(value[0]);
            var sb = new StringBuilder(4);
            sb.Append(firstCharacter);
            string previousSoundex = firstCharacter.GetSoundex();
            for (int i = 1; i < value.Length; i++)
            {
                var character = value[i];
                string soundex = character.GetSoundex();
                if (soundex != previousSoundex)
                {
                    sb.Append(soundex);
                    if (sb.Length == maxSoundexLength)
                    {
                        return sb;
                    }

                    if (!character.IsHOrW())
                    {
                        previousSoundex = soundex;
                    }
                }
            }
            return sb;
        }

        // ReSharper disable once InconsistentNaming
        private static bool IsHOrW(this char character)
        {
            return character == 'h' || character == 'i' 
                || character == 'H' || character == 'I';
        }

        private static string GetSoundex(this char character)
        {
            switch (character)
            {
                case 'b': case 'f': case 'p': case 'v':
                case 'B': case 'F': case 'P': case 'V':
                    return "1";
                case 'c': case 'g': case 'j': case 'k':
                case 'q': case 's': case 'x': case 'z':
                case 'C': case 'G': case 'J': case 'K':
                case 'Q': case 'S': case 'X': case 'Z':
                    return "2";
                case 'd': case 't': case 'D': case 'T':
                    return "3";
                case 'l': case 'L':
                    return "4";
                case 'm': case 'n': case 'M': case 'N':
                    return "5";
                case 'r': case 'R':
                    return "6";
                default:
                    return string.Empty;
            }
        }

        private static void ValidateLength(this StringBuilder stringBuilder)
        {
            int soundexLength = stringBuilder.Length;
            if (soundexLength < maxSoundexLength)
            {
                int zerosToAdd = maxSoundexLength - soundexLength;
                for (int i = 0; i < zerosToAdd; i++)
                {
                    stringBuilder.Append('0');
                }
            }
        }
    }
}