using System.Text;
using GptUnityServer.Services.Universal;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SharedLibrary;

namespace GptUnityServer.Services.KoboldAIServices
{
    public class KoboldAIChatService : IAiChatResponseService
    {

        private readonly PromptSettings promptSettings;

        public KoboldAIChatService(PromptSettings _promptSettings)
        {

            promptSettings = _promptSettings;
        }

        public async Task<AiResponse> SendMessage(string userMessage, string[] systemMessages)
        {
            string url = "http://127.0.0.1:5000/api/v1/generate";
            string message;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            // Set up the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            string chatHistory = string.Join(" ", systemMessages);
            string prompt = chatHistory + userMessage;
            request.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                prompt,
                temp = 0.7,
                top_p = 0.5,
                top_k = 40,
                typical_p = 1,
                top_a = 0,
                tfs = 1,
                epsilon_cutoff = 0,
                eta_cutoff = 0,
                rep_pen = 1.2,
                no_repeat_ngram_size = 0,
                penalty_alpha = 0,
                num_beams = 1,
                length_penalty = 1,
                min_length = 0,
                encoder_rep_pen = 1,
                do_sample = true,
                early_stopping = false,
                stopping_strings = new string[] { "You:" }               
            }), Encoding.UTF8, "application/json");

            // Send the request and get the response
            HttpResponseMessage response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(responseContent);
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
    }
}
