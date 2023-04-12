using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GptUnityServer.Services.Universal
{
    using SharedLibrary;

    public class PromptSettingsService : IPromptSettingsService
    {

        private readonly PromptSettings promptSettings;
        public PromptSettingsService(PromptSettings _promptSettings)
        {
            promptSettings = _promptSettings;
        }

        public void SetPromptDetails(string promptDetails)
        {

            if (!string.IsNullOrEmpty(promptDetails))
            {
                Console.WriteLine($"Setting prompt settings ");//to : {JsonConvert.DeserializeObject<PromptSettings>(promptDetails)}");
                promptSettings.OverritePromptSettings(JsonConvert.DeserializeObject<PromptSettings>(promptDetails));
            }
        }
    }
}
