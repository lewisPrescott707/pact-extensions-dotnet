using System.IO;
using Microsoft.Extensions.Configuration;

namespace Asos.Core.Testing.Pact.Config
{
    public class EnvironmentsConfig
    {
        public IConfigurationRoot Config { get; }

        public EnvironmentsConfig()
        {
            Config = new ConfigurationBuilder()
                .SetBasePath($"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName}//src//Asos.Core.Testing.Pact//Config")
                .AddJsonFile("pact.json").Build();

        }
    }
}
