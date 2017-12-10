using System.IO;

namespace Microsoft.SqlServer.TDS
{
    /// <summary>
    /// Interface that enables partial, sequential and continuable object inflation
    /// </summary>
    public interface IInflatable
    {
        /// <summary>
        /// Inflate the object using the next available chunk of byte stream
        /// </summary>
        /// <param name="next">Next chunk of data, not necessarily sufficient to inflate the object</param>
        /// <returns>TRUE if object is completely inflated, FALSE otherwise</returns>
        bool Inflate(Stream source);
    }
}
