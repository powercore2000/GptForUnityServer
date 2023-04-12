using GptUnityServer.Services.Universal;

namespace GptUnityServer.Services._PlaceholderServices
{
    public class MockAiModelManagerService : IAiModelManager
    {
        public async Task<string[]> GetAllModels()
        {
            return new string[] { "None" };
        }
    }
}
