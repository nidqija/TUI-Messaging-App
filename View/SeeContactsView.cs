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

            // --- Contact Selection ---
            // We create a list of strings for the prompt, adding a "Back" option at the top
            var contactChoices = requests.Select(r => r.Username).ToList();
            contactChoices.Insert(0, "[grey]<- Back to Home[/]");

            var selectedContact = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a [green]contact[/] to message or go back:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more contacts)[/]")
                    .AddChoices(contactChoices));

            

            // Handle the "Back" logic
            if (selectedContact == "[grey]Back to Home[/]")
            {
                return "home";
            }

            SessionInitializer.ActiveChatUser = selectedContact;

            // Logic for when a contact is selected
            // You might want to store the selectedContact in a session variable 
            // or pass it to the next view.
            AnsiConsole.MarkupLine($"\nOpening chat with [bold green]{selectedContact}[/]...");
            System.Threading.Thread.Sleep(500);

            return "chat with contact"; 
        }
    }
}
