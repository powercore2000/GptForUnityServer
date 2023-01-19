using GptToUnityServer.Services.UnityServerManager;
using GptToUnityServer.Services.UnityServerServices;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;


namespace GptToUnityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenAiController : Controller
    {

        private readonly UnityServerManagerService _unityService;

        public OpenAiController(UnityServerManagerService unityService)
        {

            _unityService = unityService;
        }

        [HttpGet("SendAiMessage/{prompt}")]
        public async Task<string> Get(string prompt)
        {

            //_unityService.StartServer();
            return await _unityService.CurrentServerService.SendMessage(prompt);

        }

    }

    
}
