using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUI_Messaging_App.TUI_Messaging_App.Services
{
    internal class AskOllamaServices
    {

        private String ollamaEndpointUrl = "http://localhost:11434/api/chat";


        

        public void askOllama(string question)
        {
            Console.WriteLine($"Asking Ollama: {question}");
            getOllamaResponse().Wait();
            Task.Delay(2000).Wait(); // Simulate processing time
            Console.WriteLine("Ollama's response: This is a simulated response to your question.");
        }



        private async Task<bool> getOllamaResponse()
        {
            try
            {
                Console.WriteLine($"Getting response from Ollama with endpoint of :{ollamaEndpointUrl}");
                

            } catch (Exception ex)
            {
                Console.WriteLine($"Error getting response from Ollama: {ex.Message}");
                return false;
            }

            return true;
        }
    }
}
