using SharedLibrary;

namespace GptUnityServer.Services.Universal
{
    public interface IAiChatResponseService
    {
        public Task<AiResponse> SendMessage(string userMessage, string[] systemMessages);
    }
}
