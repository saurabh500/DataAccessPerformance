using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.TDS.EndPoint.Cryptography
{
    /// <summary>
    /// Certificate toolkit
    /// </summary>
    public class Certificate
    {
        /// <summary>
        /// Create self-signed certificate
        /// </summary>
        public static byte[] CreateSelfSignCertificatePfx(string x500, DateTime startTime, DateTime endTime)
        {
            return CreateSelfSignCertificatePfx(x500, startTime, endTime, null);
        }

        /// <summary>
        /// Create self-signed certificate
        /// </summary>
        public static byte[] CreateSelfSignCertificatePfx(string x500, DateTime startTime, DateTime endTime, string password)
        {
            // Final Pfx certificate
            byte[] pfxData = null;

            // Check if x500 is specified
            if (string.IsNullOrEmpty(x500))
            {
                // Replace with empty string
                x500 = string.Empty;
            }

            // Convert time into file system time stamps
            SystemTime startSystemTime = _ToSystemTime(startTime);
            SystemTime endSystemTime = _ToSystemTime(endTime);

            string containerName = Guid.NewGuid().ToString();

            GCHandle dataHandle = new GCHandle();
            IntPtr providerContext = IntPtr.Zero;
            IntPtr cryptKey = IntPtr.Zero;
            IntPtr certContext = IntPtr.Zero;
            IntPtr certStore = IntPtr.Zero;
            IntPtr storeCertContext = IntPtr.Zero;
            
            IntPtr passwordPtr = IntPtr.Zero;

            // Tell compiler that the following section should not be interrupted
            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                // Acquire security context
                _Check(AdvancedAPIMethods.CryptAcquireContextW(out providerContext, containerName, null, 1 /* PROV_RSA_FULL */, 8 /* CRYPT_NEWKEYSET */));

                // Generate key
                _Check(AdvancedAPIMethods.CryptGenKey(providerContext, 1 /* AT_KEYEXCHANGE */, 1 /* CRYPT_EXPORTABLE */, out cryptKey));

                IntPtr errorStringPtr;
                int nameDataLength = 0;
                byte[] nameData;

                // errorStringPtr gets a pointer into the middle of the x500 string,
                // so x500 needs to be pinned until after we've copied the value
                // of errorStringPtr.
                dataHandle = GCHandle.Alloc(x500, GCHandleType.Pinned);

                // Estimate the length of the value
                if (!CryptMethods.CertStrToNameW(0x00010001 /* X509_ASN_ENCODING | PKCS_7_ASN_ENCODING */, dataHandle.AddrOfPinnedObject(), 3 /* CERT_X500_NAME_STR = 3*/, IntPtr.Zero, null, ref nameDataLength, out errorStringPtr))
                {
                    // Throw an error
                    throw new ArgumentException(Marshal.PtrToStringUni(errorStringPtr));
                }

                // Allocate name data
                nameData = new byte[nameDataLength];

                // Convert value to certificate name
                if (!CryptMethods.CertStrToNameW(0x00010001 /* X509_ASN_ENCODING | PKCS_7_ASN_ENCODING */, dataHandle.AddrOfPinnedObject(), 3 /* CERT_X500_NAME_STR = 3 */, IntPtr.Zero, nameData, ref nameDataLength, out errorStringPtr))
                {
                    // Throw an error
                    throw new ArgumentException(Marshal.PtrToStringUni(errorStringPtr));
                }

                // Release handle
                dataHandle.Free();

                dataHandle = GCHandle.Alloc(nameData, GCHandleType.Pinned);

                // Allocate cryptography blob
                CryptoAPIBlob nameBlob = new CryptoAPIBlob(nameData.Length, dataHandle.AddrOfPinnedObject());

                CryptKeyProviderInfo kpi = new CryptKeyProviderInfo();
                kpi.ContainerName = containerName;
                kpi.ProviderType = 1; // PROV_RSA_FULL
                kpi.KeySpec = 1; // AT_KEYEXCHANGE

                // Generate certificate
                certContext = CryptMethods.CertCreateSelfSignCertificate(providerContext, ref nameBlob, 0, ref kpi, IntPtr.Zero /* default = SHA1RSA */, ref startSystemTime, ref endSystemTime, IntPtr.Zero);

                // Check error
                _Check(certContext != IntPtr.Zero);

                // Release data 
                dataHandle.Free();

                // Open certificate store in memory
                certStore = CryptMethods.CertOpenStore("Memory" /* sz_CERT_STORE_PROV_MEMORY */, 0, IntPtr.Zero, 0x2000 /* CERT_STORE_CREATE_NEW_FLAG */, IntPtr.Zero);

                // Check if certificate store was opened
                _Check(certStore != IntPtr.Zero);

                // Add generated certificate to store
                _Check(CryptMethods.CertAddCertificateContextToStore(certStore, certContext, 1 /* CERT_STORE_ADD_NEW */, out storeCertContext));

                // Set private key
                CryptMethods.CertSetCertificateContextProperty(storeCertContext, 2 /* CERT_KEY_PROV_INFO_PROP_ID */, 0, ref kpi);

                // Check if password is valid
                if (!string.IsNullOrEmpty(password))
                {
                    // Convert to native string
                    passwordPtr = Marshal.StringToCoTaskMemUni(password);
                }

                // Allocate cryptography blob container
                CryptoAPIBlob pfxBlob = new CryptoAPIBlob();

                // Estimate the size of the certificate
                _Check(CryptMethods.PFXExportCertStoreEx(certStore, ref pfxBlob, passwordPtr, IntPtr.Zero, 7 /* EXPORT_PRIVATE_KEYS | REPORT_NO_PRIVATE_KEY | REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY */));

                pfxData = new byte[pfxBlob.DataLength];
                dataHandle = GCHandle.Alloc(pfxData, GCHandleType.Pinned);
                pfxBlob.Data = dataHandle.AddrOfPinnedObject();

                // Export certificate
                _Check(CryptMethods.PFXExportCertStoreEx(certStore, ref pfxBlob, passwordPtr, IntPtr.Zero, 7 /* EXPORT_PRIVATE_KEYS | REPORT_NO_PRIVATE_KEY | REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY */));

                // Free data handle
                dataHandle.Free();
            }
            finally
            {
                if (passwordPtr != IntPtr.Zero)
                {
                    // Release password string
                    Marshal.ZeroFreeCoTaskMemUnicode(passwordPtr);
                }

                if (dataHandle.IsAllocated)
                {
                    // Release data handle
                    dataHandle.Free();
                }

                if (certContext != IntPtr.Zero)
                {
                    // Release certificate context
                    CryptMethods.CertFreeCertificateContext(certContext);
                }

                if (storeCertContext != IntPtr.Zero)
                {
                    // Release certificate store handle
                    CryptMethods.CertFreeCertificateContext(storeCertContext);
                }

                if (certStore != IntPtr.Zero)
                {
                    // Close certificate store
                    CryptMethods.CertCloseStore(certStore, 0);
                }

                if (cryptKey != IntPtr.Zero)
                {
                    // Destroy private key
                    AdvancedAPIMethods.CryptDestroyKey(cryptKey);
                }

                if (providerContext != IntPtr.Zero)
                {
                    // Release cryptography context
                    AdvancedAPIMethods.CryptReleaseContext(providerContext, 0);

                    // Release keyset
                    AdvancedAPIMethods.CryptAcquireContextW(out providerContext, containerName, null, 1 /* PROV_RSA_FULL */, 0x10 /* CRYPT_DELETEKEYSET */);
                }
            }

            return pfxData;
        }

        /// <summary>
        /// Convert managed date and time into OS time stamp
        /// </summary>
        private static SystemTime _ToSystemTime(DateTime dateTime)
        {
            // Convert to file time
            long fileTime = dateTime.ToFileTime();

            // Allocate file time
            SystemTime systemTime;

            // Create system time from file time
            _Check(KernelMethods.FileTimeToSystemTime(ref fileTime, out systemTime));

            // Return file
            return systemTime;
        }

        /// <summary>
        /// Check native method call
        /// </summary>
        /// <param name="nativeCallSucceeded">Indicates whether native call succeeded</param>
        private static void _Check(bool nativeCallSucceeded)
        {
            // Check if native call succeeded
            if (nativeCallSucceeded)
            {
                // No error occured
                return;
            }

            // Throw an exception for OS error
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }
    }
}
