using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using TUI_Messaging_App.TUI_Messaging_App.Router;


namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class HomeView
    {
        public string displayHomeView()
        {
            // clear previous content to go to a new page
            AnsiConsole.Clear();

            // make a big title using FigletText
            AnsiConsole.Write(
                new FigletText("MESSENGER IN TERMINAL")
                    .Centered()
                    .Color(Color.Aqua));

            // add a rule for better positioning , and make it left justified for better UX
            var rule = new Rule("[white]Main Menu[/]");
            rule.LeftJustified();
            rule.Style = "grey30";
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();

            // make a choice prompt for the user to select an option
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an action:")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] {
                "View Groups",
                "Create Group",
                "View Messages",
                "[red]Logout[/]"
                    }));


            if (choice == "View Groups")
            {

                return "view groups";
            }
            else if (choice == "Create Group")
            {

                return "create group";
            }
            else if (choice == "View Messages")
            {

                return "view messages";
            }
            else if (choice == "[red]Logout[/]")
            {

                return "logout";
            }
            else
            {

                return "home";

            }
        }
    }
}
