namespace GptUnityServer.Services.OpenAiServices.OpenAiData.ModelListing
{
    using Assets.GptToUnity.SharedLibrary;
    public interface IOpenAiModelManager
    {

        public Task<ModelData[]> GetAllModels();

    }
}
