using System.Collections.Generic;
using System.IO;
using Asos.Core.Testing.Pact.Config;
using Asos.Core.Testing.Pact.CustomExtensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PactNet;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;

namespace Asos.Core.Testing.Pact.Consumer.MockProviderService
{
    public class ProviderService
    {
        private int Port { get; set; }

        public IMockProviderService MockProviderService;

        public IPactBuilder PactBuilder;

        private IConfigurationRoot Config;

        public ProviderService(string serviceConsumerName, string providerName, string configPath)
        {
            Config = new EnvironmentsConfig(configPath).Config;
            PactBuilder = new PactBuilder(new PactConfig {
                    PactDir = Config["pactConfig:pactDir"],
                    LogDir = Config["pactConfig:pactDir"],
                    SpecificationVersion = Config["pactConfig:specificationVersion"]
            })
                .ServiceConsumer(serviceConsumerName)
                .HasPactWith(providerName);
        }

        public void Initialize()
        {
            Port = int.Parse(Config["providerService:port"]);
            MockProviderService = PactBuilder.MockService(Port);
            MockProviderService.ClearInteractions();
        }

        public void Build() => PactBuilder.Build();

        public void SetupRequest<TRequest>(string given, string uponReceiving, HttpVerb method, IMatcher url, Dictionary<string, object> headers, TRequest requestBody)
        {
            MockProviderService
                .Given(given)
                .UponReceiving(uponReceiving)
                .With(new ProviderServiceRequest
                {
                    Method = method,
                    Path = url,
                    Headers = headers,
                    Body = ConstructRequestBody(requestBody)
                });
        }

        public void SetupRequest<TRequest>(string given, string uponReceiving, HttpVerb method, string url, Dictionary<string, object> headers, TRequest requestBody)
        {
            MockProviderService
                .Given(given)
                .UponReceiving(uponReceiving)
                .With(new ProviderServiceRequest
                {
                    Method = method,
                    Path = url,
                    Headers = headers,
                    Body = ConstructRequestBody(requestBody)
                });
        }

        public void SetupGetRequest<TRequest>(string given, string uponReceiving, string url, Dictionary<string, object> headers)
        {
            MockProviderService
                .Given(given)
                .UponReceiving(uponReceiving)
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = url,
                    Headers = headers
                });
        }

        public void SetupResponse<TResponse>(int responseCode, Dictionary<string, object> headers, TResponse responseBody)
        {
            MockProviderService
                .WillRespondWith(new ProviderServiceResponse()
                {
                    Status = responseCode,
                    Body = ConstructResponseBody(responseBody),
                    Headers = headers
                });
        }

        public JObject ConstructRequestBody<TRequest>(TRequest request)
        {
            var jsonRequest = request.ToDynamicRequest();
            return JObject.Parse(JsonConvert.SerializeObject(jsonRequest, new JsonSerializerSettings
            {
                ContractResolver = new JsonResolver.SkipEmptyContractResolver(),
                NullValueHandling = NullValueHandling.Include
            }));
        }

        public JObject ConstructResponseBody<TRequest>(TRequest request)
        {
            var jsonRequest = request.ToDynamicResponse();
            return JObject.Parse(JsonConvert.SerializeObject(jsonRequest, new JsonSerializerSettings
            {
                ContractResolver = new JsonResolver.SkipEmptyContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            }));
        }
    }
}
