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
        private readonly JObject _pactJson;
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
                    //_pactJson["interactions"][index]["request"]["path"] = $"{_config.Endpoints.Preferences.Get}/";
                    break;
                default:
                    break;
            }
        }
    }
}
