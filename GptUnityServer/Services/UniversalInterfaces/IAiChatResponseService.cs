using SharedLibrary;

namespace GptUnityServer.Services.UniversalInterfaces
{
    public interface IAiChatResponseService
    {
        public Task<AiResponse> SendMessage(string userMessage, string[] systemMessages);
    }
}
