using System.Collections.Generic;

namespace Microsoft.SqlServer.TDS
{
    /// <summary>
    /// Collection of TDS messages
    /// </summary>
    public class TDSMessageCollection: List<TDSMessage>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TDSMessageCollection()
        {
        }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public TDSMessageCollection(params TDSMessage[] messages)
        {
            AddRange(messages);
        }

        /// <summary>
        /// Protocol-aware deflation routine
        /// </summary>
        /// <param name="stream">Destination to deflate the message</param>
        public void Deflate(TDSStream stream)
        {
            // Iterate through each message
            foreach (TDSMessage current in this)
            {
                // Deflate the message
                current.Deflate(stream);
            }
        }
    }
}
