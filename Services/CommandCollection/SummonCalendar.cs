using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Interface;

namespace TUI_Messaging_App.TUI_Messaging_App.Services.CommandCollection
{
    internal class SummonCalendar : IGroupCommand
    {

        private readonly ZeroClawService _aiService;

        public SummonCalendar()
        {
            _aiService = new ZeroClawService();
        }
        public void Execute(string groupId, string command)
        {

            Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("Summoning calendar for group " + groupId);

                    string prompt = $"You are a Google Calendar assistant. The user wants to: {command}. " +
                                    "If they are asking to schedule something, extract the date, time, and title.";


                    var aiResponse = await _aiService.GetAIResponse(prompt);

                    Console.WriteLine("AI Response: " + aiResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while summoning calendar: " + ex.Message);
                }
            });
            
        }
    }
}
