using System;
using System.Text;
using GptToUnityServer.Models;
using Newtonsoft.Json;
using SharedLibrary;


namespace GptUnityServer.Services.OpenAiServices.PromptSending
{
    public class MockAiPromptService : IOpenAiPromptService
    {
        private readonly Settings settings;
        public MockAiPromptService(Settings _settings)
        {

            settings = _settings;
        }


        public async Task<AiResponse> SendMessage(string prompt)
        {
            string jsonOutPut = "[{\"output\":" +
                "{\"choices\":[{\"finish_reason\":\"stop\",\"index\":0,\"logprobs\":null,\"text\":\"\\n\\nI'm good, thank you. How are you?\"}]," +
                "\"created\":1675861496,\"id\":\"cmpl-6heQ4sCiXQ1szvWhOiVeaJbpiMvi9\",\"model\":\"text-davinci-002\",\"object\":\"text_completion\"," +
                "\"usage\":{\"completion_tokens\":13,\"prompt_tokens\":4,\"total_tokens\":17}}}\r\ndid it work? " +
                "{\"output\":{\"choices\":[{\"finish_reason\":\"stop\",\"index\":0,\"logprobs\":null,\"text\":\"\\n\\nI'm good, thank you. How are you?\"}]," +
                "\"created\":1675861496,\"id\":\"cmpl-6heQ4sCiXQ1szvWhOiVeaJbpiMvi9\",\"model\":\"text-davinci-002\",\"object\":\"text_completion\"," +
                "\"usage\":{\"completion_tokens\":13,\"prompt_tokens\":4,\"total_tokens\":17}}}]";

       
            AiResponse aiResponse = new AiResponse(jsonOutPut);
            string message = aiResponse.Message;


            // Print the response
            Console.WriteLine($"Ai responds with \n {message}");
            Console.WriteLine($"\n Raw Json output: {aiResponse.JsonRaw}\n\n");
            return aiResponse;
        }


    }
}
