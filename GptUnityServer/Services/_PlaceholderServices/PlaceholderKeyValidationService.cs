using GptUnityServer.Services.ServerManagment.ValidationServices;

namespace GptUnityServer.Services._PlaceholderServices
{
    public class PlaceholderKeyValidationService : IServerValidationService
    {
        public async Task<bool> ValidateKey(string key)
        {
            return true;
        }
    }
}
