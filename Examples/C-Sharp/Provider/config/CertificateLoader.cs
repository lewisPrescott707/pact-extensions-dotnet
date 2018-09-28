using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Asos.Customer.Preference.PactTests.config
{
    internal class CertificateLoader
    {
        public static X509Certificate2 FromEmbeddedResource(string certificateName, string pfxPassword)
        {
            var resourceName = $@"Asos.Customer.Preference.PactTests.config.{certificateName}";
            using (var certificateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (certificateStream == null)
                {
                    throw new Exception("Certificate not valid or not found");
                }

                var rawBytes = new byte[certificateStream.Length];
                for (var i = 0; i < certificateStream.Length; i++)
                {
                    rawBytes[i] = (byte)certificateStream.ReadByte();
                }

                return new X509Certificate2(rawBytes, pfxPassword, X509KeyStorageFlags.UserKeySet);
            }
        }
    }
}