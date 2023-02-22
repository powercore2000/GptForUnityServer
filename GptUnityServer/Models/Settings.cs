using Newtonsoft.Json;

namespace GptUnityServer.Models
{
    public class Settings
    {
        public string ?AiApiKey { get; set; }

        public string ?DefaultProtocolType { get; set; }
        public string ?DefaultConfigType { get; set; }
        public string ServerType { get { return ServerTypeEnum.ToString(); } }
        public string ServerConfig { get { return ServerConfigEnum.ToString(); } }
        /// <summary>
        /// JSON Web Token to indicate authentication privledges for serverless function calls
        /// </summary>
        public string ?CloudAuthToken { get; set; }

        /// <summary>
        /// The id of the organization your going to call
        /// </summary>
        public string? CloudProjectId { get; set; }

        public string? ResponseCloudFunction { get; set; }
        public string? ModelListCloudFunction { get; set; }

        //public bool IsApiKeyValid { get; set; }

        private ServerProtocolTypes ServerTypeEnum { get; set; }
        private ServerConfigType ServerConfigEnum { get; set; }

        public void RunSetUp(string[] args) {

            Console.WriteLine($"All arugments:\n {string.Join("\n",args)}");
            if (args.Length == 0)
            {

                StartDefaultDevMode();
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

        void StartDefaultDevMode() {
            SetServerProtocol(DefaultProtocolType);
            SetServerConfig(DefaultConfigType);
            Console.WriteLine($"Using default dev values! In {ServerConfigEnum} config");

        }

       void SetServerProtocol(string newServerType) {

           ServerProtocolTypes serverType;
           bool validType = ServerProtocolTypes.TryParse(newServerType, out serverType);

           if (!validType)
           {
               ServerTypeEnum = Enum.Parse<ServerProtocolTypes>(newServerType);
           }
           else {
               ServerTypeEnum = Enum.Parse<ServerProtocolTypes>(newServerType);
           }

            Console.WriteLine($"Set server protocol to: {ServerTypeEnum}");

            //OnServerTypeChange.Invoke(newServerType);
        }

        void SetServerConfig(string newConfigType)
        {

            ServerConfigType serverConfig;
            bool validType = ServerConfigType.TryParse(newConfigType, out serverConfig);

            if (!validType)
            {
                ServerConfigEnum = Enum.Parse<ServerConfigType>(newConfigType);
            }
            else
            {
                ServerConfigEnum = Enum.Parse<ServerConfigType>(newConfigType);
            }

            Console.WriteLine($"Set server protocol to: {ServerTypeEnum}");

            //OnServerTypeChange.Invoke(newServerType);
        }


        void SetApiKey(string newKey) {

           if (!string.IsNullOrEmpty(newKey)) {
                Console.WriteLine($"Setting api key...");
                ServerConfigEnum = ServerConfigType.Api;
                AiApiKey = newKey;
            }
        }

        void SetCloudServerData(string setupDataJson)
        {

            Console.WriteLine($"setting up server with cloud config data...");
            Console.WriteLine($"Attemtping to deseralize: {setupDataJson}");
            ServerConfigEnum = ServerConfigType.Cloud;
            CloudServerSetupData setupData = JsonConvert.DeserializeObject<CloudServerSetupData>(setupDataJson);
            CloudAuthToken = setupData.PlayerAuthenticationToken;
            ResponseCloudFunction = setupData.CloudFunctionName;

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
