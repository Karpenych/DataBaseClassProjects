using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;

namespace LinqLabs
{
    internal class LinqLab5
    {
        public static void StartLab5_CreateXMLfromDataSet(string dbFilePath)
        {
            LinqLab4.OpenDbAccess(dbFilePath);
            LinqLab4.CreateDataSetTables();
            LinqLab4.FillDataSetTables();

            LinqLab4.MyDS.EnforceConstraints = false;

            string scheme = LinqLab4.MyDS.GetXmlSchema();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("XML схема:\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(scheme + "\n");

            string file = $"../../../../materials/xml_created_from_DataSet.xml";
            LinqLab4.MyDS.WriteXml(file, System.Data.XmlWriteMode.WriteSchema);

            LinqLab4.Connection.Close();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("База данных отключена. Текущий статус: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}\n", LinqLab4.Connection.State.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void StartLab5_CreateXMLfromDataSet_Only_Changes(string dbFilePath)
        {
            LinqLab4.OpenDbAccess(dbFilePath);
            LinqLab4.CreateDataSetTables();
            LinqLab4.FillDataSetTables();

            LinqLab4.MyDS.EnforceConstraints = false;
            LinqLab4.MyDS.AcceptChanges();

            string[] titles = new string[] 
            { 
                "Убийство в восточном экспрессе",
                "Убийство на  Ниле",
                "В 4.50 из Паддингтона" 
            };

#pragma warning disable CS8602
            for (byte i = 0; i < titles.Length; ++i)
            {
                DataRow newRow = LinqLab4.MyDS.Tables["Book"].NewRow();
                newRow["Title"] = titles[i];
                newRow["id_gen"] = 2;
                newRow["id_auth"] = 2;
                LinqLab4.MyDS.Tables["Book"].Rows.Add(newRow);
            }

            for (int i = LinqLab4.MyDS.Tables["Author"].Rows.Count - 1; i >= 0; i--)
            {
#pragma warning disable CS8600  
                string str = LinqLab4.MyDS.Tables["Author"].Rows[i].ItemArray[1].ToString();
                if (str != "Кристи")
                {
                    LinqLab4.MyDS.Tables["Author"].Rows[i].Delete();
                }                                   				
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Изменённое в DataSet отношение Book:\n");
            Console.ForegroundColor = ConsoleColor.White;

            foreach (DataRow r in LinqLab4.MyDS.Tables["Book"].Rows)
            {
                string st = r.RowState.ToString();
                if (st != "Deleted")
                    Console.WriteLine("{0,-2}. {1,-35} ({2})", r["ID"], r["Title"], st);
            }
									
            if (LinqLab4.MyDS.HasChanges())
            {
#pragma warning disable CS8600
                DataSet Changes = LinqLab4.MyDS.GetChanges();
                Console.WriteLine("\nЧисло обновлений {0}", Changes.Tables.Count);
                Changes.WriteXml("../../../../materials/xml_only_changes.xml");
            }
            else 
                Console.WriteLine("\nИзменения не обнаружены");

            Console.WriteLine();
            LinqLab4.Connection.Close();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("База данных отключена. Текущий статус: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}\n", LinqLab4.Connection.State.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void StartLab5_CreateXMLfromDataSet_Only_Changes_2(string dbFilePath)
        {

        }

    }
}