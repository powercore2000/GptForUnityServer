using GptToUnityServer.Models;
using GptToUnityServer.Services.UnityServerServices;
using GptUnityServer.Services.OpenAiServices;

namespace GptUnityServer.Services.UnityServerServices
{
    public abstract class UnityNetCoreServer : IUnityNetCoreServer
    {

        protected bool isKeyValid { get; set; }


        public virtual void RestartServer()
        {
            throw new NotImplementedException();
        }

        public virtual Task<string> SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public virtual Task StartAsync(CancellationToken cancellationToken, bool _isKeyValid)
        {
            isKeyValid = _isKeyValid;
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

        
    }
}
