namespace GptUnityServer.Services.UniversalInterfaces
{
    using Assets.GptToUnity.SharedLibrary;
    public interface IAiModelManager
    {

        public Task<string[]> GetAllModels();

    }
}
