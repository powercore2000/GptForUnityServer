using Newtonsoft.Json;
using System.Text;
using SharedLibrary;
using Newtonsoft.Json.Linq;

namespace GptUnityServer.Services.UnityCloud
{
    using GptUnityServer.Services.Universal;
    using Models;
    public class CloudInstructService : IAiInstructService
    {

        private readonly UnityCloudSetupData settings;
        private readonly PromptSettings promptSettings;
        string url = "https://cloud-code.services.api.unity.com/v1/projects";

        public CloudInstructService(UnityCloudSetupData _settings, PromptSettings _promptSettings)
        {

            settings = _settings;
            promptSettings = _promptSettings;
        }


        public async Task<AiResponse> SendMessage(string prompt)
        {

            HttpResponseMessage response = await CallCloudCode(prompt);
            //Console.WriteLine("\nWhat returned was  " + response.Content);
            if (response.IsSuccessStatusCode)
            {
                string responseJsonText = response.Content.ReadAsStringAsync().Result;
                //Console.WriteLine("\n\n When paresed down it is : " + responseJsonText);

                JObject jsonData = JObject.Parse(responseJsonText);
                //Console.WriteLine($"\n Converted data {jsonData}\n");
                //Console.WriteLine($"TrimmedData:{jsonData.ToString()}");

                string trimmedData = jsonData["output"]["jsonRaw"].ToString();
                string parsedMessage = jsonData["output"]["message"].ToString();
                //Console.WriteLine($"\n stringified data {trimmedData}\n");
                // Send the request and get the response           
                return new AiResponse(trimmedData, parsedMessage);
            }

            else
            {

                Console.WriteLine($"Failed to get ai response: {response.StatusCode} --- {response.ReasonPhrase}\n{response.ToString()}");
                return new AiResponse("", $"{response.StatusCode} --- {response.ReasonPhrase}");
            }

            // Print the response
            //Console.WriteLine($"Ai responds with \n {message}");

        }



        private async Task<HttpResponseMessage> CallCloudCode(string prompt)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.UnityCloudPlayerToken}");
            var requestBody = new
            {
                @params = new
                {
                    prompt = new
                    {
                        prompt = prompt,
                        Model = promptSettings.Model,
                        temp = promptSettings.temp,
                        max_tokens = promptSettings.max_tokens,
                        top_p = promptSettings.top_p,
                        frequency_penalty = promptSettings.frequency_penalty,
                    }
                }
            };

            var jsonString = JsonConvert.SerializeObject(requestBody);
            
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");


            string finalUrl = url + $"/{settings.UnityCloudProjectId}/{settings.UnityCloudEndpoint}/{settings.UnityCloudInstructFunction}";
            Console.WriteLine($"Making http request to Cloude Code with url:{finalUrl}");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, finalUrl);
            request.Content = httpContent;

            return await client.SendAsync(request);
        }
    }



}
