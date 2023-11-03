using SharedLibrary;

namespace GptUnityServer.Services.Universal
{
    public interface IAiChatService
    {
        public Task<AiResponse> SendMessage(PromptSettings promptSettings);
    }
}
