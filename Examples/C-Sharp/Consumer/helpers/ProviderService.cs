using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.Models;

namespace Asos.Customer.Update.Tool.Api.PactTests.helpers
{
    public class ProviderService
    {
        protected static string BaseUri { get; } = "http://localhost";
        protected static int Port { get; } = 1234;
        internal IMockProviderService MockProviderService;
        internal IPactBuilder PactBuilder;

        public ProviderService(string serviceConsumerName, string providerName)
        {
            const string pactDir = @"D:\Pact";
            const string specVersion = "2.0.0";

            PactBuilder = new PactBuilder(new PactConfig()
            {
                PactDir = pactDir,
                LogDir = pactDir,
                SpecificationVersion = specVersion
            })
            .ServiceConsumer(serviceConsumerName)
            .HasPactWith(providerName);
            MockProviderService = PactBuilder.MockService(Port);
            MockProviderService.ClearInteractions();
        }

        public void SetupRequest(string given, string uponReceiving, HttpVerb method, IMatcher url, Dictionary<string, object> headers, JObject requestBody)
        {
            MockProviderService
                .Given(given)
                .UponReceiving(uponReceiving)
                .With(new ProviderServiceRequest
                {
                    Method = method,
                    Path = url,
                    Headers = headers,
                    Body = requestBody
                });
        }

        public void SetupRequest(string given, string uponReceiving, HttpVerb method, string url, Dictionary<string, object> headers, JObject requestBody)
        {
            MockProviderService
                .Given(given)
                .UponReceiving(uponReceiving)
                .With(new ProviderServiceRequest
                {
                    Method = method,
                    Path = url,
                    Headers = headers,
                    Body = requestBody
                });
        }

        public void SetupResponse(int responseCode, Dictionary<string, object> headers, JObject responseBody)
        {
            MockProviderService
                .WillRespondWith(new ProviderServiceResponse()
                {
                    Status = responseCode,
                    Body = responseBody,
                    Headers = headers
                });
        }

        public JObject ConstructRequestBody<TRequest>(TRequest request)
        {
            var jsonRequest = request.ToDynamicRequest();
            return JObject.Parse(JsonConvert.SerializeObject(jsonRequest, new JsonSerializerSettings
            {
                ContractResolver = new SkipEmptyContractResolver(),
                NullValueHandling = NullValueHandling.Include
            }));
        }

        public JObject ConstructResponseBody<TRequest>(TRequest request)
        {
            var jsonRequest = request.ToDynamicResponse();
            return JObject.Parse(JsonConvert.SerializeObject(jsonRequest, new JsonSerializerSettings
            {
                ContractResolver = new SkipEmptyContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            }));
        }

        public JObject ConstructResponseBodyExact<TRequest>(TRequest request)
        {
            var jsonRequest = request.ToDynamicRequest();
            return JObject.Parse(JsonConvert.SerializeObject(jsonRequest, new JsonSerializerSettings
            {
                ContractResolver = new SkipEmptyContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            }));
        }
    }

    public class SkipEmptyContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = obj =>
            {
                if (property.PropertyType.Name.Contains("ICollection"))
                {
                    return (property.ValueProvider.GetValue(obj) as dynamic).Count > 0;
                }
                return true;
            };
            return property;
        }
    }
}
