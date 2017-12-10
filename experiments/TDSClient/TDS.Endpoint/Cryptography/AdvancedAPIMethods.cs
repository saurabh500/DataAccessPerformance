using System;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.TDS.EndPoint.Cryptography
{
    /// <summary>
    /// Wrappers for native method calls
    /// </summary>
    internal class AdvancedAPIMethods
    {
        /// <summary>
        /// The CryptAcquireContext function is used to acquire a handle to a particular key container within a particular cryptographic service provider (CSP). This returned handle is used in calls to CryptoAPI functions that use the selected CSP.
        /// </summary>
        [DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptAcquireContextW(
            out IntPtr providerContext,
            [MarshalAs(UnmanagedType.LPWStr)] string container,
            [MarshalAs(UnmanagedType.LPWStr)] string provider,
            int providerType,
            int flags);

        /// <summary>
        /// The CryptReleaseContext function releases the handle of a cryptographic service provider (CSP) and a key container. At each call to this function, the reference count on the CSP is reduced by one. When the reference count reaches zero, the context is fully released and it can no longer be used by any function in the application.
        /// </summary>
        [DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptReleaseContext(
            IntPtr providerContext,
            int flags);

        /// <summary>
        /// The CryptGenKey function generates a random cryptographic session key or a internal/private key pair. A handle to the key or key pair is returned in phKey. This handle can then be used as needed with any CryptoAPI function that requires a key handle.
        /// </summary>
        [DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptGenKey(
            IntPtr providerContext,
            int algorithmId,
            int flags,
            out IntPtr cryptKeyHandle);

        /// <summary>
        /// The CryptDestroyKey function releases the handle referenced by the hKey parameter. After a key handle has been released, it is no longer valid and cannot be used again.
        /// </summary>
        [DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDestroyKey(
            IntPtr cryptKeyHandle);
    }
}
