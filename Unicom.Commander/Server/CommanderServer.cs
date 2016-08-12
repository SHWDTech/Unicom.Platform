using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using Unicom.Commander.Client;

namespace Unicom.Commander.Server
{
    /// <summary>
    /// 指令通信服务器
    /// </summary>
    public class CommanderServer
    {
        private Socket _serverSocket;

        private readonly List<CommanderTcpClient> _clientSockets = new List<CommanderTcpClient>();

        private Timer _routeingTimer;

        public void Start(IPAddress ipAddress, int port)
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(ipAddress, port));
            _serverSocket.LingerState = new LingerOption(false, 1);
            _serverSocket.Listen(256);
            _serverSocket.BeginAccept(AcceptClient, _serverSocket);
        }

        private void AcceptClient(IAsyncResult result)
        {
            var server = (Socket)result.AsyncState;

            var client = server.EndAccept(result);

            _clientSockets.Add(new CommanderTcpClient(client));

            server.BeginAccept(AcceptClient, server);
        }
    }
}
