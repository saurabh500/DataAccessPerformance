using System;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.TDS.EndPoint.Cryptography
{
    /// <summary>
    /// Wrappers for native method calls
    /// </summary>
    internal class CryptMethods
    {
        /// <summary>
        /// The CertStrToName function converts a null-terminated X.500 string to an encoded certificate name.
        /// </summary>
        [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CertStrToNameW(
            int certificateEncodingType,
            IntPtr x500,
            int strType,
            IntPtr reserved,
            [MarshalAs(UnmanagedType.LPArray)] [Out] byte[] encoded,
            ref int encodedLength,
            out IntPtr errorString);

        /// <summary>
        /// The CertCreateSelfSignCertificate function builds a self-signed certificate and returns a pointer to a CERT_CONTEXT structure that represents the certificate.
        /// </summary>
        [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr CertCreateSelfSignCertificate(
            IntPtr providerHandle,
            [In] ref CryptoAPIBlob subjectIssuerBlob,
            int flags,
            [In] ref CryptKeyProviderInfo keyProviderInformation,
            IntPtr signatureAlgorithm,
            [In] ref SystemTime startTime,
            [In] ref SystemTime endTime,
            IntPtr extensions);

        /// <summary>
        /// The CertFreeCertificateContext function frees a certificate context by decrementing its reference count. When the reference count goes to zero, CertFreeCertificateContext frees the memory used by a certificate context.
        /// </summary>
        [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CertFreeCertificateContext(
            IntPtr certificateContext);

        /// <summary>
        /// The CertOpenStore function opens a certificate store by using a specified store provider type.
        /// </summary>
        [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr CertOpenStore(
            [MarshalAs(UnmanagedType.LPStr)] string storeProvider,
            int messageAndCertificateEncodingType,
            IntPtr cryptProvHandle,
            int flags,
            IntPtr parameters);

        /// <summary>
        /// The CertCloseStore function closes a certificate store handle and reduces the reference count on the store. 
        /// </summary>
        [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CertCloseStore(
            IntPtr certificateStoreHandle,
            int flags);

        /// <summary>
        /// The CertAddCertificateContextToStore function adds a certificate context to the certificate store.
        /// </summary>
        [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CertAddCertificateContextToStore(
            IntPtr certificateStoreHandle,
            IntPtr certificateContext,
            int addDisposition,
            out IntPtr storeContextPtr);

        /// <summary>
        /// The CertSetCertificateContextProperty function sets an extended property for a specified certificate context.
        /// </summary>
        [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CertSetCertificateContextProperty(
            IntPtr certificateContext,
            int propertyId,
            int flags,
            [In] ref CryptKeyProviderInfo data);

        /// <summary>
        /// The PFXExportCertStoreEx function exports the certificates and, if available, their associated private keys from the referenced certificate store. 
        /// </summary>
        [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PFXExportCertStoreEx(
            IntPtr certificateStoreHandle,
            ref CryptoAPIBlob pfxBlob,
            IntPtr password,
            IntPtr reserved,
            int flags);
    }
}
