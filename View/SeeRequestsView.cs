using System;
using System.Collections.Generic;
using System.Linq;
using TUI_Messaging_App.TUI_Messaging_App.Controller;
using TUI_Messaging_App.TUI_Messaging_App.Services;
using Spectre.Console;
using TUI_Messaging_App.TUI_Messaging_App.Model;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class SeeRequestsView
    {
        public string seeRequestsView()
        {
            // 1. Session Check
            if (!SessionInitializer.isLoggedIn)
            {
                AnsiConsole.Write(new Panel("[red]Access Denied:[/] You must be logged in to see friend requests.").Border(BoxBorder.Rounded));
                Console.ReadKey();
                return "signin";
            }

            Console.Clear();

            // 2. Fetch Data
            MessageRequestsController messageRequestController = new MessageRequestsController();
            List<MessageRequestsModel.MessageRequestObject> requests = messageRequestController.handleFetchMessageRequests(SessionInitializer.Username);

            // 3. UI Header
            AnsiConsole.Write(
                new FigletText("Requests")
                    .LeftJustified()
                    .Color(Color.Cyan1));

            AnsiConsole.MarkupLine($"[grey]Logged in as:[/] [bold green]{SessionInitializer.Username}[/]\n");

            // 4. Display Logic
            if (requests == null || requests.Count == 0)
            {
                AnsiConsole.Write(new Panel("You have [yellow]no pending[/] friend requests.").Border(BoxBorder.Rounded).Padding(1, 1, 1, 1));
            }
            else
            {
                // Create a beautiful table
                var table = new Table();
                table.Border(TableBorder.Rounded);
                table.AddColumn("[bold cyan]Sender Username[/]");
                table.AddColumn("[bold cyan]Message[/]");
                table.AddColumn("[bold cyan]Status[/]");

                foreach (var request in requests)
                {
                    table.AddRow(request.Username , request.Message ?? "[grey]No Message[/]" , "[yellow]Pending[/]");
                }

                AnsiConsole.Write(table);
                AnsiConsole.MarkupLine($"\n[bold white]Total:[/] {requests.Count} request(s)");

                var choice = AnsiConsole.Prompt(
                    
                    new SelectionPrompt<string>()
                        .Title("\n[bold green]What would you like to do?[/]")
                        .AddChoices(new[] { "Accept Request", "Go Back" }));

                if (choice == "Accept Request")
                {
                    var senderToAccept = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[bold green]Select a request to accept:[/]")
                            .AddChoices(requests.Select(r => r.Username).ToArray()));

                    UserController userController = new UserController();
                    if (userController.handleAcceptChatRequest(senderToAccept))
                    {
                        System.Threading.Thread.Sleep(2000); // Pause for 2 seconds to show the success message
                        return "home"; // Redirect back to home after accepting
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[red]Failed to accept the request from {senderToAccept}. Please try again.[/]");
                        return "view requests"; // Stay on the same page to try again
                    }

                }
            }

            // 5. Footer
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]Press any key to return to the home view...[/]");
            Console.ReadKey(true);
            return "home";
        }
    }
}