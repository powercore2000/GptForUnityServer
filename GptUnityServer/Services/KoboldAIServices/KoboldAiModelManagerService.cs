using GptUnityServer.Services.UniversalInterfaces;
using Microsoft.Extensions.ObjectPool;

namespace GptUnityServer.Services.KoboldAIServices
{
    public class KoboldAiModelManagerService : IAiModelManager
    {
        public async Task<string[]> GetAllModels()
        {
            string[] array = new string[] { "None"};

            await Task.Delay(1);

            return array;
        }
    }
}
