using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.Model
{
    internal class UserModel
    {

        private DatabaseService dbService;

        public bool createUser(string username , string password , string email)
        {
            Console.WriteLine($"User created with username: {username}, email: {email}");

            dbService = new DatabaseService();

            String sql = $"INSERT INTO users (username, email , password) VALUES ('{username}', '{email}' , '{password}')";

            dbService.performSQLOperation(sql);

            return true;

            

        }

        public bool validateUser(string username , string password)
        {
            Console.WriteLine($"Validating user with username: {username}");

            dbService = new DatabaseService();
            // We want to count how many users match this pair
            string sql = "SELECT COUNT(1) FROM users WHERE username = @username AND password = @password";

            // You should use parameters to prevent SQL Injection!
            return dbService.checkifExists(sql, new { username, password });
        }
    }
}
