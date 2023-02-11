using GptUnityServer.Models;

namespace GptUnityServer.Services.ServerSetup
{
    public class ServerSetupService : IServerSetupService
    {

        private readonly Settings settings;

        public ServerSetupService(Settings _settings) {

            settings = _settings;
        }
        public void SetUpServer(ServerSetupData data)
        {
            settings.PlayerIdToken = data.PlayerAuthenticationToken;
            settings.CloudFunctionName = data.CloudFunctionName;
        }



    }
}
