namespace GptUnityServerTests
{
    using System.Reflection;
    using GptUnityServer.Services.KoboldAIServices;
    using GptUnityServer.Services.OobaUiServices;
    

    public class OobaWebUiTests
    {
        PromptSettings promptSettings;

        [SetUp]
        public void Setup()
        {
            promptSettings = new PromptSettings();
            promptSettings.Temperature = 0.3f;
            promptSettings.Prompt = "What is Texas?";
        }

        [Test]
        public void Get_Ai_Response_From_Web_Ui()
        {
            
            var responseServie = new OobaUiResponseService(promptSettings);

            var response = responseServie.SendMessage(promptSettings.Prompt).Result;
            Console.WriteLine(response.Message);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Async_Get_Ai_Response_From_Web_Ui()
        {

            var responseServie = new OobaUiResponseService(promptSettings);

            var response = await responseServie.SendMessage(promptSettings.Prompt);
            Console.WriteLine(response.Message);
            Assert.IsNotNull(response);
        }

        [Test]
        public void Get_Chat_Response_From_Kobold()
        {



            var responseServie = new OobaUiChatService(promptSettings);
            string persona = "Chiharu Yamada is a young, computer engineer-nerd with a knack for problem solving and a passion for technology.";
            string greeting = "*Chiharu strides into the room with a smile, her eyes lighting up when she sees you. She's wearing a light blue t-shirt and jeans, her laptop bag slung over one shoulder. She takes a seat next to you, her enthusiasm palpable in the air*\n\nHey! I'm so excited to finally meet you. I've heard so many great things about you and I'm eager to pick your brain about computers. I'm sure you have a wealth of knowledge that I can learn from. *She grins, eyes twinkling with excitement* Let's get started!";

            string exampleStringsMerged = string.Join(" ", promptSettings.SystemStrings);

            promptSettings.Prompt = "You: Who are you?";
            Console.WriteLine($"Sending message {promptSettings.Prompt}\n Array of strings:{exampleStringsMerged}");

            var response = responseServie.SendMessage(greeting + persona + exampleStringsMerged + promptSettings.Prompt, promptSettings.SystemStrings).Result;
            Console.WriteLine(response.Message);
            Assert.IsNotNull(response);
        }
    }
}