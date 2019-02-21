using System.Collections.Generic;
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
        protected static int Port { get; set; }

        public IMockProviderService MockProviderService;

        private readonly IPactBuilder _pactBuilder;

        private static readonly IConfigurationRoot Config = new EnvironmentsConfig().Config;

        public ProviderService(string serviceConsumerName, string providerName)
        {
            Port = int.Parse(Config["providerService:port"]);
            _pactBuilder = new PactBuilder(new PactConfig {
                    PactDir = Config["pactConfig:pactDir"],
                    LogDir = Config["pactConfig:pactDir"],
                    SpecificationVersion = Config["pactConfig:specificationVersion"]
            })
                .ServiceConsumer(serviceConsumerName)
                .HasPactWith(providerName);
        }

        public void Initialize()
        {
            MockProviderService = _pactBuilder.MockService(Port);
            MockProviderService.ClearInteractions();
        }

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
