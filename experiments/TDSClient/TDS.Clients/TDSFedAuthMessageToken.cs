using System.IO;

namespace Microsoft.SqlServer.TDS.Clients
{
    internal class TDSFedAuthMessageToken : TDSPacketToken
    {
        public byte[] Nonce { get; internal set; }
        public byte[] Data { get; internal set; }

        public override void Deflate(Stream destination)
        {
            
        }

        public override bool Inflate(Stream source)
        {
            return false;
        }
    }
}