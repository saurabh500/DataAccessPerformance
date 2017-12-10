
namespace Microsoft.SqlServer.TDS.ColMetadata
{
    /// <summary>
    /// Indicates type of updatability of the column
    /// </summary>
    public enum TDSColumnDataUpdatableFlag: byte
    {
        ReadOnly = 0,
        ReadWrite = 1,
        Unknown = 2
    }
}
