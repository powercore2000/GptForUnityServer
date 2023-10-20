using GptForUnityServer.Services.EmotionClassificationServices;
using GptForUnityServer.Services.Universal;
using GptUnityServer.Services.ServerProtocols;
using SharedLibrary;

namespace GptForUnityServer.Services.ServerManagment
{
    public class ServiceSelectorService
    {
        
        private ClassificationServiceTypes classificationService = ClassificationServiceTypes.Mock;
        public ClassificationServiceTypes ClassificationService => classificationService;

        private readonly IServiceProvider serviceProvider;
        private readonly IEnumerable<IEmotionClassificationService> emotionClassificationServices;

        
        public ServiceSelectorService(IServiceProvider _serviceProvider, IEnumerable<IEmotionClassificationService> _emotionClassificationServices) {

            serviceProvider = _serviceProvider;
            emotionClassificationServices = _emotionClassificationServices;
        }

        public void SetEmotionClassificationService(ClassificationServiceTypes newServiceType) {

            classificationService = newServiceType;
        }


        public IEmotionClassificationService GetEmotionClassificationService() {


            switch (classificationService) {

                case ClassificationServiceTypes.SillyTavernExtras:
                     return emotionClassificationServices.Single(server => server is SillyTavernExtraSimpleClassifyService); 
                
                case ClassificationServiceTypes.Mock:
                     return emotionClassificationServices.Single(server => server is MockEmotionClassificationService); 
                
                default:
                    return emotionClassificationServices.Single(server => server is MockEmotionClassificationService);

            }
        }
    }

}
