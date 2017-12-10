using System;
using System.Collections.Generic;

using Microsoft.SqlServer.TDS.EndPoint;
using Microsoft.SqlServer.TDS.PreLogin;

namespace Microsoft.SqlServer.TDS.Clients
{
    /// <summary>
    /// Run time values of the TDS client
    /// </summary>
    public class GenericTDSClientContext : ITDSClientContext
    {
        /// <summary>
        /// Host or IP address on which SQL Server is running
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// Port number of the SQL Server
        /// </summary>
        public uint ServerPort { get; set; }

        /// <summary>
        /// Name of the server client connected to
        /// </summary>
        public string ServerName { get; internal set; }

        /// <summary>
        /// Server version
        /// </summary>
        public Version ServerVersion { get; internal set; }

        /// <summary>
        /// TDS version of the conversation
        /// </summary>
        public Version TDSVersion { get; internal set; }

        /// <summary>
        /// Current database
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Size of the TDS packet
        /// </summary>
        public uint PacketSize { get; set; }

        /// <summary>
        /// Failover partner for connected SQL Server
        /// </summary>
        public string FailoverPartner { get; set; }

        /// <summary>
        /// Encryption used on the session
        /// </summary>
        public TDSEncryptionType Encryption { get; set; }

        /// <summary>
        /// Query text to be sent to the TDS server
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Information sent by SQL Server
        /// </summary>
        public IList<string> Messages { get; internal set; }

        /// <summary>
        /// Contains the Client Nonce which has been sent in the prelogin packet
        /// </summary>
        public byte[] ClientNonce { get; internal set; }

        /// <summary>
        /// Contains the Nonce option sent by the server
        /// </summary>
        public byte[] ServerNonce { get; internal set; }

        /// <summary>
        /// Contains the federated authentication response from the server
        /// </summary>
        public string ServerPipe { get => ""; set => throw new NotImplementedException(); }

        public string ServerDescription => throw new NotImplementedException();

        public string Language => throw new NotImplementedException();

        public byte[] Collation => throw new NotImplementedException();

        public IList<object[]> QueryResponse { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid ConnectionID => throw new NotImplementedException();

        public object SessionState => throw new NotImplementedException();

        /// <summary>
        /// Default constructor
        /// </summary>
        public GenericTDSClientContext()
        {
            // By default we turn off encryption
            Encryption = TDSEncryptionType.Off;

            // Establish default packet size
            PacketSize = 4096;
        }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public GenericTDSClientContext(TDSClientArguments arguments)
        {
            // By default we turn off encryption
            Encryption = TDSEncryptionType.Off;

            // Initialize context
            Initialize(arguments);
        }

        /// <summary>
        /// Initialize context with values from the arguments
        /// </summary>
        /// <param name="arguments">Arguments to seed the context</param>
        public void Initialize(TDSClientArguments arguments)
        {
            // Save packet size
            PacketSize = arguments.PacketSize;

            // Save server information
            ServerHost = arguments.ServerHost;
            ServerPort = arguments.ServerPort;
            ServerName = arguments.ServerName;
        }
    }
}