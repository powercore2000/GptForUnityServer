using GptToUnityServer.Services.UnityServerServices;
using GptToUnityServer.Services;
using GptToUnityServer.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton(settings);

builder.Services.AddTransient<IOpenAiService, GenericOpenAiService>();

builder.Services.AddHostedService<TcpServerService>();

var app = builder.Build();


app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
