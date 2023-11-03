using GptUnityServer.Services.Universal;
using SharedLibrary;

namespace GptUnityServer.Services._PlaceholderServices
{
    public class MockAiChatService : IAiChatService
    {
        public Task<AiResponse> SendMessage(PromptSettings promptSettings)
        {
            throw new NotImplementedException();
        }
    }
}
