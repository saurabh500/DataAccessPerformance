
namespace Microsoft.SqlServer.TDS
{
    /// <summary>
    /// Types of encryption TDS clients and servers supports
    /// </summary>
    public enum TDSEncryptionType
    {
        /// <summary>
        /// No transport encryption
        /// </summary>
        Off,

        /// <summary>
        /// Encryption of the login packet only
        /// </summary>
        LoginOnly,

        /// <summary>
        /// Encryption of the entire session
        /// </summary>
        Full
    }
}
