using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqLabs
{
    internal class LinqLab1
    {
        public static void StartLab1_words()
        {
            List<string> allWords = new(50);

            string filePath = "../../../../materials/lab1_words.txt";

            using (StreamReader sr = new(File.OpenRead(filePath)))
            {
                char[] separators = { ' ', ',', '.', '!', '?', ':', ';' };
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == "") continue;
                    var words = line.Split(separators);

                    foreach (var word in words)
                    {
                        var wrd = word.Trim();
                        if (wrd == "") continue;
                        allWords.Add(wrd);
                    }
                }
            }

            var query = allWords.Distinct().OrderBy(word => word).GroupBy(word => word.Length).OrderBy(group => group.First().Length).Select(group => group);

            foreach (var g in query)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write($"Length = {g.First().Length:d2} N = {g.LongCount():d2} - ");

                Console.ForegroundColor = ConsoleColor.White;
                foreach (var e in g) 
                    Console.Write($"{e}  ");

                Console.WriteLine("\n");
            }

            Console.WriteLine();
        }

        public static void StartLab1_digits()
        {
            List<string> digits = new(50);

            string filePath = "../../../../materials/lab1_digits.txt";

            using (StreamReader sr = new(File.OpenRead(filePath)))
            {
                char[] separators = { ' ', ',', '.', '!', '?', ':', ';' };
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == "") continue;
                    var words = line.Split(separators);

                    foreach (var word in words)
                    {
                        var wrd = word.Trim();
                        if (wrd == "") continue;

                        if (int.TryParse(wrd, out _))
                            digits.Add(wrd);
                    }
                }
            }

            var query = digits.Select(n => n);

            foreach (var el in query)
                Console.Write($"{el}  ");
            
            Console.WriteLine("\n");
        }
    }
}
