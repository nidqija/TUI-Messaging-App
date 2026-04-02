using StackExchange.Redis;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using TUI_Messaging_App.TUI_Messaging_App.Controller;

namespace TUI_Messaging_App.TUI_Messaging_App.Services
{
    internal class AskOllamaServices
    {

        private String ollamaEndpointUrl = "http://localhost:11434/api/chat";
        private readonly HttpClient httpClient = new HttpClient();
        private static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        private readonly MessagesController messagesController = new MessagesController();





        public void Start()
        {
            // subscribe to redis channel for new messages to ollama
            var sub = redis.GetSubscriber();

            // listen for new messages sent to ollama and respond with the AI response
            sub.Subscribe("messages:ollama", async (channel, message) =>
            {
                // 1. Get the prompt from the Redis message content
                string userPrompt = message.ToString();
                string cleanPrompt = userPrompt.Replace("/ollama", "").Replace("@ollama", "").Trim();

                // 2. Call Ollama
                string aiResult = await GetOllamaResponse(cleanPrompt);

                // 3. Save to DB
                messagesController.insertMessage("ollama", SessionInitializer.Username, $"[OLLAMA] {aiResult}");

                // 4. Tell the UI to refresh
                var pub = redis.GetSubscriber();
                pub.Publish($"messages:{SessionInitializer.Username}", "REFRESH_CHAT");
            });


        }


        private async Task<string> GetOllamaResponse(string prompt)
        {
            try
            {
                var payload = new
                {
                    model = "gemma3:1b",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a helpful TUI Assistant." },
                        new { role = "user", content = prompt }

                    },
                    stream = false
                };

                var response = await httpClient.PostAsJsonAsync(ollamaEndpointUrl, payload);
                var result = await response.Content.ReadFromJsonAsync<OllamaChatResponse>();

                return result?.message.content ?? "Im sorry , I cannot answer that ";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while calling Ollama API: {ex.Message}");
                return "Sorry, I'm having trouble responding right now.";
            }
        }

        public class OllamaChatResponse
        {
            [JsonPropertyName("message")]
            public OllamaMessage message { get; set; }
        }

        public class OllamaMessage
        {
            [JsonPropertyName("role")]
            public string role { get; set; }

            [JsonPropertyName("content")]
            public string content { get; set; }
        }


    }
}
