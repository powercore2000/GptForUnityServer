using GptUnityServer.Models;
using GptUnityServer.Services.OpenAiServices;
using GptUnityServer.Services.OpenAiServices.Api_Validation;
using GptUnityServer.Services.ServerManagment.ServerManagerServices;
using GptUnityServer.Services.ServerManagment.UnityServerServices;
using GptUnityServer.Services.OpenAiServices.OpenAiData;
using GptUnityServer.Services.OpenAiServices.ResponseService;
using GptUnityServer.Services.OpenAiServices.ChatResponseService;

var builder = WebApplication.CreateBuilder(args);

var promptSettings = new SharedLibrary.PromptSettings();
//builder.Configuration.Bind("", promptSettings);

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);


settings.RunSetUp(args);


builder.Services.AddSingleton(settings);
builder.Services.AddSingleton(promptSettings);

if (settings.ServerConfig == "Cloud")
{
    builder.Services.AddTransient<IAiResponseService, CloudResponseService>();
    builder.Services.AddTransient<IOpenAiModelManager, CloudModelManager>();
    builder.Services.AddTransient<IApiKeyValidation, MockApiKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, CloudChatResponseService>();
}

else if (settings.ServerConfig == "Api")
{
    builder.Services.AddTransient<IAiResponseService, ApiResponseService>();
    builder.Services.AddTransient<IOpenAiModelManager, ApiModelManager>();
    //builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidationService>();
    builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, CloudChatResponseService>();
}


builder.Services.AddTransient<IPromptSettingsService, PromptSettingsService>();
builder.Services.AddTransient<IUnityNetCoreServer, TcpServerService>();
builder.Services.AddTransient<IUnityNetCoreServer, UdpServerService>();




builder.Services.AddHostedService<UnityServerManagerService>();




var app = builder.Build();


app.Run();
