
namespace Microsoft.SqlServer.TDS.PreLogin
{
    /// <summary>
    /// Types of the tokens in TDS prelogin packet
    /// </summary>
    public enum TDSPreLoginTokenOptionType: byte
    {
        Version = 0x00,
        Encryption = 0x01,
        Instance = 0x02,
        ThreadID = 0x03,
        Mars = 0x04,
		TraceID = 0x05,
		FederatedAuthenticationRequired = 0x06,
		NonceOption = 0x07,
        Terminator = 0xff
    }
}
