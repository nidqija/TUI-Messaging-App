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



        public T GetSingle<T>(string sql, object parameters)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // This finds the first match and maps it to your class (UserModel)
                return connection.QueryFirstOrDefault<T>(sql, parameters);
            }
        }


        public bool checkifExists(string sql, object parameters)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                if (command == null)
                {
                    Console.WriteLine("Failed to create SQL command.");
                    return false;
                }
                var result = connection.QuerySingle<int>(sql, parameters);
                return result > 0;
            }
        }


        public void initializeDB()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS users (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL UNIQUE,
                        password TEXT NOT NULL ,
                        email TEXT NOT NULL UNIQUE
                    );
                ";
                tableCmd.ExecuteNonQuery();

                var requestsTableCmd = connection.CreateCommand();
                requestsTableCmd.CommandText =
                    @"
                    CREATE TABLE IF NOT EXISTS requests (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        sender_id INTEGER NOT NULL,
                        receiver_id INTEGER NOT NULL,
                        message TEXT,
                        status TEXT NOT NULL,
                        FOREIGN KEY (sender_id) REFERENCES users(id),
                        FOREIGN KEY (receiver_id) REFERENCES users(id)
                    );";

                requestsTableCmd.ExecuteNonQuery();

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


        public void getConnection()
        {
            using (var connection = new SqliteConnection(connectionString))
            {

                if (connection == null)
                {
                    Console.WriteLine("Failed to create database connection.");
                    return;
                }
                connection.Open();
                Console.WriteLine("Database connection established successfully.");
            }
        }



        public void performSQLOperation(string sql)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;

                if (command == null)
                {
                    Console.WriteLine("Failed to create SQL command.");
                    return;
                }

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} rows affected.");

                
            }
        }
    }
}
