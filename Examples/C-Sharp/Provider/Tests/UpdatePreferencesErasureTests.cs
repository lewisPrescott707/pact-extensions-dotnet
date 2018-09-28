using System.Collections.Generic;
using Asos.Customer.Preference.PactTests.config;
using Asos.Customer.Preference.PactTests.helpers;
using NUnit.Framework;
using PactNet;

namespace Asos.Customer.Preference.PactTests.Tests
{
    [TestFixture]
    public class UpdatePreferencesErasureTests
    {
        private readonly IPactVerifier _pactVerifier;
        private readonly string _serviceUri;
        private readonly string _pactDirectory;
        private const string ProviderName = "Asos.Customer.Preference.Api";
        private const string ConsumerName = "Asos.Customer.Update.Tool";
        private readonly EnvironmentsConfig _config;

        public UpdatePreferencesErasureTests()
        {
            _config = EnvironmentsConfig.ReadConfigurationFile();
            _serviceUri = _config.SelectedEnvironment.CustomerApi.Url;
            var token = new TokenGenerator().GenerateToken().Result;
            var pactVerifierConfig = new PactVerifierConfig
            {
                CustomHeader = new KeyValuePair<string, string>("Authorization", $"Bearer {token}")
            };
            _pactVerifier = new PactVerifier(pactVerifierConfig);
            _pactDirectory = @"D:\updated-pact.json";
        }

        [SetUp]
        public void SetUp()
        {
            var providerState = new ProviderState(ProviderName, ConsumerName, _pactDirectory);
            foreach (var interaction in providerState.PactContract.Interactions)
            {
                providerState.SetDynamicPathAndFields(interaction.ProviderState);
            }
        }

        [Test]
        public void EnsureUpdatePreferencesApiHonoursPactWithConsumer()
        {
            _pactVerifier
                .ServiceProvider("Customer Update Tool", _serviceUri)
                .HonoursPactWith(ProviderName)
                .PactUri(_pactDirectory, new PactUriOptions(_config.PactBroker.Username, _config.PactBroker.Password))
                .Verify();
        }
    }
}
