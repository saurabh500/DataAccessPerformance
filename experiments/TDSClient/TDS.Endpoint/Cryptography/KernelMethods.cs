using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.TDS.EndPoint.Cryptography
{
    /// <summary>
    /// Wrappers for native method calls
    /// </summary>
    internal class KernelMethods
    {
        /// <summary>
        /// This function converts a 64-bit file time to system time format.
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FileTimeToSystemTime(
            [In] ref long fileTime,
            out SystemTime systemTime);
    }
}
