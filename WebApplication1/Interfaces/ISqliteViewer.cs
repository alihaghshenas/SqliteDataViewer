namespace ShowSqliteData.Interfaces
{
    public interface ISqliteViewer
    {
        public List<List<string>> GetAllTableNames(string DatabasePath);
        public List<List<string>> GetTableData(string tableName,string DatabasePath, Dictionary<string, string> conditions);
        public List<List<string>> SendQuery(string query,string DatabasePath);
        public string[] GetSqliteDatabasesPaths();

    }
}
