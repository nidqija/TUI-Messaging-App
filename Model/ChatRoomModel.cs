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
        


            public class GroupChatObject
            { 
                public int Id { get; set; }
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


            public List<GroupChatObject> fetchAllChatRoom(int userId)
            {
                string sql = $"SELECT id AS Id,  room_name AS GroupName FROM chat_rooms WHERE user_id = '{userId}'";
            
               
                return databaseService.GetList<GroupChatObject>(sql).ToList();
            }

            public List<GroupChatObject> fetchRoomId(string roomName)
           {

            string sql = $"SELECT id AS Id , room_name AS GroupName from chat_rooms WHERE room_name = `{roomName}`";
            return databaseService.GetList<GroupChatObject>(sql).ToList();
           }

        public bool insertGroupMessage(string senderUsername, int roomId, string messageContent)
        {
            if (string.IsNullOrEmpty(senderUsername) || string.IsNullOrEmpty(messageContent))
            {
                Console.WriteLine("Sender and message content cannot be empty.");
                return false;
            }
            string safeContent = messageContent.Replace("'", "''");
            string sql = $"INSERT INTO room_messages (sender_username, room_id, message, timestamp) " +
                         $"VALUES ('{senderUsername}', '{roomId}', '{safeContent}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";
            databaseService.performSQLOperation(sql);
            if (databaseService == null)
            {
                Console.WriteLine("Database service is not initialized.");
                return false;
            }
            Console.WriteLine($"Message from {senderUsername} to room {roomId}: {messageContent}");
            return true;
        }
    }
    }
