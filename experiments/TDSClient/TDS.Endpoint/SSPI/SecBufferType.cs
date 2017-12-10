
namespace Microsoft.SqlServer.TDS.EndPoint.SSPI
{
    /// <summary>
    /// Type of security buffer
    /// </summary>
    internal enum SecBufferType: int
    {
        Empty = 0,
        Data = 1,
        Token = 2
    }
}
