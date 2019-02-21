using System;
using Asos.Core.Testing.Pact.Consumer.MockProviderService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Asos.Core.Testing.Pact.UnitTests.Consumer
{
    [TestFixture]
    public class ProviderServiceTests
    {
        [Test]
        public void When_Construct_Response_Body__Then_Matchers_Applied_In_Response_Body()
        {
            var providerService = new ProviderService("consumer", "provider");
            var dateTime = DateTime.Today;

            var responseBody = providerService.ConstructResponseBody(new TestContract()
            {
                Age = 10,
                Date = dateTime,
                Id = "uniqueId123"
            });
            var jsonExpected = JObject.Parse(
                $" {{ \"id\": {{ \"json_class\": \"Pact::SomethingLike\", \"contents\": \"uniqueId123\" }}," +
                $"  \"date\": {{ \"json_class\": \"Pact::SomethingLike\", \"contents\": \"{dateTime:O}\" }}," +
                $"  \"age\": {{ \"json_class\": \"Pact::SomethingLike\", \"contents\": 10 }} }} ");

            Assert.AreEqual(jsonExpected, responseBody);
        }

        [Test]
        public void When_Construct_Request_Body__Then_Matchers_Applied_In_Request_Body()
        {
            var providerService = new ProviderService("consumer", "provider");
            var dateTime = DateTime.Today;

            var requestBody = providerService.ConstructRequestBody(new TestContract()
            {
                Age = 10,
                Date = dateTime,
                Id = "uniqueId123"
            });
            var jsonExpected = JObject.Parse(
                $" {{ \"id\": \"uniqueId123\",  \"date\": \"{dateTime:O}\",  \"age\": 10 }} ");

            Assert.AreEqual(jsonExpected, requestBody);
        }

        [Test]
        public void Given_Setup_MockProviderService__Then_Access_MockProviderService_Not_Null()
        {
            var providerService = new ProviderService("consumer", "provider");
            providerService.Initialize();

            Assert.IsNotNull(providerService.MockProviderService);
        }

        public class TestContract
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("date")]
            public DateTime Date { get; set; }

            [JsonProperty("age")]
            public int Age { get; set; }
        }
    }
}
