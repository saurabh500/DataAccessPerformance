using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.TDS.EndPoint.SSPI
{
    /// <summary>
    /// Security integer
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecurityInteger
    {
        public uint LowPart;
        public int HighPart;
    }
}
