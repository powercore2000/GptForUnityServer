namespace GptToUnityServer.Services.UnityServerServices
{
    public interface IUnityNetCoreServer
    {

        //public Action<string> OnReciveAiMessage { get; set; }
        public void StartServer(int _port = 1111);
        public void StopServer();

        public void RestartServer();

        public Task<string> SendMessage(string message);

        public Task StartAsync(CancellationToken cancellationToken, bool _isKeyValid, Action _onFailedValidation);

        public Task StopAsync(CancellationToken cancellationToken);

    }
}
