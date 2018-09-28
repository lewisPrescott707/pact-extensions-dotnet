using System;
using System.Threading.Tasks;
using Asos.Customer.Profile.Api.Client;
using Asos.Customer.Profile.Api.Client.Communication.Security;
using Asos.Identity.Core.Api.TokenGenerator;
using Asos.Identity.Core.Api.TokenGenerator.Adal;

namespace Asos.Customer.Preference.PactTests.config
{
    public class TokenGenerator
    {
        private static EnvironmentsConfig _config;
        public TokenGenerator()
        {
            _config = EnvironmentsConfig.ReadConfigurationFile();
        }
        public async Task<string> GenerateToken()
        {
            var certificate = CertificateLoader.FromEmbeddedResource("asos-test-customer-erasure.pfx", "RHZSWCNt9BDUejuh1cFb");

            var generateToken = new ClientAssertionCertificateTokenGenerator
            {
                ClientId = "910e4bc3-7408-4a4c-95c1-5c39a95adc89",
                Certificate = certificate,
                Authority = "https://login.microsoftonline.com/4af8322c-80ee-4819-a9ce-863d5afbea1c/oauth2/token",
                Resource = "https://api.asos.com"
            };

            var settings = new ProfileApiSettings
            {
                AkamaiOrigin = "NOT_SET",
                ApplicationNameHeader = "unit-test",
                CustomerApiBaseUri = new Uri(_config.SelectedEnvironment.CustomerApi.Url),
                TokenGenerator = generateToken,
                HttpClientTimeout = TimeSpan.FromSeconds(10)
            };

            var jwtToken = new JwtTokenAcquisition(new TokenCache(settings.TokenGenerator));
            return await jwtToken.GetJwtTokenAsync();
        }

        public static CustomerProfileApi Create()
        {
            var certificate = CertificateLoader.FromEmbeddedResource("asos-test-customer-erasure.pfx", "RHZSWCNt9BDUejuh1cFb");

            var generateToken = new ClientAssertionCertificateTokenGenerator
            {
                ClientId = "910e4bc3-7408-4a4c-95c1-5c39a95adc89",
                Certificate = certificate,
                Authority = "https://login.microsoftonline.com/4af8322c-80ee-4819-a9ce-863d5afbea1c/oauth2/token",
                Resource = "https://api.asos.com"
            };

            var settings = new ProfileApiSettings
            {
                AkamaiOrigin = "NOT_SET",
                ApplicationNameHeader = "unit-test",
                CustomerApiBaseUri = new Uri(_config.SelectedEnvironment.CustomerApi.Url),
                TokenGenerator = generateToken,
                HttpClientTimeout = TimeSpan.FromSeconds(10)
            };

            return new CustomerProfileApi(settings);
        }
    }
}
