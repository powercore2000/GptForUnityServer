using Microsoft.Extensions.Hosting.Internal;

namespace GptUnityServer.Services.ServerManagment
{
    using System;
    using SharedLibrary;
    using GptUnityServer.Controllers;
    using GptUnityServer.Services.ServerProtocols;
    using GptUnityServer.Services.Universal;
    //using GptUnityServer.Services.OpenAiServices.OpenAiData;
    using Models;
    public class UnityServerManagerService : IHostedService
    {
        private static bool hasCheckedForValidation = false;
        //private readonly IServiceProvider serviceProvider;
        private IUnityProtocolServer selectedServerService;
        private IEnumerable<IUnityProtocolServer> allProtocolServers;
        private readonly Settings settings;
        private readonly AiApiSetupData aiApiSetupData;
        private readonly UnityCloudSetupData UnityCloudSetupData;
        public IUnityProtocolServer CurrentServerService { get { return selectedServerService; } }
        protected readonly IKeyValidationService validatonService;
        protected readonly IHostApplicationLifetime applicationLifetime;
        //protected readonly IOpenAiModelManager openAiModelManager;
        protected bool IsApiKeyValid { get; set; }
        public UnityServerManagerService(
            IEnumerable<IUnityProtocolServer> _allProtocolServers,
            Settings _settings,
            AiApiSetupData _aiApiSetupData,
            UnityCloudSetupData _UnityCloudSetupData,
            IKeyValidationService _validationService,
            IHostApplicationLifetime _applicationLifetime
            )
        {

            validatonService = _validationService;
            settings = _settings;
            aiApiSetupData = _aiApiSetupData;
            UnityCloudSetupData = _UnityCloudSetupData;
            allProtocolServers = _allProtocolServers;
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

                        selectedServerService = allProtocolServers.Single(server => server is TcpServerService);

                    }
                    break;

                case ServerProtocolTypes.UDP:
                    {

                        selectedServerService = allProtocolServers.Single(server => server is UdpServerService);

                    }
                    break;
                case ServerProtocolTypes.HTTP:
                    {
                        Console.WriteLine("Rest Api Detected, grabbing API Controller Server...");
                        selectedServerService = allProtocolServers.Single(server => server is RestApiServerService);

                    }
                    break;

                default:
                    {
                        Console.WriteLine($"Defaulted to TCP due to unrecognized server type!!");
                        selectedServerService = allProtocolServers.Single(server => server is TcpServerService);
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
            if (!hasCheckedForValidation)
            {
                string validationKey = "";
                string validationUrl = "";
                if (settings.ServerServiceEnum == ServerServiceTypes.AiApi)
                {
                    validationKey = aiApiSetupData.ApiKey;
                    validationUrl = aiApiSetupData.ApiKeyValidationUrl;
                }

                else if (settings.ServerServiceEnum == ServerServiceTypes.UnityCloud)
                {
                    validationKey = UnityCloudSetupData.UnityCloudPlayerToken;
                    validationUrl = "https://cloud-code.services.api.unity.com/v1/projects" + $"/{UnityCloudSetupData.UnityCloudProjectId}/{UnityCloudSetupData.UnityCloudEndpoint}/{UnityCloudSetupData.UnityCloudModelsFunction}";
                }


                IsApiKeyValid = await validatonService.ValidateKey(validationKey, validationUrl);
                hasCheckedForValidation = true;
                //Console.WriteLine($"Starting Unity Server service! \nCurrent key validation : {IsApiKeyValid}");         
            }
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
