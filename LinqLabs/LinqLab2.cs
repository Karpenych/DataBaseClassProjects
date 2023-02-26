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


        struct Anketa
        {
            public string fio { get; set; }
            public string city { get; set; }
            public string birthday { get; set; }
        }        

        public static void StartLab2_Except_Intersect_Union()
        {
            Anketa[] anketas1 =
            {
                new Anketa() { fio = "Иванов И.И.", city = "Владивосток", birthday = "2002.06.14" },
                new Anketa() { fio = "Краснов К.К.", city = "Уссурмйск", birthday = "1999.11.20" },
                new Anketa() { fio = "Зайцев З.З.", city = "Артём", birthday = "2012.03.06" },
                new Anketa() { fio = "Абобов А.А.", city = "Владивосток", birthday = "2000.04.01" },
                new Anketa() { fio = "Кеков К.К.", city = "Уссурийск", birthday = "1999.07.15" },
            };

            Anketa[] anketas2 =
            {
                new Anketa() { fio = "Иванов И.И.", city = "Владивосток", birthday = "2002.06.14" },
                new Anketa() { fio = "Краснов К.К.", city = "Уссурмйск", birthday = "1999.11.20" },
                new Anketa() { fio = "Амуров А.А.", city = "Артём", birthday = "2012.03.06" },
                new Anketa() { fio = "Абобов А.А.", city = "Владивосток", birthday = "2000.04.01" },
                new Anketa() { fio = "Лолов Л.Л.", city = "Уссурийск", birthday = "1999.07.15" },
            };

            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.WriteLine("Except");

            var queryExcept = anketas1.Except(anketas2); // Except1

            Console.ForegroundColor = ConsoleColor.Blue;  
            Console.WriteLine("  anketas1 - anketas2:");
            Console.ForegroundColor = ConsoleColor.White;
            foreach ( Anketa a in queryExcept ) 
                Console.WriteLine("    {0}  -  {1}  -  {2}", a.fio, a.city, a.birthday);

            queryExcept = anketas2.Except(anketas1); // Except2

            Console.ForegroundColor = ConsoleColor.Blue; 
            Console.WriteLine("  anketas2 - anketas1:");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (Anketa a in queryExcept)
                Console.WriteLine("    {0}  -  {1}  -  {2}", a.fio, a.city, a.birthday);
            Console.WriteLine('\n');

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Intersect");

            var queryIntersect = anketas1.Intersect(anketas2); // Intersect

            Console.ForegroundColor = ConsoleColor.White;
            foreach (Anketa a in queryIntersect)
                Console.WriteLine("  {0}  -  {1}  -  {2}", a.fio, a.city, a.birthday);
            Console.WriteLine('\n');


            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Union");

            var queryUnion = anketas1.Union(anketas2); // Union

            Console.ForegroundColor = ConsoleColor.White;
            foreach (Anketa a in queryUnion)
                Console.WriteLine("  {0}  -  {1}  -  {2}", a.fio, a.city, a.birthday);
            Console.WriteLine('\n');
        }
    }
}
