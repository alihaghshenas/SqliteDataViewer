
using Microsoft.Data.Sqlite;
using ShowSqliteData.Interfaces;
using System.Data;

namespace ShowSqliteData.Services
{
    public class SqliteViewerService : ISqliteViewer
    {
        private static readonly string QUERY_SELECT_ALL_TABLES = "SELECT tbl_name FROM sqlite_schema WHERE type = 'table'";
        public List<List<string>> GetAllTableNames(string DatabasePath)
        {
            if (string.IsNullOrEmpty(DatabasePath)) return new List<List<string>>();

            SqliteDataReader dataReader = ConnectToSqlite(QUERY_SELECT_ALL_TABLES, DatabasePath);
            var columns = GetTableColumns(dataReader);
            var rows = new List<List<string>>();

            rows.Add(columns);
            while (dataReader.Read()) 
            {
                var tempList = new List<string>();
                for (int i = 0; i < columns.Count; i++) {
                    tempList.Add(dataReader[columns[i]].ToString());
                }
                rows.Add(tempList);
            }

            dataReader.Close();
            return rows;
        }

        public List<List<string>> GetTableData(string tableName,string DatabasePath,Dictionary<string,string> conditions)
        {
            string QUERY = $"SELECT * FROM [{tableName}]" + DictionaryToQuery(conditions);

            SqliteDataReader dataReader = ConnectToSqlite(QUERY, DatabasePath);
            var columns = GetTableColumns(dataReader);
            var rows = new List<List<string>>();

            rows.Add(columns);
            while (dataReader.Read())
            {
                var tempList = new List<string>();
                for (int i = 0; i < columns.Count; i++)
                {
                    tempList.Add(dataReader[columns[i]].ToString());
                }
                rows.Add(tempList);
            }

            dataReader.Close();
            return rows;
        }

        public List<List<string>> SendQuery(string query, string DatabasePath)
        {
            try
            {
                SqliteDataReader dataReader = ConnectToSqlite(query, DatabasePath);
                var columns = GetTableColumns(dataReader);
                var rows = new List<List<string>>();

                rows.Add(columns);
                while (dataReader.Read())
                {
                    var tempList = new List<string>();
                    for (int i = 0; i < columns.Count; i++)
                    {
                        tempList.Add(dataReader[columns[i]].ToString());
                    }
                    rows.Add(tempList);
                }

                dataReader.Close();
                return rows;
            }
            catch (Exception ex) {
                var rows = new List<List<string>>();
                var tempList = new List<string>();
                tempList.Add("خطا");
                tempList.Add("کوئری قابل اجرا نیست");
                tempList.Add(ex.Message.ToString());
                rows.Add(tempList);
                return rows;
            }
        }

        public string[] GetSqliteDatabasesPaths() {
            string[] allfiles = Directory.GetFiles(
                Path.Combine(Environment.CurrentDirectory, "PrivateFiles", "LogData"),
                "*.db",SearchOption.AllDirectories);
            return allfiles;
        }

        private string DictionaryToQuery(Dictionary<string, string> conditions) { 
            string query = string.Empty;
            conditions.Remove("DatabasePath");
            conditions.Remove("tableName");
            if (conditions.Count != 0 && conditions.Where(x => x.Value != "").Count() > 0)
            {
                query += " WHERE ";


                foreach (var condition in conditions)
                {
                    if (!string.IsNullOrEmpty(condition.Value))
                    {
                        if (condition.Key != conditions.Last(x => x.Value != "").Key)
                            query += condition.Key + " LIKE '%" + condition.Value + "%' AND ";
                        else
                            query += condition.Key + " LIKE '%" + condition.Value + "%';";
                    }
                }
            }
            return query;
        }

        private SqliteDataReader ConnectToSqlite(string query,string connectionString) {
            SqliteConnection connection = new SqliteConnection("Data Source=" + connectionString);
            connection.Open();

            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = query;

            SqliteDataReader? dataReader = command.ExecuteReader();
            return dataReader;
        }

        private List<string> GetTableColumns(SqliteDataReader dataReader) {
            var columns = new List<string>();
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                columns.Add(dataReader.GetName(i));
            }
            return columns;
        }
    }
}
