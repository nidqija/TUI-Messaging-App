using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Model;

namespace TUI_Messaging_App.TUI_Messaging_App.Controller
{
    internal class MessagesController
    {

        MessagesModal messagesModal = new MessagesModal();
        public bool insertMessage(string senderUsername, string receiverUsername, string messageContent)
        {

            Console.WriteLine($"Attempting to send message from {senderUsername} to {receiverUsername} with content: {messageContent}");

            if (messagesModal.insertMessage(senderUsername, receiverUsername, messageContent))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Failed to send message. Please check the database connection and ensure the message content is valid.");
                return false;
            }

        }

        public List<MessagesModal> getMessagesBetweenUsers(string user1, string user2)
        {

            if (string.IsNullOrEmpty(user1) || string.IsNullOrEmpty(user2))
            {
                Console.WriteLine("Usernames cannot be null or empty. Please provide valid usernames.");
                return new List<MessagesModal>();
            }


            return messagesModal.getMessagesBetweenUsers(user1, user2);
        }
    }
}
