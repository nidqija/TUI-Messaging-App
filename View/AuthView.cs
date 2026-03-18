using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class AuthView
    {
        public string authView()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold red]You have been logged out.[/]");
            AnsiConsole.Write(
                new FigletText("Authentication" + "\n")
                    .Centered()
                    .Color(Color.Green));


            AnsiConsole.MarkupLine("\nPress [blue]any key[/] to go to the Login screen...");
            Console.ReadKey(true);


            return "home";

        }
    }
}
