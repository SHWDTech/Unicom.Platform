using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Unicom.Commander.Client
{
    public class CommanderClient : ICommanderConnection
    {
        private readonly Socket _clientSocket;

        public bool IsConnected { get; private set; }

        /// <summary>
        /// 数据接收缓存
        /// </summary>
        public IList<ArraySegment<byte>> ReceiveBuffer { get; }
            = new List<ArraySegment<byte>>() { new ArraySegment<byte>(new byte[4096]) };

        /// <summary>
        /// 数据处理缓存
        /// </summary>
        public IList<byte> ProcessBuffer { get; } = new List<byte>();

        /// <summary>
        /// 客户端数据接收事件
        /// </summary>
        public event ClientReceivedDataEventHandler ClientReceivedDataEvent;

        /// <summary>
        /// 客户端断开事件
        /// </summary>
        public event ClientDisconnectEventHandler ClientDisconnectEvent;

        public CommanderClient(Socket clientSocket)
        {
            _clientSocket = clientSocket;
            _clientSocket.BeginReceive(ReceiveBuffer, SocketFlags.None, Received, _clientSocket);
        }

        private void Received(IAsyncResult result)
        {
            var client = (Socket)result.AsyncState;

            lock (ReceiveBuffer)
            {
                int readCount;
                try
                {
                    readCount = client.EndReceive(result);

                    var array = ReceiveBuffer.Last().Array;
                    for (var i = 0; i < readCount; i++)
                    {
                        ProcessBuffer.Add(array[i]);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message == "远程主机强迫关闭了一个现有的连接。")
                    {
                        client.Close();
                        IsConnected = false;
                    }

                    OnClientDisconnect();
                    return;
                }

                if (readCount <= 0)
                {
                    OnClientDisconnect();
                    client.Close(0);
                    IsConnected = false;
                    return;
                }
            }

            OnReceivedData();

            client.BeginReceive(ReceiveBuffer, SocketFlags.None, Received, client);
        }

        private void SocketSend(byte[] sendBytes)
        {
            _clientSocket.Send(sendBytes);
        }

        /// <summary>
        /// 数据接收时出发
        /// </summary>
        private void OnReceivedData()
        {
            ClientReceivedDataEvent?.Invoke(this);
        }

        /// <summary>
        /// 断开连接时触发
        /// </summary>
        private void OnClientDisconnect()
        {
            ClientDisconnectEvent?.Invoke(this);
        }
    }
}
