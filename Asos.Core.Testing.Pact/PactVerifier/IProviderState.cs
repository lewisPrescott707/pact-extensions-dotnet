using System.IO;
using System.Linq;
using Asos.Core.Testing.Pact.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace Asos.Core.Testing.Pact.PactVerifier
{
    public interface IProviderState
    {
        int GetIndex(string providerState);

        void SerializeJsonToFile(string localDirectory);
    }
}
