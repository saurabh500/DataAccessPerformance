using System;

namespace Microsoft.SqlServer.TDS.PreLogin
{
    /// <summary>
    /// Type of encryption specified in the pre-login packet
    /// </summary>
    public enum TDSPreLoginTokenEncryptionType: byte
    {
        Off = 0x00,
        On = 0x01,
        NotSupported = 0x02,
        Required = 0x03
    }
}
