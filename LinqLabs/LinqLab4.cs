using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1416 // Проверка совместимости платформы в Visual Stidio 2022

namespace LinqLabs
{
    internal class LinqLab4
    {
        static OleDbConnection? con = null;
        static DataSet myDS = new();

        public static OleDbConnection Connection { get { return con; } }
        public static DataSet MyDS { get { return myDS; } }


        public static void OpenDbAccess(string dbFilePath)
        {
            string? conString = "Provider=Microsoft.ACE.OLEDB.12.0;"
                               + $"Data Source={dbFilePath};"
                               + "Persist Security Info=True;";
            con = new OleDbConnection(conString);
            try
            {
                con.Open(); 
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("База данных подключена. Текущий статус: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}\n", con.State.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); return;
            }
        }

        public static void ReadDBTable(string table_name)
        {
            OleDbCommand cmd = new()
            {
                Connection = con,
                CommandText = "SELECT * FROM " + table_name
            };

            try
            {
                using OleDbDataReader dr = cmd.ExecuteReader();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Читаем записи таблицы ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(table_name);
                Console.ForegroundColor = ConsoleColor.White;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int n = dr.GetInt32(0);
                        string str = dr.GetString(1);
                        if (table_name != "Book")
                            Console.WriteLine("\t{0}\t{1}", n, str);
                        else Console.WriteLine("\t{0}\t{1,-30}\t{2}\t{3}", n, str, dr.GetInt32(2), dr.GetInt32(3));
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Таблица пуста");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Указанной таблицы несуществует\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
        }

        public static void CreateDataSetTables()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Создание DataSet\n");
            Console.ForegroundColor = ConsoleColor.White;

            DataTable dtAuthor = new();
            DataColumn pr_keyAuthor = dtAuthor.Columns.Add("ID", typeof(int));
            pr_keyAuthor.Unique = true;			  
            pr_keyAuthor.AllowDBNull = false; 	  
            pr_keyAuthor.AutoIncrement = true; 	  
            pr_keyAuthor.AutoIncrementSeed = 1; 	  
            pr_keyAuthor.AutoIncrementStep = 1;
            dtAuthor.Columns.Add("AuthorName", typeof(string));
            dtAuthor.PrimaryKey = new DataColumn[] { pr_keyAuthor };
            myDS.Tables.Add(dtAuthor);
            myDS.Tables[0].TableName = "Author";
            Console.WriteLine("Имя таблицы: {0}\n\t{1}\t{2}\n", myDS.Tables[0].TableName, myDS.Tables[0].Columns[0], myDS.Tables[0].Columns[1]);       

            DataTable dtGenre = new();
            DataColumn pr_keyGenre = dtGenre.Columns.Add("ID", typeof(int));
            pr_keyGenre.Unique = true;
            pr_keyGenre.AllowDBNull = false;
            pr_keyGenre.AutoIncrement = true;
            pr_keyGenre.AutoIncrementSeed = 1;
            pr_keyGenre.AutoIncrementStep = 1;
            dtGenre.Columns.Add("Genre", typeof(string));
            dtGenre.PrimaryKey = new DataColumn[] { pr_keyGenre };
            myDS.Tables.Add(dtGenre);
            myDS.Tables[1].TableName = "Genre";
            Console.WriteLine("Имя таблицы: {0}\n\t{1}\t{2}\n", myDS.Tables[1].TableName, myDS.Tables[1].Columns[0], myDS.Tables[1].Columns[1]);

            DataTable dtBook = new();
            DataColumn pr_keyBook = dtBook.Columns.Add("ID", typeof(int));
            pr_keyBook.Unique = true;
            pr_keyBook.AllowDBNull = false;
            pr_keyBook.AutoIncrement = true;
            pr_keyBook.AutoIncrementSeed = 1;
            pr_keyBook.AutoIncrementStep = 1;
            dtBook.Columns.Add("Title", typeof(string));
            dtBook.Columns.Add("id_gen", typeof(int));
            dtBook.Columns.Add("id_auth", typeof(int));
            dtBook.PrimaryKey = new DataColumn[] { pr_keyBook };
            myDS.Tables.Add(dtBook);
            myDS.Tables[2].TableName = "Book";
            Console.WriteLine("Имя таблицы: {0}\n\t{1}\t{2}\t{3}\t{4}\n", myDS.Tables[2].TableName, myDS.Tables[2].Columns[0], 
                myDS.Tables[2].Columns[1], myDS.Tables[2].Columns[2], myDS.Tables[2].Columns[3]);

            DataColumn parentCol_A = myDS.Tables["Author"].Columns["ID"];
            DataColumn childCol_A = myDS.Tables["Book"].Columns["id_auth"];
            myDS.Relations.Add(new DataRelation("Auth_Book", parentCol_A, childCol_A));
            myDS.Relations[0].Nested = true;

            DataColumn parentCol_G = myDS.Tables["Genre"].Columns["ID"];
            DataColumn childCol_G = myDS.Tables["Book"].Columns["id_gen"];
            myDS.Relations.Add(new System.Data.DataRelation("Gen_Book", parentCol_G, childCol_G));

            Console.WriteLine("Связи:");
            for (int i = 0; i < 2; i++)
            {
                DataRelation rd = myDS.Relations[i];
                string str =
                              "\nRelationName         = " + rd.RelationName +
                              "\nParentTable          = " + rd.ParentTable.TableName +
                              "\nChildTable           = " + rd.ChildTable.TableName +
                              "\nParentKeyConstraint  = " + rd.ParentKeyConstraint.GetType() +
                              "\nIsPrimaryKey         = " + rd.ParentKeyConstraint.IsPrimaryKey +
                              "\nChildKeyConstraint   = " + rd.ChildKeyConstraint.GetType() +
                              "\nChildKeyConstraint   = " + rd.ChildKeyConstraint.DeleteRule.ToString();
                Console.WriteLine("{0}", str);
            }
            Console.WriteLine();
        }

