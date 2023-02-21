using GptUnityServer.Services.ServerSetup;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GptUnityServer.Models
{
    public class Settings
    {
        public string ?ApiKey { get; set; }

        public string ?DefaultServerType { get; set; }
        public string ServerType { get { return ServerTypeEnum.ToString(); } }
        public string ServerConfig { get { return ServerConfigEnum.ToString(); } }
        /// <summary>
        /// JSON Web Token to indicate authentication privledges for serverless function calls
        /// </summary>
        public string ?AuthToken { get; set; }

        /// <summary>
        /// Name of the Open Ai Model to call
        /// </summary>
        public string? ProjectId { get; set; }


        /// <summary>
        /// Name of the Open Ai Model to call
        /// </summary>
        public string ?AiModel { get; set; }


        public string? CloudFunctionName { get; set; }
        public int Temperature { get; set; }
        public int MaxTokens { get; set; }
        public int TopP { get; set; }
        public int FrequencyPenalty { get; set; }


        //public bool IsApiKeyValid { get; set; }

        private ServerProtocolTypes ServerTypeEnum { get; set; }
        private ServerConfigType ServerConfigEnum { get; set; }

        public void RunSetUp(string[] args) {

            Console.WriteLine($"All arugments:\n {string.Join("\n",args)}");
            if (args.Length == 0)
            {
                SetServerProtocol(DefaultServerType);
                ServerConfigEnum = ServerConfigType.Api;
                Console.WriteLine($"Using default dev values! In {ServerConfigEnum} config");
                
            }

            else
            {
                if (args.Length >= 1)
                    SetServerProtocol(args[0]);

                if (args.Length >= 2) {

                    //If this argument is avalid JSON, use it as set up data for cloud code configuartion
                    if (args[1].StartsWith("{") && args[1].EndsWith("}"))
                        SetCloudServerData(args[1]);

                    else
                        SetApiKey(args[1]);
                }
                    


            }
        }

       void SetServerProtocol(string newServerType) {

           ServerProtocolTypes serverType;
           bool validType = ServerProtocolTypes.TryParse(newServerType, out serverType);

           if (!validType)
           {
               ServerTypeEnum = Enum.Parse<ServerProtocolTypes>(DefaultServerType);
           }
           else {
               ServerTypeEnum = Enum.Parse<ServerProtocolTypes>(newServerType);
           }

            Console.WriteLine($"Set server protocol to: {ServerTypeEnum}");

            //OnServerTypeChange.Invoke(newServerType);
        }

       void SetApiKey(string newKey) {

           if (!string.IsNullOrEmpty(newKey)) {
                Console.WriteLine($"Setting api key...");
                ServerConfigEnum = ServerConfigType.Api;
                ApiKey = newKey;
            }
        }

        void SetCloudServerData(string setupDataJson)
        {

            Console.WriteLine($"setting up server with cloud config data...");
            Console.WriteLine($"Attemtping to deseralize: {setupDataJson}");
            ServerConfigEnum = ServerConfigType.Cloud;
            CloudServerSetupData setupData = JsonConvert.DeserializeObject<CloudServerSetupData>(setupDataJson);
            AuthToken = setupData.PlayerAuthenticationToken;
            CloudFunctionName = setupData.CloudFunctionName;

        }

        //public Action<string> OnServerTypeChange;

        private enum ServerProtocolTypes { 
        
            TCP,
            UDP
        }

        private enum ServerConfigType
        {

            Api,
            Cloud
        }

    }
}
