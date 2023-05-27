using GptUnityServer.Models;
using GptUnityServer.Services.OobaUiServices;
using GptUnityServer.Services.KoboldAIServices;
using GptUnityServer.Services._PlaceholderServices;
using GptUnityServer.Services.UnityCloudCode;
using GptUnityServer.Services.ServerManagment;
using GptUnityServer.Services.OpenAiServices;
using GptUnityServer.Services.Universal;
using GptUnityServer.Services.ServerProtocols;
using Microsoft.AspNetCore.Mvc;
using GptUnityServer.Controllers;

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

//For Debuggin Purposes only. DO NOT CALL USE IN PRODUCTION
else if (settings.ServerServiceEnum == ServerServiceTypes.AiApi)
{
    builder.Services.AddTransient<IAiResponseService, ApiResponseService>();
    builder.Services.AddTransient<IAiModelManager, ApiModelManager>(); 
    builder.Services.AddTransient<IServerValidationService, AiApiKeyValidationService>();
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

//Add all protocol NetCoreServer Types here
builder.Services.AddTransient<IUnityProtocolServer, TcpServerService>();
builder.Services.AddTransient<IUnityProtocolServer, UdpServerService>();
builder.Services.AddTransient<IUnityProtocolServer, RestApiServerService>();

//builder.Services.AddHostedService<UnityServerManagerService>();
builder.Services.AddSingleton<UnityServerManagerService>();
builder.Services.AddHostedService<UnityServerManagerService>(provider => provider.GetService<UnityServerManagerService>());

if (settings.ServerProtocolEnum == ServerProtocolTypes.RestApi)
{

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}




var app = builder.Build();



if (settings.ServerProtocolEnum == ServerProtocolTypes.RestApi)
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
