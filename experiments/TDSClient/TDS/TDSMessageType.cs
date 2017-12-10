
namespace Microsoft.SqlServer.TDS
{
    /// <summary>
    /// Type of TDS packet
    /// </summary>
    public enum TDSMessageType: byte
    {
        SQLBatch = 1,
        PreTDS7Login = 2,
        RPC = 3,
        Response = 4,
        Attention = 6,
        BulkLoad = 7,
        FederatedAuthenticationToken = 8,
        TransactionManager = 14,
        TDS7Login = 16,
        SSPI = 17,
        PreLogin = 18,
        FederatedAuthenticationInfo = 238
    }
}
