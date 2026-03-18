using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class SignInView
    {
        public string signInView()
        {
            // clear previous content to go to a new page
            AnsiConsole.Clear();
            // make a big title using FigletText

            AnsiConsole.Write(
                new FigletText("SIGN IN")
                    .Centered()
                    .Color(Color.Green));

            // add a rule for better positioning , and make it left justified for better UX
            var rule = new Rule("[white]Please enter your credentials[/]");
            rule.LeftJustified();
            rule.Style = "grey30";
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();

            // prompt the user for username and password
            string username = AnsiConsole.Ask<string>("Username:");
            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("Password:")
                    .PromptStyle("red")
                    .Secret());

            // For this example, we'll just check if they are not empty
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                AnsiConsole.MarkupLine("[green]Login successful![/]");
                return "home"; // Navigate to home view after successful login
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid credentials. Please try again.[/]");
                return "signin"; // Stay on the sign-in view if login fails
            }
        }
    }
}
