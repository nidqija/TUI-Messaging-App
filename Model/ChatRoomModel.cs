using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.Model
{
    internal class ChatRoomModel
    {
        private DatabaseService databaseService = new DatabaseService();
        SessionInitializer sessionInitializer;
        


        public class GroupChatObject
        {
            public int Userid { get; set; }
            public string GroupName { get; set; }
        }
        


        public bool insertNewGroup(string roomName , int userId )
        {

            if (roomName == null || userId == null)
            {
                Console.WriteLine("Group name and user ID cannot be null.");
                return false;
            }

            string sql = $"INSERT INTO  chat_rooms (room_name , user_id,  timestamp) VALUES ( '{roomName}' , '{userId}' , '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";

            databaseService.performSQLOperation(sql);

            if (databaseService == null)
            {
                Console.WriteLine("Database service is not initialized.");
                return false;
            }

  
            return true;


        }
    }
}
