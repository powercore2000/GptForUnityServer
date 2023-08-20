using GptUnityServer.Models;
using GptUnityServer.Services._PlaceholderServices;
using GptUnityServer.Services.AiApiServices;
using GptUnityServer.Services.KoboldAIServices;
using GptUnityServer.Services.OobaUiServices;
using GptUnityServer.Services.ServerManagment;
using GptUnityServer.Services.ServerProtocols;
using GptUnityServer.Services.UnityCloud;
using GptUnityServer.Services.Universal;
using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);

var promptSettings = new SharedLibrary.PromptSettings();
//builder.Configuration.Bind("", promptSettings);

var settings = new Settings();
var aiApiSetup = new AiApiSetupData();
var unityCloudSetupData = new UnityCloudSetupData();
//var 
builder.Configuration.Bind("Settings", settings);
builder.Configuration.Bind("AiApiSetup", aiApiSetup);
builder.Configuration.Bind("UnityCloudSetup", unityCloudSetupData);

settings.RunSetUp(args);

if (settings.ServerServiceEnum == ServerServiceTypes.AiApi)
    aiApiSetup.RunSetUp(args);
else if (settings.ServerServiceEnum == ServerServiceTypes.UnityCloud)
    unityCloudSetupData.RunSetUp(args);


builder.Services.AddSingleton(settings);
builder.Services.AddSingleton(aiApiSetup);
builder.Services.AddSingleton(unityCloudSetupData);
builder.Services.AddSingleton(promptSettings);


//Provide Cloud Access to Ai Endpoints in Unity's Cloud Code Infrastructure
if (settings.ServerServiceEnum == ServerServiceTypes.UnityCloud)
{
    builder.Services.AddTransient<IAiResponseService, CloudResponseService>();
    builder.Services.AddTransient<IAiModelManager, CloudModelManager>();
    builder.Services.AddTransient<IKeyValidationService, CloudCodeValidationServices>();
    builder.Services.AddTransient<IAiChatResponseService, CloudChatResponseService>();
}

//For Debuggin Purposes mainly. DO NOT USE IN PRODUCTION UNLESS YOU KNOW WHAT YOU ARE DOING
else if (settings.ServerServiceEnum == ServerServiceTypes.AiApi)
{
    builder.Services.AddTransient<IAiResponseService, AiApiResponseService>();
    builder.Services.AddTransient<IAiModelManager, AiApiModelManager>(); 
    builder.Services.AddTransient<IKeyValidationService, AiApiKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, AiApiChatResponseService>();
}

//Assign services to point to Ooba Text Generation Web Ui's Api
else if (settings.ServerServiceEnum == ServerServiceTypes.OobaUi)
{
    builder.Services.AddTransient<IAiResponseService, OobaUiResponseService>();
    builder.Services.AddTransient<IAiModelManager, MockAiModelManagerService>();
    builder.Services.AddTransient<IKeyValidationService, MockApiKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, OobaUiChatService>();
}

//Assign services to point to Kobold Ai's Api
else if (settings.ServerServiceEnum == ServerServiceTypes.KoboldAi)
{
    builder.Services.AddTransient<IAiResponseService, KoboldAiResponseService>();
    builder.Services.AddTransient<IAiModelManager, MockAiModelManagerService>();
    builder.Services.AddTransient<IKeyValidationService, MockApiKeyValidationService>();
    builder.Services.AddTransient<IAiChatResponseService, KoboldAIChatService>();
}




builder.Services.AddTransient<IPromptSettingsService, PromptSettingsService>();

//Add all protocol NetCoreServer Types here
builder.Services.AddTransient<IUnityProtocolServer, TcpServerService>();
builder.Services.AddTransient<IUnityProtocolServer, UdpServerService>();
builder.Services.AddTransient<IUnityProtocolServer, RestApiServerService>();

builder.Services.AddSingleton<UnityServerManagerService>();
builder.Services.AddHostedService<UnityServerManagerService>(provider => provider.GetService<UnityServerManagerService>());

if (settings.ServerProtocolEnum == ServerProtocolTypes.HTTP)
{

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}




var app = builder.Build();



if (settings.ServerProtocolEnum == ServerProtocolTypes.HTTP)
{

    Console.WriteLine("Rest Api connect");
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        Console.WriteLine("Dev enviroment");
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

}



//app.Run("http://localhost:6776");
app.Run();
