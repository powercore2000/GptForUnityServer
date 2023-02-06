using GptToUnityServer.Models;
using GptToUnityServer.Services.UnityServerServices;
using GptUnityServer.Services.OpenAiServices;

namespace GptUnityServer.Services.UnityServerServices
{
    public abstract class UnityNetCoreServer : IUnityNetCoreServer
    {

        protected bool isKeyValid { get; set; }
        protected bool displayedStatusMessage { get; set; }
        protected Action onValidationFail { get; set; }
        protected Action onValidationSucess { get; set; }
        protected string serverType { get; set; }


        public Action<string> OnAiMessageRecived;

        public virtual void RestartServer()
        {
            throw new NotImplementedException();
        }

        public virtual Task<string> SendMessage(string message)
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
            string response;

            if (displayedStatusMessage)
                response = await SendMessage(clientMessage);

            else
                response = CheckApiValidity();

            Console.WriteLine($"displaying Ai response: {response}");
            OnAiMessageRecived.Invoke(response);

            

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
