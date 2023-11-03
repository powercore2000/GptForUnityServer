namespace GptUnityServer.Services.ServerProtocols
{
    public class RestApiServerService : IUnityProtocolServer
    {
        public bool ApiKeyValid { get; private set; }

        #region Net Core Server Methods
        public void RestartServer()
        {
            throw new NotImplementedException();
        }

        public Task<string> SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken, bool _isKeyValid, Action _onFailedValidation)
        {
            Console.WriteLine("Rest Api Server Service launched!");
            ApiKeyValid = _isKeyValid;
            return Task.CompletedTask;
        }

        public void StartServer(int _port = 1111)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Deactivating Rest Api Server!");
            throw new NotImplementedException();
        }

        public void StopServer()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
