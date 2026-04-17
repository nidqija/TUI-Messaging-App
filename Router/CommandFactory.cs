using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Interface;
using TUI_Messaging_App.TUI_Messaging_App.Services.CommandCollection;

namespace TUI_Messaging_App.TUI_Messaging_App.Router
{
    internal class CommandFactory
    {
        public static IGroupCommand ParseComment(string commandType)
        {
            if (commandType.StartsWith("/admit")) return new AddMember();
            if (commandType.StartsWith("/terminate")) return new DeleteMember();
            if (commandType.StartsWith("/list")) return new ViewMember();
            if (commandType.StartsWith("/scheduler")) return new SummonCalendar();



            return null;
        }
    }
}
