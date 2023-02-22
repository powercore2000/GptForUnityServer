using GptUnityServer.Services.OpenAiServices.OpenAiData;

namespace GptUnityServer.Services.OpenAiServices
{
    public class MockApiKeyValidationService : IApiKeyValidation
    {

        public Task<bool> ValidateApiKey(string key) {

            return Task.FromResult(true); }





    }
}
