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
    using GptForUnityServer.Services.ServerManagment;
    using Microsoft.Extensions.Logging;

    public class UnityServerManagerService : IHostedService
    {
        private static bool hasCheckedForValidation = false;
        private IUnityProtocolServer selectedServerProtocolService;
        private IEnumerable<IUnityProtocolServer> allProtocolServers;
        private readonly IEnumerable<IUnityProtocolServer> protocolServerServices;
        private readonly Settings settings;
        private readonly AiApiSetupData aiApiSetupData;
        private readonly UnityCloudSetupData UnityCloudSetupData;
        private readonly ModularServiceSelector modularServiceSelector;
        public IUnityProtocolServer CurrentServerService { get { return selectedServerProtocolService; } }
        protected readonly IHostApplicationLifetime applicationLifetime;
        //protected readonly IOpenAiModelManager openAiModelManager;
        public bool IsApiKeyValid { get; private set; }
        public UnityServerManagerService(
            IEnumerable<IUnityProtocolServer> _allProtocolServers,
            Settings _settings,
            AiApiSetupData _aiApiSetupData,
            UnityCloudSetupData _UnityCloudSetupData,
            ModularServiceSelector _modularServiceSelector,
            IHostApplicationLifetime _applicationLifetime,
            IEnumerable<IUnityProtocolServer> _protocolServerServices
            )
        {
            settings = _settings;
            aiApiSetupData = _aiApiSetupData;
            UnityCloudSetupData = _UnityCloudSetupData;
            allProtocolServers = _allProtocolServers;
            applicationLifetime = _applicationLifetime;
            modularServiceSelector = _modularServiceSelector;
            protocolServerServices = _protocolServerServices;         
            
        }

        void DeactivateService()
        {

            applicationLifetime.StopApplication();
        }

        public void SetUnityServerService(ServerProtocolTypes newServerType)
        {
            selectedServerProtocolService?.StopAsync(CancellationToken.None);
            Console.WriteLine($"Selected {newServerType} for server type!");
            switch (newServerType)
            {

                case ServerProtocolTypes.TCP:
                    {

                        selectedServerProtocolService = protocolServerServices.Single(server => server is TcpServerService);

                    }
                    break;

                case ServerProtocolTypes.UDP:
                    {

                        selectedServerProtocolService = protocolServerServices.Single(server => server is UdpServerService);

                    }
                    break;
                case ServerProtocolTypes.HTTP:
                    {
                        Console.WriteLine("Rest Api Detected, grabbing API Controller Server...");
                        selectedServerProtocolService = protocolServerServices.Single(server => server is RestApiServerService);

                    }
                    break;

                default:
                    {
                        Console.WriteLine($"Defaulted to TCP due to unrecognized server type!!");
                        selectedServerProtocolService = protocolServerServices.Single(server => server is TcpServerService);
                    }
                    break;
            }

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //logger.Log("Starting");
            await StartServerService();

        }

        private async Task StartServerService()
        {
            Console.WriteLine("Starting new async server");           

            SetUnityServerService(settings.ServerProtocolEnum);

            string validationKey = "";
            string validationUrl = "";
            modularServiceSelector.SetKeyValidationService(KeyValidationServiceTypes.Offline);
            modularServiceSelector.SetAiChatService(settings.ServerServiceEnum);
            modularServiceSelector.SetAiInstructService(settings.ServerServiceEnum);
            modularServiceSelector.SetEmotionClassificationService(settings.ClassificationServiceEnum);

            if (settings.ServerServiceEnum == AiChatServiceTypes.AiApi)
            {
                modularServiceSelector.SetKeyValidationService(KeyValidationServiceTypes.AiApi);
                validationKey = aiApiSetupData.ApiKey;
                validationUrl = aiApiSetupData.ApiKeyValidationUrl;
            }

            else if (settings.ServerServiceEnum == AiChatServiceTypes.UnityCloud)
            {
                modularServiceSelector.SetKeyValidationService(KeyValidationServiceTypes.Cloud);
                validationKey = UnityCloudSetupData.UnityCloudPlayerToken;
                validationUrl = "https://cloud-code.services.api.unity.com/v1/projects" + $"/{UnityCloudSetupData.UnityCloudProjectId}/{UnityCloudSetupData.UnityCloudEndpoint}/{UnityCloudSetupData.UnityCloudModelsFunction}";
            }


            IsApiKeyValid = modularServiceSelector.RunKeyValidationService(validationKey, validationUrl);
            //Console.WriteLine($"Starting Unity Server service! \nCurrent key validation : {IsApiKeyValid}");         

            await selectedServerProtocolService.StartAsync(CancellationToken.None, IsApiKeyValid, DeactivateService);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping Unity Server service!");
            selectedServerProtocolService.StopAsync(cancellationToken);
            return Task.CompletedTask;
        }
    }
}
