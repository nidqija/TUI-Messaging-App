        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;
        using TUI_Messaging_App.TUI_Messaging_App.Services;

        namespace TUI_Messaging_App.TUI_Messaging_App.Model
        {
    internal class MessageRequestsModel
    {
        private DatabaseService dbService;


        public class MessageRequestObject
        {
            public string Username { get; set; }
            public string Message { get; set; }
        }


        // this method will fetch all the message requests for a given user and return it as a list
        // of strings

        public List<MessageRequestObject> fetchMessageRequests(string currentUsername)
        {
            dbService = new DatabaseService();
            string sql = @"SELECT u.username AS Username , r.message AS Message FROM requests r JOIN users u ON r.sender_id = u.id WHERE 
                               r.receiver_id = (SELECT id FROM users WHERE username = @username) AND r.status = 'pending'";

            var results = dbService.GetList<MessageRequestObject>(sql, new { username = currentUsername });

            return results.ToList();

        }


        public List<MessageRequestObject> fetchAcceptedChatRequests(string currentUsername)
        {
            dbService = new DatabaseService();
            string sql = @"SELECT u.username AS Username , r.message AS Message FROM requests r JOIN users u ON r.sender_id = u.id WHERE 
                               r.receiver_id = (SELECT id FROM users WHERE username = @username) AND r.status = 'accepted'

                          UNION
        
                        SELECT u.username AS Username, r.message AS Message 
                        FROM requests r 
                        JOIN users u ON r.receiver_id = u.id 
                        WHERE r.sender_id = (SELECT id FROM users WHERE username = @username) 
                            AND r.status = 'accepted'
                       ";

            var results = dbService.GetList<MessageRequestObject>(sql, new { username = currentUsername });


            return results.ToList();



        }
    }
        }
