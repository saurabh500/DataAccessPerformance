using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.SqlServer.TDS.Authentication
{
    /// <summary>
    /// A single option of the feature extension acknowledgement block
    /// </summary>
    public abstract class TDSFedAuthInfoOption : IDeflatable, IInflatable
    {
        /// <summary>
        /// FedAuth Info Identifier.
        /// </summary>
        public abstract TDSFedAuthInfoId FedAuthInfoId { get; }

        /// <summary>
        /// Initialization Constructor.
        /// </summary>
        public TDSFedAuthInfoOption()
        {
        }

        /// <summary>
        /// Inflate the token
        /// </summary>
        public abstract bool Inflate(Stream source);

        /// <summary>
        /// Deflate the token.
        /// </summary>
        public abstract void Deflate(Stream destination);
    }
}