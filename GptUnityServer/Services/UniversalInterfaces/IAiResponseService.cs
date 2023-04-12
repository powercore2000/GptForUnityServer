using SharedLibrary;

namespace GptUnityServer.Services.UniversalInterfaces
{
    public interface IAiResponseService
    {

        public Task<AiResponse> SendMessage(string prompt);
    }

}
