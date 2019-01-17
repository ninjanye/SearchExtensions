using System;
using System.Globalization;

namespace NinjaNye.SearchExtensions.Levenshtein
{
    public static class LevenshteinProcessor
    {
        /// <summary>
        /// Compute the Levenshtein Distance between two strings.
        /// </summary>
        /// <param name="source">Source string to compare</param>
        /// <param name="comparedTo">String to compare to source</param>
        /// <returns>Calculated Levenshtein Distance</returns>
        public static int LevenshteinDistance(string source, string comparedTo)
        {
            bool nullSource = source == null;
            bool nullCompare = comparedTo == null;
            if (nullSource && nullCompare)
            {
                return 0;
            }

            if (nullSource)
            {
                return comparedTo.Length;
            }

            if (nullCompare)
            {
                return source.Length;
            }

            if (source.Equals(comparedTo, StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            return ComputeDistance(source, comparedTo);
        }

        private static int ComputeDistance(string source, string comparedTo)
        {
            int sourceLength = source.Length + 1;
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            source = textInfo.ToLower(source).PadLeft(sourceLength);
            comparedTo = textInfo.ToLower(comparedTo);
            var previousValues = new int[sourceLength];
            var currentValues = new int[sourceLength];
            for (int row = 0; row < sourceLength; row++)
            {
                previousValues[row] = row;
            }

            for (int column = 0; column < comparedTo.Length; column++)
            {
                bool isFirst = true;
                char comparedToCharacter = comparedTo[column];
                for (int row = 0; row < sourceLength; row++)
                {
                    int minimumCost = previousValues[row] + 1;
                    if (!isFirst)
                    {
                        int cost = source[row] != comparedToCharacter ? 1 : 0;
                        var previousRow = row - 1;
                        int diagonalIncrement = previousValues[previousRow] + cost;
                        int topIncrement = currentValues[previousRow] + 1;
                        minimumCost = Math.Min(Math.Min(diagonalIncrement, topIncrement), minimumCost);
                    }
                    currentValues[row] = minimumCost;
                    isFirst = false;
                }

                previousValues = currentValues;
                currentValues = new int[sourceLength];
            }
            return previousValues[source.Length - 1];
        }
    }
}