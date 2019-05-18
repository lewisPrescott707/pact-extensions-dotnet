using System.IO;
using Newtonsoft.Json;

namespace PactTests.config
{
    public partial class EnvironmentsConfig
    {
        [JsonProperty("pactBroker")]
        public PactBroker PactBroker { get; set; }
    }

    public class PactBroker
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public partial class EnvironmentsConfig
    {
        private static readonly string BinDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "");

        private static string _jsonConfigPath;

        public static EnvironmentsConfig ReadConfigurationFile()
        {
            _jsonConfigPath =
                $"{Directory.GetParent(BinDirectory).Parent.Parent.FullName}\\config\\pact.json";

            string json;
            using (var reader = new StreamReader(_jsonConfigPath))
            {
                json = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<EnvironmentsConfig>(json, JsonSettings.Settings);
        }
    }
}