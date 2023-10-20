using GptForUnityServer.Services.Universal;
using SharedLibrary;

namespace GptForUnityServer.Services.EmotionClassificationServices
{
    public class MockEmotionClassificationService : IEmotionClassificationService
    {
        public async Task<List<EmotionData>> ClassifyMessage(string message)
        {
            return new List<EmotionData>();
        }
     }
}
