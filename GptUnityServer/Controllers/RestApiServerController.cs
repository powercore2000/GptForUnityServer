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
using GptForUnityServer.Services.Universal;
using GptForUnityServer.Services.ServerManagment;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace GptUnityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestApiServerController : ControllerBase
    {
        private readonly ILogger<RestApiServerController> logger;
        private readonly IAiResponseService aiResponseService;
        private readonly IAiModelManager aiModelManager;
        private readonly IAiChatResponseService aiChatResponseService;
        private readonly IServiceProvider serviceProvider;
        private readonly RestApiServerService restApiServerService;
        private readonly UnityServerManagerService unityServerManagmentService;
        private readonly PromptSettings promptSettings;
        private readonly ServiceSelectorService serviceSelectorService;
        private bool apiKeyValid;

        public RestApiServerController(
            ILogger<RestApiServerController> _logger,
            IAiModelManager _aiModelManager,
            IAiResponseService _aiResponseService,         
            IAiChatResponseService _aiChatResponseService,
            IServiceProvider _serviceProvider,
            ServiceSelectorService _serviceSelectorService,
            UnityServerManagerService _unityServerManagmentService, 
            PromptSettings _promptSettings)
        {

            logger = _logger;
            aiResponseService = _aiResponseService;
            aiModelManager = _aiModelManager;
            aiChatResponseService = _aiChatResponseService;
            unityServerManagmentService = _unityServerManagmentService;
            serviceSelectorService = _serviceSelectorService;
            serviceProvider = _serviceProvider;

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
                return await aiResponseService.SendMessage(promptParams.prompt);
            }

            else {

                return new AiResponse("Api Key was Invalid!", "Api Key was Invalid!");
            }
        }

        [HttpPost("/SendChat")]
        public async Task<AiResponse> SendChat([FromBody] PromptSettings promptParams)
        {
            Console.WriteLine("Hitting send chat endpoint");
            if (restApiServerService.ApiKeyValid)
            {
                promptSettings.OverritePromptSettings(promptParams);
                return await aiChatResponseService.SendMessage(promptParams);
            }

            else
            {

                return new AiResponse("Api Key was Invalid!", "Api Key was Invalid!");
            }
        }

        [HttpPost("/ClassifyMessageEmotion")]
        public async Task<List<EmotionData>> GetEmotionFromMessage([FromBody] MessageData messageData)
        {
            Console.WriteLine("Grabbing emotion data");
            using (var scope = serviceProvider.CreateScope())
            {
                return await serviceSelectorService.GetEmotionClassificationService().ClassifyMessage(messageData.Message);

            }

        }

        [HttpPost("/SetClassifyEmotionService")]
        public IActionResult SetEmotionClassification([FromBody] MessageData newClassificationType) {

            ClassificationServiceTypes newType = serviceSelectorService.ClassificationService;

            if (!Enum.TryParse(newClassificationType.Message, out newType)) {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Invalid ClassificationServiceTypes passed in! Defaulting to Mock...");
            }
            using (var scope = serviceProvider.CreateScope())
            {
                serviceSelectorService.SetEmotionClassificationService(newType);
                return Ok();
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
