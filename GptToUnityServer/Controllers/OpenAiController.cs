using GptToUnityServer.Services.UnityServerServices;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;


namespace GptToUnityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenAiController : Controller
    {

        private readonly TcpServerService _unityService;

        public OpenAiController(TcpServerService unityService)
        {

            _unityService = unityService;
        }

        [HttpGet]
        public async Task<string> Get()
        {

            //_unityService.StartServer();
            return await _unityService.SendMessage("What are you?");

        }

        [HttpPost]
        public async Task<string> Post(string message)
        {

            //_unityService.StartServer();
            return await _unityService.SendMessage(message);

        }
    }

    
}
