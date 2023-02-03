using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GptUnityServer.Services.OpenAiServices.Api_Validation
{
    public class ApiKeyValidationService : IApiKeyValidation
    {
        public async Task<bool> ValidateApiKey(string key) {



            string apiUrl = "https://api.openai.com/v1/models";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"The API key is valid.\n Status Code: {response.StatusCode} \nApi key: {key}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"The API key is invalid.\n Status Code: {response.StatusCode} \nApi key: {key}");
                    return false;
                }
            }
        }
    





    }
}
