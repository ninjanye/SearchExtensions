using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NinjaNye.SearchExtensions.Portable.Tests
{
    public class BuildStringTestsBase
    {
        private const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        protected IList<string> BuildWords(int wordCount, int minSize = 2, int maxSize = 10)
        {
            Console.WriteLine("Building {0} words...", wordCount);
            var result = new List<string>();
            for (int i = 0; i < wordCount; i++)
            {
                string randomWord = BuildRandomWord(minSize, maxSize);
                result.Add(randomWord);
            }
            Console.WriteLine("Built words: {0}", wordCount);
            return result;
        }

        protected string BuildRandomWord(int minSize, int maxSize)
        {
            var letterCount = RandomInt(minSize, maxSize);
            var sb = new StringBuilder(letterCount);
            for (int i = 0; i < letterCount; i++)
            {
                var letterIndex = RandomInt(0, 51);
                sb.Append(letters[letterIndex]);
            }
            return sb.ToString();
        }

        private int RandomInt(int min, int max)
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[4];

            rng.GetBytes(buffer);
            int result = BitConverter.ToInt32(buffer, 0);

            return new Random(result).Next(min, max);
        }
    }
}