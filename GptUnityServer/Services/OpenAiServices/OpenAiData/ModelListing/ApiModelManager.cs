using System.Diagnostics;
using System.Net.Http.Headers;
namespace GptUnityServer.Services.OpenAiServices.OpenAiData.ModelListing
{
    using System.Reflection;
    using System.Text.Json;
    using Assets.GptToUnity.SharedLibrary;
    using Models;
    
    public class ApiModelManager : IOpenAiModelManager
    {

        private readonly Settings settings;

        public ApiModelManager(Settings _settings)
        {

            settings = _settings;
        }

        public async Task<ModelData[]> GetAllModels()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.openai.com/v1/organizations/{settings.ProjectId}/models");
            ModelsResponse models = new ModelsResponse();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                models = JsonSerializer.Deserialize<ModelsResponse>(responseContent);
                foreach (var model in models.Data)
                {
                    Console.WriteLine(model.ModelName);
                }
            }
            else
            {
                Console.WriteLine($"Failed to get models: {response.StatusCode} - {response.ReasonPhrase}");
            }
            Console.WriteLine($"Returning list of models with count: {models.Data.Length}\nAnd with elements: {models.Data}");
            return models.Data;
        }

        public class ModelsResponse
        {
            public ModelData[] Data { get; set; }
        }



    }
}
