using System;

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

            if (nullSource || nullCompare)
            {
                if (nullSource)
                {
                    return comparedTo.Length;
                }

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
            int sourceLength = source.Length;
            int comparedToLength = comparedTo.Length;
            if (sourceLength == 0)
            {
                return comparedToLength;
            }

            if (comparedToLength == 0)
            {
                return sourceLength;
            }

            int paddedSourceLength = sourceLength + 1;
            source = source.ToLower().PadLeft(paddedSourceLength);
            comparedTo = comparedTo.ToLower();
            var previousValues = new int[paddedSourceLength];
            var currentValues = new int[paddedSourceLength];
            for (int row = 0; row < paddedSourceLength; row++)
            {
                previousValues[row] = row;
            }

            for (int column = 1; column <= comparedToLength; column++)
            {
                for (int row = 0; row <= sourceLength; row++)
                {
                    int cost = 0;
                    bool isFirst = row == 0;
                    int previousColumn = column - 1;
                    if (isFirst || !source[row].Equals(comparedTo[previousColumn]))
                    {
                        cost = 1;
                    }

                    int minimumCost = previousValues[row] + 1;
                    if (!isFirst)
                    {
                        var previousRow = row - 1;
                        int diagonalIncrement = previousValues[previousRow] + cost;
                        int topIncrement = currentValues[previousRow] + 1;
                        minimumCost = Math.Min(Math.Min(diagonalIncrement, topIncrement), minimumCost);
                    }

                    currentValues[row] = minimumCost;
                }

                previousValues = currentValues;
                currentValues = new int[paddedSourceLength];
            }
            return previousValues[source.Length - 1];
        }
    }
}
