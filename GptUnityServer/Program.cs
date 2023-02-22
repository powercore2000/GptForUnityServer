using GptUnityServer.Models;
using GptUnityServer.Services.OpenAiServices;
using GptUnityServer.Services.OpenAiServices.Api_Validation;
using GptUnityServer.Services.ServerManagment.ServerManagerServices;
using GptUnityServer.Services.ServerManagment.UnityServerServices;
using GptUnityServer.Services.OpenAiServices.OpenAiData;
using GptUnityServer.Services.OpenAiServices.ResponseService;

var builder = WebApplication.CreateBuilder(args);

var promptSettings = new SharedLibrary.PromptSettings();
//builder.Configuration.Bind("", promptSettings);

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);


settings.RunSetUp(args);


// Add services to the container.
//builder.Services.AddControllers();

builder.Services.AddSingleton(settings);
builder.Services.AddSingleton(promptSettings);

if (settings.ServerConfig == "Cloud")
{
    builder.Services.AddTransient<IAiResponseService, CloudResponseService>();
    builder.Services.AddTransient<IOpenAiModelManager, CloudModelManager>();
    builder.Services.AddTransient<IApiKeyValidation, MockApiKeyValidationService>();
}

else if (settings.ServerConfig == "Api")
{
    builder.Services.AddTransient<IAiResponseService, ApiResponseService>();
    builder.Services.AddTransient<IOpenAiModelManager, ApiModelManager>();
    //builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidationService>();
    builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidationService>();
}


builder.Services.AddTransient<IPromptSettingsService, PromptSettingsService>();
builder.Services.AddTransient<IUnityNetCoreServer, TcpServerService>();
builder.Services.AddTransient<IUnityNetCoreServer, UdpServerService>();




builder.Services.AddHostedService<UnityServerManagerService>();




var app = builder.Build();


//app.UseHttpsRedirection();


//app.UseAuthorization();

//app.MapControllers();

app.Run();
