using System.Text;
using GptForUnityServer.Services.Universal;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SharedLibrary;

namespace GptForUnityServer.Services.EmotionClassificationServices
{
    public class SillyTavernExtraSimpleClassifyService : IEmotionClassificationService
    {
        public async Task<List<EmotionData>> ClassifyMessage(string message) {

            string url = "http://localhost:5100/api/classify";
            List<EmotionData> emotions = new List<EmotionData>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            // Set up the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                text = message

            }), Encoding.UTF8, "application/json");

            // Send the request and get the response
            HttpResponseMessage response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                emotions = new List<EmotionData>();
                //JObject responseJsonArray = JObject.Parse(responseContent);
                ClassificationResultArray classificationResult = JsonConvert.DeserializeObject<ClassificationResultArray>(responseContent);

                foreach (EmotionModel emotionModel in classificationResult.Classification) {

                    emotions.Add(new EmotionData(emotionModel.Label, emotionModel.Score));
                }
               
            }

          


            // Print the response
            //Console.WriteLine($"Ai responds with \n {message}");
            //Console.WriteLine($"\n Raw Json output: {aiResponse.JsonRaw}\n\n");
            return emotions;
        }
    }


    class ClassificationResultArray
    {
        public EmotionModel[] Classification { get; set; }
    }

    class EmotionModel
    {
        public string Label { get; set; }
        public float Score { get; set; }
    }


}
