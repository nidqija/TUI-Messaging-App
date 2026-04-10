using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Spectre.Console;
using TUI_Messaging_App.TUI_Messaging_App.Interface;
using TUI_Messaging_App.TUI_Messaging_App.Router;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class ChatInGroupView
    {
        private bool _needRefresh = false;

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
                    RenderGroupInterface(groupName, currentUser, inputBuffer.ToString());
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

        private void RenderGroupInterface(string groupName, string currentUser, string currentInput)
        {
            AnsiConsole.Clear();

            // --- 1. HEADER ---
            AnsiConsole.Write(new Rule($"[bold cyan]Group: {groupName}[/]").LeftJustified());

            // --- 2. MOCKED MESSAGE DATA ---
            // This mimics the structure of your models for the UI render
            var mockMessages = new[]
            {
                new { Sender = "Alice", Content = "Hey everyone! Status update?", Time = "10:05", IsAI = false },
                new { Sender = "ollama", Content = "I am ready to assist with code reviews.", Time = "10:06", IsAI = true },
                new { Sender = "AdminUser", Content = "I'm pushing the latest TUI changes now.", Time = "10:10", IsAI = false }
            };

            var grid = new Grid().Expand().AddColumn().AddColumn();

            foreach (var msg in mockMessages)
            {
                bool isMe = msg.Sender == currentUser;
                string senderDisplay = isMe ? "You" : msg.Sender;

                var panel = new Panel(Markup.Escape(msg.Content))
                    .RoundedBorder()
                    .Header($"[grey]{senderDisplay} • {msg.Time}[/]", isMe ? Justify.Right : Justify.Left)
                    .BorderColor(isMe ? Color.SlateBlue1 : (msg.IsAI ? Color.Gold1 : Color.Grey));

                // Right-align "My" messages, Left-align "Others"
                if (isMe)
                    grid.AddRow(Text.Empty, panel);
                else
                    grid.AddRow(panel, Text.Empty);
            }

            AnsiConsole.Write(grid);

            // --- 3. FOOTER / INPUT AREA ---
            AnsiConsole.Write(new Rule().RuleStyle("grey30"));

            // Display "You:" and then the current buffer (what the user is typing)
            AnsiConsole.Markup($"[bold cyan]Message {groupName}:[/] {currentInput}");

            // This cursor visual helps the user feel like they are typing in a real app
            if (DateTime.Now.Millisecond < 500)
                AnsiConsole.Markup("[blink]_[/]");
        }
    }
}