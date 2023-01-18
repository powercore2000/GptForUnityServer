using System;
using GptToUnityServer.Models;
using SharedLibrary;

namespace GptToUnityServer.Services
{
    public interface IOpenAiService {

        public Task<AiResponse> SendMessage(string prompt);
    }

    public class GenericOpenAiService : IOpenAiService
    {
        private readonly Settings settings;
        public GenericOpenAiService(Settings _settings) {

            settings = _settings;
        }


        public async Task<AiResponse> SendMessage(string prompt)
        {
            string url = "https://api.openai.com/v1/engines/davinci/completions";
            string apiKey = settings.BearerKey;
            string model = "text-davinci-002";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

            // Set up the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent("{\"prompt\":\"" + prompt + "\",\"temperature\":0.5}");
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            // Send the request and get the response
            HttpResponseMessage response = await client.SendAsync(request);
            AiResponse aiResponse = new AiResponse(await response.Content.ReadAsStringAsync());
            string message = aiResponse.Message;


            // Print the response
            Console.WriteLine($"Ai responds with {message}");
            Console.WriteLine($"\n\n Raw Json output: {aiResponse.JsonRaw}");
            return aiResponse;
        }


    }
}
