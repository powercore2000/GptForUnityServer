using GptUnityServer.Models;
using GptUnityServer.Services.ServerManagment.ServerManagerServices;
using GptUnityServer.Services.ServerManagment.UnityServerServices;
using GptUnityServer.Services.OpenAiServices.OpenAiData;
using GptUnityServer.Services.OpenAiServices.ResponseService;
using GptUnityServer.Services.OpenAiServices.ChatResponseService;
using GptUnityServer.Services.ServerManagment.ValidationServices;
using GptUnityServer.Services.UniversalInterfaces;
using GptUnityServer.Services.OobaUiServices;
using GptUnityServer.Services.KoboldAIServices;
using GptUnityServer.Services._PlaceholderServices;

var builder = WebApplication.CreateBuilder(args);

var promptSettings = new SharedLibrary.PromptSettings();
//builder.Configuration.Bind("", promptSettings);

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);


settings.RunSetUp(args);


builder.Services.AddSingleton(settings);
builder.Services.AddSingleton(promptSettings);

if (settings.ServerServiceEnum == ServerServiceTypes.UnityCloudCode)
{
    builder.Services.AddTransient<IAiResponseService, CloudResponseService>();
    builder.Services.AddTransient<IAiModelManager, CloudModelManager>();
    builder.Services.AddTransient<IServerValidationService, CloudCodeValidationServices>();
    builder.Services.AddTransient<IAiChatResponseService, CloudChatResponseService>();
}

else if (settings.ServerServiceEnum == ServerServiceTypes.Api)
{
    builder.Services.AddTransient<IAiResponseService, ApiResponseService>();
    builder.Services.AddTransient<IAiModelManager, ApiModelManager>(); 
    builder.Services.AddTransient<IServerValidationService, PlaceholderKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, CloudChatResponseService>();
}

else if (settings.ServerServiceEnum == ServerServiceTypes.OobaUi)
{
    builder.Services.AddTransient<IAiResponseService, OobaUiResponseService>();
    builder.Services.AddTransient<IAiModelManager, OobaUiModelManagerService>();
    builder.Services.AddTransient<IServerValidationService, PlaceholderKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, CloudChatResponseService>();
}

else if (settings.ServerServiceEnum == ServerServiceTypes.KoboldAi)
{
    builder.Services.AddTransient<IAiResponseService, KoboldAiResponseService>();
    builder.Services.AddTransient<IAiModelManager, KoboldAiModelManagerService>();
    builder.Services.AddTransient<IServerValidationService, ApiKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, CloudChatResponseService>();
}




builder.Services.AddTransient<IPromptSettingsService, PromptSettingsService>();
builder.Services.AddTransient<IUnityNetCoreServer, TcpServerService>();
builder.Services.AddTransient<IUnityNetCoreServer, UdpServerService>();




builder.Services.AddHostedService<UnityServerManagerService>();




var app = builder.Build();


app.Run();
