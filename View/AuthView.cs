using System;
using Spectre.Console;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class AuthView
    {
        private string[] options = new string[] { "Sign In", "Sign Up", "Quit" };

        public string authView()
        {
            AnsiConsole.Clear();

            // 1. Header area
            var header = new Grid();
            header.AddColumn();
            header.AddRow(new FigletText("MESSENGER IN TERMINAL").Centered().Color(Color.Green));
            header.AddRow(new Rule("Authentication").LeftJustified().RuleStyle("white"));
            AnsiConsole.Write(header);

            AnsiConsole.WriteLine();

            // 2. Selection Prompt
            var prompt = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold white]Please select an option to continue:[/]")
                .PageSize(5)
                .UseConverter(name => $" [green]❯[/] {name}") // Consistent icon usage
                .HighlightStyle(new Style(foreground: Color.Green, decoration: Decoration.Bold))
                .AddChoices(options));

            AnsiConsole.WriteLine();

            // 3. Footer
            AnsiConsole.Write(new Rule().RuleStyle("grey30"));
            AnsiConsole.MarkupLine("[grey]Status: [red]Offline[/] | Server: Localhost[/]");

            // 4. Transition Logic
            string destination = "home";
            string statusMessage = "Processing...";

            switch (prompt)
            {
                case "Sign In":
                    destination = "signin";
                    statusMessage = "Opening login portal...";
                    break;
                case "Sign Up":
                    destination = "signup";
                    statusMessage = "Loading registration form...";
                    break;
                case "Quit":
                    destination = "exit";
                    statusMessage = "Shutting down...";
                    break;
            }

            // The Spinner Transition
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .Start($"[green]{statusMessage}[/]", ctx =>
                {
                    System.Threading.Thread.Sleep(700);
                });

            return destination;
        }
    }
}
