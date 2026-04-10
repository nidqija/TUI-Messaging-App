using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.Model
{
    internal class GroupAdminModel
    {

        DatabaseService databaseService;

        public bool insertNewGroupMember( int groupId, int memberId)
        {
            //to be implemented later

            String sql = $"INSERT INTO group_members (room_id, user_id) VALUES ({groupId}, {memberId})";
            databaseService.performSQLOperation(sql);


            if (databaseService == null)
            {
                Console.WriteLine("Error on the database service.");
                return false;
            }
            else
            {
                Console.WriteLine($"Group member {memberId} added to group {groupId} successfully.");
                return true;
            }



                
        }
    }
}
