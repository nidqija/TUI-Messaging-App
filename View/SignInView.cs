using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using TUI_Messaging_App.TUI_Messaging_App.Controller;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class SignInView
    {
        public string signInView()
        {
            AnsiConsole.Clear();
            UserController userController = new UserController();


            AnsiConsole.Write(
                new FigletText("SIGN IN")
                    .Centered()
                    .Color(Color.Green));

            var rule = new Rule("[white]Please enter your credentials[/]");
            rule.LeftJustified();
            rule.Style = "grey30";
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();

            string username = AnsiConsole.Ask<string>("Username:");
            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("Password:")
                    .PromptStyle("red")
                    .Secret());

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {

                if (userController.handleSignIn(username, password))
                {
                    AnsiConsole.MarkupLine("[green]Login successful![/]");
                    return "home"; 
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid credentials. Please try again.[/]");
                    return "signin"; 
                }


            }
            else if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                AnsiConsole.MarkupLine("[red]Invalid credentials. Please try again.[/]");
                return "signin"; // Stay on the sign-in view if login fails
            }


            return "signin"; // Stay on the sign-in view if login fails







        }
    }
}
