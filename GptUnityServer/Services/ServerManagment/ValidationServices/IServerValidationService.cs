namespace GptUnityServer.Services.ServerManagment.ValidationServices
{
    public interface IServerValidationService
    {
        public Task<bool> ValidateKey(string key);
    }
}
