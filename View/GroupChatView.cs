using Spectre.Console;
using System;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class GroupChatView
    {
        public string groupChatView()
        {
            AnsiConsole.Clear();

            var header = new FigletText("GROUP CHATS")
                .Centered()
                .Color(Color.Aqua);

            AnsiConsole.Write(header);

            var content = new SelectionPrompt<string>()
                .Title("[bold yellow]Select a group to enter the conversation:[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more groups)[/]")
                .UseConverter(name => $" [blue]➜[/] {name}") // Adds a custom pointer icon
                .AddChoices(new[] {
                    "Group 1",
                    "Group 2",
                    "Group 3",
                    "General Chat",
                    "Development Team",
                    "[red]Back to Home[/]"
                });

            AnsiConsole.Write(new Rule("[white]💬 Active Conversations[/]").LeftJustified().RuleStyle("grey30"));
            AnsiConsole.WriteLine();

           
            var choice = AnsiConsole.Prompt(content);

           
            if (choice == "[red]Back to Home[/]")
            {
               
                AnsiConsole.Status()
                    .Start("Returning to home...", ctx => {
                        System.Threading.Thread.Sleep(600);
                    });
                return "home";
            }

            return "view groups";
        }
    }
}
