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
                    RenderFullChat(SessionInitializer.groupChatID);
                    _needRefresh = false;
                }

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        string content = inputBuffer.ToString().Trim();
                        if (content.ToLower() == ":q") break; // Exit command


                      if (content.StartsWith("/"))
                        {
                            var command = CommandFactory.ParseComment(content);

                            if ( command != null)
                            {
                                string argument = content.Split(' ').Last();
                            }


                        }




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

        private void RenderFullChat(int roomId)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[bold magenta]DEBUG: Session Room ID is {SessionInitializer.groupChatID}[/]");
            AnsiConsole.MarkupLine($"[bold cyan]Group: {SessionInitializer.ActiveGroupChatName}[/]");
            // 1. Fetch regular messages between you and the contact
            var messages = messagesController.handleFetchMessagesfromRoom(roomId) ?? new List<MessagesModal>();

            if (messages.Count == 0)
            {
                AnsiConsole.MarkupLine("[grey]No messages yet. Start the conversation![/]");
                return;
            }



            var grid = new Grid().Expand().AddColumn().AddColumn();

            foreach (var message in messages)
            {
             

                var panel = new Panel(Markup.Escape(message.MessageContent ?? ""))
                    .RoundedBorder()
                    .Header($"[bold cyan]{message.SenderUsername}[/] [grey]{message.Timestamp:HH:mm}[/]");



            }

            AnsiConsole.Write(grid);
            AnsiConsole.Write(new Rule().RuleStyle("grey30"));
        }
    }
}
