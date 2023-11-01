namespace GptUnityServer.Services.ServerProtocols
{
    public interface IUnityProtocolServer
    {

        //public Action<string> OnReciveAiMessage { get; set; }
        public bool ApiKeyValid { get; }
        public void StartServer(int _port = 1111);
        public void StopServer();

        public void RestartServer();

        public Task<string> SendMessage(string message);

        public Task StartAsync(CancellationToken cancellationToken, bool _isKeyValid, Action _onFailedValidation);

        public Task StopAsync(CancellationToken cancellationToken);

    }
}
