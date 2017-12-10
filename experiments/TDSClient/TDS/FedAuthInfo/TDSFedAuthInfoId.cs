using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TDS.Authentication
{
    public enum TDSFedAuthInfoId
    {
        /// <summary>
        /// STS URL as Token Endpoint
        /// </summary>
        STSURL = 0x01,

        /// <summary>
        /// Service Principal Name
        /// </summary>
        SPN = 0x02,

        /// <summary>
        /// Invalid InfoId
        /// </summary>
        Invalid = 0xEE
    }
}
