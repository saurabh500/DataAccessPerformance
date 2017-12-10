using System;
using Microsoft.SqlServer.TDS.Login7;

namespace Microsoft.SqlServer.TDS.EndPoint.FederatedAuthentication
{
    /// <summary>
    /// Point of entry for the Federated Authentication Ticket Service
    /// </summary>
    public class FederatedAuthenticationTicketService
    {
        /// <summary>
        /// Given a Federated Authentication ticket, returns the Session Key from the authentication service
        /// </summary>
        public static IFederatedAuthenticationTicket DecryptTicket(TDSFedAuthLibraryType ticketType, byte[] encryptedTicket)
        {
            if (encryptedTicket == null)
            {
                throw new NullReferenceException("encryptedTicket is null. Unable to decrypt.");
            }

            switch (ticketType)
            {
                case TDSFedAuthLibraryType.IDCRL:
                    return RpsTicket.DecryptTicket(encryptedTicket);

                case TDSFedAuthLibraryType.SECURITY_TOKEN:
                    return JwtTicket.DecryptTicket(encryptedTicket);

                case TDSFedAuthLibraryType.ADAL:
                    // For now, fake fed auth tokens are sent for ADAL, so just return null
                    return null;

                default:
                    throw new ArgumentOutOfRangeException("Unexpected Federated Authentication ticket type.");
            }
        }
    }
}
