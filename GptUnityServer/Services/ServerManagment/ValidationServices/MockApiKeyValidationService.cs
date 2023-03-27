namespace GptUnityServer.Services.ServerManagment.ValidationServices
{
    public class MockApiKeyValidationService : IServerValidationService
    {

        public Task<bool> ValidateKey(string key)
        {

            return Task.FromResult(true);
        }





    }
}
