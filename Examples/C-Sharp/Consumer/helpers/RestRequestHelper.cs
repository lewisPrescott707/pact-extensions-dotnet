using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using RestSharp;

namespace Asos.Customer.Update.Tool.Api.PactTests.helpers
{
    public class RestRequestHelper : ProviderService
    {
        public RestRequestHelper(string serviceConsumerName, string providerName) : base(serviceConsumerName, providerName)
        {
        }

        public static void Get<TRequest, TResponse>(string url, Method method, TRequest requestJson)
        {
            var restClient = new RestClient($"{BaseUri}:{Port}");
            var request = new RestRequest(url, method)
            {
                RequestFormat = DataFormat.Json,
                Parameters = { new Parameter()
                {
                    ContentType = "application/json; charset=utf-8",
                    Name = "Content-Type",
                    Value = JsonConvert.SerializeObject(requestJson, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }),
                    Type = ParameterType.RequestBody
                }}
            };
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("asos-c-name", "https://www.asos.com");

            var response = restClient.Deserialize<TResponse>(restClient.Execute(request)).Content.FirstOrDefault().ToString();
            Assert.NotNull(response);
        }
        public static void Get<TResponse>(string url, Method method)
        {
            var restClient = new RestClient($"{BaseUri}:{Port}");
            var request = new RestRequest(url, method)
            {
                RequestFormat = DataFormat.Json,
                Parameters = { new Parameter()
                {
                    ContentType = "application/json; charset=utf-8",
                    Name = "Content-Type"
                }}
            };

            var response = restClient.Deserialize<TResponse>(restClient.Execute(request)).Content.FirstOrDefault().ToString();
            Assert.NotNull(response);
        }

    }
}
