using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ElaRandom
{
    class Program
    {
        private static void Main()
        {

            //Test();
            var files = Directory.GetFiles(".", "*.csv");
            foreach (FileInfo file in files.Select(f => new FileInfo(f)))
            {
                if (file.FullName.EndsWith("_output.csv"))
                    continue;
                string outputFile = Path.GetFileNameWithoutExtension(file.FullName) + "_output.csv";
                Dictionary<string, SortedList<int, char>> subDic = new Dictionary<string, SortedList<int, char>>();
                string[][] lines = File.ReadAllLines(file.FullName).Select(l=>l.Split(',')).ToArray();
                string[] title = lines[0];
                int trialNumCol = GetIndex(title, "trialnum");
                int responseCol = GetIndex(title, "response");
                int subnumCol = GetIndex(title, "subnum");
                if (trialNumCol == -1 || responseCol == -1 || subnumCol == -1)
                    continue;
                foreach (string[] line in lines.Skip(1))
                {
                    int trialNum = int.Parse(line[trialNumCol]);
                    char responseC = line[responseCol][0];
                    string sub = line[subnumCol];
                    if (!subDic.ContainsKey(sub))
                        subDic[sub] = new SortedList<int, char> {{trialNum, responseC}};
                    else
                        subDic[sub][trialNum] = responseC;
                }
                List<string> outputLines = new List<string> {"subnum,random score,possible pairs,response,response1234"};
                foreach (var kvp in subDic)
                {
                    string subString = new string(kvp.Value.Values.ToArray());
                    string subString1234 = subString.Replace("s", "1").Replace("d", "2").Replace("h", "3").Replace("j", "4");
                    double possiblePairs;
                    double subScore = RandomScorer.GetRandomScore(subString1234, out possiblePairs);
                    //Console.WriteLine(kvp.Key + " " + subScore);
                    outputLines.Add($"{kvp.Key},{subScore},{possiblePairs},{subString},'{subString1234}");
                }
                File.WriteAllLines(outputFile, outputLines);
            }
        }

        private static int GetIndex(string[] arr, string title)
        {
            int index = Array.IndexOf(arr, title);
            if (index == -1)
                Console.WriteLine($"Couldn't find column {title}");
            return index;
        }

        private static void Test()
        {
            foreach (int strlen in new int[] { 155, 160, 165, 170, 175, 180, 360, 1800, 3600 })
            {
                List<double> nums = new List<double>();
                for (int run = 0; run < 1000; run++)
                {
                    //const int STR_LEN = 180;
                    Random r = new Random(run);
                    string input = "";
                    for (int i = 0; i < strlen; i++)
                        input += r.Next(1, 5);
                    //input = "1434231432412123413124341324142433221114443323342241234121442234142341414214223332111223214342314324121234131243413241424332211144433233422412341214422341423414142142233321112232";
                    //double expected = (double) input.Length/16;
                    //foreach (string substr in substrs)
                    //    Console.WriteLine(
                    //        $"{substr}: {CountOccur(input, substr)} {Math.Pow(CountOccur(input, substr)-expected, 2)/(expected*expected)}");
                    double possiblePairs;
                    nums.Add(RandomScorer.GetRandomScore(input, out possiblePairs));
                }
                Console.WriteLine(strlen + " " + nums.Average());
            }
        }
    }
}
