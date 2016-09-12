using System;
using System.Collections.Generic;
using System.Linq;

namespace ElaRandom
{
    static class RandomScorer
    {
        public static double GetRandomScore(string input, out double possiblePairs)
        {
            string[] substrings = input.Split(new[] {'X'}, StringSplitOptions.RemoveEmptyEntries).Where(substring=>substring.Length>=2).ToArray();
            possiblePairs = substrings.Sum(substring => substring.Length - 1);
            double expected = possiblePairs / _pairs.Count;
            
            double num = _pairs.Sum(pair =>
                        Math.Pow(substrings.Sum(substring => CountOccur(substring, pair)) - expected, 2)/
                        (expected*expected));
            Console.WriteLine(expected + " " + num);
            return num;
        }

        private static int CountOccur(string source, string substring)
        {
            int count = 0, n = 0;

            if (substring != "")
            {
                while ((n = source.IndexOf(substring, n, StringComparison.InvariantCulture)) != -1)
                {
                    n++;
                    ++count;
                }
            }
            return count;
        }

        private static readonly HashSet<string> _pairs =
            new HashSet<string>(new[] {"1", "2", "3", "4"}.SelectMany(s1 => new[] {"1", "2", "3", "4"}, (s1, s2) => s1 + s2));
    }
}