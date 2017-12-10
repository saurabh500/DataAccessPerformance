using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.SqlServer.TDS.Authentication
{
    /// <summary>
    /// TDS FedAuth Info Option for SPN
    /// </summary>
    public class TDSFedAuthInfoOptionSPN : TDSFedAuthInfoOption
    {
        /// <summary>
        /// FedAuth Information ID
        /// </summary>
        private TDSFedAuthInfoId m_fedAuthInfoId;

        /// <summary>
        /// Information Data Length
        /// </summary>
        private uint m_infoDataLength;

        /// <summary>
        /// STS URL
        /// </summary>
        private byte[] m_spn;

        /// <summary>
        /// Return the SPN as a unicode string.
        /// </summary>
        public string SPN
        {
            get
            {
                if (m_spn != null)
                {
                    return Encoding.Unicode.GetString(m_spn);
                }

                return null;
            }
        }

        /// <summary>
        /// Return the FedAuthInfo Id.
        /// </summary>
        public override TDSFedAuthInfoId FedAuthInfoId
        {
            get
            {
                return m_fedAuthInfoId;
            }
        }

        /// <summary>
        /// Default public contructor
        /// </summary>
        public TDSFedAuthInfoOptionSPN()
        {
            m_fedAuthInfoId = TDSFedAuthInfoId.SPN;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="infoDataLength">Info Data Length</param>
        public TDSFedAuthInfoOptionSPN(uint infoDataLength)
            : this()
        {
            m_infoDataLength = infoDataLength;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="spn">SPN string</param>
        public TDSFedAuthInfoOptionSPN(string spn)
            : this()
        {
            m_spn = Encoding.Unicode.GetBytes(spn);
            m_infoDataLength = (uint)m_spn.Length;
        }

        /// <summary>
        /// Inflate the data from the stream, when receiving this token.
        /// </summary>
        public override bool Inflate(Stream source)
        {
            // Read the information data
            // 
            if (m_infoDataLength > 0)
            {
                m_spn = new byte[m_infoDataLength];
                source.Read(m_spn, 0, m_spn.Length);
            }

            return true;
        }

        /// <summary>
        /// Deflate the data to the stream, when writing this token.
        /// </summary>
        /// <param name="source"></param>
        public override void Deflate(Stream source)
        {
            if (m_infoDataLength > 0)
            {
                source.Write(m_spn, 0, m_spn.Length);
            }
        }
    }
}