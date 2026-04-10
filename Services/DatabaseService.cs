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



        // fetch any single query from the database based on table queries
        
        public T GetSingle<T>(string sql, object parameters)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // This finds the first match and maps it to your class (UserModel)
                return connection.QueryFirstOrDefault<T>(sql, parameters);
            }
        }

        // fetch all queries from the database based on table queries
        public IEnumerable<T> GetList<T>(string sql, object parameters = null)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                return connection.Query<T>(sql , parameters);

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


                var messagesTableCmd = connection.CreateCommand();

                // Inside initializeDB()
                messagesTableCmd.CommandText = @"
                   CREATE TABLE IF NOT EXISTS messages (
                     id INTEGER PRIMARY KEY AUTOINCREMENT,
                     sender_username TEXT NOT NULL,
                     receiver_username TEXT NOT NULL,
                     message TEXT,
                     timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                     FOREIGN KEY (sender_username) REFERENCES users(username),
                     FOREIGN KEY (receiver_username) REFERENCES users(username)
                 );";


                messagesTableCmd.ExecuteNonQuery();

                var chatRoomTableCms = connection.CreateCommand();

                chatRoomTableCms.CommandText = @"
                    CREATE TABLE IF NOT EXISTS chat_rooms (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        room_name TEXT NOT NULL UNIQUE,
                        user_id INTEGER NOT NULL,  
                        timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (user_id) REFERENCES users(id)
                    );";

                chatRoomTableCms.ExecuteNonQuery(); 


                var groupChatTableCmd = connection.CreateCommand();

                groupChatTableCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS room_members (
                        room_id INTEGER NOT NULL,
                        user_id INTEGER NOT NULL,
                        joined_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                        PRIMARY KEY (room_id, user_id),
                        FOREIGN KEY (room_id) REFERENCES chat_rooms(id) ON DELETE CASCADE,
                        FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
                  ); ";

                groupChatTableCmd.ExecuteNonQuery();


                // ADD THIS: Create system user for Ollama if it doesn't exist
                var systemUserCmd = connection.CreateCommand();
                systemUserCmd.CommandText = @"
                    INSERT OR IGNORE INTO users (username, password, email) 
                    VALUES ('ollama', 'SYSTEM_USER', 'ollama@system.local');
                ";
                systemUserCmd.ExecuteNonQuery();

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

        public void performSQLOperationWithParameters(string sql, Dictionary<string, object> parameters)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;

                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} rows affected.");
            }
        }
    }
}
