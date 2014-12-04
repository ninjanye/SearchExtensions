using System.Text;

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

            char firstCharacter = value[0];
            char upperCharacter = char.ToUpper(firstCharacter);
            var sb = new StringBuilder(4);
            sb.Append(upperCharacter);
            string previousSoundex = GetSoundexValue(firstCharacter);
            for (int i = 1; i < value.Length; i++)
            {
                var character = value[i];
                string soundex = GetSoundexValue(character);
                if (!soundex.Equals(previousSoundex))
                {
                    sb.Append(soundex);
                    if (sb.Length == maxSoundexLength)
                    {
                        return sb.ToString();
                    }

                    if (!IsHOrW(character))
                    {
                        previousSoundex = soundex;
                    }
                }
            }

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

            char lastCharacter = value[value.Length-1];
            char upperCharacter = char.ToUpper(lastCharacter);
            var sb = new StringBuilder(4);
            sb.Append(upperCharacter);
            string previousSoundex = GetSoundexValue(lastCharacter);
            for (int i = value.Length - 2; i >= 0; i--)
            {
                char character = value[i];
                string soundex = GetSoundexValue(character);
                if (!soundex.Equals(previousSoundex))
                {
                    sb.Append(soundex);
                    if (sb.Length == maxSoundexLength)
                    {
                        return sb.ToString();
                    }

                    if (!IsHOrW(character))
                    {
                        previousSoundex = soundex;
                    }
                }
            }

            ValidateLength(sb);
            return sb.ToString();
        }

        private static bool IsHOrW(char character)
        {
            return character.Equals('h') || character.Equals('w')
                   || character.Equals('H') || character.Equals('W');
        }

        private static string GetSoundexValue(char character)
        {
            if (char.IsUpper(character))
            {
                switch (character)
                {
                    case 'B': case 'F': case 'P': case 'V':
                        return "1";
                    case 'C': case 'G': case 'J': case 'K':
                    case 'Q': case 'S': case 'X': case 'Z':
                        return "2";
                    case 'D': case 'T':
                        return "3";
                    case 'L':
                        return "4";
                    case 'M': case 'N':
                        return "5";
                    case 'R': 
                        return "6";
                    default:
                        return string.Empty;
                }
            }

            switch (character)
            {
                case 'b': case 'f': case 'p': case 'v':
                    return "1";
                case 'c': case 'g': case 'j': case 'k':
                case 'q': case 's': case 'x': case 'z':
                    return "2";
                case 'd': case 't':
                    return "3";
                case 'l':
                    return "4";
                case 'm': case 'n':
                    return "5";
                case 'r':
                    return "6";
                default:
                    return string.Empty;
            }
        }

        private static void ValidateLength(StringBuilder stringBuilder)
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