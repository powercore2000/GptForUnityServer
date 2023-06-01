using System.Diagnostics;
using System.Net.Http.Headers;

namespace GptUnityServer.Services.AiApiServices
{
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using Assets.GptToUnity.SharedLibrary;
    using GptUnityServer.Services.Universal;
    using Microsoft.AspNetCore.DataProtection.KeyManagement;
    using Models;
    using Newtonsoft.Json.Linq;
    using SharedLibrary;

    /// <summary>
    /// THIS IS FOR TESTING ONLY. It is not recomended to use your API key on the client's machine to perform these operations.
    /// </summary>
    public class AiApiModelManager : IAiModelManager
    {

        private readonly AiApiSetupData aiApiSetupData;

        public AiApiModelManager(AiApiSetupData _setupData)
        {

            aiApiSetupData = _setupData;
        }

        public async Task<string[]> GetAllModels()
        {
            List<string> modelList = new List<string>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + aiApiSetupData.ApiKey);
            HttpResponseMessage response = await httpClient.GetAsync(aiApiSetupData.ApiKeyValidationUrl);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully aquired models!");
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject responseJson = JObject.Parse(responseContent);

                foreach (JObject element in responseJson["data"])
                {
                    modelList.Add(element["id"].ToString());
                }
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
