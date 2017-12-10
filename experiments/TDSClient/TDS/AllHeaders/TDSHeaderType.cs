
namespace Microsoft.SqlServer.TDS.AllHeaders
{
    /// <summary>
    /// Type of the individual header
    /// </summary>
    public enum TDSHeaderType: ushort
    {
        QueryNotifications = 0x0001,
        TransactionDescriptor = 0x0002,
        Trace = 0x0003
    }
}
