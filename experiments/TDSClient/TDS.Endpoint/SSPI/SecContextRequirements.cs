
namespace Microsoft.SqlServer.TDS.EndPoint.SSPI
{
    /// <summary>
    /// Security context requirements
    /// </summary>
    internal enum SecContextRequirements: int
    {
        Delegate = 0x00000001,
        MutualAuthentication = 0x00000002,
        Integrity = 0x00010000,
        ExtendedError = 0x00004000
    }
}
