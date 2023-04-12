using GptUnityServer.Services.Universal;
using SharedLibrary;

namespace GptUnityServer.Services._PlaceholderServices
{
    public class MockChatService : IAiChatResponseService
    {
        public Task<AiResponse> SendMessage(string userMessage, string[] systemMessages)
        {
            throw new NotImplementedException();
        }
    }
}
