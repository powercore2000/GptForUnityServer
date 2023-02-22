namespace GptUnityServer.Services.OpenAiServices.OpenAiData
{
    using Assets.GptToUnity.SharedLibrary;
    public interface IOpenAiModelManager
    {

        public Task<string[]> GetAllModels();

    }
}
