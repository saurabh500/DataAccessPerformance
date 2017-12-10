
namespace Microsoft.SqlServer.TDS.ColMetadata
{
    /// <summary>
    /// Metadata associated with decimal/numeric column
    /// </summary>
    public class TDSDecimalColumnSpecific
    {
        /// <summary>
        /// Length of the data
        /// </summary>
        public byte Length { get; set; }

        /// <summary>
        /// Precision of the data
        /// </summary>
        public byte Precision { get; set; }

        /// <summary>
        /// Scale of the data
        /// </summary>
        public byte Scale { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TDSDecimalColumnSpecific()
        {
        }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public TDSDecimalColumnSpecific(byte length)
        {
            Length = length;
        }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public TDSDecimalColumnSpecific(byte length, byte precision)
        {
            Length = length;
            Precision = precision;
        }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public TDSDecimalColumnSpecific(byte length, byte precision, byte scale)
        {
            Length = length;
            Precision = precision;
            Scale = scale;
        }
    }
}
