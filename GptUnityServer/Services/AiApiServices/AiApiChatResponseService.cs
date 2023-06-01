using Newtonsoft.Json;
using System.Text;
using SharedLibrary;
using Newtonsoft.Json.Linq;

namespace GptUnityServer.Services.AiApiServices
{
    using Universal;
    using Models;
    public class AiApiChatResponseService : IAiChatResponseService
    {

        private readonly AiApiSetupData aiApiSetupData;
        private readonly PromptSettings promptSettings;

        public AiApiChatResponseService(AiApiSetupData _aiApiSetupData, PromptSettings _promptSettings)
        {

            aiApiSetupData = _aiApiSetupData;
            promptSettings = _promptSettings;
        }


        public async Task<AiResponse> SendMessage(string userMessage, string[] systemMessages)
        {
            Console.WriteLine("Running cloud based response for chat with system messages:");

            string apiKey = aiApiSetupData.ApiKey;
            //string message = userMessage;

            string formattedSystemMessages;

            formattedSystemMessages = "[";
            foreach (string message in systemMessages)
            { 
                int colonIndex = message.IndexOf(':');
                string messageAuthor = message.Substring(0, colonIndex);
                string messageContent = message.Remove(0, colonIndex+1);

                Console.WriteLine($"-{message}");
                formattedSystemMessages += "\n{";
                formattedSystemMessages +=
                   $"\"role\":\"{messageAuthor}\"," +
                    $"\"content\":\"{messageContent}\"";
                formattedSystemMessages += "},\n";
 
            }

            formattedSystemMessages += "{";
            formattedSystemMessages +=
               "\"role\":\"user\"," +
                $"\"content\":\"{userMessage}\"";
            formattedSystemMessages += "}\n]";
            Console.WriteLine("Displaying system messages in format:");
            Console.WriteLine($"{formattedSystemMessages}");
            HttpResponseMessage response = await CallAiApi(formattedSystemMessages);

            //Console.WriteLine("\nWhat returned was  " + response.Content);
            if (response.IsSuccessStatusCode)
            {
                string responseJsonText = response.Content.ReadAsStringAsync().Result;
                //Console.WriteLine("\n\n When paresed down it is"" : " + responseJsonText);

                JObject jsonData = JObject.Parse(responseJsonText);
                //Console.WriteLine($"\n Converted data {jsonData}\n");

                Console.WriteLine("Attempting to format response Json");
                string trimmedData = jsonData["choices"].ToString();
                string parsedMessage = jsonData["choices"][0]["message"]["content"].ToString();
                Console.WriteLine($"\n stringified data {trimmedData}\n");
                // Send the request and get the response           
                return new AiResponse(jsonData.ToString(), parsedMessage);
            }

            else
            {

                Console.WriteLine($"Failed to get ai response: {response.StatusCode} --- {response.ReasonPhrase}\n{response.ToString()}");
                return new AiResponse("", $"{response.StatusCode} --- {response.ReasonPhrase}");
            }

            // Print the response
            //Console.WriteLine($"Ai responds with \n {message}");

        }



        private async Task<HttpResponseMessage> CallAiApi(string messageList)
        {

            HttpClient client = new HttpClient();
            JArray array = JArray.Parse(messageList);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {aiApiSetupData.ApiKey}");
            var requestBody = new
            {
                    messages = array,
                    model = promptSettings.Model,
                    temperature = promptSettings.Temperature,
                    max_tokens = promptSettings.MaxTokens,
                    top_p = promptSettings.TopP,
                    frequency_penalty = promptSettings.FrequencyPenalty,
                
            };
            
            var jsonString = JsonConvert.SerializeObject(requestBody);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            Console.WriteLine(jsonString.ToString());
            string finalUrl = aiApiSetupData.ApiChatUrl;
            Console.WriteLine($"Making http request to Ai Api with url:{finalUrl}");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, finalUrl);
            request.Content = httpContent;

            return await client.SendAsync(request);
        }
    }



}


