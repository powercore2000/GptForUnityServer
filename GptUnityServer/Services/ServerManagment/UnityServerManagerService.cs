using Microsoft.Extensions.Hosting.Internal;

namespace GptUnityServer.Services.ServerManagment
{
    using GptUnityServer.Services.NetCoreProtocol;
    //using GptUnityServer.Services.OpenAiServices.OpenAiData;
    using Models;
    public class UnityServerManagerService : IHostedService
    {

        //private readonly IServiceProvider serviceProvider;
        private IUnityNetCoreServer selectedServerService;
        private IEnumerable<IUnityNetCoreServer> allNetCoreServers;
        private readonly Settings settings;
        public IUnityNetCoreServer CurrentServerService { get { return selectedServerService; } }
        protected readonly IServerValidationService validatonService;
        protected readonly IHostApplicationLifetime applicationLifetime;
        //protected readonly IOpenAiModelManager openAiModelManager;
        protected bool IsApiKeyValid { get; set; }
        public UnityServerManagerService(
            IEnumerable<IUnityNetCoreServer> _allNetCoreServers,
            Settings _settings,
            IServerValidationService _validationService,
            IHostApplicationLifetime _applicationLifetime//,
                                                         //IOpenAiModelManager _openAiModelManager
            )
        {

            validatonService = _validationService;
            settings = _settings;
            allNetCoreServers = _allNetCoreServers;
            applicationLifetime = _applicationLifetime;
            //openAiModelManager = _openAiModelManager;
            DetermineSelectedServerType(settings.ServerProtocolEnum);

        }

        void DetermineSelectedServerType(ServerProtocolTypes newServerType)
        {

            Console.WriteLine($"Selected {newServerType} for server type!");
            switch (newServerType)
            {

                case ServerProtocolTypes.TCP:
                    {

                        selectedServerService = allNetCoreServers.Single(server => server is TcpServerService);

                    }
                    break;

                case ServerProtocolTypes.UDP:
                    {

                        selectedServerService = allNetCoreServers.Single(server => server is UdpServerService);

                    }
                    break;


                default:
                    {
                        Console.WriteLine($"Defaulted to TCP due to unrecognized server type!!");
                        selectedServerService = allNetCoreServers.Single(server => server is TcpServerService);
                    }
                    break;
            }

        }


        void DeactivateService()
        {

            applicationLifetime.StopApplication();
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string validationKey = "";
            if (settings.ServerServiceEnum == ServerServiceTypes.Api)
                validationKey = settings.AiApiKey;

            else if (settings.ServerServiceEnum == ServerServiceTypes.UnityCloudCode)
                validationKey = settings.CloudAuthToken;



            IsApiKeyValid = await validatonService.ValidateKey(validationKey);


            //Console.WriteLine($"Starting Unity Server service! \nCurrent key validation : {IsApiKeyValid}");           
            await selectedServerService.StartAsync(cancellationToken, IsApiKeyValid, DeactivateService);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping Unity Server service!");
            selectedServerService.StopAsync(cancellationToken);
            return Task.CompletedTask;
        }
    }
}
