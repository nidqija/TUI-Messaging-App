using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using TUI_Messaging_App.TUI_Messaging_App.Controller;
using TUI_Messaging_App.TUI_Messaging_App.Model;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class ChatwithContactView
    {
        private readonly MessagesController messagesController = new MessagesController();

        public string chatwithContactView()
        {
            string contact = SessionInitializer.ActiveChatUser;
            string currentUser = SessionInitializer.Username;

            if (string.IsNullOrEmpty(contact))
            {
                AnsiConsole.MarkupLine("[red]No active chat user found. Returning to contacts...[/]");
                System.Threading.Thread.Sleep(1000);
                return "view contacts";
            }

            while (true)
            {
                AnsiConsole.Clear();

                // --- Header ---
                AnsiConsole.Write(new Rule($"[bold yellow]Chat: {contact}[/]").RuleStyle("grey").LeftJustified());
                AnsiConsole.WriteLine();

                // 1. Fetch messages safely to avoid NullReferenceException
                var messages = messagesController.getMessagesBetweenUsers(currentUser, contact) ?? new List<MessagesModal>();

                // --- Messaging Grid ---
                var grid = new Grid().Expand().AddColumn().AddColumn();

                if (!messages.Any())
                {
                    grid.AddRow(new Text(""), new Panel("[italic grey]No messages yet. Send a greeting![/]").Border(BoxBorder.None));
                }
                else
                {
                    foreach (var message in messages)
                    {
                        bool isMe = message.SenderUsername == currentUser;
                        string text = message.MessageContent ?? "";

                        // Create the "Bubble"
                        var panel = new Panel(text)
                            .RoundedBorder()
                            .Header($"[grey]{message.Timestamp:HH:mm}[/]", isMe ? Justify.Right : Justify.Left)
                            .BorderColor(isMe ? Color.Blue : Color.Green);

                        if (isMe)
                        {
                            // My message on the right
                            grid.AddRow(Text.Empty, panel);
                        }
                        else
                        {
                            // Contact's message on the left
                            grid.AddRow(panel, Text.Empty);
                        }
                    }
                }

                AnsiConsole.Write(grid);
                AnsiConsole.Write(new Rule().RuleStyle("grey30"));

                // --- Input Section ---
                AnsiConsole.Markup($"[bold blue]You:[/] ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                // Exit logic
                if (input.Equals(":q", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                // --- Send Logic ---
                if (messagesController.insertMessage(currentUser, contact, input))
                {
                    // Brief pause so the clear doesn't feel jarring
                    System.Threading.Thread.Sleep(100);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Error: Message not sent.[/]");
                    System.Threading.Thread.Sleep(1000);
                }
            }

            return "view contacts";
        }
    }
}