using StackExchange.Redis;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using TUI_Messaging_App.TUI_Messaging_App.Controller;

namespace TUI_Messaging_App.TUI_Messaging_App.Services
{
    internal class AskOllamaServices
    {

        private String ollamaEndpointUrl = "http://localhost:11434/api/generate";
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
                var recentMessages = messagesController.getMessagesBetweenUsers("ollama", SessionInitializer.Username);
                var lastUserMsg = recentMessages.LastOrDefault(m => m.SenderUsername !=  "ollama");


                if (lastUserMsg != null &&
        lastUserMsg.SenderUsername != "ollama" &&
        (lastUserMsg.MessageContent.StartsWith("/ollama") || lastUserMsg.MessageContent.StartsWith("@ollama")))
                {
                    string cleanPrompt = lastUserMsg.MessageContent
            .Replace("/ollama", "")
            .Replace("@ollama", "")
            .Trim();

                    string aiResult = await GetOllamaResponse(cleanPrompt);

                    messagesController.insertMessage("ollama", SessionInitializer.Username, $"[OLLAMA] {aiResult}");
                }
            });


        }


        private async Task<string> GetOllamaResponse(string prompt)
        {
            try
            {
                var payload = new
                {
                    model = "gemma3:1b",
                    prompt = $"Instructions: You are a helpful TUI Assistant. User says: {prompt}",
                    stream = false
                };

                var response = await httpClient.PostAsJsonAsync(ollamaEndpointUrl, payload);
                var result = await response.Content.ReadFromJsonAsync<OllamaPayload>();

                return result?.response ?? "Im sorry , I cannot answer that ";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while calling Ollama API: {ex.Message}");
                return "Sorry, I'm having trouble responding right now.";
            }
        }

        public class OllamaPayload
        {
            [JsonPropertyName("response")]
            public string response { get; set; }
        }


    }
}
