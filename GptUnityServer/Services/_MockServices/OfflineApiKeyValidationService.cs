using GptUnityServer.Services.Universal;

namespace GptForUnityServer.Services._MockServices
{
    public class OfflineApiKeyValidationService : IKeyValidationService
    {

        public Task<bool> ValidateKey(string key, string validationUrl)
        {

            return Task.FromResult(true);
        }

    }
}
