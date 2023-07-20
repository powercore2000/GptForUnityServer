using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GptUnityServer.Models;
using GptUnityServer.Services.AiApiServices;

namespace GptUnityServerTests
{
    internal class AiApiTests
    {
        PromptSettings promptSettings;

        AiApiSetupData mockAiApiSetup = new AiApiSetupData();

        [SetUp]
        public void Setup()
        {
            promptSettings = new PromptSettings();
            promptSettings.temp = 0.3f;
            promptSettings.prompt = "How are you today?";
            promptSettings.max_tokens = 20;

            mockAiApiSetup.ApiResponseUrl = "";
            mockAiApiSetup.ApiChatUrl = "";
            mockAiApiSetup.ApiKey = "";
            mockAiApiSetup.ApiKeyValidationUrl = "";

        }


        [Test]
        public void Get_Models_From_Api()
        {

            var aiApiModelManager = new AiApiModelManager(mockAiApiSetup);

            string[] models = aiApiModelManager.GetAllModels().Result;
            Console.WriteLine(models);
            Assert.IsNotNull(models);
        }

        [Test]
        public void Get_Ai_Chat_From_Api()
        {
            promptSettings.Model = "gpt-3.5-turbo";
            promptSettings.chat_history = new string[]
                {
                    "system:You are a kansas city farmer",
                    "user:Who are you?",
                    "assistant:Howdy pardner! Muh name's billy bob!" ,

            };

            AiApiChatResponseService chatService = new AiApiChatResponseService(mockAiApiSetup, promptSettings);
            AiResponse response = chatService.SendMessage(promptSettings.prompt,promptSettings.chat_history).Result;
            Console.WriteLine(response.Message);
            Assert.IsNotNull(response);

        }

        [Test]
        public void Get_Ai_Response_From_Api()
        {

            AiApiResponseService responseServie = new AiApiResponseService(mockAiApiSetup,promptSettings);

            AiResponse response = responseServie.SendMessage(promptSettings.prompt).Result;
            Console.WriteLine(response.Message);
            Assert.IsNotNull(response);
        }
    }
}
