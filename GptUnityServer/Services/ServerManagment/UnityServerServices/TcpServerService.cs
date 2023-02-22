using System;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using NetCoreServer;

namespace GptUnityServer.Services.ServerManagment.UnityServerServices
{

    public class TcpServerService : UnityNetCoreServer
    {

        #region Class Definitions
        private class AiChatSession : TcpSession
        {

            public Action<string> OnClientMessageRecived;
            public Action OnClientConnect;
            public AiChatSession(TcpServer server) : base(server)
            {

                if (server is AiChatServer)
                {

                    AiChatServer chatServer = server as AiChatServer;
                    chatServer.OnReciveAiMessage += MulticastWrapper;
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
                    OnClientConnect.Invoke();
                    OnClientMessageRecived += chatServer.OnReciveClientMessage.Invoke;
                }
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
            public Action<string>? OnReciveAiMessage;
            public Action<string>? OnReciveClientMessage;
            public Action? OnClientConnect;


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

            protected override void OnConnecting(TcpSession session)
            {
                base.OnConnecting(session);
                if (session is AiChatSession)
                {
                    Console.WriteLine("An ai session is connecting!");
                    AiChatSession? aiChatSession = session as AiChatSession;
                    aiChatSession.OnClientConnect += OnClientConnect.Invoke;

                    //When a valid session connects to the server,
                    //If the service hasnt added the servers delegates yet, add them as now they have been filled to be valid
                    if (serverService.OnAiMessageRecived == null)
                        serverService.OnAiMessageRecived += OnReciveAiMessage.Invoke;
                }
            }

            protected override void OnDisconnecting(TcpSession session)
            {

                if (session is AiChatSession)
                {

                    AiChatSession? aiChatSession = session as AiChatSession;
                    aiChatSession.OnClientMessageRecived -= OnReciveClientMessage.Invoke;
                    aiChatSession.OnClientConnect -= OnClientConnect.Invoke;


                }
            }

            protected override void OnStopping()
            {
                serverService.OnAiMessageRecived -= OnReciveAiMessage;
                base.OnStopping();
            }

        }

        #endregion



        #region Fields

        AiChatServer? server;


        public Action OnClientConnect;


        #endregion

        #region Constructor


        public TcpServerService(IServiceProvider _serviceProvider) : base(_serviceProvider)
        {

            OnClientConnect += delegate { Console.WriteLine("Client connected!"); };
            onValidationFail += delegate { server?.DisconnectAll(); };
            serverType = "TCP";
        }

        #endregion



        #region UnityNetCoreServer Methods

        #endregion

        #region Server Management
        public override void StartServer(int _port = 1111)
        {
            Console.WriteLine($"TCP server service has started!");
            int port = _port;
            Console.WriteLine($"TCP server port: {port}");
            // TCP server port      
            Console.WriteLine();
            server = new AiChatServer(IPAddress.Any, port, this);
            // Start the server
            Console.Write("Server starting...");
            server.Start();
            Console.WriteLine("Done!");


        }

        public override void StopServer()
        {

            // Stop the server
            Console.Write("Server stopping...");
            server?.Stop();
            Console.WriteLine("Done!");
        }

        public override void RestartServer()
        {
            Console.Write("Server restarting...");
            server?.Restart();
            Console.WriteLine("Done!");
        }
        #endregion

        #region Service Managment
        public override Task StartAsync(CancellationToken cancellationToken, bool _isKeyValid, Action _onFailedValidation)
        {
            base.StartAsync(cancellationToken, _isKeyValid, _onFailedValidation);
            StartServer();
            server.OnReciveClientMessage += TriggerAiResponse;
            server.OnClientConnect += OnClientConnect.Invoke;
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            server.OnReciveClientMessage -= TriggerAiResponse;
            server.OnClientConnect -= OnClientConnect.Invoke;
            StopServer();
            return Task.CompletedTask;
        }


        #endregion
    }
}
