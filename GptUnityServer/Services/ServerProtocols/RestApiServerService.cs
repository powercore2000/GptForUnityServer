namespace GptUnityServer.Services.ServerProtocols
{
    public class RestApiServerService : IUnityProtocolServer
    {
        private bool apiKeyValid;

        public bool ApiKeyValid => apiKeyValid;

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
            apiKeyValid = _isKeyValid;
            return Task.CompletedTask;
        }

        public void StartServer(int _port = 1111)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void StopServer()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
