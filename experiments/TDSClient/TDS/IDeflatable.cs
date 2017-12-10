using System.IO;

namespace Microsoft.SqlServer.TDS
{
    /// <summary>
    /// Interface that enables object deflation
    /// </summary>
    public interface IDeflatable
    {
        /// <summary>
        /// Deflate the object into a byte stream
        /// </summary>
        void Deflate(Stream destination);
    }
}
