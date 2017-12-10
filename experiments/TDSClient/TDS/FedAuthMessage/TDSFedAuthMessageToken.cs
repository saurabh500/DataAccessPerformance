using System.IO;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TDS.Authentication
{
    /// <summary>
    /// FedAuthToken Message definition.
    /// </summary>
    public class TDSFedAuthToken : TDSPacketToken
    {
        /// <summary>
        /// Federated Authentication Token
        /// </summary>
        private byte[] m_token;

        /// <summary>
        /// Nonce
        /// </summary>
        private byte[] m_nonce;

        public byte[] Token { get { return m_token; } }
        public byte[] Nonce { get { return m_nonce; } }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public TDSFedAuthToken()
        {
        }

        /// <summary>
        /// Initialization constructor.
        /// </summary>
        /// <param name="token">Token</param>
        public TDSFedAuthToken(byte[] token, byte[] nonce) :
            this()
        {

            m_token = new byte[token.Length];
            token.CopyTo(m_token, 0);

            if (nonce != null)
            {
                m_nonce = new byte[nonce.Length];
                nonce.CopyTo(m_nonce, 0);
            }
        }

        /// <summary>
        /// Inflating constructor.
        /// </summary>
        /// <param name="source"></param>
        public TDSFedAuthToken(Stream source) :
            this()
        {
            Inflate(source);
        }

        /// <summary>
        /// Inflate the token
        /// NOTE: This operation is not continuable and assumes that the entire token is available in the stream
        /// </summary>
        /// <param name="source">Stream to inflate the token from.</param>
        /// <returns>True in case of success, false otherwise.</returns>
        public override bool Inflate(Stream source)
        {
            // Read length of entire message
            uint totalLengthOfData = TDSUtilities.ReadUInt(source);

            // Read length of the fedauth token
            uint tokenLength = TDSUtilities.ReadUInt(source);

            // Read the fedauth token
            m_token = new byte[tokenLength];
            source.Read(m_token, 0, (int)tokenLength);

            // Read nonce if it exists
            if (totalLengthOfData > tokenLength)
            {
                m_nonce = new byte[totalLengthOfData - tokenLength];
                source.Read(m_nonce, 0, (int)(totalLengthOfData - tokenLength));
            }
            else if (tokenLength > totalLengthOfData)
            {
                // token length cannot be greater than the total length of the message
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deflate the token.
        /// </summary>
        /// <param name="destination">Stream the token to deflate to.</param>
        public override void Deflate(Stream destination)
        {
            // Write the total Length
            uint totalLengthOfData = (uint)(sizeof(uint) /*bytes to carry the token length itself*/ + m_token.Length + ((m_nonce != null) ? m_nonce.Length : 0));
            TDSUtilities.WriteUInt(destination, totalLengthOfData);

            // Write the Length of FedAuthToken
            TDSUtilities.WriteUInt(destination, (uint)m_token.Length);

            // Write Fake Token
            destination.Write(m_token, 0, m_token.Length);

            // Write Nonce
            if (m_nonce != null)
            {
                destination.Write(m_nonce, 0, m_nonce.Length);
            }
        }
    }
}