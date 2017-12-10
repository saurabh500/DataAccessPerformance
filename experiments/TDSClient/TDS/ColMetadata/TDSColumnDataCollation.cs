﻿
namespace Microsoft.SqlServer.TDS.ColMetadata
{
    /// <summary>
    /// Collation associated with TDS column
    /// </summary>
    public class TDSColumnDataCollation
    {
        /// <summary>
        /// I have no clue what this stands for
        /// </summary>
        public uint WCID { get; set; }

        /// <summary>
        /// Sort identifier
        /// </summary>
        public byte SortID { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TDSColumnDataCollation()
        {
        }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public TDSColumnDataCollation(uint wcid)
        {
            WCID = wcid;
        }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public TDSColumnDataCollation(uint wcid, byte sortID)
        {
            WCID = wcid;
            SortID = sortID;
        }
    }
}
