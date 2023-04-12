using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using SharedLibrary;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using GptUnityServer.Services.Universal;

namespace GptUnityServer.Services.KoboldAIServices
{
    public class KoboldAiResponseService : IAiResponseService
    {

        private readonly PromptSettings promptSettings;

        public KoboldAiResponseService(PromptSettings _promptSettings)
        {

            promptSettings = _promptSettings;
        }


        public async Task<AiResponse> SendMessage(string prompt)
        {
            string url = "http://127.0.0.1:5000/api/v1/generate";
            string message;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            // Set up the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            //request.Content = new StringContent("{\"prompt\":\"" + prompt + "\",\"temperature\":0.5}");
            //request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            request.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                prompt,
                //model = promptSettings.Model,
                temperature = promptSettings.Temperature,
                //max_tokens = promptSettings.MaxTokens,
                top_p = promptSettings.TopP,
                //frequency_penalty = promptSettings.FrequencyPenalty,
            }), Encoding.UTF8, "application/json");

            // Send the request and get the response
            HttpResponseMessage response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                JObject responseJson = JObject.Parse(responseContent);
                message = responseJson["results"][0]["text"].ToString();
            }

            else
            {
                message = responseContent;
            }

            AiResponse aiResponse = new AiResponse(responseContent, message);


            // Print the response
            //Console.WriteLine($"Ai responds with \n {message}");
            Console.WriteLine($"\n Raw Json output: {aiResponse.JsonRaw}\n\n");
            return aiResponse;
        }

        public async Task<string> GetModel()
        {
            string url = "http://127.0.0.1:5000/api/v1/generate";

            string message;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");

            // Set up the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);


            // Send the request and get the response
            HttpResponseMessage response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                JObject responseJson = JObject.Parse(responseContent);
                message = responseJson["result"].ToString();
            }

            else
            {
                message = responseContent;
            }


            // Print the response
            //Console.WriteLine($"Ai responds with \n {message}");
            Console.WriteLine($"\n Raw Json output: {responseContent}\n\n");
            return message;
        }
    }
}

