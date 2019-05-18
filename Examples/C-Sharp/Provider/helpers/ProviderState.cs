using System.IO;
using System.Linq;
using PactTests.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PactTests.config;
using RestSharp;
using RestSharp.Authenticators;

namespace PactTests.helpers
{
    public class ProviderState
    {
        private readonly EnvironmentsConfig _config;
        private JObject _pactJson;
        internal PactContract PactContract { get; set; }
        private readonly string _localDirectory;

        public ProviderState(string providerName, string consumerName, string localDirectory)
        {
            _config = EnvironmentsConfig.ReadConfigurationFile();
            var client = new RestClient(_config.PactBroker.Host)
            {
                Authenticator =
                    new HttpBasicAuthenticator(_config.PactBroker.Username, _config.PactBroker.Password)
            };
            var request = new RestRequest($"/pacts/provider/{providerName}/consumer/{consumerName}/latest", Method.GET);
            var response = client.Execute(request);
            PactContract = JsonConvert.DeserializeObject<PactContract>(response.Content);
            _pactJson = JObject.Parse(response.Content);
            _localDirectory = localDirectory;
        }

        public void SetDynamicPathAndFields(string providerState)
        {
            switch (providerState)
            {
                case "Database entry exists":
                    break;
                default:
                    _pactJson = JObject.Parse("{\r\n  \"consumer\": {\r\n    \"name\": \"Web\"\r\n  },\r\n  \"provider\": {\r\n    \"name\": \"Api\"\r\n  },\r\n  \"interactions\": [\r\n    {\r\n      \"description\": \"a request to get response\",\r\n      \"providerState\": \"200 Response\",\r\n      \"request\": {\r\n        \"method\": \"GET\",\r\n        \"path\": \"/pact/1\",\r\n        \"headers\": {\r\n          \"Accept\": \"application/json, text/plain, */*\"\r\n        }\r\n      },\r\n      \"response\": {\r\n        \"status\": 200,\r\n        \"headers\": {\r\n          \"Content-Type\": \"application/json\"\r\n        },\r\n        \"body\": {\r\n          \"pact\": \"1\"\r\n        }\r\n      }\r\n    }\r\n  ],\r\n  \"metadata\": {\r\n    \"pactSpecification\": {\r\n      \"version\": \"2.0.0\"\r\n    }\r\n  }\r\n}");
                    SerializeJsonToFile(_localDirectory);
                    break;
            }
        }

        private int GetIndex(string providerState)
        {
            return PactContract.Interactions.IndexOf(PactContract.Interactions.First(x => x.ProviderState == providerState));
        }

        private void SerializeJsonToFile(string localDirectory)
        {
            using (var json = File.CreateText(localDirectory))
            {
                var serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None
                };
                serializer.Serialize(json, _pactJson);
            }
        }

        private void UpdatePath(string providerState, int index)
        {
            switch (providerState)
            {
                case "Database entry exists":
                    break;
                default:
                    break;
            }
        }
    }
}
