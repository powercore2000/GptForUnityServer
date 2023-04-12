using GptUnityServer.Services.UniversalInterfaces;
using Microsoft.Extensions.ObjectPool;

namespace GptUnityServer.Services.OobaUiServices
{
    public class OobaUiModelManagerService : IAiModelManager
    {
        public async Task<string[]> GetAllModels()
        {
            string[] array = new string[] { "None"};

            await Task.Delay(1);

            return array;
        }
    }
}
