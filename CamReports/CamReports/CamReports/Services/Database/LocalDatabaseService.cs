using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CamReports.Helpers;
using CamReports.Models;
using GalaSoft.MvvmLight.Ioc;
using SQLite.Net;
using SQLite.Net.Interop;
using Xamarin.Forms;

namespace CamReports.Services.Database
{
    public class LocalDatabaseService
    {
        public void Initialize()
        {
            var databasePath = DependencyService.Get<IFileHelper>().GetLocalFilePath("appDatabase.db3");
            ISQLitePlatform sqlitePlatform = SimpleIoc.Default.GetInstance<ISQLitePlatform>();

            DatabaseConnection = new SQLiteConnection(sqlitePlatform, databasePath);
            TableMapping map = new TableMapping(typeof(Issue), new PropertyInfo[]{}); // Instead of mapping to a specific table just map the whole database type
            
            int tableCount = DatabaseConnection.Query(map, "SELECT * FROM sqlite_master WHERE type = 'table' AND name = 'Issue'").Count; // Executes the query from which we can count the results
            if (tableCount == 0)
                DatabaseConnection.CreateTable<Issue>();

            Issues = new Issues(this);
        }

        public SQLiteConnection DatabaseConnection { get; private set; }

        public Issues Issues { get; private set; }
    }
}
