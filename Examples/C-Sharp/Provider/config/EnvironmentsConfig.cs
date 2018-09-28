using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Asos.Customer.Preference.PactTests.config
{
    public partial class EnvironmentsConfig
    {
        [JsonProperty("environments")]
        public List<IdentityEnvironment> Environments { get; set; }
        [JsonProperty("pactBroker")]
        public PactBroker PactBroker { get; set; }
        [JsonProperty("endpoints")]
        public Endpoints Endpoints { get; set; }
    }

    public class Endpoints
    {
        [JsonProperty("preferences")]
        public PreferencesEndpoints Preferences { get; set; }
    }

    public class PreferencesEndpoints
    {
        [JsonProperty("get")]
        public string Get { get; set; }
    }

    public class IdentityEnvironment
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("customerApi")]
        public CustomerApi CustomerApi { get; set; }
    }

    public class CustomerApi
    {
        [JsonProperty("url")]
        public string Url { get; set; }
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
        public static IConfiguration Configuration { get; set; }

        private static string _selectedEnvrionmentName;

        private static string _selectedPactsDirectory;

        private static readonly string BinDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "");

        private static string _jsonConfigPath;

        public static EnvironmentsConfig ReadConfigurationFile()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(BinDirectory).Parent.Parent.FullName)
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            _selectedEnvrionmentName = Configuration.GetValue<string>("AppSettings:SelectedIdentityEnvironment");
            _selectedPactsDirectory = Configuration.GetValue<string>("AppSettings:SelectedPactDirectory");
            _jsonConfigPath =
                $"{Directory.GetParent(BinDirectory).Parent.Parent.FullName}\\{Configuration.GetValue<string>("AppSettings:EnvironmentsConfigPath")}";

            string json;
            using (var reader = new StreamReader(_jsonConfigPath))
            {
                json = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<EnvironmentsConfig>(json, JsonSettings.Settings);
        }

        public static string GetPactsDirectory(string providerName = "", string consumerName = "")
        {
            var pactBrokerHost = ReadConfigurationFile().PactBroker.Host;
            var pactsDirectory = _selectedPactsDirectory == "local" ? $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName}/{SetPactUrl(providerName, consumerName)}/{consumerName.ToLower()}-{providerName.ToLower()}.json" : $"{pactBrokerHost}/{SetPactUrl(providerName, consumerName)}";
            return pactsDirectory;
        }

        public static string GetPactsPath(string providerName = "", string consumerName = "")
        {
            return $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName}\\{SetPactUrl(providerName, consumerName)}";
        }

        private static string SetPactUrl(string providerName = "", string consumerName = "")
        {
            return $"pacts/provider/{providerName}/consumer/{consumerName}/latest";
        }

        public virtual IdentityEnvironment SelectedEnvironment
        {
            get { return Environments.FirstOrDefault(x => x.Name.Equals(_selectedEnvrionmentName, StringComparison.InvariantCultureIgnoreCase)); }
        }
    }
}