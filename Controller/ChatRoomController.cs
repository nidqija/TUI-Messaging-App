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

        public bool handleCreateChatRoom(int userId , string roomName)
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
    }
}
