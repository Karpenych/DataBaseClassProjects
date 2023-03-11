using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Drawing;
using System.Data.OleDb;
using System.Runtime.Intrinsics.X86;
using System.Data;
using System.Xml.Linq;

#pragma warning disable CA1416

namespace LinqLabs
{
    internal class LinqLab6
    {
        public static string strCon = @"Data Source=(localdb)\LocalDB_MY_MSSQL;Initial Catalog=db1;"
                        + "Integrated Security=True;Connect Timeout=30;";

        public static SqlConnection? sqlCon = null;


        public static void ConnectToSQLDB()
        {
            sqlCon = new SqlConnection(strCon);
            sqlCon.Open();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("База данных SQL подключена. Текущий статус: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}\n", sqlCon.State.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        static public void CloseSQLDB()
        {
            sqlCon.Close();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("База данных SQL отключена. Текущий статус: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}\n", sqlCon.State.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void CreateSQL_db1()
        {
            SqlCommand sqlCmd = new()
            {
                Connection = sqlCon,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCmd.CommandText = "DeleteTables";
            sqlCmd.ExecuteNonQuery();
            sqlCmd.CommandText = "CreateTables";
            sqlCmd.ExecuteNonQuery();
        }

        static void CopyFromAccessToSQL()
        {
            DataTable dt = LinqLab4.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            foreach (DataRow item in dt.Rows)
            {
                string tableName = (string)item["TABLE_NAME"];

                if (tableName != "Book")
                {
                    OleDbCommand cmd = new()
                    {
                        Connection = LinqLab4.Connection,
                        CommandText = "SELECT * FROM " + tableName
                    };

                    string cmdText = "INSERT INTO " + tableName + " VALUES (N'";
                    SqlCommand sqlCmd = new(cmdText, sqlCon);

                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                sqlCmd.CommandText = cmdText + dr.GetString(1) + "')";
                                sqlCmd.ExecuteNonQuery();
                            }
                    }
                }
            }

            OleDbCommand cmdB = new()
            {
                Connection = LinqLab4.Connection,
                CommandText = "SELECT * FROM " + "Book"
            };

            string cmdTextB = "INSERT INTO " + "Book" + " VALUES (N'";
            SqlCommand sqlCmdB = new(cmdTextB, sqlCon);

            using (OleDbDataReader dr = cmdB.ExecuteReader())
            {
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        sqlCmdB.CommandText = cmdTextB + dr.GetString(1) + "',"
                                                         + dr.GetInt32(2) + ","
                                                         + dr.GetInt32(3) + ")";
                        sqlCmdB.ExecuteNonQuery();
                    }
            }
        }

        public static void StartLab6(string dbFilePat)
        {
            LinqLab4.OpenDbAccess(dbFilePat);
            ConnectToSQLDB();

            CreateSQL_db1();

            CopyFromAccessToSQL();

            LinqLab4.CloseDbAccess();
            CloseSQLDB();
        }

    }
}
