using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Core.Testing.Pact.Models;
using Core.Testing.Pact.Provider.PactVerifier;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;

namespace Core.Testing.Pact.UnitTests.Provider
{
    [TestFixture]
    public class ProviderStateTests
    {
        private string configPath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Config";

        [Test]
        public void Given_Broker_Returns_Pact_Contract__When_Get_ProviderState_Index__Then_Index_Returned()
        {
            const string providerStateName = "Test State";
            var pactContract = new PactContract {
                Interactions = new List<ProviderServiceInteraction>{
                    new ProviderServiceInteraction{
                        ProviderState = providerStateName
                    }
                }
            };
            using (var client = new HttpClient())
            {
                var providerState = new Mock<ProviderState>(client, configPath);
                providerState.Setup(x => x.PactContract).Returns(pactContract);
                providerState.Setup(x => x.PactJson).Returns("");

                var index = providerState.Object.GetIndex(providerStateName);

                Assert.AreEqual(0, index);
            }
        }

        [Test]
        public void Given_Broker_Returns_Pact_Contract__When_Serialize_To_New_Json_File__Then_No_Errors_Thrown()
        {
            using (var client = new HttpClient())
            {
                var providerState = new Mock<ProviderState>(client, configPath);
                providerState.Setup(x => x.PactJson).Returns("{}");

                Assert.DoesNotThrow(() => providerState.Object.SerializeJsonToFile("./pact-file.json"));
            }
        }
    }
}