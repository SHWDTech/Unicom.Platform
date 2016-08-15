using System;
using Newtonsoft.Json;
using SHWDTech.Platform.Utility;

namespace Unicom.Commander
{
    public class CommandPackage
    {
        public CommandPackage(byte[] receivedBytes)
        {
            _commandBytes = receivedBytes;
            CommandDateTime = DateTime.Now;
            WarpperString = Globals.ByteArrayToUtf8String(receivedBytes.SubBytes(4, receivedBytes.Length - 4));
            Warpper = JsonConvert.DeserializeObject<CommandWarpper>(WarpperString);
        }

        public bool Finalized { get; private set; }

        public int PackageLenth => _commandBytes.Length;

        private readonly byte[] _commandBytes;

        public DateTime CommandDateTime { get; private set; }

        public PackageStatus Status { get; set; } = PackageStatus.UnFinalized;

        public CommandWarpper Warpper { get; private set; }

        public string WarpperString { get; }
    }
}
