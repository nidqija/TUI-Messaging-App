using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Model;

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
    }
}
