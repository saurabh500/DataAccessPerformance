using System;
using System.IO;

namespace Microsoft.SqlServer.TDS
{
    /// <summary>
    /// Container for the packet data
    /// </summary>
    public abstract class TDSPacketToken: IInflatable, IDeflatable
    {
        /// <summary>
        /// Inflate the token
        /// </summary>
        /// <param name="source">Stream to inflate the token from</param>
        /// <returns>TRUE if inflation is complete</returns>
        public abstract bool Inflate(Stream source);

        /// <summary>
        /// Deflate the token
        /// </summary>
        /// <param name="destination">Stream to deflate token to</param>
        public abstract void Deflate(Stream destination);
    }
}
