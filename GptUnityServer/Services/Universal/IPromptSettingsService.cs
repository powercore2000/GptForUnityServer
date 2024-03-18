using SharedLibrary;

namespace GptUnityServer.Services.Universal
{
    public interface IPromptSettingsService
    {
        public void SetPromptByString(string promptDetails);

        public void SetPrompt(PromptSettings newPromptSettings);
    }
}
