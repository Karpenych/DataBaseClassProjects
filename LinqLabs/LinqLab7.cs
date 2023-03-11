using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LinqLabs
{
    internal class LinqLab7
    {
        static void AddCortej(string table, params string[] values)
        {
            string cmdText;
            if (table == "Book")
                cmdText = "INSERT INTO " + table + " VALUES (N'" + values[0] + "', " + int.Parse(values[1]) + ", " + int.Parse(values[2]) + ")";
            else
                cmdText = "INSERT INTO " + table + " VALUES (N'" + values[0] + "')";

            SqlCommand sqlCmd = new(cmdText, LinqLab6.sqlCon);

            sqlCmd.ExecuteNonQuery();
        }

        static void DeleteCortej(string table, string name)
        {
            string cmdText;
            if (table == "Book")
                cmdText = "DELETE FROM " + table + " WHERE Title = N'" + name + "'";
            else if (table == "Genre")
                cmdText = "DELETE FROM " + table + " WHERE Genre = N'" + name + "'";
            else
                cmdText = "DELETE FROM " + table + " WHERE AuthorName = N'" + name + "'";

            SqlCommand sqlCmd = new(cmdText, LinqLab6.sqlCon);

            sqlCmd.ExecuteNonQuery();
        }

        static void UpdateCortej(string table, string _old, string _new)
        {
            string cmdText;
            if (table == "Book")
                cmdText = "UPDATE " + table + " SET Title = N'" + _new + "' WHERE Title = N'" + _old + "'";
            else if (table == "Genre")
                cmdText = "UPDATE " + table + " SET Genre = N'" + _new + "' WHERE Genre = N'" + _old + "'";
            else
                cmdText = "UPDATE " + table + " SET AuthorName = N'" + _new + "' WHERE AuthorName = N'" + _old + "'";

            SqlCommand sqlCmd = new(cmdText, LinqLab6.sqlCon);

            sqlCmd.ExecuteNonQuery();
        }

        static void EnableTriggers()
        {
            string cmdText = "ENABLE TRIGGER Author_INSERT, Author_DELETE, Author_Update ON Author;\n" +
                             "ENABLE TRIGGER Book_DELETE, Book_INSERT, Book_UPDATE ON Book;\n" +
                             "ENABLE TRIGGER Genre_DELETE, Genre_INSERT, Genre_UPDATE ON Genre;";

            SqlCommand sqlCmd = new(cmdText, LinqLab6.sqlCon);

            sqlCmd.ExecuteNonQuery();
        }



        public static void StartLab7()
        {
            LinqLab6.ConnectToSQLDB();

            EnableTriggers();

            AddCortej("Book", "Capibara", "1003", "1002");

            LinqLab6.CloseSQLDB();
        }
    }
}
