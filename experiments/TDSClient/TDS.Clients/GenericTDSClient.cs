using System;
using System.Collections.Generic;
using System.Security.Cryptography;

using Microsoft.SqlServer.TDS.AllHeaders;
using Microsoft.SqlServer.TDS.EndPoint;
using Microsoft.SqlServer.TDS.EnvChange;
using Microsoft.SqlServer.TDS.FeatureExtAck;
using Microsoft.SqlServer.TDS.Login7;
using Microsoft.SqlServer.TDS.LoginAck;
using Microsoft.SqlServer.TDS.PreLogin;
using Microsoft.SqlServer.TDS.SQLBatch;

namespace Microsoft.SqlServer.TDS.Clients
{
    /// <summary>
    /// Highly customizable generic TDS client
    /// </summary>
    public class GenericTDSClient : ITDSClient
    {
        /// <summary>
        /// Current client state (required for TDS parser)
        /// </summary>
        public TDSClientState State { get; private set; }

        /// <summary>
        /// Configuration of the client
        /// </summary>
        public GenericTDSClientArguments Arguments { get; set; }

        /// <summary>
        /// Run time context of the client
        /// </summary>
        public ITDSClientContext Context { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GenericTDSClient() :
            this(new GenericTDSClientArguments())
        {
        }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public GenericTDSClient(GenericTDSClientArguments arguments)
        {
            // Indicate that transport should be established
            State = TDSClientState.Final;

            // Instantiate context
            Context = new GenericTDSClientContext(arguments);

            // Save arguments
            Arguments = arguments;
        }

        /// <summary>
        /// Initial request
        /// </summary>
        public virtual TDSMessage OnPreLogin()
        {
            // Reset client context
            (Context as GenericTDSClientContext).Initialize(Arguments);

            // Create a new request message
            TDSMessage requestMessage = new TDSMessage(TDSMessageType.PreLogin);

            // Create TDS prelogin packet
            TDSPreLoginToken preLoginToken = new TDSPreLoginToken(Arguments.ClientVersion, Arguments.Encryption, false, (uint)System.Threading.Thread.CurrentThread.ManagedThreadId);

            // Generate cryptographically strong byte arrays.

            // Set prelogin packet datas.
            //preLoginToken.Nonce = _GenerateRandomBytes(32);

            // Keep the nonce option sent by client
            (Context as GenericTDSClientContext).ClientNonce = preLoginToken.Nonce;            

            // Generate Activity ID, first 16 bits are Guid followed by 4 four 0s
            preLoginToken.ActivityID = new byte[20];
            Array.Copy(Guid.NewGuid().ToByteArray(), preLoginToken.ActivityID, 16);            

            // Generate Trace ID.
            preLoginToken.ClientTraceID = Guid.NewGuid().ToByteArray();

            // Log request
            TDSUtilities.Log(Arguments.Log, "Request", preLoginToken);

            // Serialize the prelogin token into the request packet
            requestMessage.Add(preLoginToken);

            // Transition state machine into pre-login sent state
            State = TDSClientState.PreLoginSent;

            return requestMessage;
        }

        /// <summary>
        /// Pre-login response received
        /// </summary>
        public virtual TDSMessage OnPreLoginResponse(TDSMessage response)
        {
            // Create a new request message
            TDSMessage requestMessage = new TDSMessage(TDSMessageType.TDS7Login);

            // Inflate pre-login request from the message
            TDSPreLoginToken preLoginResponse = response[0] as TDSPreLoginToken;

            // Log response
            TDSUtilities.Log(Arguments.Log, "Response", preLoginResponse);

            // Update client state with encryption resolution
            Context.Encryption = TDSUtilities.ResolveEncryption(Arguments.Encryption, preLoginResponse.Encryption);

            // Keep the Nonce sent by server
            (Context as GenericTDSClientContext).ServerNonce = preLoginResponse.Nonce;            

            // Based on prelogin response do either federated or sql authentication.
            // Create TDS login packet
            TDSLogin7Token loginToken = null;

            // Check the authentication type.
            if (Arguments.SendCompleteLogin)
            {
                // Doing complete federated authentication.				
                loginToken = CompleteFedAuthLoginRequest(preLoginResponse);
            }
           
            // Log request
            TDSUtilities.Log(Arguments.Log, "Request", loginToken);

            // Serialize login token into the request packet
            requestMessage.Add(loginToken);

            return requestMessage;
        }


        /// <summary>
        /// Received login acknowledgement
        /// </summary>
        public virtual void OnCompleteLoginResponse(TDSMessage response)
        {
            // Prepare exception
            Exception exception = null;

            // Reset all messages
            (Context as GenericTDSClientContext).Messages = new List<string>();

            // Print all the tokens
            foreach (TDSPacketToken token in response)
            {
                // Check if log is available
                TDSUtilities.Log(Arguments.Log, "Response", token);

                // Check current token type
                if (token is TDSLoginAckToken)
                {
                    // Cast to login acknowledgement
                    TDSLoginAckToken loginAck = token as TDSLoginAckToken;

                    // Populate run time context
                    (Context as GenericTDSClientContext).ServerName = loginAck.ServerName;
                    (Context as GenericTDSClientContext).ServerVersion = loginAck.ServerVersion;
                    (Context as GenericTDSClientContext).TDSVersion = loginAck.TDSVersion;

                    // Check if version is supported by client
                    if (!TDSVersion.IsSupported(Context.TDSVersion))
                    {
                        // We can't work with the server that we don't support
                        exception = new Exception("Negotiated TDS version is not supported by client", exception);
                    }
                    else
                    {
                        // Transition state machine into logged-in
                        State = TDSClientState.LoggedIn;

                        // Check if log is available
                        if (Arguments.Log != null)
                        {
                            Arguments.Log.WriteLine("Logged In");
                        }
                    }
                }
                else if (token is TDSEnvChangeToken)
                {
                    // Cast
                    TDSEnvChangeToken envChange = token as TDSEnvChangeToken;

                    // Act based on type
                    switch (envChange.Type)
                    {
                        case TDSEnvChangeTokenType.PacketSize:
                            {
                                // Set packet size
                                (Context as GenericTDSClientContext).PacketSize = uint.Parse((string)envChange.NewValue);

                                // Check if log is available
                                if (Arguments.Log != null)
                                {
                                    Arguments.Log.WriteLine("Changed packet size");
                                }

                                break;
                            }
                        case TDSEnvChangeTokenType.Database:
                            {
                                // Save database we connected to
                                (Context as GenericTDSClientContext).Database = (string)envChange.NewValue;

                                // Check if log is available
                                if (Arguments.Log != null)
                                {
                                    Arguments.Log.WriteLine("Changed database");
                                }

                                break;
                            }
                        case TDSEnvChangeTokenType.RealTimeLogShipping:
                            {
                                // Save failover partner
                                (Context as GenericTDSClientContext).FailoverPartner = (string)envChange.NewValue;

                                // Check if log is available
                                if (Arguments.Log != null)
                                {
                                    Arguments.Log.WriteLine("Changed failover partner");
                                }

                                break;
                            }
                        case TDSEnvChangeTokenType.Routing:
                            {
                                // Cast new data
                                TDSRoutingEnvChangeTokenValue routingValue = envChange.NewValue as TDSRoutingEnvChangeTokenValue;

                                // Check protocol
                                if (routingValue.Protocol != TDSRoutingEnvChangeTokenValueType.TCP)
                                {
                                    // Create exception to represent this error
                                    exception = new Exception("Unsupported routing protocol", exception);
                                }

                                // Save new end-point in the context
                                (Context as GenericTDSClientContext).ServerHost = routingValue.AlternateServer;
                                (Context as GenericTDSClientContext).ServerPort = (ushort)routingValue.ProtocolProperty;

                                // Check if this instruction should be ignored
                                if (!Arguments.IgnoreRouting)
                                {
                                    // Change client state to carry out routing
                                    State = TDSClientState.ReConnect;

                                    // Check if log is available
                                    if (Arguments.Log != null)
                                    {
                                        Arguments.Log.WriteLine("Client is being routed");
                                    }
                                }
                                else
                                {
                                    // Check if log is available
                                    if (Arguments.Log != null)
                                    {
                                        Arguments.Log.WriteLine("Ignored routing instruction");
                                    }
                                }

                                break;
                            }
                        default:
                            {
                                // Do nothing
                                break;
                            }
                    }
                }
                else if (token is TDSFeatureExtAckToken)
                {
                    // Cast to the ExtensionFlag Ack token.
                    TDSFeatureExtAckToken featureExtAckToken = token as TDSFeatureExtAckToken;

                    // Additional logic will be added here
                }
                else if (token is Microsoft.SqlServer.TDS.Info.TDSInfoToken)
                {
                    // Register message
                    (Context as GenericTDSClientContext).Messages.Add((token as Microsoft.SqlServer.TDS.Info.TDSInfoToken).Message);

                    // Check if log is available
                    if (Arguments.Log != null)
                    {
                        Arguments.Log.WriteLine("Message: {0}", (token as Microsoft.SqlServer.TDS.Info.TDSInfoToken).Message);
                    }
                }
                else if (token is Microsoft.SqlServer.TDS.Error.TDSErrorToken)
                {
                    // Create exception to represent this error
                    exception = new Exception((token as Microsoft.SqlServer.TDS.Error.TDSErrorToken).Message, exception);

                    // Check if log is available
                    if (Arguments.Log != null)
                    {
                        Arguments.Log.WriteLine("Error: {0}", (token as Microsoft.SqlServer.TDS.Error.TDSErrorToken).Message);
                    }
                }
            }

            // Check if exception is available
            if (exception != null)
            {
                // Transition state machine into final state
                State = TDSClientState.Final;

                // Throw it
                throw exception;
            }
        }

        /// <summary>
        /// Create a request to SQL Server after authentication
        /// </summary>
        public virtual TDSMessage OnRequest()
        {
            // Build a new SQL Batch
            TDSSQLBatchToken batchRequest = new TDSSQLBatchToken
            {
                // Initialize headers
                AllHeaders = new TDSAllHeadersToken()
            };
            batchRequest.AllHeaders.Headers.Add(new TDSTransactionDescriptorHeader(0, 1));

            // Set batch text
            batchRequest.Text = string.IsNullOrEmpty(Context.Query) ? string.Empty : Context.Query;

            // Transition state machine into request sent state
            State = TDSClientState.RequestSent;

            // Prepare request message
            return new TDSMessage(TDSMessageType.SQLBatch, batchRequest);
        }

        /// <summary>
        /// Process response to the request
        /// </summary>
        public virtual void OnResponse(TDSMessage response)
        {
            // Dump the response
            foreach (TDSPacketToken token in response)
            {
                // Log the token.
                TDSUtilities.Log(Arguments.Log, "Response", token);
            }

            // Transition state machine into request received
            State = TDSClientState.LoggedIn;
        }

        /// <summary>
        /// Create a request to SQL Server to logout
        /// </summary>
        public virtual TDSMessage OnLogout()
        {
            // Transition state machine into final state
            State = TDSClientState.Final;

            // There's nothing we'd like to tell TDS Server at this point
            return null;
        }
        
        /// <summary>
        /// Process response to the logout request
        /// </summary>
        public virtual void OnLogoutResponse(TDSMessage message)
        {
            // We shouldn't be here
            throw new NotImplementedException("OnLogoutResponse is not implemented");
        }

        /// <summary>
        /// Fills the loginToken common part for the SQL and Fed Auths
        /// </summary>		
        protected virtual void FillLoginCommonParts(TDSLogin7Token loginToken)
        {
            loginToken.ApplicationName = Arguments.ApplicationName;
            loginToken.ClientID = new byte[6] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6 };
            loginToken.ClientLCID = 1033;
            loginToken.ClientPID = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
            loginToken.ClientProgramVersion = 0x7000000;
            loginToken.ClientTimeZone = 480;
            loginToken.LibraryName = Arguments.LibraryName;
            loginToken.HostName = Environment.MachineName;
            loginToken.PacketSize = Arguments.PacketSize;
            loginToken.UserID = Arguments.Login;
            loginToken.Password = Arguments.Password;
            loginToken.TDSVersion = TDSVersion.GetTDSVersion(Arguments.ClientVersion);
            loginToken.Database = Arguments.Database;
            loginToken.AttachDatabaseFile = Arguments.AttachDatabase;
            loginToken.ServerName = Arguments.ServerName;

            // Configure first optional flags
            loginToken.OptionalFlags1.CharacterSet = TDSLogin7OptionalFlags1Char.Ascii;
            loginToken.OptionalFlags1.Database = TDSLogin7OptionalFlags1Database.Fatal;
            loginToken.OptionalFlags1.DumpLoad = TDSLogin7OptionalFlags1DumpLoad.On;
            loginToken.OptionalFlags1.FloatingPoint = TDSLogin7OptionalFlags1Float.IEEE754;
            loginToken.OptionalFlags1.Language = TDSLogin7OptionalFlags1Language.On;
            loginToken.OptionalFlags1.Order = TDSLogin7OptionalFlags1Order.OrderX86;
            loginToken.OptionalFlags1.UseDB = TDSLogin7OptionalFlags1UseDB.Off;

            // Configure second optional flags
            loginToken.OptionalFlags2.IntegratedSecurity = TDSLogin7OptionalFlags2IntSecurity.Off;
            loginToken.OptionalFlags2.Language = TDSLogin7OptionalFlags2Language.Fatal;
            loginToken.OptionalFlags2.Odbc = TDSLogin7OptionalFlags2Odbc.On;
            loginToken.OptionalFlags2.UserType = TDSLogin7OptionalFlags2UserType.Server;

            // Configure things optional flags
            loginToken.OptionalFlags3.ChangePassword = TDSLogin7OptionalFlags3ChangePassword.No;
            loginToken.OptionalFlags3.IsUserInstance = false;
            loginToken.OptionalFlags3.UnknownCollation = TDSLogin7OptionalFlags3Collation.Must;            

            // Configure type flags
            loginToken.TypeFlags.OleDb = TDSLogin7TypeFlagsOleDb.On;
            loginToken.TypeFlags.SQL = TDSLogin7TypeFlagsSQL.Default;
            loginToken.TypeFlags.ReadOnlyIntent = Arguments.ApplicationIntent;
        }

