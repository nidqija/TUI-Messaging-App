using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Security.Cryptography.X509Certificates;

namespace TUI_Messaging_App.TUI_Messaging_App.Services
{
    internal class DatabaseService
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messaging_app.db");
        private string connectionString = $"Data Source={dbPath}";


        public void initializeDB() {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS users (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL UNIQUE,
                        password TEXT NOT NULL
                    );
                ";
                tableCmd.ExecuteNonQuery();

                if (tableCmd.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine("Table 'users' created successfully.");
                }
                else
                {
                    Console.WriteLine("Table 'users' already exists or failed to create.");
                }
            }

           

        }
    }
}
