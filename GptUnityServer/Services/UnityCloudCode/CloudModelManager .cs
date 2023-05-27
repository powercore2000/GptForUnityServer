namespace GptUnityServer.Services.UnityCloudCode
{
    using System;
    using System.Text;
    using Assets.GptToUnity.SharedLibrary;
    using Models;
    using Assets.GptToUnity.SharedLibrary;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using GptUnityServer.Services.Universal;

    public class CloudModelManager : IAiModelManager
    {

        private readonly Settings settings;
        string url = "https://cloud-code.services.api.unity.com/v1/projects";

        public CloudModelManager(Settings _settings)
        {

            settings = _settings;
        }

        public async Task<string[]> GetAllModels()
        {
            Console.WriteLine("Running cloud based mode list fetch!");
            List<string> modelList = new List<string>();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.CloudAuthToken}");


            string finalUrl = url + $"/{settings.CloudProjectId}/{settings.CloudCodeEndpoint}/{settings.CloudModelListFunction}";
            Console.WriteLine($"Making http request with url:\n{finalUrl}");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, finalUrl);


            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully aquired models!");
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject listObject = JObject.Parse(responseContent);
                JArray responseJson = JArray.Parse(listObject["output"].ToString());
                dynamic jsonList = JsonConvert.DeserializeObject(responseJson.ToString());
                //Ugly hack solution because im tired :(
                for (int i = 0; i < responseJson.Count; i++)
                    {

                        modelList.Add(responseJson[i].ToString());

                    }

                /*
                foreach (JObject element in responseJson["output"])
                {
                    modelList.Add(element["id"].ToString());
                }*/
                Console.WriteLine($"Got models with list length: {modelList.Count}");
            }
            else
            {
                Console.WriteLine($"Failed to get models: {response.StatusCode} - {response.ReasonPhrase}");
            }


            return modelList.ToArray();
        }



    }
}
