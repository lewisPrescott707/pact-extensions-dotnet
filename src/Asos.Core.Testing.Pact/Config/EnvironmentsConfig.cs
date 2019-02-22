using System.IO;
using Microsoft.Extensions.Configuration;

namespace Asos.Core.Testing.Pact.Config
{
    public class EnvironmentsConfig
    {
        public IConfigurationRoot Config { get; set; }

        public EnvironmentsConfig(string configPath)
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(configPath)
                .AddJsonFile("pact.json").Build();

        }
    }
}
