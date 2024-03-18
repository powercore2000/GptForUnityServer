using Newtonsoft.Json;
using System;

namespace SharedLibrary
{
    public class AiApiSetupData
    {
        public string? ApiKey { get; set; }

        public string? ApiChatUrl { get; set; }
        public string? ApiInstructUrl { get; set; }

        public string? ApiKeyValidationUrl { get; set; }


        public void RunSetUp(string[] args)
        {

            if (args.Length >= 3)
                SetServerAuthenticationData(args[2]);
        }

        /// <summary>
        /// Sets the authentication data needed for the server to make web requests:
        /// API Mode: Data is a JSON object containing the ApiKey, the Api Url, and the Url for Api Key Validation
        /// Cloud Mode: Data is a JSON object containing the names of the endpoint functions used, and authentication data
        /// </summary>
        /// <param name="data"></param>
        void SetServerAuthenticationData(string data)
        {
            
            if (string.IsNullOrEmpty(data))
                return;

            if (data.StartsWith("{") && data.EndsWith("}"))
            {

                Console.WriteLine($"Attemtping to deseralize AiApiData: {data}");
                AiApiSetupData setupData = JsonConvert.DeserializeObject<AiApiSetupData>(data);
                ApiKey = setupData.ApiKey;
                ApiInstructUrl = setupData.ApiInstructUrl;
                ApiChatUrl = setupData.ApiChatUrl;
                ApiKeyValidationUrl = setupData.ApiKeyValidationUrl;

            }


        }

    }
}
