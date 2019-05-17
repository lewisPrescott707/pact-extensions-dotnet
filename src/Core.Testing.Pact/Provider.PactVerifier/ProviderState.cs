using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Core.Testing.Pact.Config;
using Core.Testing.Pact.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Testing.Pact.Provider.PactVerifier
{
    public class ProviderState : IProviderState
    {
        public virtual string PactJson { get; set; }
        public virtual PactContract PactContract { get; set; }

        public ProviderState(HttpClient client, string configPath)
        {
            var pactBrokerConfig = new EnvironmentsConfig(configPath).Config;
            client.BaseAddress = new Uri(pactBrokerConfig["pactBroker:url"]);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            $"{pactBrokerConfig["pactBroker:username"]}:{pactBrokerConfig["pactBroker:password"]}")));
        }

        public async Task<HttpResponseMessage> GetContract(string providerName, string consumerName, HttpClient client)
        {
            var response = await client.GetAsync($"/pacts/provider/{providerName}/consumer/{consumerName}/latest");
            if (!response.IsSuccessStatusCode) return response;
            var content = await response.Content.ReadAsStringAsync();
            PactContract = JsonConvert.DeserializeObject<PactContract>(content);
            PactJson = content;
            return response;
        }

        public int GetIndex(string providerState)
        {
            return PactContract.Interactions.IndexOf(PactContract.Interactions.First(x => x.ProviderState == providerState));
        }

        public void SerializeJsonToFile(string localDirectory)
        {
            using (var streamWriter = File.CreateText(localDirectory))
            {
                SerializeJson(streamWriter);
            }
        }

        public void SerializeJson(StreamWriter streamWriter)
        {
            var serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };
            serializer.Serialize(streamWriter, JObject.Parse(PactJson));
        }
    }
}
