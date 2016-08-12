using System;
using System.Linq;
using SHWDTech.Platform.Utility;

namespace Unicom.Commander.Client
{
    public class CommanderClient
    {
        private readonly CommanderTcpClient _tcpClient;

        /// <summary>
        /// 是否在解码协议
        /// </summary>
        private bool _decoding;

        /// <summary>
        /// 协议解码失败次数
        /// </summary>
        private byte _decodeErrorTimes;

        private byte[] _frameHead = {53, 48};

        private byte[] _frameTail = {57, 44};

        public CommanderClient(CommanderTcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _tcpClient.ClientReceivedDataEvent += OnReceivedData;
        }

        private void OnReceivedData(ICommanderConnection conn)
        {
            if(!_decoding) Process();
        }

        private void Process()
        {
            lock (_tcpClient.ProcessBuffer)
            {
                _decoding = true;

                while (_tcpClient.ProcessBuffer.Count > 0)
                {
                    try
                    {
                        Decode(_tcpClient.ProcessBuffer.ToArray());
                    }
                    catch (Exception ex)
                    {
                        LogService.Instance.Warn("解析通信数据错误。", ex);
                        _decodeErrorTimes++;
                        if (_decodeErrorTimes == 5)
                        {
                            _tcpClient.ProcessBuffer.Clear();
                            _decodeErrorTimes = 0;
                        }
                    }
                }
            }
        }

        private void Decode(byte[] bufferBytes)
        {
            
        }


    }
}
