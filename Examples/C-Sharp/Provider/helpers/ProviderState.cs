﻿using System.IO;
using System.Linq;
using Asos.Customer.Contracts.Profile.V2.Rest.Customer;
using Asos.Customer.Preference.PactTests.config;
using Asos.Customer.Preference.PactTests.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace Asos.Customer.Preference.PactTests.helpers
{
    public class ProviderState
    {
        private readonly EnvironmentsConfig _config;
        private readonly JObject _pactJson;
        private CustomerResponseV2 _customer;
        internal PactContract PactContract { get; set; }
        private readonly string _localDirectory;

        public ProviderState(string providerName, string consumerName, string localDirectory)
        {
            _config = EnvironmentsConfig.ReadConfigurationFile();
            var client = new RestClient(_config.PactBroker.Host)
            {
                Authenticator =
                    new HttpBasicAuthenticator(_config.PactBroker.Username, _config.PactBroker.Password)
            };
            var request = new RestRequest($"/pacts/provider/{providerName}/consumer/{consumerName}/latest", Method.GET);
            var response = client.Execute(request);
            PactContract = JsonConvert.DeserializeObject<PactContract>(response.Content);
            _pactJson = JObject.Parse(response.Content);
            _localDirectory = localDirectory;
        }

        public async void SetDynamicPathAndFields(string providerState)
        {
            switch (providerState)
            {
                case "A Customer With Preferences Exists":
                    var customerGuid = PactContract.Interactions.First(x => x.ProviderState == providerState).Request.Path.ToString().Split('/')[5];
                    var index = GetIndex(providerState);
                    var customer = new Customer();
                    var findCustomer = customer.Find(customerGuid).Result;
                    if (findCustomer.Data.TotalFound == 0)
                    {
                        _customer = await customer.Create();
                        var update = await customer.Update(_customer, customerGuid);
                        _customer = update.Data;
                        UpdatePath(providerState, index);
                        SerializeJsonToFile(_localDirectory);
                    }
                    break;
                default:
                    break;
            }
        }

        private int GetIndex(string providerState)
        {
            return PactContract.Interactions.IndexOf(PactContract.Interactions.First(x => x.ProviderState == providerState));
        }

        private void SerializeJsonToFile(string localDirectory)
        {
            using (var json = File.CreateText(localDirectory))
            {
                var serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None
                };
                serializer.Serialize(json, _pactJson);
            }
        }

        private void UpdatePath(string providerState, int index)
        {
            switch (providerState)
            {
                case "A Customer With Preferences Exists":
                    _pactJson["interactions"][index]["request"]["path"] = $"{_config.Endpoints.Preferences.Get}/{_customer.CustomerGuid}";
                    break;
                default:
                    break;
            }
        }
    }
}