using GptUnityServer.Services.ServerManagment;

namespace GptUnityServer.Services._PlaceholderServices
{
    public class MockApiKeyValidationService : IServerValidationService
    {

        public Task<bool> ValidateKey(string key, string validationUrl)
        {

            return Task.FromResult(true);
        }





    }
}
