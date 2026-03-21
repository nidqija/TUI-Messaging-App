using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using TUI_Messaging_App.TUI_Messaging_App.Controller;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class SearchUserView
    {
        public string searchUserView()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("SEARCH USER")
                    .Centered()
                    .Color(Color.Green));

            var rule = new Rule("[white]Enter username to find a contact[/]");
            rule.Style = "grey30";
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();


            string username = AnsiConsole.Ask<string>("Username:");


            if (string.IsNullOrWhiteSpace(username))
            {
                AnsiConsole.MarkupLine("[red]Please enter a valid username.[/]");
                System.Threading.Thread.Sleep(1000);
                return "search users";
            }

            UserController userController = new UserController();

            if (userController.handleSearchUser(username))
            {
                AnsiConsole.MarkupLine($"\n[green]User '{username}' found![/]");
                AnsiConsole.WriteLine();

                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"\nActions for [bold yellow]{username}[/]:")
                        .AddChoices(new[] { "Send Chat Request", "Search Another User", "Back to Home" }));

                 if (action == "Back to Home")
                {
                    return "home";
                }

                return "search users";
            }
            else
            {
                AnsiConsole.MarkupLine($"\n[red]User '{username}' not found.[/]");
                AnsiConsole.WriteLine("\n[grey]Press any key to try again...[/]");
                Console.ReadKey(true);
                return "search users";
            }
        }
    }
}
