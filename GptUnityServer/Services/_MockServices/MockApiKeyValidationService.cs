using GptUnityServer.Services.Universal;

namespace GptUnityServer.Services._PlaceholderServices
{
    public class MockApiKeyValidationService : IKeyValidationService
    {

        public Task<bool> ValidateKey(string key, string validationUrl)
        {

            return Task.FromResult(true);
        }





    }
}
