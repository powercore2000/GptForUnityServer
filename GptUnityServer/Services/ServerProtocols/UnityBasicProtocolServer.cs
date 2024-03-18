using Newtonsoft.Json;
using SharedLibrary;
using GptUnityServer.Models;
using Microsoft.Extensions.ObjectPool;
using GptUnityServer.Services.Universal;
using Newtonsoft.Json.Linq;
using GptForUnityServer.Services.ServerManagment;

namespace GptUnityServer.Services.ServerProtocols
{

    public abstract class UnityBasicProtocolServer : IUnityProtocolServer
    {


        public bool ApiKeyValid { get; set; }
        protected bool displayedStatusMessage { get; set; }
        protected Action? onValidationFail { get; set; }
        protected Action? onValidationSucess { get; set; }
        protected string? serverType { get; set; }

        //protected readonly IServiceProvider serviceProvider;
        protected readonly PromptSettings promptSettings;
        protected readonly ModularServiceSelector modularServiceSelector;

        public Action<string>? OnAiMessageRecived;

        public UnityBasicProtocolServer(PromptSettings _promptSettings, ModularServiceSelector _modularServiceSelector)
        {
            promptSettings = _promptSettings;
            modularServiceSelector = _modularServiceSelector;
        }

        public virtual void RestartServer()
        {
            throw new NotImplementedException();
        }


        public virtual Task StartAsync(CancellationToken cancellationToken, bool _isKeyValid, Action _onFailedValidation)
        {
            ApiKeyValid = _isKeyValid;
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

            if (clientMessage == CommunicationData.InitalizationIdText)
            {
                string response = CheckApiKeyValidity();

                Console.WriteLine($"Triggered Protocol Api Key Validity with response {response}");

                OnAiMessageRecived.Invoke(response);
                string modelList = await SendModelList();
                OnAiMessageRecived.Invoke(modelList);
            }

            else

                await ProcessClientInput(clientMessage);

        }
        /// <summary>
        /// Parse the contents of the client's JSON message to determine which service to send it to for handling
        /// </summary>
        /// <param name="clientMessage"></param>
        /// <returns></returns>
        public virtual async Task ProcessClientInput(string clientMessage)
        {
            string userMessage = clientMessage;
            string response;

            SetPromptDetails(clientMessage);


            if (promptSettings.PromptTypeString == "Chat")
                response = await SendChatMessage(promptSettings);

            else
                response = await SendMessage(userMessage);

            Console.WriteLine($"displaying Ai response: {response}");
            OnAiMessageRecived.Invoke(response);

        }

        #region Service Callers
        public virtual async Task<string> SendMessage(string message)
        {

            // The scope informs the service provider when you're
            // done with the transient service so it can be disposed
            AiResponse response = await modularServiceSelector.GetAiInstructService().SendMessage(message);
            return response.Message;

        }

        public virtual async Task<string> SendChatMessage(PromptSettings promptSettings)
        {

            AiResponse response = await modularServiceSelector.GetAiChatService().SendMessage(promptSettings);
            return response.Message;

        }

        protected void SetPromptDetails(string promptDetailsString)
        {

            modularServiceSelector.GetPromptSettingsService().SetPromptByString(promptDetailsString);

        }


        protected virtual string CheckApiKeyValidity()
        {
            displayedStatusMessage = true;
            if (ApiKeyValid)
            {
                onValidationSucess?.Invoke();
                Console.WriteLine($"{serverType} server api key is valid!");
                //server.Me
                return $"SUCCESS: Welcome to GPT to Unity using {serverType}";
            }

            else
            {

                ValidationFailFunctions();
                Console.WriteLine($"Invalid {serverType} server api key!");

                return $"ERROR: Invalid API Key With {serverType}";
            }


        }

        protected async virtual Task<string> SendModelList()
        {
            string message;
            string[] modelList;


            modelList = await modularServiceSelector.GetModelManagerService().GetAllModels();

            JArray modelData = JArray.FromObject(modelList);
            
            //Formats the models array sent into a JSON Object containing an array
            return "{ \"models\":" + modelData.ToString() + "}";


        }


        async Task ValidationFailFunctions()
        {
            //Why is this await here?
            await Task.Delay(300);
            onValidationFail?.Invoke();
        }
        #endregion



    }
}
