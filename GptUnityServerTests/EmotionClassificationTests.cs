using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GptForUnityServer.Services.EmotionClassificationServices;

namespace GptUnityServerTests
{
    internal class EmotionClassificationTests
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
        public void Test_Silly_Tavern_Extras_Emotion_Classification()
        {
            SillyTavernExtraSimpleClassifyService stEmotions = new SillyTavernExtraSimpleClassifyService();
            string message = "I LOVE YOU";

            List<EmotionData> emotions = stEmotions.ClassifyMessage(message).Result;

            foreach (EmotionData emote in emotions)
            {
                Console.WriteLine(emote.ToString());
            }

            Assert.That(emotions.Count > 0);

        }
    }
}
