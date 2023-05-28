using GptUnityServer.Models;
using GptUnityServer.Services.Universal;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GptUnityServer.Services.AiApiServices
{
    public class AiApiKeyValidationService : IKeyValidationService
    {

        public async Task<bool> ValidateKey(string key, string validationUrl)
        {

            using (HttpClient client = new HttpClient())
            {
                
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");

                var response = await client.GetAsync(validationUrl);


                if (response.IsSuccessStatusCode)
                {

                    Console.WriteLine($"The API key is valid.\n Status Code: {response.StatusCode} \n With Validation url: {validationUrl}");
                    
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
