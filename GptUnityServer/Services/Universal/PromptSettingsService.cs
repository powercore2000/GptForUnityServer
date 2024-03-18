using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GptUnityServer.Services.Universal
{
    using System.Reflection;
    using SharedLibrary;

    public class PromptSettingsService : IPromptSettingsService
    {

        private readonly PromptSettings promptSettings;
        public PromptSettingsService(PromptSettings _promptSettings)
        {
            promptSettings = _promptSettings;
        }

        public void SetPromptByString(string promptString)
        {
            try
            {
                PromptSettings newPromptSettings = JsonConvert.DeserializeObject<PromptSettings>(promptString);

                SetPrompt(newPromptSettings);
            }
            catch  {

                Console.WriteLine($"Invalid promptDetails passed in! {promptString}");
            }

        }

        public void SetPrompt(PromptSettings newPromptSettings)
        {
            Console.WriteLine($"Setting prompt settings ");
            if (newPromptSettings != null)
            {
                Console.WriteLine($"Setting prompt settings ");//to : {JsonConvert.DeserializeObject<PromptSettings>(promptDetails)}");
                                                               //Get's all properties from the class PromptSettings
                PropertyInfo[] properties = typeof(PromptSettings).GetProperties();

                // Iterate through each property and update the _foo instance
                foreach (PropertyInfo property in properties)
                {
                    // Check if the property has both a getter and a setter
                    if (property.CanRead && property.CanWrite)
                    {
                        object otherValue = property.GetValue(newPromptSettings);
                        property.SetValue(promptSettings, otherValue);
                    }
                   
                }

                //promptSettings.OverritePromptSettings(JsonConvert.DeserializeObject<PromptSettings>(promptDetails));
            }
        }
    }
}
