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
        private readonly Services.RedisMessagingServices redisMessagingServices = new Services.RedisMessagingServices();
        public bool insertMessage(string senderUsername, string receiverUsername, string messageContent)
        {

            Console.WriteLine($"Attempting to send message from {senderUsername} to {receiverUsername} with content: {messageContent}");

            return redisMessagingServices.insertMessage(senderUsername, receiverUsername, messageContent);

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

        public List<MessagesModal> getLatestMessage(string user1, string user2)
        {
            var messages = getMessagesBetweenUsers(user1, user2);
            if (messages.Count > 0)
            {
                return new List<MessagesModal> { messages.Last() };
            }
            else
            {
                Console.WriteLine($"No messages found between {user1} and {user2}.");
                return new List<MessagesModal>();
            }
        }
    }
}
