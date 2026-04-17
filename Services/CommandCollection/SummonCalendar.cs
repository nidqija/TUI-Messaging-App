using Google.Apis.Calendar.v3;
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
        private readonly GoogleCalendar _googleCalendar;

        public SummonCalendar()
        {
            _aiService = new ZeroClawService();
            _googleCalendar = new GoogleCalendar();
        }
        public void Execute(string groupId, string command)
        {

            Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("Summoning calendar for group " + groupId);

                    string prompt = $@"Today is {DateTime.Now:yyyy-MM-dd HH:mm}. 
                        Extract event details from this input: ""{command}""
                        Output ONLY this JSON, no other text:
                        {{""title"": ""string"", ""start"": ""YYYY-MM-DDTHH:mm:ss"", ""end"": ""YYYY-MM-DDTHH:mm:ss""}}";


                    string aiRawResponse = await _aiService.GetAIResponse(prompt);
                    Console.WriteLine("DEBUG RAW: " + aiRawResponse);

                    if (string.IsNullOrWhiteSpace(aiRawResponse) || aiRawResponse.StartsWith("[Error]") || aiRawResponse.StartsWith("[Ollama Error]"))
                    {
                        Console.WriteLine("[AI Error] AI service returned an error or empty response: " + aiRawResponse);
                        return;
                    }

                    int startIndex = aiRawResponse.IndexOf('{');
                    int endIndex = aiRawResponse.LastIndexOf('}');

                    if (startIndex != -1 && endIndex != -1)
                    {
                        string cleanJson = aiRawResponse.Substring(startIndex, (endIndex - startIndex) + 1);

                        var eventDetails = System.Text.Json.JsonSerializer.Deserialize<CalendarEvent>(
                            cleanJson,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );

                        if (eventDetails != null)
                        {
                            await _googleCalendar.AddGoogleEventAsync(groupId, eventDetails);
                            Console.WriteLine($"[Success] '{eventDetails.title}' added to {groupId}'s calendar!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[AI Error] Could not find JSON block in response: " + aiRawResponse);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while summoning calendar: " + ex.Message);
                }
            });
            
        }
    }
}
