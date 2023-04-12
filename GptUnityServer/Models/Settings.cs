using Newtonsoft.Json;

namespace GptUnityServer.Models
{
    using SharedLibrary;
    public class Settings
    {
        #region Fields


        /// <summary>
        /// Only populated in test or debug senarios!
        /// </summary>
        public string? AiApiKey { get; set; }

        #region Universal Server Fields
       
        /// <summary>
        /// The protocol type to fall back to if none is provides when server is initalized, or started in dev mode.
        /// </summary>
        public string? DefaultProtocolType { get; set; }
        
        /// <summary>
        /// The config type to fall back to if none is provides when server is initalized, or started in dev mode.
        /// </summary>
        public string? DefaultConfigType { get; set; }
        
       // public string ServerProtocolType { get { return ServerProtocolEnum.ToString(); } }
        //public string ServerServiceType { get { return ServerServiceEnum.ToString(); } }

        public ServerProtocolTypes ServerProtocolEnum { get; private set; }

        public ServerServiceTypes ServerServiceEnum { get; private set; }
        #endregion

        #region Fields for Unity Cloud Code
        /// <summary>
        /// JSON Web Token to indicate authentication privledges for cloud function calls
        /// </summary>
        public string? CloudAuthToken { get; set; }

        /// <summary>
        /// The id of the organization your going to call. MUST BE SET MANUALLY IN APP SETTINGS
        /// </summary>
        public string? CloudProjectId { get; set; }
        /// <summary>
        /// Endpoint name of response based cloud functions
        /// </summary>
        public string? ResponseCloudFunction { get; set; }

        /// <summary>
        /// Endpoint name of response chat based cloud function
        /// </summary>
        public string? ChatCloudFunction { get; set; }

        /// <summary>
        /// Endpoint name of for cloud funtion that retrives the model listing
        /// </summary>
        public string? ModelListCloudFunction { get; set; }
        #endregion

        //public bool IsApiKeyValid { get; set; }


        #endregion

        #region Initalization Methods
        
        /// <summary>
        /// Parses data passed in via Program.Start() argument parameters to handle the server's protocol, config, and operation data
        /// </summary>
        /// <param name="args"></param>
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
                SetServerServices(args[1]);            

            if (args.Length >= 3)
                SetServerAuthenticationData(args[2]);
        }

        /// <summary>
        /// If no initalizaton arguments are provided when the server starts up, trigger dev mode and use default data provided in AppSettings and Secrets
        /// </summary>
        void StartDefaultDevMode()
        {
            SetServerProtocol(DefaultProtocolType);
            SetServerServices(DefaultConfigType);
            //SetServerData is not called because the data will be filled from enviroment secrets
            Console.WriteLine($"Using default dev values! In {ServerServiceEnum} config");

        }
        #endregion

        #region Server Setup Methods
        /// <summary>
        /// Sets the connection protocol 
        /// </summary>
        /// <param name="newServerType"></param>
        void SetServerProtocol(string newServerType)
        {

            ServerProtocolTypes serverType;
            bool validType = ServerProtocolTypes.TryParse(newServerType, out serverType);

            if (!validType)
            {
                ServerProtocolEnum = Enum.Parse<ServerProtocolTypes>(newServerType);
            }
            else
            {
                ServerProtocolEnum = Enum.Parse<ServerProtocolTypes>(newServerType);
            }

            Console.WriteLine($"Set server protocol to: {ServerProtocolEnum}");

            //OnServerTypeChange.Invoke(newServerType);
        }
        /// <summary>
        /// Sets the configuration type of the server to either be Cloud Based, API Based, or default
        /// </summary>
        /// <param name="newServiceType"></param>
        void SetServerServices(string newServiceType)
        {

            ServerServiceTypes targetServerService;
            bool validType = Enum.TryParse(newServiceType, out targetServerService);

            if (validType)            
                ServerServiceEnum = targetServerService;
            
            else
            {
                Console.WriteLine($"ERROR: Unrecognized service type passed into Program.Start()\nNo Server service types corresponds to {newServiceType}");
            }

            Console.WriteLine($"Setting server service to: {ServerServiceEnum}");

            //OnServerTypeChange.Invoke(newServerType);
        }

        /// <summary>
        /// Sets the authentication data needed for the server to make web requests:
        /// API Mode: Data contains the API Key used
        /// Cloud Mode: Data is a JSON object containing the names of the endpoint functions used, and authentication data
        /// </summary>
        /// <param name="data"></param>
        void SetServerAuthenticationData(string data)
        {

            if (string.IsNullOrEmpty(data))
                return;

            if (ServerServiceEnum == ServerServiceTypes.OpenAi)
                AiApiKey = data;

            else if (ServerServiceEnum == ServerServiceTypes.UnityCloudCode && data.StartsWith("{") && data.EndsWith("}"))
            {
               
                Console.WriteLine($"Attemtping to deseralize CloudServerSetupData: {data}");
                CloudServerSetupData setupData = JsonConvert.DeserializeObject<CloudServerSetupData>(data);
                CloudAuthToken = setupData.PlayerAuthenticationToken;
                ResponseCloudFunction = setupData.ResponseFunctionName;
                ChatCloudFunction = setupData.ChatFunctionName;
                ModelListCloudFunction = setupData.ModelListCloudFunction;
            }

        }
        #endregion



    }

    #region Enums
    public enum ServerProtocolTypes
    {

        TCP,
        UDP
    }

    public enum ServerServiceTypes
    {
        /// <summary>
        /// Makes calls to Open Ai's Api directly. Should NOT be used on distributted software
        /// </summary>
        OpenAi,
        
        /// <summary>
        /// Makes calls to Unity's Clode Code Api. Used as a template for other cloud service support
        /// </summary>
        UnityCloudCode,

        /// <summary>
        /// Makes calls to a local instance of Oobabooga's text-generation-webui Api. 
        /// </summary>
        OobaUi,

        /// <summary>
        /// Makes calls to a local instance of Kobold Ai's Api
        /// </summary>
        KoboldAi
    }
    #endregion

}
