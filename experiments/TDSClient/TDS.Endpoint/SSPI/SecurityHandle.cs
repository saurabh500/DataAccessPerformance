using System;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.TDS.EndPoint.SSPI
{
    /// <summary>
    /// Security handle
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecurityHandle
    {
        public IntPtr LowPart;
        public IntPtr HighPart;

        /// <summary>
        /// Check if instance of the security handle is valid
        /// </summary>
        internal bool IsValid()
        {
            return (LowPart != IntPtr.Zero) || (HighPart != IntPtr.Zero);
        }
    }
}
