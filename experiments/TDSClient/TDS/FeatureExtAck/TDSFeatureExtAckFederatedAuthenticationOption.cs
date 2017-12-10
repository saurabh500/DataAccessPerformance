using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.SqlServer.TDS.SessionState;

namespace Microsoft.SqlServer.TDS.FeatureExtAck
{
    /// <summary>
    /// Acknowledgement for federated authentication
    /// </summary>
    public class TDSFeatureExtAckFederatedAuthenticationOption : TDSFeatureExtAckOption
    {
        /// <summary>
        /// Fixed Length of Nonce
        /// </summary>
        private static readonly uint NonceDataLength = 32;

        /// <summary>
        /// Fixed Length of Signature
        /// </summary>
        private static readonly uint SignatureDataLength = 32;

        /// <summary>
        /// Signed nonce
        /// </summary>
        public byte[] ClientNonce { get; set; }

        /// <summary>
        /// The HMAC-SHA-256 [RFC6234] of the server-specified nonce
        /// </summary>
        public byte[] Signature { get; set; }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public TDSFeatureExtAckFederatedAuthenticationOption(byte[] clientNonce, byte[] signature) :
            this()
        {
            // Nonce and/or Signature can be null depending on the FedAuthLibrary used
            if (clientNonce == null && signature != null)
            {
                throw new ArgumentNullException("signature");
            }
            else if (clientNonce != null && clientNonce.Length != NonceDataLength)
            {
                throw new ArgumentOutOfRangeException("nonce");
            }
            else if (signature != null && signature.Length != SignatureDataLength)
            {
                throw new ArgumentOutOfRangeException("signature");
            }

            // Save nonce
            ClientNonce = clientNonce;

            // Save signature
            Signature = signature;
        }

        /// <summary>
        /// Inflation constructor
        /// </summary>
        public TDSFeatureExtAckFederatedAuthenticationOption(Stream source) :
            this()
        {
            // Inflate
            Inflate(source);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        private TDSFeatureExtAckFederatedAuthenticationOption()
        {
            // Set feature identifier
            FeatureID = TDSFeatureID.FederatedAuthentication;
        }

        /// <summary>
        /// Deflate state into the stream
        /// </summary>
        public override void Deflate(Stream destination)
        {
            // Write feature extension acknowledgement
            destination.WriteByte((byte)TDSFeatureID.FederatedAuthentication);

            // Write the data length
            TDSUtilities.WriteUInt(destination, ((ClientNonce != null) ? NonceDataLength : 0) + ((Signature != null) ? SignatureDataLength : 0));

            if (ClientNonce != null)
            {
                // Write the Nonce            
                destination.Write(ClientNonce, 0, (int)NonceDataLength);
            }

            if (Signature != null)
            {
                // Write the signature
                destination.Write(Signature, 0, (int)SignatureDataLength);
            }
        }

        /// <summary>
        /// Inflate from stream
        /// </summary>
        public override bool Inflate(Stream source)
        {
            // We skip feature ID because it was read by construction factory

            // Read the data length.
            uint dataLength = TDSUtilities.ReadUInt(source);

            if (dataLength > 0)
            {
                // Allocate a container
                ClientNonce = new byte[NonceDataLength];

                // Read the data
                source.Read(ClientNonce, 0, (int)NonceDataLength);
            }

            if (dataLength > NonceDataLength)
            {
                // Allocate Signature
                Signature = new byte[SignatureDataLength];

                // Read the data
                source.Read(Signature, 0, (int)SignatureDataLength);
            }

            // Inflation is complete
            return true;
        }
    }
}