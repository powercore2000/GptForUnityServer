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
        public AiChatResponseServiceTypes AiChatResponseService { get; private set; } = AiChatResponseServiceTypes.Mock;

        public PromptSettingServiceTypes PromptSettingService { get; private set; } = PromptSettingServiceTypes.Default;

        public bool IsApiKeyValid { get; private set; }
        private bool hasSetApiKey = false;

        private readonly IServiceProvider serviceProvider;
        //private IUnityProtocolServer selectedServerService;

        private readonly IEnumerable<IEmotionClassificationService> emotionClassificationServices;
        private readonly IEnumerable<IAiModelManager> aiModelManagerServices;
        private readonly IEnumerable<IKeyValidationService> keyValidationServices;
        private readonly IEnumerable<IAiChatResponseService> aiChatResponseServices;
        private readonly IEnumerable<IAiResponseService> aiResponseServices;
        private readonly IEnumerable<IPromptSettingsService> promptSettingService;
        private readonly IEnumerable<IUnityProtocolServer> protocolServerServices;

        public ModularServiceSelector(
            IServiceProvider _serviceProvider, 
            IEnumerable<IEmotionClassificationService> _emotionClassificationServices,
            IEnumerable<IAiModelManager> _aiModelManagerServices,
            IEnumerable<IKeyValidationService> _keyValidationServices,
            IEnumerable<IAiChatResponseService> _aiChatResponseServices,
            IEnumerable<IAiResponseService> _aiResponseServices,
            IEnumerable<IPromptSettingsService> _promptSettingService
            
            ) {

            serviceProvider = _serviceProvider;
            emotionClassificationServices = _emotionClassificationServices;
            aiModelManagerServices = _aiModelManagerServices;
            keyValidationServices = _keyValidationServices;
            aiChatResponseServices = _aiChatResponseServices;
            aiResponseServices = _aiResponseServices;
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

        public void SetAiChatResponseService(AiChatResponseServiceTypes newServiceType)
        {

            AiChatResponseService = newServiceType;
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

        public IAiChatResponseService GetAiChatResponseService()
        {


            switch (AiChatResponseService)
            {

                case AiChatResponseServiceTypes.AiApi:
                    return aiChatResponseServices.Single(server => server is AiApiChatResponseService);

                case AiChatResponseServiceTypes.Cloud:
                    return aiChatResponseServices.Single(server => server is CloudChatResponseService);

                case AiChatResponseServiceTypes.OobaUi:
                    return aiChatResponseServices.Single(server => server is OobaUiChatService);

                case AiChatResponseServiceTypes.Kobold:
                    return aiChatResponseServices.Single(server => server is KoboldAIChatService);

                default:
                    return aiChatResponseServices.Single(server => server is MockAiChatService);

            }
        }

        public IAiResponseService GetAiResponseService()
        {


            switch (AiChatResponseService)
            {

                case AiChatResponseServiceTypes.AiApi:
                    return aiResponseServices.Single(server => server is AiApiResponseService);

                case AiChatResponseServiceTypes.Cloud:
                    return aiResponseServices.Single(server => server is CloudResponseService);

                case AiChatResponseServiceTypes.OobaUi:
                    return aiResponseServices.Single(server => server is OobaUiResponseService);

                case AiChatResponseServiceTypes.Kobold:
                    return aiResponseServices.Single(server => server is KoboldAiResponseService);

                default:
                    return aiResponseServices.Single(server => server is MockAiResponseService);

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