        /// <summary>
        /// Completes the Login7 token creation for the SQL authentication 
        /// </summary>		
        protected virtual TDSLogin7Token CompleteSqlAuthRequest(TDSPreLoginToken preLoginToken)
        {
            // Create TDSLogin7Token
            TDSLogin7Token loginToken = new TDSLogin7Token();

            // Filling login7 common parts
            FillLoginCommonParts(loginToken);

            // Change the state of the client.
            State = TDSClientState.CompleteLogin7Sent;

            return loginToken;
        }

        

        /// <summary>
        /// Completes Login7 token creation for the Federated authentication
        /// </summary>
        protected virtual TDSLogin7Token CompleteFedAuthLoginRequest(TDSPreLoginToken preLoginToken)
        {
            // Create TDSLogin7 token.
            TDSLogin7Token loginToken = new TDSLogin7Token();

            // Filling login7 common parts
            FillLoginCommonParts(loginToken);

            // Set the featureExt option
            loginToken.OptionalFlags3.ExtensionFlag = true;

            // Change the state of the client.
            State = TDSClientState.CompleteLogin7Sent;

            return loginToken;
        }

        /// <summary>
        /// Generates random bytes
        /// </summary>
        /// <param name="count">The number of bytes to be generated.</param>
        /// <returns>Generated random bytes.</returns>
        private byte[] _GenerateRandomBytes(int count)
        {
            byte[] randomBytes = new byte[count];

            RNGCryptoServiceProvider gen = new RNGCryptoServiceProvider();
            // Generate bytes
            gen.GetBytes(randomBytes);

            return randomBytes;
        }

        public void OnPreConnect()
        {
            return;
        }

        public TDSMessage OnSSPIResponse(TDSMessage message)
        {
            throw new NotImplementedException();
        }

        public TDSMessage OnFedAuthInfoTokenResponse(TDSMessage message)
        {
            throw new NotImplementedException();
        }

        public void OnLoginResponse(TDSMessage message)
        {
            State = TDSClientState.LoggedIn;
            return;
        }
    }
}
