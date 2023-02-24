﻿using System.Diagnostics;
using System.Net.Http.Headers;
namespace GptUnityServer.Services.OpenAiServices.OpenAiData
{
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using Assets.GptToUnity.SharedLibrary;
    using Microsoft.AspNetCore.DataProtection.KeyManagement;
    using Models;
    using Newtonsoft.Json.Linq;

    public class ApiModelManager : IOpenAiModelManager
    {

        private readonly Settings settings;

        public ApiModelManager(Settings _settings)
        {

            settings = _settings;
        }

        public async Task<string[]> GetAllModels()
        {
            List<string> modelList = new List<string>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + settings.AiApiKey);
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.openai.com/v1/models");

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