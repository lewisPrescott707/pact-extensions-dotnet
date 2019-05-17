using System.Collections.Generic;
using Newtonsoft.Json;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.Models;

namespace PactTests.models
{
    public class PactContract
    {
        [JsonProperty(PropertyName = "consumer", Order = 1)]
        internal Pacticipant Consumer;

        [JsonProperty(PropertyName = "provider", Order = 2)]
        internal Pacticipant Provider;

        [JsonProperty(PropertyName = "interactions", Order = 3)]
        internal List<ProviderServiceInteraction> Interactions;

        [JsonProperty(PropertyName = "metadata", Order = 4)]
        internal Metadata Metadata;
    }

    internal class Metadata
    {
        [JsonProperty(PropertyName = "pactSpecification")]
        internal PactSpecification PactSpecification;
    }

    internal class PactSpecification
    {
        [JsonProperty(PropertyName = "version")]
        internal string Version;
    }
}
