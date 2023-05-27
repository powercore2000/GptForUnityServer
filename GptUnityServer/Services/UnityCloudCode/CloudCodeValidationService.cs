using GptUnityServer.Services.Universal;

namespace GptUnityServer.Services.UnityCloudCode
{
    public class CloudCodeValidationServices : IKeyValidationService
    {

        public Task<bool> ValidateKey(string key, string valdiationUrl)
        {
            return Task.FromResult(!string.IsNullOrEmpty(key));
        }





    }
}
