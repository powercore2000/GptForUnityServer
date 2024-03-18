namespace GptUnityServer.Services.UnityCloud
{
    using System;
    using System.Text;
    using SharedLibrary;
    using Models;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using GptUnityServer.Services.Universal;

    public class CloudModelManager : IAiModelManager
    {

        private readonly UnityCloudSetupData settings;
        string url = "https://cloud-code.services.api.unity.com/v1/projects";

        public CloudModelManager(UnityCloudSetupData _settings)
        {

            settings = _settings;
        }

        public async Task<string[]> GetAllModels()
        {
            Console.WriteLine("Running cloud based mode list fetch!");
            List<string> modelList = new List<string>();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.UnityCloudPlayerToken}");


            string finalUrl = url + $"/{settings.UnityCloudProjectId}/{settings.UnityCloudEndpoint}/{settings.UnityCloudModelsFunction}";
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
