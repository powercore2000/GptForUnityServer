using SharedLibrary;

namespace GptUnityServer.Services.OpenAiServices.PromptServices
{
    public interface IOpenAiPromptService
    {

        public Task<AiResponse> SendMessage(string prompt);
    }

}
