using System.Collections.Generic;
using Newtonsoft.Json;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.Models;

namespace Asos.Core.Testing.Pact.Models
{
    public class PactContract
    {
        [JsonProperty(PropertyName = "consumer", Order = 1)]
        public Pacticipant Consumer;

        [JsonProperty(PropertyName = "provider", Order = 2)]
        public Pacticipant Provider;

        [JsonProperty(PropertyName = "interactions", Order = 3)]
        public List<ProviderServiceInteraction> Interactions;

        [JsonProperty(PropertyName = "metadata", Order = 4)]
        public Metadata Metadata;
    }

    public class Metadata
    {
        [JsonProperty(PropertyName = "pactSpecification")]
        public PactSpecification PactSpecification;
    }

    public class PactSpecification
    {
        [JsonProperty(PropertyName = "version")]
        public string Version;
    }
}
