using Asos.Customer.Preference.PactTests.config;
using NUnit.Framework;
using PactNet;

namespace Asos.Customer.Preference.PactTests.Tests
{
    [TestFixture]
    public class SystemPreferencesWebTests
    {
        private readonly IPactVerifier _pactVerifier;
        private readonly string _serviceUri;
        private readonly string _pactDirectory;
        private const string ProviderName = "Asos.Customer.Preference.Api";
        private const string ConsumerName = "Asos.Identity.Web";
        private readonly EnvironmentsConfig _configSettings;

        public SystemPreferencesWebTests()
        {
            _configSettings = EnvironmentsConfig.ReadConfigurationFile();
            _serviceUri = _configSettings.SelectedEnvironment.CustomerApi.Url;
            var pactVerifierConfig = new PactVerifierConfig { };
            _pactVerifier = new PactVerifier(pactVerifierConfig);
            _pactDirectory = EnvironmentsConfig.GetPactsDirectory(ProviderName, ConsumerName);
        }
  
        [Test]
        public void EnsureSystemPreferencesApiHonoursPactWithConsumer()
        {
            _pactVerifier
                .ServiceProvider("Identity", _serviceUri)
                .HonoursPactWith(ProviderName)
                .PactUri($"{_pactDirectory}", new PactUriOptions(_configSettings.PactBroker.Username, _configSettings.PactBroker.Password))
                .Verify();
        }
    }
}
