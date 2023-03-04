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

            LinqLab4.CloseDbAccess();
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
            LinqLab4.CloseDbAccess();
        }

        public static void StartLab5_CreateXMLfromDataSet_Only_Changes_2(string xmlShortSchemePath, string xmlMainPath)
        {
            DataSet DS_1 = new();
            DS_1.ReadXmlSchema(xmlShortSchemePath);
#pragma warning disable CS0618
            XmlDataDocument xmlDoc = new(DS_1);
            xmlDoc.Load(xmlMainPath);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("DataSet связи:");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (DataRelation r in DS_1.Relations) 	
                Console.WriteLine(r.RelationName);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("DataSet отношения:");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (DataTable t in DS_1.Tables)			
                Console.WriteLine(t.TableName);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("DataSet атрибуты Book:");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (DataColumn c in DS_1.Tables["Book"].Columns)      
                Console.WriteLine("{0}", c.ColumnName);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("DataSet кортежи Book:");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (DataRow r in DS_1.Tables["Book"].Rows)	 
                Console.WriteLine("{0,-2}. {1} ", r.ItemArray[0], r.ItemArray[1]);
            Console.WriteLine();

            DS_1.EnforceConstraints = false;
            DS_1.AcceptChanges();

            string[] titles = new string[] 
            {
                "Оно", "Зелёная миля", "Сияние",
                "Гордость и предубеждения", "Разум и чувства", "Доводы рассудка"
            };

            DataRow newRow = DS_1.Tables["Author"].NewRow();
            newRow["AuthorName"] = "Остен";
            DS_1.Tables["Author"].Rows.Add(newRow);

            int id_aut = -1;
            foreach (DataRow r in DS_1.Tables["Author"].Rows)
            {
                if (r["AuthorName"].ToString() == "Остен") 
                { 
                    id_aut = (int)r["ID"]; 
                    break; 
                }
            }

            for (byte i = 0; i < titles.Length; ++i)
            {
                DataRow newRow_ = DS_1.Tables["Book"].NewRow();
                newRow_["Title"] = titles[i];
                if (i < 3) 
                    newRow_["id_auth"] = 3; 
                else 
                    newRow_["id_auth"] = id_aut;

                DS_1.Tables["Book"].Rows.Add(newRow_);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("кортежи Book:");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (DataRow r in DS_1.Tables["Book"].Rows)
            {
                string st = r.RowState.ToString();
                if (st != "Deleted")
                    Console.WriteLine("{0,-2}. {1,-35} {2,-2} ({3})", r["ID"], r["Title"], r["id_auth"], st);
            }

            if (DS_1.HasChanges())
            {
                DataSet Changes = DS_1.GetChanges();
                Changes.WriteXml("../../../../materials/xml_only_changes_2.xml");                
            }
            else 
                Console.WriteLine("Изменения не обнаружены");

            Console.WriteLine();
        }

        public static void CreateNewXMLScheme(string xmlToChangePath)
        {
#pragma warning disable CS0618
            XmlDataDocument xmlDoc = new();
            xmlDoc.Load(xmlToChangePath);
            string str = xmlDoc.InnerXml;    				   
            string pattern = "<xs:schema.+xs:schema>";
            string STR = System.Text.RegularExpressions.Regex.Matches(str, pattern).ElementAt(0).ToString();
            Console.WriteLine(STR);                      

            pattern = "<xs:element name=\"Genre\">.+?xs:element>";
            var STR1 = System.Text.RegularExpressions.Regex.Matches(STR.ToString(), pattern).First();
            Console.WriteLine("\n{0}  ", STR1);          

            pattern = "<xs:unique name=\"Genre.+?xs:unique>";
            var STR2 = System.Text.RegularExpressions.Regex.Matches(STR.ToString(), pattern).First();
            Console.WriteLine("\n{0}  ", STR2);

            pattern = "<xs:keyref name=\"Gen_Book\".+?xs:keyref>";
            var STR3 = System.Text.RegularExpressions.Regex.Matches(STR.ToString(), pattern).First();
            Console.WriteLine("\n{0}  ", STR3);             

            pattern = "<xs:element name=\"id_gen\".+?>";
            var STR4 = System.Text.RegularExpressions.Regex.Matches(STR.ToString(), pattern).First();
            Console.WriteLine("\n{0}  ", STR4);                 

            str = STR.Replace(STR1.ToString(), "")      
                     .Replace(STR2.ToString(), "")
                     .Replace(STR3.ToString(), "")
                     .Replace(STR4.ToString(), "")
                     .Replace("<".ToString(), "\n<");

            str = "<?xml version =\"1.0\" encoding=\"utf-8\" ?>" + str;
            Console.WriteLine("\n{0}  ", str);

            StreamWriter sw = new ("../../../../materials/mySchema.xml");
            sw.WriteLine(str);						
            sw.Close();
        }
    }
}