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

        public class GroupMemberObject
        {
            public int Id { get; set; }
            public string Username { get; set; }
        }



        DatabaseService databaseService = new DatabaseService();

        public bool insertNewGroupMember(int groupId, int memberId)
        {

            string sql = "INSERT INTO room_members (room_id, user_id) VALUES (@roomId, @userId)";

            var parameters = new Dictionary<string, object>
    {
        { "@roomId", groupId },
        { "@userId", memberId }
    };

            try
            {
                databaseService.performSQLOperationWithParameters(sql, parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                return false;
            }
        }


        // helper method to fetch user details based on username, used to get the user id for inserting into room_members table
        public List<GroupMemberObject> fetchUserforGroup(string memberName)
        {
            string sql = $"SELECT id AS Id, username AS Username FROM users WHERE username = '{memberName}'";
            return databaseService.GetList<GroupMemberObject>(sql).ToList();
        }


        public bool deleteGroupMemberfromRoom(int groupId, int memberId)
        {
            string sql = "DELETE FROM room_members WHERE room_id = @roomId AND user_id = @userId";


            var parameters = new Dictionary<string, object>
            {
                { "@roomId", groupId },
                { "@userId", memberId }
            };

            try
            {
                databaseService.performSQLOperationWithParameters(sql, parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                return false;


            }

        }

    }
    }


