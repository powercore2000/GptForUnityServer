using Newtonsoft.Json;

namespace GptUnityServer.Models
{
    using SharedLibrary;
    public class Settings
    {
        #region Fields


        #region Universal Server Fields

        /// <summary>
        /// The protocol type to fall back to if none is provides when server is initalized, or started in dev mode.
        /// </summary>
        public string? DefaultProtocolType { get; set; }
        
        /// <summary>
        /// The config type to fall back to if none is provides when server is initalized, or started in dev mode.
        /// </summary>
        public string? DefaultServiceType { get; set; }
        
       // public string ServerProtocolType { get { return ServerProtocolEnum.ToString(); } }
        //public string ServerServiceType { get { return ServerServiceEnum.ToString(); } }

        public ServerProtocolTypes ServerProtocolEnum { get; private set; }

        public ServerServiceTypes ServerServiceEnum { get; private set; }
        #endregion

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

        }

        /// <summary>
        /// If no initalizaton arguments are provided when the server starts up, trigger dev mode and use default data provided in AppSettings and Secrets
        /// </summary>
        void StartDefaultDevMode()
        {
            SetServerProtocol(DefaultProtocolType);
            SetServerServices(DefaultServiceType);
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
                ServerProtocolEnum = Enum.Parse<ServerProtocolTypes>(DefaultProtocolType);
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


        #endregion



    }


}
