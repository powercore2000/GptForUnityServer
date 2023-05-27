namespace GptUnityServer.Services.Universal
{
    public interface IKeyValidationService
    {
        public Task<bool> ValidateKey(string key, string validationUrl);
    }
}
