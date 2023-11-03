using SharedLibrary;


namespace GptUnityServer.Services._PlaceholderServices
{
    using GptUnityServer.Services.Universal;
    using Models;
    public class MockAiInstructService : IAiInstructService
    {
        private readonly Settings settings;
        public MockAiInstructService(Settings _settings)
        {

            settings = _settings;
        }


        public async Task<AiResponse> SendMessage(string prompt)
        {
            string jsonOutPut = "";


            AiResponse aiResponse = new AiResponse(jsonOutPut, jsonOutPut);
            string message = aiResponse.Message;


            // Print the response
            //Console.WriteLine($"Ai responds with \n {message}");
            Console.WriteLine($"\n Raw Json output: {aiResponse.JsonRaw}\n\n");
            return aiResponse;
        }


    }
}
