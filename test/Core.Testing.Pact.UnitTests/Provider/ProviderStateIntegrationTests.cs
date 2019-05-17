using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Testing.Pact.Provider.PactVerifier;
using NUnit.Framework;

namespace Core.Testing.Pact.UnitTests.Provider
{
    [TestFixture]
    public class ProviderStateIntegrationTests
    {
        [Test]
        public async Task Given_Contract_Not_In_Pact_Broker__When_Get_Contract__Then_Pact_Not_Found()
        {
            const string provider = "Provider";
            const string consumer = "Consumer";

            using (var client = new HttpClient())
            {
                var providerState = new ProviderState(client, $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Config");

                var pact = await providerState.GetContract(provider, consumer, client);

                Assert.AreEqual(HttpStatusCode.NotFound, pact.StatusCode);
            }
        }
    }
}