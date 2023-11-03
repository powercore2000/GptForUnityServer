using SharedLibrary;

namespace GptUnityServer.Services.Universal
{
    public interface IAiInstructService
    {

        public Task<AiResponse> SendMessage(string prompt);
    }

}
