using System;
using System.IO;
using NUnit.Framework;
using PactNet;
using PactTests.helpers;

namespace PactTests.Tests
{
    [TestFixture]
    public class WebTests
    {
        private readonly IPactVerifier _pactVerifier;
        private readonly string _serviceUri;
        private readonly string _pactDirectory;
        private const string ProviderName = "Publishing%20API";
        private const string ConsumerName = "GDS%20API%20Adapters";

        public WebTests()
        {
            _serviceUri = "http://echo.jsontest.com";
            var pactVerifierConfig = new PactVerifierConfig
            {
                ProviderVersion = "2.0.0"
            };
            _pactVerifier = new PactVerifier(pactVerifierConfig);
            _pactDirectory = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName}/pacts/updated-pact.json";
        }

        [SetUp]
        public void Setup()
        {
            var providerState = new ProviderState(ProviderName, ConsumerName, _pactDirectory);
            foreach (var interaction in providerState.PactContract.Interactions)
            {
                providerState.SetDynamicPathAndFields(interaction.ProviderState);
            }
        }
  
        [Test]
        public void EnsureEchoHonoursPactWithConsumer()
        {
            _pactVerifier
                .ServiceProvider(ProviderName, _serviceUri)
                .HonoursPactWith(ConsumerName)
                .PactUri(_pactDirectory)
                .Verify();
        }
    }
}
