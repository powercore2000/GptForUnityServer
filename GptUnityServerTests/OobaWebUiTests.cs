namespace GptUnityServerTests
{
    using System.Reflection;
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
    }
}