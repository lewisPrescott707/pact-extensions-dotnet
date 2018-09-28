using System.IO;
using System.Linq;
using Asos.Core.Testing.Pact.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace Asos.Core.Testing.Pact.PactVerifier
{
    public class ProviderState : IProviderState
    {
        public virtual string PactJson { get; set; }
        public virtual PactContract PactContract { get; set; }

        public ProviderState(string providerName, string consumerName, string localDirectory)
        {
            var client = new RestClient("https://asos.pact.dius.com.au")
            {
                Authenticator =
                    new HttpBasicAuthenticator("{username}", "{password}")
            };
            var request = new RestRequest($"/pacts/provider/{providerName}/consumer/{consumerName}/latest", Method.GET);
            var response = client.Execute(request);
            PactContract = JsonConvert.DeserializeObject<PactContract>(response.Content);
            PactJson = response.Content;
        }

        public int GetIndex(string providerState)
        {
            return PactContract.Interactions.IndexOf(PactContract.Interactions.First(x => x.ProviderState == providerState));
        }

        public void SerializeJsonToFile(string localDirectory)
        {
            using (var json = File.CreateText(localDirectory))
            {
                var serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None
                };
                serializer.Serialize(json, JObject.Parse(PactJson));
            }
        }
    }
}
