using System;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.TDS.EndPoint.Cryptography
{
    /// <summary>
    /// Structure that contains provider information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct CryptKeyProviderInfo
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string ContainerName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string ProviderName;
        public int ProviderType;
        public int Flags;
        public int ProviderParameterCount;
        public IntPtr ProviderParameters; // PCRYPT_KEY_PROV_PARAM
        public int KeySpec;
    }
}
