using GptToUnityServer.Services.UnityServerServices;

namespace GptToUnityServer.Services.UnityServerManager
{
    public class UnityServerManagerService : IHostedService
    {

        //private readonly IServiceProvider serviceProvider;
        private IUnityNetCoreServer selectedServerService;
        private IEnumerable<IUnityNetCoreServer> allNetCoreServers;
        public IUnityNetCoreServer CurrentServerService { get { return selectedServerService; } }

        public UnityServerManagerService(IEnumerable<IUnityNetCoreServer> _allNetCoreServers)
        {

            allNetCoreServers = _allNetCoreServers;
            DetermineSelectedServerType("UDP");
        }

        void DetermineSelectedServerType(string serverTypeCommand) {

            switch (serverTypeCommand) {

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
            Console.WriteLine("Starting Unity Server service!");           
            await selectedServerService.StartAsync(cancellationToken);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping Unity Server service!");
            selectedServerService.StopAsync(cancellationToken);
            return Task.CompletedTask;
        }
    }
}
