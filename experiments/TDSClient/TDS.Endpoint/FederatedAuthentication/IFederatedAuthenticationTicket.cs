using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TDS.EndPoint.FederatedAuthentication
{
    /// <summary>
    /// Interface for federated authentication ticket
    /// </summary>
    public interface IFederatedAuthenticationTicket
    {
        /// <summary>
        /// Computes and returns the signature of the provided buffer using the internal session Key associated with the auth
        /// ticket.
        /// </summary>
        byte[] GetSignature(byte[] bufferToSign);
    }
}
