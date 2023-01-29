using GptToUnityServer.Models;
using GptToUnityServer.Services.UnityServerServices;
using GptUnityServer.Services.OpenAiServices;
using GptUnityServer.Services.UnityServerServices;

namespace GptToUnityServer.Services.ServerManagerServices
{
    public class UnityServerManagerService : IHostedService
    {

        //private readonly IServiceProvider serviceProvider;
        private IUnityNetCoreServer selectedServerService;
        private IEnumerable<IUnityNetCoreServer> allNetCoreServers;
        private readonly Settings settings;
        public IUnityNetCoreServer CurrentServerService { get { return selectedServerService; } }
        protected readonly IApiKeyValidation validatonService;
        protected bool IsApiKeyValid { get; set; }
        public UnityServerManagerService(IEnumerable<IUnityNetCoreServer> _allNetCoreServers, Settings _settings, IApiKeyValidation _validationService)
        {
            validatonService = _validationService;
            settings = _settings;
            allNetCoreServers = _allNetCoreServers;
            DetermineSelectedServerType(settings.ServerType);
        }

        void DetermineSelectedServerType(string newServerType) {


            switch (newServerType) {

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
                        selectedServerService = allNetCoreServers.Single(server => server is TcpServerService);
                    }
                    break;
            }
            
        }


        


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IsApiKeyValid = validatonService.ValidateApiKey(settings.ApiKey);
            Console.WriteLine($"Starting Unity Server service! \nCurrent key validation : {IsApiKeyValid}");           
            await selectedServerService.StartAsync(cancellationToken, IsApiKeyValid);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping Unity Server service!");
            selectedServerService.StopAsync(cancellationToken);
            return Task.CompletedTask;
        }
    }
}
