namespace GptUnityServer.Services.Universal
{
    using Assets.GptToUnity.SharedLibrary;
    public interface IAiModelManager
    {

        public Task<string[]> GetAllModels();

    }
}
