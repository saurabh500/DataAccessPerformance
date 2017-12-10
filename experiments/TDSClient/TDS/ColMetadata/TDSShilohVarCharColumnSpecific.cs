
namespace Microsoft.SqlServer.TDS.ColMetadata
{
    /// <summary>
    /// Metadata associated with Shiloh variable character column
    /// </summary>
    public class TDSShilohVarCharColumnSpecific
    {
        /// <summary>
        /// Length of the data
        /// </summary>
        public ushort Length { get; set; }

        /// <summary>
        /// Collation
        /// </summary>
        public TDSColumnDataCollation Collation { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TDSShilohVarCharColumnSpecific()
        {
        }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public TDSShilohVarCharColumnSpecific(ushort length)
        {
            Length = length;
        }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public TDSShilohVarCharColumnSpecific(ushort length, TDSColumnDataCollation collation)
        {
            Length = length;
            Collation = collation;
        }
    }
}
