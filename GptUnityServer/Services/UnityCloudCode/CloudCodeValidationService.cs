using GptUnityServer.Services.ServerManagment;

namespace GptUnityServer.Services.UnityCloudCode
{
    public class CloudCodeValidationServices : IServerValidationService
    {

        public Task<bool> ValidateKey(string key)
        {

            return Task.FromResult(!string.IsNullOrEmpty(key));
        }





    }
}
