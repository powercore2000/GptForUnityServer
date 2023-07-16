using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GptUnityServer.Services.KoboldAIServices;
using GptUnityServer.Services.OobaUiServices;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace GptUnityServerTests
{
    internal class KoboldAiTests
    {
        PromptSettings promptSettings;
        [SetUp]
        public void Setup()
        {
            promptSettings = new PromptSettings();
            promptSettings.Temperature = 0.5f;
            promptSettings.Prompt = "You: How are you?";
            promptSettings.MaxTokens = 500;
            promptSettings.TopP = 0.9f;

            string messages = "{{user}}: So how did you get into computer engineering?\n{{char}}: I've always loved tinkering with technology since I was a kid.\n{{user}}: That's really impressive!\n{{char}}: *She chuckles bashfully* Thanks!\n{{user}}: So what do you do when you're not working on computers?\n{{char}}: I love exploring, going out with friends, watching movies, and playing video games.\n{{user}}: What's your favorite type of computer hardware to work with?\n{{char}}: Motherboards, they're like puzzles and the backbone of any system.\n{{user}}: That sounds great!\n{{char}}: Yeah, it's really fun. I'm lucky to be able to do this as a job.";
           // messages += "\n*Chiharu strides into the room with a smile, her eyes lighting up when she sees you. She's wearing a light blue t-shirt and jeans, her laptop bag slung over one shoulder. She takes a seat next to you, her enthusiasm palpable in the air*\n\nHey! I'm so excited to finally meet you. I've heard so many great things about you and I'm eager to pick your brain about computers. I'm sure you have a wealth of knowledge that I can learn from. *She grins, eyes twinkling with excitement* Let's get started!";
            messages = messages.Replace("{{user}}", "You");
            messages = messages.Replace("{{char}}", "Chiharu Yamada");

            promptSettings.SystemStrings = messages.Split('\n');
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

        [Test]
        public void Get_Chat_Response_From_Kobold()
        {



            var responseServie = new KoboldAIChatService(promptSettings);
            string persona = "Chiharu Yamada is a young, computer engineer-nerd with a knack for problem solving and a passion for technology.";
            string greeting = "*Chiharu strides into the room with a smile, her eyes lighting up when she sees you. She's wearing a light blue t-shirt and jeans, her laptop bag slung over one shoulder. She takes a seat next to you, her enthusiasm palpable in the air*\n\nHey! I'm so excited to finally meet you. I've heard so many great things about you and I'm eager to pick your brain about computers. I'm sure you have a wealth of knowledge that I can learn from. *She grins, eyes twinkling with excitement* Let's get started!";

            string exampleStringsMerged = string.Join(" ", promptSettings.SystemStrings);

            promptSettings.Prompt = "You: Who are you?";
            Console.WriteLine($"Sending message {promptSettings.Prompt}\n Array of strings:{exampleStringsMerged}");

            var response = responseServie.SendMessage(promptSettings.Prompt, promptSettings.SystemStrings).Result;
            Console.WriteLine(response.Message);
            Assert.IsNotNull(response);
        }
    }
}
