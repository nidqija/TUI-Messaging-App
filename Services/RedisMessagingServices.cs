using StackExchange.Redis;
using System.Text.Json;
using TUI_Messaging_App.TUI_Messaging_App.Model;

namespace TUI_Messaging_App.TUI_Messaging_App.Services
{
    internal class RedisMessagingServices
    {
        // connect to redis server in localhost

        private static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        private readonly MessagesModal messagesModal = new MessagesModal();


        public bool insertMessage(string senderUsername, string receiverUsername, string messageContent)
        {
            if (messagesModal.insertMessage(senderUsername, receiverUsername, messageContent))
            {
                // Publish a message to the Redis channel to notify the receiver of a new message
                var sub = redis.GetSubscriber();

                // channel name is "messages:{receiverUsername}" to target the specific receiver
                string channel = $"messages:{receiverUsername}";

                // publish a simple message to the channel 
                // tells the channel that a new message has been sent to the receiver and they should refresh their chat view
                // event name is "REFRESH_CHAT"

                sub.Publish(channel, "REFRESH_CHAT");

                return true;




            }
            else
            {
                Console.WriteLine("Failed to send message. Please check the database connection and ensure the message content is valid.");
                return false; 
            }
                

        }
    }
}
