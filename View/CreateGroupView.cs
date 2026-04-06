using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class CreateGroupView
    {
        public string createGroupView()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
               new FigletText("CREATE GROUP CHAT")
                   .Centered()
                   .Color(Color.Aqua));

            var rule = new Rule("[white]Main Menu[/]");
            rule.LeftJustified();
            rule.Style = "grey30";
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();

            // --- STEP 2: INPUT BOX (The 'Room Name') ---
            // Concept: KISS (Keep it Simple - one clear question)
            var roomName = AnsiConsole.Prompt(
                new TextPrompt<string>("What is the [olive]Name[/] of this group?")
                    .PromptStyle("cyan")
                    .Validate(name =>
                        name.Length >= 3 ? ValidationResult.Success() : ValidationResult.Error("[red]Name too short (min 3 chars)[/]"))
            );

            // --- STEP 3: SELECTION LIST (The 'Members') ---
            // Imagine these are fetched from your DatabaseService
            var dummyUsers = new List<string> { "alice", "bob", "charlie", "delta_v", "echo_user" };

            var selected = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Who do you want to [green]Invite[/]?")
                    .NotRequired()
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more users)[/]")
                    .InstructionsText("[grey](Press [blue]<space>[/] to select, [green]<enter>[/] to finish)[/]")
                    .AddChoices(dummyUsers)
            );

            // --- STEP 4: SUMMARY PANEL ---
            // Concept: Refinement (Elaborating on what is about to happen)
            AnsiConsole.WriteLine();
            var summaryTable = new Table().Border(TableBorder.Rounded);
            summaryTable.AddColumn("[grey]Field[/]");
            summaryTable.AddColumn("[grey]Value[/]");
            summaryTable.AddRow("Group Name", $"[bold cyan]{roomName}[/]");
            summaryTable.AddRow("Members", $"[bold green]{selected.Count}[/] users selected");

            AnsiConsole.Write(
                new Panel(summaryTable)
                    .Header("[yellow] Review Details [/]")
                    .Expand()
            );

            // --- STEP 5: FINAL CONFIRMATION ---
            if (AnsiConsole.Confirm("Do you want to create this group now?"))
            {
                AnsiConsole.MarkupLine($"[bold green]Success![/] Room [white]{roomName}[/] is being provisioned...");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Creation cancelled.[/] Returning to menu...");
            }

            AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
            Console.ReadKey(true);

            return "create group chat";
        }
    }
}
