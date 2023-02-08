using SharedLibrary;

namespace GptUnityServer.Services.OpenAiServices.PromptSending
{
    public interface IOpenAiPromptService
    {

        public Task<AiResponse> SendMessage(string prompt);
    }

}
