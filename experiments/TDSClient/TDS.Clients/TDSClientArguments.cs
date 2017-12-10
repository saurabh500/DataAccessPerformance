using System;
using System.IO;

using Microsoft.SqlServer.TDS.Login7;
using Microsoft.SqlServer.TDS.PreLogin;

namespace Microsoft.SqlServer.TDS.Clients
{
    /// <summary>
    /// Common arguments for TDS client
    /// </summary>
    public class TDSClientArguments
    {
        /// <summary>
        /// Log to which send TDS conversation
        /// </summary>
        public TextWriter Log { get; set; }

        /// <summary>
        /// Host to connect to
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// Port to connect to
        /// </summary>
        public uint ServerPort { get; set; }

        /// <summary>
        /// Client version
        /// </summary>
        public Version ClientVersion { get; set; }

        /// <summary>
        /// Login to use for SQL Server
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Password for the login
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Database to log into
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Database path to attach to this session
        /// </summary>
        public string AttachDatabase { get; set; }

        /// <summary>
        /// Size of the TDS packet to use
        /// </summary>
        public uint PacketSize { get; set; }

        /// <summary>
        /// Application name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Application intent
        /// </summary>
        public TDSLogin7TypeFlagsReadOnlyIntent ApplicationIntent { get; set; }

        /// <summary>
        /// Server name client should deliver during login
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Instructs client to ignore routing instruction from SQL Server
        /// </summary>
        public bool IgnoreRouting { get; set; }

        /// <summary>
        /// Transport encryption
        /// </summary>
        public TDSPreLoginTokenEncryptionType Encryption { get; set; }

        /// <summary>
        /// Library name
        /// </summary>
        public string LibraryName { get; set; }

		/// <summary>
		/// Specifies whether complete login7 message will be sent or not
		/// </summary>
		public bool SendCompleteLogin { get; set; }			 

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public TDSClientArguments()
        {
            // Assign default client version
            ClientVersion = new Version(10, 0, 1083);

            // Default intent is read-write
            ApplicationIntent = TDSLogin7TypeFlagsReadOnlyIntent.ReadWrite;

            // By default we use 4K packets
            PacketSize = 4096;

            // By default we encrypt only login packet
            Encryption = TDSPreLoginTokenEncryptionType.Off;

            // Use default library name
            LibraryName = "GenericTDSClient";

			// By default non federated authentication will be used.
			//AuthenticationType = TDSPreLoginAuthenticationType.WindowsIntegrated;

			// By default complete login token will be sent
			SendCompleteLogin = true;

        }
    }
}
