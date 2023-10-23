using Microsoft.AspNetCore.Mvc;
using ShowSqliteData.Interfaces;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        ISqliteViewer sqliteViewer;

        public HomeController(ISqliteViewer sqliteViewer)
        {
            this.sqliteViewer = sqliteViewer;
        }

        [Route("")]
        public IActionResult GetAllTableNames(string DatabasePath)
        {
            string[] databasesPaths = sqliteViewer.GetSqliteDatabasesPaths();
            List<string> databasesNames = new List<string>();

            foreach (string databasePath in databasesPaths)
            {
                databasesNames.Add(databasePath.Substring(databasePath.LastIndexOf('\\') + 1));
            }

            ViewBag.DatabasesDirectories = databasesPaths;
            ViewBag.DatabasesNames = databasesNames;

            if (string.IsNullOrEmpty(DatabasePath))
                if (databasesPaths.Length > 0) DatabasePath = databasesPaths[0];

            ViewBag.CurrentDataBase = DatabasePath;
            return View(sqliteViewer.GetAllTableNames(DatabasePath));
        }

        public IActionResult GetTableData(string tableName, string DatabasePath)
        {
            var conditions = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());
            return View(sqliteViewer.GetTableData(tableName, DatabasePath, conditions));
        }

        public IActionResult RunCustomQuery(string query, string DatabasePath)
        {
            string[] databases = sqliteViewer.GetSqliteDatabasesPaths();
            if (string.IsNullOrEmpty(DatabasePath))
                if (databases.Length > 0) DatabasePath = databases[0];

            ViewBag.CurrentDataBase = DatabasePath;
            if (string.IsNullOrEmpty(query)) return View();
            return View("GetTableData", sqliteViewer.SendQuery(query, DatabasePath));
        }
    }
}