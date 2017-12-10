﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;

namespace Microsoft.SqlServer.TDS.EndPoint.FederatedAuthentication
{
    /// <summary>
    /// RPS implementation of federated authentication ticket
    /// </summary>
    public class RpsTicket : IFederatedAuthenticationTicket
    {
        /// <summary>
        /// Singleton instance of an RPS class, which drives authentication at the RPS level.
        /// </summary>
        private const string siteNameInt = "dev.mscds.com";

        /// <summary>
        /// Singleton instance of an RPS class, which drives authentication at the RPS level.
        /// </summary>        
        private static RPS rps = null;           

        /// <summary>
        /// The short-lived session key associated with this authentication ticket
        /// </summary>
        public readonly byte[] sessionKey;

        /// <summary>
        /// The RPS representation of the ticket
        /// </summary>                
        private object rpsTicket = null;

        /// <summary>
        /// Constructor that takes the RPS representation of the ticket as an argument
        /// </summary>                
        private RpsTicket(object _ticket, byte[] _sessionKey)
        {
            rpsTicket = _ticket;
            sessionKey = _sessionKey;
        }        

        /// <summary>
        /// Static constructor for the RpsTicket class.
        /// </summary>
        static RpsTicket()
        {         
            // Initialize the rps object
            rps = new RPS();
            rps.Initialize(null);                        
        }
        
        /// <summary>
        /// Computes and returns the HMACSHA256 of the provided buffer using the Session Key associated with the auth
        /// ticket.
        /// </summary>
        public byte[] GetSignature(byte[] bufferToSign)
        {
            // Argument check to avoid throwing exceptions in this part
            if (bufferToSign == null)
            {
                return null;
            }

            using (HMACSHA256 shaHash = new HMACSHA256(sessionKey))
            {                
                return shaHash.ComputeHash(bufferToSign);
            }
        }

        /// <summary>
        /// Takes the encrypted wire format of the ticket and, on successful authentication, returns
        /// the resulting ticket.
        /// </summary>
        public static IFederatedAuthenticationTicket DecryptTicket(byte[] encryptedTicket)
        {                        
            // Get the decrypted RPS ticket by calling Authenticate with the encrypted ticket
            object tempTicket = rps.Authenticate(encryptedTicket, siteNameInt);
            
            // Instantiate the RpsTicket using the decrypted ticket and its session key
            return new RpsTicket(tempTicket, rps.GetSessionKeyFromRpsDecryptedTicket(tempTicket));
        }
    }

    /// <summary>
    /// JWT implementation of federated authentication ticket
    /// Move this class to a different file once msbuild conversion is completed.
    /// </summary>
    public class JwtTicket : IFederatedAuthenticationTicket
    {
        /// <summary>
        /// the ticket
        /// </summary>
        private readonly byte[] m_ticket;

        /// <summary>
        /// Computes and returns the HMACSHA256 of the provided buffer using the Session Key associated with the auth
        /// ticket.
        /// </summary>
        public byte[] GetSignature(byte[] bufferToSign)
        {
           return null;
        }

        /// <summary>
        /// Constructor that takes the RPS representation of the ticket as an argument
        /// </summary>                
        private JwtTicket(byte[] ticket)
        {
            m_ticket = ticket;
        }  

        /// <summary>
        /// Takes the encrypted wire format of the ticket and, on successful authentication, returns
        /// the resulting ticket.
        /// </summary>
        public static IFederatedAuthenticationTicket DecryptTicket(byte[] encryptedTicket)
        {
            // Instantiate the RpsTicket using the decrypted ticket and its session key
            return new JwtTicket(encryptedTicket);
        }
    }
}