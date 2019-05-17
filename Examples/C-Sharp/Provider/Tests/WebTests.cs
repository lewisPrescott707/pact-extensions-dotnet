using System;
using System.IO;
using NUnit.Framework;
using PactNet;

namespace PactTests.Tests
{
    [TestFixture]
    public class WebTests
    {
        private readonly IPactVerifier _pactVerifier;
        private readonly string _serviceUri;
        private readonly string _pactDirectory;
        private const string ProviderName = "Api";
        private const string ConsumerName = "Web";

        public WebTests()
        {
            _serviceUri = "http://echo.jsontest.com";
            var pactVerifierConfig = new PactVerifierConfig
            {
                ProviderVersion = "2.0.0"
            };
            _pactVerifier = new PactVerifier(pactVerifierConfig);
            _pactDirectory = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName}/pacts/web-api.json";
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
