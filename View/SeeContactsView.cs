using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using TUI_Messaging_App.TUI_Messaging_App.Controller;
using TUI_Messaging_App.TUI_Messaging_App.Model;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class SeeContactsView
    {
        MessageRequestsController messageRequestController = new MessageRequestsController();

        public string seeContactView()
        {
            AnsiConsole.Clear();

            // --- Header ---
            AnsiConsole.Write(new Rule("[yellow]Your Contacts[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.WriteLine();

            if (!SessionInitializer.isLoggedIn)
            {
                AnsiConsole.Write(new Panel("[red]Access Denied:[/] You must be logged in to see contacts.")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Red));
                AnsiConsole.MarkupLine("\n[grey]Press any key to return to Sign In...[/]");
                Console.ReadKey(true);
                return "signin";
            }

            List<MessageRequestsModel.MessageRequestObject> requests = messageRequestController.handleFetchApprovedContacts(SessionInitializer.Username);

            if (requests == null || requests.Count == 0)
            {
                AnsiConsole.Write(new Panel("[yellow]No contacts found yet. Try adding some friends![/]")
                    .Border(BoxBorder.Rounded)
                    .Header("[bold]Information[/]"));
                AnsiConsole.MarkupLine("\n[grey]Press any key to return home...[/]");
                Console.ReadKey(true);
                return "home";
            }
            else
            {
                // Create a table for a cleaner layout
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Blue)
                    .AddColumn(new TableColumn("[u]Username[/]").Centered())
                    .AddColumn(new TableColumn("[u]Status[/]").Centered());

                foreach (var request in requests)
                {
                    table.AddRow($"[green]{request.Username}[/]", "[italic grey]Connected[/]");
                }

                AnsiConsole.Write(table);
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule().RuleStyle("grey"));

            // Using a Selection Prompt makes the UI feel more like an app than a terminal script
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(new[] { "Back to Home", "View Message History" }));

            return choice == "Back to Home" ? "home" : "view contacts";
        }
    }
}
