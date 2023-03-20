using SharedLibrary;

namespace GptUnityServer.Services.OpenAiServices.ChatResponseService
{
    public interface IAiChatResponseService
    {
        public Task<AiResponse> SendMessage(string userMessage, string[] systemMessages);
    }
}
