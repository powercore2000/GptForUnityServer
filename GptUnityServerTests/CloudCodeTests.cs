using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GptUnityServer.Services.AiApiServices;
using GptUnityServer.Services.UnityCloud;

namespace GptUnityServerTests
{
    internal class CloudCodeTests
    {
        PromptSettings promptSettings = new PromptSettings();

        UnityCloudSetupData mockCloudCodeSetup = new UnityCloudSetupData();
        [SetUp]
        public void Setup()
        {

            promptSettings.temp = 0.3f;
            promptSettings.prompt = "How are you today?";
            promptSettings.max_tokens = 20;

            /*
            mockCloudCodeSetup.UnityCloudPlayerToken = "";
            mockCloudCodeSetup.UnityCloudResponseFunction = "";
            mockCloudCodeSetup.UnityCloudChatFunction = "";
            mockCloudCodeSetup.UnityCloudModelsFunction = "";
            mockCloudCodeSetup.UnityCloudProjectId = "";
            mockCloudCodeSetup.UnityCloudEndpoint = "";
            */
            mockCloudCodeSetup.UnityCloudPlayerToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6InB1YmxpYzpBNTYwOTVEQS0xODJDLTQ1MjMtOUQyNS1DNzlEMzNBNEY5OUIiLCJ0eXAiOiJKV1QifQ.eyJhdWQiOlsiaWRkOmQ4Njg2ZDNmLTViYTMtNGJiNy1hMDBjLTM2YzVhNzQxOWZkZCIsImVudk5hbWU6cHJvZHVjdGlvbiIsImVudklkOjU5NThkYjRlLTVmZTctNDVlOS1hZTk0LTkzNDBkZTg3YjQxYyIsInVwaWQ6NjQxZDcwMGEtYjQ4YS00YjUxLWIyZGItZTc3NjU1MGIwMDA5Il0sImV4cCI6MTY4NTYxMjQ3NiwiaWF0IjoxNjg1NjA4ODc2LCJpZGQiOiJkODY4NmQzZi01YmEzLTRiYjctYTAwYy0zNmM1YTc0MTlmZGQiLCJpc3MiOiJodHRwczovL3BsYXllci1hdXRoLnNlcnZpY2VzLmFwaS51bml0eS5jb20iLCJqdGkiOiIwMjIzODc2NC1kZjU4LTQ4YmYtOGM2MS04ZGMwNjAzNzRiMTIiLCJuYmYiOjE2ODU2MDg4NzYsInByb2plY3RfaWQiOiI2NDFkNzAwYS1iNDhhLTRiNTEtYjJkYi1lNzc2NTUwYjAwMDkiLCJzaWduX2luX3Byb3ZpZGVyIjoiYW5vbnltb3VzIiwic3ViIjoibEUxbHVjcVlpOFRHVjVwMEQwTU9ZZEJuOTdNNSIsInRva2VuX3R5cGUiOiJhdXRoZW50aWNhdGlvbiIsInZlcnNpb24iOiIxIn0.0Wjxirb5bD3lmJgUCSPn1i3LcKNj1eGBR5RkkfuzZHtIGBuNQDtx4iZIP3muiBqlRI8DV0w-q-sbkQbdz9SQWrJwW73VVYV7-jEvuXms8mnjLMJZU6UvFbScn3mQ17ukXx5r6ets6RYjggrrnqEoxUg3OH0ew1E68_7Rq9lJzOfXYKCDKRu3wT-50hHvunLXI2CgRvcNnFSdaukrz2MXC9rpWeUswW8J-i0BmJhmi-WkVto-gHLCWhJnbqrePGOrFk2VgzCHmvi514sDFlCk1bVK0r1RptCoz-Tn2iOcN_Ty4UCK9EoBj7iO2ccH8mK3Um_xg3fR_s4fKeg4vSdIhA";
            mockCloudCodeSetup.UnityCloudProjectId = "641d700a-b48a-4b51-b2db-e776550b0009";
            mockCloudCodeSetup.UnityCloudEndpoint = "modules/OpenAiModule";
            mockCloudCodeSetup.UnityCloudModelsFunction = "GetAiModelList";
            mockCloudCodeSetup.UnityCloudResponseFunction = "GetAiResponse";
            mockCloudCodeSetup.UnityCloudChatFunction = "OpenAiChatRequest";
        }


        [Test]
        public void Get_Models_From_Unity_Cloud()
        {

            var aiApiModelManager = new CloudModelManager(mockCloudCodeSetup);

            string[] models = aiApiModelManager.GetAllModels().Result;
            Console.WriteLine(models);
            Assert.IsNotNull(models);
        }

        [Test]
        public void Get_Chat_From_Unity_Cloud()
        {
            promptSettings.Model = "gpt-3.5-turbo";
            promptSettings.chat_history = new string[]
                {
                    "system:You are a kansas city farmer",
                    "user:Who are you?",
                    "assistant:Howdy pardner! Muh name's billy bob!" ,

            };

            CloudChatResponseService chatService = new CloudChatResponseService(mockCloudCodeSetup, promptSettings);
            AiResponse response = chatService.SendMessage(promptSettings.prompt, promptSettings.chat_history).Result;
            Console.WriteLine(response.Message);
            Assert.IsNotNull(response);

        }

        [Test]
        public void Get_Response_From_Unity_Cloud()
        {

            CloudResponseService responseServie = new CloudResponseService(mockCloudCodeSetup, promptSettings);

            AiResponse response = responseServie.SendMessage(promptSettings.prompt).Result;
            Console.WriteLine(response.Message);
            Assert.IsNotNull(response);
        }
    }
}
