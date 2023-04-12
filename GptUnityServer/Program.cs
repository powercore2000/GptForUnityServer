using GptUnityServer.Models;
using GptUnityServer.Services.OobaUiServices;
using GptUnityServer.Services.KoboldAIServices;
using GptUnityServer.Services._PlaceholderServices;
using GptUnityServer.Services.UnityCloudCode;
using GptUnityServer.Services.ServerManagment;
using GptUnityServer.Services.OpenAiServices;
using GptUnityServer.Services.Universal;
using GptUnityServer.Services.NetCoreProtocol;

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

else if (settings.ServerServiceEnum == ServerServiceTypes.OpenAi)
{
    builder.Services.AddTransient<IAiResponseService, ApiResponseService>();
    builder.Services.AddTransient<IAiModelManager, ApiModelManager>(); 
    builder.Services.AddTransient<IServerValidationService, ApiKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, MockChatService>();
}

else if (settings.ServerServiceEnum == ServerServiceTypes.OobaUi)
{
    builder.Services.AddTransient<IAiResponseService, OobaUiResponseService>();
    builder.Services.AddTransient<IAiModelManager, MockAiModelManagerService>();
    builder.Services.AddTransient<IServerValidationService, MockApiKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, MockChatService>();
}

else if (settings.ServerServiceEnum == ServerServiceTypes.KoboldAi)
{
    builder.Services.AddTransient<IAiResponseService, KoboldAiResponseService>();
    builder.Services.AddTransient<IAiModelManager, MockAiModelManagerService>();
    builder.Services.AddTransient<IServerValidationService, MockApiKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, MockChatService>();
}




builder.Services.AddTransient<IPromptSettingsService, PromptSettingsService>();
builder.Services.AddTransient<IUnityNetCoreServer, TcpServerService>();
builder.Services.AddTransient<IUnityNetCoreServer, UdpServerService>();




builder.Services.AddHostedService<UnityServerManagerService>();




var app = builder.Build();


app.Run("http://localhost:6776");
