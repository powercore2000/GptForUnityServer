using System;
using System.Text;
using Newtonsoft.Json;
using SharedLibrary;

namespace GptUnityServer.Services.OpenAiServices
{
    using GptUnityServer.Services.Universal;
    using Models;
    using Newtonsoft.Json.Linq;

    public class ApiResponseService : IAiResponseService
    {
        private readonly Settings settings;
        private readonly PromptSettings promptSettings;
        public ApiResponseService(Settings _settings, PromptSettings _promptSettings)
        {

            settings = _settings;
            promptSettings = _promptSettings;
        }


        public async Task<AiResponse> SendMessage(string prompt)
        {
            string url = "https://api.openai.com/v1/completions";
            string apiKey = settings.AiApiKey;
            string message;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

            // Set up the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            //request.Content = new StringContent("{\"prompt\":\"" + prompt + "\",\"temperature\":0.5}");
            //request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            request.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                prompt,
                model = promptSettings.Model,
                temperature = promptSettings.Temperature,
                max_tokens = promptSettings.MaxTokens,
                top_p = promptSettings.TopP,
                frequency_penalty = promptSettings.FrequencyPenalty,
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
