using GptToUnityServer.Models;
using GptToUnityServer.Services.UnityServerServices;
using GptUnityServer.Services.OpenAiServices;

namespace GptUnityServer.Services.UnityServerServices
{
    public abstract class UnityNetCoreServer : IUnityNetCoreServer
    {

        protected bool isKeyValid { get; set; }
        protected Action onFailedValidation { get; private set; }
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
            onFailedValidation = _onFailedValidation;
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

        protected virtual string CheckApiValidity()
        {

            throw new NotImplementedException();
        }
        /*
        protected async void TriggerAiResponse(string clientMessage)
        {
            string response;

            if (isKeyValid)
                response = await SendMessage(clientMessage);

            else
                response = CheckApiValidity();

            Console.WriteLine($"displaying Ai response: {response}");
            OnAiMessageRecived.Invoke(response);

        }*/


    }
}
