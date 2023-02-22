using GptUnityServer.Models;
using Microsoft.Extensions.DependencyInjection;
using GptUnityServer.Services.OpenAiServices;
using GptUnityServer.Services.OpenAiServices.Api_Validation;
using GptUnityServer.Services.ServerManagment.ServerManagerServices;
using GptUnityServer.Services.ServerManagment.UnityServerServices;
using GptUnityServer.Services.OpenAiServices.PromptServices;
using GptUnityServer.Services.OpenAiServices.OpenAiData.PromptSettings;
using GptUnityServer.Services.OpenAiServices.OpenAiData.ModelListing;

var builder = WebApplication.CreateBuilder(args);

var promptSettings = new SharedLibrary.PromptSettings();
//builder.Configuration.Bind("", promptSettings);

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);


settings.RunSetUp(args);


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton(settings);
builder.Services.AddSingleton(promptSettings);

if(settings.ServerConfig == "Cloud")
    builder.Services.AddTransient<IOpenAiPromptService, CloudPromptService>();

else if(settings.ServerConfig == "Api")
    builder.Services.AddTransient<IOpenAiPromptService, ApiPromptService>();


builder.Services.AddTransient<IOpenAiModelManager, ApiModelManager>();
builder.Services.AddTransient<IPromptSettingsService, PromptSettingsService>();
builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidationService>();

builder.Services.AddTransient<IUnityNetCoreServer, TcpServerService>();
builder.Services.AddTransient<IUnityNetCoreServer, UdpServerService>();




builder.Services.AddHostedService<UnityServerManagerService>();




var app = builder.Build();


app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
