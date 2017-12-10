using System;

namespace Microsoft.SqlServer.TDS.Done
{
    /// <summary>
    /// Status of the token
    /// </summary>
    [Flags]
    public enum TDSDoneTokenStatusType: ushort
    {
        Final = 0x00,
        More = 0x01,
        Error = 0x02,
        TransactionInProgress = 0x04,
        Count = 0x10,
        Attention = 0x20,
        RPCInBatch = 0x80,
        ServerError = 0x100,
    }
}
