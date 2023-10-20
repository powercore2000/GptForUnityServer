using SharedLibrary;

namespace GptForUnityServer.Services.Universal
{
    public interface IEmotionClassificationService
    {
        public Task<List<EmotionData>> ClassifyMessage(string message);
    }
}
