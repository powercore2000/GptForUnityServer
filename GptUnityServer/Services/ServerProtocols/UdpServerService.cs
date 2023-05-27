using System;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using NetCoreServer;
using SharedLibrary;

namespace GptUnityServer.Services.ServerProtocols
{

    public class UdpServerService : UnityBasicProtocolServer
    {

        #region Class Definitions

        private class AiChatServer : UdpServer
        {
            public Action<string> OnAiMessageRecived;
            public Action<string> OnClientMessageRecived;
            private EndPoint userEndpoint;

            protected UdpServerService serverService;
            //public sessionGuids
            public AiChatServer(IPAddress address, int port, UdpServerService _serverService) : base(address, port)
            {

                serverService = _serverService;
                Console.WriteLine("Prepared to recive ai messages!");
                //serverService.OnAiMessageRecived += SendMessageToClient;
            }

            protected override void OnStarted()
            {
                // Start receive datagrams
                serverService.OnAiMessageRecived += SendMessageToClient;
                ReceiveAsync();
            }
            void SendMessageToClient(string message)
            {

                byte[] buffer = Encoding.UTF8.GetBytes(message);
                SendAsync(userEndpoint, buffer, 0, buffer.Length);
            }

            protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
            {

                string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
                Console.WriteLine($"Displaying client message:\n {message}\n");
                userEndpoint = endpoint;
                OnClientMessageRecived.Invoke(message);
            }

            protected override void OnSent(EndPoint endpoint, long sent)
            {
                // Continue receive datagrams
                ReceiveAsync();
            }

            protected override void OnError(SocketError error)
            {
                Console.WriteLine($"Echo UDP server caught an error with code {error}");
            }




            protected override void OnStopping()
            {
                serverService.OnAiMessageRecived += SendMessageToClient;
                base.OnStopping();
            }

        }

        #endregion

        AiChatServer? server;

        public UdpServerService(IServiceProvider _serviceProvider, PromptSettings _promptSettings) : base(_serviceProvider, _promptSettings)
        {
            serverType = "UDP";
        }


        #region Server Management
        public override void StartServer(int _port = 1111)
        {
            Console.WriteLine($"UDP server service has started!");
            int port = _port;
            Console.WriteLine($"UDP server port: {port}");
            Console.WriteLine("Started hosted server process!");
            // UDP server port      
            Console.WriteLine();
            server = new AiChatServer(IPAddress.Any, port, this);
            // Start the server
            Console.Write("Server starting...");
            server.Start();
            Console.WriteLine("Done!");

            Console.WriteLine("Press Enter to stop the server or '!' to restart the server...");


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
            server.OnClientMessageRecived += TriggerAiResponse;
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            server.OnClientMessageRecived -= TriggerAiResponse;
            StopServer();
            return Task.CompletedTask;
        }


        #endregion
    }
}
