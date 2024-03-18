using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace SharedLibrary
{
    [ComVisible(true)]
    public class UnityCloudSetupData
    {
       
        public string UnityCloudPlayerToken { get; set; }      

        public string UnityCloudProjectId { get; set; }
        public string UnityCloudEndpoint { get; set; }

        public string UnityCloudModelsFunction { get; set; }

        public string UnityCloudInstructFunction { get; set; }
        public string UnityCloudChatFunction { get; set; }

        public string ImageFunctionName { get; set; }



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

                //Console.WriteLine($"Attemtping to deseralize UnityCloudSetupData: {data}");
                UnityCloudSetupData setupData = JsonConvert.DeserializeObject<UnityCloudSetupData>(data);
                UnityCloudPlayerToken = setupData.UnityCloudPlayerToken;
                UnityCloudInstructFunction = setupData.UnityCloudInstructFunction;
                UnityCloudChatFunction = setupData.UnityCloudChatFunction;
                UnityCloudModelsFunction = setupData.UnityCloudModelsFunction;
                UnityCloudProjectId = setupData.UnityCloudProjectId;
                UnityCloudEndpoint = setupData.UnityCloudEndpoint;
            }


        }

    }
}