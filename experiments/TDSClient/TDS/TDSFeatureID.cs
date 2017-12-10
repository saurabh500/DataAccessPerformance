namespace Microsoft.SqlServer.TDS
{
    /// <summary>
    /// TDS feature identifier
    /// </summary>
    public enum TDSFeatureID : byte
    {
        /// <summary>
        /// Session recovery (connection resiliency)
        /// </summary>
        SessionRecovery = 0x01,

        /// <summary>
        /// Federated authentication
        /// </summary>
        FederatedAuthentication = 0x02,

        /// <summary>
        /// End of the list
        /// </summary>
        Terminator = 0xFF
    }
}