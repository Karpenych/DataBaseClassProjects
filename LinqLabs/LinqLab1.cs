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
        public static void StartLab1_words(string filePath)
        {
            List<string> allWords = new(50);

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

            var query = allWords.Distinct().OrderBy(word => word).GroupBy(word => word.Length).OrderBy(group => group.Key).Select(group => group);

            foreach (var group in query)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write($"Length = {group.Key:d2} N = {group.LongCount():d2} - ");

                Console.ForegroundColor = ConsoleColor.White;
                foreach (var element in group) 
                    Console.Write($"{element}  ");

                Console.WriteLine("\n");
            }

            Console.WriteLine();
        }

        public static void StartLab1_digits(string filePath)
        {
            List<string> digits = new(50);

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
