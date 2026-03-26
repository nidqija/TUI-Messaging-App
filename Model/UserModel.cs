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

        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public UserModel()
        {
            dbService = new DatabaseService();
        }

        public bool createUser(string username, string password, string email)
        {
            Console.WriteLine($"User created with username: {username}, email: {email}");

            String sql = $"INSERT INTO users (username, email , password) VALUES ('{username}', '{email}' , '{password}')";

            dbService.performSQLOperation(sql);

            return true;
        }

        // This method will validate if the user exists in the database with the given username and password
        public UserModel validateUser(string username, string password)
        {
            Console.WriteLine($"Validating user with username: {username}");

            // We want to count how many users match this pair
            string sql = "SELECT id, username, email FROM users WHERE username = @username AND password = @password";

            // You should use parameters to prevent SQL Injection!
            // once we get the result , we will return a usermodel object with the data of the current user if it exists ,
            // otherwise we will return null

            return dbService.GetSingle<UserModel>(sql, new { username = username, password = password });
        }

        public UserModel searchUser(string username)
        {
            Console.WriteLine($"Searching for user with username: {username}");

            string sql = "SELECT id, username, email FROM users WHERE username = @username";

            return dbService.GetSingle<UserModel>(sql, new { username = username });
        }


        public bool sendMessageAsRequest(string senderUsername, string receiverUsername, string messageContent)
        {
            Console.WriteLine($"Sending message from {senderUsername} to {receiverUsername} with content: {messageContent}");

            // First, we need to get the user IDs of the sender and receiver

            UserModel sender = searchUser(senderUsername);
            UserModel receiver = searchUser(receiverUsername);
            if (sender == null || receiver == null)
            {
                Console.WriteLine("Sender or receiver does not exist.");
                return false;
            }


            string sql = $"INSERT INTO requests (sender_id, receiver_id, message , status) VALUES ({sender.id}, {receiver.id}, '{messageContent}' , 'pending' )";
            dbService.performSQLOperation(sql);
            return true;

        }


        public bool acceptMessageRequest(string senderUsername, string receiverUsername)
        {
            Console.WriteLine($"Accepting message request from {senderUsername} to {receiverUsername}");
            UserModel sender = searchUser(senderUsername);
            UserModel receiver = searchUser(receiverUsername);
            if (sender == null || receiver == null)
            {
                Console.WriteLine("Sender or receiver does not exist.");
                return false;
            }
            string sql = $"UPDATE requests SET status = 'accepted' WHERE sender_id = {sender.id} AND receiver_id = {receiver.id} AND status = 'pending'";
            dbService.performSQLOperation(sql);
            return true;

        }


       /* public bool seeApprovedMessageRequest(string senderUsername , string receiverUsername)
        {
            Console.WriteLine($"Seeing approved message request from {senderUsername} to {receiverUsername}");
            UserModel sender = searchUser(senderUsername);
            UserModel receiver = searchUser(receiverUsername);

            if (sender == null || receiver == null)
            {
                Console.WriteLine("Sender or receiver does not exist.");
                return false;
            }

            string sql = $"SELECT receiver_id FROM requests WHERE sender_id= {sender.id}";

            dbService.performSQLOperation(sql);

            return true;
        } */

    }
}
