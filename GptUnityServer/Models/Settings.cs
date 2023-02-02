namespace GptToUnityServer.Models
{
    public class Settings
    {
        public string ApiKey { get; set; }

        public string DefaultServerType { get; set; }
        public string ServerType { get { return ServerTypeEnum.ToString(); } }

        public string AiModel { get; set; }

        //public bool IsApiKeyValid { get; set; }

        private ServerTypes ServerTypeEnum { get; set; }


        public void RunSetUp(string[] args) {

            Console.WriteLine($"All arugments:\n {string.Join("\n",args)}");
            if (args.Length >=1)
            ChangeServerType(args[0]);

            if (args.Length >= 2)
                ChangeApiKey(args[1]);

            if (args.Length >= 3)
                ChangeModel(args[2]);
        }

       void ChangeServerType(string newServerType) {

           ServerTypes serverType;
           Console.WriteLine($"Server Argument passed in was {newServerType}");
           bool validType = ServerTypes.TryParse(newServerType, out serverType);

           if (!validType)
           {
               Console.WriteLine($"Now parsing Default Server value: {DefaultServerType}");
               ServerTypeEnum = Enum.Parse<ServerTypes>(DefaultServerType);
           }
           else {
               Console.WriteLine($"Now parsing new Server value: {newServerType}");
               ServerTypeEnum = Enum.Parse<ServerTypes>(newServerType);
           }

           //OnServerTypeChange.Invoke(newServerType);
       }

       void ChangeApiKey(string newKey) {

           if (!string.IsNullOrEmpty(newKey)) {

               ApiKey = newKey;
           }
       }

        void ChangeModel(string newKey)
        {

            if (!string.IsNullOrEmpty(newKey))
            {

                AiModel = newKey;
            }
        }

        //public Action<string> OnServerTypeChange;

        private enum ServerTypes { 
        
            TCP,
            UDP
        }

    }
}
