using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.SqlServer.TDS.Authentication
{
    /// <summary>
    /// TDS FedAuth Info Option for STS URL
    /// </summary>
    public class TDSFedAuthInfoOptionSTSURL : TDSFedAuthInfoOption
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
        private byte[] m_stsUrl;

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
        /// Return the STSURL as a unicode string.
        /// </summary>
        public string STSURL
        {
            get
            {
                if (m_stsUrl != null)
                {
                    return Encoding.Unicode.GetString(m_stsUrl);
                }

                return null;
            }
        }

        /// <summary>
        /// Default public contructor
        /// </summary>
        public TDSFedAuthInfoOptionSTSURL()
        {
            m_fedAuthInfoId = TDSFedAuthInfoId.STSURL;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="infoDataLength">Info Data Length</param>
        public TDSFedAuthInfoOptionSTSURL(uint infoDataLength) : this()
        {
            m_infoDataLength = infoDataLength;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stsurl">STSURL string</param>
        public TDSFedAuthInfoOptionSTSURL(string stsurl) : this()
        {
            m_stsUrl = Encoding.Unicode.GetBytes(stsurl);
            m_infoDataLength = (uint)m_stsUrl.Length;
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
                m_stsUrl = new byte[m_infoDataLength];
                source.Read(m_stsUrl, 0, m_stsUrl.Length);
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
                source.Write(m_stsUrl, 0, m_stsUrl.Length);
            }
        }
    }
}