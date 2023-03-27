namespace GptUnityServer.Services.ServerManagment.ValidationServices
{
    public class CloudCodeValidationServices : IServerValidationService
    {

        public Task<bool> ValidateKey(string key)
        {

            return Task.FromResult(!string.IsNullOrEmpty(key));
        }





    }
}
