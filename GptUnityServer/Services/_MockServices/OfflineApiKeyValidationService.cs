using GptUnityServer.Services.Universal;

namespace GptForUnityServer.Services._MockServices
{
    public class OfflineApiKeyValidationService : IKeyValidationService
    {

        public Task<bool> ValidateKey(string key, string validationUrl)
        {
            Console.WriteLine("Offline api key validation defaulting to true!");
            return Task.FromResult(true);
        }

    }
}
