using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.Model
{
    internal class MessagesModal
    {
        public int Id { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public string MessageContent { get; set; }
        public DateTime Timestamp { get; set; }

        DatabaseService databaseService = new DatabaseService();


        public bool insertMessage(string senderUsername, string receiverUsername, string messageContent)
        {
            if (string.IsNullOrEmpty(senderUsername) || string.IsNullOrEmpty(receiverUsername) || string.IsNullOrEmpty(messageContent))
            {
                Console.WriteLine("Sender, receiver, and message content cannot be empty.");
                return false;
            }


            string sql = $"INSERT INTO messages (sender_username, receiver_username, message, timestamp) " +
                         $"VALUES ('{senderUsername}', '{receiverUsername}', '{messageContent}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";

            databaseService.performSQLOperation(sql);

            if (databaseService == null)
            {
                Console.WriteLine("Database service is not initialized.");
                return false;
            }

            Console.WriteLine($"Message from {senderUsername} to {receiverUsername}: {messageContent}");
            return true; // Return true if the message was successfully "sent"
        }


        public List<MessagesModal> getMessagesBetweenUsers(string user1, string user2)
        {
            // Note the "message AS MessageContent" and "sender_username AS SenderUsername"
            string sql = $"SELECT id, " +
                         $"sender_username AS SenderUsername, " +
                         $"receiver_username AS ReceiverUsername, " +
                         $"message AS MessageContent, " +
                         $"timestamp AS Timestamp " +
                         $"FROM messages " +
                         $"WHERE (sender_username = '{user1}' AND receiver_username = '{user2}') " +
                         $"OR (sender_username = '{user2}' AND receiver_username = '{user1}') " +
                         $"ORDER BY timestamp ASC";

            return databaseService.GetList<MessagesModal>(sql).ToList();
        }

       
    }
}
