using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TUI_Messaging_App.TUI_Messaging_App.Controller;
using TUI_Messaging_App.TUI_Messaging_App.Interface;
using TUI_Messaging_App.TUI_Messaging_App.Model;
using TUI_Messaging_App.TUI_Messaging_App.Router;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class ChatInGroupView
    {
        private bool _needRefresh = false;
        private MessagesController messagesController = new MessagesController();
        private ChatRoomController chatRoomController = new ChatRoomController();

        public string chatInGroupView()
        {
            // Static UI placeholders
            string groupName = "Developers Hub";
            string currentUser = "AdminUser";
            StringBuilder inputBuffer = new StringBuilder();

            _needRefresh = true;

            while (true)
            {
                if (_needRefresh)
                {
                    RenderFullChat(SessionInitializer.groupChatID, inputBuffer.ToString());
                    _needRefresh = false;
                }

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        string content = inputBuffer.ToString().Trim();
                        if (content.ToLower() == ":q") break; // Exit command


                        if (content.StartsWith("/admit"))
                        {

                            string targetUser = content.Replace("/admit" , "").Trim();

                            if (!string.IsNullOrEmpty(targetUser))
                            {
                                var command = CommandFactory.ParseComment("/admit");

                                if (command != null)
                                {
                                    command.Execute(SessionInitializer.groupChatID.ToString(), targetUser);
                                }
                                else
                                {
                                    // If it's null, the Factory doesn't recognize "/admin"
                                    AnsiConsole.MarkupLine("[red]Error: Command '/admit' not recognized by CommandFactory.[/]");
                                    Thread.Sleep(2000);
                                }

                            }

                            inputBuffer.Clear();
                            _needRefresh = true;
                            continue;


                        }

                        if (content.StartsWith("/terminate"))
                        {
                            string targetUser = content.Replace("/terminate", "").Trim();

                            if (!string.IsNullOrEmpty(targetUser))
                            {
                                var command = CommandFactory.ParseComment("/terminate");

                                if (command != null)
                                {
                                    command.Execute(SessionInitializer.groupChatID.ToString(), targetUser);
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("[red]Error: Command '/admin' not recognized by CommandFactory.[/]");
                                    Thread.Sleep(2000);
                                }
                            }

                            inputBuffer.Clear();
                            _needRefresh = true;
                            continue;
                        }

                       

                            chatRoomController.handleInsertMessageToChatRoom(SessionInitializer.Username, SessionInitializer.groupChatID, content);





                        // Logic for "sending" would go here
                        inputBuffer.Clear();
                        _needRefresh = true;
                    }
                    else if (key.Key == ConsoleKey.Backspace && inputBuffer.Length > 0)
                    {
                        inputBuffer.Remove(inputBuffer.Length - 1, 1);
                        _needRefresh = true;
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        inputBuffer.Append(key.KeyChar);
                        _needRefresh = true;
                    }
                }

                Thread.Sleep(50);
            }

            return "chat in group";
        }

        private void RenderFullChat(int roomId, string currentInput)
        {
            AnsiConsole.Clear();

            // Get the name from the session we saved earlier
            string groupName = SessionInitializer.ActiveGroupChatName ?? $"Room {roomId}";
            string currentUser = SessionInitializer.Username;

            AnsiConsole.Write(new Rule($"[bold yellow]Chat: {groupName}[/]").LeftJustified());

            // 1. Fetch messages for the group
            var messages = messagesController.handleFetchMessagesfromRoom(roomId) ?? new List<MessagesModal>();

            // 2. We use a Grid with two columns for the "Left/Right" bubble effect
            var grid = new Grid().Expand().AddColumn().AddColumn();

            foreach (var message in messages)
            {
                // Check if the message is from the logged-in user
                bool isMe = message.SenderUsername == currentUser;

                // You can add logic here for specific users (like a bot or admin)
                bool isAdmin = message.SenderUsername == "Admin";

                var panel = new Panel(Markup.Escape(message.MessageContent ?? ""))
                    .RoundedBorder()
                    // Design match: Timestamp in the header, justified based on sender
                    .Header($"[bold]{message.SenderUsername}[/] [grey]{message.Timestamp:HH:mm}[/]", isMe ? Justify.Right : Justify.Left)
                    // Design match: Blue for 'Me', Green for others (Yellow for AI/Admin if needed)
                    .BorderColor(isMe ? Color.Blue : (isAdmin ? Color.Yellow : Color.Green));

                

                if (isMe)
                {
                    // Message on the right
                    grid.AddRow(Text.Empty, panel);
                }
                else
                {
                    // Message on the left
                    grid.AddRow(panel, Text.Empty);
                }
            }

            AnsiConsole.Write(grid);

            // --- User Input Section ---
            AnsiConsole.Write(new Rule().RuleStyle("grey30"));

            // Using Markup to match your design: "[bold blue]You:[/] currentText"
            AnsiConsole.Markup($"[bold blue]You:[/] {currentInput}");
        }
    }
}
