using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SharedLibrary;
using GptUnityServer.Services.Universal;
using System.Threading.Tasks;
using GptUnityServer.Services.ServerProtocols;
using GptUnityServer.Services.ServerManagment;

namespace GptUnityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestApiServerController : ControllerBase
    {
        /*
         Sets for adding a new Unity -> GptToUnity Server:
            -Add communication method as an IUnityNetCoreServer class
            -Pull that specific class from the allNetCoreServers variable inside of UnityServerManagerService
            -Make that server add a specific validation method to the validation service
            -Set up the Start Async and Stop Async methods in that IUnityNetCoreServer class
         */

        private readonly ILogger<RestApiServerController> logger;
        private readonly IAiResponseService aiResponseService;
        private readonly RestApiServerService restApiServerService;
        private readonly UnityServerManagerService unityServerManagmentService;
        private bool apiKeyValid;

        public RestApiServerController(ILogger<RestApiServerController> _logger, IAiResponseService _aiResponseService, UnityServerManagerService _unityServerManagmentService)
        {
            logger = _logger;
            aiResponseService = _aiResponseService;
            unityServerManagmentService = _unityServerManagmentService;
            IUnityProtocolServer save = unityServerManagmentService.CurrentServerService;
            restApiServerService = save as RestApiServerService;
        }

        #region CRUD Endpoints
        [HttpGet(Name = "ApiKeyValidity")]
        public bool Get()
        {
            return restApiServerService.ApiKeyValid;
        }

        [HttpPost(Name = "SendMessage")]
        public async Task<AiResponse> Get(string prompt)
        {
            if (restApiServerService.ApiKeyValid)
            {
                return await aiResponseService.SendMessage(prompt);
            }

            else {

                return new AiResponse("Api Key was Invalid!", "Api Key was Invalid!");
            }
        }

        #endregion

    }
}