        public static void FillDataSetTables()
        {
            OleDbCommand cmd = new()
            {
                Connection = con,
                CommandText = "SELECT * FROM Author"
            };

            using (OleDbDataReader dr = cmd.ExecuteReader())
            {
                DataRow? nRow;
                if (dr.HasRows)
                {
                    while (dr.Read())               
                    {
                        nRow = myDS.Tables["Author"].NewRow();
                        nRow["ID"] = dr.GetInt32(0);
                        nRow["AuthorName"] = dr.GetString(1);
                        myDS.Tables["Author"].Rows.Add(nRow); 
                    }
                }
            }

            cmd.CommandText = "SELECT * FROM Genre";

            using (OleDbDataReader dr = cmd.ExecuteReader())
            {
                DataRow? nRow;
                if (dr.HasRows)
                {
                    while (dr.Read())               
                    {
                        nRow = myDS.Tables["Genre"].NewRow();
                        nRow["ID"] = dr.GetInt32(0);
                        nRow["Genre"] = dr.GetString(1);
                        myDS.Tables["Genre"].Rows.Add(nRow); 
                    }
                }
            }

            cmd.CommandText = "SELECT * FROM Book";

            using (OleDbDataReader dr = cmd.ExecuteReader())
            {
                DataRow? nRow;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        nRow = myDS.Tables["Book"].NewRow();
                        nRow["ID"] = dr.GetInt32(0);
                        nRow["Title"] = dr.GetString(1);
                        nRow["id_gen"] = dr.GetInt32(2);
                        nRow["id_auth"] = dr.GetInt32(3);
                        myDS.Tables["Book"].Rows.Add(nRow);
                    }
                }
            }
        }

        public static void SelectRowsFromDataSet(string table)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выбрать картежи отношения из DataSet:");
            Console.ForegroundColor = ConsoleColor.White;

            DataRow[] _table = new DataRow[myDS.Tables[table].Rows.Count];
            myDS.Tables[table].Rows.CopyTo(_table, 0);

            var query = _table.Select(n => n);

            foreach (var row in query)
            {
                if (table != "Book")
                    Console.WriteLine("\t{0}\t{1}", row[0], row[1]);
                else 
                    Console.WriteLine("\t{0}\t{1,-30}\t{2}\t{3}", row[0], row[1], row[2], row[3]);
            }       
            Console.WriteLine();
        }

        public static void SelectSortedRowsFromDataSet(string table)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выбрать и отсортировать картежи отношения из DataSet:");
            Console.ForegroundColor = ConsoleColor.White;

            DataRow[] _table = new DataRow[myDS.Tables[table].Rows.Count];
            myDS.Tables[table].Rows.CopyTo(_table, 0);

            var query = _table.OrderBy(m => m.ItemArray[1]).Select(n => n);

            foreach (var row in query)
            {
                if (table != "Book")
                    Console.WriteLine("\t{0}\t{1}", row[0], row[1]);
                else
                    Console.WriteLine("\t{0}\t{1,-30}\t{2}\t{3}", row[0], row[1], row[2], row[3]);
            }
            Console.WriteLine();
        }

        public static void InsertAuthorInDataSet(string authorName)
        {
            DataRow row = myDS.Tables["Author"].NewRow();
            row["AuthorName"] = authorName;
            myDS.Tables["Author"].Rows.Add(row);
        }


        public static void StartLab4(string dbFilePath)
        {
            OpenDbAccess(dbFilePath);

            ReadDBTable("Book");
            ReadDBTable("Author");
            ReadDBTable("Genre");

            CreateDataSetTables();
            FillDataSetTables();

            SelectRowsFromDataSet("Author");

            SelectSortedRowsFromDataSet("Author");
#pragma warning disable CS8602       
            con.Close();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("База данных отключена. Текущий статус: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}\n", con.State.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
