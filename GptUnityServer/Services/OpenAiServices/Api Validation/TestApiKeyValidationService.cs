namespace GptUnityServer.Services.OpenAiServices
{
    public class TestApiKeyValidationService : IApiKeyValidation
    {




        public Task<bool> ValidateApiKey(string key) { return Task.FromResult(true); }





    }
}
