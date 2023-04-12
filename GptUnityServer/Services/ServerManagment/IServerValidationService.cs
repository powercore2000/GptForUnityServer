namespace GptUnityServer.Services.ServerManagment
{
    public interface IServerValidationService
    {
        public Task<bool> ValidateKey(string key);
    }
}
