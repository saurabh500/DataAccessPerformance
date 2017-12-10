using System;

namespace Microsoft.SqlServer.TDS.Clients
{
    /// <summary>
    /// TDS version routines
    /// </summary>
    internal static class TDSVersion
    {
        /// <summary>
        /// Denali TDS version
        /// </summary>
        internal static Version Denali = new Version(7, 4, 0, 4);

        /// <summary>
        /// Katmai TDS version
        /// </summary>
        internal static Version Katmai = new Version(7, 3, 11, 3);

        /// <summary>
        /// Yukon TDS version
        /// </summary>
        internal static Version Yukon = new Version(7, 2, 9, 2);

        /// <summary>
        /// Map client build version to TDS version
        /// </summary>
        /// <param name="buildVersion">Build version to analyze</param>
        /// <returns>TDS version that corresponding build version supports</returns>
        internal static Version GetTDSVersion(Version buildVersion)
        {
            // Check build version Major part
            if (buildVersion.Major == 11)
            {
                // Denali
                return Denali;
            }
            else if (buildVersion.Major == 10)
            {
                // Katmai
                return Katmai;
            }
            else if (buildVersion.Major == 9)
            {
                // Yukon
                return Yukon;
            }
            else
            {
                // Not supported TDS version
                throw new NotSupportedException("Specified build version is not supported");
            }
        }

        /// <summary>
        /// Check whether TDS version is supported by server
        /// </summary>
        internal static bool IsSupported(Version tdsVersion)
        {
            return tdsVersion >= Yukon && tdsVersion <= Denali;
        }
    }
}
