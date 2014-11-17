using System;
using System.Collections.Generic;

namespace NinjaNye.SearchExtensions.Levenshtein
{
    public static class LevenshteinProcessor
    {
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

            //return ComputeDistance(source, comparedTo);
            return ComputeDistanceMemoryEfficient(source, comparedTo);
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

            source = source.ToLower().PadLeft(sourceLength + 1);
            comparedTo = comparedTo.ToLower();
            var debug = new int[comparedToLength + 1,sourceLength + 1];
            for (int row = 0; row < sourceLength + 1; row++)
            {
                debug[0, row] = row;
            }

            for (int column = 1; column <= comparedToLength; column++)
            {
                for (int row = 0; row <= sourceLength; row++)
                {
                    int cost = 0;
                    bool hasRelated = true;
                    int previousColumn = column - 1;
                    if (row == 0)
                    {
                        cost = 1;
                        hasRelated = false;
                    }
                    else if (source[row] != comparedTo[previousColumn])
                    {
                        cost = 1;
                    }

                    int leftIncrement = debug[previousColumn, row] + 1;
                    int minimumCost;
                    if (hasRelated)
                    {
                        var previousRow = row - 1;
                        int diagonalIncrement = debug[previousColumn, previousRow] + cost;
                        int topIncrement = debug[column, previousRow] + 1;
                        minimumCost = Math.Min(Math.Min(diagonalIncrement, topIncrement), leftIncrement);
                    }
                    else
                    {
                        minimumCost = leftIncrement;
                    }

                    debug[column, row] = minimumCost;
                }
            }
            return debug[comparedToLength, sourceLength];
        }

        private static int ComputeDistanceMemoryEfficient(string source, string comparedTo)
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
