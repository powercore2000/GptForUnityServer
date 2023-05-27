using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SharedLibrary;
using GptUnityServer.Services.Universal;
using System.Threading.Tasks;
using GptUnityServer.Services.ServerProtocols;
using GptUnityServer.Services.ServerManagment;
using GptUnityServer.Services.AiApiServices;

namespace GptUnityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestApiServerController : ControllerBase
    {
        /*
         Sets for adding a new Unity -> GptToUnity Server:
            -Add communication method as an IUnityProtocolServer class
            - Either add addiotnal features of communication to the class or make another class that access the IUnityProtocolServer class
            -Pull that specific class from the allProtocolServers variable inside of UnityServerManagerService
            -Make that server add a specific validation method to the validation service
            -Set up the Start Async and Stop Async methods in that IUnityProtocolServer class
         */

        private readonly ILogger<RestApiServerController> logger;
        private readonly IAiResponseService aiResponseService;
        private readonly IAiModelManager aiModelManager;
        private readonly IAiChatResponseService aiChatResponseService;
        private readonly RestApiServerService restApiServerService;
        private readonly UnityServerManagerService unityServerManagmentService;
        private readonly PromptSettings promptSettings;
        private bool apiKeyValid;

        public RestApiServerController(
            ILogger<RestApiServerController> _logger,
            IAiModelManager _aiModelManager,
            IAiResponseService _aiResponseService,         
            IAiChatResponseService _aiChatResponseService,
            UnityServerManagerService _unityServerManagmentService, 
            PromptSettings _promptSettings)
        {

            logger = _logger;
            aiResponseService = _aiResponseService;
            aiModelManager = _aiModelManager;
            aiChatResponseService = _aiChatResponseService;
            unityServerManagmentService = _unityServerManagmentService;
            
            restApiServerService = unityServerManagmentService.CurrentServerService as RestApiServerService;
            promptSettings = _promptSettings;
        }

        #region CRUD Endpoints
        [HttpGet("/ApiKeyValidity")]
        public bool GetApiKeyValidity()
        {
            return restApiServerService.ApiKeyValid;
        }

        [HttpPost("/SendResponse")]
        public async Task<AiResponse> SendResponse([FromBody]PromptSettings promptParams)
        {
            if (restApiServerService.ApiKeyValid)
            {
                promptSettings.OverritePromptSettings(promptParams);
                return await aiResponseService.SendMessage(promptParams.Prompt);
            }

            else {

                return new AiResponse("Api Key was Invalid!", "Api Key was Invalid!");
            }
        }

        [HttpPost("/SendChat")]
        public async Task<AiResponse> SendChat([FromBody] PromptSettings promptParams)
        {
            if (restApiServerService.ApiKeyValid)
            {
                promptSettings.OverritePromptSettings(promptParams);
                return await aiChatResponseService.SendMessage(promptParams.Prompt,promptParams.SystemStrings);
            }

            else
            {

                return new AiResponse("Api Key was Invalid!", "Api Key was Invalid!");
            }
        }


        [HttpGet("/Models")]
        public async Task<string[]> GetModels()
        {
            return await aiModelManager.GetAllModels();
        }

        #endregion

    }
}
