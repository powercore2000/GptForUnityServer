using Newtonsoft.Json;
using System.Text;
using SharedLibrary;
using Newtonsoft.Json.Linq;

namespace GptUnityServer.Services.OpenAiServices.PromptServices
{
    using Models;
    public class CloudPromptService : IOpenAiPromptService
    {

        private readonly Settings settings;
        string url = "https://cloud-code.services.api.unity.com/v1/projects";

        public CloudPromptService(Settings _settings)
        {

            settings = _settings;
        }


        public async Task<AiResponse> SendMessage(string prompt)
        {

            Console.WriteLine(settings.ApiKey);

            HttpResponseMessage response = await CallCloudCode(prompt);
            Console.WriteLine("\nWhat returned was  " + response.Content);

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
            return aiResponse;
        }



        private async Task<HttpResponseMessage> CallCloudCode(string prompt)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.AuthToken}");
            var requestBody = new
            {
                @params = new
                {
                    prompt,
                    model = settings.AiModel,
                    temperature = settings.Temperature,
                    max_tokens = settings.MaxTokens,
                    top_p = settings.TopP,
                    frequency_penalty = settings.FrequencyPenalty,
                }
            };

            var jsonString = JsonConvert.SerializeObject(requestBody);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");


            string finalUrl = url + $"/{settings.ProjectId}/scripts/{settings.CloudFunctionName}";
            Console.WriteLine($"Making http request with url:{finalUrl}");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, finalUrl);
            request.Content = httpContent;

            return await client.SendAsync(request);
        }
    }



}
