using Newtonsoft.Json;

namespace Asos.Customer.Preference.PactTests.config
{
    public class JsonSettings
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };
    }
}
