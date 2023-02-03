namespace GptUnityServer.Services.OpenAiServices
{
    public interface IApiKeyValidation
    {
        public Task<bool> ValidateApiKey(string key);
    }
}
