using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TUI_Messaging_App.TUI_Messaging_App.Services
{
    internal class ZeroClawService
    {
        private readonly HttpClient _client = new HttpClient
        {
            // 5 minutes allows the model to load into VRAM on the first call
            Timeout = TimeSpan.FromMinutes(5)
        };

        public async Task<string> GetAIResponse(string prompt)
        {
            var requestBody = new
            {
                model = "gemma3:1b",
                prompt = prompt,
                system = "You are a JSON extractor. Output ONLY raw valid JSON, nothing else. No explanation, no markdown, no code fences. Use exactly this format: {\"title\":\"...\",\"start\":\"YYYY-MM-DDTHH:mm:ss\",\"end\":\"YYYY-MM-DDTHH:mm:ss\"}",
                stream = false,
                options = new
                {
                    temperature = 0,
                    num_predict = 200
                }
            };

            try
            {
                var response = await _client.PostAsJsonAsync("http://localhost:11434/api/generate", requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    return $"[Ollama Error {response.StatusCode}]: {errorDetails}";
                }

                // Read raw string first to verify content exists
                string rawJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine("[ZeroClaw DEBUG] Raw Ollama body: " + rawJson);

                if (string.IsNullOrWhiteSpace(rawJson))
                    return "[Error]: Ollama returned an empty body.";

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<OllamaGenerateResponse>(rawJson, options);

                // qwen3 thinking models write reasoning to "thinking" and answer to "response".
                // If response is empty, the model likely ran out of tokens mid-think — extract JSON from thinking as fallback.
                string content = result?.Response;
                if (string.IsNullOrWhiteSpace(content))
                    content = result?.Thinking;

                if (string.IsNullOrWhiteSpace(content))
                    return $"[Error]: Ollama response field was empty. Full body: {rawJson}";

                return content;
            }
            catch (TaskCanceledException)
            {
                return "[Error]: Ollama took too long to load the model. Is your GPU busy?";
            }
            catch (Exception ex)
            {
                return $"[Local Error]: {ex.Message}";
            }
        }
    }

    public class OllamaGenerateResponse
    {
        [JsonPropertyName("response")]
        public string Response { get; set; }

        [JsonPropertyName("thinking")]
        public string Thinking { get; set; }
    }
}