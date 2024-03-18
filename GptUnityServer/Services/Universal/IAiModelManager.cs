namespace GptUnityServer.Services.Universal
{
    using SharedLibrary;
    public interface IAiModelManager
    {

        public Task<string[]> GetAllModels();

    }
}
