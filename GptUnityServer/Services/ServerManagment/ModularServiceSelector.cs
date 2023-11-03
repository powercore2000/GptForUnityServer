using GptForUnityServer.Services._MockServices;
using GptForUnityServer.Services.EmotionClassificationServices;
using GptForUnityServer.Services.Universal;
using GptUnityServer.Services._PlaceholderServices;
using GptUnityServer.Services.AiApiServices;
using GptUnityServer.Services.KoboldAIServices;
using GptUnityServer.Services.OobaUiServices;
using GptUnityServer.Services.ServerProtocols;
using GptUnityServer.Services.UnityCloud;
using GptUnityServer.Services.Universal;
using SharedLibrary;

namespace GptForUnityServer.Services.ServerManagment
{
    public class ModularServiceSelector
    {

        public ClassificationServiceTypes ClassificationService { get; private set; } = ClassificationServiceTypes.Mock;
        public ModelManagerServiceTypes ModelManagerService { get; private set; } = ModelManagerServiceTypes.Mock;
        public KeyValidationServiceTypes KeyValidationService { get; private set; } = KeyValidationServiceTypes.Offline;
        public AiChatServiceTypes AiChatService { get; private set; } = AiChatServiceTypes.Mock;
        public AiChatServiceTypes AiInstructService { get; private set; } = AiChatServiceTypes.Mock;
        public PromptSettingServiceTypes PromptSettingService { get; private set; } = PromptSettingServiceTypes.Default;

        public bool IsApiKeyValid { get; private set; }
        private bool hasSetApiKey = false;

        private readonly IServiceProvider serviceProvider;
        //private IUnityProtocolServer selectedServerService;

        private readonly IEnumerable<IEmotionClassificationService> emotionClassificationServices;
        private readonly IEnumerable<IAiModelManager> aiModelManagerServices;
        private readonly IEnumerable<IKeyValidationService> keyValidationServices;
        private readonly IEnumerable<IAiChatService> aiChatServices;
        private readonly IEnumerable<IAiInstructService> aiInstructServices;
        private readonly IEnumerable<IPromptSettingsService> promptSettingService;
        private readonly IEnumerable<IUnityProtocolServer> protocolServerServices;

        public ModularServiceSelector(
            IServiceProvider _serviceProvider, 
            IEnumerable<IEmotionClassificationService> _emotionClassificationServices,
            IEnumerable<IAiModelManager> _aiModelManagerServices,
            IEnumerable<IKeyValidationService> _keyValidationServices,
            IEnumerable<IAiChatService> _aiChatServices,
            IEnumerable<IAiInstructService> _aiResponseServices,
            IEnumerable<IPromptSettingsService> _promptSettingService
            
            ) {

            serviceProvider = _serviceProvider;
            emotionClassificationServices = _emotionClassificationServices;
            aiModelManagerServices = _aiModelManagerServices;
            keyValidationServices = _keyValidationServices;
            aiChatServices = _aiChatServices;
            aiInstructServices = _aiResponseServices;
            promptSettingService = _promptSettingService;
        }

        public void SetEmotionClassificationService(ClassificationServiceTypes newServiceType) {

            ClassificationService = newServiceType;
        }

        public void SetModelManagerService(ModelManagerServiceTypes newServiceType)
        {

            ModelManagerService = newServiceType;
        }

        public void SetKeyValidationService(KeyValidationServiceTypes newServiceType)
        {

            KeyValidationService = newServiceType;
        }
        public void SetAiChatService(AiChatServiceTypes newServiceType)
        {

            AiChatService = newServiceType;
        }

        public void SetAiInstructService(AiChatServiceTypes newServiceType)
        {

            AiInstructService = newServiceType;
        }

        public IEmotionClassificationService GetEmotionClassificationService() {


            switch (ClassificationService) {

                case ClassificationServiceTypes.SillyTavernExtras:
                     return emotionClassificationServices.Single(server => server is SillyTavernExtraSimpleClassifyService); 
                
                case ClassificationServiceTypes.Mock:
                     return emotionClassificationServices.Single(server => server is MockEmotionClassificationService); 
                
                default:
                    return emotionClassificationServices.Single(server => server is MockEmotionClassificationService);

            }
        }

        public IAiModelManager GetModelManagerService()
        {


            switch (ModelManagerService)
            {

                case ModelManagerServiceTypes.AiApi:
                    return aiModelManagerServices.Single(server => server is AiApiModelManager);

                case ModelManagerServiceTypes.Cloud:
                    return aiModelManagerServices.Single(server => server is CloudModelManager);

                case ModelManagerServiceTypes.Mock:
                    return aiModelManagerServices.Single(server => server is MockAiModelManagerService);

                default:
                    return aiModelManagerServices.Single(server => server is MockAiModelManagerService);

            }
        }

        public bool RunKeyValidationService(string key, string validationUrl) {
           
            if (!hasSetApiKey)
            {
                IsApiKeyValid = GetKeyValidationService().ValidateKey(key, validationUrl).Result;
            }
            hasSetApiKey = true;
            return IsApiKeyValid;
        }
        private IKeyValidationService GetKeyValidationService()
        {


            switch (KeyValidationService)
            {

                case KeyValidationServiceTypes.Offline:
                    return keyValidationServices.Single(server => server is OfflineApiKeyValidationService);

                case KeyValidationServiceTypes.AiApi:
                    return keyValidationServices.Single(server => server is AiApiKeyValidationService);

                case KeyValidationServiceTypes.Cloud:
                    return keyValidationServices.Single(server => server is CloudCodeValidationServices);

                default:
                    return keyValidationServices.Single(server => server is OfflineApiKeyValidationService);

            }
        }

        public IAiChatService GetAiChatService()
        {


            switch (AiChatService)
            {

                case AiChatServiceTypes.AiApi:
                    return aiChatServices.Single(server => server is AiApiChatService);
                        
                case AiChatServiceTypes.UnityCloud:
                    return aiChatServices.Single(server => server is CloudChatService);

                case AiChatServiceTypes.OobaUi:
                    return aiChatServices.Single(server => server is OobaUiChatService);

                case AiChatServiceTypes.KoboldAi:
                    return aiChatServices.Single(server => server is KoboldAIChatService);

                default:
                    return aiChatServices.Single(server => server is MockAiChatService);

            }
        }

        public IAiInstructService GetAiInstructService()
        {


            switch (AiInstructService)
            {

                case AiChatServiceTypes.AiApi:
                    return aiInstructServices.Single(server => server is AiApiInstructService);

                case AiChatServiceTypes.UnityCloud:
                    return aiInstructServices.Single(server => server is CloudInstructService);

                case AiChatServiceTypes.OobaUi:
                    return aiInstructServices.Single(server => server is OobaUiInstructService);

                case AiChatServiceTypes.KoboldAi:
                    return aiInstructServices.Single(server => server is KoboldAiInstructService);

                default:
                    return aiInstructServices.Single(server => server is MockAiInstructService);

            }
        }

        public IPromptSettingsService GetPromptSettingsService() {

            switch (PromptSettingService)
            {

                case PromptSettingServiceTypes.Default:
                    return promptSettingService.Single(server => server is PromptSettingsService);


                default:
                    return promptSettingService.Single(server => server is PromptSettingsService);

            }
        }

    }

}
