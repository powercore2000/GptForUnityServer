using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SharedLibrary;
using GptUnityServer.Services.Universal;
using System.Threading.Tasks;

namespace GptUnityServer
{
    [ApiController]
    [Route("[controller]")]
    public class UnityServerApiController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<UnityServerApiController> logger;
        private readonly IAiResponseService aiResponseService;

        public UnityServerApiController(ILogger<UnityServerApiController> _logger, IAiResponseService _aiResponseService)
        {
            logger = _logger;
            aiResponseService = _aiResponseService;
        }

        [HttpGet(Name = "SomeMethod")]
        public IEnumerable<UnityServerApiController> Get()
        {
            return null;
        }

        [HttpPost(Name = "SendMessage")]
        public async Task<AiResponse> Get(string prompt)
        {
            return await aiResponseService.SendMessage(prompt);
        }

        /*string url = "http://127.0.0.1:5000/api/v1/generate";
            string message;
            PromptSettings promptSettings = new PromptSettings();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            // Set up the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                prompt,
                temperature = promptSettings.Temperature,
                top_p = promptSettings.TopP,
                //no_repeat_ngram_size = 1,
                early_stopping = true,
                stopping_strings = new string[] { "You:", "\n[", "]:", "##", "###", "<noinput>", "\\end" },
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
            return aiResponse;*/
    }
}
