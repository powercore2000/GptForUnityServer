using GptUnityServer.Services.OpenAiServices.OpenAiData;
using Newtonsoft.Json;
using SharedLibrary;
using GptUnityServer.Models;
using Microsoft.Extensions.ObjectPool;
using GptUnityServer.Services.OpenAiServices.ResponseService;

namespace GptUnityServer.Services.ServerManagment.UnityServerServices
{

    public abstract class UnityNetCoreServer : IUnityNetCoreServer
    {
        
     
        protected bool isKeyValid { get; set; }
        protected bool displayedStatusMessage { get; set; }
        protected Action onValidationFail { get; set; }
        protected Action onValidationSucess { get; set; }
        protected string serverType { get; set; }

        protected readonly IServiceProvider serviceProvider;
        //protected readonly IServerSetupService serverSetupService;

        public Action<string> OnAiMessageRecived;

        public UnityNetCoreServer(IServiceProvider _serviceProvider)
        {

            serviceProvider = _serviceProvider;
            //serverSetupService = _serverSetupService;
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

            if (clientMessage == "INITIALIZATION-MESSAGE")
            {
                string response = CheckApiValidity();


                OnAiMessageRecived.Invoke(response);

                OnAiMessageRecived.Invoke(SendModelList().Result);
            }

            else
            
                await ProcessClientInput(clientMessage);
            
        }

        /*
        protected async void TriggerAiResponse(string clientMessage)
        {

            if (displayedStatusMessage)
            {
                await ProcessClientInput(clientMessage);
            }

            else
            {              
                string response = CheckApiValidity();
                

                using (var scope = serviceProvider.CreateScope())
                {
                    IServerSetupService serverSetupService = scope.ServiceProvider.GetRequiredService<IServerSetupService>();
                    Console.WriteLine($"setting up server with data...");
                    ServerSetupData setupData = JsonConvert.DeserializeObject<ServerSetupData>(clientMessage);
                    serverSetupService.SetUpServer(setupData);
                }
                
                OnAiMessageRecived.Invoke(response);
            }

            

        }*/

        public virtual async Task ProcessClientInput(string clientMessage)
        {
            if (clientMessage.Contains("PROMPT-SETTINGS:"))
                SendPromptDetails(clientMessage.Replace("PROMPT-SETTINGS:", ""));


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
                IAiResponseService openAiService = scope.ServiceProvider.GetRequiredService<IAiResponseService>();
                AiResponse response = await openAiService.SendMessage(message);
                return response.Message;
            }

        }

        protected virtual void SendPromptDetails(string promptDetailsString)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                IPromptSettingsService promptDetailService = scope.ServiceProvider.GetRequiredService<IPromptSettingsService>();
                promptDetailService.SetPromptDetails(promptDetailsString);

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

        protected async virtual Task<string> SendModelList()
        {
            string message;
            string[] modelList;


            using (var scope = serviceProvider.CreateScope()) {

                IOpenAiModelManager modelManager = scope.ServiceProvider.GetRequiredService<IOpenAiModelManager>();
                modelList = await modelManager.GetAllModels();
            }

            var modelData = new {

                models = modelList
            };
            message = JsonConvert.SerializeObject(modelData);
            return message;


        }


        async Task ValidationFailFunctions()
        {

            await Task.Delay(300);
            onValidationFail?.Invoke();
        }




    }
}
