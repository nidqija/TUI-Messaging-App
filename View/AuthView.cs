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
        private string[] options = new string[] { "Login", "Register", "Quit" };

        public string authView()
        {
            AnsiConsole.Clear();

            // 1. Create a centered header area
            var header = new Grid();
            header.AddColumn();
            header.AddRow(new FigletText("MESSENGER IN TERMINAL").Centered().Color(Color.Green));
            header.AddRow(new Rule("Authentication").LeftJustified().RuleStyle("white"));
            AnsiConsole.Write(header);

            AnsiConsole.WriteLine(); // Spacing

            // 2. Build the Selection Prompt with custom styling
            var prompt = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold white]Please select an option to continue:[/]")
                .PageSize(5)
                .HighlightStyle(new Style(foreground: Color.Green, decoration: Decoration.Bold))
                .AddChoices(options));



            

            
            AnsiConsole.WriteLine();

            // 4. Add a clean footer rule
            AnsiConsole.Write(new Rule().RuleStyle("grey30"));
            AnsiConsole.MarkupLine("[grey]Status: [red]Offline[/] | Server: Localhost[/]");


            if ( prompt == "Login")
            {
                return "signin";
            }
            else if (prompt == "Register")
            {
                return "signup";
            }
            else if (prompt == "Quit")
            {
                return "exit";
            }


            // Your original return logic (you can expand this to check the choice later)
            return "home";
        }
    }
}
