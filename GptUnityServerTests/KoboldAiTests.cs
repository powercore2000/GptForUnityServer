using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GptUnityServer.Services.KoboldAIServices;
using GptUnityServer.Services.OobaUiServices;

namespace GptUnityServerTests
{
    internal class KoboldAiTests
    {
        PromptSettings promptSettings;
        [SetUp]
        public void Setup()
        {
            promptSettings = new PromptSettings();
            promptSettings.Temperature = 0.3f;
            promptSettings.Prompt = "You: How are you my wonderful girlfriend?";
        }


        [Test]
        public void Get_Model_From_Kobold()
        {

            var responseServie = new KoboldAiResponseService(promptSettings);

            string response = responseServie.GetModel().Result;
            Console.WriteLine(response);
            Assert.IsNotNull(response);
        }

        [Test]
        public void Get_Ai_Response_From_Kobold()
        {

            var responseServie = new KoboldAiResponseService(promptSettings);

            var response = responseServie.SendMessage(promptSettings.Prompt).Result;
            Console.WriteLine(response.Message);
            Assert.IsNotNull(response);
        }
    }
}
