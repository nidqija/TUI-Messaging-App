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
    internal class DeleteMember : IGroupCommand
    {
        private GroupAdminController groupAdminController = new GroupAdminController();
        private GroupAdminModel groupAdminModel = new GroupAdminModel();
        public void Execute(string groupId, string command)
        {

            string memberName = command.Trim();

            if (string.IsNullOrWhiteSpace(memberName))
            {
                Console.WriteLine("Error: Please provide a username. Usage: /terminate username");
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
                bool success = groupAdminController.handleRemoveMemberfromGroup(gId, user.Id);

                if (success)
                {
                    AnsiConsole.MarkupLine("[bold red]✔ User deleted successfully![/]");
                    Thread.Sleep(1500); 
                }
            }
            else
            {
                Console.WriteLine("Error: Group ID must be a numeric value.");
            }
        }
    }
}
