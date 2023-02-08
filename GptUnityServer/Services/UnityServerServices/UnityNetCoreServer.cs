using GptToUnityServer.Models;
using GptToUnityServer.Services.UnityServerServices;
using GptUnityServer.Services.OpenAiServices;
using GptUnityServer.Services.OpenAiServices.PromptDetails;
using GptUnityServer.Services.OpenAiServices.PromptSending;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary;

namespace GptUnityServer.Services.UnityServerServices
{
    public abstract class UnityNetCoreServer : IUnityNetCoreServer
    {

        protected bool isKeyValid { get; set; }
        protected bool displayedStatusMessage { get; set; }
        protected Action onValidationFail { get; set; }
        protected Action onValidationSucess { get; set; }
        protected string serverType { get; set; }

        protected readonly IServiceProvider serviceProvider;

        public Action<string> OnAiMessageRecived;

        public UnityNetCoreServer(IServiceProvider _serviceProvider) {

            serviceProvider = _serviceProvider;
        }

        public virtual void RestartServer()
        {
            throw new NotImplementedException();
        }


        public virtual Task StartAsync(CancellationToken cancellationToken, bool _isKeyValid, Action _onFailedValidation)
        {
            isKeyValid = _isKeyValid;
            onValidationFail += _onFailedValidation;
            return Task.CompletedTask;
        }

        public virtual void StartServer(int _port)
        {
            throw new NotImplementedException();
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual void StopServer()
        {
            throw new NotImplementedException();
        }

        protected async void TriggerAiResponse(string clientMessage)
        {

            if (displayedStatusMessage)
            {
                await ProcessClientInput(clientMessage);
            }

            else
            {
                string response = CheckApiValidity();
                Console.WriteLine($"displaying Ai response: {response}");
                OnAiMessageRecived.Invoke(response);
            }

            

        }

        public virtual async Task ProcessClientInput(string clientMessage)
        {
            if (clientMessage.Contains("PROMPT_MESSAGE: "))
                SendPromptDetails(clientMessage);

            else
            {
                string response = await SendMessage(clientMessage);
                Console.WriteLine($"displaying Ai response: {response}");
                OnAiMessageRecived.Invoke(response);
            }

        }

        public virtual async Task<string> SendMessage(string message)
        {

            // The scope informs the service provider when you're
            // done with the transient service so it can be disposed
            using (var scope = serviceProvider.CreateScope())
            {
                IOpenAiPromptService openAiService = scope.ServiceProvider.GetRequiredService<IOpenAiPromptService>();
                AiResponse response = await openAiService.SendMessage(message);
                return response.Message;
            }

        }

        protected virtual void SendPromptDetails(string promptDetailsString)
        {

            using (var scope = serviceProvider.CreateScope())
            {
                IPromptDetailService promptDetailService = scope.ServiceProvider.GetRequiredService<IPromptDetailService>();
                promptDetailService.SetPromptDetails();

            }
        }


        protected virtual string CheckApiValidity()
        {
            displayedStatusMessage = true;
            //isKeyValid = validationService.ValidateApiKey();
            if (isKeyValid)
            {
                onValidationSucess?.Invoke();
                Console.WriteLine($"UDP server api key is valid!");
                //server.Me
                return $"SUCCESS: Welcome to GPT to Unity using {serverType}";
            }

            else
            {

                ValidationFailFunctions();
                Console.WriteLine($"Invalid UDP server api key!");

                return $"ERROR: Invalid Open Ai API Key With {serverType}";
            }


        }


        async Task ValidationFailFunctions() {

            await Task.Delay(300);
                onValidationFail?.Invoke();
        }




    }
}
