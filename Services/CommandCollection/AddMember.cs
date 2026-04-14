using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Controller;
using TUI_Messaging_App.TUI_Messaging_App.Interface;
using TUI_Messaging_App.TUI_Messaging_App.Model;

namespace TUI_Messaging_App.TUI_Messaging_App.Services.CommandCollection
{
    internal class AddMember : IGroupCommand
    {
        private GroupAdminController groupAdminController = new GroupAdminController();
        private GroupAdminModel groupAdminModel = new GroupAdminModel();
        public void Execute(string groupId, string command)
        {
           
            string memberName = command.Trim();

            if (string.IsNullOrWhiteSpace(memberName))
            {
                Console.WriteLine("Error: Please provide a username. Usage: /admin username");
                return;
            }

            var user = groupAdminModel.fetchUserforGroup(memberName).FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine($"Error: User '{memberName}' not found in the database.");
                return; 
            }

           
            if (int.TryParse(groupId, out int gId))
            {
                bool success = groupAdminController.handleAddMembertoGroup(gId, user.Id);

                if (success)
                {
                    AnsiConsole.MarkupLine("[bold green]✔ User added successfully![/]");
                    Thread.Sleep(1500); // Pause for 1.5 seconds so the user can read it
                }
            }
            else
            {
                Console.WriteLine("Error: Group ID must be a numeric value.");
            }
        }
    }
}
