using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Model;
using TUI_Messaging_App.TUI_Messaging_App.View;

namespace TUI_Messaging_App.TUI_Messaging_App.Controller
{


    internal class ChatRoomController
    {

        private ChatRoomModel chatRoomModel = new ChatRoomModel();

        public bool handleCreateChatRoom(int userId, string roomName)
        {
            if (userId.Equals(null) || roomName.Equals(""))
            {
                Console.WriteLine("User ID and room name cannot be empty.");
                return false;
            }


            if (chatRoomModel.insertNewGroup(roomName, userId))
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to create chat room '{roomName}' for user ID: {userId}");
                return false;
            }

        }

        public List<ChatRoomModel.GroupChatObject> handleFetchChatRooms(int userId)
        {
            if (userId.Equals(null))
            {
                // This check is redundant since userId is a value type (int) and cannot be null.
                Console.WriteLine("User ID cannot be empty.");
                return new List<ChatRoomModel.GroupChatObject>();
            }

            var chatRooms = chatRoomModel.fetchAllChatRoom(userId);

            if (chatRooms == null || !chatRooms.Any())
            {
                Console.WriteLine($"No chat rooms found for user ID: {userId}");
                return new List<ChatRoomModel.GroupChatObject>();
            }
            return chatRooms;
        }


        public List<ChatRoomModel.GroupChatObject> handleFetchChatRoomByID(string chatRoom)
        {


            if (string.IsNullOrEmpty(chatRoom))
            {
                Console.WriteLine("Chat room cannot be empty");
                return new List<ChatRoomModel.GroupChatObject>();
            }

            var result = chatRoomModel.fetchRoomId(chatRoom);


            return result ?? new List<ChatRoomModel.GroupChatObject>();
        }


        public bool handleInsertMessageToChatRoom(string senderUsername, int roomId, string messageContent)
        {
            if (string.IsNullOrEmpty(senderUsername) || roomId <= 0 || string.IsNullOrEmpty(messageContent))
            {
                Console.WriteLine("Sender username, room ID, and message content cannot be empty or invalid.");
                return false;
            }
            if (chatRoomModel.insertGroupMessage(senderUsername, roomId, messageContent))
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to insert message from '{senderUsername}' to chat room ID: {roomId}");
                return false;
            }
        }
    }
}
