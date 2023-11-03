using System;
using System.Text;
using Newtonsoft.Json;
using SharedLibrary;

namespace GptUnityServer.Services.AiApiServices
{
    using GptUnityServer.Services.Universal;
    using Models;
    using Newtonsoft.Json.Linq;

    public class AiApiInstructService : IAiInstructService
    {
        private readonly AiApiSetupData aiApiSetupData;
        private readonly PromptSettings promptSettings;
        public AiApiInstructService(AiApiSetupData _aiApiSetupData, PromptSettings _promptSettings)
        {

            aiApiSetupData = _aiApiSetupData;
            promptSettings = _promptSettings;
        }


        public async Task<AiResponse> SendMessage(string prompt)
        {
            string url = aiApiSetupData.ApiInstructUrl;
            string apiKey = aiApiSetupData.ApiKey;
            string message;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

            // Set up the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                prompt,
                model = promptSettings.Model,
                temp = promptSettings.temp,
                max_tokens = promptSettings.max_tokens,
                top_p = promptSettings.top_p,
                frequency_penalty = promptSettings.frequency_penalty,
            }), Encoding.UTF8, "application/json");



            // Send the request and get the response
            HttpResponseMessage response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                JObject responseJson = JObject.Parse(responseContent);
                message = responseJson["choices"][0]["text"].ToString();
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


    }
}
