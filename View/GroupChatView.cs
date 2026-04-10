using Spectre.Console;
using System;
using TUI_Messaging_App.TUI_Messaging_App.Controller;
using TUI_Messaging_App.TUI_Messaging_App.Model;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class GroupChatView
    {
        public string groupChatView()
        {
            ChatRoomController chatRoomController = new ChatRoomController();

            List<ChatRoomModel.GroupChatObject> requests = chatRoomController.handleFetchChatRooms(SessionInitializer.UserID);


            AnsiConsole.Clear();

            var header = new FigletText("GROUP CHATS")
                .Centered()
                .Color(Color.Aqua);

            AnsiConsole.Write(header);

            if (!SessionInitializer.isLoggedIn)
            {
                AnsiConsole.Write(new Panel("[red]Access Denied:[/] You must be logged in to see contacts.")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Red));
                AnsiConsole.MarkupLine("\n[grey]Press any key to return to Sign In...[/]");
                Console.ReadKey(true);
                return "signin";
            }

            if (requests.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]You have no active group chats. Create or join a group to start chatting![/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey](Press any key to return to home)[/]");
                Console.ReadKey();
                return "home";
            }   

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Grey30)
                .AddColumn(new TableColumn("[bold cyan]Group Name[/]").Centered())
                .AddColumn(new TableColumn("[bold cyan]Members[/]").Centered());

            var contactChoices = requests.Select(r => r.GroupName).ToList();
            contactChoices.Add("[red]Back to Home[/]");




            AnsiConsole.Write(new Rule("[white]💬 Active Conversations[/]").LeftJustified().RuleStyle("grey30"));
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a group or go back:")
                    .AddChoices(contactChoices));

            

            if (choice == "[red]Back to Home[/]")
            {
               
                AnsiConsole.Status()
                    .Start("Returning to home...", ctx => {
                        System.Threading.Thread.Sleep(600);
                    });
                return "home";
            }

            return "chat in group";
        }
    }
}
