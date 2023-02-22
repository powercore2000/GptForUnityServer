using GptUnityServer.Services.OpenAiServices;
using Microsoft.Extensions.Hosting.Internal;
using GptUnityServer.Services.ServerManagment.UnityServerServices;

namespace GptUnityServer.Services.ServerManagment.ServerManagerServices
{
    using GptUnityServer.Services.OpenAiServices.OpenAiData;
    using Models;
    public class UnityServerManagerService : IHostedService
    {

        //private readonly IServiceProvider serviceProvider;
        private IUnityNetCoreServer selectedServerService;
        private IEnumerable<IUnityNetCoreServer> allNetCoreServers;
        private readonly Settings settings;
        public IUnityNetCoreServer CurrentServerService { get { return selectedServerService; } }
        protected readonly IApiKeyValidation validatonService;
        protected readonly IHostApplicationLifetime applicationLifetime;
        protected readonly IOpenAiModelManager openAiModelManager;
        protected bool IsApiKeyValid { get; set; }
        public UnityServerManagerService(
            IEnumerable<IUnityNetCoreServer> _allNetCoreServers,
            Settings _settings,
            IApiKeyValidation _validationService,
            IHostApplicationLifetime _applicationLifetime,
            IOpenAiModelManager _openAiModelManager)
        {

            validatonService = _validationService;
            settings = _settings;
            allNetCoreServers = _allNetCoreServers;
            applicationLifetime = _applicationLifetime;
            openAiModelManager = _openAiModelManager;
            DetermineSelectedServerType(settings.ServerType);

        }

        void DetermineSelectedServerType(string newServerType)
        {

            Console.WriteLine($"Selected {newServerType} for server type!");
            switch (newServerType)
            {

                case "TCP":
                    {

                        selectedServerService = allNetCoreServers.Single(server => server is TcpServerService);

                    }
                    break;

                case "UDP":
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

            IsApiKeyValid = await validatonService.ValidateApiKey(settings.AiApiKey);


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
