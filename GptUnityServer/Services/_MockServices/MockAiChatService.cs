using GptUnityServer.Services.Universal;
using SharedLibrary;

namespace GptUnityServer.Services._PlaceholderServices
{
    public class MockAiChatService : IAiChatResponseService
    {
        public Task<AiResponse> SendMessage(PromptSettings promptSettings)
        {
            throw new NotImplementedException();
        }
    }
}
