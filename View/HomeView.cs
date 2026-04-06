    using System;
using Spectre.Console;
using TUI_Messaging_App.TUI_Messaging_App.Controller;
using TUI_Messaging_App.TUI_Messaging_App.Model;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class HomeView
    {


        public string displayHomeView()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new FigletText("MESSENGER IN TERMINAL")
                    .Centered()
                    .Color(Color.Aqua));

            var rule = new Rule("[white]Main Menu[/]");
            rule.LeftJustified();
            rule.Style = "grey30";
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();

            if (SessionInitializer.isLoggedIn)
            {
                AnsiConsole.MarkupLine($"[green]Welcome back, {SessionInitializer.Username}![/]");
            }

            // Menu selection
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an action:")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .UseConverter(name => $" [cyan]❯[/] {name}") 
                    .AddChoices(new[] {
                        "Search Users",
                        "View Contacts",
                        "View Groups",
                        "Create Group",
                        "View Messages",
                        "View User Requests",
                        "[red]Logout[/]"
                    }));

            // Transition Logic
            string destination = "home";
            string statusMessage = "Loading...";

            switch (choice)
            {

                case "Search Users":
                    destination = "search users";
                    statusMessage = "Searching for users...";
                    break;

                case "View Contacts":
                    destination = "view contacts";
                    statusMessage = "Loading your contacts...";
                    break;

                case "View Groups":
                    destination = "view groups";
                    statusMessage = "Opening groups...";
                    break;

                case "View User Requests":
                    destination = "view requests";
                    statusMessage = "Checking your requests...";
                    break;

                case "Create Group":
                    destination = "create group chat";
                    statusMessage = "Opening your request...";
                    break;

                case "View Messages":
                    destination = "view messages";
                    statusMessage = "Fetching your messages...";
                    break;
                case "[red]Logout[/]":
                    destination = "logout";
                    statusMessage = "Logging out securely...";
                   
                    break;
            }

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .Start($"[yellow]{statusMessage}[/]", ctx =>
                {
                    System.Threading.Thread.Sleep(700);
                });

            return destination;
        }
    }
}
