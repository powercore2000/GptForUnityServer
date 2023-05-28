using System;
using GptUnityServer.Services.Universal;
using Microsoft.OpenApi.Validations;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GptUnityServer.Models;
using static System.Net.WebRequestMethods;

namespace GptUnityServer.Services.UnityCloudCode
{
    public class CloudCodeValidationServices : IKeyValidationService
    {
        public async Task<bool> ValidateKey(string key, string valdiationUrl)
        {

            Console.WriteLine("Validadting cloud code!");

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");

          
            Console.WriteLine($"Making http request with url:\n{valdiationUrl}");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, valdiationUrl);


            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"The API key is valid.\n Status Code: {response.StatusCode} \n With Validation url: {valdiationUrl}");

                return await Task.FromResult(true);
            }

            else
            {
                Console.WriteLine($"The API key is invalid.\n Status Code: {response.StatusCode} \nURL: {valdiationUrl}");

                return await Task.FromResult(false);
            }


            
        }

    }





}
