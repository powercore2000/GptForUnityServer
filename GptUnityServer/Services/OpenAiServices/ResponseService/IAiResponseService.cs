using SharedLibrary;

namespace GptUnityServer.Services.OpenAiServices.ResponseService
{
    public interface IAiResponseService
    {

        public Task<AiResponse> SendMessage(string prompt);
    }

}
