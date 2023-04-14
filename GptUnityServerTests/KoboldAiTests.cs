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
            promptSettings.Prompt = "You: But I cant date anyone who cant beat me in hand to hand combat";
            promptSettings.MaxTokens = 300;
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
        public void Conext_Ooba_Test()
        {

            promptSettings.Prompt = "*A shy girl in a white dress enters the room and stares at you blankly*" +
                "You: Sally have you seen my dog? His name is Jimbo" +
                "*Sally does not respond*" +
                "You: Are you alright Sally?" +
                "Sally:...Hmm? Oh sorry, I was lost in thought." +
                "You:Do you even know my dogs name Sally?"
                ;
            var responseServie = new KoboldAiResponseService(promptSettings);
            var response = responseServie.SendMessage(promptSettings.Prompt).Result;
            Console.WriteLine(response.Message);
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
