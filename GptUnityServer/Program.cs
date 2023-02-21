using GptUnityServer.Services.UnityServerServices;
using GptUnityServer.Models;
using Microsoft.Extensions.DependencyInjection;
using GptUnityServer.Services.ServerManagerServices;
using GptUnityServer.Services.OpenAiServices;
using GptUnityServer.Services.OpenAiServices.Api_Validation;
using GptUnityServer.Services.OpenAiServices.PromptSending;
using GptUnityServer.Services.ServerSetup;
using GptUnityServer.Services.OpenAiServices.PromptSettings;

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
    builder.Services.AddTransient<IOpenAiPromptService, CloudFunctionPromptService>();

else if(settings.ServerConfig == "Api")
    builder.Services.AddTransient<IOpenAiPromptService, GenericOpenAiService>();

builder.Services.AddTransient<IServerSetupService, ServerSetupService>();
builder.Services.AddTransient<IPromptSettingsService, PromptSettingsService>();
builder.Services.AddTransient<IApiKeyValidation, TestApiKeyValidationService>();

builder.Services.AddTransient<IUnityNetCoreServer, TcpServerService>();
builder.Services.AddTransient<IUnityNetCoreServer, UdpServerService>();




builder.Services.AddHostedService<UnityServerManagerService>();




var app = builder.Build();


app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
