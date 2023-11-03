using System.Text;
using GptUnityServer.Services.Universal;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SharedLibrary;

namespace GptUnityServer.Services.OobaUiServices
{
    public class OobaUiChatService : IAiChatService
    {


        public async Task<AiResponse> SendMessage(PromptSettings promptSettings)
        {
            string url = "http://127.0.0.1:5000/api/v1/generate";
            string message;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            // Set up the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            string additonalContext = string.Empty;
            if (promptSettings.context_history != null)
                additonalContext = string.Join(" ", promptSettings.context_history);


            string prompt = additonalContext + "\r\n"+ promptSettings.prompt;
            Console.WriteLine($"Final prompt {prompt}");
            request.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                prompt,
                promptSettings.temp,
                promptSettings.top_p,
                promptSettings.top_k,
                promptSettings.typical_p,
                promptSettings.top_a,
                promptSettings.tfs,
                promptSettings.epsilon_cutoff,
                promptSettings.eta_cutoff,
                promptSettings.rep_pen,
                promptSettings.no_repeat_ngram_size,
                promptSettings.penalty_alpha,
                promptSettings.num_beams,
                promptSettings.length_penalty,
                promptSettings.min_length,
                promptSettings.encoder_rep_pen,
                promptSettings.do_sample,
                promptSettings.early_stopping,
                promptSettings.stopping_strings              
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