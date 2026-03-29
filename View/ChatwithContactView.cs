using Spectre.Console;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUI_Messaging_App.TUI_Messaging_App.Controller;
using TUI_Messaging_App.TUI_Messaging_App.Model;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class ChatwithContactView
    {
        private readonly MessagesController messagesController = new MessagesController();
        private bool _needRefresh = false;
        private static ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");


        public string chatwithContactView()
        {
            string contact = SessionInitializer.ActiveChatUser;
            string currentUser = SessionInitializer.Username;
            StringBuilder inputBuffer = new StringBuilder();
            _needRefresh = true; // 1. start with full render

            // 2. subscribe to redis channel for this user to get notified of new messages
            var sub = connectionMultiplexer.GetSubscriber();
            string channelName = $"messages:{currentUser}";

            // whenever a new message is created , messagecontroller will create an event  
            // this event will trigger a redis publish to the channel "messages:currentUser"
            sub.Subscribe(channelName, (channel, message) =>
            {
                if (message.ToString() == "REFRESH_CHAT")
                {
                    _needRefresh = true;
                }
            });

            while (true)
            {
                // only re render the chat if the event triggered
                if (_needRefresh)
                {
                    RenderFullChat(currentUser, contact, inputBuffer.ToString());
                    _needRefresh = false;
                }

                // 3. Non-blocking input check
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        string content = inputBuffer.ToString().Trim();
                        if (content.ToLower() == ":q") break;

                        if (!string.IsNullOrEmpty(content))
                        {
                            // This triggers the SQL Save + Redis Publish
                            messagesController.insertMessage(currentUser, contact, content);
                            inputBuffer.Clear();
                            _needRefresh = true;
                        }
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

                Thread.Sleep(50); // Prevent CPU spiking
            }

            // unsubscribe from the channel when the chat is exited to clean up resources
            sub.Unsubscribe(channelName);
            return "view contacts";
        }

        // Helper method to keep the main loop clean
        // this method is responsible for rendering the entire chat history and the current input buffer from database
        private void RenderFullChat(string currentUser, string contact, string currentInput)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule($"[bold yellow]Chat: {contact}[/]").LeftJustified());

            var messages = messagesController.getMessagesBetweenUsers(currentUser, contact) ?? new List<MessagesModal>();
            var grid = new Grid().Expand().AddColumn().AddColumn();

            foreach (var message in messages)
            {
                bool isMe = message.SenderUsername == currentUser;
                var panel = new Panel(message.MessageContent ?? "")
                    .RoundedBorder()
                    .Header($"[grey]{message.Timestamp:HH:mm}[/]", isMe ? Justify.Right : Justify.Left)
                    .BorderColor(isMe ? Color.Blue : Color.Green);

                if (isMe) grid.AddRow(Text.Empty, panel);
                else grid.AddRow(panel, Text.Empty);
            }

            AnsiConsole.Write(grid);
            AnsiConsole.Write(new Rule().RuleStyle("grey30"));
            AnsiConsole.Markup($"[bold blue]You:[/] {currentInput}");
        }
    }
}