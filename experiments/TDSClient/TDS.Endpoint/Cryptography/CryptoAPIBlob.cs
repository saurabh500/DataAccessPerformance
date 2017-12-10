using System;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.TDS.EndPoint.Cryptography
{
    /// <summary>
    /// Cryptography blob
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct CryptoAPIBlob
    {
        internal int DataLength;
        internal IntPtr Data;

        internal CryptoAPIBlob(int dataLength, IntPtr data)
        {
            this.DataLength = dataLength;
            this.Data = data;
        }
    }
}
