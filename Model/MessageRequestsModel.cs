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


               // this method will fetch all the message requests for a given user and return it as a list
               // of strings
               
               public List<string> fetchMessageRequests(string currentUsername)
              {
                  dbService = new DatabaseService();
                  string sql = @"SELECT u.username FROM requests r JOIN users u ON r.sender_id = u.id WHERE 
                               r.receiver_id = (SELECT id FROM users WHERE username = @username) AND r.status = 'pending'";
                   
                  var results = dbService.GetList<string>(sql, new { username = currentUsername });

                  return results.ToList();

                 }

        
                    



            }
        }
