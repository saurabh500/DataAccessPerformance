using System;
using System.Reflection;
using System.Linq;

namespace Microsoft.SqlServer.TDS.EndPoint.FederatedAuthentication
{
    /// <summary>
    /// Wrapper for dynamic loading of RPS dll
    /// </summary>
    class RPS
    {
        /// <summary>
        /// Instance of the dynamially loaded RPS assembly from Microsoft.Passport.RPS
        /// </summary>
        private Assembly rpsAssembly = null;

        /// <summary>
        /// Type of Microsoft.Passport.RPS.RPS
        /// </summary>
        private readonly Type rpsType = null;

        /// <summary>
        /// Type of Microsoft.Passport.RPS.RPSTicket
        /// </summary>
        private readonly Type rpsTicketType = null;

        /// <summary>
        /// Type of Microsoft.Passport.RPS.RPSPropBag
        /// </summary>
        private readonly Type rpsPropBagType = null;

        /// <summary>
        /// Type of Microsoft.Passport.RPS.RPSAuth
        /// </summary>
        private readonly Type rpsAuthType = null;

        /// <summary>
        /// Type of Microsoft.Passport.RPS.RPSTicket.RPSTicketProperty
        /// </summary>
        private readonly Type rpsTicketPropertyType = null;

        /// <summary>
        /// Instance of the Microsoft.Passport.RPS.RPS object
        /// </summary>
        private object rps = null;

        /// <summary>
        /// RPS Wrapper constructor
        /// </summary>
        public RPS()
        {
            // Load dynamically the assembly
            rpsAssembly = Assembly.Load("Microsoft.Passport.RPS, Version=6.1.6206.0, Culture=neutral, PublicKeyToken=283dd9fa4b2406c5, processorArchitecture=MSIL");

            // Extract the types that will be needed to perform authentication
            rpsType = rpsAssembly.GetTypes().ToList().Where(t => t.Name == "RPS").Single();
            rpsTicketType = rpsAssembly.GetTypes().ToList().Where(t => t.Name == "RPSTicket").Single();
            rpsPropBagType = rpsAssembly.GetTypes().ToList().Where(t => t.Name == "RPSPropBag").Single();
            rpsAuthType = rpsAssembly.GetTypes().ToList().Where(t => t.Name == "RPSAuth").Single();
            rpsTicketPropertyType = rpsTicketType.GetNestedTypes().ToList().Where(t => t.Name == "RPSTicketProperty").Single();

            // Create instance of the RPS object
            rps = Activator.CreateInstance(rpsType);
        }

        /// <summary>
        /// Calling Initialize in the RPS real object created from the dynamically loaded RPS assembly
        /// </summary>
        public void Initialize(string s)
        {
            // Call initialize in the previously created rps object
            rpsType.GetMethod("Initialize").Invoke(rps, new object[] { s });
        }

        /// <summary>
        /// Given an encrypted ticket, calls RPS Authenticate and returns the decrypted ticket 
        /// </summary>
        public object Authenticate(byte[] encryptedTicket, string siteName)
        {
            // Create instance of the rpsPropBag using the rps object
            object rpsPropBag = Activator.CreateInstance(rpsPropBagType, new object[] { rps });

            // Create instance of the rpsAuth Authenticator using the rps object
            object authenticator = Activator.CreateInstance(rpsAuthType, new object[] { rps });

            // Call Authenticate in the Authenticator object using the encrypted ticket and the site name provided
            return rpsAuthType.GetMethod("Authenticate").Invoke(authenticator, new object[] { siteName, System.Text.Encoding.Unicode.GetString(encryptedTicket), (UInt32)2 /*compact ticket*/, rpsPropBag });            
        }

        /// <summary>
        /// Given an rps decrypted ticket, get the session key
        /// </summary>
        public byte[] GetSessionKeyFromRpsDecryptedTicket(object rpsTicket)
        {
            // Take the Property field from the rpsTicket provided
            object rpsTicketProperty = rpsTicketType.GetFields().ToList().Where(p => p.Name == "Property").Single().GetValue(rpsTicket);

            // Get the Property["SessionKey"] value from the rpsTicketProperty
            object sessionkey = rpsTicketPropertyType.GetMethod("get_Item").Invoke(rpsTicketProperty, new object[] { "SessionKey" });
                
            // Return the Session Key as an array of bytes
            return GetStringAsBytes((string)sessionkey);            
        }

        /// <summary>
        /// Convert a "string" that is actually a byte array into an actual byte array - needed for interop
        /// with COM methods that are returning binary data as a BSTR.
        /// </summary>
        private static byte[] GetStringAsBytes(string toConvert)
        {
            byte[] convertedBytes = new byte[toConvert.Length * sizeof(char)];
            for (int i = 0; i < toConvert.Length; i++)
            {
                byte[] charAsBytes = BitConverter.GetBytes(toConvert[i]);
                convertedBytes[(2 * i)] = charAsBytes[0];
                convertedBytes[(2 * i) + 1] = charAsBytes[1];
            }

            return convertedBytes;
        }
    }
}
