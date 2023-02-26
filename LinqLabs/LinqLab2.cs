using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqLabs
{
    internal class LinqLab2
    {
        public static void StartLab2_Frequency_Text_Analysis(string filePath)
        {
            List<char> allChars = new(200);

            using (StreamReader sr = new(File.OpenRead(filePath)))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    var chars = line.ToLower().ToCharArray();

                    foreach (var chr in chars)
                    {
                        allChars.Add(chr);
                    }
                }
            }

            var query = allChars.OrderBy(chr => chr).GroupBy(chr => (int)chr).OrderBy(group => group.Key).Select(g => g);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Количество разных символов:  {0}\n", query.Count());
            Console.ForegroundColor = ConsoleColor.White;

            byte ctr = 0;
            foreach (var group in query)
            {
                if (group.Key < 950 || group.Key > 3000)
                {
                    Console.Write("{0:d4} \"{1}\" {2}\t", group.Key, (char)group.Key, group.Count());
                    ++ctr;
                    if (ctr % 4 == 0) Console.WriteLine();
                }
            }
            Console.WriteLine("\n");

            ctr = 0;
            foreach (var group in query)
            {
                if (group.Key >= 950 && group.Key <= 3000)
                {
                    Console.Write("{0:d4} \"{1}\" {2}\t", group.Key, (char)group.Key, group.Count());
                    ++ctr;
                    if (ctr % 4 == 0) Console.WriteLine();
                }
            }
            Console.WriteLine("\n");
        }


    }
}
