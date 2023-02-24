using Newtonsoft.Json;

namespace GptUnityServer.Models
{
    public class Settings
    {
        #region Fields
        public string? AiApiKey { get; set; }

        public string? DefaultProtocolType { get; set; }
        public string? DefaultConfigType { get; set; }
        public string ServerType { get { return ServerTypeEnum.ToString(); } }
        public string ServerConfig { get { return ServerConfigEnum.ToString(); } }
        /// <summary>
        /// JSON Web Token to indicate authentication privledges for serverless function calls
        /// </summary>
        public string? CloudAuthToken { get; set; }

        /// <summary>
        /// The id of the organization your going to call
        /// </summary>
        public string? CloudProjectId { get; set; }

        public string? ResponseCloudFunction { get; set; }
        public string? ModelListCloudFunction { get; set; }

        //public bool IsApiKeyValid { get; set; }

        private ServerProtocolTypes ServerTypeEnum { get; set; }
        private ServerConfigType ServerConfigEnum { get; set; }

        #endregion

        #region Initalization Methods
        public void RunSetUp(string[] args)
        {

            Console.WriteLine($"All arugments:\n {string.Join("\n", args)}");
            if (args.Length == 0)
            {

                StartDefaultDevMode();
                return;
            }


            if (args.Length >= 1)
                SetServerProtocol(args[0]);

            if (args.Length >= 2)           
                SetServerConfig(args[1]);            

            if (args.Length >= 3)
                SetServerData(args[2]);
        }

        void StartDefaultDevMode()
        {
            SetServerProtocol(DefaultProtocolType);
            SetServerConfig(DefaultConfigType);
            //SetServerData is not called because the data will be filled from enviroment secrets
            Console.WriteLine($"Using default dev values! In {ServerConfigEnum} config");

        }
        #endregion

        #region Server Setup Methods
        void SetServerProtocol(string newServerType)
        {

            ServerProtocolTypes serverType;
            bool validType = ServerProtocolTypes.TryParse(newServerType, out serverType);

            if (!validType)
            {
                ServerTypeEnum = Enum.Parse<ServerProtocolTypes>(newServerType);
            }
            else
            {
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

        void SetServerData(string data)
        {

            if (string.IsNullOrEmpty(data))
                return;

            if (ServerConfigEnum == ServerConfigType.Api)
                AiApiKey = data;

            else if (ServerConfigEnum == ServerConfigType.Cloud && data.StartsWith("{") && data.EndsWith("}"))
            {
               
                Console.WriteLine($"Attemtping to deseralize cloud data: {data}");
                CloudServerSetupData setupData = JsonConvert.DeserializeObject<CloudServerSetupData>(data);
                CloudAuthToken = setupData.PlayerAuthenticationToken;
                ResponseCloudFunction = setupData.CloudFunctionName;

            }

        }
        #endregion

        #region Enums
        private enum ServerProtocolTypes {

            TCP,
            UDP
        }

        private enum ServerConfigType
        {

            Api,
            Cloud
        }
        #endregion


    }
}
