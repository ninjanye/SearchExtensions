using System.Text;

namespace NinjaNye.SearchExtensions.Soundex
{
    public static class SoundexProcessor
    {
        #region Constants Definition

        private const int maxSoundexLength = 4;
        private const string oneString = "1";
        private const string twoString = "2";
        private const string threeString = "3";
        private const string fourString = "4";
        private const string fiveString = "5";
        private const string sixString = "6";
        private const char characterZero = '0';
        private const char characterB = 'B';
        private const char characterF = 'F';
        private const char characterP = 'P';
        private const char characterV = 'V';
        private const char characterC = 'C';
        private const char characterG = 'G';
        private const char characterJ = 'J';
        private const char characterK = 'K';
        private const char characterQ = 'Q';
        private const char characterS = 'S';
        private const char characterX = 'X';
        private const char characterZ = 'Z';
        private const char characterD = 'D';
        private const char characterT = 'T';
        private const char characterL = 'L';
        private const char characterM = 'M';
        private const char characterN = 'N';
        private const char characterR = 'R';
        private const char characterH = 'H';
        private const char characterW = 'W';

        private const char lowerCharacterB = 'b';
        private const char lowerCharacterF = 'f';
        private const char lowerCharacterP = 'p';
        private const char lowerCharacterV = 'v';
        private const char lowerCharacterC = 'c';
        private const char lowerCharacterG = 'g';
        private const char lowerCharacterJ = 'j';
        private const char lowerCharacterK = 'k';
        private const char lowerCharacterQ = 'q';
        private const char lowerCharacterS = 's';
        private const char lowerCharacterX = 'x';
        private const char lowerCharacterZ = 'z';
        private const char lowerCharacterD = 'd';
        private const char lowerCharacterT = 't';
        private const char lowerCharacterL = 'l';
        private const char lowerCharacterM = 'm';
        private const char lowerCharacterN = 'n';
        private const char lowerCharacterR = 'r';
        private const char lowerCharacterH = 'h';
        private const char lowerCharacterW = 'w';

        #endregion

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
                }

                if (!IsHOrW(character))
                {
                    previousSoundex = soundex;
                }
            }

            ValidateLength(sb);
            return sb.ToString();
        }

        private static bool IsHOrW(char character)
        {
            return character.Equals(lowerCharacterH) || character.Equals(lowerCharacterW)
                   || character.Equals(characterH) || character.Equals(characterW);
        }

        private static string GetSoundexValue(char character)
        {
            if (char.IsUpper(character))
            {
                switch (character)
                {
                    case characterB: case characterF: case characterP: case characterV:
                        return oneString;
                    case characterC: case characterG: case characterJ: case characterK:
                    case characterQ: case characterS: case characterX: case characterZ:
                        return twoString;
                    case characterD: case characterT:
                        return threeString;
                    case characterL:
                        return fourString;
                    case characterM: case characterN:
                        return fiveString;
                    case characterR: 
                        return sixString;
                    default:
                        return string.Empty;
                }
            }

            switch (character)
            {
                case lowerCharacterB: case lowerCharacterF: case lowerCharacterP: case lowerCharacterV:
                    return oneString;
                case lowerCharacterC: case lowerCharacterG: case lowerCharacterJ: case lowerCharacterK:
                case lowerCharacterQ: case lowerCharacterS: case lowerCharacterX: case lowerCharacterZ:
                    return twoString;
                case lowerCharacterD: case lowerCharacterT:
                    return threeString;
                case lowerCharacterL:
                    return fourString;
                case lowerCharacterM: case lowerCharacterN:
                    return fiveString;
                case lowerCharacterR:
                    return sixString;
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
                    stringBuilder.Append(characterZero);
                }
            }
        }
    }
}
