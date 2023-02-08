using Newtonsoft.Json;
using System.Text;
using SharedLibrary;
using GptToUnityServer.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace GptUnityServer.Services.OpenAiServices.PromptSending
{
    public class UnityCloudCodePromptService : IOpenAiPromptService
    {

        private readonly Settings settings;
        public UnityCloudCodePromptService(Settings _settings)
        {

            settings = _settings;
        }

        public async Task<AiResponse> SendMessage(string prompt)
        {
            string url = "https://cloud-code.services.api.unity.com/v1/projects";
            string apiKey = settings.ApiKey;
            string playerSessionId = settings.PlayerIdToken;
            string titleId = "641d700a-b48a-4b51-b2db-e776550b0009";



            HttpResponseMessage response = await CallCloudCode(playerSessionId, titleId,url);
            Console.WriteLine("\nWhat returend was  " + response.Content);

            string responseJsonText = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("\n\n When paresed down it is : " + responseJsonText);

            JObject jsonData = JObject.Parse(responseJsonText);
            Console.WriteLine($"\n Converted data {jsonData}\n");

            string trimmedData = jsonData["output"].ToString();
            Console.WriteLine($"\n stringified data {trimmedData}\n");
            // Send the request and get the response
            
            AiResponse aiResponse = new AiResponse(trimmedData);
            string message = aiResponse.Message;


            // Print the response
            Console.WriteLine($"Ai responds with \n {message}");
            Console.WriteLine($"\n Raw Json output: {aiResponse.JsonRaw}\n\n");
            return aiResponse;
        }



        private static async Task<HttpResponseMessage> CallCloudCode(string playerId, string titleID, string url)
        {
            HttpClient client = new HttpClient();
    
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {playerId}");

            var requestBody = new
            {
                @params = new
                {
                    prompt = "How are you?"
                }
            };

            var jsonString = JsonConvert.SerializeObject(requestBody);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            string finalUrl = url + $"/{titleID}/scripts/OpenAiRequest";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, finalUrl);
            request.Content = httpContent;

            return await client.SendAsync(request);
        }
    }



}
