using SharedLibrary;

namespace GptUnityServer.Services.Universal
{
    public interface IAiResponseService
    {

        public Task<AiResponse> SendMessage(string prompt);
    }

}
