using System;
using Spectre.Console;

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

            // Menu selection
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an action:")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .UseConverter(name => $" [cyan]❯[/] {name}") 
                    .AddChoices(new[] {
                        "View Groups",
                        "Create Group",
                        "View Messages",
                        "[red]Logout[/]"
                    }));

            // Transition Logic
            string destination = "home";
            string statusMessage = "Loading...";

            switch (choice)
            {
                case "View Groups":
                    destination = "view groups";
                    statusMessage = "Opening groups...";
                    break;
                case "Create Group":
                    destination = "create group";
                    statusMessage = "Preparing group setup...";
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
