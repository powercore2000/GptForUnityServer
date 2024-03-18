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
        private readonly IServiceProvider serviceProvider;
        //private readonly UnityServerManagerService unityServerManagmentService;
        private readonly PromptSettings promptSettings;
        private readonly ModularServiceSelector serviceSelectorService;
        private readonly IPromptSettingsService promptSettingsService;
        //private readonly RestApiServerService currentRestApiServerService;
        private bool hasCheckedApiKey;

        public RestApiServerController(
            ILogger<RestApiServerController> _logger,
            IServiceProvider _serviceProvider,
            ModularServiceSelector _serviceSelectorService,
            IPromptSettingsService _promptSettingsService,
            //UnityServerManagerService _unityServerManagmentService, 
            PromptSettings _promptSettings)
        {

            logger = _logger;
            //unityServerManagmentService = _unityServerManagmentService;
            serviceSelectorService = _serviceSelectorService;
            serviceProvider = _serviceProvider;
            
            promptSettings = _promptSettings;
            promptSettingsService = _promptSettingsService;

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




        [HttpPost("/SendInstruct")]
        public async Task<AiResponse> SendInstruct([FromBody]PromptSettings promptParams)
        {
            if (GetApiKeyValidity())
            {
                promptSettingsService.SetPrompt(promptParams);
                return await serviceSelectorService.GetAiInstructService().SendMessage(promptParams.prompt);
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
                promptSettingsService.SetPrompt(promptParams);
                return await serviceSelectorService.GetAiChatService().SendMessage(promptParams);
            }

            else
            {

                return new AiResponse("Api Key was Invalid!", "Api Key was Invalid!");
            }
        }
        
        [HttpPost("/SetAiChatService")]
        public IActionResult SetAiChat([FromBody] MessageData newClassificationType)
        {

            AiChatServiceTypes newType = serviceSelectorService.AiChatService;

            if (!Enum.TryParse(newClassificationType.Message, out newType))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Invalid ClassificationServiceTypes passed in! Defaulting to Mock...");
            }
            using (var scope = serviceProvider.CreateScope())
            {
                serviceSelectorService.SetAiChatService(newType);
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
            return await serviceSelectorService.GetModelManagerService().GetAllModels();
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
