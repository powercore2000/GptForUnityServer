using GptToUnityServer.Services.UnityServerServices;
using GptToUnityServer.Services;
using GptToUnityServer.Models;
using Microsoft.Extensions.DependencyInjection;
using GptToUnityServer.Services.UnityServerManager;

var builder = WebApplication.CreateBuilder(args);

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton(settings);

builder.Services.AddTransient<IOpenAiService, GenericOpenAiService>();

builder.Services.AddTransient<IUnityNetCoreServer, TcpServerService>();
builder.Services.AddTransient<IUnityNetCoreServer, UdpServerService>();


builder.Services.AddHostedService<UnityServerManagerService>();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();


app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
