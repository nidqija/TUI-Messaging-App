using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Interface;

namespace TUI_Messaging_App.TUI_Messaging_App.Services.CommandCollection
{
    internal class AddMember : IGroupCommand
    {
        public void Execute(string groupId, string command)
        {
            // command format : "add memberId"
            var parts = command.Split(' ', 2);
            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid command format. Use: add memberId");
                return;
            }
            string memberId = parts[1].Trim();
            Console.WriteLine($"Member '{memberId}' added to group '{groupId}'.");
        }
    }
}
