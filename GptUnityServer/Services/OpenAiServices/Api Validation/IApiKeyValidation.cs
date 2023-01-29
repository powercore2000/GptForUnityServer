namespace GptUnityServer.Services.OpenAiServices
{
    public interface IApiKeyValidation
    {
        public bool ValidateApiKey(string key);
    }
}
