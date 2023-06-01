using System.Text;
using GptUnityServer.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SharedLibrary;
using GptUnityServer.Services.Universal;

namespace GptUnityServer.Services.OobaUiServices
{
    public class OobaUiResponseService : IAiResponseService
    {

        private readonly PromptSettings promptSettings;

        public OobaUiResponseService(PromptSettings _promptSettings)
        {

            promptSettings = _promptSettings;
        }


        public async Task<AiResponse> SendMessage(string prompt)
        {
            string url = "http://127.0.0.1:7860/run/textgen";
            string message;

            var paramsObj = new
            {
                //model = promptSettings.Model,
                temperature = promptSettings.Temperature,
                max_new_tokens = promptSettings.MaxTokens,
                top_p = promptSettings.TopP,
                seed = -1,
                //frequency_penalty = promptSettings.FrequencyPenalty,
            };
            
            string payload = JsonConvert.SerializeObject(new object[] { prompt, paramsObj });

            HttpClient client = new HttpClient();

            // Set up the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(
                JsonConvert.SerializeObject(
                    new { 
                    
                        data = new string[] { payload } 

                    }),
                    Encoding.UTF8, "application/json");

            // Send the request and get the response
            HttpResponseMessage response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                JObject responseJson = JObject.Parse(responseContent);
                message = responseJson["data"][0].ToString();
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
