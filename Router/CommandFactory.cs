using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Interface;

namespace TUI_Messaging_App.TUI_Messaging_App.Router
{
    internal class CommandFactory
    {
        public static IGroupCommand GetCommand(string commandType)
        {
            switch (commandType.ToLower())
            {
                case "add":
                    return "add";
            }
        }
    }
}
