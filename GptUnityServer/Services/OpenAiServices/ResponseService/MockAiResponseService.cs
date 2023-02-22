using SharedLibrary;


namespace GptUnityServer.Services.OpenAiServices.ResponseService
{
    using Models;
    public class MockAiResponseService : IAiResponseService
    {
        private readonly Settings settings;
        public MockAiResponseService(Settings _settings)
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
