using System;
using System.Collections.Generic;
using System.Linq;

namespace ElaRandom
{
    class Program
    {
        private static void Main()
        {
            for (int run = 0; run < 100; run++)
            {
                const int STR_LEN = 180;
                Random r = new Random(run);
                string input = "";
                for (int i = 0; i < STR_LEN; i++)
                    input += r.Next(1, 5);
                HashSet<string> substrs =
                    new HashSet<string>(
                        new[] {"1", "2", "3", "4"}.SelectMany(s => new[] {s + "1", s + "2", s + "3", s + "4"}));
                //foreach(string substr in substrs)
                //    Console.WriteLine($"{substr}: " + CountOccur(input,substr));
                int expected = STR_LEN/substrs.Count;
                double score = substrs.Aggregate(0, (i, s) => (int) (i + Math.Pow(CountOccur(input, s) - expected, 2)));
                Console.WriteLine(score/STR_LEN);
            }
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
    }
}
