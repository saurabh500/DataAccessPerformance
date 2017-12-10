
namespace Microsoft.SqlServer.TDS.EndPoint.SSPI
{
    /// <summary>
    /// Result of security operation
    /// </summary>
    internal enum SecResult: uint
    {
        Ok = 0,
        ContinueNeeded = 0x90312,
        PackageNotFound = 0x80090305,
        CompleteAndContinue = 0x00090314,
        CompleteNeeded = 0x00090313
    }
}
