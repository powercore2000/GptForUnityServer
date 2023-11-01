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
using Microsoft.OpenApi.Extensions;

namespace GptUnityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestApiServerController : ControllerBase
    {
        private readonly ILogger<RestApiServerController> logger;
        private readonly IAiResponseService aiResponseService;
        private readonly IAiModelManager aiModelManager;
        private readonly IServiceProvider serviceProvider;
        private readonly UnityServerManagerService unityServerManagmentService;
        private readonly PromptSettings promptSettings;
        private readonly ModularServiceSelector serviceSelectorService;
        //private readonly RestApiServerService currentRestApiServerService;
        private bool hasCheckedApiKey;

        public RestApiServerController(
            ILogger<RestApiServerController> _logger,
            IAiModelManager _aiModelManager,
            IAiResponseService _aiResponseService,         
            IAiChatResponseService _aiChatResponseService,
            IServiceProvider _serviceProvider,
            ModularServiceSelector _serviceSelectorService,
            UnityServerManagerService _unityServerManagmentService, 
            PromptSettings _promptSettings)
        {

            logger = _logger;
            aiResponseService = _aiResponseService;
            aiModelManager = _aiModelManager;
            unityServerManagmentService = _unityServerManagmentService;
            serviceSelectorService = _serviceSelectorService;
            serviceProvider = _serviceProvider;
            
            //These methods need to be run to set up the Api to be usable
            serviceSelectorService.SetKeyValidationService(KeyValidationServiceTypes.Offline);
            serviceSelectorService.RunKeyValidationService("", "");
            serviceSelectorService.SetEmotionClassificationService(ClassificationServiceTypes.SillyTavernExtras);
            serviceSelectorService.SetAiChatResponseService(AiChatResponseServiceTypes.OobaUi);
            //currentRestApiServerService = serviceSelectorService.GetServerService() as RestApiServerService;
            promptSettings = _promptSettings;

        }

        #region CRUD Endpoints
        [HttpGet("/ApiKeyValidity")]
        public bool GetApiKeyValidity()
        {

            return serviceSelectorService.IsApiKeyValid;
            //return restApiServerService.ApiKeyValid;
        }

        [HttpPost("/SetAiKeyValidationService")]
        public IActionResult SetKeyValidation([FromBody] MessageData newClassificationType)
        {

            KeyValidationServiceTypes newType = serviceSelectorService.KeyValidationService;

            if (!Enum.TryParse(newClassificationType.Message, out newType))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Invalid ClassificationServiceTypes passed in! Defaulting to Mock...");
            }
            using (var scope = serviceProvider.CreateScope())
            {
                serviceSelectorService.SetKeyValidationService(newType);
                return Ok();
            }
        }




        [HttpPost("/SendResponse")]
        public async Task<AiResponse> SendResponse([FromBody]PromptSettings promptParams)
        {
            if (GetApiKeyValidity())
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
            if (GetApiKeyValidity())
            {
                promptSettings.OverritePromptSettings(promptParams);
                return await serviceSelectorService.GetAiChatResponseService().SendMessage(promptParams);
            }

            else
            {

                return new AiResponse("Api Key was Invalid!", "Api Key was Invalid!");
            }
        }
        
        [HttpPost("/SetAiChatResponseService")]
        public IActionResult SetAiChatResponse([FromBody] MessageData newClassificationType)
        {

            AiChatResponseServiceTypes newType = serviceSelectorService.AiChatResponseService;

            if (!Enum.TryParse(newClassificationType.Message, out newType))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Invalid ClassificationServiceTypes passed in! Defaulting to Mock...");
            }
            using (var scope = serviceProvider.CreateScope())
            {
                serviceSelectorService.SetAiChatResponseService(newType);
                return Ok();
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



        [HttpGet("/GetModels")]
        public async Task<string[]> GetModels()
        {
            return await aiModelManager.GetAllModels();
        }

        [HttpPost("/SetModelManagerService")]
        public IActionResult SetModelManager([FromBody] MessageData newClassificationType)
        {

            ModelManagerServiceTypes newType = serviceSelectorService.ModelManagerService;

            if (!Enum.TryParse(newClassificationType.Message, out newType))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Invalid ClassificationServiceTypes passed in! Defaulting to Mock...");
            }
            using (var scope = serviceProvider.CreateScope())
            {
                serviceSelectorService.SetModelManagerService(newType);
                return Ok();
            }
        }

        #endregion

    }
}
