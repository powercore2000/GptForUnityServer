using System;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using NetCoreServer;
using GptToUnityServer.Services;
using SharedLibrary;

namespace GptToUnityServer.Services.UnityServerServices
{

    public class TcpServerService : IUnityNetCoreServer
    {

        #region Class Definitions
        private class AiChatSession : TcpSession
        {

            public Action<string> OnClientMessageRecived;

            public AiChatSession(TcpServer server) : base(server)
            {

                if (server is AiChatServer)
                {

                    AiChatServer chatServer = server as AiChatServer;

                }

            }


            void MulticastWrapper(string message)
            {

                Server.Multicast(message);
            }


            protected override void OnConnected()
            {
                Console.WriteLine($"Chat TCP session with Id {Id} connected!");
                if (Server is AiChatServer)
                {
                    AiChatServer chatServer = Server as AiChatServer;
                    chatServer.OnReciveAiMessage += MulticastWrapper;
                }
                // Send invite message
                string message = "Hello from TCP chat! Your connection to the Unity Net Core Server has been established!";
                SendAsync(message);
            }

            protected override void OnDisconnected()
            {

                Console.WriteLine($"Chat TCP session with Id {Id} disconnected!");
                if (Server is AiChatServer)
                {
                    AiChatServer chatServer = Server as AiChatServer;
                    chatServer.OnReciveAiMessage -= MulticastWrapper;
                }
            }

            protected override void OnReceived(byte[] buffer, long offset, long size)
            {
                string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
                Console.WriteLine("Displaying client message: " + message);

                OnClientMessageRecived.Invoke(message);

                // If the buffer starts with '!' the disconnect the current session
                if (message == "!")
                    Disconnect();
            }

            protected override void OnError(SocketError error)
            {
                Console.WriteLine($"Chat TCP session caught an error with code {error}");
            }
        }

        private class AiChatServer : TcpServer
        {
            public Action<string> OnReciveAiMessage;
            public Action<string> OnReciveClientMessage;

            protected TcpServerService serverService;
            //public sessionGuids
            public AiChatServer(IPAddress address, int port, TcpServerService _serverService) : base(address, port)
            {

                serverService = _serverService;
                Console.WriteLine("Prepared to recive ai messages!");

            }

            protected override TcpSession CreateSession()
            {

                return new AiChatSession(this);

            }

            protected override void OnError(SocketError error)
            {
                Console.WriteLine($"Chat TCP server caught an error with code {error}");
            }


            protected override void OnConnected(TcpSession session)
            {

                if (session is AiChatSession)
                {

                    AiChatSession aiChatSession = session as AiChatSession;
                    aiChatSession.OnClientMessageRecived += OnReciveClientMessage.Invoke;

                    //When a valid session connects to the server,
                    //If the service hasnt added the servers delegates yet, add them as now they have been filled to be valid
                    if (serverService.OnReciveAiMessage == null)
                        serverService.OnReciveAiMessage += OnReciveAiMessage.Invoke;

                }
            }

            protected override void OnDisconnecting(TcpSession session)
            {

                if (session is AiChatSession)
                {

                    AiChatSession aiChatSession = session as AiChatSession;
                    aiChatSession.OnClientMessageRecived -= OnReciveClientMessage.Invoke;



                }
            }

            protected override void OnStopping()
            {
                serverService.OnReciveAiMessage -= OnReciveAiMessage;
                base.OnStopping();
            }

        }

        #endregion



        #region Fields

        int port = 0;
        AiChatServer server;

        public Action<string> OnReciveAiMessage;
        #endregion

        #region Constructor
        private readonly IServiceProvider serviceProvider;

        public TcpServerService(IServiceProvider _serviceProvider)
        {

            serviceProvider = _serviceProvider;

        }

        #endregion



        #region UnityNetCoreServer Methods
        public async Task<string> SendMessage(string message)
        {

            // The scope informs the service provider when you're
            // done with the transient service so it can be disposed
            using (var scope = serviceProvider.CreateScope())
            {
                IOpenAiService openAiService = scope.ServiceProvider.GetRequiredService<IOpenAiService>();
                AiResponse response = await openAiService.SendMessage(message);
                return response.Message;
            }

        }

        async void TriggerAiResponse(string clientMessage)
        {

            string aiResponse = await SendMessage(clientMessage);
            Console.WriteLine($"displaying Ai response: {aiResponse}");
            OnReciveAiMessage.Invoke(aiResponse);
        }
        #endregion

        #region Server Management
        public void StartServer(int _port = 1111)
        {
            Console.WriteLine($"TCP server service has started!");
            int port = _port;
            Console.WriteLine($"TCP server port: {port}");
            Console.WriteLine("Started hosted server process!");
            // TCP server port      
            Console.WriteLine();
            server = new AiChatServer(IPAddress.Any, port, this);
            // Start the server
            Console.Write("Server starting...");
            server.Start();
            Console.WriteLine("Done!");

            Console.WriteLine("Press Enter to stop the server or '!' to restart the server...");


        }

        public void StopServer()
        {

            // Stop the server
            Console.Write("Server stopping...");
            server?.Stop();
            Console.WriteLine("Done!");
        }

        public void RestartServer()
        {
            Console.Write("Server restarting...");
            server?.Restart();
            Console.WriteLine("Done!");
        }
        #endregion

        #region Service Managment
        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartServer();
            server.OnReciveClientMessage += TriggerAiResponse;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            server.OnReciveClientMessage -= TriggerAiResponse;
            StopServer();
            return Task.CompletedTask;
        }
        #endregion
    }
}
