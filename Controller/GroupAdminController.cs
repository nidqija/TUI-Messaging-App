using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Model;

namespace TUI_Messaging_App.TUI_Messaging_App.Controller
{
    internal class GroupAdminController
    {
        private GroupAdminModel groupAdminModel = new GroupAdminModel();
        public bool handleAddMembertoGroup(int groupName , int memberUsername)
        {
            
            if ( groupName <= 0 || memberUsername <= 0)
            {
                Console.WriteLine("Group name and member username cannot be null.");
                return false;
            }

            groupAdminModel.insertNewGroupMember(groupName, memberUsername); 



            return true;
        }
    }
}
