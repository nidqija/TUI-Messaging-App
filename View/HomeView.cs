using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class HomeView
    {
        public void displayHomeView()
        {
            AnsiConsole.MarkupLine("[bold green]Welcome to the TUI Messaging App![/]");
        }
    }
}
