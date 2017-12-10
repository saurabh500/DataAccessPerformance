
namespace Microsoft.SqlServer.TDS.EndPoint.SSPI
{
    /// <summary>
    /// Constants that are used accros security API
    /// </summary>
    internal static class SecConstants
    {
        /// <summary>
        /// Security packages used for SSPI authenication
        /// </summary>
        internal const string Negotiate = "negotiate";
        internal const string Kerberos = "kerberos";
        internal const string NTLM = "ntlm";
    }
}
