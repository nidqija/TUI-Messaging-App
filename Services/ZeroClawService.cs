using OllamaSharp.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static TUI_Messaging_App.TUI_Messaging_App.Services.AskOllamaServices;

namespace TUI_Messaging_App.TUI_Messaging_App.Services
{
    internal class ZeroClawService
    {
        private readonly HttpClient _client = new HttpClient();

        public class ZeroClawResponse
        {
            public List<Choice> choices { get; set; }
        }


        public class Choice
        {
            public Message message { get; set; }

        }


        public class Message
        {
            public string content { get; set; }
        }


        public async Task<string> GetAIResponse(string prompt)
        {
            var requestBody = new
            {
                model = "qwen3-vl:8b", // Make sure this matches your 'ollama list'
                messages = new[]
                {
            new { role = "user", content = prompt }
        },
                stream = false // Set to false for a single complete response
            };

            try
            {
                var response = await _client.PostAsJsonAsync("http://localhost:11434/api/chat", requestBody);

                if (!response.IsSuccessStatusCode)
                    return $"[Ollama Error]: {response.StatusCode}";

                // Ollama's response structure is slightly different from OpenAI/ZeroClaw
                var result = await response.Content.ReadFromJsonAsync<OllamaChatResponse>();
                return result?.message?.content ?? "No response.";
            }
            catch (Exception ex)
            {
                return $"[Local Error]: Is Ollama running? {ex.Message}";
            }
        }



    }
}
