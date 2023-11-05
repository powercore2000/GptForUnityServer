using GptForUnityServer.Services._MockServices;
using GptForUnityServer.Services.EmotionClassificationServices;
using GptForUnityServer.Services.ServerManagment;
using GptForUnityServer.Services.Universal;
using GptUnityServer.Models;
using GptUnityServer.Services._PlaceholderServices;
using GptUnityServer.Services.AiApiServices;
using GptUnityServer.Services.KoboldAIServices;
using GptUnityServer.Services.OobaUiServices;
using GptUnityServer.Services.ServerManagment;
using GptUnityServer.Services.ServerProtocols;
using GptUnityServer.Services.UnityCloud;
using GptUnityServer.Services.Universal;
using Microsoft.OpenApi.Models;
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

if (settings.ServerServiceEnum == AiChatServiceTypes.AiApi)
    aiApiSetup.RunSetUp(args);
else if (settings.ServerServiceEnum == AiChatServiceTypes.UnityCloud)
    unityCloudSetupData.RunSetUp(args);


builder.Services.AddSingleton(settings);
builder.Services.AddSingleton(aiApiSetup);
builder.Services.AddSingleton(unityCloudSetupData);
builder.Services.AddSingleton(promptSettings);

builder.Services.AddTransient<IEmotionClassificationService, MockEmotionClassificationService>();
builder.Services.AddTransient<IEmotionClassificationService, SillyTavernExtraSimpleClassifyService>();

//Ai Model Manager
builder.Services.AddTransient<IAiModelManager, CloudModelManager>();
builder.Services.AddTransient<IAiModelManager, AiApiModelManager>();
builder.Services.AddTransient<IAiModelManager, MockAiModelManagerService>();

builder.Services.AddTransient<IAiInstructService, CloudInstructService>();
builder.Services.AddTransient<IAiInstructService, AiApiInstructService>();
builder.Services.AddTransient<IAiInstructService, OobaUiInstructService>();
builder.Services.AddTransient<IAiInstructService, KoboldAiInstructService>();
builder.Services.AddTransient<IAiInstructService, MockAiInstructService>();

builder.Services.AddTransient<IKeyValidationService, CloudCodeValidationServices>();
builder.Services.AddTransient<IKeyValidationService, AiApiKeyValidationService>();
builder.Services.AddTransient<IKeyValidationService, OfflineApiKeyValidationService>();

builder.Services.AddTransient<IAiChatService, CloudChatService>();
builder.Services.AddTransient<IAiChatService, AiApiChatService>();
builder.Services.AddTransient<IAiChatService, OobaUiChatService>();
builder.Services.AddTransient<IAiChatService, KoboldAIChatService>();
builder.Services.AddTransient<IAiChatService, MockAiChatService>();


builder.Services.AddTransient<IPromptSettingsService, PromptSettingsService>();

//Add all protocol NetCoreServer Types here
builder.Services.AddTransient<IUnityProtocolServer, TcpServerService>();
builder.Services.AddTransient<IUnityProtocolServer, UdpServerService>();
builder.Services.AddTransient<IUnityProtocolServer, RestApiServerService>();

builder.Services.AddSingleton<ModularServiceSelector>();
builder.Services.AddHostedService<UnityServerManagerService>();
if (settings.ServerProtocolEnum == ServerProtocolTypes.HTTP)
{

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1.1",
            Title = "GptForUnityServer API v1.1",
            Description = "Used for all modern versions of the GptForUnityServer"
        });
        

        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    });
}




var app = builder.Build();

//app.Services.GetRequiredService<UnityServerManagerService>().StartServer();

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


app.Run();
